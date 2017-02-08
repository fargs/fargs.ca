using System;
using System.Collections.Generic;
using System.Data;
using Dapper;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace Orvosi.Hangfire
{
    public class WorkJobs
    {
        string connectionString;
        public WorkJobs(string connectionString)
        {
            this.connectionString = connectionString;
        }
        public void UpdateAssessmentDayState()
        {
            using (IDbConnection dbConnection = new SqlConnection(connectionString))
            {
                dbConnection.Open();
                dbConnection.Execute("API.UpdateAssessmentDayTaskState @Now", new { Now = DateTime.UtcNow });
            }
        }
    }
}
