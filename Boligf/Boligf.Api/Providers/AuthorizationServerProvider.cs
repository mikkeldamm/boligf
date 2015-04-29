using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json;

namespace Boligf.Api.Providers
{
	public class AuthorizationServerProvider : OAuthAuthorizationServerProvider
	{
		public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
		{
			context.OwinContext.Set("as:clientAllowedOrigin", "*");
			context.Validated();

			return Task.FromResult<object>(null);
		}

		public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
		{
			var allowedOrigin = context.OwinContext.Get<string>("as:clientAllowedOrigin") ?? "*";

			if (!context.OwinContext.Response.Headers.ContainsKey("Access-Control-Allow-Origin"))
			{
				context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { allowedOrigin });
			}

			//using (AuthRepository _repo = new AuthRepository())
			//{
			//	IdentityUser user = await _repo.FindUser(context.UserName, context.Password);

			//	if (user == null)
			//	{
			//		context.SetError("invalid_grant", "The user name or password is incorrect.");
			//		return;
			//	}
			//}

			var identity = new ClaimsIdentity(context.Options.AuthenticationType);
			identity.AddClaim(new Claim(ClaimTypes.Name, context.UserName));
			identity.AddClaim(new Claim(ClaimTypes.Role, "Admin")); // TODO: get users roles and insert here
			identity.AddClaim(new Claim(ClaimTypes.Role, "User"));

			var roles = identity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();

			var props = new AuthenticationProperties(
				new Dictionary<string, string>
				{
					{ "userName", context.UserName },
					{ "roles", JsonConvert.SerializeObject(roles.Select(x=>x.Value)) }
				}
			);

			var ticket = new AuthenticationTicket(identity, props);

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
	}
}