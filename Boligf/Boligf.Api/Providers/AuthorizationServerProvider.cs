using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Boligf.Api.Domain.Entities;
using Boligf.Api.Infrastructure;
using Boligf.Api.Play;
using Boligf.Api.Views.User;
using d60.Cirqus.Views.ViewManagers;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;

namespace Boligf.Api.Providers
{
	public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
		private const string AllowedOrigins = "*";
		private const string AsClientallowedorigin = "as:clientAllowedOrigin";
		private const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";

		private readonly IUserManager _userManager;
		private readonly IViewManager<GetUsersAssociationsView> _getUsersAssociationsView;

		public AuthorizationServerProvider(IUserManager userManager, IViewManager<GetUsersAssociationsView> getUsersAssociationsView)
		{
			_userManager = userManager;
			_getUsersAssociationsView = getUsersAssociationsView;
		}

		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			context.OwinContext.Set(AsClientallowedorigin, AllowedOrigins);
			context.Validated();

			return Task.FromResult<object>(null);
		}

		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			SetAllowedOriginsToResponseHeader(context);

			var user = await FindUserByUsernameAndPassword(context.UserName, context.Password);
			if (user == null)
			{
				context.SetError("invalid_grant", "The user name or password is incorrect.");
				return;
			}

			var ticket = CreateAuthenticationTicket(context, user.Id);

			context.Validated(ticket);
		}

		public override Task TokenEndpoint(OAuthTokenEndpointContext context)
		{
			foreach (var property in context.Properties.Dictionary)
			{
				context.AdditionalResponseParameters.Add(property.Key, property.Value);
			}

			return Task.FromResult<object>(null);
		}

		private void SetAllowedOriginsToResponseHeader(OAuthGrantResourceOwnerCredentialsContext context)
		{
			var allowedOrigin = context.OwinContext.Get<string>(AsClientallowedorigin) ?? AllowedOrigins;
			if (!context.OwinContext.Response.Headers.ContainsKey(AccessControlAllowOrigin))
			{
				context.OwinContext.Response.Headers.Add(AccessControlAllowOrigin, new[] { allowedOrigin });
			}
		}

		private async Task<UserIdentity> FindUserByUsernameAndPassword(string username, string password)
		{
			return await _userManager.FindAsync(username, password);
		}

		private AuthenticationTicket CreateAuthenticationTicket(OAuthGrantResourceOwnerCredentialsContext context, string userId)
		{
			var claimIdentity = CreateClaimsIdentity(context, userId);
			var authProperties = CreateAuthenticationProperties(context, claimIdentity, userId);
			var ticket = new AuthenticationTicket(claimIdentity, authProperties);

			return ticket;
		}

		private ClaimsIdentity CreateClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context, string userId)
		{
			var identity = new ClaimsIdentity(context.Options.AuthenticationType);
			identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
			identity.AddClaim(new Claim(ClaimTypes.Email, context.UserName));
			identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));

			// TODO: get users roles from user and insert here
			//identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

			return identity;
		}

		private AuthenticationProperties CreateAuthenticationProperties(OAuthGrantResourceOwnerCredentialsContext context, ClaimsIdentity identity, string userId)
		{
			var userAssociationsData = _getUsersAssociationsView.Load();
			var userAssociations = userAssociationsData.GetUsersAssociations(userId);

			if (!userAssociations.Any())
			{
				throw new InvalidDataException("User cannot exists without an association, as it is right now");
			}

			var roles = identity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
			var props = new AuthenticationProperties(
				new Dictionary<string, string>
				{
					{ "userName", context.UserName },
					{ "userId", userId },
					{ "associationId", userAssociations[0].Id },
					{ "roles", JsonConvert.SerializeObject(roles.Select(x => x.Value)) }
				}
			);

			return props;
		}
	}
}