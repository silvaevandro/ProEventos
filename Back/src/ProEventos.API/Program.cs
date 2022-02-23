using ProEventos.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.DomainService;
using ProEventos.Infra.Data.Repository;
using ProEventos.Infra.Data;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;
using ProEventos.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProEventos.API.Helpers;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddDbContext<ProEventosContext>(
    context => context.UseMySql(builder.Configuration.GetConnectionString("Default"), new MySqlServerVersion(new Version(8, 0, 11)))
);

builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
})
.AddRoles<Role>()
.AddRoleManager<RoleManager<Role>>()
.AddSignInManager<SignInManager<User>>()
.AddRoleValidator<RoleValidator<Role>>()
.AddEntityFrameworkStores<ProEventosContext>()
.AddDefaultTokenProviders();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

builder.Services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      builder =>
                      {
                          builder.WithOrigins("http://localhost:4200")
                                              .AllowAnyHeader()
                                              .AllowAnyMethod();
                      });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "ProEventos.API", Version = "v1" });
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header usando Bearer.
                                Entre com 'Bearer ' [espaço] então coloque seu token.
                                Exemplo: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
    });
});
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<ILoteService, LoteService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPalestranteService, PalestranteService>();
builder.Services.AddScoped<IRedeSocialService, RedeSocialService>();

builder.Services.AddScoped<IGeralRepository, GeralRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();
builder.Services.AddScoped<ILoteRepository, LoteRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPalestranteRepository, PalestranteRepository>();
builder.Services.AddScoped<IRedeSocialRepository, RedeSocialRepository>();

builder.Services.AddScoped<IUtil, Util>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
    RequestPath = new PathString("/Resources")
});

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();



app.Run();


//, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.26-mysql"), b => b.MigrationsAssembly("ProEventos.API)