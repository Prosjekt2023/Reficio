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
    /// <summary>
    /// The Program class is the main entry point for an ASP.NET Core web application.
    /// </summary>
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

            // Configure database connection
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
                
            
            // Setup data connections and authentication
            SetupDataConnections(builder);
            SetupAuthentication(builder);
            
            // Adds and configures the anti-forgery service which is used to generate anti-forgery tokens.
            builder.Services.AddAntiforgery(options => {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            // Build the application
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

            // Map controller routes
            app.MapControllerRoute(name: "default", pattern: "{controller=Account}/{action=Login}/{id?}");
            app.MapControllers();
            // Runs the application. This is the call that starts listening for incoming HTTP requests.
            app.Run();
        }

        
        /// <summary>
        /// Configures and sets up the data connections for the web application.
        /// </summary>
        /// <remarks>
        /// This method is responsible for registering services related to database connectivity and operations.
        /// It adds the necessary DbContext and configures it for use with a MySQL database.
        /// </remarks>
        /// <param name="builder">The WebApplicationBuilder instance used to configure and build the application.</param>
        private static void SetupDataConnections(WebApplicationBuilder builder)
        {
            // Registers the ISqlConnector interface with its concrete implementation SqlConnector.
            // The service is registered with a transient lifetime, meaning a new instance will be created each time it is injected
            builder.Services.AddTransient<ISqlConnector, SqlConnector>();
            
            // Adds and configures the DataContext for Entity Framework Core.
            // Configures the DataContext to use MySQL with the connection string from the application configuration.
            // The MySQL server version is auto-detected for compatibility.
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
            });

        }

        
        /// <summary>
        /// Configures the authentication and authorization middleware for the web application.
        /// </summary>
        /// <remarks>
        /// This method is critical for enforcing security in the application. It enables the middleware 
        /// that processes authentication and authorization headers in incoming HTTP requests. 
        /// Authentication middleware identifies the user making the request, while authorization middleware 
        /// checks if the authenticated user has the necessary permissions to access the requested resources.
        /// </remarks>
        /// <param name="app">The WebApplication instance representing the running application.</param>
        private static void UseAuthentication(WebApplication app)
        {
            // Enables authentication middleware, which processes authentication data in the request headers 
            // to identify the user on each request.
            app.UseAuthentication();
            app.UseAuthorization();
        }
        
        /// <summary>
        /// Configures authentication services and settings for the web application.
        /// </summary>
        /// <remarks>
        /// This method sets up the necessary components for handling user authentication in the application.
        /// It configures the identity options, adds the identity services, and specifies the authentication 
        /// schemes. The method is integral for managing user accounts, roles, and how users sign in.
        /// </remarks>
        /// <param name="builder">The WebApplicationBuilder used to configure services for the application.</param>
        private static void SetupAuthentication(WebApplicationBuilder builder)
        {
            // Configures the IdentityOptions for the application.
            // This includes lockout settings, sign-in requirements, and user options.
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // Sets the default lockout duration to 5 minutes and maximum failed access attempts to 5.
                // Disables lockout for new users.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = false;
                // Disables requirements for confirmed phone number, email, and confirmed accounts for signing in.
                // Requires a unique email for each user.
                options.SignIn.RequireConfirmedPhoneNumber = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
            });
            // Adds and configures the Identity services.
            // This includes setting up roles, entity framework stores for data persistence,
            // sign-in management, and token providers.
            builder.Services
                .AddIdentityCore<IdentityUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();
            
            // Configures the authentication schemes for the application.
            builder.Services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;

            }).AddIdentityCookies(o => { });
            
            // Registers a transient service for sending emails, using the AuthMessageSender implementation.
            builder.Services.AddTransient<IEmailSender, AuthMessageSender>();
        }
        
        /// <summary>
        /// A class that implements the IEmailSender interface for sending emails.
        /// </summary>
        /// <remarks>
        /// This implementation is a basic example and only writes the email details to the console.
        /// In a production scenario, this method should be replaced with actual email sending logic,
        /// potentially using an email service provider.
        /// </remarks>
        public class AuthMessageSender : IEmailSender
        {
            /// <summary>
            /// Asynchronously sends an email.
            /// </summary>
            /// <param name="email">The destination email address.</param>
            /// <param name="subject">The subject of the email.</param>
            /// <param name="htmlMessage">The HTML message body of the email.</param>
            /// <returns>A task that represents the asynchronous operation.</returns>
            /// <remarks>
            /// This implementation is for demonstration purposes and does not actually send an email.
            /// It writes the email details to the console. Replace this with actual email sending logic.
            /// </remarks>
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