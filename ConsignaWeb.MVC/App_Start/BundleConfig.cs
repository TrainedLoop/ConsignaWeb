using System.Web;
using System.Web.Optimization;

namespace ConsignaWeb.MVC
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.IgnoreList.Clear();
            bundles.Add(new StyleBundle("~/stylesheets/bootstrap").Include(
                "~/Css/jquery-ui/demo_page.css",
                "~/Css/jquery-ui/demo_table.css",
                "~/Css/jquery-ui/jquery-ui-1.10.3.custom.min.css",
                "~/Css/jquery-ui/custom.css",
                "~/Css/bootstrap.min.css",
                "~/Css/custom.css"));
            bundles.Add(new StyleBundle("~/stylesheets/tables").Include(
               "~/Css/demo_page.css",
               "~/Css/demo_table.css"
               ));
            bundles.Add(new ScriptBundle("~/scripts/bootstrap").Include(
                       "~/Scripts/JS/collapse.js",
                       "~/Scripts/JS/dropdown.js",
                       "~/Scripts/JS/jquery-1.10.2.min.js",
                       "~/Scripts/JS/jquery-ui-1.10.3.custom.min.js",
                       "~/Scripts/JS/jquery-ui-1.10.4.custom.min.js"));

            bundles.Add(new ScriptBundle("~/scripts/tables").Include(
           "~/Scripts/JS/jquery.dataTables.min.js"
           ));
        }
    }
}