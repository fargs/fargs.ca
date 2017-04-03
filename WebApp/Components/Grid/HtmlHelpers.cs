using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.Components.Grid;
using WebApp.Library.Extensions;
using WebApp.ViewDataModels;

namespace WebApp.Components.Grid
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString GridColumnHeader(this HtmlHelper html, GridColumn col, GridArgs args)
        {
            string tag = $"<span>{col.DisplayName}</span>";

            if (col.IsSortable)
            {
                var a = new TagBuilder("a");
                a.InnerHtml = col.DisplayName;

                var link = html.ViewContext.HttpContext.Request.Url
                    .AddQuery("sort", col.Name.ToLower())
                    .AddQuery("sortDir", "asc");

                if (args.sort == col.Name.ToLower())
                {
                    args.sortDir = args.sortDir.ToLower() == "asc" ? "desc" : "asc";
                    link = link.AddQuery("sortDir", args.sortDir);

                    var i = new TagBuilder("i");
                    i.AddCssClass("fa fa-sort-" + args.sortDir);
                    a.InnerHtml += i.ToString();
                }

                a.Attributes.Add("onclick", $"taskgridArgsChanged('{html.Raw(link)}')");

                tag = a.ToString();
            }

            return new MvcHtmlString(tag);
        }
    }
}