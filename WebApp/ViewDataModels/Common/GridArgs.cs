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
    }
}