using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class InvoiceSentLogDto
    {
        public int Id { get; set; } // Id (Primary key)
        public string EmailTo { get; set; } // EmailTo (length: 128)
        public System.DateTime SentDate { get; set; } // SentDate
        public string ModifiedUser { get; set; }

        public static Expression<Func<InvoiceSentLog, InvoiceSentLogDto>> FromInvoiceSentLogEntity = i => new InvoiceSentLogDto
        {
            Id = i.Id,
            SentDate = i.SentDate,
            EmailTo = i.EmailTo,
            ModifiedUser = i.ModifiedUser
        };
    }
}