using DotNetAdvance.DatabaseContext;
using DotNetAdvance.Interface;
using DotNetAdvance.Logging;
using DotNetAdvance.Middleware;
using DotNetAdvance.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Serilog.Ui.Web;
using Serilog.Ui.MsSqlServerProvider;
using DotNetAdvance.Swagger;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using DotNetAdvance.Services;
using DotNetAdvance.Swagger;
using DotNetAdvance.Chaching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options =>
{
    // Add a custom operation filter which sets default values
    options.OperationFilter<SwaggerDefaultValues>();
});
builder.Services.AddScoped(typeof(IDatabaseRepository<>), typeof(DatabaseRepository<>));
builder.Services.AddScoped(typeof(ITodoItem), typeof(TodoItemService));
builder.Services.AddScoped(typeof(IToDoItemUpdated), typeof(ToDoItemUpdatedService));
builder.Services.AddMemoryCache();
builder.Services
    .AddApiVersioning()
    .AddApiExplorer(options =>
    {
        options.GroupNameFormat = "'v'VVV";
        options.SubstituteApiVersionInUrl = true;
        
    });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DefaultContext>(options => options.UseSqlServer(connectionString));

Log.Logger= new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.File("Logs/log.txt")
    .WriteTo.MSSqlServer(
        connectionString: connectionString,
        tableName: "Logs",
        autoCreateSqlTable: true
    ).CreateLogger();

builder.Services.AddSerilogUi(options =>
      options.UseSqlServer(connectionString, "Logs")
);
var app = builder.Build();

app.UseMiddleware<LoggingMiddleware>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();

        // Build a swagger endpoint for each discovered API version
        foreach (var description in descriptions)
        {
            var url = $"/swagger/{description.GroupName}/swagger.json";
            var name = description.GroupName.ToUpperInvariant();
            options.SwaggerEndpoint(url, name);
        }
    });
}

app.UseSerilogUi();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<CachingMiddleware>();

app.UseRouting();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
