using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Boligf.Api.Infrastructure;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;

namespace Boligf.Api.Providers
{
	public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
		private static string _allowedOrigins = "*";
		private static string _asClientallowedorigin = "as:clientAllowedOrigin";
		private static string _accessControlAllowOrigin = "Access-Control-Allow-Origin";

		private readonly IUserManager _userManager;

		public AuthorizationServerProvider(IUserManager userManager)
		{
			_userManager = userManager;
		}

		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			context.OwinContext.Set(_asClientallowedorigin, _allowedOrigins);
			context.Validated();

			return Task.FromResult<object>(null);
		}

		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			SetAllowedOriginsToResponseHeader(context);

			if (await DoesUserExist(context.UserName, context.Password))
			{
				context.SetError("invalid_grant", "The user name or password is incorrect.");
				return;
			}

			var ticket = CreateAuthenticationTicket(context);

			context.Validated(ticket);
		}

		private static void SetAllowedOriginsToResponseHeader(OAuthGrantResourceOwnerCredentialsContext context)
		{
			var allowedOrigin = context.OwinContext.Get<string>(_asClientallowedorigin) ?? _allowedOrigins;
			if (!context.OwinContext.Response.Headers.ContainsKey(_accessControlAllowOrigin))
			{
				context.OwinContext.Response.Headers.Add(_accessControlAllowOrigin, new[] { allowedOrigin });
			}
		}

		private async Task<bool> DoesUserExist(string username, string password)
		{
			var user = await _userManager.FindAsync(username, password);
			return user == null;
		}

		public override Task TokenEndpoint(OAuthTokenEndpointContext context)
		{
			foreach (var property in context.Properties.Dictionary)
			{
				context.AdditionalResponseParameters.Add(property.Key, property.Value);
			}

			return Task.FromResult<object>(null);
		}

		private AuthenticationTicket CreateAuthenticationTicket(OAuthGrantResourceOwnerCredentialsContext context)
		{
			var claimIdentity = CreateClaimsIdentity(context);
			var authProperties = CreateAuthenticationProperties(context, claimIdentity);
			var ticket = new AuthenticationTicket(claimIdentity, authProperties);

			return ticket;
		}

		private ClaimsIdentity CreateClaimsIdentity(OAuthGrantResourceOwnerCredentialsContext context)
		{
			var identity = new ClaimsIdentity(context.Options.AuthenticationType);
			identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));

			// TODO: get users roles from user and insert here
			//identity.AddClaim(new Claim(ClaimTypes.Role, "Admin"));

			return identity;
		}

		private AuthenticationProperties CreateAuthenticationProperties(OAuthGrantResourceOwnerCredentialsContext context,
			ClaimsIdentity identity)
		{
			var roles = identity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();
			var props = new AuthenticationProperties(
				new Dictionary<string, string>
				{
					{ "userName", context.UserName },
					{ "roles", JsonConvert.SerializeObject(roles.Select(x => x.Value)) }
				}
			);

			return props;
		}
	}
}