using System;
using System.Collections.Generic;
using System.Web.Http;
using Boligf.Api.Views.Association;
using d60.Cirqus.Views.ViewManagers;

namespace Boligf.Api.Controllers
{
	[RoutePrefix("api/association")]
    public class AssociationUserController : ApiController
	{
		private readonly IViewManager<GetAssociationUserView> _getAssociationView;

		public AssociationUserController(
			IViewManager<GetAssociationUserView> getAssociationView)
		{
			_getAssociationView = getAssociationView;
		}

		[Route("{associationId}/user"), HttpGet]
		public IEnumerable<Models.View.AssociationUserProfile> Get()
		{
			return new Models.View.AssociationUserProfile[] {};
		}

		[Route("{associationId}/user/{id}"), HttpGet]
		public Models.View.AssociationUserProfile Get(string associationId, string id)
		{
			var view = _getAssociationView.Load(associationId);

			return new Models.View.AssociationUserProfile {Email = "larssnabeladamm.dk"};
		}
	}
}
