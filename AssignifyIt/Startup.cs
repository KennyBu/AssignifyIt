using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AssignifyIt.Startup))]
namespace AssignifyIt
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
