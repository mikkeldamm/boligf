using d60.Cirqus.Events;

namespace Boligf.Api.Domain.Events
{
	public class UserEmailUpdated : DomainEvent<User>
	{
		public string Email { get; set; }
	}
}
