using Boligf.Api.Domain;
using d60.Cirqus.Commands;

namespace Boligf.Api.Commands
{
	public class RegisterUserToAssociationCommand : Command<Association>
	{
		public string UserId { get; set; }

		public RegisterUserToAssociationCommand(string associationId) : base(associationId)
		{
		}

		public override void Execute(Association aggregateRoot)
		{
			aggregateRoot.RegisterMember(UserId);
		}
	}
}