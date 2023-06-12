using Nest;
using Serilog.Sinks.Elasticsearch;
using Serilog;

namespace Example.Logs.API.Kibana.Extensions
{
    public static class ElasticSearchExtensions
    {
        public static void AddElasticsearch(
            this IServiceCollection services, IConfiguration configuration)
        {
            var url = configuration["Elasticsearch:Uri"];
            var defaultIndex = configuration["Elasticsearch:Index"];

            var settings = new ConnectionSettings(new Uri(url))
                // .BasicAuthentication(userName, pass)
                .PrettyJson()
                .DefaultIndex(defaultIndex);

            //AddDefaultMappings(settings);

            var client = new ElasticClient(settings);

            services.AddSingleton<IElasticClient>(client);

            CreateIndex(client, defaultIndex);
        }
        public static void AddElasticsearch(this IServiceCollection services)
        {
            // Other configurations...
            var elastichPath = "http://127.0.0.1:9200";
            // Configure Serilog
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elastichPath))
                {
                    IndexFormat = "apifinancas-{0:yyyy.MM.dd}",
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    NumberOfShards = 2,
                    NumberOfReplicas = 1
                })
                .WriteTo.Debug()
                .WriteTo.Console()
                .CreateLogger();

            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());

            var client = new ElasticClient(new Uri(elastichPath));

            services.AddSingleton<IElasticClient>(client);
            var createIndexResponse = client.Indices.Create("apifinancas");


        }




        //private static void AddDefaultMappings(ConnectionSettings settings)
        //{
        //    settings
        //        .DefaultMappingFor<Product>(m => m
        //            .Ignore(p => p.Price)
        //            .Ignore(p => p.Measurement)
        //        );
        //}


        private static void CreateIndex(IElasticClient client, string indexName)
        {

            var createIndexResponse = client.Indices.Create(indexName);

            //var createIndexResponse = client.Indices.Create(indexName,
            //    index => index.Map<Product>(x => x.AutoMap())
            //);
        }
    }
}
