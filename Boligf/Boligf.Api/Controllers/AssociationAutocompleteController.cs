using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Boligf.Api.Models.View;
using Boligf.Api.Play;
using Boligf.Api.Views.Association;
using d60.Cirqus.Views.ViewManagers;

namespace Boligf.Api.Controllers
{
	[RoutePrefix("api/association")]
	public class AssociationAutocompleteController : ApiController
	{
		private readonly IViewManager<GetAssociationsView> _getAssociationsView;

		public AssociationAutocompleteController(IViewManager<GetAssociationsView> getAssociationsView)
		{
			_getAssociationsView = getAssociationsView;
		}
		
		[Route("autocomplete/query"), HttpGet]
		public List<Association> Get(string q)
		{
			var searchQuery = q.ToLower();
			var view = _getAssociationsView.Load();

			var associationsMatched = view.Associations
				.Where(association => association.Name.ToLower().Contains(searchQuery) || association.Addresses.Any(a => a.GetFullAddress().ToLower().Contains(searchQuery)))
				.ToList();

			if (associationsMatched.Any())
				associationsMatched = associationsMatched.Take(10).ToList();

			return associationsMatched;
		}
	}
}
