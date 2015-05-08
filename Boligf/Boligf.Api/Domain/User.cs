using Boligf.Api.Domain.Events;
using d60.Cirqus.Aggregates;
using d60.Cirqus.Events;

namespace Boligf.Api.Domain
{
	public class User : AggregateRoot, 
		IEmit<UserCreated>, 
		IEmit<UserEmailUpdated>, 
		IEmit<UserDetailsUpdated>
	{
		public string Email { get; set; }
		public string FirstName { get; set; }

		protected override void Created()
		{
			Emit(new UserCreated());
		}

		public void UpdateNewlyCreatedDetails(string email, string firstName)
		{
			Emit(new UserEmailUpdated { Email = email });
			Emit(new UserDetailsUpdated { Firstname = firstName });
		}

		public void Apply(UserCreated e)
		{
		}

		public void Apply(UserEmailUpdated e)
		{
			Email = e.Email;
		}

		public void Apply(UserDetailsUpdated e)
		{
			FirstName = e.Firstname;
		}
	}
}