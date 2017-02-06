using System.Web;
using System.Web.Optimization;

namespace D
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.11.1.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/jquery-ui.css"
                                     ));

            bundles.Add(new ScriptBundle("~/bundles/jquery-ui").Include(
                        "~/Scripts/jquery-ui.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/myScripts").Include(
                       "~/Scripts/myScripts.js"                      
                       ));

            bundles.Add(new ScriptBundle("~/bundles/popupCreateGoods").Include(
                       
                       "~/Scripts/popupCreateGoods.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/popupCreateOrders").Include(

                       "~/Scripts/popupCreateOrders.js"
                       ));

            bundles.Add(new ScriptBundle("~/bundles/popupCreateMoney").Include(

                       "~/Scripts/popupCreateMoney.js"
                       ));
        }
    }
}
