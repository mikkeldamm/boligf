using d60.Cirqus.Events;

namespace Boligf.Api.Domain.Events
{
	public class UserAttachedToAddress : DomainEvent<Association>
	{
		public string UserId { get; set; }
		public string AddressId { get; set; }
	}
}