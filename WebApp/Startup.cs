using Microsoft.Owin;
using Owin;
using WebApp.Models;
using WebApp.ViewModels;

[assembly: OwinStartupAttribute(typeof(WebApp.Startup))]
namespace WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            AutoMapper.Mapper.CreateMap<SpecialRequestFormViewModel, SpecialRequest>();
        }
    }
}
