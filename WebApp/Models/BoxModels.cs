using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.BoxModels
{
    public class AuthModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class ErrorModel
    {
        public string Message { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
    }
}