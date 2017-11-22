using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace WebApp.Models
{
    public class ServiceRequestTemplateTaskDto
    {
        public System.Guid Id { get; set; } // Id (Primary key)
        public short? Sequence { get; set; } // Sequence
        public short ServiceRequestTemplateId { get; set; } // ServiceRequestTemplateId
        public short? TaskId { get; set; } // TaskId
        public string DueDateType { get; set; } // DueDateType (length: 10)
        public System.Guid? ResponsibleRoleId { get; set; } // ResponsibleRoleId
        public bool IsBaselineDate { get; set; } // IsBaselineDate
        public short? DueDateDurationFromBaseline { get; set; } // DueDateDurationFromBaseline
        public short? EffectiveDateDurationFromBaseline { get; set; } // EffectiveDateDurationFromBaseline
        public bool IsCriticalPath { get; set; } // IsCriticalPath
        public bool IsBillable { get; set; } // IsBillable
        public bool IsDeleted { get; set; } // IsDeleted
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)

        public string DueDateTypeTrimmed { get
            {
                return string.IsNullOrEmpty(DueDateType) ? string.Empty : DueDateType.Trim(' ');
            }
        }

        // Reverse navigation
        public virtual IEnumerable<TaskDto> ServiceRequestTasks { get; set; } // ServiceRequestTask.FK_ServiceRequestTask_ServiceRequestTemplateTask
        public virtual IEnumerable<ServiceRequestTemplateTaskDto> Child { get; set; } // Many to many mapping
        public virtual IEnumerable<ServiceRequestTemplateTaskDto> Parent { get; set; } // Many to many mapping

        // Foreign keys
        public virtual LookupDto<Guid> ResponsibleRole { get; set; } // FK_ServiceRequestTemplateTask_AspNetRoles
        public virtual TaskDto TaskDefinition { get; set; } // FK_ServiceRequestTemplateTask_Task
        public virtual ServiceRequestTemplateDto ServiceRequestTemplate { get; set; } // FK_ServiceRequestTemplateTask_ServiceRequestTemplate

        public static Expression<Func<ServiceRequestTemplateTask, ServiceRequestTemplateTaskDto>> FromEntity = srt => srt == null ? null : new ServiceRequestTemplateTaskDto()
        {
            Id = srt.Id,
            Sequence = srt.Sequence,
            ServiceRequestTemplateId = srt.ServiceRequestTemplateId,
            TaskId = srt.TaskId,
            DueDateType = srt.DueDateType,
            ResponsibleRoleId = srt.ResponsibleRoleId,
            IsBaselineDate = srt.IsBaselineDate,
            DueDateDurationFromBaseline = srt.DueDateDurationFromBaseline,
            EffectiveDateDurationFromBaseline = srt.EffectiveDateDurationFromBaseline,
            IsCriticalPath = srt.IsCriticalPath,
            IsBillable = srt.IsBillable,
            IsDeleted = srt.IsDeleted
        };
    }
}