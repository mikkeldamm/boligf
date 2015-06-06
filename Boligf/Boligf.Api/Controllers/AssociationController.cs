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
		
		public IEnumerable<string> Get()
		{
			return new[] {""};
		}
		
		public string Get(string id)
		{
			var associationId = id;
			var view = _getAssociationView.Load(associationId);

			return view.AssociationName;
		}

		[AllowAnonymous]
		public string Post([FromBody]Models.Post.AssociationRegister associationRegister)
		{
			var associationId = Guid.NewGuid().ToString();

			_commandProcessor.ProcessCommand(new CreateAssociationCommand(associationId) { Name = associationRegister.Name });
			_commandProcessor.ProcessCommand(new RegisterUserToAssociationCommand(associationId) { UserId = associationRegister.UserId });

			return associationId;
        }
		
		public void Put(int id, [FromBody]string value)
		{
		}
		
		public void Delete(int id)
		{
		}
	}
}
