using Microsoft.AspNet.SignalR;
using Orvosi.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApp.Library;
using WebApp.Library.Extensions;

namespace WebApp
{
    public class ServiceRequestHub : Hub
    {
        static ConcurrentDictionary<string, string> dic = new ConcurrentDictionary<string, string>();
        private string _roomPrefix = "servicerequestroom_";

        public void Send(string name, string message, string roomName)
        {
            // Call the broadcastMessage method to update clients.
            Clients.OthersInGroup(roomName).addChatMessage(name, message);
        }
        public System.Threading.Tasks.Task JoinRoom(string roomName)
        {
            return Groups.Add(Context.ConnectionId, roomName);
        }
        public System.Threading.Tasks.Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            using (var db = new OrvosiDbContext())
            {
                // Retrieve user.
                var tasks = db.API_GetAssignedServiceRequests(Context.User.Identity.GetGuidUserId(), SystemTime.Now());

                var requestIds = tasks.Where(u => u.AssignedTo == Context.User.Identity.GetGuidUserId())
                    .Select(t => t.ServiceRequestId)
                    .Distinct();
                
                // Add to each assigned group.
                foreach (var request in requestIds)
                {
                    Groups.Add(Context.ConnectionId, $"servicerequestroom_{request}");
                }
            }
            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            return base.OnDisconnected(stopCalled);
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            return base.OnReconnected();
        }
    }
}