using System;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;

namespace Boligf.Api.Providers
{
	public class AuthorizationServerOptions : OAuthAuthorizationServerOptions
	{
		public AuthorizationServerOptions(IOAuthAuthorizationServerProvider oAuthAuthorizationServerProvider)
		{
			TokenEndpointPath = new PathString("/token"); // Move to appsettings
			AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30); // Move to appsettings
			AllowInsecureHttp = true; // Move to appsettings
			Provider = oAuthAuthorizationServerProvider;
		}
	}
}