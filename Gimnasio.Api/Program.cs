using System.Text;
using Gimnasio.Datos;
using Gimnasio.Datos.Repositorios;
using Gimnasio.Negocio.Mapeos;
using Gimnasio.Negocio.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<GimnasioContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<ISocioRepositorio, SocioRepositorio>();
builder.Services.AddScoped<IMembresiaRepositorio, MembresiaRepositorio>();
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();

builder.Services.AddScoped<ISocioServicio, SocioServicio>();
builder.Services.AddScoped<IMembresiaServicio, MembresiaServicio>();
builder.Services.AddScoped<IAuthServicio, AuthServicio>();

builder.Services.AddAutoMapper(cfg => cfg.AddProfile<MappingProfile>());

var jwt = builder.Configuration.GetSection("Jwt");
var clave = jwt["Clave"];

if (string.IsNullOrWhiteSpace(clave) || clave.Length < 32)
    throw new InvalidOperationException(
        "Falta Jwt:Clave en los secretos de usuario o tiene menos de 32 caracteres.");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Emisor"],

            ValidateAudience = true,
            ValidAudience = jwt["Audiencia"],

            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(clave)),

            RoleClaimType = "role",
            NameClaimType = "email"
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddControllers();

builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Components ??= new OpenApiComponents();
        document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
        {
            ["Bearer"] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Pega solo el token, sin la palabra Bearer."
            }
        };

        foreach (var operation in document.Paths.Values.SelectMany(p => p.Operations!))
        {
            operation.Value.Security ??= [];
            operation.Value.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        }

        return Task.CompletedTask;
    });
});

var app = builder.Build();

app.MapOpenApi();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Gimnasio API v1");
    options.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();

app.UseAuthentication();   
app.UseAuthorization();

app.MapControllers();

app.Run();