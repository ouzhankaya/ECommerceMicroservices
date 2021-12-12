using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microservices.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Basket.Services;
using Services.Basket.Settings;

namespace Services.Basket
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
      var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
        opt.Authority = Configuration["IdentityServerUrl"];
        opt.Audience = "resource_basket";
        opt.RequireHttpsMetadata = false;
      });
      services.AddHttpContextAccessor();
      services.AddScoped<ISharedIdentiyService, SharedIdentityService>();
      services.AddScoped<IBasketService, BasketService>();
      services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));

      services.AddSingleton<RedisService>(sp =>
      {
        var redisSettings = sp.GetRequiredService<IOptions<RedisSettings>>().Value;

        var redis = new RedisService(redisSettings.Host, redisSettings.Port);

        redis.Connect();

        return redis;
      });

      services.AddControllers(opt => {
        opt.Filters.Add(new AuthorizeFilter(requireAuthorizePolicy));
      });
      services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("v1/swagger.json", "Services.Basket v1");
      });
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();
      app.UseAuthentication();
      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
