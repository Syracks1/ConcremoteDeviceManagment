using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ConcremoteDeviceManagment.Startup))]
namespace ConcremoteDeviceManagment
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
