using System.Data.Entity;
using System.Diagnostics;
using Boligf.Api.Domain;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Boligf.Api.Context
{
	public class UserContext : IdentityDbContext<User>
	{
		public UserContext() : base(Connection.DataConnectionName)
		{
			#if DEBUG
				Database.Log = s => Debug.Write(s);
			#endif
		}

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
		}
	}
}