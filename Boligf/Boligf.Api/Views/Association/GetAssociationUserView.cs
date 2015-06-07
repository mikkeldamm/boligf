using Boligf.Api.Domain.Events;
using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Api.Views.Association
{
	public class GetAssociationUserView : IViewInstance<InstancePerAggregateRootLocator>,
		ISubscribeTo<MemberRegisteredToAssociation>
	{
		public string Id { get; set; }
		public long LastGlobalSequenceNumber { get; set; }

		public void Handle(IViewContext context, MemberRegisteredToAssociation domainEvent)
		{

		}
	}
}