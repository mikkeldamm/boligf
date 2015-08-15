using System.Collections.Generic;
using System.Text;

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
			public string No { get; set; }
			public string Floor { get; set; }
			public string Door { get; set; }
			public string Zip { get; set; }
			public string City { get; set; }
			public string Country { get; set; }

			public string GetFullAddress()
			{
				var stringBuilder = new StringBuilder();

				stringBuilder.Append(StreetAddress);

				if (!string.IsNullOrWhiteSpace(No))
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(No);
					stringBuilder.Append(",");
				}

				if (!string.IsNullOrWhiteSpace(Floor) && !string.IsNullOrWhiteSpace(Door))
				{
					stringBuilder.Append(" ");
					stringBuilder.Append(Floor);
					stringBuilder.Append(".");
					stringBuilder.Append(Door);
					stringBuilder.Append(",");
				}

				stringBuilder.Append(" ");
				stringBuilder.Append(Zip);
				stringBuilder.Append(" ");
				stringBuilder.Append(City);

				return stringBuilder.ToString();
			}

			public List<string> Residents { get; set; }

			public Address()
			{
				Residents = new List<string>();
			}
		}
	}
}