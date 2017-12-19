using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CSharpBotAdmin.Startup))]
namespace CSharpBotAdmin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
