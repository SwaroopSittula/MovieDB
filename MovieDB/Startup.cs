using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MovieDB.Middleware;
using MovieDB.Models;
using MovieDB.Repository;
using MovieDB.Repository.Interfaces;
using Serilog;
using StackExchange.Profiling;
using System;

namespace MovieDB
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
            Log.Logger = new LoggerConfiguration()
                          .ReadFrom.Configuration(Configuration)
                          .CreateLogger();

            services.AddHttpClient("MovieDbAPI", client =>
            {
                client.BaseAddress = new Uri(Configuration.GetValue<string>("MovieDBSettings:RestApi:BaseUrl"));
            });

            //Configurations settings
            services.Configure<DatabaseSettings>(options => { Configuration.GetSection("DatabaseSettings").Bind(options); });
            services.Configure<UserSettings>(options => { Configuration.GetSection("UserSettings").Bind(options); });
            services.Configure<MovieDBSettings>(options => { Configuration.GetSection("MovieDBSettings").Bind(options); });


            //Register application dependancies
            services.AddSingleton<IMovieRepository, MovieRepository>();

            services.AddControllers();

            services.AddSwaggerDocument( doc =>
            {
                doc.Title = "Movie Proxy API";
            });
            if (Configuration["UserSettings:EnableMiniProfiler"] == "True")
            {
                services.AddMiniProfiler(options =>
                {
                    options.RouteBasePath = Configuration["UserSettings:BasePath"] + "/miniprofiler";
                    options.IgnorePath("/swagger/");
                    options.IgnorePath("/miniprofiler/");
                });
            }

            //To disable Model validation errors automatically trigger an HTTP 400 response and return custom error response
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;

            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware(typeof(AuditMiddleware));
            app.UseMiddleware(typeof(ErrorHandlerMiddleware));

            app.UseHttpsRedirection();

            //middlewares
            app.UseSerilogRequestLogging();
            if (Configuration["UserSettings:EnableMiniProfiler"] == "True")
            {
                app.UseMiniProfiler();
            }
            app.UseOpenApi();
            app.UseSwaggerUi3();

            

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
