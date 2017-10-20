using System.Web;
using System.Web.Optimization;

namespace GPD.Dashboard
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
        //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
        //          "~/Scripts/jquery-{version}.js",
        //          "~/Scripts/jquery.validate*",
        //          "~/Scripts/d3.min.js",
        //          "~/Scripts/c3.min.js"));

        // Use the development version of Modernizr to develop with and learn from. Then, when you're
        // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
        //            "~/Scripts/modernizr-*"));

        //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
        //          "~/Scripts/bootstrap.js",
        //          "~/Scripts/respond.js"));


        

            bundles.Add(new ScriptBundle("~/bundles/angular/js").Include(
                      "~/Content/angular/js/angular.js",
                      "~/Content/angular/js/angular-animate.js",
                      "~/Content/angular/js/angular-toastr.tpls.min.js",
                      "~/Content/angular-ui/js/ui-bootstrap-tpls.js",
                      "~/Content/angular/js/angular-ui-router.min.js",
                      "~/Content/angular/js/angular-ui-router-title.js",
                      "~/Content/angular/js/loading-bar.min.js",
                      "~/Content/c3/js/d3.min.js",
                      "~/Content/c3/js/c3.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom/js").Include(
                      "~/Content/custom/js/GPD.*"));

            bundles.Add(new StyleBundle("~/bundles/angular/css").Include(
                      "~/Content/angular/css/loading-bar.min.css",
                      "~/Content/angular/css/angular-toastr.min.css",
                      "~/Content/c3/css/c3.min.css"));

            bundles.Add(new StyleBundle("~/bundles/custom/css").Include(
                     "~/Content/custom/css/Site.css"));
        }
    }
}
