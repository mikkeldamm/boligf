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

		public AssociationAddressController(
			ICommandProcessor commandProcessor,
			IViewManager<GetAddressesView> getAddressesView)
		{
			_commandProcessor = commandProcessor;
			_getAddressesView = getAddressesView;
		}

		[Route("address/code/{code}"), HttpGet]
		[AllowAnonymous]
		public AssociationAddressCode Get(string code)
		{
			var view = _getAddressesView.Load();
			return view.GetAddressByCode(code);
		}

		[Route("{associationId}/address"), HttpGet]
		public IEnumerable<AssociationAddressCode> GetAllAddresses(string associationId)
		{
			var view = _getAddressesView.Load();
			return view.GetAllAddresses(associationId);
		}
			
		[Route("{associationId}/address/{addresId}/code"), HttpGet]
		public IEnumerable<string> Get(string associationId, string addressId)
		{
			var view = _getAddressesView.Load();
			return view.GetAddressCodes(associationId, addressId);
		}

		[Route("{associationId}/address/{addresId}/user"), HttpPost]
		public void Post(string associationId, string addressId, [FromBody] string userId)
		{
			_commandProcessor.ProcessCommand(new RegisterUserToAssociationCommand(associationId) { UserId = userId });
			_commandProcessor.ProcessCommand(new AttachUserToAddressCommand(associationId) { AddressId = addressId, UserId = userId });
		}

		[Route("{associationId}/address"), HttpPost]
		public void Post(string associationId, [FromBody]List<AssociationAddressRegister> addresses)
		{
			var command = new AddAddressesToAssociationCommand(associationId);

			foreach (var address in addresses)
			{
				command.Addresses.Add(new AddAddressesToAssociationCommand.Address
				{
					Id = address.Id,
					FullAddress = address.FullAddress,
					City = address.City,
					Zip = address.Zip,
					Country = "+45",
					Door = address.Door,
					Floor = address.Floor,
					No = address.No,
					Streetname = address.Streetname,
					Latitude = address.Latitude,
					Longitude = address.Longitude
				});
			}

			if (addresses.Any())
				return;

			_commandProcessor.ProcessCommand(command);

			foreach (var address in addresses)
			{
				_commandProcessor.ProcessCommand(new AddCodeToAddressCommand(associationId)
				{
					AddressId = address.Id,
					Code = HumanReadableUniqueId.NewUid()
				});
			}
		}
	}
}
