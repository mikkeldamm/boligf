using System.Threading.Tasks;
using Boligf.Api.Commands;
using Boligf.Api.Domain.Entities;
using d60.Cirqus;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Boligf.Api.Infrastructure
{
	public interface IUserManager
	{
		Task<UserIdentity> FindAsync(string userName, string password);
		Task<UserIdentity> FindByIdAsync(string id);
		Task<IdentityResult> CreateAsync(UserIdentity user, string password);
		Task<IdentityResult> UpdateAsync(UserIdentity user);
		Task<IdentityResult> DeleteAsync(UserIdentity user);
	}

	public class UserManager : UserManager<UserIdentity>, IUserManager
	{
		private readonly ICommandProcessor _commandProcessor;

		public UserManager(UserStore<UserIdentity> store, ICommandProcessor commandProcessor) : base(store)
		{
			_commandProcessor = commandProcessor;

			SetupUserValidations();
			SetupPasswordValidations();
		}

		public override async Task<IdentityResult> CreateAsync(UserIdentity user, string password)
		{
			var createdIdentity = await base.CreateAsync(user, password);
			if (createdIdentity.Succeeded)
			{
				var newlyCreatedUser = await FindAsync(user.UserName, password);
				if (newlyCreatedUser != null)
				{
					_commandProcessor.ProcessCommand(new CreateUserCommand(newlyCreatedUser.Id)
					{
						Email = newlyCreatedUser.Email
					});
				}
			}

			return createdIdentity;
		}

		public override async Task<IdentityResult> UpdateAsync(UserIdentity user)
		{
			var result = await base.UpdateAsync(user);
			if (result.Succeeded)
			{
				_commandProcessor.ProcessCommand(new UpdateUserDetailsCommand(user.Id)
				{
					Email = user.Email,
					FirstName = user.Name,
					LastName = user.Lastname
				});
			}

			return result;
		}

		public override async Task<IdentityResult> DeleteAsync(UserIdentity user)
		{
			var result = await base.DeleteAsync(user);
			if (result.Succeeded)
			{
				_commandProcessor.ProcessCommand(new DeleteUserCommand(user.Id));
			}

			return result;
		}

		private void SetupUserValidations()
		{
			UserValidator = new UserValidator<UserIdentity>(this)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};
		}

		private void SetupPasswordValidations()
		{
			PasswordValidator = new PasswordValidator
			{
				RequiredLength = 4,
				RequireNonLetterOrDigit = false,
				RequireDigit = false,
				RequireLowercase = false,
				RequireUppercase = false,
			};
		}

	}
}