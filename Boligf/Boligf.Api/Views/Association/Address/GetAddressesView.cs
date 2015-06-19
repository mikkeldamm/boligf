using System.Collections.Generic;
using System.Linq;
using Boligf.Api.Domain.Events;
using Boligf.Api.Models.View;
using d60.Cirqus.Extensions;
using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Api.Views.Association.Address
{
	public class GetAddressesView : IViewInstance<GlobalInstanceLocator>,
		ISubscribeTo<AddressAddedToAssociation>,
		ISubscribeTo<AddressRemovedFromAssociation>,
		ISubscribeTo<CodeAssignedToAddress>
	{
		public string Id { get; set; }
		public long LastGlobalSequenceNumber { get; set; }

		private readonly Dictionary<string, List<AssociationAddressCode>> _associationAddresses;

		public GetAddressesView()
		{
			_associationAddresses = new Dictionary<string, List<AssociationAddressCode>>();
        }

		public AssociationAddressCode GetAddressByCode(string code)
		{
			return _associationAddresses
				.SelectMany(kv => kv.Value)
				.FirstOrDefault(address => address.Codes.Any(addressCode => addressCode == code));
		}

		public List<string> GetAddressCodes(string associationId, string addressId)
		{
			if (!_associationAddresses.ContainsKey(associationId))
				return null;

			var addresses = _associationAddresses[associationId];
			var address = addresses.SingleOrDefault(a => a.Id == addressId);
			if (address == null)
				return null;

			return address.Codes;
		}

		public void Handle(IViewContext context, AddressAddedToAssociation domainEvent)
		{
			var associationId = domainEvent.GetAggregateRootId();
			var addressWithCode = new AssociationAddressCode();

			addressWithCode.AssociationId = associationId;
            addressWithCode.MapFromEvent(domainEvent);

			GetAssociationAddressList(associationId).Add(addressWithCode);
		}

		public void Handle(IViewContext context, AddressRemovedFromAssociation domainEvent)
		{
			var associationId = domainEvent.GetAggregateRootId();

			GetAssociationAddressList(associationId).RemoveAll(a => a.Id == domainEvent.Id);
		}

		public void Handle(IViewContext context, CodeAssignedToAddress domainEvent)
		{
			var associationId = domainEvent.GetAggregateRootId();

			var address = GetAssociationAddressList(associationId).SingleOrDefault(a => a.Id == domainEvent.AddressId);
			if (address != null)
			{
				address.Codes.Add(domainEvent.Code);
            }
		}

		private List<AssociationAddressCode> GetAssociationAddressList(string associationId)
		{
			if (!_associationAddresses.ContainsKey(associationId))
				_associationAddresses.Add(associationId, new List<AssociationAddressCode>());

			return _associationAddresses[associationId];
		}
	}
}