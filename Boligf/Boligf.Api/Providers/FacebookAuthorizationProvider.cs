using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Facebook;

namespace Boligf.Api.Providers
{
	public class FacebookOAuthOptions : FacebookAuthenticationOptions
	{
		public FacebookOAuthOptions(IFacebookAuthenticationProvider facebookAuthenticationProvider)
		{
			AppId = "xxxxxx";
			AppSecret = "xxxxxx";
			Provider = facebookAuthenticationProvider;
		}
	}

	public class FacebookAuthorizationProvider : FacebookAuthenticationProvider
	{
		public override Task Authenticated(FacebookAuthenticatedContext context)
		{
			context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));

			return Task.FromResult<object>(null);
		}
	}
}