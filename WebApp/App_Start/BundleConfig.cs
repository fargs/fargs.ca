using System.Web;
using System.Web.Optimization;

namespace WebApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js",
                        "~/Scripts/lodash.min.js",
                        "~/Scripts/select2.min.js",
                        "~/Scripts/list.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                     "~/Scripts/jquery.validate*",
                     "~/Scripts/jquery.validate.unobtrusive.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryfileupload").Include(
                        "~/Scripts/jquery.ui.widget.js",
                        "~/Scripts/jQuery.FileUpload/jquery.iframe-transport.js",
                        "~/Scripts/jQuery.FileUpload/jquery.fileupload.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-1.12.1.custom/jquery-ui.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/markdown").Include(
                        "~/Scripts/MarkdownDeepLib.min.js"));

            bundles.Add(new StyleBundle("~/Bundles/Content/markdown").Include(
                      "~/Scripts/mdd_styles.css",
                      new CssRewriteUrlTransform()));

            bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
                      "~/Scripts/knockout-3.3.0.js",
                      "~/Scripts/knockout.mapping-latest.js"));

            bundles.Add(new ScriptBundle("~/bundles/signalr").Include(
                       "~/Scripts/jquery.signalR-2.2.1.min.js", 
                       "~/signalr/hubs"));

            bundles.Add(new ScriptBundle("~/bundles/highcharts").Include(
                      "~/Scripts/highcharts/4.2.0/highcharts.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/bootstrap-datepicker.min.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/toastr.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/fullcalendar").Include(
                      "~/Scripts/moment.min.js",
                      "~/Scripts/fullcalendar.min.js"));

            bundles.Add(new StyleBundle("~/Bundles/Content/css").Include(
                      "~/Content/theme.min.css",
                      "~/Content/bootstrap-custom/bootstrap.min.css",
                      "~/Content/toastr.min.css",
                      "~/Scripts/jquery-ui-1.12.1.custom/jquery-ui.min.css",
                      "~/Content/jQuery.FileUpload/css/jquery.fileupload.css",
                      "~/Content/bootstrap-datepicker/bootstrap-datepicker.min.css",
                      "~/Content/css/select2.min.css",
                      "~/Content/site.css"));

            //bundles.Add(new StyleBundle("~/Content/landing").Include(
            //          "~/Content/bootstrap.css",
            //          "~/Content/landing-page.css"));
        }
    }
}
