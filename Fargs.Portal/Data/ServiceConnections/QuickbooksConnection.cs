using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fargs.Portal.Data.ServiceConnections
{
    public class QuickbooksConnection
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string RealmId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string Scopes { get; set; }
    }
}
