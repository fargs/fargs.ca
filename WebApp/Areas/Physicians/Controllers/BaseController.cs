using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Physicians.Controllers
{
    [Authorize(Roles = "Physician")]
    public class BaseController : Controller
    {
    }
}