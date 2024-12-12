using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snp.Api;
using Snp.Api.GraphQL.Mutations;
using Snp.Api.GraphQL.Queries;
using Snp.Api.SnpCache;
using Snp.Domain.Managers.Snp;
using StackExchange.Redis;
using Start.Common.Classes;
using Start.Infrastructure.Entites;

namespace AppContainer
{
    public class Startup
    {
        public IConfiguration _configuration { get; }
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //var appSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();
            //services.AddSingleton(appSettings);
            services.AddSingleton(_configuration);
            services.AddSingleton<CustomConfigurationProvider>();
            services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("redis, abortConnect=false"));
            services.AddSingleton<ISnpManager, SnpManager>();
            services.AddSingleton<SnpCacheService>();
            services.AddGraphQLServer()
                    .AddQueryType<Query>()
                    .AddMutationType<Mutation>()
                    .AddFiltering()
                    .AddSorting();

            services.AddAuthorization();
            services.AddControllers();

            services.AddHttpClient();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.SnpModuleMaps();
            }).CreateMapper();

            services.AddSingleton(mapper);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowSpecificOrigin");
            app.UseRouting();
            app.UseHttpsRedirection();
            //app.UseMiddleware<JwtAuthorizationMiddleware>();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
