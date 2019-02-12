using Box.V2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Library.Extensions
{
    public static class BoxExtensions
    {
        public static List<BoxItem> Entries(this BoxFolder value)
        {
            return value.ItemCollection == null ? new List<BoxItem>() : value.ItemCollection.Entries;
        }
    }
}