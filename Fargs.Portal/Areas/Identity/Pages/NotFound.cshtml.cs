using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Fargs.Portal.Areas.Identity.Pages
{
    [AllowAnonymous]
    public class NotFoundModel : PageModel
    {
        public void OnGet()
        {

        }
    }
}

