using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Boligf.Api.Domain
{
	public class User : IdentityUser
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string Lastname { get; set; }
	}
}