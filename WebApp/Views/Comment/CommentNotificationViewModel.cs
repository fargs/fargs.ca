using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Models;
using WebApp.Views.Shared;

namespace WebApp.Views.Comment
{
    public class CommentNotificationViewModel : ICaseNotificationViewModel
    {
        public CaseNotificationViewModel CaseNotificationViewModel { get; set; }

        public CommentViewModel Comment { get; set; }
    }
}