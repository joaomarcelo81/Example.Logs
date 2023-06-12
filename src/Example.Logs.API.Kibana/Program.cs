using Example.Logs.API.Kibana.Extensions;
using Nest;
using Serilog;
using Serilog.Sinks.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//var logger = new LoggerConfiguration()
//                       .ReadFrom.Configuration(builder.Configuration)
//                       .Enrich.FromLogContext()
//                       .WriteTo.Elasticsearch(
//                            options:
//                                new ElasticsearchSinkOptions(
//                                    new Uri(builder.Configuration["Elasticsearch:Uri"]))
//                                {
//                                    AutoRegisterTemplate = true,
//                                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
//                                    IndexFormat = "apifinancas-{0:yyyy.MM}"
//                                })
//                        .WriteTo.Console()
//                       .CreateLogger();
//builder.Host.UseSerilog(logger);


//builder.Services.AddElasticsearch(builder.Configuration);


builder.Services.AddElasticsearch();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseSerilogRequestLogging();

app.Run();
