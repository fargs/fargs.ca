﻿using System.Web;
using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                     "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryfileupload").Include(
                        "~/Scripts/jquery.ui.widget.js",
                        "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
                        "~/Scripts/jQuery.FileUpload/jquery.fileupload.js"));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                      "~/Scripts/knockout-3.3.0.js",
                      "~/Scripts/knockout.mapping-latest.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                      "~/Scripts/moment.min.js",
                      "~/Scripts/fullcalendar.min.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap-custom/bootstrap.css",
                      "~/Content/toastr.min.css",
                      "~/Content/jQuery.FileUpload/css/jquery.fileupload.css",
                      "~/Content/fullcalendar.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Content/landing").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/landing-page.css"));
        }
    }
}
