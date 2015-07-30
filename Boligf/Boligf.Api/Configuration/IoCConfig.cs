using System.Collections.Generic;
using System.Web.Http;
using Boligf.Api.Context;
using Boligf.Api.Domain.Entities;
using Boligf.Api.Infrastructure;
using Boligf.Api.Providers;
using Boligf.Api.Views.Association;
using Boligf.Api.Views.Association.Address;
using Boligf.Api.Views.User;
using d60.Cirqus;
using d60.Cirqus.MongoDb.Config;
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
		private static readonly List<IViewManager> Views = new List<IViewManager>();
		private static Container _container;

		public static void Setup(IAppBuilder app)
		{
			_container = new Container();

			SetupCirqusConfiguration(_container);
			SetupUserStuff(_container);
			SetupProviders(_container);

			_container.RegisterWebApiControllers(GlobalConfiguration.Configuration);
			_container.Verify();

			GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(_container);

			IoCContainer.Container = _container;
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
			RegisterSimpleViewInstance<GetAssociationView>();
			RegisterSimpleViewInstance<GetAssociationsView>();
			RegisterSimpleViewInstance<GetAssociationUserView>();
			RegisterSimpleViewInstance<GetUsersAssociationsView>();
			RegisterSimpleViewInstance<GetAddressesView>();
			RegisterSimpleViewInstance<GetUserView>();
			RegisterSimpleViewInstance<GetUsersView>();

			var commandProcessor = CommandProcessor
				.With()
				.EventStore(e => e.UseMongoDb(Connection.DataMongoDbConnection, "eventData"))
				.EventDispatcher(ed => ed.UseViewManagerEventDispatcher(Views.ToArray()))
				.Create();

			container.RegisterSingle<ICommandProcessor>(commandProcessor);
		}

		private static void RegisterSimpleViewInstance<TView>() where TView : class, IViewInstance, ISubscribeTo, new()
		{
			var viewInstance = new InMemoryViewManager<TView>();

			Views.Add(viewInstance);

			_container.RegisterSingle<IViewManager<TView>>(viewInstance);
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