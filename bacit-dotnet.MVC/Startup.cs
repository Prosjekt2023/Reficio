using Microsoft.AspNetCore.Builder;

namespace bacit_dotnet.MVC
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {

            // Security set up of HTTP headers
            // Setting up various security HTTP headers to enhance web application security

            app.Use(async (context, next) =>
            {

                // Adding X-XSS-Protection header to prevent cross-site scripting attacks
                context.Response.Headers.Add("X-Xss-Protection", "1");

                // Adding X-Frame-Options header to prevent clickjacking attacks
                context.Response.Headers.Add("X-Frame-Options", "DENY");

                // Adding Referrer-Policy header to control referrer information sent with requests
                context.Response.Headers.Add("Referrer-Policy", "no-referrer");

                // Adding X-Content-Type-Options header to prevent MIME-sniffing attacks
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");

                // Adding Content-Security-Policy header to define approved sources for content
                context.Response.Headers.Add(
                    "Content-Security-Policy",
                    "default-src 'self'; " +
                    "img-src 'self'; " +
                    "font-src 'self'; " +
                    "style-src 'self'; " +
                    "script-src 'self'" +
                    "frame-src 'self';" +
                    "connect-src 'self';");

                await next(); // Passing the request to the next middleware
            });

            // To enforce HTTPS connection
            // Adding HTTP Strict Transport Security (HSTS) middleware to enforce HTTPS connections

            app.UseHsts();

            //// For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
            // A commented-out reference link for additional configuration information
        }
    }
}
