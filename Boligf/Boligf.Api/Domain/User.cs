using d60.Cirqus.Aggregates;

namespace Boligf.Api.Domain
{
	public class User : AggregateRoot
	{
		public string Firstname { get; set; }
		public string Lastname { get; set; }

		public string Email { get; set; }
		public string PasswordHash { get; set; }
	}
}