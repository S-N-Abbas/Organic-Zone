using System.Web;
using System.Web.Optimization;

namespace Organic_Zone
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-{version}-min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/MyScripts").Include(
                        "~/Scripts/jquery-{version}.min.js",
                        "~/Scripts/responsiveslides.min.js",
                        "~/Sctipts/owl.carousel.js",
                        "~/Scripts/minicart.min.js",
                        "~/Scripts/jquery.vide.min.js",
                        "~/Scripts/jquery.nicescroll.js",
                        "~/Scripts/scripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/MyCSS").Include(
                "~/Content/font-awesome.css",
                "~/Content/owl.carousel.css",
                "~/Content/style.css"));
        }
    }
}
