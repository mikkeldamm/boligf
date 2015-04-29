using d60.Cirqus.Aggregates;
using Microsoft.AspNet.Identity;

namespace Boligf.Api.Domain
{
	public class User : AggregateRoot, IUser
	{
		public string Email { get; set; }
		public string UserName { get; set; }
	}
}