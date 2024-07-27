using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using SistemaTickets.Data;
using System.Text;

namespace SistemaTickets.Extensions
{
    public static class applicationExtensions
    {

       public static void addCorsApplication(this IServiceCollection service)
        {
            service.AddCors(options =>
            {
                options.AddPolicy("Dev", app =>
                {
                    app.WithOrigins("http://localhost:4200")
                       .AllowAnyMethod()
                       .AllowAnyHeader().
                       AllowCredentials();
                });
            });
        }

       public static void configurationConnection(this IServiceCollection service,IConfiguration configuration) {

            string strlConnection = configuration.GetConnectionString("connectionDefault");
            service.AddDbContext<appDbContext>(options => options.UseMySql(strlConnection, ServerVersion.AutoDetect(strlConnection)));
        }

       public static void addJwtApplication(this IServiceCollection service,IConfiguration configuration) {

            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"])),
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = false
                };
            });

        }

       public static void comprenssionData(this IServiceCollection service)
        {
            service.AddResponseCompression(opt =>
            {
                opt.Providers.Add<BrotliCompressionProvider>();
                opt.Providers.Add<GzipCompressionProvider>();
            });
        }


       public static void fileServerApplication( WebApplication app,IConfiguration configuration)
        {

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(configuration["pathFile:path"]),
                RequestPath = "/files",
                EnableDirectoryBrowsing = true
            });
        }
    }
}
