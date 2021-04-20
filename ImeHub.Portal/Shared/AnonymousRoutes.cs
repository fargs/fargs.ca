using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ImeHub.Portal.Shared
{
    /// <summary>
    /// All routes begin with a forward slash.
    /// Any route name ending with the word Base will include a forward trailing slash.
    /// </summary>
    public static class AnonymousRoutes
    {
        public const string RegisterBase    = "/identity/account/register/";
        public const string NotFound        = "/notfound";
        public const string Unauthorized    = "/unauthorized";
    }
}
