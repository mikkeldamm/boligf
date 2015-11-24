using System.Collections.Generic;
using Boligf.Api.Domain;
using d60.Cirqus.Commands;

namespace Boligf.Api.Commands
{
	public class AddAddressesToAssociationCommand : Command<Association>
	{
		public class Address
		{
			public string Id { get; set; }
			public string Streetname { get; set; }
			public string No { get; set; }
			public string Floor { get; set; }
			public string Door { get; set; }
			public string Zip { get; set; }
			public string City { get; set; }
			public string Latitude { get; set; }
			public string Longitude { get; set; }
			public string FullAddress { get; set; }
			public string Country { get; set; }
		}

		public List<Address> Addresses { get; set; }

		public AddAddressesToAssociationCommand(string associationId) : base(associationId)
		{
			Addresses = new List<Address>();
        }

		public override void Execute(Association aggregateRoot)
		{
			foreach (var address in Addresses)
			{
				aggregateRoot.AddAddress(
					address.Id, 
					address.Streetname, 
					address.No, 
					address.Floor, 
					address.Door, 
					address.City, 
					address.Zip, 
					address.Country
				);
			}
		}
	}
}