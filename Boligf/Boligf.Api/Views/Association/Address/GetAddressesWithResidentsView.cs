using System;
using System.Collections.Generic;
using System.Linq;
using Boligf.Api.Domain.Events;
using Boligf.Api.Models.View;
using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Api.Views.Association.Address
{
	public class GetAddressesWithResidentsView : IViewInstance<InstancePerAggregateRootLocator>,
		ISubscribeTo<AddressAddedToAssociation>,
		ISubscribeTo<AddressRemovedFromAssociation>,
		ISubscribeTo<UserAttachedToAddress>
	{
		public string Id { get; set; }
		public long LastGlobalSequenceNumber { get; set; }

		private readonly List<AssociationAddress> _addresses;

		public GetAddressesWithResidentsView()
		{
			_addresses = new List<AssociationAddress>();
		}
		
		public IEnumerable<AssociationAddress> GetAllAddresses()
		{
			return _addresses.OrderBy(a => a.FullAddress);
		}

		public void Handle(IViewContext context, AddressAddedToAssociation domainEvent)
		{
			var address = new AssociationAddress();

			address.MapFromEvent(domainEvent);

			_addresses.Add(address);
		}

		public void Handle(IViewContext context, AddressRemovedFromAssociation domainEvent)
		{
			_addresses.RemoveAll(a => a.Id == domainEvent.Id);
		}

		public void Handle(IViewContext context, UserAttachedToAddress domainEvent)
		{
			var address = _addresses.FirstOrDefault(a => a.Id == domainEvent.AddressId);
			if (address != null)
			{
				if (!address.Residents.Any(r => r.Id == domainEvent.UserId))
				{
					address.Residents.Add(new Resident() { Id = domainEvent.UserId });
                }
            }
        }
	}
}