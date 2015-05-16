using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Boligf.Api.Domain.Entities
{
	public class UserIdentity : IdentityUser
	{
		public string Name { get; set; }
		public string Lastname { get; set; }

		public UserIdentity()
		{

		}
	}
}