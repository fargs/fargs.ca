using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Admin.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class BaseController : Controller
    {
    }
}