using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Boligf.Api.Models.View;
using Boligf.Api.Views.Association;
using Boligf.Api.Views.User;
using d60.Cirqus.Views.ViewManagers;

namespace Boligf.Api.Controllers
{
	[RoutePrefix("api/association")]
    public class AssociationUserController : ApiController
	{
		private readonly IViewManager<GetAssociationUserView> _getAssociationUserView;
		private readonly IViewManager<GetUserView> _getUserView;

		public AssociationUserController(
			IViewManager<GetAssociationUserView> getAssociationUserView, 
			IViewManager<GetUserView> getUserView)
		{
			_getAssociationUserView = getAssociationUserView;
			_getUserView = getUserView;
		}

		[Route("{associationId}/user"), HttpGet]
		public IEnumerable<AssociationMember> Get(string associationId)
		{
			var view = _getAssociationUserView.Load(associationId);

			return view.GetMembers().Select(m =>
			{
				m.MapUserDetailsToMember(_getUserView);
				return m;
			});
		}

		[Route("{associationId}/user/{id}"), HttpGet]
		public AssociationMember Get(string associationId, string id)
		{
			var view = _getAssociationUserView.Load(associationId);
			var member = view.GetMember(id);

			member.MapUserDetailsToMember(_getUserView);

			return member;
		}
	}
}
