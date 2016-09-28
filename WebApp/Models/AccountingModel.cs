using data = Orvosi.Data;
using Orvosi.Shared.Enums;
using Orvosi.Shared.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Models.AccountingModel
{
    public class Mapper
    {
        public static DayFolder MapToToday(Guid serviceProviderId, DateTime now)
        {
            using (var context = new data.OrvosiDbContext())
            {
                var source = context.ServiceRequests
                    .Where(d => d.AppointmentDate.HasValue
                        && d.ServiceRequestTasks.Any(srt => srt.AssignedTo == serviceProviderId)
                        && !d.IsClosed)
                    .Select(sr => new ServiceRequest
                    {
                        Id = sr.Id,
                        ClaimantName = sr.ClaimantName,
                        DueDate = sr.DueDate,
                        AppointmentDate = sr.AppointmentDate,
                        Now = now,
                        StartTime = sr.StartTime,
                        CancelledDate = sr.CancelledDate,
                        IsClosed = sr.IsClosed,
                        BoxCaseFolderId = sr.BoxCaseFolderId,
                        Service = new Service
                        {
                            Id = sr.Service.Id,
                            Name = sr.Service.Name,
                            Code = sr.Service.Code
                        },
                        Company = new Company
                        {
                            Id = sr.Company.Id,
                            Name = sr.Company.Name
                        },
                        Address = new Address
                        {
                            Id = sr.Address.Id,
                            Name = sr.Address.Name,
                            City = sr.Address.City_CityId.Name
                        }
                        //,
                        //InvoiceDetails = sr.InvoiceDetails.Select(id => new InvoiceDetail
                        //{
                        //    Description = id.Description,
                        //    Amount = id.Amount,
                        //    Discount = id.Discount,
                        //    Total = id.Total
                        //})
                    }).ToList();

                return source // this filters out the days
                    .Where(s => s.AppointmentDate == now.Date)
                    .GroupBy(d => new { d.AppointmentDate, d.Address, d.Company })
                    .Select(d => new DayFolder
                    {
                        Day = d.Key.AppointmentDate.Value,
                        Company = d.Key.Company,
                        Address = d.Key.Address,
                        ServiceRequests = source.Where(s => s.AppointmentDate == d.Key.AppointmentDate.Value)
                    }).FirstOrDefault();
            }
        }
    }
}