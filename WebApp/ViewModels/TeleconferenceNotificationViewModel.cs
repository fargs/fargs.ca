using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using System.Net.Mail;

namespace WebApp.ViewModels
{
    public class TeleconferenceNotificationViewModel : ICaseNotificationViewModel
    {
        public MailMessage Message { get; set; }

        public CaseNotificationViewModel CaseNotificationViewModel { get; set; }
        
        public TeleconferenceViewModel Teleconference { get; set; }
    }
}