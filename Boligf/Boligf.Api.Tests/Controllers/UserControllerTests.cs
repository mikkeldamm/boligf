using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Boligf.Api.Controllers;
using Boligf.Api.Domain.Entities;
using Boligf.Api.Infrastructure;
using Boligf.Api.Models.Post;
using Boligf.Api.Models.Put;
using Boligf.Api.Models.View;
using Boligf.Api.Play;
using Boligf.Api.Tests.TestExtensions;
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
			List<UserProfile> userProfiles = _userController.Get();

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

		[Fact]
		public async Task Post_EmailAndPasswordSet_CreatesIdentity()
		{
			// Arrange
			var userRegister = new UserRegister
			{
				Email = "a@b.dk",
				Password = "1234"
			};

			// Act
			await _userController.Post(userRegister);

			// Assert
			_userManagerMock
				.Received()
				.CreateAsync(Arg.Is<UserIdentity>(i => i.Email == "a@b.dk" && i.UserName == "a@b.dk"), Arg.Is("1234"))
				.IgnoreAwait();
		}

		[Fact]
		public async Task Put_UpdateProfileInfoSet_UpdatesIdentity()
		{
			// Arrange
			var userId = "1234";
			var userProfile = new UserUpdate
			{
				Email = "a@b.dk",
				FirstName = "a",
				LastName = "b"
			};

			_userManagerMock
				.FindByIdAsync(Arg.Any<string>())
				.Returns(Task.FromResult(new UserIdentity { Id = userId }));

			// Act
			await _userController.Put(userId, userProfile);

			// Assert
			_userManagerMock
				.Received()
				.UpdateAsync(Arg.Is<UserIdentity>(i => 
					i.Email == "a@b.dk" && 
					i.UserName == "a@b.dk" &&
					i.Name == "a" && 
					i.Lastname == "b" &&
					i.Id == userId
				))
				.IgnoreAwait();
		}

		[Fact]
		public async Task Delete_UserFoundById_UserIsDeleted()
		{
			// Arrange
			var userId = "1234";

			SetupPrincipalInControllerContext(userId);

			_userManagerMock
				.FindByIdAsync(Arg.Any<string>())
				.Returns(Task.FromResult(new UserIdentity { Id = userId }));

			// Act
			await _userController.Delete(userId);

			// Assert
			_userManagerMock
				.Received()
				.DeleteAsync(Arg.Is<UserIdentity>(i => i.Id == userId))
				.IgnoreAwait();
		}

		[Fact]
		public void  Delete_InputIdDifferentFromUserId_ThrowsUnAuthorized()
		{
			// Arrange
			var contextUserId = "4321";
			var userId = "1234";

			SetupPrincipalInControllerContext(contextUserId);

			// Act & Assert
			Should.Throw<UnauthorizedAccessException>(async () =>
			{
				await _userController.Delete(userId);
			});
		}

		private void SetupPrincipalInControllerContext(string userId)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, userId)
			};

			var genericIdentity = new GenericIdentity("newMockIdentity");
			
			genericIdentity.AddClaims(claims);
			
			var genericPrincipal = new GenericPrincipal(genericIdentity, new string[] {});

			_userController.User = genericPrincipal;
		}
	}
}
