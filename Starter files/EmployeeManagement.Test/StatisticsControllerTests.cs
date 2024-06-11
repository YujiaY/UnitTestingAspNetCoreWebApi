using System.Net;
using AutoMapper;
using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManagement.Test
{
    public class StatisticsControllerTests
    {
        [Fact]
        public void GetStatistics_InputFromHttpConnectionFeature_MustReturnInputtedIps()
        {
            // Arrange
            IPAddress localIpAddress = System.Net.IPAddress.Parse("111.111.111.111");
            int localPort = 5000;
            IPAddress remoteIpAddress = System.Net.IPAddress.Parse("222.222.222.222");
            int remotePort = 8080;

            var httpConnectionFeature = new HttpConnectionFeature()
            {
                LocalIpAddress = localIpAddress,
                LocalPort = localPort,
                RemoteIpAddress = remoteIpAddress,
                RemotePort = remotePort
            };

            var featureCollectionMock = new Mock<IFeatureCollection>();
            featureCollectionMock.Setup(m => m.Get<IHttpConnectionFeature>())
                .Returns(httpConnectionFeature);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(m => m.Features)
                .Returns(featureCollectionMock.Object);
            
            var mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperProfiles.StatisticsProfile>();
            });
            var mapper = new Mapper(mapperConfiguration);

            var statisticsController = new StatisticsController(mapper);
            
            statisticsController.ControllerContext = new ControllerContext()
            {
                HttpContext = httpContextMock.Object,
            };
            
            // Act
            ActionResult<StatisticsDto> result = statisticsController.GetStatistics();
            
            // Assert
            ActionResult<StatisticsDto> resultResult = Assert.IsType<ActionResult<StatisticsDto>>(result);
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(resultResult.Result);
            StatisticsDto statisticsDto = Assert.IsType<StatisticsDto>(okObjectResult.Value);
            Assert.Equal(localIpAddress.ToString(), statisticsDto.LocalIpAddress);
            Assert.Equal(localPort, statisticsDto.LocalPort);
            Assert.Equal(remoteIpAddress.ToString(), statisticsDto.RemoteIpAddress);
            Assert.Equal(remotePort, statisticsDto.RemotePort);
            
            // Fluent Assertions
            ActionResult<StatisticsDto>? resultResult2 = result.Should().BeOfType<ActionResult<StatisticsDto>>().Subject;
            OkObjectResult? okObjectResult2 = resultResult2.Result.Should().BeOfType<OkObjectResult>().Subject;
            StatisticsDto? statisticsDto2 = okObjectResult2.Value.Should().BeOfType<StatisticsDto>().Subject;
            
            // Working Method 3: Expected DTO based on the HttpConnectionFeature settings
            StatisticsDto expectedDto = new StatisticsDto
            {
                LocalIpAddress = localIpAddress.ToString(),
                LocalPort = localPort,
                RemoteIpAddress = remoteIpAddress.ToString(),
                RemotePort = remotePort
            };

            // Asserting the DTO properties directly
            statisticsDto.Should().BeEquivalentTo(expectedDto, options => options
                .ComparingByMembers<StatisticsDto>());
            
            
            // Working Method 2
            StatisticsDto expectedDto2 = new StatisticsDto
            {
                LocalIpAddress = localIpAddress.ToString(),
                LocalPort = localPort,
                RemoteIpAddress = remoteIpAddress.ToString(),
                RemotePort = remotePort
            };
            statisticsDto2.Should()
                .BeEquivalentTo(expectedDto2,
                    options => options
                        .Including(f => f.LocalIpAddress)
                        .Including(f => f.LocalPort)
                        .Including(f => f.RemoteIpAddress)
                        .Including(f => f.RemotePort)
                    );
            
            // Working Method 1
            statisticsDto2.LocalIpAddress.Should().Be(localIpAddress.ToString());
            statisticsDto2.LocalPort.Should().Be(localPort);
            statisticsDto2.RemoteIpAddress.Should().Be(remoteIpAddress.ToString());
            statisticsDto2.RemotePort.Should().Be(remotePort);
        }
    }
}
