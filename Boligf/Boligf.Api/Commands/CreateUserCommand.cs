using Boligf.Api.Domain;
using d60.Cirqus.Commands;

namespace Boligf.Api.Commands
{
	public class CreateUserCommand: Command<User>
	{
		public string Email { get; set; }

		public CreateUserCommand(string id) : base(id)
		{

		}

		public override void Execute(User aggregateRoot)
		{
			aggregateRoot.UpdateEmail(Email);
		}
	}
}