using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
	public class UMDatabaseContextFactory : IDesignTimeDbContextFactory<UMDatabaseContext>
	{
		public UMDatabaseContext CreateDbContext(string[] args)
		{
			var conString = "Server=localhost;Port=3306;Database=usersdb;User=root;Password=rootpwd";
			// Create and configure DbContextOptionsBuilder
			var optionsBuilder = new DbContextOptionsBuilder<UMDatabaseContext>();
			optionsBuilder.UseMySql(conString, ServerVersion.AutoDetect(conString));

			return new UMDatabaseContext(optionsBuilder.Options);
		}
	}

}