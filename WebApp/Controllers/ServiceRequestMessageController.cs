using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Orvosi.Shared.Model;
using System.Net;
using WebApp.Library.Extensions;

namespace WebApp.Controllers
{
    public class ServiceRequestMessageController : Controller
    {
        // GET: ServiceRequestMessage
        public ActionResult Discussion(int serviceRequestId)
        {
            using (var context = new Orvosi.Data.OrvosiDbContext())
            {
                var data = context.ServiceRequests
                    .Where(sr => sr.Id == serviceRequestId)
                    .Select(sr => new ServiceRequest
                    {
                        Id = sr.Id,
                        ClaimantName = sr.ClaimantName,
                        Messages = sr.ServiceRequestMessages.OrderBy(srm => srm.PostedDate).Select(srm => new ServiceRequestMessage
                        {
                            Id = srm.Id,
                            Message = srm.Message,
                            PostedDate = srm.PostedDate,
                            PostedBy = new Person
                            {
                                Id = srm.AspNetUser.Id,
                                Title = srm.AspNetUser.Title,
                                FirstName = srm.AspNetUser.FirstName,
                                LastName = srm.AspNetUser.LastName,
                                Email = srm.AspNetUser.Email,
                                ColorCode = srm.AspNetUser.ColorCode,
                                Role = srm.AspNetUser.AspNetUserRoles.Select(r => new UserRole
                                {
                                    Id = r.AspNetRole.Id,
                                    Name = r.AspNetRole.Name
                                }).FirstOrDefault()
                            }
                        })
                    }).FirstOrDefault();
                return PartialView("_Discussion", data);
            }
        }

        public ActionResult PostMessage(int serviceRequestId, string message)
        {
            using (var context = new Orvosi.Data.OrvosiDbContext())
            {
                var newMessage = new Orvosi.Data.ServiceRequestMessage()
                {
                    Id = Guid.NewGuid(),
                    Message = message,
                    UserId = User.Identity.GetGuidUserId(),
                    PostedDate = SystemTime.UtcNow(),
                    ServiceRequestId = serviceRequestId
                };
                context.ServiceRequestMessages.Add(newMessage);
                context.SaveChanges();
                return Json(newMessage);
            }
        }

        public ActionResult GetMessage(Guid serviceRequestMessageId)
        {
            using (var context = new Orvosi.Data.OrvosiDbContext())
            {
                var data = context.ServiceRequestMessages
                    .Where(srm => srm.Id == serviceRequestMessageId)
                    .Select(srm => new ServiceRequestMessage
                    {
                        Id = srm.Id,
                        Message = srm.Message,
                        PostedDate = srm.PostedDate,
                        PostedBy = new Person
                        {
                            Id = srm.AspNetUser.Id,
                            Title = srm.AspNetUser.Title,
                            FirstName = srm.AspNetUser.FirstName,
                            LastName = srm.AspNetUser.LastName,
                            Email = srm.AspNetUser.Email,
                            ColorCode = srm.AspNetUser.ColorCode,
                            Role = srm.AspNetUser.AspNetUserRoles.Select(r => new UserRole
                            {
                                Id = r.AspNetRole.Id,
                                Name = r.AspNetRole.Name
                            }).FirstOrDefault()
                        }
                    }).FirstOrDefault();
                return PartialView("_ServiceRequestMessage", data);
            }
        }
    }
}