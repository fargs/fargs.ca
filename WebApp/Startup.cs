using Microsoft.Owin;
using Owin;

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
        }
    }
}
