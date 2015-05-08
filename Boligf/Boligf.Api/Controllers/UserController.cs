using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Boligf.Api.Domain.Entities;
using Boligf.Api.Infrastructure;
using Boligf.Api.Play;
using Boligf.Api.Views.Association;
using d60.Cirqus.Views.ViewManagers;

namespace Boligf.Api.Controllers
{
	public class UserController : ApiController
	{
		private readonly IUserManager _userManager;
		private readonly IViewManager<GetUserAssociationsView> _getUserAssociationsView;

		public UserController(IUserManager userManager, IViewManager<GetUserAssociationsView> getUserAssociationsView)
		{
			_userManager = userManager;
			_getUserAssociationsView = getUserAssociationsView;
		}

		public string Get(string id)
		{
			var view = _getUserAssociationsView.LoadFully();
			var associations = view.GetUsersAssociations(id);

			return associations.Any() ? associations[0].Name : "none";
		}

		[HttpPost]
		public async Task Post(string username, string password)
		{
			var user = new UserIdentity
			{
				Name = "Mikkel",
				Lastname = "Damm",
				UserName = username, 
				Email =  username
			};

			var test = await _userManager.CreateAsync(user, password);
		}
	}
}
