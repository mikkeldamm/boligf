using Boligf.Api.Domain;
using d60.Cirqus.Commands;

namespace Boligf.Api.Commands
{
	public class DeleteUserCommand: Command<User>
	{
		public DeleteUserCommand(string id)
			: base(id)
		{

		}

		public override void Execute(User aggregateRoot)
		{
			aggregateRoot.DeleteUser();
		}
	}
}