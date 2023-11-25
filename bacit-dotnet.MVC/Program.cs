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
            // Creates a new WebApplication builder with the arguments passed to the program.
            // This typically includes configurations and services that the application will use.
            var builder = WebApplication.CreateBuilder(args);
            // Configures the Kestrel server, which is the web server included with ASP.NET Core.
            // Specifically, this configuration sets not to include the server header in the response,
            // which is a common security practice to minimize information about the server infrastructure.
            builder.WebHost.ConfigureKestrel(x => x.AddServerHeader = false);
            
            // Adds MVC controllers with their associated views to the application's services.
            // This includes a configuration to add a filter that automatically validates anti-forgery tokens for all incoming requests
            // to prevent cross-site request forgery (CSRF) attacks. This filter ensures that your application validates tokens on POST, PUT, DELETE, and PATCH requests.
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            // Configure the database connection.
            // Retrieves the connection string named "DefaultConnection" from the application's configuration,
            // which is typically defined in the appsettings.json file or through environment variables.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddScoped<IDbConnection>(_ => new MySqlConnection(connectionString));
            
            builder.Services.AddScoped<IDbConnection>(_ =>
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                return new MySqlConnection(connectionString);
            });

            // Register your repository here.
            
            // Adds a transient service of type IServiceFormRepository to the dependency injection container,
            // with the concrete implementation being ServiceFormRepository. Transient services are created each time they are requested
            builder.Services.AddTransient<IServiceFormRepository, ServiceFormRepository>();
            builder.Services.AddTransient<ICheckListRepository, CheckListRepository>();
            builder.Services.AddTransient<IUserRepository, InMemoryUserRepository>();
            builder.Services.AddTransient<IUserRepository, SqlUserRepository>();
            builder.Services.AddTransient<IUserRepository, DapperUserRepository>();
            builder.Services.AddScoped<IUserRepository, EFUserRepository>();
                
            
            // Calls a method to set up the data connections for the application.
            // This would typically involve configuring the database contexts, connection strings,
            SetupDataConnections(builder);
            // Calls a method to set up the authentication services for the application.
            // This includes setting up the identity management, authentication schemes, 
            // cookie settings, and any other security-related services or configurations
            SetupAuthentication(builder);
            
            // Adds and configures the anti-forgery service which is used to generate anti-forgery tokens.
            // These tokens are used to prevent Cross-Site Request Forgery (CSRF) attacks.
            // Here, the header name for the token in HTTP requests is set to "X-CSRF-TOKEN".
            builder.Services.AddAntiforgery(options => {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            // Builds the web application using the configurations and services defined in 'builder'.
            // This includes compiling the middleware pipeline and making the application ready to start accepting requests.
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            // Checks if the application is not running in the Development environment.
            if (!app.Environment.IsDevelopment())
            {
                // Configures the app to use a custom error handler page located at '/Home/Error'.
                // This is where the application will redirect for any unhandled exceptions in non-Development environments.
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // Enforces the use of HTTPS Strict Transport Security (HSTS) by adding the HSTS header to responses.
                // This header tells browsers to only access the application over HTTPS for a period of time (default is 30 days).
                // HSTS is a security feature to force clients to use HTTPS, thus reducing the risk of man-in-the-middle attacks.
                app.UseHsts();
            }
            
            // Custom middleware to add various security headers to every HTTP response.
            app.Use(async (context, next) =>
            {
                // Adds the X-XSS-Protection header with a value of 1 to enable the Cross-Site Scripting (XSS) filter in the browser.
                context.Response.Headers.Add("X-Xss-Protection", "1");
                // Adds the X-Frame-Options header set to DENY to prevent the page from being displayed in a frame or iframe, 
                // which can protect against clickjacking attacks.
                context.Response.Headers.Add("X-Frame-Options", "DENY");
                // Adds the Referrer-Policy header set to no-referrer, which will not send the HTTP referrer header with outgoing requests.
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");
                // Adds the X-Content-Type-Options header set to nosniff, which instructs the browser to not guess (sniff) the MIME type,
                // and strictly follow the declared content-type.
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                // Adds a strict Content-Security-Policy (CSP) header, which defines approved sources of content that the browser may load.
                // It's configured here to only allow content from the same origin ('self').
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
            
            // The HTTPS redirection middleware is commented out. When active, it redirects all HTTP requests to HTTPS.
            //app.UseHttpsRedirection();
            
            // Enables serving static files (like images, CSS, JavaScript files) from the wwwroot folder.
            app.UseStaticFiles();
            // Adds middleware for routing. This is necessary to map incoming HTTP requests to appropriate route handlers.
            app.UseStaticFiles();
            // "Måt finne ut av
            app.UseRouting();

            UseAuthentication(app);

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
            //Setup for Authentication
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Default Lockout settings.
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