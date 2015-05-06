using System.Threading.Tasks;
using Boligf.Api.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Boligf.Api.Infrastructure
{
	public interface IUserManager
	{
		Task<User> FindAsync(string userName, string password);
		Task<IdentityResult> CreateAsync(User user, string password);
	}

	public class UserManager : UserManager<User>, IUserManager
	{
		public UserManager(UserStore<User> store) : base(store)
		{
			SetupUserValidations();
			SetupPasswordValidations();
		}

		private void SetupUserValidations()
		{
			UserValidator = new UserValidator<User>(this)
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