using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewModels.DashboardViewModels
{
    public class AgendaViewModel
    {
        public AgendaViewModel()
        {
        }
        public Orvosi.Shared.Model.DayFolder DayFolder { get; set; }
        public DateTime Day { get; set; }
        public Guid SelectedUserId { get; set; }
        public List<SelectListItem> UserSelectList { get; set; }
        public ServiceRequestMessageJSViewModel ServiceRequestMessageJSViewModel { get; set; }
    }
}