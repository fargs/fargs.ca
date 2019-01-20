using Microsoft.Owin;
using Owin;
using ImeHub.Data;
using ImeHub.Models.Database;
using System;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;
using Enums = ImeHub.Models.Enums;

[assembly: OwinStartupAttribute(typeof(WebApp.Startup))]
namespace WebApp
{
    public partial class Startup
    {
        public async void Configuration(IAppBuilder app)
        {
            //TODO: Check if debug
            //SystemTime.Now = () => new DateTime(2016, 09, 19, 00, 00, 00);

            ConfigureAuth(app);

            app.MapSignalR();

            var superAdminUserId = new Guid("00190852-3FA5-4234-80E0-C5784073BC1D");
            using (var db = new ImeHubDbContext())
            {
                var intializer = new DbInitializer(db);
                intializer.SeedDatabase();

                db.Users.AddOrUpdate(u => u.Id, new User { Id = superAdminUserId, UserName = "lesliefarago", Email = "lesliefarago@gmail.com", EmailConfirmed = true, FirstName = "Leslie", LastName = "Farago", ColorCode = "#0B0B61", RoleId = Enums.Role.SuperAdmin });

                await db.SaveChangesAsync();
            }

            var context = Models.ApplicationDbContext.Create();
            var userStore = new UserStore<Models.ApplicationUser, Models.ApplicationRole, Guid, Models.ApplicationUserLogin, Models.ApplicationUserRole, Models.ApplicationUserClaim>(context);
            var userManager = new ApplicationUserManager(userStore);
            var result = await userManager.AddPasswordAsync(superAdminUserId, "P@ssword1");
        }
    }
}
