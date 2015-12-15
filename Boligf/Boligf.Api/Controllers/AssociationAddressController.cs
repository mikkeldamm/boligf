using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Boligf.Api.Commands;
using Boligf.Api.Models.Post;
using Boligf.Api.Models.View;
using Boligf.Api.Play;
using Boligf.Api.Views.Association.Address;
using d60.Cirqus;
using d60.Cirqus.Views.ViewManagers;
using Boligf.Api.Utils;

namespace Boligf.Api.Controllers
{
	[RoutePrefix("api/association")]
	[Authorize]
    public class AssociationAddressController : ApiController
	{
		private readonly ICommandProcessor _commandProcessor;
		private readonly IViewManager<GetAddressesView> _getAddressesView;
		private readonly IViewManager<GetAddressesWithResidentsView> _getAddressesWithResidentsView;

		public AssociationAddressController(
			ICommandProcessor commandProcessor,
			IViewManager<GetAddressesView> getAddressesView,
			IViewManager<GetAddressesWithResidentsView> getAddressesWithResidentsView
		)
		{
			_commandProcessor = commandProcessor;
			_getAddressesView = getAddressesView;
			_getAddressesWithResidentsView = getAddressesWithResidentsView;
		}

		/*
		** Address
		*/
		[Route("{associationId}/address"), HttpGet]
		[AllowAnonymous]
		public IEnumerable<AssociationAddress> GetAllAddresses(string associationId)
		{
			var view = _getAddressesWithResidentsView.Load(associationId);
			return view.GetAllAddresses();
		}

		[Route("{associationId}/address"), HttpPost]
		public void Post(string associationId, [FromBody]List<AssociationAddressRegister> addresses)
		{
			foreach (var address in addresses)
			{
				_commandProcessor.ProcessCommand(new AddAddressToAssociationCommand(associationId)
				{
					Id = address.Id,
					City = address.City,
					Zip = address.Zip,
					Country = "+45",
					Door = address.Door,
					Floor = address.Floor,
					No = address.No,
					StreetAddress = address.Streetname
				});

				_commandProcessor.ProcessCommand(new AddCodeToAddressCommand(associationId)
				{
					AddressId = address.Id,
					Code = HumanReadableUniqueId.NewUid()
				});
			}
		}

		/*
		** Address Code
		*/
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

		/*
		** Address User
		*/
		[Route("{associationId}/address/{addresId}/user"), HttpPost]
		public void Post(string associationId, string addressId, [FromBody] string userId)
		{
			_commandProcessor.ProcessCommand(new RegisterUserToAssociationCommand(associationId) { UserId = userId });
			_commandProcessor.ProcessCommand(new AttachUserToAddressCommand(associationId) { AddressId = addressId, UserId = userId });
		}
	}
}
