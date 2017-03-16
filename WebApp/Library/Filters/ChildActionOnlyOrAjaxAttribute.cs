using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Library.Filters
{
    public class ChildActionOnlyOrAjaxAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!(filterContext.IsChildAction || filterContext.HttpContext.Request.IsAjaxRequest()))
            {
                throw new Exception($"The action {filterContext.ActionDescriptor.ActionName} is accessible only by a child request or by an ajax call.");
            }

            base.OnActionExecuting(filterContext);
        }
    }
}