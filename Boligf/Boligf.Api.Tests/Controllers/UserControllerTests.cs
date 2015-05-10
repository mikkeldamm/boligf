using System;
using Boligf.Api.Controllers;
using Boligf.Api.Infrastructure;
using Boligf.Api.Models.View;
using Boligf.Api.Play;
using Boligf.Api.Views.User;
using d60.Cirqus.Views.ViewManagers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace Boligf.Api.Tests.Controllers
{
	public class UserControllerTests
	{
		private readonly IUserManager _userManagerMock;
		private readonly IViewManager<GetUserView> _getUserViewMock;
		private readonly IViewManager<GetUsersView> _getUsersViewMock;

		private readonly UserController _userController;

		public UserControllerTests()
		{
			_userManagerMock = Substitute.For<IUserManager>();
			_getUserViewMock = Substitute.For<IViewManager<GetUserView>>();
			_getUsersViewMock = Substitute.For<IViewManager<GetUsersView>>();

			_userController = new UserController(_userManagerMock, _getUserViewMock, _getUsersViewMock);
		}

		[Fact]
		public void Get_NoUsersExists_ReturnsEmptyList()
		{
			// Arrange
			_getUsersViewMock
				.Load()
				.Returns(new GetUsersView());

			// Act
			var userProfiles = _userController.Get();

			// Assert
			userProfiles.ShouldBeEmpty();
		}

		[Fact]
		public void Get_NoUsersLoaded_ThrowsNullReference()
		{ 
			// Arrange
			_getUsersViewMock
			   .Load()
			   .Returns((GetUsersView)null);

			// Act & Assert
			Should.Throw<NullReferenceException>(() =>
			{
				_userController.Get();
			});
		}

		[Fact]
		public void GetById_UserDontExist_ReturnNull()
		{
			// Arrange
			var userId = "123456";

			_getUserViewMock
				.Load(userId)
				.Returns(new GetUserView());

			// Act
			var userProfile = _userController.Get(userId);

			// Assert
			userProfile.ShouldBe(null);
		}

		[Fact]
		public void GetById_UserNotLoaded_ThrowsNullReference()
		{
			// Arrange
			var userId = "123456";

			_getUserViewMock
			   .Load(userId)
			   .Returns((GetUserView)null);

			// Act &  Assert
			Should.Throw<NullReferenceException>(() =>
			{
				_userController.Get(userId);
			});
		}

		[Fact]
		public void GetById_UserExist_ReturnsUserProfile()
		{
			// Arrange
			var userId = "123456";
			var expectedEmail = "a@b.dk";

			_getUserViewMock
				.Load(userId)
				.Returns(new GetUserView
				{
					UserProfile = new UserProfile
					{
						Email = expectedEmail
					}
				});

			// Act
			var userProfile = _userController.Get(userId);

			// Assert
			userProfile.Email.ShouldBe(expectedEmail);
		}
	}
}
