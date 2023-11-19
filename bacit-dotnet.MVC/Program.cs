using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;


using bacit_dotnet.MVC.Repositories;
using MySqlConnector;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore;

using bacit_dotnet.MVC.DataAccess;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace bacit_dotnet.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel(x => x.AddServerHeader = false);

            /*builder.Services.AddDefaultIdentity<IdentityUser>(
                    options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();*/
            
            // Add services to the container.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            // Configure the database connection.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddScoped<IDbConnection>(_ => new MySqlConnection(connectionString));
            
            builder.Services.AddScoped<IDbConnection>(_ =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                return new MySqlConnection(connectionString);
            });

            // Register your repository here.
            builder.Services.AddTransient<IServiceFormRepository, ServiceFormRepository>();
            builder.Services.AddTransient<ICheckListRepository, CheckListRepository>();
            

            builder.Services.AddTransient<IUserRepository, InMemoryUserRepository>();
            builder.Services.AddTransient<IUserRepository, SqlUserRepository>();
            builder.Services.AddTransient<IUserRepository, DapperUserRepository>();
            builder.Services.AddScoped<IUserRepository, EFUserRepository>();
                
            SetupDataConnections(builder);
            SetupAuthentication(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            UseAuthentication(app);

            app.MapControllerRoute(name: "default", pattern: "{controller=Account}/{action=Login}/{id?}");
            app.MapControllers();

            app.Run();

            builder.Services.AddAntiforgery(options => { options.HeaderName = "X-CSRF-TOKEN"; });

            WebHost.CreateDefaultBuilder(args)
                .ConfigureKestrel(c => c.AddServerHeader = false)
                .UseStartup<Startup>()
                .Build();
        }

        // Method to set up data connections
        private static void SetupDataConnections(WebApplicationBuilder builder)
        {
            // Registering ISqlConnector as a transient service
            builder.Services.AddTransient<ISqlConnector, SqlConnector>();

            // Configuring DbContext (Entity Framework Core) for data access
            builder.Services.AddDbContext<DataContext>(options =>
            {
                // Configuring the DbContext to use MySQL with connection string and server version
                options.UseMySql(
                    builder.Configuration.GetConnectionString("DefaultConnection"), // Getting the connection string
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")) // Auto-detecting server version
                );
            });
        }

        // Method to configure middleware for authentication and authorization
        private static void UseAuthentication(WebApplication app)
        {
            // Configuring the middleware pipeline to use authentication
            app.UseAuthentication();

            // Configuring the middleware pipeline to use authorization
            app.UseAuthorization();
        }
        
        // Method to configure authentication settings
        private static void SetupAuthentication(WebApplicationBuilder builder)
        {
            // Setting up configuration options for Identity
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Setting default lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
            });

            // Adding Identity services
            builder.Services
                .AddIdentityCore<IdentityUser>() // Adding Identity services for IdentityUser
                .AddRoles<IdentityRole>() // Adding support for roles
                .AddEntityFrameworkStores<DataContext>() // Setting up Entity Framework stores
                .AddSignInManager() // Adding sign-in manager
                .AddDefaultTokenProviders(); // Adding default token providers for password reset, etc.

            // Adding authentication services
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme; // Setting the default authentication scheme
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme; // Setting the default sign-in scheme
            }).AddIdentityCookies(o => { }); // Adding Identity cookies for authentication

            // Adding the AuthMessageSender (email sender) as a transient service
            builder.Services.AddTransient<IEmailSender, AuthMessageSender>();
        }


        // Implementing the IEmailSender interface to handle email sending
        public class AuthMessageSender : IEmailSender
        {
            // Implementing the method required by the IEmailSender interface
            public Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                // Printing the email, subject, and htmlMessage to the console (for demonstration purposes)
                Console.WriteLine(email);
                Console.WriteLine(subject);
                Console.WriteLine(htmlMessage);

                // Returning a completed task since this method doesn't perform actual email sending
                return Task.CompletedTask;
            }
        }
        
        private static async Task SetRoles(RoleManager<IdentityRole> roleManager)
        {
            // Array containing role names to be checked and created
            string[] roleNames = { "Admin", "ServiceSenterAnsatt", "Mekaniker" };

            // Loop through each role name in the array
            foreach (var roleName in roleNames)
            {
                // Check if the role already exists in the RoleManager
                var roleExist = await roleManager.RoleExistsAsync(roleName);

                // If the role doesn't exist, create it
                if (!roleExist)
                {
                    // Creating a new IdentityRole with the roleName and adding it via RoleManager
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }

    }
}