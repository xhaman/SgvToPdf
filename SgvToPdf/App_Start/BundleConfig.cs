using System.Web;
using System.Web.Optimization;

namespace SgvToPdf
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/popper").Include(
                       "~/Scripts/popper.min.js"));
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //        "~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-materialkit").Include(
"~/Scripts/MaterialKit/bootstrap-material-design.js"));

            bundles.Add(new ScriptBundle("~/bundles/materialkit").Include(
          "~/Scripts/MaterialKit/material-kit.js"));



            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/materialkit/css").
                Include("~/Content/MaterialKit/material-kit.css"));

          
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/site.css"));



        }
    }
}
