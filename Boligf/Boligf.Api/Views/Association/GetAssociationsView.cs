using System.Collections.Generic;
using System.Linq;
using Boligf.Api.Domain.Events;
using d60.Cirqus.Events;
using d60.Cirqus.Extensions;
using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Api.Views.Association
{
	public class GetAssociationsView : IViewInstance<GlobalInstanceLocator>,
		ISubscribeTo<AssociationCreated>,
		ISubscribeTo<AssociationNameUpdated>,
		ISubscribeTo<AddressAddedToAssociation>,
		ISubscribeTo<UserAttachedToAddress>
	{
		public string Id { get; set; }
		public long LastGlobalSequenceNumber { get; set; }

		public List<Models.View.Association> Associations { get; set; }

		public GetAssociationsView()
		{
			Associations = new List<Models.View.Association>();
		}

		public void Handle(IViewContext context, AssociationCreated domainEvent)
		{
			Associations.Add(new Models.View.Association
			{
				Id = domainEvent.GetAggregateRootId()
			});
		}

		public void Handle(IViewContext context, AssociationNameUpdated domainEvent)
		{
			FindInternalAssociation(domainEvent).Name = domainEvent.Name;
		}

		public void Handle(IViewContext context, AddressAddedToAssociation domainEvent)
		{
			var address = new Models.View.Association.Address
			{
				Id = domainEvent.Id,
				StreetAddress = domainEvent.StreetAddress,
				No = domainEvent.No,
				Floor = domainEvent.Floor,
				Door = domainEvent.Door,
				City = domainEvent.City,
				Zip = domainEvent.Zip,
				Country = domainEvent.Country
			};

			FindInternalAssociation(domainEvent).Addresses.Add(address);
		}

		public void Handle(IViewContext context, UserAttachedToAddress domainEvent)
		{
			var address = FindInternalAssociation(domainEvent).Addresses.SingleOrDefault(a => a.Id == domainEvent.AddressId);
			if (address != null)
			{
				address.Residents.Add(domainEvent.UserId);
			}
		}

		private Models.View.Association FindInternalAssociation(DomainEvent domainEvent)
		{
			return Associations.SingleOrDefault(a => a.Id == domainEvent.GetAggregateRootId());
		}
	}
}