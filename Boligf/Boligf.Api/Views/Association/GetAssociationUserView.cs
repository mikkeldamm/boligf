using System.Collections.Generic;
using System.Linq;
using Boligf.Api.Domain.Events;
using Boligf.Api.Models.View;
using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Api.Views.Association
{
	public class GetAssociationUserView : IViewInstance<InstancePerAggregateRootLocator>,
		ISubscribeTo<MemberRegisteredToAssociation>,
		ISubscribeTo<AddressAddedToAssociation>,
		ISubscribeTo<UserAttachedToAddress>
	{
		public string Id { get; set; }
		public long LastGlobalSequenceNumber { get; set; }

		public List<AssociationMember> AssociationMembers { get; set; }
		public List<AssociationAddress> AssociationAddresses { get; set; }
		
		public GetAssociationUserView()
		{
			AssociationMembers = new List<AssociationMember>();
			AssociationAddresses = new List<AssociationAddress>();
		}

		public IEnumerable<AssociationMember> GetMembers()
		{
			return AssociationMembers;
		}

		public AssociationMember GetMember(string userId)
		{
			return AssociationMembers.SingleOrDefault(m => m.Id == userId);
		}

		public void Handle(IViewContext context, MemberRegisteredToAssociation domainEvent)
		{
			AssociationMembers.Add(new AssociationMember
			{
				Id = domainEvent.MemberId
			});
        }

		public void Handle(IViewContext context, AddressAddedToAssociation domainEvent)
		{
			var address = new AssociationAddress();
			address.MapFromEvent(domainEvent);

            AssociationAddresses.Add(address);
        }

		public void Handle(IViewContext context, UserAttachedToAddress domainEvent)
		{
			var userProfile = AssociationMembers.SingleOrDefault(m => m.Id == domainEvent.UserId);
			var address = AssociationAddresses.SingleOrDefault(a => a.Id == domainEvent.AddressId);

			if (userProfile != null && address != null)
			{
				userProfile.Address = address;
			}
		}
	}
}