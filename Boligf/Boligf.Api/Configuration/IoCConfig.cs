using System.Web.Http;
using Boligf.Api.Context;
using Boligf.Api.Domain.Entities;
using Boligf.Api.Infrastructure;
using Boligf.Api.Providers;
using Boligf.Api.Views.Association;
using d60.Cirqus;
using d60.Cirqus.MsSql.Config;
using d60.Cirqus.Views;
using d60.Cirqus.Views.ViewManagers;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
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

			SetupCirqusConfiguration(container);
			SetupUserStuff(container);
			SetupProviders(container);

			container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
			container.Verify();

			GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

			IoCContainer.Container = container;
		}

		private static void SetupUserStuff(Container container)
		{
			var userContext = new UserContext();

			container.RegisterSingle<UserContext>(() => userContext);
			container.RegisterSingle<UserStore<UserIdentity>>(() => new UserStore<UserIdentity>(userContext));
			container.RegisterSingle<IUserManager, UserManager>();
		}

		private static void SetupProviders(Container container)
		{
			container.RegisterSingle<IOAuthAuthorizationServerProvider, AuthorizationServerProvider>();
			container.RegisterSingle<IGoogleOAuth2AuthenticationProvider, GoogleAuthorizationProvider>();
			container.RegisterSingle<IFacebookAuthenticationProvider, FacebookAuthorizationProvider>();

			container.RegisterSingle<OAuthBearerAuthenticationOptions, OAuthBearerAuthenticationOptions>();
			container.RegisterSingle<OAuthAuthorizationServerOptions, AuthorizationServerOptions>();
			container.RegisterSingle<GoogleOAuth2AuthenticationOptions, GoogleOAuthOptions>();
			container.RegisterSingle<FacebookAuthenticationOptions, FacebookOAuthOptions>();
		}

		private static void SetupCirqusConfiguration(Container container)
		{
			var getAssociationView = new InMemoryViewManager<GetAssociationView>();
			var getUserAssociationsView = new InMemoryViewManager<GetUserAssociationsView>();

			var views = new IViewManager[] { getAssociationView, getUserAssociationsView };

			var commandProcessor = CommandProcessor
				.With()
				.EventStore(e => e.UseSqlServer(Connection.DataConnectionName, "test2events"))
				.EventDispatcher(ed => ed.UseViewManagerEventDispatcher(views))
				.Create();

			container.RegisterSingle<IViewManager<GetAssociationView>>(getAssociationView);
			container.RegisterSingle<IViewManager<GetUserAssociationsView>>(getUserAssociationsView);

			container.RegisterSingle<ICommandProcessor>(commandProcessor);
		}
	}

	public static class IoCContainer
	{
		public static Container Container { get; set; }

		public static T Resolve<T>() where T : class
		{
			return Container.GetInstance<T>();
		}
	}
}