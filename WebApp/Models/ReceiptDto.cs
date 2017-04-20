using Orvosi.Data;
using System;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class ReceiptDto
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime ReceivedDate { get; set; }

        public static Expression<Func<Receipt, ReceiptDto>> FromReceiptEntity = r => new ReceiptDto
        {
            Id = r.Id,
            ReceivedDate = r.ReceivedDate,
            Amount = r.Amount
        };
    }
}