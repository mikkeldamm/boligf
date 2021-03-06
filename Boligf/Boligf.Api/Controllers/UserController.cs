﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using Boligf.Api.Domain.Entities;
using Boligf.Api.Infrastructure;
using Boligf.Api.Models.View;
using Boligf.Api.Play;
using Boligf.Api.Views.User;
using d60.Cirqus.Views.ViewManagers;

namespace Boligf.Api.Controllers
{
	[RoutePrefix("api/user")]
	[Authorize]
	public class UserController : ApiController
	{
		private readonly IUserManager _userManager;
		private readonly IViewManager<GetUserView> _getUserView;
		private readonly IViewManager<GetUsersView> _getUsersView;

		public UserController(IUserManager userManager,
			IViewManager<GetUserView> getUserView,
			IViewManager<GetUsersView> getUsersView)
		{
			_userManager = userManager;
			_getUserView = getUserView;
			_getUsersView = getUsersView;
        }

		[Route(""), HttpGet]
		public List<UserProfile> Get()
		{
			return _getUsersView.Load().UserProfiles;
		}

		[Route("{id}"), HttpGet]
		public UserProfile Get(string id)
		{
			return _getUserView.Load(id).UserProfile;
		}

		[Route(""), HttpPost]
		[AllowAnonymous]
		public async Task<string> Post([FromBody]Models.Post.UserRegister userRegister)
		{
			var user = new UserIdentity
			{
				UserName = userRegister.Email,
				Email = userRegister.Email
			};

			await _userManager.CreateAsync(user, userRegister.Password);

			return user.Id;
		}

		[Route(""), HttpPut]
		public async Task Put(string id, [FromBody]Models.Put.UserUpdate userUpdate)
		{
			var user = await _userManager.FindByIdAsync(id);

			user.UserName = userUpdate.Email;
			user.Email = userUpdate.Email;
			user.Name = userUpdate.FirstName;
			user.Lastname = userUpdate.LastName;

			await _userManager.UpdateAsync(user);
		}

		[Route(""), HttpDelete]
		public async Task Delete(string id)
		{
			if (User.GetId() != id)
				throw new UnauthorizedAccessException("You cannot delete a user other than yours");

			var user = await _userManager.FindByIdAsync(id);

			await _userManager.DeleteAsync(user);
		}
	}
}
