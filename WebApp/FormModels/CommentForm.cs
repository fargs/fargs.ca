using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApp.ViewModels;

namespace WebApp.FormModels
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
        public ContactViewModel<Guid> Physician { get; set; }
        public List<Guid> AccessList { get; set; }
    }
}