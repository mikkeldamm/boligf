using Boligf.Api.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Boligf.Api.Configuration
{
	public class AuthConfig
	{
		public static string PublicClientId { get; private set; }

		private static OAuthBearerAuthenticationOptions _oAuthBearerAuthenticationOptions;
		private static GoogleOAuth2AuthenticationOptions _googleAuthOptions;
		private static OAuthAuthorizationServerOptions _oAuthAuthorizationServerOptions;
		private static FacebookOAuthOptions _facebookAuthOptions;

		public static void Setup(IAppBuilder app)
		{
			PublicClientId = "boligf";

			_oAuthBearerAuthenticationOptions = IoCContainer.Resolve<OAuthBearerAuthenticationOptions>();
			_oAuthAuthorizationServerOptions = IoCContainer.Resolve<OAuthAuthorizationServerOptions>();
			_googleAuthOptions = IoCContainer.Resolve<GoogleOAuth2AuthenticationOptions>();
			_facebookAuthOptions = IoCContainer.Resolve<FacebookOAuthOptions>();

			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			app.UseOAuthAuthorizationServer(_oAuthAuthorizationServerOptions);
			app.UseOAuthBearerAuthentication(_oAuthBearerAuthenticationOptions);
			app.UseGoogleAuthentication(_googleAuthOptions);
			app.UseFacebookAuthentication(_facebookAuthOptions);
		}
	}
}