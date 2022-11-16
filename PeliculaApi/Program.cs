using System.Net;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PeliculaApi.Data;
using PeliculaApi.Helpers;
using PeliculaApi.PeliculaMapper;
using PeliculaApi.Repository;
using PeliculaApi.Repository.IRepository;

var builder = WebApplication.CreateBuilder(args);

#region "Configure Services"

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(x => x.UseSqlServer(connectionString));
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IPeliculaRepository, PeliculaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddAutoMapper(typeof(PeliculaMappers));

/* Agregar dependencia del token */
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["AppSettings:Token"])),
        ValidateIssuer = false,
        ValidateAudience = false
    });

/* Habilitar CORS */
builder.Services.AddCors();
    
#endregion

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("ApiCategorias", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Categorias",
        Description = "Peliculas ASP.NET Core Web API",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Miguel Barreto Torres",
            Email = string.Empty,
            Url = new Uri("https://twitter.com/spboyer"),
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://en.wikipedia.org/wiki/MIT License"),
        }
    });
    
    o.SwaggerDoc("ApiPeliculas", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Peliculas",
        Description = "Peliculas ASP.NET Core Web API",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Miguel Barreto Torres",
            Email = string.Empty,
            Url = new Uri("https://twitter.com/spboyer"),
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://en.wikipedia.org/wiki/MIT License"),
        }
    });
    
    o.SwaggerDoc("ApiUsuarios", new OpenApiInfo
    {
        Version = "v1",
        Title = "API Usuarios",
        Description = "Peliculas ASP.NET Core Web API",
        TermsOfService = new Uri("https://example.com/terms"),
        Contact = new OpenApiContact
        {
            Name = "Miguel Barreto Torres",
            Email = string.Empty,
            Url = new Uri("https://twitter.com/spboyer"),
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://en.wikipedia.org/wiki/MIT License"),
        }
    });

    o.IncludeXmlComments(
        Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    
    // Definir el schema de seguridad
    o.AddSecurityDefinition("Bearer",
            new OpenApiSecurityScheme
            {
                Description = "Authentication JWT (Bearer)",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });
    
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            }, new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler(b =>
    {
        b.Run(async context =>
        {
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
            var error = context.Features.Get<IExceptionHandlerFeature>();

            if (error is not null)
            {
                context.Response.AddApplicationError(error.Error.Message);
                await context.Response.WriteAsync(error.Error.Message);
            }
        });
    });
}

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/ApiCategorias/swagger.json", "API Categorias");
    options.SwaggerEndpoint("/swagger/ApiPeliculas/swagger.json", "API Peliculas");
    options.SwaggerEndpoint("/swagger/ApiUsuarios/swagger.json", "API Usuarios");
});

// app.UseRouting();

/* Autenticacion y autorizacion*/
app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

/* Soporte para CORS */
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.Run();
