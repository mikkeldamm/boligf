using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Api.Play
{
	public static class ViewManagerExtensions
	{
		public static TView LoadFully<TView>(this IViewManager<TView> viewInstance) where TView : IViewInstance
		{
			return viewInstance.Load(GlobalInstanceLocator.GetViewInstanceId());
		}
	}
}