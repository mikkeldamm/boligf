using d60.Cirqus.Events;

namespace Boligf.Api.Domain.Events
{
	public class CodeAssignedToAddress : DomainEvent<Association>
	{
		public string AddressId { get; set; }
		public string Code { get; set; }
	}
}