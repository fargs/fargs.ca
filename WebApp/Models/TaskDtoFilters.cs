using Orvosi.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using WebApp.Library;

namespace WebApp.Models
{
    public static class TaskDtoFilters
    {
        public static IEnumerable<TaskDto> AreActive(this IEnumerable<TaskDto> tasks)
        {
            return tasks
                .Where(srt => srt.StatusId == TaskStatuses.ToDo || srt.StatusId == TaskStatuses.Waiting || srt.StatusId == TaskStatuses.OnHold);
        }
        public static IEnumerable<TaskDto> AreAssignedToUser(this IEnumerable<TaskDto> tasks, Guid userId)
        {
            return tasks
                .Where(srt => srt.AssignedToId == userId);
        }
        public static IEnumerable<TaskDto> AreEffectiveBetween(this IEnumerable<TaskDto> tasks, DateFilter range)
        {
            var startDate = range.StartDate.Date;
            var endDate = range.EndDate.Date.AddDays(1);
            return tasks.Where(s => s.EffectiveDate.HasValue && s.EffectiveDate.Value >= startDate && s.EffectiveDate.Value < endDate);
        }
        public static IEnumerable<TaskDto> AreDueBetween(this IEnumerable<TaskDto> tasks, DateFilter range)
        {
            var startDate = range.StartDate.Date;
            var endDate = range.EndDate.Date.AddDays(1);
            return tasks
                .Where(s => s.DueDate.HasValue && s.DueDate.Value >= startDate && s.DueDate.Value < endDate); // this filters out the days
        }
        public static IEnumerable<TaskDto> AreAssignedToRoles(this IEnumerable<TaskDto> tasks, Guid?[] rolesThatShouldBeSeen)
        {
            return tasks.Where(t => rolesThatShouldBeSeen.Contains(t.ResponsibleRoleId));
        }
        public static IEnumerable<TaskDto> AreAssignedToUserOrRoles(this IEnumerable<TaskDto> tasks, Guid userId, Guid?[] rolesThatShouldBeSeen)
        {
            return tasks.Where(t => rolesThatShouldBeSeen.Contains(t.ResponsibleRoleId) || t.AssignedToId == userId);
        }
        public static IEnumerable<TaskDto> AreOnCriticalPath(this IEnumerable<TaskDto> tasks)
        {
            return tasks.Where(t => t.IsCriticalPath);
        }
        public static IEnumerable<TaskDto> ExcludeSubmitInvoice(this IEnumerable<TaskDto> tasks)
        {
            return tasks.Where(t => t.TaskId != Tasks.SubmitInvoice);
        }
    }
}