using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using WebApp.Models;
using WebApp.ViewModels;

namespace WebApp.FormModels
{
    public class AdditionalResourceForm
    {
        [Required]
        public Guid PhysicianId { get; set; }
        [Required]
        public int ServiceRequestId { get; set; }
        [Required]
        public Guid UserId { get; set; }
        public Guid? ResourceId { get; set; }

        public static Expression<Func<ResourceDto, AdditionalResourceForm>> FromResourceDto = dto => dto == null ? null : new AdditionalResourceForm
        {
            ServiceRequestId = dto.ServiceRequestId,
            UserId = dto.UserId,
            ResourceId = dto.Id
        };
    }

    public class RequiredResourceForm
    {
        public Guid? UserId { get; set; }
        [Required]
        public Guid? RoleId { get; set; }

        public static Expression<Func<ResourceDto, RequiredResourceForm>> FromResourceDto = dto => dto == null ? null : new RequiredResourceForm
        {
            UserId = dto.UserId,
            RoleId = dto.RoleId
        };
    }
}