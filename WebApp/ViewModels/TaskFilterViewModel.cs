using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using WebApp.Controllers;

namespace WebApp.ViewModels
{
    public class TaskFilterViewModel
    {
        public TaskFilterViewModel()
        {
        }

        public short[] SelectedTaskTypes { get; set; }
        public List<TaskIndexDto> Tasks { get; set; }
        public Guid SelectedUserId { get; set; }
    }

    public class TaskIndexDto
    {
        public short? Id { get; set; }
        public string Name { get; set; }
        public short? Sequence { get; internal set; }
    }
}