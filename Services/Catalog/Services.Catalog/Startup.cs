using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Services.Catalog.Services;
using Services.Catalog.Settings;

namespace Services.Catalog
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
      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
      {
        options.Authority = Configuration["IdentityServerURL"];
        options.Audience = "resource_catalog";
        options.RequireHttpsMetadata = false;
      });
      services.AddScoped<ICategoryService, CategoryService>();
      services.AddScoped<ICourseService, CourseService>();
      services.AddAutoMapper(typeof(Startup));
      services.AddControllers(opt => {
        opt.Filters.Add(new AuthorizeFilter());
      });

      services.Configure<DatabaseSettings>(Configuration.GetSection("DatabaseSettings"));
      services.AddSingleton<IDatabaseSettings>(sp =>
      {
        return sp.GetRequiredService<IOptions<DatabaseSettings>>().Value;
      });

      services.AddSwaggerGen();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      app.UseSwagger();
      app.UseSwaggerUI(c =>
      {
        c.SwaggerEndpoint("v1/swagger.json", "Services.Catalog v1");
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
