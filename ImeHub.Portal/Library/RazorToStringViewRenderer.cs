using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace ImeHub.Portal.Library
{
    public interface IRazorToStringViewRenderer
    {
        Task<string> RenderAsync<TModel>(string name, TModel model);
    }

    public class RazorToStringViewRenderer : IRazorToStringViewRenderer
    {
        private readonly IRazorViewEngine _viewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;

        public RazorToStringViewRenderer(
            IRazorViewEngine viewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider)
        {
            _viewEngine = viewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public async Task<string> RenderAsync<TModel>(string name, TModel model)
        {
            var actionContext = GetActionContext();

            var viewEngineResult = _viewEngine.GetView(name, name, isMainPage: false);

            if (!viewEngineResult.Success)
            {
                viewEngineResult = _viewEngine.FindView(actionContext, name, isMainPage: false);
            }

            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", name));
            }

            var view = viewEngineResult.View;

            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                    actionContext,
                    view,
                    new ViewDataDictionary<TModel>(
                        metadataProvider: new EmptyModelMetadataProvider(),
                        modelState: new ModelStateDictionary())
                    {
                        Model = model
                    },
                    new TempDataDictionary(
                        actionContext.HttpContext,
                        _tempDataProvider),
                    output,
                    new HtmlHelperOptions());

                await view.RenderAsync(viewContext);

                return output.ToString();
            }
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext
            {
                RequestServices = _serviceProvider
            };
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }

    public interface IPageRenderService
    {
        Task<string> RenderPageToStringAsync(string pageName, object model);
    }

    public class PageRenderService : IPageRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IActionContextAccessor _actionContext;

        public PageRenderService(
            IRazorViewEngine razorViewEngine,
            ITempDataProvider tempDataProvider,
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContext,
            IActionContextAccessor actionContext
            )
        {
            _razorViewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
            _httpContext = httpContext;
            _actionContext = actionContext;
        }

        public async Task<string> RenderPageToStringAsync(string pageName, object model)
        {
            var tempData = new TempDataDictionary(_httpContext.HttpContext, _tempDataProvider);

            var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var actionContext = new ActionContext(
                _httpContext.HttpContext,
                _httpContext.HttpContext.GetRouteData(),
                _actionContext.ActionContext.ActionDescriptor
            );

            var page = _razorViewEngine.FindPage(actionContext, pageName).Page;
            if (page == null)
            {
                throw new ArgumentNullException($"Sorry! {pageName} does not match any available page");
            }

            using (var sw = new StringWriter())
            {
                // TODO seemingly the page.ViewContext.View is empty and results in a nullreferenceexception
                page.ViewContext = new ViewContext(
                        actionContext,
                        page.ViewContext.View,
                        viewDictionary,
                        tempData,
                        sw,
                        new HtmlHelperOptions()
                );
                page.ViewContext.ViewData = viewDictionary;
                await page.ExecuteAsync();
            }

            return page.BodyContent.ToString();

        }
    }
}
