using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Shoe.Startup))]
namespace Shoe
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
