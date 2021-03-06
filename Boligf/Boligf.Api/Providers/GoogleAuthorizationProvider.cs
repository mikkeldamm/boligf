﻿using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Owin.Security.Google;

namespace Boligf.Api.Providers
{
	public class GoogleOAuthOptions : GoogleOAuth2AuthenticationOptions
	{
		public GoogleOAuthOptions(IGoogleOAuth2AuthenticationProvider googleOAuth2AuthenticationProvider)
		{
			ClientId = "xxxxxx";
			ClientSecret = "xxxxxx";
			Provider = googleOAuth2AuthenticationProvider;
		}
	}

	public class GoogleAuthorizationProvider : IGoogleOAuth2AuthenticationProvider
	{
		public void ApplyRedirect(GoogleOAuth2ApplyRedirectContext context)
		{
			context.Response.Redirect(context.RedirectUri);
		}

		public Task Authenticated(GoogleOAuth2AuthenticatedContext context)
		{
			context.Identity.AddClaim(new Claim("ExternalAccessToken", context.AccessToken));
			return Task.FromResult<object>(null);
		}

		public Task ReturnEndpoint(GoogleOAuth2ReturnEndpointContext context)
		{
			return Task.FromResult<object>(null);
		}
	}
}