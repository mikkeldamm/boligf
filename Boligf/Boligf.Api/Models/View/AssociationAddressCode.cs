using System.Collections.Generic;

namespace Boligf.Api.Models.View
{
	public class AssociationAddressCode : AssociationAddress
	{
		public string AssociationId { get; set; }
		public List<string> Codes { get; set; }

		public AssociationAddressCode()
		{
			Codes = new List<string>();
		}
	}
}