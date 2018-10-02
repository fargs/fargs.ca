using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using WebApp.Library.Extensions;

namespace WebApp.Areas.Work.Views.Schedule
{
    public class DayViewModel
    {
        public DayViewModel()
        {

        }
        public long Id { get; set; }
        public string DayName { get; set; }
        public DateTime Day { get; set; }

        public IEnumerable<string> Addresses { get; set; }
        public IEnumerable<string> Companies { get; set; }

        public IEnumerable<AppointmentViewModel> Appointments { get; set; }

        public static Func<IGrouping<DateTime, AppointmentViewModel>, DayViewModel> FromServiceRequestDtoGrouping = dto => dto == null ? null : new DayViewModel
        {
            Day = dto.Key,
            DayName = dto.Key.ToOrvosiLongDateFormat(),
            Companies = dto.Select(sr => sr.Company == null ? "No company" : sr.Company.Name).Distinct(),
            Addresses = dto.Select(sr => sr.Address == null ? "No address" : sr.Address.City).Distinct().ToArray(),
            Appointments = dto
        };
    }
}