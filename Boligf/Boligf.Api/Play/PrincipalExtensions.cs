using System.Security.Principal;
using Microsoft.AspNet.Identity;

namespace Boligf.Api.Play
{
	public static class PrincipalExtensions
	{
		public static string GetId(this IPrincipal principal)
		{
			return principal.Identity.GetUserId();
		}
	}
}