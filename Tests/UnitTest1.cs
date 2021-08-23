
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MovieDB.Controllers;
using MovieDB.Models;
using MovieDB.Repository;
using MovieDB.Repository.Interfaces;
using System.Threading.Tasks;
using System.Net.Http;
using Moq;
using MovieDB.Helpers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using MovieDB;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Tests
{
    [TestClass]
    public class UnitTest1
    {
        static IMovieRepository Repo;
        static MovieController Controller;

        IHttpClientFactory httpClientFactory;
        IOptions<DatabaseSettings> dbSettings;
        IOptions<MovieDBSettings> apiSettings;
        [TestInitialize]
        public void Initialize()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Development.json")
                .Build();
            
            var services = new ServiceCollection();
            services.AddHttpClient("MovieDbAPI", client =>
            {
                client.BaseAddress = new Uri(config.GetValue<string>("MovieDBSettings:RestApi:BaseUrl"));
            });

            httpClientFactory = services.BuildServiceProvider().GetRequiredService<IHttpClientFactory>();


            dbSettings = Options.Create(new DatabaseSettings() 
            {
                ConnectionString = config.GetValue<string>("DatabaseSettings:ConnectionString"),
                DatabaseName = config.GetValue<string>("DatabaseSettings:DatabaseName"),
                CollectionName = config.GetValue<string>("DatabaseSettings:CollectionName")
            });
            apiSettings = Options.Create(new MovieDBSettings()
            {
                RestApi = new ApiDetails()
                {
                    BaseUrl = config.GetValue<string>("MovieDBSettings:RestApi:BaseUrl"),
                    ApiKey = config.GetValue<string>("MovieDBSettings:RestApi:ApiKey")
                }
            });

            Repo = new MovieRepository(httpClientFactory, apiSettings, dbSettings);
            Controller = new MovieController(Repo);
        }


        /// <summary>
        /// From The MovieDB API
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task SuccessFromAPI()
        {
            var id = 15;//not in Movie Cache in Mongo DB
            var reponse = await Controller.GetMovie(id) as ContentResult;
            Assert.AreEqual(200, reponse.StatusCode);
        }

        [TestMethod]
        public async Task NotFoundInApi()
        {
            var id = 1;//no resource
            var reponse = await Controller.GetMovie(id) as ContentResult;
            Assert.AreEqual(404, reponse.StatusCode);
        }

        [TestMethod]
        public async Task InvalidApiKey()
        {
            // Arrange
            var projectDir = Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder()
                .SetBasePath(projectDir)
                .AddJsonFile("appsettings.Development.json")
                .Build();

            config["MovieDBSettings:RestApi:ApiKey"] = "Wrong API Key";
            var server = new TestServer(new WebHostBuilder()
                .UseContentRoot(projectDir)
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .UseSerilog());


            // Act
            using (var client = server.CreateClient())
            {
                var id = 0; //Not in Cache, reource not found implies always not in cache
                var result = await client.GetAsync($"/movie/{id}");
                // Ensure Success StatusCode is returned from response

                Assert.AreEqual(401, (int)result.StatusCode);
            }
        }


        [TestMethod]
        public async Task SuccessFromCache()
        {
            var id = 5;
            var response = await Controller.GetMovie(id) as ContentResult;
            Assert.AreEqual(200, response.StatusCode);
        }
        [TestMethod]
        public async Task HealthCheck()
        {
            var response = await Controller.IsAliveAsync() as ContentResult;
            Assert.AreEqual("true", response.Content);
        }


        [TestMethod]
        public async Task InternalServerError()
        {
            // Arrange
            var projectDir = Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder()
                .SetBasePath(projectDir)
                .AddJsonFile("appsettings.Development.json")
                .Build();
            
            config["DatabaseSettings:ConnectionString"] = "Wrong Connection String";
            var server = new TestServer(new WebHostBuilder()
                .UseContentRoot(projectDir)
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .UseSerilog());


            // Act
            using (var client = server.CreateClient())
            {
                var id = 5; //cache
                var result = await client.GetAsync($"/movie/{id}");
                // Ensure Success StatusCode is returned from response

                Assert.AreEqual(500, (int)result.StatusCode);
            }

        }


        //	To test ValidateFilterAtribute class, we use mocking to mock the objects.Go to Manage Nuget Packages and install Moq.Below is the code which shows how to mock the objects.
        [TestMethod]
        public void TestMethod6()
        {
            var ioptions = new Mock<IOptions<DatabaseSettings>>();
            var datarepo = new Mock<MovieRepository>(ioptions);
            var validationFilter = new ValidateModelAttribute();
            var modelState = new ModelStateDictionary();
            modelState.AddModelError("id", Constants.ID_Required);

            var actionContext = new ActionContext(
                Mock.Of<HttpContext>(),
                Mock.Of<RouteData>(),
                Mock.Of<ActionDescriptor>(),
                modelState
            );

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new Mock<MovieController>(datarepo)
            );
            
            validationFilter.OnActionExecuting(actionExecutingContext);
            Assert.IsInstanceOfType(actionExecutingContext.Result, typeof(BadRequestObjectResult));
        }

        //	Add the following lines of code to test Startup.cs class.
        [TestMethod]
        public async Task StartUpHealth_Success_Test()
        {
            // Arrange
            var projectDir = Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder()
                .SetBasePath(projectDir)
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var server = new TestServer(new WebHostBuilder()
                .UseContentRoot(projectDir)
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .UseSerilog());


            // Act
            using (var client = server.CreateClient())
            {
                var id = 5; //cache
                var result = await client.GetAsync($"/movie/{id}");
                // Ensure Success StatusCode is returned from response
                //var responseBody = await result.Content.ReadAsStringAsync()
                result.EnsureSuccessStatusCode();

                // Assert
                Assert.IsTrue(result.IsSuccessStatusCode);
                Assert.AreEqual(200, (int)result.StatusCode);
            }
        }
        [TestMethod]
        public async Task StartUpHealth_error404()
        {
            // Arrange
            var projectDir = Directory.GetCurrentDirectory();

            var config = new ConfigurationBuilder()
                .SetBasePath(projectDir)
                .AddJsonFile("appsettings.Development.json")
                .Build();

            var server = new TestServer(new WebHostBuilder()
                .UseContentRoot(projectDir)
                .UseConfiguration(config)
                .UseStartup<Startup>()
                .UseSerilog());


            // Act
            using (var client = server.CreateClient())
            {
                var id = 0; //not  found in api
                var result = await client.GetAsync($"/movie/{id}");
                // Ensure Success StatusCode is returned from response
                //var responseBody = await result.Content.ReadAsStringAsync()
                Assert.AreEqual(404, (int)result.StatusCode);
            }

        }
    }
}
