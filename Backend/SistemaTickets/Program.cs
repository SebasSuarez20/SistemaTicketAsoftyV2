
using SistemaPoscloud.Services.ServicesConfig.ExtensionsConfig;
using SistemaTickets.Extensions;
using SistemaTickets.Services.SignalR;

var builder = WebApplication.CreateBuilder(args);


builder.Services.configurationConnection(builder.Configuration);
builder.Services.addCorsApplication();

builder.Services.addJwtApplication(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddAplicationServices(builder.Configuration);

// Configuraci�n de Swagger/OpenAPI
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();


// Configuraci�n del acceso al contexto HTTP
builder.Services.AddHttpContextAccessor();

#region compresionDeRespuesta
builder.Services.comprenssionData();
#endregion



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
applicationExtensions.fileServerApplication(app,builder.Configuration);

#region se Agrega el middleware para que comprima todo tipo de respuesta
app.UseResponseCompression();
#endregion

app.UseCors("Dev");
app.UseHttpsRedirection();
app.UseAuthorization();
// Configuraci�n de CORS para permitir el acceso desde http://localhost:4200
app.MapHub<HubConnection>("/realTime");

// Configuraci�n de enrutamiento para controladores
app.MapControllers();

app.Run();
