using Orvosi.Data;
using Orvosi.Data.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using WebApp.Library.Extensions;
using WebApp.ViewModels;

namespace WebApp.Library
{
    public class AccountingService
    {
        IOrvosiDbContext db;
        IIdentity identity;
        DateTime now;
        Guid userId;
        Guid physicianId;
        UserContextViewModel userContext;
        string apiKey;

        public AccountingService(IOrvosiDbContext db, IIdentity identity)
        {
            this.db = db;
            this.identity = identity;
            userId = identity.GetGuidUserId();
            userContext = identity.GetUserContext();
            physicianId = userContext.Id;
            this.now = SystemTime.Now();
            apiKey = ConfigurationManager.AppSettings["Html2PdfRocketApiKey"];
        }

        public async Task<Invoice> GenerateInvoicePdf(Invoice invoice, string content)
        {
            
            using (var client = new WebClient())
            {
                NameValueCollection options = new NameValueCollection();
                options.Add("apikey", apiKey);
                options.Add("value", content);
                options.Add("MarginTop", "10");
                options.Add("MarginBottom", "10");
                options.Add("MarginLeft", "10");
                options.Add("MarginRight", "10");

                // Call the API convert to a PDF
                var ms = new MemoryStream(client.UploadValues("http://api.html2pdfrocket.com/pdf", options));

                // Get the physician invoice folder id
                var physician = await db.Physicians.FindAsync(invoice.ServiceProviderGuid);

                // Save the file to Box
                var box = new BoxManager();
                var boxFile = await box.UploadInvoiceAsync(physician.BoxInvoicesFolderId, invoice.GetFileName(), invoice.BoxFileId, invoice.ObjectGuid, ms);

                // update the invoice with the box file id if it has changed
                if (invoice.BoxFileId != boxFile.Id)
                {
                    invoice.BoxFileId = boxFile.Id;
                    await db.SaveChangesAsync();
                }
                return invoice;
            }
        }
    }
}