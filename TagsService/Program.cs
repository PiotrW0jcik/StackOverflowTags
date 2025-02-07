using StackExchange.Redis;
using TagsService.Services;

namespace TagsService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Register TagService and RedisCache
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddSingleton<IRedisCache, RedisCache>();

            // Register IHttpClientFactory
            builder.Services.AddHttpClient();

            // Register IConnectionMultiplexer as a Singleton
            var redisConnectionString = builder.Environment.IsDevelopment()
             ? "localhost:6379"  
             : "redis:6379";

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
