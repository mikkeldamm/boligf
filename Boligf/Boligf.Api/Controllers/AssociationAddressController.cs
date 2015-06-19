using System.Collections.Generic;
using System.Web.Http;
using Boligf.Api.Models.View;
using Boligf.Api.Play;
using Boligf.Api.Views.Association.Address;
using d60.Cirqus.Views.ViewManagers;

namespace Boligf.Api.Controllers
{
	[RoutePrefix("api/association/")]
	[Authorize]
    public class AssociationAddressController : ApiController
	{
		private readonly IViewManager<GetAddressesView> _getAddressesView;

		public AssociationAddressController(
			IViewManager<GetAddressesView> getAddressesView)
		{
			_getAddressesView = getAddressesView;
		}

		[Route("address/code/{code}"), HttpGet]
		[AllowAnonymous]
		public AssociationAddressCode Get(string code)
		{
			var view = _getAddressesView.Load();
			return view.GetAddressByCode(code);
		}

		[Route("{associationId}/address/{addresId}/code"), HttpGet]
		public IEnumerable<string> Get(string associationId, string addressId)
		{
			var view = _getAddressesView.Load();
			return view.GetAddressCodes(associationId, addressId);
		}
	}
}
