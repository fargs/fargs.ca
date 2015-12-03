using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Enums
{
    public static class Roles
    {
        public const string Physician = "8359141f-e423-4e48-8925-4624ba86245a";
        public const string Company = "7b930663-b091-44ca-924c-d8b11a1ee7ea";
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
}