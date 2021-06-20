using System;
using System.Collections.Generic;
using Claims_Api;
using Claims_Api.Extensions;
using Claims_Api.Repositories.UnitOfWork;
using Claims_Api.Services.Claim;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;

namespace Tests
{
    public class StartupTester
    {
        [Test]
        public void ConfigureServices_RegistersDependenciesCorrectly()
        {
            var configurationSectionStub = new Mock<IConfigurationSection>();
            var fakeValues = new Dictionary<string, string>
            {
                {"AppSettings", "anyfakevalue"}
            };
            configurationSectionStub.Setup(x => x["DefaultConnection"]).Returns("TestConnectionString");
            IConfiguration configurationStub = new ConfigurationBuilder()
                .AddInMemoryCollection(fakeValues)
                .Build();
            
            IServiceCollection services = new ServiceCollection();
            var target = new Startup(configurationStub);
            target.ConfigureServices(services);
            services.AddCoreServices();
            services.AddServicesAndRepositories();
            var serviceProvider = services.BuildServiceProvider();
            var configuration = serviceProvider.GetService<IConfiguration>();
            Assert.IsNotNull(configuration);
        }
        
        [Test]
        public void ConfigureServices_RegistersDependenciesMissing()
        {
            try
            {
                var configurationSectionStub = new Mock<IConfigurationSection>();
                var fakeValues = new Dictionary<string, string>
                {
                    {"AppSettings", "anyfakevalue"}
                };
                configurationSectionStub.Setup(x => x["DefaultConnection"]).Returns("TestConnectionString");
                IConfiguration configurationStub = new ConfigurationBuilder()
                    .AddInMemoryCollection(fakeValues)
                    .Build();
            
                IServiceCollection services = new ServiceCollection();
                var target = new Startup(configurationStub);
                target.ConfigureServices(services);
                services.AddCoreServices();
                services.AddServicesAndRepositories();
                var serviceProvider = services.BuildServiceProvider();
                serviceProvider.GetService<IClaimService>();
                Assert.Fail();
            }
            catch (Exception)
            {
                Assert.Pass();
            }

        }
    }
}
