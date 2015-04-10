using Boligf.Api.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Boligf.Api.Startup))]
namespace Boligf.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            app.UseCors(CorsOptions.AllowAll);

            WebApiConfig.Setup(app);
        }

        public void ConfigureAuth(IAppBuilder app)
        {
            AuthConfig.Setup(app);
        }
    }
}