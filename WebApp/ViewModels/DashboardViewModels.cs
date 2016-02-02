using Model;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using System;

namespace WebApp.ViewModels.DashboardViewModels
{
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            ThisWeekCards = new List<DateCard>();
            NextWeekCards = new List<DateCard>();
            TaskCards = new List<TaskCard>();
        }

        public User User { get; set; }
        public List<DateCard> ThisWeekCards { get; set; }
        public int? ThisWeekTotal { get; set; }
        public List<DateCard> NextWeekCards { get; set; }
        public int? NextWeekTotal { get; set; }
        public List<TaskCard> TaskCards { get; set; }

        public void AddCard(List<GetDashboardServiceRequest_Result> list, GetDashboardSchedule_Result summary)
        {
            ThisWeekCards.Add(new DateCard
            {
                Summary = summary,
                ServiceRequests = list
            });
        }

        public void AddTask(DashboardTaskSummary summary)
        {

            TaskCards.Add(new TaskCard
            {
                TaskName = summary.TaskName,
                AssignedToUserId = summary.AssignedToUserId,
                AssignedToDisplayName = summary.AssignedToDisplayName,
                Total = summary.TaskCount.Value
            });
        }

        public class DateCard
        {
            public DateCard()
            {
                ServiceRequests = new List<GetDashboardServiceRequest_Result>();
            }
            public GetDashboardSchedule_Result Summary { get; set; }
            public List<GetDashboardServiceRequest_Result> ServiceRequests { get; set; }
        }

        public class TaskCard
        {
            public string CardId { get; set; }
            public string CardTitle { get; set; }
            public string CardSubTitle { get; set; }
            public int Total { get; set; }
            public string TaskName { get; set; }
            public string AssignedToUserId { get; set; }
            public string AssignedToDisplayName { get; set; }
            public List<ServiceRequestTask> Tasks { get; set; }
        }
    }
}