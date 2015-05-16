using d60.Cirqus.Events;

namespace Boligf.Api.Tests.TestHelpers
{
	public static class DomainEventHelper
	{
		public static TDomainEvent Create<TDomainEvent>(string aggregateRootId) where TDomainEvent : DomainEvent, new()
		{
			var domainEvent = new TDomainEvent();

			domainEvent.Meta.Add(DomainEvent.MetadataKeys.AggregateRootId, aggregateRootId);

			return domainEvent;
		}
	}
}
