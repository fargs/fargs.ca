using AutoMapper;
using Microsoft.Owin;
using Owin;
using Model;
using WebApp.ViewModels;
using WebApp.Library;
using System;

[assembly: OwinStartupAttribute(typeof(WebApp.Startup))]
namespace WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //TODO: Check if debug
            SystemTime.Now = () => new DateTime(2016, 01, 13, 00, 00, 00);

            ConfigureAuth(app);

            Mapper.CreateMap<SpecialRequestFormViewModel, SpecialRequest>();
        }
    }
}
