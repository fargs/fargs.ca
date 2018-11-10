using System.Web.Mvc;
using WebApp.Library.Filters;
using Features = Orvosi.Shared.Enums.Features;
using WebApp.Areas.Admin.Views.Home;
using System.Security.Principal;
using System;

namespace WebApp.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IIdentity identity, DateTime now) : base(identity, now)
        {
        }
        [AuthorizeRole(Feature = Features.Admin.Section)]
        public ActionResult Index()
        {
            var viewModel = new IndexViewModel(identity, now);
            return View(viewModel);
        }
    }
}