using System.Collections.Generic;
using System.Linq;
using Boligf.Api.Domain.Events;
using Boligf.Api.Models.View;
using d60.Cirqus.Events;
using d60.Cirqus.Extensions;
using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Api.Views.User
{
	public class GetUsersView : IViewInstance<GlobalInstanceLocator>,
		ISubscribeTo<UserCreated>,
		ISubscribeTo<UserEmailUpdated>,
		ISubscribeTo<UserDetailsUpdated>,
		ISubscribeTo<UserDeleted>
	{
		public string Id { get; set; }
		public long LastGlobalSequenceNumber { get; set; }

		public List<UserProfile> UserProfiles { get; set; }

		public GetUsersView()
		{
			UserProfiles = new List<UserProfile>();
		}

		public void Handle(IViewContext context, UserCreated domainEvent)
		{
			UserProfiles.Add(new UserProfile
			{
				Id = domainEvent.GetAggregateRootId()
			});
		}

		public void Handle(IViewContext context, UserEmailUpdated domainEvent)
		{
			FindInternalUserProfile(domainEvent).Email = domainEvent.Email;
		}

		public void Handle(IViewContext context, UserDetailsUpdated domainEvent)
		{
			var user = FindInternalUserProfile(domainEvent);

			user.FirstName = domainEvent.FirstName;
			user.LastName = domainEvent.LastName;
		}

		public void Handle(IViewContext context, UserDeleted domainEvent)
		{
			UserProfiles.RemoveAll(u => u.Id == domainEvent.Id);
		}

		private UserProfile FindInternalUserProfile(DomainEvent domainEvent)
		{
			return UserProfiles.SingleOrDefault(u => u.Id == domainEvent.GetAggregateRootId());
		}
	}
}