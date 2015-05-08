using Boligf.Api.Domain.Events;
using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Api.Views.Association
{
	public class GetAssociationView : IViewInstance<InstancePerAggregateRootLocator>, 
		ISubscribeTo<AssociationCreated>,
		ISubscribeTo<AssociationNameUpdated>
	{
		public string Id { get; set; }
		public long LastGlobalSequenceNumber { get; set; }

		public string AssociationName { get; set; }

		public void Handle(IViewContext context, AssociationCreated domainEvent)
		{

		}

		public void Handle(IViewContext context, AssociationNameUpdated domainEvent)
		{
			AssociationName = domainEvent.Name;
		}
	}
}
