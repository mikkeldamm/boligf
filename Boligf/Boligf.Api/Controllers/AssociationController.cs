using System;
using System.Collections.Generic;
using System.Web.Http;
using Boligf.Api.Commands;
using Boligf.Api.Models.Post;
using Boligf.Api.Models.View;
using Boligf.Api.Play;
using Boligf.Api.Views.Association;
using d60.Cirqus;
using d60.Cirqus.Views.ViewManagers;

namespace Boligf.Api.Controllers
{
	[RoutePrefix("api/association")]
	[Authorize]
	public class AssociationController : ApiController
	{
		private readonly ICommandProcessor _commandProcessor;
		private readonly IViewManager<GetAssociationsView> _getAssociationsView;
		private readonly IViewManager<GetAssociationView> _getAssociationView;

		public AssociationController(
			ICommandProcessor commandProcessor,
			IViewManager<GetAssociationsView> getAssociationsView,
            IViewManager<GetAssociationView> getAssociationView
			)
		{
			_commandProcessor = commandProcessor;
			_getAssociationsView = getAssociationsView;
			_getAssociationView = getAssociationView;
		}
		
		[Route(""), HttpGet]
		[AllowAnonymous]
		public List<Association> Get()
		{
			var view = _getAssociationsView.Load();
            return view.Associations;
		}

		[Route("{id}"), HttpGet]
		public string Get(string id)
		{
			var view = _getAssociationView.Load(id);
			return view.AssociationName;
		}
		
		[Route(""), HttpPost]
		[AllowAnonymous]
		public string Post([FromBody]AssociationRegister associationRegister)
		{
			var associationId = Guid.NewGuid().ToString();
			var addressId = associationRegister.AddressId ?? Guid.NewGuid().ToString();
			
			_commandProcessor.ProcessCommand(new CreateAssociationCommand(associationId)
			{
				Name = associationRegister.Name
			});

			_commandProcessor.ProcessCommand(new RegisterUserToAssociationCommand(associationId)
			{
				UserId = associationRegister.UserId
			});

			_commandProcessor.ProcessCommand(new AddAddressToAssociationCommand(associationId)
			{
				Id = addressId,
                StreetAddress = associationRegister.StreetAddress,
				No = associationRegister.No,
				Floor = associationRegister.Floor,
				Door = associationRegister.Door,
				City = associationRegister.City,
				Zip = associationRegister.Zip,
				Country = "+45"
			});

			_commandProcessor.ProcessCommand(new AttachUserToAddressCommand(associationId)
			{
				UserId = associationRegister.UserId,
				AddressId = addressId
			});

			return associationId;
        }

		[Route(""), HttpPut]
		public void Put(int id, [FromBody]string value)
		{
		}

		[Route(""), HttpDelete]
		public void Delete(int id)
		{
		}
	}
}
