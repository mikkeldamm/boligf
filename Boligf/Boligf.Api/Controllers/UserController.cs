using System.Threading.Tasks;
using System.Web.Http;
using Boligf.Api.Domain;
using Boligf.Api.Infrastructure;

namespace Boligf.Api.Controllers
{
	public class UserController : ApiController
	{
		private readonly IUserManager _userManager;

		public UserController(IUserManager userManager)
		{
			_userManager = userManager;
		}

		[HttpPost]
		public async Task Post(string username, string password)
		{
			var user = new User
			{
				Name = "Mikkel",
				Lastname = "Damm",
				UserName = username
			};

			var test = await _userManager.CreateAsync(user, password);
		}
	}
}
