using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.ViewModels;

namespace WebApp.ViewDataModels
{
    public class CaseLinkArgs
    {
        public int ServiceRequestId { get; set; }
        public ViewTarget ViewTarget { get; set; }
    }

    public static class CaseLinkArgsExtensions
    {
        public const string Key = "caseViewArgs";
        public static CaseLinkArgs CaseViewArgs_Get(this ViewDataDictionary viewData)
        {
            return viewData[Key] as CaseLinkArgs;
        }
        public static void CaseViewArgs_Set(this ViewDataDictionary viewData, CaseLinkArgs value)
        {
            viewData.Add(Key, value);
        }
    }
}