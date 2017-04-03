using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.ViewDataModels
{
    public class GridArgs
    {
        public string sort { get; set; }
        public string sortDir { get; set; } = "asc";
        public int take { get; set; } = 50;
        public int skip { get; set; } = 0;
        public string searchTerms { get; set; }
        public string PostBackUrl { get; set; }

        public string ToQueryString()
        {
            var properties = from p in this.GetType().GetProperties()
                             where p.GetValue(this, null) != null && p.Name != "PostBackUrl"
                             select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(this, null).ToString());

            return string.Join("&", properties.ToArray());
        }
    }
}