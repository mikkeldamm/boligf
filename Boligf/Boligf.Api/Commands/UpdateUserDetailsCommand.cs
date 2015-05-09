using Boligf.Api.Domain;
using d60.Cirqus.Commands;

namespace Boligf.Api.Commands
{
	public class UpdateUserDetailsCommand: Command<User>
	{
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }

		public UpdateUserDetailsCommand(string id) : base(id)
		{

		}

		public override void Execute(User aggregateRoot)
		{
			aggregateRoot.UpdateEmail(Email);
			aggregateRoot.UpdateDetails(FirstName, LastName);
		}
	}
}