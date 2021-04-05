using ImeHub.Portal.Data;
using ImeHub.Portal.Services.DateTimeService;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Pages.Invoices
{
    public partial class Index
    {
        [Inject] private ApplicationDbContext _dbContext { get; init; }
        [Inject] private ILogger<Index> _logger { get; init; }

        [CascadingParameter]
        public AuthenticationState AuthState { get; set; }

        bool IsLoading { get; set; }
        IEnumerable<ListItemGrouping> PendingDownloadListItems { get; set; }
        IEnumerable<ListItemGrouping> RecentlyDownloadedListItems { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _logger.LogInformation("Invoices Initialized handler started");

            if (IsLoading)
            {
                _logger.LogInformation("Concurrent call to onInitialized cancelled.");
                return;
            }

            IsLoading = true;

            try
            {
                var links = await _dbContext.InvoiceDownloadLinks
                    // Link is not expired
                    .Where(link => link.ExpiryDate.HasValue && _dateTime.UtcNow() > link.ExpiryDate)
                    // Has attempts left
                    .Where(link => link.AllowedAttempts > link.InvoiceDownloads.Count())
                    .Select(link => new ListItem
                    {
                        ObjectGuid = link.ObjectGuid,
                        AllowedAttempts = link.AllowedAttempts,
                        ActualAttempts = link.InvoiceDownloads.Count(),
                        ExpiryDate = link.ExpiryDate.Value,
                        GroupingKey = new ListItemGrouping_Key
                        {
                            InvoiceGuid = link.Invoice.ObjectGuid,
                            ServiceProviderName = link.Invoice.ServiceProviderName,
                            InvoiceNumber = link.Invoice.InvoiceNumber,
                            InvoiceDate = link.Invoice.InvoiceDate
                        }

                    })
                    .ToListAsync();

                PendingDownloadListItems = links
                    // Not download yet
                    .Where(link => link.ActualAttempts == 0)
                    // Group by invoice
                    .GroupBy(grp => grp.GroupingKey, new ListItemGrouping_KeyComparer())
                    .Select(grouping => new ListItemGrouping
                    {
                        Key = grouping.Key,
                        ListItems = grouping
                    });

                RecentlyDownloadedListItems = links
                    // Not download yet
                    .Where(link => link.ActualAttempts > 0)
                    // Group by invoice
                    .GroupBy(grp => grp.GroupingKey)
                    .Select(grouping => new ListItemGrouping
                    {
                        Key = grouping.Key,
                        ListItems = grouping
                    });

                _logger.LogInformation($"Invoices retreived: {PendingDownloadListItems.Count()}");
            }
            finally
            {
                IsLoading = false;
            }

            await base.OnInitializedAsync();
        }

        class ListItemGrouping_Key
        {
            public Guid InvoiceGuid { get; set; }
            public string ServiceProviderName { get; set; }
            public string InvoiceNumber { get; set; }
            public DateTime InvoiceDate { get; set; }
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
            public int ActualAttempts { get; set; }
            public DateTime ExpiryDate { get; set; }
            public ListItemGrouping_Key GroupingKey { get; set; }
        }
        class ListItemGrouping
        {
            public ListItemGrouping_Key Key { get; set; }
            public IEnumerable<ListItem> ListItems { get; set; }
        }
    }
}
