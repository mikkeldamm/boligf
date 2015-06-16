using System.ComponentModel.DataAnnotations;

namespace Boligf.Api.Models.Post
{
	public class AssociationRegister
	{
		[Required]
		public string Name { get; set; }
		public string AddressId { get; set; }
		[Required]
		public string StreetAddress { get; set; }
		public string No { get; set; }
		public string Floor { get; set; }
		public string Door { get; set; }
		[Required]
		public string Zip { get; set; }
		[Required]
		public string City { get; set; }
		[Required]
		public string UserId { get; set; }
	}
}