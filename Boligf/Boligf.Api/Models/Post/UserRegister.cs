using System.ComponentModel.DataAnnotations;

namespace Boligf.Api.Models.Post
{
	public class UserRegister
	{
		[Required]
		public string Email { get; set; }
		[Required]
		public string Password { get; set; }
	}
}