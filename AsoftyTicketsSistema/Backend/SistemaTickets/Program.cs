using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using SistemaPoscloud.Services.ServicesConfig.ExtensionsConfig;
using SistemaTickets.Data;
using SistemaTickets.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de la cadena de conexi�n a la base de datos MySQL
var connectionString = builder.Configuration.GetConnectionString("connectionDefault");
builder.Services.AddDbContext<appDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configuraci�n de CORS para permitir el acceso desde http://localhost:4200
builder.Services.AddCors(options =>
{
    options.AddPolicy("Dev", app =>
    {
        app.WithOrigins("http://localhost:4200")
           .AllowAnyMethod()
           .AllowAnyHeader().
           AllowCredentials();
    });
});



// Configuraci�n de la autenticaci�n JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false
    };
});

builder.Services.AddControllers();
builder.Services.AddAplicationServices(builder.Configuration); // Supongo que este m�todo agrega tus servicios de aplicaci�n

// Configuraci�n de Swagger/OpenAPI
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

// Configuraci�n del acceso al contexto HTTP
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configuraci�n de Swagger/OpenAPI en el entorno de desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Obtenci�n de la ruta de la carpeta de archivos desde appSettings
var path = builder.Configuration["pathFile:path"];
var pathCompany = builder.Configuration["pathFile:pathCompany"];

// Creaci�n de la carpeta si no existe
if (!Directory.Exists(path))
    Directory.CreateDirectory(path);

if (!Directory.Exists(pathCompany))
    Directory.CreateDirectory(pathCompany);

// Configuraci�n del servidor de archivos est�ticos
app.UseFileServer(new FileServerOptions
{
    FileProvider = new PhysicalFileProvider(path),
    RequestPath = "/files",
    EnableDirectoryBrowsing = true
});

app.UseHttpsRedirection();
app.UseAuthorization();


// Configuraci�n de CORS para permitir el acceso desde http://localhost:4200
app.UseCors("Dev");


app.MapHub<HubConnection>("/realTime");

// Configuraci�n de enrutamiento para controladores
app.MapControllers();

app.Run();
