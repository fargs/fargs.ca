using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Components.Grid
{
    public class GridColumn
    {
        public string DisplayName { get; set; }
        public bool IsSortable { get; internal set; }
        public string Name { get; internal set; }
    }
}