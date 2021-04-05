using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;

namespace ImeHub.Portal.Data
{
    public class ApplicationUserLogin : IdentityUserLogin<Guid> { }
    public class ApplicationUserClaim : IdentityUserClaim<Guid> { }
    public class ApplicationUserRole : IdentityUserRole<Guid> { }
    public class ApplicationRole : IdentityRole<Guid>
    {
        public const string NewUserString = "BDAA9A65-CFAD-4AEC-9664-1BBD82BEF18B";
        public static Guid NewUser = new Guid(NewUserString);

    }
    public class ApplicationRoleClaim : IdentityRoleClaim<Guid> { }
    public class ApplicationUserToken : IdentityUserToken<Guid> { }

    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public string ColorCode { get; set; }

        public string DisplayName
        {
            get
            {
                string s;
                if (string.IsNullOrEmpty(this.Title))
                {
                    s = string.Format("{0} {1}", this.FirstName, this.LastName);
                }
                else
                {
                    s = string.Format("{0} {1} {2}", this.Title, this.FirstName, this.LastName);
                }
                return s;
            }
        }
        public string Initials
        {
            get
            {
                if (string.IsNullOrEmpty(FirstName))
                    return "Unassigned";
                else
                    return $"{FirstName.ToUpper().First()}{LastName.ToUpper().First()}";
            }
        }
    }

}