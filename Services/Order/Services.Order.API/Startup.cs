using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using MassTransit;
using MediatR;
using Microservices.Shared.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Services.Order.Application.Consumers;
using Services.Order.Infrastracture;

namespace Services.Order.API
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

      services.AddMassTransit(x =>
      {
        x.AddConsumer<CreateOrderMessageCommandConsumer>();
        //x.AddConsumer<CourseNameChangedEventConsumer>();

        x.UsingRabbitMq((context, cfg) =>
        {
          cfg.Host(Configuration["RabbitMQUrl"], "/", host =>
          {
            host.Username("guest");
            host.Password("guest");
          });

          cfg.ReceiveEndpoint("create-order-service", e =>
          {
            e.ConfigureConsumer<CreateOrderMessageCommandConsumer>(context);
          });
          cfg.ReceiveEndpoint("course-name-changed-event-order-service", e =>
          {
            //e.ConfigureConsumer<CourseNameChangedEventConsumer>(context);
          });
        });
      });

      services.AddMassTransitHostedService();

      var requireAuthorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
      JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Remove("sub");
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt => {
        opt.Authority = Configuration["IdentityServerUrl"];
        opt.Audience = "resource_order";
        opt.RequireHttpsMetadata = false;
      });

      services.AddDbContext<OrderDbContext>(opt =>
      {
        opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), configure =>
        {
          //configure.MigrationsAssembly("Services.Order.Infrastructure");
        });
      });

      services.AddHttpContextAccessor();
      services.AddScoped<ISharedIdentityService, SharedIdentityService>();

      services.AddMediatR(typeof(Application.Handlers.CreateOrderCommandHandler).Assembly);

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
        c.SwaggerEndpoint("v1/swagger.json", "Services.Order v1");
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
