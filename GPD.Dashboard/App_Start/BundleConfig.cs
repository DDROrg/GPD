using System.Web;
using System.Web.Optimization;

namespace GPD.Dashboard
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/jquery.validate*",
                      "~/Scripts/d3.min.js",
                      "~/Scripts/c3.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                      "~/Scripts/angular.js",
                       "~/Scripts/angular-animate.js",
                      "~/Scripts/angular-toastr.tpls.min.js",
                      "~/Scripts/angular-ui/ui-bootstrap-tpls.js",
                      "~/Scripts/angular-ui-router.min.js",
                      "~/Scripts/angular-ui-router-title.js",
                      "~/Scripts/loading-bar.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom/script").Include(
                      "~/Scripts/custom/GPD.*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/loading-bar.min.css",
                      "~/Content/angular-toastr.min.css",
                      "~/Content/c3.min.css",
                      "~/Content/site.css"));
        }
    }
}
