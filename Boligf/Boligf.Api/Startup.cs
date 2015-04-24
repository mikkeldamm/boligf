using Boligf.Api.Configuration;
using Boligf.Application;
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
            ConfigureCors(app);
            ConfigureWebApi(app);
	        ConfigureIoC(app);
	        ConfigureDomain(app);
        }

	    private static void ConfigureAuth(IAppBuilder app)
        {
            AuthConfig.Setup(app);
        }

		private static void ConfigureCors(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
		}

		private static void ConfigureWebApi(IAppBuilder app)
		{
			WebApiConfig.Setup(app);
		}

		private static void ConfigureIoC(IAppBuilder app)
		{
			IoCConfig.Setup(app);
		}

		private static void ConfigureDomain(IAppBuilder app)
		{
			ProcessorConfiguration.Setup().Wait();
		}
    }
}