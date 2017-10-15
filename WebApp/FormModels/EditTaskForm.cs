using System;

namespace WebApp.FormModels
{
    public class EditTaskForm
    {
        public EditTaskForm()
        {
        }

        public int ServiceRequestTaskId { get; set; }
        public DateTime DueDate { get; set; }
    }
}