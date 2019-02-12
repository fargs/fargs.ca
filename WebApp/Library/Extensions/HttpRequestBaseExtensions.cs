using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Library.Extensions
{
    public static class HttpRequestBaseExtensions
    {
        public static string GetBaseUrl(this HttpRequestBase request)
        {
            if (request.Url == (Uri)null)
                return string.Empty;
            else
                return request.Url.Scheme + "://" + request.Url.Authority + VirtualPathUtility.ToAbsolute("~");
        }
    }
}