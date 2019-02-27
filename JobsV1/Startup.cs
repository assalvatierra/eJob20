using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JobsV1.Startup))]
namespace JobsV1
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
