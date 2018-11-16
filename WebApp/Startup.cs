using Microsoft.Owin;
using Owin;
using ImeHub.Data;
using ImeHub.Models.Database;

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
                var intializer = new DbInitializer(db);
                intializer.SeedDatabase();
            }

        }
    }
}
