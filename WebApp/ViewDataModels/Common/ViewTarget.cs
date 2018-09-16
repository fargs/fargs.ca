using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.ViewDataModels
{
    public enum ViewTarget
    {
        DaySheet,
        DueDates,
        Schedule,
        Additionals,
        Details,
        Modal
    }
    public static class ViewTargetExtensions
    {
        public const string Key = "viewtarget";
        public static ViewTarget ViewTarget_Get(this ViewDataDictionary viewData)
        {
            return (ViewTarget)viewData[Key];
        }
        public static void ViewTarget_Set(this ViewDataDictionary viewData, ViewTarget value)
        {
            viewData.Add(Key, value);
        }
    }
}