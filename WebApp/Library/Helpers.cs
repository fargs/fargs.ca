using Model.Enums;
using System;
using System.IO;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace WebApp.Library.Helpers
{
    public static class HtmlHelpers
    {
        public static string RenderPartialViewToString(Controller controller, string viewName, object model)
        {
            controller.ViewData.Model = model;
            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.ToString();
            }
        }

        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string area)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");

            var builder = new TagBuilder("li")
            {
                InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName, new { area = area }).ToHtmlString()
            };

            if (controllerName == currentController && actionName == currentAction)
                builder.AddCssClass("active");

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString UserMenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string area, Nullable<byte> parentId)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            var currentParentIdString = htmlHelper.ViewContext.RouteData.Values["parentId"].ToString();
            Nullable<int> currentParentId = null;

            if (string.IsNullOrEmpty(currentParentIdString))
            {
                currentParentId = RoleCategory.Physician;
            }
            else
            {
                currentParentId = byte.Parse(currentParentIdString);
            }

            var builder = new TagBuilder("li")
            {
                InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName, new { area = area, parentId = parentId }, new { }).ToHtmlString()
            };

            if (controllerName == currentController && actionName == currentAction && parentId == currentParentId)
                builder.AddCssClass("active");

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string area, Nullable<byte> parentId)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            var currentParentIdString = htmlHelper.ViewContext.HttpContext.Request.QueryString["parentId"];
            Nullable<int> currentParentId = null;

            if (string.IsNullOrEmpty(currentParentIdString))
            {
                currentParentId = null;
            }
            else
            {
                currentParentId = byte.Parse(currentParentIdString);
            }

            var builder = new TagBuilder("li")
            {
                InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName, new { area = area, parentId = parentId }, new { }).ToHtmlString()
            };

            if (controllerName == currentController && actionName == currentAction && parentId == currentParentId)
                builder.AddCssClass("active");

            return new MvcHtmlString(builder.ToString());
        }

        public static MvcHtmlString MenuLink(this HtmlHelper htmlHelper, string linkText, string actionName, string controllerName, string area, string userId, Nullable<byte> parentId)
        {
            var currentAction = htmlHelper.ViewContext.RouteData.GetRequiredString("action");
            var currentController = htmlHelper.ViewContext.RouteData.GetRequiredString("controller");
            var currentParentIdString = htmlHelper.ViewContext.HttpContext.Request.QueryString["parentId"];
            Nullable<int> currentParentId = null;

            if (string.IsNullOrEmpty(currentParentIdString))
            {
                currentParentId = null;
            }
            else
            {
                currentParentId = byte.Parse(currentParentIdString);
            }

            var builder = new TagBuilder("li")
            {
                InnerHtml = htmlHelper.ActionLink(linkText, actionName, controllerName, new { area = area, userId = userId, parentId = parentId }, new { }).ToHtmlString()
            };

            if (controllerName == currentController && actionName == currentAction && parentId == currentParentId)
                builder.AddCssClass("active");

            return new MvcHtmlString(builder.ToString());
        }
    }
}