using System;
using System.Threading.Tasks;
using Boligf.Api.Domain;
using Boligf.Api.Providers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Facebook;
using Microsoft.Owin.Security.Google;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Boligf.Api.Configuration
{
	public class AuthConfig
	{
		public static OAuthBearerAuthenticationOptions OAuthBearerOptions { get; private set; }
		public static GoogleOAuth2AuthenticationOptions GoogleAuthOptions { get; private set; }
		public static FacebookAuthenticationOptions FacebookAuthOptions { get; private set; }

		public static string PublicClientId { get; private set; }

		public static void Setup(IAppBuilder app)
		{
			PublicClientId = "boligf";

			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			OAuthBearerOptions = new OAuthBearerAuthenticationOptions();
			
			var oAuthServerOptions = new OAuthAuthorizationServerOptions
			{
				TokenEndpointPath = new PathString("/token"),
				AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
				Provider = new AuthorizationServerProvider(),
				AllowInsecureHttp = true
			};

			app.UseOAuthAuthorizationServer(oAuthServerOptions);
			app.UseOAuthBearerAuthentication(OAuthBearerOptions);

			//Configure Google External Login
			GoogleAuthOptions = new GoogleOAuth2AuthenticationOptions()
			{
				ClientId = "xxxxxx",
				ClientSecret = "xxxxxx",
				Provider = new GoogleAuthorizationProvider()
			};

			app.UseGoogleAuthentication(GoogleAuthOptions);

			//Configure Facebook External Login
			FacebookAuthOptions = new FacebookAuthenticationOptions()
			{
				AppId = "xxxxxx",
				AppSecret = "xxxxxx",
				Provider = new FacebookAuthorizationProvider()
			};

			app.UseFacebookAuthentication(FacebookAuthOptions);


			//
			// Below is just test thingeling
			//
			var userManager = new UserManager<User>(new CustomUserStore());

			userManager.UserValidator = new UserValidator<User>(userManager)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			// Configure validation logic for passwords
			userManager.PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = false,
				RequireDigit = false,
				RequireLowercase = false,
				RequireUppercase = false,
			};
		}

		public class CustomUserStore : IUserStore<User>
		{
			public void Dispose()
			{
				throw new System.NotImplementedException();
			}

			public Task CreateAsync(User user)
			{
				throw new System.NotImplementedException();
			}

			public Task UpdateAsync(User user)
			{
				throw new System.NotImplementedException();
			}

			public Task DeleteAsync(User user)
			{
				throw new System.NotImplementedException();
			}

			public Task<User> FindByIdAsync(string userId)
			{
				throw new System.NotImplementedException();
			}

			public Task<User> FindByNameAsync(string userName)
			{
				throw new System.NotImplementedException();
			}
		}
	}
}