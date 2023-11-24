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
            
            // Configure Antiforgery
            builder.Services.AddAntiforgery(options => {
                options.HeaderName = "X-CSRF-TOKEN";
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
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
                await next();
            });
            
            //app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            UseAuthentication(app);

            app.MapControllerRoute(name: "default", pattern: "{controller=Account}/{action=Login}/{id?}");
            app.MapControllers();
            
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