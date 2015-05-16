using Boligf.Api;
using Boligf.Api.Configuration;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace Boligf.Api
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureIoC(app);
			ConfigureCors(app);
			ConfigureAuth(app);
			ConfigureWebApi(app);
		}

		private static void ConfigureIoC(IAppBuilder app)
		{
			IoCConfig.Setup(app);
		}

		private static void ConfigureCors(IAppBuilder app)
		{
			app.UseCors(CorsOptions.AllowAll);
		}

		private static void ConfigureAuth(IAppBuilder app)
		{
			AuthConfig.Setup(app);
		}

		private static void ConfigureWebApi(IAppBuilder app)
		{
			WebApiConfig.Setup(app);
		}
	}
}