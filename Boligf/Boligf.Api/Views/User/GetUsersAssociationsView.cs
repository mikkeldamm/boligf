using System.Collections.Generic;
using Boligf.Api.Domain.Events;
using d60.Cirqus.Extensions;
using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Api.Views.User
{
	public class GetUsersAssociationsView : IViewInstance<GlobalInstanceLocator>, 
		ISubscribeTo<MemberRegisteredToAssociation>
	{
		public string Id { get; set; }
		public long LastGlobalSequenceNumber { get; set; }

		public Dictionary<string, List<UserAssociation>> AllUsersAssociations { get; set; }

		public GetUsersAssociationsView()
		{
			AllUsersAssociations = new Dictionary<string, List<UserAssociation>>();
		}

		public List<UserAssociation> GetUsersAssociations(string userId)
		{
			return AllUsersAssociations[userId];
		}

		public void Handle(IViewContext context, MemberRegisteredToAssociation domainEvent)
		{
			if (!AllUsersAssociations.ContainsKey(domainEvent.MemberId))
			{
				AllUsersAssociations.Add(domainEvent.MemberId, new List<UserAssociation>());
			}

			var association = context.Load<Domain.Association>(domainEvent.GetAggregateRootId());

			AllUsersAssociations[domainEvent.MemberId].Add(new UserAssociation
			{
				Id = association.Id,
				Name = association.Name
			});
		}

		public class UserAssociation
		{
			public string Id { get; set; }
			public string Name { get; set; }
		}
	}
}
