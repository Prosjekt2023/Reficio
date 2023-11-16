using bacit_dotnet.MVC.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration; // Add this namespace
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using bacit_dotnet.MVC.DataAccess;
using bacit_dotnet.MVC.Models;
using bacit_dotnet.MVC.Models.ApplicationUser;
using bacit_dotnet.MVC.DataAccess;

namespace bacit_dotnet.MVC.DataAccess
{
    public class DataContext : IdentityDbContext<ReficioApplicationUser>
    {
        private readonly IConfiguration _configuration; // Add a field to store the configuration

        public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration; // Initialize the configuration
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Get the connection string from appsettings.json using the IConfiguration
                var connectionString = _configuration.GetConnectionString("DefaultConnection");

                // Configure the database connection
                optionsBuilder.UseMySql(connectionString, new MariaDbServerVersion(new Version(10, 5, 12)));
            }
        }
    }
}