using Microsoft.VisualStudio.TestTools.UnitTesting;
using ImeHub.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Data.Tests
{
    [TestClass()]
    public class ServiceRequestFiltersTests
    {
        [TestMethod()]
        public void CanAccessTest()
        {
            using (var context = new ImeHubDbContext())
            {
                var userId = Guid.NewGuid();
                var roleId = Guid.NewGuid();
                var physicianId = Guid.NewGuid();

                var serviceRequests = context.ServiceRequests.CanAccess(userId, physicianId, roleId);

                Assert.AreEqual(serviceRequests.Count(), 1);
            }
        }
    }
}