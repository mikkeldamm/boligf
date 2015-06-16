using System.Collections.Generic;

namespace Boligf.Api.Models.View
{
	public class Association
	{
		public string Id { get; set; }
		public string Name { get; set; }

		public List<Address> Addresses { get; set; }

		public Association()
		{
			Addresses = new List<Address>();
		}

		public class Address
		{
			public string Id { get; set; }
			public string StreetAddress { get; set; }
			public string Zip { get; set; }
			public string City { get; set; }
			public string Country { get; set; }

			public List<string> Residents { get; set; }

			public Address()
			{
				Residents = new List<string>();
			}
		}
	}
}