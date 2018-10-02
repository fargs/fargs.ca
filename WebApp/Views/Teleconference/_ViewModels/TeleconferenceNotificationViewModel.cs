using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using System.Net.Mail;
using WebApp.Views.Shared;

namespace WebApp.Views.Teleconference
{
    public class TeleconferenceNotificationViewModel : ICaseNotificationViewModel
    {
        public MailMessage Message { get; set; }

        public CaseNotificationViewModel CaseNotificationViewModel { get; set; }
        
        public TeleconferenceViewModel Teleconference { get; set; }
    }
}