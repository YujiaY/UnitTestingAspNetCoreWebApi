using EmployeeManagement.Middleware;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace EmployeeManagement.Test
{
    public class EmployeeManagementSecurityHeadersMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_Invoke_SetsExpectedRequestAndResponseHeaders()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            RequestDelegate next = (HttpContext httpContext) => Task.CompletedTask;

            var middleware = new EmployeeManagementSecurityHeadersMiddleware(next);

            // Act
            await middleware.InvokeAsync(httpContext);

            // Assert
            var cspResponseHeader = httpContext.Response.Headers.ContentSecurityPolicy.ToString();
            var xContentTypeOptionsResponseHeader = httpContext.Response.Headers.XContentTypeOptions.ToString();
            var cspRequestHeader = httpContext.Request.Headers.ContentSecurityPolicy.ToString();
            var xContentTypeOptionsRequestHeader = httpContext.Request.Headers.XContentTypeOptions.ToString();

            Assert.Equal("default-src 'self';frame-ancestors 'none';", cspResponseHeader);
            Assert.Equal("nosniff", xContentTypeOptionsResponseHeader);
            
            cspResponseHeader.Should().Be("default-src 'self';frame-ancestors 'none';");
            xContentTypeOptionsResponseHeader.Should().Be("nosniff");
            
            Assert.Equal("default-src 'self';frame-ancestors 'none';", cspRequestHeader);
            Assert.Equal("nosniff", xContentTypeOptionsRequestHeader);
            cspRequestHeader.Should().Be("default-src 'self';frame-ancestors 'none';");
            xContentTypeOptionsRequestHeader.Should().Be("nosniff");
        }
    }
}
