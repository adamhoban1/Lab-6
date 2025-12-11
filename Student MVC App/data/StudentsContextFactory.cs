using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Student_MVC_App.Data;

namespace Student_MVC_App.data
{
    public class StudentsContextFactory : IDesignTimeDbContextFactory<StudentsContext>
    {
        public StudentsContext CreateDbContext(string[] args)
        {
            var envPath = Directory.GetCurrentDirectory();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(envPath)
                .AddJsonFile("appsettings.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found. Add it to appsettings.json.");
            }

            var optionsBuilder = new DbContextOptionsBuilder<StudentsContext>();
            optionsBuilder.UseSqlite(connectionString);

            return new StudentsContext(optionsBuilder.Options);
        }
    }
}