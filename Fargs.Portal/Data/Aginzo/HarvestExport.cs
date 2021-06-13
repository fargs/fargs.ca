using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Data.Aginzo
{
    public class HarvestExport
    {
        public long Id { get; set; }
        public int InvoiceId { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public string ItemType { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemQuantity { get; set; }
        public decimal ItemUnitPrice { get; set; }
        public decimal ItemAmount { get; set; }
        public decimal ItemDiscount { get; set; }
        public decimal ItemTax { get; set; }
        public decimal ItemTax2 { get; set; }
        public string Currency { get; set; }
        public string InvoiceType { get; set; }
        public string QB_Terms { get; set; }
        public string QB_BillDate { get; set; }
        public int QB_BillNo { get; set; }
        public string QB_Description { get; set; }
        public string ItemType2 { get; set; }
        public string QB_Account { get; set; }
        public string HarvestConsultant { get; set; }
        public string QB_Vendor { get; set; }
        public decimal PayAmount { get; set; }
        public string SalesLead_1 { get; set; }
        public string SL1_QB_Vendor { get; set; }
        public string SL1_QB_Account { get; set; }
        public decimal SL1_Percent { get; set; }
        public decimal SL1_Amount { get; set; }
        public string SalesSupport_1 { get; set; }
        public string SS1_QB_Vendor { get; set; }
        public string SS1_QB_Account { get; set; }
        public decimal SS1_Percent { get; set; }
        public decimal SS1_Amount { get; set; }
        public string SalesLead_2 { get; set; }
        public string SL2_QB_Vendor { get; set; }
        public string SL2_QB_Account { get; set; }
        public decimal SL2_Percent { get; set; }
        public decimal SL2_Amount { get; set; }
        public string SalesSupport_2 { get; set; }
        public string SS2_QB_Vendor { get; set; }
        public string SS2_QB_Account { get; set; }
        public decimal SS2_Percent { get; set; }
        public decimal SS2_Amount { get; set; }



    }
}
