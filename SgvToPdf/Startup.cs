using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(SgvToPdf.Startup))]
namespace SgvToPdf
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
