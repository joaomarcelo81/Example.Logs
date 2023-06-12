using Microsoft.AspNetCore.Hosting;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Host.ConfigureLogging(logging =>
//{
//    logging.ClearProviders();
//    logging.AddSerilog(new LoggerConfiguration()
//        .Enrich.FromLogContext()
//        .WriteTo.SQLite(
//            sqliteDbPath: builder.Configuration.GetConnectionString("BaseLog"),
//            tableName: "LogsAPIFinancas")
//        .WriteTo.Console()
//        .CreateLogger());
//});

var logger = new LoggerConfiguration()
                       .ReadFrom.Configuration(builder.Configuration)
                       .Enrich.FromLogContext()
                        .WriteTo.SQLite(
                            sqliteDbPath: builder.Configuration.GetConnectionString("BaseLog"),
                            tableName: "LogsAPIFinancas")
                        .WriteTo.Console()
                       .CreateLogger();
builder.Host.UseSerilog(logger);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapControllers();

app.UseSerilogRequestLogging();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

Serilog.Log.Information("Iniciando Web Host");

app.Run();













