using d60.Cirqus.Events;

namespace Boligf.Api.Domain.Events
{
	public class UserDetailsUpdated : DomainEvent<User>
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}
