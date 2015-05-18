using System.Web.Http;
using Boligf.Api.Views.Association;
using d60.Cirqus;
using d60.Cirqus.MsSql.Config;
using d60.Cirqus.Views.ViewManagers;
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

			SetupConfiguration(container);

			container.RegisterWebApiControllers(GlobalConfiguration.Configuration);

			container.Verify();

			GlobalConfiguration.Configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
		}

		private static void SetupConfiguration(Container container)
		{
			var getAllAssociationsView = new InMemoryViewManager<GetAllAssociationsView>();
			var getSingleAssociationView = new InMemoryViewManager<GetSingleAssociationsView>();

			const string msSqlConnection = @"Data Source=(localdb)\v11.0;Initial Catalog=BoligfTest;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
			const string msSqlEventStoreTableName = "events";

			var commandProcessor = CommandProcessor
				.With()
				.EventStore(e => e.UseSqlServer(msSqlConnection, msSqlEventStoreTableName))
				.EventDispatcher(ed => ed.UseViewManagerEventDispatcher(getAllAssociationsView, getSingleAssociationView))
				.Create();

			container.RegisterSingle<IViewManager<GetAllAssociationsView>>(getAllAssociationsView);
			container.RegisterSingle<IViewManager<GetSingleAssociationsView>>(getSingleAssociationView);

			container.RegisterSingle<ICommandProcessor>(commandProcessor);
		}
	}
}