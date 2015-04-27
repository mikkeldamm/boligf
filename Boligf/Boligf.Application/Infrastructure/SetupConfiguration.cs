using Boligf.DataAccess.ViewManagers;
using Boligf.Views.Association;
using d60.Cirqus;
using d60.Cirqus.MsSql.Config;
using d60.Cirqus.Views.ViewManagers;
using SimpleInjector;

namespace Boligf.Application.Infrastructure
{
	public static class SetupConfiguration
	{
		private static Container _container;

		// TODO: This is only for test purpose. All of this needs to be structured
		//       and setted up correctly. Use SimpleInjector to load Interface types
		//       from assembly to avoid setting up specific instances
		public static void Setup(Container container)
		{
			var getAllAssociationsView = new DataViewManager<GetAllAssociationsView>();
			var getSingleAssociationView = new DataViewManager<GetSingleAssociationsView>();

			const string msSqlConnection = @"Data Source=(localdb)\v11.0;Initial Catalog=BoligfTest;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
			const string msSqlEventStoreTableName = "events";

			var commandProcessor = CommandProcessor
				.With()
				.EventStore(e => e.UseSqlServer(msSqlConnection, msSqlEventStoreTableName))
				.EventDispatcher(ed => ed.UseViewManagerEventDispatcher(getAllAssociationsView, getSingleAssociationView))
				.Create();

			_container = container;

			_container.RegisterSingle<IViewManager<GetAllAssociationsView>>(getAllAssociationsView);
			_container.RegisterSingle<IViewManager<GetSingleAssociationsView>>(getSingleAssociationView);

			_container.RegisterSingle<ICommandProcessor>(commandProcessor);
		} 
	}
}
