using d60.Cirqus.Events;

namespace Boligf.Api.Domain.Events
{
	public class AddressRemovedFromAssociation : DomainEvent<Association>
	{
		public string Id { get; set; }
	}
}