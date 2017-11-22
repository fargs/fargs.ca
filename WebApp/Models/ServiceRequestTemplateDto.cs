using LinqKit;
using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace WebApp.Models
{
    public class ServiceRequestTemplateDto
    {
        public short Id { get; set; } // Id (Primary key)
        public string Name { get; set; } // Name (length: 128)
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)
        public bool IsDefault { get; set; } // IsDefault
        public System.Guid? PhysicianId { get; set; } // PhysicianId

        // Reverse navigation
        public virtual IEnumerable<PhysicianServiceRequestTemplateDto> PhysicianServiceRequestTemplates { get; set; } // Many to many mapping
        public virtual IEnumerable<ServiceRequestDto> ServiceRequests { get; set; } // ServiceRequest.FK_ServiceRequest_ServiceRequestTemplate
        public virtual IEnumerable<ServiceRequestTemplateTaskDto> TaskTemplates { get; set; } // ServiceRequestTemplateTask.FK_ServiceRequestTemplateTask_ServiceRequestTemplate

        // Foreign keys
        public virtual PersonDto Physician { get; set; } // FK_ServiceRequestTemplate_Physician

        public static Expression<Func<ServiceRequestTemplate, ServiceRequestTemplateDto>> FromServiceRequestTemplateEntity = srt => srt == null ? null : new ServiceRequestTemplateDto()
        {
            Id = srt.Id,
            Name = srt.Name,
            IsDefault = srt.IsDefault,
            PhysicianId = srt.PhysicianId,
            TaskTemplates = srt.ServiceRequestTemplateTasks.AsQueryable().Select(ServiceRequestTemplateTaskDto.FromEntity.Expand())
        };
    }
}