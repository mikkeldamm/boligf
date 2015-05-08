using Boligf.Api.Domain;
using d60.Cirqus.Commands;

namespace Boligf.Api.Commands
{
	public class CreateAssociationCommand : Command<Association>
	{
		public string Name { get; set; }

		public CreateAssociationCommand(string associationId) : base(associationId)
		{

		}

		public override void Execute(Association aggregateRoot)
		{
			aggregateRoot.UpdateName(Name);
		}
	}
}
