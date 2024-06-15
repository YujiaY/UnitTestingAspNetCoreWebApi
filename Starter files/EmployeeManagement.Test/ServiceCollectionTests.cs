using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Services;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace EmployeeManagement.Test;

public class ServiceCollectionTests
{
    [Fact]
    public void RegisterDataServices_Execute_DataServicesAreRegistered()
    {
        // Arrange
        ServiceCollection serviceCollection = new();
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(
                new Dictionary<string, string>
                {
                    { "ConnectionStrings:EmployeeManagementDB111", "AnyValueWillDo" }
                } as IEnumerable<KeyValuePair<string, string?>>
            )
            .Build();

        // Act
        serviceCollection.RegisterDataServices(configuration);
        ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

        // Assert
        Assert.NotNull(
            serviceProvider.GetService<IEmployeeManagementRepository>());
        Assert.IsType<EmployeeManagementRepository>(
            serviceProvider.GetService<IEmployeeManagementRepository>());

        IEmployeeManagementRepository? repository = serviceProvider.GetService<IEmployeeManagementRepository>();
        repository.Should().NotBeNull();
        repository.Should().BeOfType<EmployeeManagementRepository>();
    }

    [Fact]
    public void RegisterBusinessServices_Execute_BusinessServicesAreRegistered()
    {
        // Arrange
        ServiceCollection services = new ServiceCollection();

        // Act
        services.RegisterBusinessServices();

        // Assert
        services.Any(sd =>
                sd.ServiceType == typeof(IEmployeeService) && sd.Lifetime == ServiceLifetime.Scoped)
            .Should().BeTrue();
        services.Any(sd =>
                sd.ServiceType == typeof(IPromotionService) && sd.Lifetime == ServiceLifetime.Scoped)
            .Should().BeTrue();
        services.Any(sd =>
                sd.ServiceType == typeof(EmployeeFactory) && sd.Lifetime == ServiceLifetime.Scoped)
            .Should().BeTrue();
    }

    [Fact]//(Skip = "Still working on this")]
    public void RegisterBusinessServices_Execute_BusinessServicesAreRegistered_v2()
    {
        // Arrange
        // Arrange
        ServiceCollection services = new ServiceCollection();
        IConfiguration configuration = new ConfigurationBuilder().AddInMemoryCollection().Build();

        // Act
        services.RegisterDataServices(configuration);
        services.RegisterBusinessServices();

        // Assert
        using (new AssertionScope())
        {
            // Check that all expected services are registered with the correct lifetimes and implementations
            ServiceProvider serviceProvider = services.BuildServiceProvider();

            // Verify the specific services
            IEmployeeService? employeeService = serviceProvider.GetService<IEmployeeService>();
            employeeService.Should().NotBeNull();
            employeeService.Should().BeOfType<EmployeeService>();

            IPromotionService? promotionService = serviceProvider.GetService<IPromotionService>();
            promotionService.Should().NotBeNull();
            promotionService.Should().BeOfType<PromotionService>();

            var employeeFactory = serviceProvider.GetService<EmployeeFactory>();
            employeeFactory.Should().NotBeNull();

            // Check that services are registered as Scoped
            services.FirstOrDefault(sd => sd.ServiceType == typeof(IEmployeeService))
                .Should().NotBeNull()
                .And.Match<ServiceDescriptor>(sd => sd.Lifetime == ServiceLifetime.Scoped);
            
            services.FirstOrDefault(sd => sd.ServiceType == typeof(IPromotionService))
                .Should().NotBeNull()
                .And.Match<ServiceDescriptor>(sd => sd.Lifetime == ServiceLifetime.Scoped);
            
            services.FirstOrDefault(sd => sd.ServiceType == typeof(EmployeeFactory))
                .Should().NotBeNull()
                .And.Match<ServiceDescriptor>(sd => sd.Lifetime == ServiceLifetime.Scoped);

            // // You can also check the service descriptor to ensure they are registered as Scoped
            var employeeServiceDescriptor = services.FirstOrDefault(
                sd => sd.ServiceType == typeof(IEmployeeService) && sd.Lifetime == ServiceLifetime.Scoped);
            Assert.NotNull(employeeServiceDescriptor);
            Assert.Equal(ServiceLifetime.Scoped, employeeServiceDescriptor.Lifetime);
            
            var promotionServiceDescriptor = services.FirstOrDefault(
                sd => sd.ServiceType == typeof(IPromotionService) && sd.Lifetime == ServiceLifetime.Scoped);
            Assert.NotNull(promotionServiceDescriptor);
            Assert.Equal(ServiceLifetime.Scoped, promotionServiceDescriptor.Lifetime);
            
            var employeeFactoryDescriptor = services.FirstOrDefault(
                sd => sd.ServiceType == typeof(EmployeeFactory) && sd.Lifetime == ServiceLifetime.Scoped);
            Assert.NotNull(employeeFactoryDescriptor);
            Assert.Equal(ServiceLifetime.Scoped, employeeFactoryDescriptor.Lifetime);
        }
    }
}