using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Orvosi.Shared.Enums
{
    public static class DueDateTypes
    {
        public const string AppointmentDate = "A";
        public const string ReportDueDate = "R";
        public const string None = "N";
    }

    public static class AspNetRoles
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
    }

    public static class ActionStates
    {
        public const byte Expection = 0;
        public const byte Saved = 1;
        public const byte HasErrors = 2;
        public const byte HasWarnings = 3;
    }

    public static class Actions
    {
        public const byte None = 0;
        public const byte Added = 1;
        public const byte Updated = 2;
        public const byte Deleted = 3;
    }

    public static class AddressTypes
    {
        public const byte CompanyAssessmentOffice = 1;
        public const byte PhysicianClinic = 2;
        public const byte PrimaryOffice = 3;
        public const byte BillingAddress = 4;
    }
    public static class RoleCategories
    {
        public const byte Physician = 1;
        public const byte Company = 2;
        public const byte Staff = 3;
        public const byte Admin = 4;
    }

    public static class ParentCompanies
    {
        public const byte Examworks = 1;
        public const byte SCM = 10;
    }

    public static class ServicePortfolios
    {
        public const byte Orvosi = 1;
        public const byte Physician = 2;
    }

    public static class ServiceRequestStatus
    {
        public const byte Open = 10;
        public const byte Closed = 11;
    }

    public static class EntityTypes
    {
        public const string Company = "COMPANY";
        public const string Physician = "PHYSICIAN";
        public const string User = "USER";
        public const string Service = "SERVICE";
    }

    public static class Lookups
    {
        public const byte RelationshipStatus = 1;
        public const byte ServiceRequestStatus = 2;
        public const byte TaskStatus = 3;
        public const byte LocationAreas = 4;
        public const byte PhysicianLocationStatus = 5;
        public const byte Specialties = 6;
    }

    public static class DocumentTemplates
    {
        public const byte CV = 1;
        public const byte CSPO = 2;
        public const byte CMPA = 3;
    }

    public static class Services
    {
        public const byte PaperReview = 16;
        public const byte Addendum = 17;
        public const byte TeleConference = 13;
    }

    public static class ServiceCategories
    {
        public const short IndependentMedicalExam = 5;
        public const short MedicalConsultation = 6;
        public const short AddOn = 7;
    }

    public static class PhysicianLocationStatus
    {
        public const byte Primary = 24;
        public const byte WillingToTravel = 25;
        public const byte UnwillingToTravel = 26;
    }

    public static class RelationshipStatuses
    {
        public const byte Interested = 1;
        public const byte NotInterested = 2;
        public const byte InProgress = 3;
        public const byte Active = 4;

    }

    public static class Specialties
    {
        public const byte Physiatry = 27;
        public const byte Orthopedics = 28;
        public const byte Neurology = 29;
    }

    public static class ServiceStatus
    {
        public const byte LateCancellation = 32;
        public const byte Cancellation = 31;
        public const byte NoShow = 30;
        public const byte Complete = 36;
        public const byte Active = 48;

    }

    public static class Tasks
    {
        public const byte CreateCaseFolder = 16;
        public const byte IntakeInterview = 12;
        public const byte SubmitInvoice = 24;
        public const byte RespondToQAComments = 21;
        public const byte ObtainFinalReportCompany = 28;
        public const byte CloseCase = 30;
        public const byte CloseAddOn = 131;
        public const byte SubmitReport = 19;
        public const byte SaveMedBrief = 18;
        public const byte AssessmentDay = 133;
        public const byte ApproveReport = 9;
        public const byte AdditionalEdits = 132;
        public const byte PaymentReceived = 134;
    }

    //public static class TaskStatuses
    //{
    //    public const byte ToDo = 14;
    //    public const byte InProgress = 15;
    //    public const byte Waiting = 16;
    //    public const byte Done = 47;
    //    public const byte Obsolete = 40;
    //}

    public static class TaskStatuses
    {
        public const byte Waiting = 1;
        public const byte ToDo = 2;
        public const byte Done = 3;
        public const byte Obsolete = 4;
    }

    public static class Timeline
    {
        public const byte Past = 37;
        public const byte Present = 38;
        public const byte Future = 39;
    }

    public static class DiscountTypes
    {
        public const byte NoShow = 1;
        public const byte LateCancellation = 2;
    }

    public static class Workload
    {
        public const byte Low = 1;
        public const byte High = 10;
    }

    public static class DateRanges
    {
        public const byte Today = 1;
        public const byte ThisWeek = 2;
        public const byte LastWeek = 3;
        public const byte ThisMonth = 4;
        public const byte LastMonth = 5;
        public const byte Next10Days = 6;
        public const byte Next10Weeks = 7;
        public const byte Next20Weeks = 8;
        public const byte Last10Days = 9;

        public static DateTime[] GetRange(byte RangeType)
        {
            DateTime[] range = new DateTime[2];
            switch (RangeType)
            {
                case DateRanges.Today:
                    range[0] = DateTime.Today.Date;
                    range[1] = DateTime.Today.AddDays(1).Date;
                    break;
                case DateRanges.Next10Days:
                    range[0] = DateTime.Today.Date;
                    range[1] = DateTime.Today.AddDays(10).Date;
                    break;
                default:
                    break;
            }
            return range;
        }

        public static Dictionary<int, string> GetMonths()
        {
            var months = new Dictionary<int, string>();
            months.Add(1, "Jan");
            months.Add(2, "Feb");
            months.Add(3, "Mar");
            months.Add(4, "Apr");
            months.Add(5, "May");
            months.Add(6, "Jun");
            months.Add(7, "Jul");
            months.Add(8, "Aug");
            months.Add(9, "Sep");
            months.Add(10, "Oct");
            months.Add(11, "Nov");
            months.Add(12, "Dec");
            return months;
        }

        public static Dictionary<int, string> GetYears()
        {
            var months = new Dictionary<int, string>();
            months.Add(2015, "2015");
            months.Add(2016, "2016");
            months.Add(2017, "2017");
            months.Add(2018, "2018");
            months.Add(2019, "2019");
            months.Add(2020, "2020");
            return months;
        }
    }

    public static class TimeZones
    {
        public const string EasternStandardTime = "Eastern Standard Time";
        public const string PacificStandardTime = "Pacific Standard Time";
    }
}