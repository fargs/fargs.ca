using LinqKit;
using Orvosi.Data;
using System;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class InvoiceDetailDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        private decimal? _Rate;
        public decimal Rate
        {
            get
            {
                return _Rate.GetValueOrDefault(1);
            }
            set
            {
                _Rate = value;
            }
        }
        public decimal? Amount { get; set; }
        public bool HasDiscount => Discount != 1;
        public decimal Discount { get; set; }
        public string DiscountDescription { get; set; }
        public decimal? Total { get; set; }
        public string AdditionalNotes { get; set; }
        public int InvoiceId { get; set; }
        public int? ServiceRequestId { get; set; }
        public ServiceRequestDto ServiceRequest { get; set; }

        public static Expression<Func<InvoiceDetail, InvoiceDetailDto>> FromInvoiceDetailEntity = id => new InvoiceDetailDto
        {
            Id = id.Id,
            Description = id.Description,
            Amount = id.Amount,
            _Rate = id.Rate,
            Total = id.Total,
            DiscountDescription = id.DiscountDescription,
            AdditionalNotes = id.AdditionalNotes,
            InvoiceId = id.InvoiceId,
            ServiceRequestId = id.ServiceRequestId,
            //ServiceRequest = id.ServiceRequest == null ? null : ServiceRequestDto.FromServiceRequestEntityForInvoiceDetail.Invoke(id.ServiceRequest)
        };
    }
}