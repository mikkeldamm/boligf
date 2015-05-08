using System;
using System.Collections.Generic;
using System.Web.Http;
using Boligf.Api.Commands;
using Boligf.Api.Views.Association;
using d60.Cirqus;
using d60.Cirqus.Views.ViewManagers;

namespace Boligf.Api.Controllers
{
	[Authorize]
	public class AssociationController : ApiController
	{
		private readonly ICommandProcessor _commandProcessor;
		private readonly IViewManager<GetAssociationView> _getAssociationView;

		public AssociationController(
			ICommandProcessor commandProcessor,
			IViewManager<GetAssociationView> getAssociationView
			)
		{
			_commandProcessor = commandProcessor;
			_getAssociationView = getAssociationView;
		}

		// GET api/values
		public IEnumerable<string> Get()
		{
			return new[] {""};
		}

		// GET api/values/5
		public string Get(string id)
		{
			var associationId = id;
			var view = _getAssociationView.Load(associationId);

			return view.AssociationName;
		}

		// POST api/values
		public void Post([FromBody]string name, string userId)
		{
			var associationId = Guid.NewGuid().ToString();

			_commandProcessor.ProcessCommand(new CreateAssociationCommand(associationId) { Name = name });
			_commandProcessor.ProcessCommand(new RegisterUserToAssociationCommand(associationId) { UserId = userId });
		}

		// PUT api/values/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/values/5
		public void Delete(int id)
		{
		}
	}
}
