using System.Web.Http;
using Owin;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;

namespace Boligf.Api.Configuration
{
	public class IoCConfig
	{
		public static void Setup(IAppBuilder app)
		{
			var container = new Container();

			Application.Infrastructure.SetupConfiguration.Setup(container);

			container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

			container.Verify();

			GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
		}
	}
}