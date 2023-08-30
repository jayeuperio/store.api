using Microsoft.EntityFrameworkCore;
using Store.Api.Domain;
using Store.Api.Extensions;
using Store.Api.Services.Command.Base;
using Store.Api.Services.Query.Base;

var builder = WebApplication.CreateBuilder(args);

var connection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<EntityContext>(options =>
                options.UseSqlServer(connection));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<UnitOfWork>();

builder.Services.RegisterAssemblies(DependencyResolver.DependancyLifetime.Transient, new[] { "Store.Api.Services" }, typeof(IQueryHandler));
builder.Services.RegisterAssemblies(DependencyResolver.DependancyLifetime.Transient, new[] { "Store.Api.Services" }, typeof(ICommandHandler));

builder.Host
    .ConfigureAppConfiguration(configuration =>
    {
        configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile(
            $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true);
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
else
{
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        options.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
