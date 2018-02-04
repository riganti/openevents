using DotVVM.Framework.Configuration;
using DotVVM.Framework.Controls.Bootstrap;
using DotVVM.Framework.ResourceManagement;
using DotVVM.Framework.Routing;

namespace OpenEvents.Admin
{
    public class DotvvmStartup : IDotvvmStartup
    {
        // For more information about this class, visit https://dotvvm.com/docs/tutorials/basics-project-structure
        public void Configure(DotvvmConfiguration config, string applicationPath)
        {
            config.AddBootstrapConfiguration(new DotvvmBootstrapOptions()
            {
                BootstrapCssUrl = "~/lib/bootstrap/dist/css/bootstrap.min.css",
                BootstrapJsUrl = "~/lib/bootstrap/dist/js/bootstrap.min.js",
                JQueryUrl = "~/lib/jquery/dist/jquery.min.js"
            });

            ConfigureRoutes(config, applicationPath);
            ConfigureControls(config, applicationPath);
            ConfigureResources(config, applicationPath);
        }

        private void ConfigureRoutes(DotvvmConfiguration config, string applicationPath)
        {
            config.RouteTable.Add("Default", "", "Views/default.dothtml");

            config.RouteTable.Add("EventList", "events", "Views/EventList.dothtml");
            config.RouteTable.Add("EventDetail", "event/{id?}", "Views/EventDetail.dothtml");

            config.RouteTable.Add("OrderList", "orders", "Views/OrderList.dothtml");
            config.RouteTable.Add("OrderDetail", "order/{id?}", "Views/OrderDetail.dothtml");

            config.RouteTable.Add("MailTemplateList", "mailtemplates", "Views/MailTemplateList.dothtml");
            config.RouteTable.Add("MailTemplateDetail", "mailtemplate/{id?}", "Views/MailTemplateDetail.dothtml");
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            // register code-only controls and markup controls
            config.Markup.AddMarkupControl("cc", "SidebarMenu", "Controls/SidebarMenu.dotcontrol");
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            // register custom resources and adjust paths to the built-in resources
            config.Resources.Register("site-style", new StylesheetResource()
            {
                Location = new UrlResourceLocation("~/css/site.min.css"),
                Dependencies = new [] { "bootstrap" }
            });
        }
    }
}
