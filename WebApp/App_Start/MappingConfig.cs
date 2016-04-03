using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public static class MappingConfig
    {
        public static void RegisterMaps()
        {
            AutoMapper.Mapper.Initialize(config =>
            {
                //config.CreateMap<ServiceRequestTask, ViewModels.ServiceRequestTaskViewModels.UserTaskViewModel>();
            });
        }
    }
}