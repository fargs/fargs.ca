using MarkdownDeep;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fargs.Web.Library.Extensions
{
    public static class MarkdownHelper
    {
        static readonly Markdown md = new Markdown();

        public static IHtmlString Markdown(this HtmlHelper helper, string text)
        {
            var html = md.Transform(text);
            return MvcHtmlString.Create(html);
        }
    }
}