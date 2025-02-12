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

            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddSingleton<IRedisCache, RedisCache>();

            builder.Services.AddHttpClient();

            builder.Services.AddSingleton(ConnectionMultiplexer.Connect("redis:6379"));

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
