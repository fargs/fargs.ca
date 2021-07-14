using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Data.Aginzo
{
    public class HarvestExport_GroupingKey
    {
        public long Id { get; set; }
        public int InvoiceId { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public string ItemType { get; set; }
        public string ItemDescription { get; set; }
        public string HarvestConsultant { get; set; }
        public string QB_Description { get; set; }
        public string QB_Vendor { get; set; }
    }

    class HarvestExport_GroupingKeyComparer : IEqualityComparer<HarvestExport_GroupingKey>
    {
        public bool Equals(HarvestExport_GroupingKey left, HarvestExport_GroupingKey right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            return left.InvoiceId == right.InvoiceId && left.QB_Vendor == right.QB_Vendor;
        }

        public int GetHashCode(HarvestExport_GroupingKey obj)
        {
            return (obj.InvoiceId).GetHashCode() + obj.QB_Vendor.GetHashCode();
        }
    }
    public class HarvestExport_Grouping
    {
        public HarvestExport_GroupingKey Key { get; set; }
        public IEnumerable<HarvestExport_LineItemGrouping> LineItems { get; set; }
    }

    public class HarvestExport_LineItemGroupingKey : HarvestExport_GroupingKey
    {
        public string QB_Service { get; set; }
        public string QB_Account { get; set; }
    }

    class HarvestExport_LineItemGroupingKeyComparer : IEqualityComparer<HarvestExport_LineItemGroupingKey>
    {
        public bool Equals(HarvestExport_LineItemGroupingKey left, HarvestExport_LineItemGroupingKey right)
        {
            if ((object)left == null && (object)right == null)
            {
                return true;
            }
            if ((object)left == null || (object)right == null)
            {
                return false;
            }
            return left.InvoiceId == right.InvoiceId && left.QB_Vendor == right.QB_Vendor && left.QB_Service == right.QB_Service;
        }

        public int GetHashCode(HarvestExport_LineItemGroupingKey obj)
        {
            return (obj.InvoiceId).GetHashCode() + obj.QB_Vendor.GetHashCode() + obj.QB_Service.GetHashCode();
        }
    }

    public class HarvestExport_LineItemGrouping
    {
        public HarvestExport_LineItemGroupingKey Key { get; set; }
        public decimal QB_Amount { get; set; }
    }
}
