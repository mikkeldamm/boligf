using d60.Cirqus.Views.ViewManagers;

namespace Boligf.DataAccess.ViewManagers
{
	public class DataViewManager<TViewInstance> : InMemoryViewManager<TViewInstance> where TViewInstance : class, IViewInstance, ISubscribeTo, new()
	{

	}
}
