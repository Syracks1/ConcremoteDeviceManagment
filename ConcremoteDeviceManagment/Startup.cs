using Microsoft.Owin;
using Owin;
using System;

[assembly: OwinStartupAttribute(typeof(ConcremoteDeviceManagment.Startup))]
namespace ConcremoteDeviceManagment
{
    public partial class Startup
    {
        //[STAThread]
        //static void Main(IAppBuilder app)
        //{
        //    ConfigureAuth(app);
        //}
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}