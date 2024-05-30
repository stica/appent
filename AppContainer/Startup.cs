using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Security.Api.Public;
using Security.Domain.Managers;
using Security.Domain.Services;
using Start.Infrastructure.Entites;
using Start.Infrastructure.Interfaces;
using Start.Infrastructure.Middlewares;

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
            var appSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();
            services.AddSingleton(appSettings);
            services.AddAuthorization();
            services.AddControllers();

            services.AddSingleton<ISecurityManager, SecurityManager>();
            services.AddSingleton<IAuthenticationService, AuthenticationService>();
            services.AddSingleton<IAuthorizationService, AuthorizationService>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder.WithOrigins("http://localhost:4200")
                                      .AllowAnyHeader()
                                      .AllowAnyMethod());
            });

            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.SecurityAdminMaps();
                cfg.SecurityPublicMaps();
            }).CreateMapper();

            services.AddSingleton(mapper);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowSpecificOrigin");
            app.UseRouting();
            app.UseHttpsRedirection();
            app.UseMiddleware<JwtAuthorizationMiddleware>();
            app.UseAuthorization();
            app.UseAuthentication();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
