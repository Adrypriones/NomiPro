using System.Web;
using System.Web.Optimization;

namespace NomiProProject
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Content/js/jquery-{version}.min.js",
                        "~/Content/js/jquery.mCustomScrollbar.concat.min.js",
                        "~/Content/js/bootstrap.min.js",
                        "~/Content/js/material.min.js",
                        "~/Content/js/ripples.min.js",
                        "~/Content/js/main.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/css/main.css",
                      "~/Content/css/bootstrap.min.css",
                      "~/Content/css/jquery.mCustomScrollbar.css"));
        }
    }
}
