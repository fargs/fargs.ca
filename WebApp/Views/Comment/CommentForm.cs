using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.Views.Shared;

namespace WebApp.Views.Comment
{
    public class CommentForm
    {
        public CommentForm()
        {
            this.AccessList = new List<Guid>();
        }
        public Guid? CommentId { get; set; }
        public int ServiceRequestId { get; set; }
        [Required]
        public string Message { get; set; }
        [Required]
        public byte CommentTypeId { get; set; }
        public bool IsPrivate { get; set; }
        public ContactViewModel Physician { get; set; }
        public List<Guid> AccessList { get; set; }
    }
}