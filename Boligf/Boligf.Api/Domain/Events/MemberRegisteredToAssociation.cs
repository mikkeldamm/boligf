using d60.Cirqus.Events;

namespace Boligf.Api.Domain.Events
{
	public class MemberRegisteredToAssociation : DomainEvent<Association>
	{
		public string MemberId { get; set; }
	}
}