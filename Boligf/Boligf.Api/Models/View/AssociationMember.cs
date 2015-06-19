using Boligf.Api.Views.User;
using d60.Cirqus.Views.ViewManagers;

namespace Boligf.Api.Models.View
{
	public class AssociationMember
	{
		public string Id { get; set; }
		public string Email { get; set; }

		public AssociationAddress Address { get; set; }

		
		public void MapUserDetailsToMember(IViewManager<GetUserView> getUserView)
		{
			var userView = getUserView.Load(Id);

			Email = userView.UserProfile.Email;
		}
	}
}