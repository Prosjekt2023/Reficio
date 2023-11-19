using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

// Other necessary namespaces...

namespace bacit_dotnet.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.WebHost.ConfigureKestrel(x => x.AddServerHeader = false);

            // Configuration for Identity (commented out for reference)
            /*builder.Services.AddDefaultIdentity<IdentityUser>(
                    options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();*/
            
            // Adding services to the container
            builder.Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
            });

            // Configuring the database connection
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddScoped<IDbConnection>(_ => new MySqlConnection(connectionString));
            
            // Registering repositories
            // (Transient lifetime services)
            builder.Services.AddTransient<IServiceFormRepository, ServiceFormRepository>();
            builder.Services.AddTransient<ICheckListRepository, CheckListRepository>();
            builder.Services.AddTransient<IUserRepository, InMemoryUserRepository>();
            builder.Services.AddTransient<IUserRepository, SqlUserRepository>();
            builder.Services.AddTransient<IUserRepository, DapperUserRepository>();
            builder.Services.AddScoped<IUserRepository, EFUserRepository>();
                
            SetupDataConnections(builder);
            SetupAuthentication(builder);

            var app = builder.Build();

            // Configuring the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseStaticFiles();
            app.UseRouting();
            UseAuthentication(app);

            app.MapControllerRoute(name: "default", pattern: "{controller=Account}/{action=Login}/{id?}");
            app.MapControllers();
            app.Run();

            // Adding Antiforgery service
            builder.Services.AddAntiforgery(options => { options.HeaderName = "X-CSRF-TOKEN"; });

            // Creating and building the web host
            WebHost.CreateDefaultBuilder(args)
                .ConfigureKestrel(c => c.AddServerHeader = false)
                .UseStartup<Startup>()
                .Build();
        }

        // Method to set up data connections
        private static void SetupDataConnections(WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ISqlConnector, SqlConnector>();

            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")));
            });
        }

        // Method to configure authentication
        private static void SetupAuthentication(WebApplicationBuilder builder)
        {
            // Configuring IdentityOptions
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

        // Class to send emails asynchronously
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

        // Method to set roles (async)
        private static async Task SetRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "ServiceSenterAnsatt", "Mekaniker" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }
        }
    }
}
