using Boligf.Api.Domain.Events;

namespace Boligf.Api.Models.View
{
	public class AssociationAddress
	{
		public string Id { get; set; }
		public string StreetAddress { get; set; }
		public string No { get; set; }
		public string Floor { get; set; }
		public string Door { get; set; }
		public string Zip { get; set; }
		public string City { get; set; }
		public string Country { get; set; }

		public void MapFromEvent(AddressAddedToAssociation domainEvent)
		{
			this.Id = domainEvent.Id;
			this.StreetAddress = domainEvent.StreetAddress;
			this.No = domainEvent.No;
			this.Floor = domainEvent.Floor;
			this.Door = domainEvent.Door;
			this.City = domainEvent.City;
			this.Zip = domainEvent.Zip;
			this.Country = domainEvent.Country;
		}
	}
}