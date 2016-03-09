using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Model
{
    public static class XmlExtensionMethods
    {
        public static string GetAsString(this XElement attr)
        {
            string ret = string.Empty;

            if (attr != null && !string.IsNullOrEmpty(attr.Value))
            {
                ret = attr.Value;
            }

            return ret;
        }
        public static int GetAsInteger(this XElement attr)
        {
            int ret = 0;
            int value = 0;

            if (attr != null && !string.IsNullOrEmpty(attr.Value))
            {
                if (int.TryParse(attr.Value, out value))
                    ret = value;
            }

            return ret;
        }

        public static short? GetAsShort(this XElement attr)
        {
            short? ret = null;
            short value = 0;

            if (attr != null && !string.IsNullOrEmpty(attr.Value))
            {
                if (short.TryParse(attr.Value, out value))
                    ret = value;
            }

            return ret;
        }

        public static decimal? GetAsDecimal(this XElement attr)
        {
            decimal? ret = null;
            decimal value = 0;

            if (attr != null && !string.IsNullOrEmpty(attr.Value))
            {
                if (decimal.TryParse(attr.Value, out value))
                    ret = value;
            }

            return ret;
        }
    }

    partial class Invoice
    {
        public decimal ServiceRequestPrice { get; set; }
        public string Notes { get; set; }
    }

    public class ServiceCatalogueItem
    {
        public Nullable<short> ServiceCatalogueId { get; set; }
        public short ServiceId { get; set; }
        public Nullable<short> LocationId { get; set; }
        public decimal ServicePrice { get; set; }
        public Nullable<decimal> Price { get; set; }
    }

    partial class GetServiceCatalogue_Result
    {
        public ServiceCatalogueItem ServiceCatalogueItem
        {
            get
            {
                return LoadItem(this.ServiceCatalogueData);
            }
        }

        private ServiceCatalogueItem LoadItem(string xml)
        {
            if (!string.IsNullOrEmpty(xml))
            {
                XDocument sc = XDocument.Parse(xml);
                var e = from d in sc.Descendants("ServiceCatalogue")
                        select new ServiceCatalogueItem
                        {
                            ServiceCatalogueId = d.Element("ServiceCatalogueId").GetAsShort(),
                            ServiceId = d.Element("ServiceId").GetAsShort().Value,
                            LocationId = d.Element("LocationId").GetAsShort(),
                            ServicePrice = d.Element("ServicePrice").GetAsDecimal().Value,
                            Price = d.Element("Price").GetAsDecimal()
                        };
                if (e != null)
                {
                    return e.FirstOrDefault();
                }
            }
            return null;
        }
    }
    
}
