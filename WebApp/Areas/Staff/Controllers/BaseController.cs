using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Areas.Staff.Controllers
{
    [Authorize(Roles = "Case Coordinator,Intake Assistant, Accountant")]
    public class BaseController : Controller
    {
    }
}