using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImeHub.Models.Enums
{
    public static class Role
    {
        public static Guid Physician = new Guid("8359141f-e423-4e48-8925-4624ba86245a");
        public static string PhysicianName = "Physician";
        public static Guid Company = new Guid("7b930663-b091-44ca-924c-d8b11a1ee7ea");
        public static string CompanyName = "Company";
        public static Guid IntakeAssistant = new Guid("9dd582a0-cf86-4fc0-8894-477266068c12");
        public static string IntakeAssistantName = "Intake Assistant";
        public static Guid CaseCoordinator = new Guid("9eab89c0-225c-4027-9f42-cc35e5656b14");
        public static string CaseCoordinatorName = "Case Coordinator";
        public static Guid DocumentReviewer = new Guid("22B5C8AC-2C96-4A74-8057-976914031A7E");
        public static string DocumentReviewerName = "Document Reviewer";
        public static Guid SuperAdmin = new Guid("7fab67dd-286b-492f-865a-0cb0ce1261ce");
        public static string SuperAdminName = "Super Admin";
        public static Guid AppTester = new Guid("46f7c109-1a23-4969-bca6-3a937db912d5");
        public static string AppTesterName = "App Tester";
        public static Guid ExternalAdmin = new Guid("D4AA9AC8-396D-486D-9FF1-A9E9D5A8FAB7");
        public static string ExternalAdminName = "External Admin";
        public static Guid Manager = new Guid("5F8705D3-DFCD-4233-834D-C7DB1DFA99A8");
        public static string ManagerName = "Manager";

    }
}
