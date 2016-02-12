using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Model.Enums
{
    public static class Roles
    {
        public const string Physician = "8359141f-e423-4e48-8925-4624ba86245a";
        public const string PhysicianName = "Physician";
        public const string Company = "7b930663-b091-44ca-924c-d8b11a1ee7ea";
        public const string CompanyName = "Company";
        public const string IntakeAssistant = "9dd582a0-cf86-4fc0-8894-477266068c12";
        public const string IntakeAssistantName = "Intake Assistant";
        public const string CaseCoordinator = "9eab89c0-225c-4027-9f42-cc35e5656b14";
        public const string CaseCoordinatorName = "Case Coordinator";
        public const string DocumentReviewer = "22B5C8AC-2C96-4A74-8057-976914031A7E";
        public const string DocumentReviewerName = "Document Reviewer";
        public const string SuperAdmin = "7fab67dd-286b-492f-865a-0cb0ce1261ce";
        public const string SuperAdminName = "Super Admin";
    }

    public static class ActionStates
    {
        public const byte Expection = 0;
        public const byte Saved = 1;
        public const byte HasErrors = 2;
        public const byte HasWarnings = 3;
    }

    public static class RoleCategory
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

    public static class TaskStatuses
    {
        public const byte ToDo = 14;
        public const byte InProgress = 15;
        public const byte Done = 16;
        public const byte Obsolete = 40;
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

    public static class ServiceCategories
    {
        public const byte IndependentMedicalExam = 5;
        public const byte MedicalConsultation = 6;
        public const byte AddOn = 7;
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
        public const byte Completed = 36;
    }

    public static class Tasks
    {
        public const byte CreateCaseFolder = 16;
        public const byte SubmitInvoice = 24;
    }

    public static class Timeline
    {
        public const byte Past = 37;
        public const byte Present = 38;
        public const byte Future = 39;
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
}