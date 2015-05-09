using Boligf.Api.Domain.Events;
using Boligf.Api.Models.View;
using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Api.Views.User
{
	public class GetUserView : IViewInstance<InstancePerAggregateRootLocator>,
		ISubscribeTo<UserCreated>,
		ISubscribeTo<UserEmailUpdated>,
		ISubscribeTo<UserDetailsUpdated>
	{
		public string Id { get; set; }
		public long LastGlobalSequenceNumber { get; set; }

		public UserProfile UserProfile { get; set; }

		public void Handle(IViewContext context, UserCreated domainEvent)
		{
			UserProfile = new UserProfile();
		}

		public void Handle(IViewContext context, UserEmailUpdated domainEvent)
		{
			UserProfile.Email = domainEvent.Email;
		}

		public void Handle(IViewContext context, UserDetailsUpdated domainEvent)
		{
			UserProfile.FirstName = domainEvent.FirstName;
			UserProfile.LastName = domainEvent.LastName;
		}
	}
}