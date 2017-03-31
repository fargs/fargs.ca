using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Orvosi.Data;
using Orvosi.Shared.Enums;

namespace WebApp.ViewModels.ServiceRequestTaskViewModels
{
    //public class TaskViewModel
    //{
    //    public int Id { get; set; }
    //    public int ServiceRequestId { get; set; }
    //    public short? TaskId { get; set; }
    //    public string Name { get; set; }
    //    public string ShortName { get; set; }
    //    public Guid? AssignedTo { get; set; }
    //    public Guid? AssignedToRoleId { get; set; }
    //    public string Initials { get; set; }
    //    public DateTime? CompletedDate { get; set; }
    //    public bool IsObsolete { get; set; } = false;
    //    public byte? DueDateBase { get; set; }
    //    public short? DueDateDiff { get; set; }
    //    public DateTime? AppointmentDate { get; set; }
    //    public DateTime? ReportDate { get; set; }
    //    public string DependsOn { get; set; }
    //    public short? Sequence { get; set; }
    //    public List<TaskViewModel> Dependencies { get; set; }
    //    public TaskViewModel Parent { get; set; }
    //    public TaskStatusViewModel Status
    //    {
    //        get
    //        {
    //            if (this.CompletedDate.HasValue)
    //            {
    //                return new TaskStatusViewModel(TaskStatuses.Done);
    //            }
    //            else if (this.IsObsolete == true)
    //            {
    //                return new TaskStatusViewModel(TaskStatuses.Obsolete);
    //            }
    //            else if (this.DependsOn == "ExamDate")
    //            {
    //                if (DateTime.Now >= this.AppointmentDate)
    //                {
    //                    return new TaskStatusViewModel(TaskStatuses.ToDo);
    //                }
    //                return new TaskStatusViewModel(TaskStatuses.Waiting);
    //            }
    //            else if (string.IsNullOrEmpty(this.DependsOn))
    //            {
    //                return new TaskStatusViewModel(TaskStatuses.ToDo);
    //            }
    //            else
    //            {
    //                var status = new TaskStatusViewModel(TaskStatuses.ToDo);
    //                foreach (var item in Dependencies.Where(d => !d.CompletedDate.HasValue && d.IsObsolete == false))
    //                {
    //                    status.Id = TaskStatuses.Waiting;
    //                    status.Name = "Waiting On";
    //                    status.WaitingOn.Add(item);
    //                }
    //                return status;
    //            }
    //        }
    //    }
    //    public string AssignedToDisplayName { get; internal set; }
    //    public string AssignedToColorCode { get; internal set; }
    //    public DateTime DueDate { get; set; }

    //    public TaskViewModel()
    //    {
    //        Dependencies = new List<TaskViewModel>();
    //    }
    //}

    //public class NextTaskViewModel
    //{
    //    public int RowNum { get; set; }
    //    public int Id { get; set; }
    //    public int ServiceRequestId { get; set; }
    //    public short? TaskId { get; set; }
    //    public string Name { get; set; }
    //    public string ShortName { get; set; }
    //    public byte StatusId { get; set; }
    //    public string StatusName { get; set; }
    //    public Guid? AssignedTo { get; set; }
    //    public Guid? AssignedToRoleId { get; set; }
    //    public string Initials { get; set; }
    //    public string AssignedToDisplayName { get; internal set; }
    //    public string AssignedToColorCode { get; internal set; }
    //    public DateTime DueDate { get; set; }
    //}

    //public class FilterArgs
    //{
    //    public string Sort { get; set; }
    //    public byte? DateRange { get; set; }
    //    public byte? StatusId { get; set; }
    //    public string Ids { get; set; }
    //    public string ClaimantName { get; set; }
    //    public string PhysicianId { get; set; }
    //    public bool? ShowAll { get; set; }
    //}
}