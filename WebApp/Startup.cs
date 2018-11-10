using ImeHub.Data;
using Microsoft.Owin;
using Owin;
using System;
using System.Data.Entity.Migrations;

[assembly: OwinStartupAttribute(typeof(WebApp.Startup))]
namespace WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //TODO: Check if debug
            //SystemTime.Now = () => new DateTime(2016, 09, 19, 00, 00, 00);

            ConfigureAuth(app);

            app.MapSignalR();

            using (var db = new ImeHubDbContext())
            {
                // ROLES

                var ROLE_SUPERADMIN = new Guid("7fab67dd-286b-492f-865a-0cb0ce1261ce");
                var role_superadmin = new Role { Id = ROLE_SUPERADMIN, Name = "Super Admin" };
                db.Roles.AddOrUpdate(role_superadmin);

                var ROLE_PHYSICIAN = new Guid("234df31c-69a1-4186-9815-1cf37233d448");
                var role_physician = new Role { Id = ROLE_PHYSICIAN, Name = "Physician" };
                db.Roles.AddOrUpdate(role_physician);

                // FEATURES
                db.Features.AddOrUpdate(new ImeHub.Models.Features.Physicians().ToFeature());
                var work = new ImeHub.Models.Features.Work().ToFeature();
                db.Features.AddOrUpdate(work);

                // ROLES AND FEATURES

                db.RoleFeatures.AddOrUpdate(new RoleFeature { RoleId = ROLE_PHYSICIAN, FeatureId = work.Id });
                
                db.SaveChanges();
            }

        }
    }
}
