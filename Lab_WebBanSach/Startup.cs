using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Lab_WebBanSach.Startup))]
namespace Lab_WebBanSach
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
