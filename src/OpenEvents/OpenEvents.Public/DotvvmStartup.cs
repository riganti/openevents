	using DotVVM.Framework.Configuration;
	using DotVVM.Framework.Controls.Bootstrap;
	using DotVVM.Framework.ResourceManagement;
using DotVVM.Framework.Routing;

namespace OpenEvents.Public
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
            config.RouteTable.Add("RegisterFree", "register/free/{Id}", "Views/RegisterFree.dothtml");
            config.RouteTable.Add("RegisterPaid", "register/paid/{Id}", "Views/RegisterPaid.dothtml");
        }

        private void ConfigureControls(DotvvmConfiguration config, string applicationPath)
        {
            // register code-only controls and markup controls
        }

        private void ConfigureResources(DotvvmConfiguration config, string applicationPath)
        {
            // register custom resources and adjust paths to the built-in resources
            config.Resources.Register("site-style", new StylesheetResource()
            {
                Location = new UrlResourceLocation("~/css/site.min.css"),
                Dependencies = new[] { "bootstrap" }
            });
        }
    }
}
