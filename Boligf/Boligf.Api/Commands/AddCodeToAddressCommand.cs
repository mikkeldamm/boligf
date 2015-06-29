using Boligf.Api.Domain;
using d60.Cirqus.Commands;

namespace Boligf.Api.Commands
{
	public class AddCodeToAddressCommand : Command<Association>
	{
		public string Code { get; set; }
		public string AddressId { get; set; }

		public AddCodeToAddressCommand(string associationId) : base(associationId)
		{
		}

		public override void Execute(Association aggregateRoot)
		{
			aggregateRoot.AddCodeToAddress(AddressId, Code);
		}
	}
}