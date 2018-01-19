using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.FormModels
{
    public class PickupTasksAssignedToRoleForm
    {
        public int ServiceRequestId { get; set; }
        public Guid RoleId { get; set; }
        public Guid? UserId { get; set; }
    }
}