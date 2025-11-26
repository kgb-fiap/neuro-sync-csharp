using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Oracle.EntityFrameworkCore;

namespace NeuroSync.Infrastructure.Persistence
{
    public class NeuroSyncDbContextFactory : IDesignTimeDbContextFactory<NeuroSyncDbContext>
    {
        public NeuroSyncDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: true)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<NeuroSyncDbContext>();
            optionsBuilder.UseOracle(configuration.GetConnectionString("DefaultConnection") ?? "User Id=NEURO_SYNC;Password=senha;Data Source=localhost:1521/XEPDB1");

            return new NeuroSyncDbContext(optionsBuilder.Options);
        }
    }
}
