using Microsoft.AspNet.SignalR;
using Orvosi.Data;
using Orvosi.Shared.Enums;
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
        private string _roomPrefix = "service-request-room-";

        public void PostMessage(string message, int serviceRequestId)
        {
            var roomName = _roomPrefix + serviceRequestId;

            using (var context = new Orvosi.Data.OrvosiDbContext())
            {
                var newMessage = new Orvosi.Data.ServiceRequestMessage()
                {
                    Id = Guid.NewGuid(),
                    Message = message,
                    UserId = Context.User.Identity.GetGuidUserId(),
                    PostedDate = SystemTime.UtcNow(),
                    ServiceRequestId = serviceRequestId
                };
                context.ServiceRequestMessages.Add(newMessage);
                context.SaveChanges();

                Clients.Group(roomName).addChatMessage(newMessage.Id, serviceRequestId);
            }
        }
        public System.Threading.Tasks.Task JoinRoom(string roomName)
        {
            return Groups.Add(Context.ConnectionId, roomName);
        }
        public async System.Threading.Tasks.Task JoinRooms(int[] serviceRequestIds)
        {
            foreach (var id in serviceRequestIds)
            {
                await Groups.Add(Context.ConnectionId, $"{_roomPrefix}{id}");
            }
        }
        public System.Threading.Tasks.Task LeaveRoom(string roomName)
        {
            return Groups.Remove(Context.ConnectionId, roomName);
        }
        public async System.Threading.Tasks.Task LeaveRooms(int[] serviceRequestIds)
        {
            foreach (var id in serviceRequestIds)
            {
                await Groups.Remove(Context.ConnectionId, $"{_roomPrefix}{id}");
            }
        }
        public override System.Threading.Tasks.Task OnConnected()
        {
            //var now = SystemTime.UtcNow().ToLocalTimeZone(TimeZones.EasternStandardTime);
            //DateTime day;
            //var dayQs = Context.Request.QueryString.FirstOrDefault(qs => qs.Key == "day");
            //if (!DateTime.TryParse(dayQs.Value, out day))
            //{
            //    day = now;
            //}

            //Guid? serviceProviderId = null;
            //var serviceProviderQs = Context.Request.QueryString.FirstOrDefault(qs => qs.Key.ToLower() == "serviceProviderId".ToLower());
            //if (!serviceProviderQs.Equals(default(KeyValuePair<string, string>)))
            //{
            //    serviceProviderId = new Guid(serviceProviderQs.Value);
            //}

            //var loggedInUserId = Context.User.Identity.GetGuidUserId();
            //var baseUrl = Context.Request.Url.ToString();

            //Guid userId = Context.User.Identity.GetGuidUserId();
            //// Admins can see the Service Provider dropdown and view other's dashboards. Otherwise, it displays the data of the current user.
            //if (Context.User.Identity.IsAdmin() && serviceProviderId.HasValue)
            //{
            //    userId = serviceProviderId.Value;
            //}
            ////userId = new Guid("8e9885d8-a0f7-49f6-9a3e-ff1b4d52f6a9");

            //using (var db = new OrvosiDbContext())
            //{
            //    // Retrieve user.
            //    var dayFolder = Models.ServiceRequestModels2.ServiceRequestMapper2.MapToToday(userId, day, now, userId, Context.Request.Url.ToString());
                
            //    if (dayFolder != null)
            //    {
            //        // Add to each assigned group.
            //        foreach (var request in dayFolder.Assessments)
            //        {
            //            Groups.Add(Context.ConnectionId, $"{_roomPrefix}{request.Id}");
            //        }
            //    }
            //}
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