using AutoMapper;
using Microsoft.Owin;
using Owin;
using Orvosi.Data;
using WebApp.ViewModels;
using WebApp.Library;
using System;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartupAttribute(typeof(WebApp.Startup))]
namespace WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //TODO: Check if debug
            SystemTime.Now = () => new DateTime(2016, 07, 09, 00, 00, 00);

            ConfigureAuth(app);

            app.MapSignalR();
        }
    }
}
