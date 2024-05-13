using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nest;
using WebScraping.Configurations;
using Elasticsearch.Net;
using WebScraping.Services;
using WebScraping.Entities;

namespace WebScraping
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            var settings=new ConnectionSettings(new Uri("http://localhost:9200")).DefaultIndex("search-webscrapingindex");
            var client = new ElasticClient(settings);
            builder.Services.AddSingleton(client);
            builder.Services.AddScoped<IElasticsearchService<Yayin>, ElasticsearchService<Yayin>>();    

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: "FrontendUI", policy =>
                {
                    policy.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader();
                });
            });
            builder.Services.AddHttpClient();
            builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("MongoDatabase"));
            builder.Services.AddSingleton<YayinService>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("FrontendUI");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();   
        }
    }
}