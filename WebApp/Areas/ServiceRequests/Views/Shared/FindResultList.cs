using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace WebApp.Areas.ServiceRequests.Views.Shared
{
    public class FindResultList
    {
        private ImeHubDbContext db;
        public FindResultList() { }
        public FindResultList(Guid physicianId, string searchText, ImeHubDbContext db)
        {
            this.PhysicianId = physicianId;
            this.db = db;
            this.Claimants = db.ServiceRequests
                .Where(sr => sr.ClaimantName.Contains(searchText))
                .Select(c => new FindResult()
                {
                    Id = c.Id.ToString(),
                    ClaimantName = c.ClaimantName,
                    CompanyName = c.Service.Company.Name
                })
                .OrderBy(c => c.ClaimantName)
                .ToList();
        }

        public Guid PhysicianId { get; set; }
        public IEnumerable<FindResult> Claimants { get; }

        public class FindResult
        {
            public string Id { get; set; }
            public string ClaimantName { get; set; }
            public string CompanyName { get; set; }
        }
    }
}