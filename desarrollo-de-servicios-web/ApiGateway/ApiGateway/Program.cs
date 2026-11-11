var builder = WebApplication.CreateBuilder(args);

// 1. Configurar YARP Reverse Proxy
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// 2. Configurar CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseCors("PermitirFrontend");

// 3. Endpoint de bienvenida en la raíz
app.MapGet("/", () => Results.Ok(new
{
    Sistema = "AdoptaMas API - Microservicios Gateway",
    Version = "1.0.0 LTS",
    PuertoGateway = 6000,
    Rutas = new[]
    {
        "POST /api/auth/login",
        "POST /api/auth/registro",
        "GET  /api/mascotas",
        "GET  /api/albergues",
        "POST /api/solicitudes"
    },
    Estado = "OPERATIVO",
    DocumentacionScalar = "http://localhost:6001/scalar/v1"
}));

// 4. Mapear YARP
app.MapReverseProxy();

app.Run();
