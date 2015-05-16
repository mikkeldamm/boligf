using System.Collections.Generic;
using Boligf.Api.Domain.Events;
using d60.Cirqus.Aggregates;
using d60.Cirqus.Events;

namespace Boligf.Api.Domain
{
	public class Association : AggregateRoot, 
		IEmit<AssociationCreated>,
		IEmit<AssociationNameUpdated>, 
		IEmit<MemberRegisteredToAssociation>
	{ 
		public string Name { get; set; }
		public List<string> Members { get; set; }

		public Association()
		{
			Members = new List<string>();
		}

		protected override void Created()
		{
			Emit(new AssociationCreated());
		}

		public void UpdateName(string name)
		{
			Emit(new AssociationNameUpdated { Name = name });
		}

		public void RegisterMember(string userId)
		{
			Emit(new MemberRegisteredToAssociation { MemberId = userId });
		}

		public void Apply(AssociationCreated e)
		{

		}

		public void Apply(AssociationNameUpdated e)
		{
			Name = e.Name;
		}

		public void Apply(MemberRegisteredToAssociation e)
		{
			Members.Add(e.MemberId);
		}
	}
}
