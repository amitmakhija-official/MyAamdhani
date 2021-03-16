using System.Web;
using System.Web.Optimization;

namespace MyAamdhani
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/scriptval").Include(
                        "~/Scripts/scrip*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));


            // Scipts

            bundles.Add(new ScriptBundle("~/bundles/menu").Include(
                "~/Scripts/menu.js"));
            bundles.Add(new ScriptBundle("~/bundles/slick").Include(
                "~/Scripts/slick.js"));
            bundles.Add(new ScriptBundle("~/bundles/sidebar").Include(
                "~/Scripts/sidebar-menu.js"));
            bundles.Add(new ScriptBundle("~/bundles/parallax").Include(
                "~/Scripts/parallax.js"));
            bundles.Add(new ScriptBundle("~/bundles/proper").Include(
                "~/Scripts/proper.js"));
            bundles.Add(new ScriptBundle("~/bundles/price-range").Include(
                "~/Scripts/price-range.js"));
            bundles.Add(new ScriptBundle("~/bundles/snow").Include(
                "~/Scripts/snow.js"));
            bundles.Add(new ScriptBundle("~/bundles/zoom").Include(
                "~/Scripts/zoom-gallery.js", "~/Scripts/zoom-scripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/admin").Include(
                "~/Scripts/admin-customizer.js", "~/Scripts/admin-reports.js"));
            bundles.Add(new ScriptBundle("~/bundles/chat-menu").Include(
                "~/Scripts/chat-menu.js"));
            bundles.Add(new ScriptBundle("~/bundles/datepicker").Include(
                "~/Scripts/datepicker.js"));
            bundles.Add(new ScriptBundle("~/bundles/exit").Include(
                "~/Scripts/exit.js"));
            bundles.Add(new ScriptBundle("~/bundles/flychart").Include(
                "~/Scripts/fly-chart.js"));
            bundles.Add(new ScriptBundle("~/bundles/footer-reveal").Include(
                "~/Scripts/footer-reveal.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/height-equal").Include(
                "~/Scripts/height-equal.js"));
            bundles.Add(new ScriptBundle("~/bundles/imagesloaded.pkgd.min").Include(
                "~/Scripts/imagesloaded.pkgd.min.js"));
            bundles.Add(new ScriptBundle("~/bundles/isotope").Include(
                "~/Scripts/isotop.min.js"));
            //Chart
            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                "~/Scripts/chart/chartjs/chart.min.js", "~/Scripts/chart/chartist/chartist.js"));
            //LazyLoad

            bundles.Add(new ScriptBundle("~/bundles/lazysizes").Include(
                "~/Scripts/lazysizes.min.js"));

            //CopyCode
            bundles.Add(new ScriptBundle("~/bundles/copycode").Include(
                "~/Scripts/prism/prism.min.js", "~/Scripts/clipboard/clipboard.min.js", "~/Scripts/custom-card/custom-card.js"));
            //Feather icon js
            bundles.Add(new ScriptBundle("~/bundles/feather").Include(
                "~/Scripts/icons/feather-icon/feather.min.js", "~/Scripts/icons/feather-icon/feather-icon.js"));
            //counter
            bundles.Add(new ScriptBundle("~/bundles/counter").Include(
                "~/Scripts/counter/jquery.waypoints.min.js", "~/Scripts/counter/jquery.counterup.min.js", "~/Scripts/counter/counter-custom.js"));
            // peity chart js
            bundles.Add(new ScriptBundle("~/bundles/peity").Include(
                "~/Scripts/chart/peity-chart/peity.jquery.js"));
            // peity chart js
            bundles.Add(new ScriptBundle("~/bundles/sparkline").Include(
                "~/Scripts/chart/sparkline/sparkline.js"));
            // dashboard
            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                "~/Scripts/dashboard/default.js"));
            // admin-script
            bundles.Add(new ScriptBundle("~/bundles/admin-script").Include(
                "~/Scripts/admin-scripts.js"));
            //jquery-3.3.1.min.js
            bundles.Add(new ScriptBundle("~/bundles/jquery-3.3.1").Include(
                "~/Scripts/jquery-3.3.1.min.js", "~/Scripts/jquery-ui.min.js"));

            //Fly Cart UI
            bundles.Add(new ScriptBundle("~/bundles/flycart").Include(
               "~/Scripts/jquery-ui.min.js"));
            //popper
            bundles.Add(new ScriptBundle("~/bundles/popper").Include(
                "~/Scripts/popper.min.js"));
            //portfolio
            bundles.Add(new ScriptBundle("~/bundles/portfolio").Include(
                "~/Scripts/jquery.magnific-popup.js", "~/Scripts/zoom-gallery.js"));
            //exitintentjs
            bundles.Add(new ScriptBundle("~/bundles/exitintentjs").Include(
                "~/Scripts/jquery.exitintent.js", "~/Scripts/exit.js"));
            //bootstrap-notify.min.js
            bundles.Add(new ScriptBundle("~/bundles/bootstrap-notify").Include(
                "~/Scripts/bootstrap-notify.min.js"));
            //theme.js
            bundles.Add(new ScriptBundle("~/bundles/script").Include(
                "~/Scripts/script.js"));




            //CSS

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/fontawesome.css",
                "~/Content/icofont.css",
                "~/Content/flag-icon.css",
                "~/Content/bootstrap-datetimepicker.css"
                
            ));
            
            bundles.Add(new StyleBundle("~/bundles/admin").Include(
                "~/Content/admin.css"));

            //Chartist css
            bundles.Add(new StyleBundle("~/bundles/chartist").Include(
                "~/Content/chartist.css"));
            //prism css
            bundles.Add(new StyleBundle("~/bundles/prism").Include(
                "~/Content/prism.css"));
            //themify css
            bundles.Add(new StyleBundle("~/bundles/themify").Include(
                "~/Content/themify-icons.css"));
            //color1 css
            bundles.Add(new StyleBundle("~/bundles/color1").Include(
                "~/Content/color1.css"));
            //animate css
            bundles.Add(new StyleBundle("~/bundles/animate").Include(
                "~/Content/animate.css"));
            //Slick slider css
            bundles.Add(new StyleBundle("~/bundles/slick").Include(
                "~/Content/slick.css", "~/Content/slick-theme.css"));

        }
    }
}
