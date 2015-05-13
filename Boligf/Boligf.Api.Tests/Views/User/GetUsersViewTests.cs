using System.Linq;
using Boligf.Api.Domain.Events;
using Boligf.Api.Models.View;
using Boligf.Api.Tests.TestHelpers;
using Boligf.Api.Views.User;
using Shouldly;
using Xunit;

namespace Boligf.Api.Tests.Views.User
{
	public class GetUsersViewTests
	{
		[Fact]
		public void HandleUserCreated_UserCreated_IsAddedToList()
		{
			// Arrange
			var userId = "1234";
			var userEvent = DomainEventHelper.Create<UserCreated>(userId);

			// Act
			var view = new GetUsersView();
			view.Handle(null, userEvent);

			// Assert
			view.UserProfiles.ShouldContain(profile => profile.Id == userId);
		}

		[Fact]
		public void HandleUserEmailUpdated_UsersEmailChanged_UpdatesEmailOnProfile()
		{
			// Arrange
			var userId = "test2";
			var updatedEmail = "newemail";
			var emailEvent = DomainEventHelper.Create<UserEmailUpdated>(userId);
			emailEvent.Email = updatedEmail;

			// Act
			var view = new GetUsersView();
			view.UserProfiles.Add(new UserProfile { Id = "test1", Email = "email1" });
			view.UserProfiles.Add(new UserProfile { Id = "test2", Email = "email2" });
			view.UserProfiles.Add(new UserProfile { Id = "test3", Email = "email3" });
			view.Handle(null, emailEvent);

			// Assert
			view.UserProfiles.ShouldContain(p => p.Id == userId && p.Email == updatedEmail);
		}

		[Fact]
		public void HandleUserDetailsUpdated_UserFirstAndLastNameChanged_UpdatesDetailsOnProfile()
		{
			// Arrange
			var userId = "test2";
			var updatedFirstName = "newfirstname";
			var updatedLastName = "newlastname";
			var emailEvent = DomainEventHelper.Create<UserDetailsUpdated>(userId);
			emailEvent.FirstName = updatedFirstName;
			emailEvent.LastName = updatedLastName;

			// Act
			var view = new GetUsersView();
			view.UserProfiles.Add(new UserProfile { Id = "test1", FirstName = "a1", LastName = "b1" });
			view.UserProfiles.Add(new UserProfile { Id = "test2", FirstName = "a2", LastName = "b2" });
			view.UserProfiles.Add(new UserProfile { Id = "test3", FirstName = "a3", LastName = "b3" });
			view.Handle(null, emailEvent);

			// Assert
			view.UserProfiles.ShouldContain(p => p.Id == userId && p.FirstName == updatedFirstName && p.LastName == updatedLastName);
		}

		[Fact]
		public void HandleUserDeleted_UserIdExist_UserProfileRemovedFromList()
		{
			// Arrange
			var userId = "test2";
			var userEvent = DomainEventHelper.Create<UserDeleted>(userId);
			userEvent.Id = userId;

			// Act
			var view = new GetUsersView();
			view.UserProfiles.Add(new UserProfile { Id = "test1" });
			view.UserProfiles.Add(new UserProfile { Id = "test2" });
			view.UserProfiles.Add(new UserProfile { Id = "test3" });
			view.Handle(null, userEvent);

			// Assert
			view.UserProfiles.Count.ShouldBe(2);
			view.UserProfiles.ShouldNotContain(p => p.Id == userId);
		}
	}
}
