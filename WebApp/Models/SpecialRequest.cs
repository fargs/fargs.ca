using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApp.Models
{
    public class SpecialRequest
    {
        public short Id { get; set; }
        public string PhysicianId { get; set; }
        public short ServiceId { get; set; }
        public string Timeframe { get; set; }
        public string AdditionalNotes { get; set; }
        public string ModifiedUserName { get; set; }
        public string ModifiedUserId { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime ModifiedDate { get; set; }
    }
}