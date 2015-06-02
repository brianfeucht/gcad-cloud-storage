using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Photo.Startup))]
namespace Photo
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
