using Orvosi.Shared.Enums;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace WebApp.Library.Helpers
{
    public static class HtmlHelpers
    {
        public static string IsActive(this HtmlHelper html, string control, string action)
        {
            return IsActive(html, control, action, string.Empty);
        }
        public static string IsActive(this HtmlHelper html,
                                        string control,
                                        string action,
                                        string area)
        {
            var routeData = html.ViewContext.RouteData;

            var routeArea = (string)routeData.DataTokens["area"];
            var routeAction = (string)routeData.Values["action"];
            var routeControl = (string)routeData.Values["controller"];

            // both must match
            var returnActive = 
                area == routeArea &&
                control == routeControl &&
                action == routeAction;

            return returnActive ? "active" : "";
        }
        

        public static string GetTaskStatusCSS(this HtmlHelper helper, byte taskStatusId, string assignedTo)
        {
            if (taskStatusId == TaskStatuses.ToDo)
            {
                return "default";
            }
            else if (taskStatusId == TaskStatuses.Done)
            {
                return "success";
            }
            else if (taskStatusId == TaskStatuses.Waiting)
            {
                return "warning";
            }
            else if (string.IsNullOrEmpty(assignedTo))
            {
                return "danger";
            }

            return string.Empty;
        }

        public static string GetTaskStatusIcon(this HtmlHelper helper, byte taskStatusId)
        {
            if (taskStatusId == TaskStatuses.ToDo)
            {
                return string.Empty;
            }
            else if (taskStatusId == TaskStatuses.Done)
            {
                return "glyphicon glyphicon-ok";
            }
            else if (taskStatusId == TaskStatuses.Waiting)
            {
                return "glyphicon glyphicon-hourglass";
            }

            return string.Empty;
        }

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

        public static String RenderViewToString(ControllerContext context, String viewPath, object model = null)
        {
            context.Controller.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(context, viewPath, null);
                var viewContext = new ViewContext(context, viewResult.View, context.Controller.ViewData, context.Controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(context, viewResult.View);
                return sw.GetStringBuilder().ToString();
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
                currentParentId = RoleCategories.Physician;
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