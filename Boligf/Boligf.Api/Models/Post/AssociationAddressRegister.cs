namespace Boligf.Api.Models.Post
{
	public class AssociationAddressRegister
	{
		public string Id { get; set; }
		public string Streetname { get; set; }
		public string No { get; set; }
		public string Floor { get; set; }
		public string Door { get; set; }
		public string Zip { get; set; }
		public string City { get; set; }
		public string Latitude { get; set; }
		public string Longitude { get; set; }
		public string FullAddress { get; set; }
	}
}