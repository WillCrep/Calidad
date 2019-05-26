using System.Web;
using System.Web.Optimization;

namespace Presentacion
{
    public class BundleConfig
    {
        // Para obtener más información sobre las uniones, visite https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include("~/Content/css/bootstrap.min.css", "~/Content/css/sweetalert2.css"));
            bundles.Add(new ScriptBundle("~/bundles/js").Include("~/Content/Scripts/jquery-3.3.1.min.js", "~/Content/Scripts/bootstrap.min.js",
                "~/Content/Scripts/jquery.validate.unobtrusive.js",
                "~/Content/Scripts/jquery.validate.js",
                "~/Content/Scripts/jquery.validate.unobtrusive.min.js",
                "~/Content/Scripts/sweetalert.min.js",
                "~/Content/Scripts/SweetAlert.js",
                "~/Content/Scripts/sweetalert2.js",
                "~/Content/Scripts/Funciones.js"));
        }
    }
}
