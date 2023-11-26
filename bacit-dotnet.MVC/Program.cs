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
        /// <summary>
        /// The main entry point of the application.
        /// </summary>
        /// <param name="args">Command line arguments passed to the program.</param>
        public static void Main(string[] args)
        {
            // Initialize the WebApplication builder
            var builder = WebApplication.CreateBuilder(args);
            // Configure the Kestrel web server
            builder.WebHost.ConfigureKestrel(x => x.AddServerHeader = false);
            
            // Add MVC controllers with views and configure anti-forgery measures
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            // Registering various services with dependency injection container
            builder.Services.AddTransient<IServiceFormRepository, ServiceFormRepository>(); 
            builder.Services.AddTransient<ICheckListRepository, CheckListRepository>();
            builder.Services.AddTransient<IUserRepository, InMemoryUserRepository>();
            builder.Services.AddTransient<IUserRepository, SqlUserRepository>();
            builder.Services.AddTransient<IUserRepository, DapperUserRepository>();
            builder.Services.AddScoped<IUserRepository, EFUserRepository>();
                
            
            // Setup data connections and authentication
            SetupDataConnections(builder);
            SetupAuthentication(builder);
            
            // Adds and configures the anti-forgery service which is used to generate anti-forgery tokens.
            builder.Services.AddAntiforgery(options => {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            // Build the application
            // This step builds the application using the configurations and services defined in 'builder'.
            // 'app' will be an instance of the application that can be used to run and handle web requests.
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                // Configures the app to use a custom error handler page located at '/Home/Error'.
                // This is where the application will redirect for any unhandled exceptions in non-Development environments.
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            // Custom middleware to add various security headers to every HTTP response.
            app.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1");
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                context.Response.Headers.Add(
                    "Content-Security-Policy",
                    "default-src 'self'; " +
                    "img-src 'self'; " +
                    "font-src 'self'; " +
                    "style-src 'self'; " +
                    "script-src 'self'" +
                    "frame-src 'self';" +
                    "connect-src 'self';");
                
                // Proceed to the next middleware in the pipeline.
                await next();
            });
            
            // Enables serving static files (like images, CSS, JavaScript files) from the wwwroot folder.
            app.UseStaticFiles();
            // Adds middleware for routing. This is necessary to map incoming HTTP requests to appropriate route handlers.
            app.UseStaticFiles();
            // "Måt finne ut av
            app.UseRouting();
            
            // Apply authentication
            UseAuthentication(app);

            // Map controller routes
            app.MapControllerRoute(name: "default", pattern: "{controller=Account}/{action=Login}/{id?}");
            app.MapControllers();
            // Runs the application. This is the call that starts listening for incoming HTTP requests.
            app.Run();
        }

        
        private static void SetupDataConnections(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ISqlConnector, SqlConnector>();
            
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
            });

        }
        
        private static void UseAuthentication(WebApplication app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
        
        
        private static void SetupAuthentication(WebApplicationBuilder builder)
        {
            
            builder.Services.Configure<IdentityOptions>(options =>
            {
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
            });
            
            builder.Services
                .AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();
            
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;

            }).AddIdentityCookies(o => { });
            
            builder.Services.AddTransient<IEmailSender, AuthMessageSender>();
        }
        
        public class AuthMessageSender : IEmailSender
        {
            
            public Task SendEmailAsync(string email, string subject, string htmlMessage)
            {
                Console.WriteLine(email);
                Console.WriteLine(subject);
                Console.WriteLine(htmlMessage);
                return Task.CompletedTask;
            }
        }
    }
}