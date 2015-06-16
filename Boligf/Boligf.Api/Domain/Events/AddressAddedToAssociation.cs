using d60.Cirqus.Events;

namespace Boligf.Api.Domain.Events
{
	public class AddressAddedToAssociation : DomainEvent<Association>
	{
		public string Id { get; set; }
		public string StreetAddress { get; set; }
		public string City { get; set; }
		public string Zip { get; set; }
		public string Country { get; set; }
		public string No { get; set; }
		public string Floor { get; set; }
		public string Door { get; set; }
	}
}