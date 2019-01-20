using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImeHub.Models;

namespace WebApp.Areas.Companies.Views.Company
{
    public class TravelPriceFormModel
    {
        public TravelPriceFormModel()
        {

        }
        public TravelPriceFormModel(Guid companyId, Guid serviceId, Guid cityId)
        {
            CompanyId = companyId;
            ServiceId = serviceId;
            CityId = cityId;
        }
        public Guid CompanyId { get; set; }
        public Guid CityId { get; set; }
        public Guid ServiceId { get; set; }
        public decimal Price { get; set; }
    }

    public class EditTravelPriceFormModel : TravelPriceFormModel
    {
        public EditTravelPriceFormModel()
        {

        }
        public EditTravelPriceFormModel(Guid companyId, Guid travelPriceId, ImeHubDbContext db)
        {
            Id = travelPriceId;
            CompanyId = companyId;

            var travelPrice = db.TravelPrices.Single(tp => tp.Id == travelPriceId);
            Price = travelPrice.Price;
        }

        public Guid Id { get; set; }
    }
}