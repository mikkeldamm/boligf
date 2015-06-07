using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Boligf.Api.Controllers
{
	[RoutePrefix("api/association")]
    public class AssociationUserController : ApiController
	{
		[Route("{associationId}/user"), HttpGet]
		public IEnumerable<Models.View.AssociationUserProfile> Get()
		{
			return new Models.View.AssociationUserProfile[] {};
		}

		[Route("{associationId}/user/{id}"), HttpGet]
		public Models.View.AssociationUserProfile Get(string associationId, string id)
		{
			throw new ArgumentException("Dette er en custom fejl", associationId);
		}
	}
}
