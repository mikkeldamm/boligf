using System;
using System.Collections.Generic;
using System.Linq;
using Boligf.Api.Controllers;
using Boligf.Api.Models.View;
using Boligf.Api.Views.Association;
using Boligf.Api.Views.User;
using d60.Cirqus.Views.ViewManagers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Boligf.Api.Tests.Controllers
{
	public class AssociationUserControllerTests
	{
		private readonly IViewManager<GetAssociationUserView> _getAssociationUserViewMock;
		private readonly IViewManager<GetUserView> _getUserViewMock;

		private readonly AssociationUserController _associationUserController;

		public AssociationUserControllerTests()
		{
			_getAssociationUserViewMock = Substitute.For<IViewManager<GetAssociationUserView>>();
			_getUserViewMock = Substitute.For<IViewManager<GetUserView>>();

			_associationUserController = new AssociationUserController(_getAssociationUserViewMock, _getUserViewMock);
		}

		[Fact]
		public void Get_NoUsersExists_ReturnsEmptyList()
		{
			// Arrange
			const string dummyAssociationId = "1234";

			_getAssociationUserViewMock
				.Load(dummyAssociationId)
				.Returns(new GetAssociationUserView());

			// Act
			var members = _associationUserController.Get(dummyAssociationId);

			// Assert
			members.ShouldBeEmpty();
		}

		[Fact]
		public void Get_NoUsersLoaded_ThrowsNullReference()
		{
			// Arrange
			const string dummyAssociationId = "1234";

			_getAssociationUserViewMock
			   .Load(dummyAssociationId)
			   .Returns((GetAssociationUserView)null);

			// Act & Assert
			Should.Throw<NullReferenceException>(() =>
			{
				_associationUserController.Get(dummyAssociationId);
			});
		}

		[Fact]
		public void Get_UsersExists_ReturnsListOfMembers()
		{
			const string dummyAssociationId = "1234";

			_getUserViewMock
				.Load("123")
				.Returns(new GetUserView { UserProfile = new UserProfile { Email = "aAb.dk" } });

			_getAssociationUserViewMock
			   .Load(dummyAssociationId)
			   .Returns(new GetAssociationUserView
			   {
				   AssociationMembers = new List<AssociationMember>
				   {
					   new AssociationMember { Id = "123" },
					   new AssociationMember { Id = "123" },
					   new AssociationMember { Id = "123" }
				   }
			   });
			
			var members = _associationUserController.Get(dummyAssociationId);

			members.Count().ShouldBe(3);
		}

		[Fact]
		public void GetById_UserIsFoundAndMapped_ReturnMemberMapped()
		{
			// Arrange
			const string dummyAssociationId = "1234";
			const string userId = "123456";

			_getUserViewMock
				.Load(userId)
				.Returns(new GetUserView() { UserProfile = new UserProfile { Email = "aAb.dk" } });

			_getAssociationUserViewMock
				.Load(dummyAssociationId)
				.Returns(new GetAssociationUserView()
				{
					AssociationMembers = new List<AssociationMember>
					{
						new AssociationMember { Id = "123456" }
					}
				});

			// Act
			var member = _associationUserController.Get(dummyAssociationId, userId);

			// Assert
			member.Id.ShouldBe(userId);
			member.Email.ShouldBe("aAb.dk");
		}

		[Fact]
		public void GetById_UserNotLoaded_ThrowsNullReference()
		{
			// Arrange
			const string dummyAssociationId = "1234";
			const string userId = "123456";

			_getAssociationUserViewMock
				.Load(dummyAssociationId)
				.Returns(new GetAssociationUserView());

			// Act &  Assert
			Should.Throw<NullReferenceException>(() =>
			{
				_associationUserController.Get(dummyAssociationId, userId);
			});
		}
	}
}
