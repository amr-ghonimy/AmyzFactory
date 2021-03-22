using System.Web;
using System.Web.Optimization;

namespace AmyzFactory
{
    public class BundleConfig
    {
         public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                      "~/Scripts/jquery.validate*"));



            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));
 
            bundles.Add(new ScriptBundle("~/bundles/my_scripts").Include(
                       "~/Content/dialogs.js",
                       "~/Content/images.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-2.6.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery_user").Include(
              "~/Scripts/jquery-1.10.2.js",
              "~/Scripts/js/jquery.validate.js",
              "~/Scripts/js/jquery.validate.unobtrusive.js",
              "~/Scripts/js/bootstrap.js",
              "~/Scripts/jquery.unobtrusive-ajax.js"));






            bundles.Add(new StyleBundle("~/bundles/css_user").Include(
                  "~/Content/css/style.css",
                  "~/Content/css/responsive.css",
                  "~/Content/css/custom.css",
                  "~/Content/css/style-slide-profile.css"));


 


                  bundles.Add(new StyleBundle("~/bundles/css_admin").Include(
                      "~/Content/css/sb-admin-2.css",
                     "~/Content/dataTable/css/jquery.dataTables.css"
                     ));


 


                     bundles.Add(new ScriptBundle("~/bundles/jquery_admin").Include(
                       "~/Areas/Admin/Content/js/jquery-2.1.1.js",
                       "~/Areas/Admin/Content/vendor/bootstrap/js/bootstrap.bundle.min.js",
                       "~/Areas/Admin/Content/js/sb-admin-2.min.js",
                       "~/Scripts/jquery.unobtrusive-ajax.js"));


            bundles.Add(new ScriptBundle("~/bundles/data_table_script").Include(
                     "~/Content/dataTable/jquery.dataTables.js"));

            bundles.Add(new StyleBundle("~/bundles/data_table_css").Include(
                    "~/Content/dataTable/css/jquery.dataTables.css"
                   ));

            bundles.Add(new ScriptBundle("~/bundles/alert_script").Include(
                      "~/Areas/Admin/Content/js/dialogs.js"));


            bundles.Add(new ScriptBundle("~/bundles/images_script").Include(
                    "~/Areas/Admin/Content/js/images.js"));


        }
    }
}
