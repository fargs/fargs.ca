using ImeHub.Portal.Data;
using ImeHub.Portal.Data.Invoices;
using ImeHub.Portal.Library;
using ImeHub.Portal.Library.Security;
using ImeHub.Portal.Services.DateTimeService;
using ImeHub.Portal.Services.FileSystem;
using ImeHub.Portal.Services.HtmlToPdf;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TailBlazor.Toast.Services;

namespace ImeHub.Portal.Pages.Invoices
{
    public partial class Index
    {
        [Inject] private ApplicationDbContext _dbContext { get; set; }
        [Inject] private IConfiguration _configuration { get; set; }
        [Inject] private FileSystemFactory _fileSystemFactory { get; set; }
        [Inject] private ILogger<Index> _logger { get; set; }
        [Inject] private NavigationManager _navigationManager { get; set; }
        [Inject] private IDateTime _dateTime { get; set; }
        [Inject] private IRazorToStringViewRenderer _razor { get; set; }
        [Inject] private IHtmlToPdf _htmlToPdf { get; set; }
        [Inject] private IHttpContextAccessor _httpContextAccessor { get; set; }
        [Inject] private IToastService _toastService { get; set; }

        [CascadingParameter]
        public AuthenticationState AuthState { get; set; }

        ClaimsPrincipal _user { get; set; }
        bool IsLoading { get; set; }
        Guid[] CompanyIdsAccessList { get; set; }
        IEnumerable<ListItemGrouping> PendingDownloadListItems { get; set; }
        IEnumerable<ListItemGrouping> RecentlyDownloadedListItems { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _user = _httpContextAccessor.HttpContext.User;

            _logger.LogInformation("Invoices Initialized handler started");

            if (IsLoading)
            {
                _logger.LogInformation("Concurrent call to onInitialized cancelled.");
                return;
            }

            IsLoading = true;

            try
            {
                await LoadFromDatabase();

                _logger.LogInformation($"Invoices retreived: {PendingDownloadListItems.Count()}");
            }
            finally
            {
                IsLoading = false;
            }

            await base.OnInitializedAsync();
        }

        private async Task LoadFromDatabase()
        {
            var userId = _user.UserId();

            CompanyIdsAccessList = await _dbContext.CompanyAccesses
                .Where(c => c.UserId == userId)
                .Select(c => c.CompanyRole.Company.ObjectGuid)
                .ToArrayAsync();

            var links = await _dbContext.InvoiceDownloadLinks
                // Only include invoices where the user belongs to the company on the invoice
                .Where(idl => CompanyIdsAccessList.Contains(idl.Invoice.CustomerGuid))
                // Link is not expired
                .Where(link => link.ExpiryDate.HasValue && _dateTime.Now < link.ExpiryDate)
                .Select(link => new ListItem
                {
                    ObjectGuid = link.ObjectGuid,
                    AllowedAttempts = link.AllowedAttempts,
                    ActualDownloads = link.InvoiceDownloads.Count(),
                    ExpiryDate = link.ExpiryDate.Value,
                    GroupingKey = new ListItemGrouping_Key
                    {
                        InvoiceGuid = link.Invoice.ObjectGuid,
                        ServiceProviderName = link.Invoice.ServiceProviderName,
                        InvoiceNumber = link.Invoice.InvoiceNumber,
                        InvoiceDate = link.Invoice.InvoiceDate,
                        LastDownloadDate = link.Invoice.DownloadDate
                    }

                })
                .OrderByDescending(link => link.ExpiryDate)
                .ToListAsync();

            PendingDownloadListItems = links
                // Not download yet
                .Where(link => link.ActualDownloads == 0)
                // Group by invoice
                .GroupBy(grp => grp.GroupingKey, new ListItemGrouping_KeyComparer())
                .Select(grouping => new ListItemGrouping
                {
                    Key = grouping.Key,
                    EarliestExpiryDate = grouping.Max(link => link.ExpiryDate),
                    ListItems = grouping
                })
                .OrderBy(i => i.EarliestExpiryDate);

            RecentlyDownloadedListItems = links
                // Not download yet
                .Where(link => link.ActualDownloads > 0)
                // Group by invoice
                .GroupBy(grp => grp.GroupingKey, new ListItemGrouping_KeyComparer())
                .Select(grouping => new ListItemGrouping
                {
                    Key = grouping.Key,
                    EarliestExpiryDate = grouping.Max(link => link.ExpiryDate),
                    ListItems = grouping
                })
                .OrderByDescending(grp => grp.Key.LastDownloadDate);
        }

