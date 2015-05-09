using d60.Cirqus.Events;

namespace Boligf.Api.Domain.Events
{
	public class UserDeleted : DomainEvent<User>
	{
		public string Id { get; set; }
	}
}