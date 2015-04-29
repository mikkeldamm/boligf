using System.Collections.Generic;
using System.Web.Http;
using Boligf.Api.Commands;
using Boligf.Api.Play;
using Boligf.Api.Views.Association;
using d60.Cirqus;
using d60.Cirqus.Views.ViewManagers;
using d60.Cirqus.Views.ViewManagers.Locators;

namespace Boligf.Api.Controllers
{
	public class AssociationController : ApiController
	{
		private readonly ICommandProcessor _commandProcessor;
		private readonly IViewManager<GetAllAssociationsView> _getAllAssociationsView;
		private readonly IViewManager<GetSingleAssociationsView> _getSingleAssociationView;

		public AssociationController(
			ICommandProcessor commandProcessor,
			IViewManager<GetAllAssociationsView> getAllAssociationsView,
			IViewManager<GetSingleAssociationsView> getSingleAssociationView
			)
		{
			_commandProcessor = commandProcessor;
			_getAllAssociationsView = getAllAssociationsView;
			_getSingleAssociationView = getSingleAssociationView;
		}

		// GET api/values
		public IEnumerable<string> Get()
		{
			var view = _getAllAssociationsView.LoadFully();

			return view.Names;
		}

		// GET api/values/5
		public string Get(string id)
		{
			var associationId = id;
			var view = _getSingleAssociationView.Load(associationId);

			return view.Name;
		}

		// POST api/values
		public void Post([FromBody]string value)
		{
			_commandProcessor.ProcessCommand(new CreateAssociationCommand("1234", value));
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
