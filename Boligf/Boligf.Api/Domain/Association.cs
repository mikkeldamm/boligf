using System.Collections.Generic;
using System.Linq;
using Boligf.Api.Domain.Events;
using d60.Cirqus.Aggregates;
using d60.Cirqus.Events;

namespace Boligf.Api.Domain
{
	public class Association : AggregateRoot,
		IEmit<AssociationCreated>,
		IEmit<AssociationNameUpdated>,
		IEmit<MemberRegisteredToAssociation>,
		IEmit<AddressAddedToAssociation>,
		IEmit<UserAttachedToAddress>,
		IEmit<CodeAssignedToAddress>
	{
		public string Name { get; set; }
		public List<Address> Addresses { get; set; }
		public List<string> Members { get; set; }

		public Association()
		{
			Addresses = new List<Address>();
			Members = new List<string>();
		}
		
		protected override void Created()
		{
			Emit(new AssociationCreated());
		}

		public void Apply(AssociationCreated e)
		{
			// Created
		}

		public void UpdateName(string name)
		{
			Emit(new AssociationNameUpdated {Name = name});
		}

		public void Apply(AssociationNameUpdated e)
		{
			Name = e.Name;
		}

		public void RegisterMember(string userId)
		{
			Emit(new MemberRegisteredToAssociation {MemberId = userId});
		}

		public void Apply(MemberRegisteredToAssociation e)
		{
			Members.Add(e.MemberId);
		}

		public void AddAddress(string id, string streetAddress, string no, string floor, string door, string city, string zip, string country)
		{
			Emit(new AddressAddedToAssociation
			{
				Id = id,
				StreetAddress = streetAddress,
				No = no,
				Floor = floor,
				Door = door,
				City = city,
				Zip = zip,
				Country = country
			});
		}

		public void Apply(AddressAddedToAssociation e)
		{
			var address = new Address
			{
				Id = e.Id,
				StreetAddress = e.StreetAddress,
				No = e.No,
				Floor = e.Floor,
				Door = e.Door,
				City = e.City,
				Zip = e.Zip,
				Country = e.Country
			};
			
			Addresses.Add(address);
		}

		public void AttachUserToAddress(string userId, string addressId)
		{
			Emit(new UserAttachedToAddress { UserId = userId, AddressId = addressId });
		}

		public void Apply(UserAttachedToAddress e)
		{
			var address = Addresses.SingleOrDefault(a => a.Id == e.AddressId);
			if (address != null)
			{
				if (!address.Residents.Contains(e.UserId))
				{
					address.Residents.Add(e.UserId);
				}
			}
		}

		public void AddCodeToAddress(string addressId, string code)
		{
			Emit(new CodeAssignedToAddress { AddressId = addressId, Code = code });
		}

		public void Apply(CodeAssignedToAddress e)
		{
			var address = Addresses.SingleOrDefault(a => a.Id == e.AddressId);
			if (address != null && address.AssignCodes.Any(code => code != e.Code))
			{
				address.AssignCodes.Add(e.Code);
			}
		}
	}

	public class Address
	{
		public string Id { get; set; }
		public string StreetAddress { get; set; }
		public string No { get; set; }
		public string Floor { get; set; }
		public string Door { get; set; }
		public string City { get; set; }
		public string Zip { get; set; }
		public string Country { get; set; } // this is country code (digits)
		public string Cadastre { get; set; }
		public List<string> Residents { get; set; }
		public List<string> AssignCodes { get; set; }

		public Address()
		{
			Residents = new List<string>();
			AssignCodes = new List<string>();
        }
	}
}
