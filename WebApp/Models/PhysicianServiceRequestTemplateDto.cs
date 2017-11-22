using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class PhysicianServiceRequestTemplateDto
    {
        public System.Guid PhysicianId { get; set; } // PhysicianId (Primary key)
        public short ServiceRequestTemplateId { get; set; } // ServiceRequestTemplateId (Primary key)
        public short? ServiceCategoryId { get; set; } // ServiceCategoryId
        public short? ServiceId { get; set; } // ServiceId
        public System.DateTime ModifiedDate { get; set; } // ModifiedDate
        public string ModifiedUser { get; set; } // ModifiedUser (length: 100)

        // Foreign keys
        public virtual ServiceRequestTemplateDto ServiceRequestTemplate { get; set; } // FK_Physician_ServiceRequestTemplate_ServiceRequestTemplate
    }
}