using System.Web.Optimization;

namespace QuickspatchWeb.App_Start
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
              "~/Scripts/jquery-{version}.js",
             "~/Scripts/jquery.attrchange.js",
             "~/Scripts/jquery.AreYouSure.js",
             "~/Scripts/jquery.columnview.js",
             "~/Scripts/jquery.attrchange.js")
             );
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jquerybase").Include(
                        "~/Scripts/jquery-2.0.3.js",
                        "~/Scripts/angularApp/jquery.cookie.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/angularApp/respond.min.js",
                        "~/Scripts/Site.js",
                        "~/Scripts/angularApp/jquery.cookie.js",
                        "~/Scripts/angularApp/kendo.all.js",
                        "~/Scripts/jszip.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                        "~/Scripts/angularApp/kendo.all.min.js",
                        "~/Scripts/angularApp/angular.min.js",
                         "~/Scripts/angularApp/angular-route.min.js",
                          "~/Scripts/angularApp/angular-resource.min.js",
                           "~/Scripts/angularApp/angular-animate.min.js",
                            "~/Scripts/angularApp/angular-sanitize.js",
                            "~/Scripts/angularApp/ui-bootstrap.min.js",
                            "~/Scripts/angularApp/ui-bootstrap-tpls.min.js",
                             "~/Scripts/angularApp/angular-kendo.min.js"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css/layout").Include(
                        "~/Content/base.css",
                        "~/Content/grid.css"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                        "~/Content/css/bootstrap.min.css",
                        "~/Content/css/font-awesome.min.css",
                        "~/Content/css/kendo/kendo.common.min.css",
                        "~/Content/css/kendo/kendo.bootstrap.min.css",
                        "~/Content/css/kendo/kendo.dataviz.min.css",
                        "~/Content/css/kendo/kendo.dataviz.bootstrap.min.css",
                        "~/Content/css/kendo/kendo.mobile.all.min.css",
                        "~/Content/css/smartadmin-production-plugins.min.css",
                        "~/Content/css/smartadmin-production.min.css",
                        "~/Content/css/smartadmin-skins.min.css",
                        "~/Content/angular/css/toastr.css",
                        "~/Content/animation.css",
                         "~/Content/css/smartadmin-rtl.min.css",
                          "~/Content/css/demo.min.css",
                           "~/Content/css/mixins.css"));
            BundleTable.EnableOptimizations = true;
        }
    }
}