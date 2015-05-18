using System.Collections.Generic;
using Boligf.Api.Domain.Events;
using d60.Cirqus.Extensions;
using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Api.Views.Association
{
	public class GetSingleAssociationsView : IViewInstance<GlobalInstanceLocator>, ISubscribeTo<AssociationCreated>
	{
		public string Id { get; set; }
		public long LastGlobalSequenceNumber { get; set; }

		public string Name { get; set; }

		public void Handle(IViewContext context, AssociationCreated domainEvent)
		{
			var aggregateRootId = domainEvent.GetAggregateRootId();
			var association = context.Load<Domain.Association>(aggregateRootId);

			Name = association.Name;
		}
	}
}