        public async Task DownloadInvoice(Guid objectGuid)
        {

            var link = await _dbContext.InvoiceDownloadLinks
                // Only include invoices where the user belongs to the company on the invoice
                .Where(idl => CompanyIdsAccessList.Contains(idl.Invoice.CustomerGuid))
                .SingleOrDefaultAsync(c => c.ObjectGuid == objectGuid);

            // Check if the link exists and they have access (we don't tell them it exists)
            if (link == null)
            {
                _toastService.ShowError("Not found");
            }

            // Check if the link has expired
            if (_dateTime.Now > link.ExpiryDate)
            {
                _toastService.ShowError("Download link has expired");
            }

            var invoice = _dbContext.Invoices
                .Include(i => i.InvoiceDetails)
                .SingleOrDefault(i => i.Id == link.InvoiceId);

            if (invoice == null)
            {
                _toastService.ShowError("Not found");
            }

            // If the PDF has already been generated, then return the file
            if (invoice.FileSystemProvider == null)
            {
                var viewModel = new Shared.InvoiceTemplates.StandardModel
                {
                    Invoice = invoice
                };
                var content = await _razor.RenderAsync("InvoiceTemplates/_Standard", viewModel);
                var file = await _htmlToPdf.GenerateAsync(content);

                invoice.FileSystemProvider = Enum.GetName(FileSystemProvider.AzureBlobStorage);
                invoice.FileId = $"{invoice.ServiceProviderGuid}/invoices/{invoice.ObjectGuid}.pdf";

                var fileSystem = _fileSystemFactory.Create(FileSystemProvider.AzureBlobStorage, _configuration);
                await fileSystem.UploadFileAsync(file, invoice.FileId);
            }

            invoice.DownloadDate = _dateTime.Now;

            if (link != null)
            {
                var download = new InvoiceDownload()
                {
                    DownloadDate = _dateTime.Now,
                    DownloadedBy = _user.UserId().ToString(),
                    EmailSentTo = null
                };
                link.InvoiceDownloads.Add(download);
            }

            await _dbContext.SaveChangesAsync();

            await LoadFromDatabase();

            _navigationManager.NavigateTo($"invoices/download/{objectGuid}", true);

        }

        class ListItemGrouping_Key
        {
            public Guid InvoiceGuid { get; set; }
            public string ServiceProviderName { get; set; }
            public string InvoiceNumber { get; set; }
            public DateTime InvoiceDate { get; set; }
            public DateTime? LastDownloadDate { get; set; }
        }
        class ListItemGrouping_KeyComparer : IEqualityComparer<ListItemGrouping_Key>
        {
            public bool Equals(ListItemGrouping_Key left, ListItemGrouping_Key right)
            {
                if ((object)left == null && (object)right == null)
                {
                    return true;
                }
                if ((object)left == null || (object)right == null)
                {
                    return false;
                }
                return left.InvoiceGuid == right.InvoiceGuid;
            }

            public int GetHashCode(ListItemGrouping_Key obj)
            {
                return (obj.InvoiceGuid).GetHashCode();
            }
        }

        class ListItem
        {
            public Guid ObjectGuid { get; set; }
            public int AllowedAttempts { get; set; }
            public int ActualDownloads { get; set; }
            public DateTime ExpiryDate { get; set; }
            public ListItemGrouping_Key GroupingKey { get; set; }
        }
        class ListItemGrouping
        {
            public ListItemGrouping_Key Key { get; set; }
            public DateTime EarliestExpiryDate { get; set; }
            public IEnumerable<ListItem> ListItems { get; set; }
        }
    }
}
