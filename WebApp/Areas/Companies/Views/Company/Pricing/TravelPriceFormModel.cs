using Orvosi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class TravelPriceFormModel
    {
        public TravelPriceFormModel()
        {

        }
        public TravelPriceFormModel(Guid companyId, Guid companyServiceId, short cityId)
        {
            CompanyId = companyId;
            CompanyServiceId = companyServiceId;
            CityId = cityId;
        }
        public Guid CompanyId { get; set; }
        public short CityId { get; set; }
        public Guid CompanyServiceId { get; set; }
        public decimal Price { get; set; }
    }

    public class EditTravelPriceFormModel : TravelPriceFormModel
    {
        public EditTravelPriceFormModel()
        {

        }
        public EditTravelPriceFormModel(Guid companyId, Guid travelPriceId, OrvosiDbContext db)
        {
            Id = travelPriceId;
            CompanyId = companyId;

            var travelPrice = db.TravelPrices.Single(tp => tp.Id == travelPriceId);
            Price = travelPrice.Price;
        }

        public Guid Id { get; set; }
    }
}