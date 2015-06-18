using System.Collections.Generic;
using System.Linq;
using Boligf.Api.Domain;
using Boligf.Api.Domain.Events;
using Shouldly;
using Xunit;

namespace Boligf.Api.Tests.Domain
{
	public class AssociationTests
	{
		[Fact]
		public void AddressAddedToAssociation_AddressDetailsAdded()
		{
			var addressAddedToAssociationEvent = new AddressAddedToAssociation
			{
				Id = "aId",
				StreetAddress = "aSa",
				No = "aNo",
				Floor = "aF",
				Door = "aD",
				City = "aC",
				Zip = "aZ",
				Country = "aCo"
			};

			var association = new Association();

			association.Apply(addressAddedToAssociationEvent);

			association.Addresses[0].Id.ShouldBe(addressAddedToAssociationEvent.Id);
			association.Addresses[0].StreetAddress.ShouldBe(addressAddedToAssociationEvent.StreetAddress);
			association.Addresses[0].No.ShouldBe(addressAddedToAssociationEvent.No);
			association.Addresses[0].Floor.ShouldBe(addressAddedToAssociationEvent.Floor);
			association.Addresses[0].Door.ShouldBe(addressAddedToAssociationEvent.Door);
			association.Addresses[0].City.ShouldBe(addressAddedToAssociationEvent.City);
			association.Addresses[0].Zip.ShouldBe(addressAddedToAssociationEvent.Zip);
			association.Addresses[0].Country.ShouldBe(addressAddedToAssociationEvent.Country);
		}

		[Fact]
		public void UserAttachedToAddress_AddressDoesNotExists()
		{
			const string userId = "user123";
			const string addressId = "address123";
			
			var association = new Association();
			association.Addresses.Add(new Address { Residents = new List<string> { "user1", "user2" } });
			association.Addresses.Add(new Address { Residents = new List<string> { "user3", "user4" } });

			association.Apply(new UserAttachedToAddress { UserId = userId, AddressId = addressId });

			association.Addresses.ShouldNotContain(a => a.Residents.Any(r => r == userId));
		}

		[Fact]
		public void UserAttachedToAddress_UserAddedToAddress()
		{
			const string userId = "user123";
			const string addressId = "address123";

			var association = new Association();
			association.Addresses.Add(new Address { Id = "address123", Residents = new List<string> { "user1" } });

			association.Addresses[0].Residents.Count.ShouldBe(1);

			association.Apply(new UserAttachedToAddress { UserId = userId, AddressId = addressId });

			association.Addresses[0].Residents.Count.ShouldBe(2);
		}

		[Fact]
		public void UserAttachedToAddress_UserAlreadyAddedToAddress()
		{
			const string userId = "user123";
			const string addressId = "address123";

			var association = new Association();
			association.Addresses.Add(new Address { Id = "address123", Residents = new List<string> { "user1", "user123" } });

			association.Addresses[0].Residents.Count.ShouldBe(2);

			association.Apply(new UserAttachedToAddress { UserId = userId, AddressId = addressId });

			association.Addresses[0].Residents.Count.ShouldBe(2);
		}
	}
}
