using System.Collections.Generic;
using Boligf.Api.Commands;
using Boligf.Api.Controllers;
using Boligf.Api.Models.Post;
using Boligf.Api.Models.View;
using Boligf.Api.Play;
using Boligf.Api.Views.Association;
using d60.Cirqus;
using d60.Cirqus.Views.ViewManagers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Boligf.Api.Tests.Controllers
{
	public class AssociationControllerTests
	{
		private readonly AssociationController _controller;

		private readonly ICommandProcessor _commandProcessorMock;
		private readonly IViewManager<GetAssociationsView> _getAssociationsViewMock;
		private readonly IViewManager<GetAssociationView> _getAssociationViewMock;

		public AssociationControllerTests()
		{
			_commandProcessorMock = Substitute.For<ICommandProcessor>();
			_getAssociationsViewMock = Substitute.For<IViewManager<GetAssociationsView>>();
			_getAssociationViewMock = Substitute.For<IViewManager<GetAssociationView>>();

			_controller = new AssociationController(
				_commandProcessorMock,
				_getAssociationsViewMock,
				_getAssociationViewMock
			);
		}

		[Fact]
		public void Get_TwoAssociationsExists_ReturnsListOfAssocations()
		{
			_getAssociationsViewMock.Load().Returns(new GetAssociationsView()
			{
				Associations = new List<Association>
				{
					new Association(),
					new Association()
				}
			});
			
			var associations = _controller.Get();

			associations.Count.ShouldBe(2);
		}

		[Fact]
		public void Get_NoAssociationsExists_ReturnsEmptyList()
		{
			_getAssociationsViewMock.Load().Returns(new GetAssociationsView()
			{
				Associations = new List<Association>()
			});

			var associations = _controller.Get();

			associations.ShouldBeEmpty();
		}

		[Fact]
		public void Post_CreatedCorreclyAndAddressIdAssigned()
		{
			var postModel = new AssociationRegister
			{
				Name = "name",
				AddressId = "address123",
				StreetAddress = "address",
				No = "no",
				Floor = "floor",
				Door = "door",
				City = "city",
				Zip = "zip",
				UserId = "user123"
			};

			var associationId = _controller.Post(postModel);

			_commandProcessorMock.Received().ProcessCommand(Arg.Any<CreateAssociationCommand>());
			_commandProcessorMock.Received().ProcessCommand(Arg.Any<RegisterUserToAssociationCommand>());
			_commandProcessorMock.Received().ProcessCommand(Arg.Is<AddAddressToAssociationCommand>(ac => ac.Id == postModel.AddressId));
			_commandProcessorMock.Received().ProcessCommand(Arg.Any<AttachUserToAddressCommand>());
		}

		[Fact]
		public void Post_AddressIdIsUnique()
		{
			var postModel = new AssociationRegister
			{
				Name = "name",
				StreetAddress = "address",
				No = "no",
				Floor = "floor",
				Door = "door",
				City = "city",
				Zip = "zip",
				UserId = "user123"
			};

			var associationId = _controller.Post(postModel);
			
			_commandProcessorMock.Received().ProcessCommand(Arg.Is<AddAddressToAssociationCommand>(ac => ac.Id.Length == 36));
		}
	}
}
