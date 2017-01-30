//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.Owin;
//using Microsoft.Owin.Security.Cookies;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using System.Web;

//namespace WebApp.Library
//{
//    public static class ImpersonatingSecurityStampValidator
//    {
//        public static Func<CookieValidateIdentityContext, Task> OnValidateIdentity<TManager, TUser>(
//            TimeSpan validateInterval, Func<TManager, TUser, Task<ClaimsIdentity>> regenerateIdentity)
//            where TManager : UserManager<TUser, string>
//            where TUser : class, IUser<string>
//        {
//            return OnValidateIdentity(validateInterval, regenerateIdentity, id => id.GetUserId());
//        }

//        public static Func<CookieValidateIdentityContext, Task> OnValidateIdentity<TManager, TUser, TKey>(
//            TimeSpan validateInterval, Func<TManager, TUser, Task<ClaimsIdentity>> regenerateIdentityCallback,
//            Func<ClaimsIdentity, TKey> getUserIdCallback)
//            where TManager : UserManager<TUser, TKey>
//            where TUser : class, IUser<TKey>
//            where TKey : IEquatable<TKey>
//        {
//            if (getUserIdCallback == null)
//            {
//                throw new ArgumentNullException("getUserIdCallback");
//            }
//            return async context =>
//            {
//                var currentUtc = DateTimeOffset.UtcNow;
//                if (context.Options != null && context.Options.SystemClock != null)
//                {
//                    currentUtc = context.Options.SystemClock.UtcNow;
//                }
//                var issuedUtc = context.Properties.IssuedUtc;

//                // Only validate if enough time has elapsed
//                var validate = (issuedUtc == null);
//                if (issuedUtc != null)
//                {
//                    var timeElapsed = currentUtc.Subtract(issuedUtc.Value);
//                    validate = timeElapsed > validateInterval;
//                }
//                if (validate)
//                {
//                    var manager = context.OwinContext.GetUserManager<ApplicationUserManager>();
//                    var userId = getUserIdCallback(context.Identity);
//                    if (manager != null && userId != null)
//                    {
//                        var user = await manager.FindByIdAsync(userId);
//                        var reject = true;
//                        // Refresh the identity if the stamp matches, otherwise reject
//                        if (user != null && manager.SupportsUserSecurityStamp)
//                        {
//                            var securityStamp =
//                                context.Identity.FindFirstValue(Constants.DefaultSecurityStampClaimType);
//                            if (securityStamp == await manager.GetSecurityStampAsync(userId))
//                            {
//                                reject = false;
//                                // Regenerate fresh claims if possible and resign in
//                                if (regenerateIdentityCallback != null)
//                                {
//                                    var identity = await regenerateIdentityCallback.Invoke(manager, user);
//                                    if (identity != null)
//                                    {
//                                        /**** CHANGES START HERE ****/
//                                        if (context.Identity.FindFirstValue("UserImpersonation") == "true")
//                                        {
//                                            // need to preserve impersonation claims
//                                            identity.AddClaim(new Claim("UserImpersonation", "true"));
//                                            identity.AddClaim(context.Identity.FindFirst("OriginalUserId"));
//                                        }
//                                        /**** CHANGES END HERE ****/

//                                        context.OwinContext.Authentication.SignIn(context.Properties, identity);
//                                    }
//                                }
//                            }
//                        }
//                        if (reject)
//                        {
//                            context.RejectIdentity();
//                            context.OwinContext.Authentication.SignOut(context.Options.AuthenticationType);
//                        }
//                    }
//                }
//            };
//        }
//    }
//}