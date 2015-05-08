using d60.Cirqus.Events;

namespace Boligf.Api.Domain.Events
{
	public class AssociationNameUpdated : DomainEvent<Association>
	{
		public string Name { get; set; }
	}
}