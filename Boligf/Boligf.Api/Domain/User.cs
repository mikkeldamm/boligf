using Boligf.Api.Domain.Events;
using d60.Cirqus.Aggregates;
using d60.Cirqus.Events;

namespace Boligf.Api.Domain
{
	public class User : AggregateRoot, 
		IEmit<UserCreated>, 
		IEmit<UserEmailUpdated>, 
		IEmit<UserDetailsUpdated>,
		IEmit<UserDeleted>
	{
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		protected override void Created()
		{
			Emit(new UserCreated());
		}

		public void Apply(UserCreated e)
		{
		}


		public void UpdateEmail(string email)
		{
			Emit(new UserEmailUpdated { Email = email });
		}

		public void Apply(UserEmailUpdated e)
		{
			Email = e.Email;
		}

		
		public void UpdateDetails(string firstName, string lastName)
		{
			Emit(new UserDetailsUpdated { FirstName = firstName, LastName = lastName });
		}

		public void Apply(UserDetailsUpdated e)
		{
			FirstName = e.FirstName;
			LastName = e.LastName;
		}


		public void DeleteUser()
		{
			Emit(new UserDeleted { Id = Id });
		}

		public void Apply(UserDeleted e)
		{

		}
	}
}