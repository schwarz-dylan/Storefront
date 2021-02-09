using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Storefront.UI.MVC.Startup))]
namespace Storefront.UI.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
