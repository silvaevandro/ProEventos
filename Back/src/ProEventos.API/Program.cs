using ProEventos.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using ProEventos.Application.DomainService;
using ProEventos.Infra.Data.Repository;
using ProEventos.Infra.Data;

var builder = WebApplication.CreateBuilder(args);
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

// Add services to the container.
builder.Services.AddDbContext<ProEventosContext>(
    context => context.UseMySql(builder.Configuration.GetConnectionString("Default"), new MySqlServerVersion(new Version(8, 0, 11)))
);
builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
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
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IGeralRepository, GeralRepository>();
builder.Services.AddScoped<IEventoRepository, EventoRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(MyAllowSpecificOrigins);

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();


//, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.26-mysql"), b => b.MigrationsAssembly("ProEventos.API")