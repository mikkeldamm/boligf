using Boligf.Api.Domain.Events;
using Boligf.Api.Tests.TestHelpers;
using Boligf.Api.Views.Association.Address;
using Shouldly;
using Xunit;

namespace Boligf.Api.Tests.Views.Association.Address
{
	public class GetAddressesViewTests
	{
		private readonly AddressAddedToAssociation _addressAddedToAssociationEvent;
		private readonly AddressRemovedFromAssociation _addressRemovedFromAssociationEvent;
		private readonly CodeAssignedToAddress _codeAssignedToAddressEvent;

		public GetAddressesViewTests()
		{
			var associationId = "ass123";

			_addressAddedToAssociationEvent = DomainEventHelper.Create<AddressAddedToAssociation>(associationId);
			_addressRemovedFromAssociationEvent = DomainEventHelper.Create<AddressRemovedFromAssociation>(associationId);
			_codeAssignedToAddressEvent = DomainEventHelper.Create<CodeAssignedToAddress>(associationId);
		}

		[Fact]
		public void GetAddressByCode_AddressDoesNotExists_ReturnsNull()
		{
			var view = new GetAddressesView();

			view.Handle(null, _addressAddedToAssociationEvent);
			view.Handle(null, _codeAssignedToAddressEvent);

			view.GetAddressByCode("code123").ShouldBe(null);
		}

		[Fact]
		public void GetAddressByCode_AddressExists_ReturnsAddressWithCodeAndAssociationId()
		{
			var view = new GetAddressesView();

			_addressAddedToAssociationEvent.Id = "address123";
			_codeAssignedToAddressEvent.AddressId = "address123";
            _codeAssignedToAddressEvent.Code = "code123";
			
			view.Handle(null, _addressAddedToAssociationEvent);
			view.Handle(null, _codeAssignedToAddressEvent);

			view.GetAddressByCode("code123").AssociationId.ShouldBe("ass123");
			view.GetAddressByCode("code123").Codes.ShouldContain("code123");
			view.GetAddressByCode("code123").Id.ShouldBe("address123");
		}

		[Fact]
		public void GetAddressCodes_AssociationDoesNotExists_ReturnsNull()
		{
			var view = new GetAddressesView();
			
			view.GetAddressCodes("ass123", "address123").ShouldBe(null);
		}

		[Fact]
		public void GetAddressCodes_AssociationExistsButCantFindAddress_ReturnsNull()
		{
			var view = new GetAddressesView();

			_addressAddedToAssociationEvent.Id = "address567";

			view.Handle(null, _addressAddedToAssociationEvent);

			view.GetAddressCodes("ass123", "address123").ShouldBe(null);
		}

		[Fact]
		public void GetAddressCodes_AssociationAndAddressExists_ReturnsListOfCodes()
		{
			var view = new GetAddressesView();
			
			_addressAddedToAssociationEvent.Id = "address123";
			_codeAssignedToAddressEvent.AddressId = "address123";
			_codeAssignedToAddressEvent.Code = "code123";

			view.Handle(null, _addressAddedToAssociationEvent);
			view.Handle(null, _codeAssignedToAddressEvent);

			view.GetAddressCodes("ass123", "address123").ShouldContain("code123");
		}

		[Fact]
		public void GetAddressByCode_AddressHasBeenRemovedFromAssociation_ReturnsNull()
		{
			var view = new GetAddressesView();

			_addressAddedToAssociationEvent.Id = "address123";
			_codeAssignedToAddressEvent.AddressId = "address123";
			_codeAssignedToAddressEvent.Code = "code123";
			_addressRemovedFromAssociationEvent.Id = "address123";

			view.Handle(null, _addressAddedToAssociationEvent);
			view.Handle(null, _codeAssignedToAddressEvent);
			view.Handle(null, _addressRemovedFromAssociationEvent);

			view.GetAddressByCode("code123").ShouldBe(null);
		}
	}
}
