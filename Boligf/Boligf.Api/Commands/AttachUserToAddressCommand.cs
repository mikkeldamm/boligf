using Boligf.Api.Domain;
using d60.Cirqus.Commands;

namespace Boligf.Api.Commands
{
	public class AttachUserToAddressCommand : Command<Association>
	{
		public string AddressId { get; set; }
		public string UserId { get; set; }

		public AttachUserToAddressCommand(string associationId) : base(associationId)
		{
		}

		public override void Execute(Association aggregateRoot)
		{
			aggregateRoot.AttachUserToAddress(UserId, AddressId);
		}
	}
}