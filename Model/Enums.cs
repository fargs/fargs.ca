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
        public const string Company = "7b930663-b091-44ca-924c-d8b11a1ee7ea";
        public const string IntakeAssistant = "9dd582a0-cf86-4fc0-8894-477266068c12";
        public const string SuperAdmin = "7fab67dd-286b-492f-865a-0cb0ce1261ce";
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
        public const byte Unassigned = 0;
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
                    range[0] = DateTime.UtcNow;
                    range[1] = DateTime.UtcNow.AddDays(1);
                    break;
                case DateRanges.Next10Days:
                    range[0] = DateTime.UtcNow;
                    range[1] = DateTime.UtcNow.AddDays(10);
                    break;
                default:
                    break;
            }
            return range;
        }
    }
}