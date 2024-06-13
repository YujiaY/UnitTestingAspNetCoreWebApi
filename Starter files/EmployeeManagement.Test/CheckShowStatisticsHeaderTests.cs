using EmployeeManagement.ActionFilters;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Xunit;

namespace EmployeeManagement.Test;

public class CheckShowStatisticsHeaderTests
{
    [Fact]
    public void OnActionExecuting_InvokeWithoutShowStatisticsHeader_ReturnsBadRequest()
    {
        // Arrange 
        CheckShowStatisticsHeader checkShowStatisticsHeaderActionFilter = new();

        DefaultHttpContext httpContext = new();

        ActionContext actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor(),
            new ModelStateDictionary());
        ActionExecutingContext actionExecutingContext = new(actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            null);

        // Act
        checkShowStatisticsHeaderActionFilter.OnActionExecuting(actionExecutingContext);

        // Assert
        Assert.IsType<BadRequestResult>(actionExecutingContext.Result);
        actionExecutingContext.Result
            .Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public void OnActionExecuting_HeaderPresentButNotBoolean_ReturnsBadRequest()
    {
        // Arrange 
        CheckShowStatisticsHeader checkShowStatisticsHeaderActionFilter = new();
        DefaultHttpContext httpContext = new();
        // httpContext.Request.Headers["ShowStatistics"] = "not a boolean";
        httpContext.Request.Headers["ShowStatistics"] = false.ToString(); // false.ToString() is "False"

        ActionContext actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor(),
            new ModelStateDictionary());
        ActionExecutingContext actionExecutingContext = new(actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            null);

        // Act
        checkShowStatisticsHeaderActionFilter.OnActionExecuting(actionExecutingContext);

        // Assert
        Assert.IsType<BadRequestResult>(actionExecutingContext.Result);
        actionExecutingContext.Result
            .Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public void OnActionExecuting_HeaderPresentAndFalse_ReturnsBadRequest()
    {
        // Arrange 
        CheckShowStatisticsHeader checkShowStatisticsHeaderActionFilter = new();
        DefaultHttpContext httpContext = new();
        httpContext.Request.Headers["ShowStatistics"] = "false";

        ActionContext actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor(),
            new ModelStateDictionary());
        ActionExecutingContext actionExecutingContext = new(actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            null);

        // Act
        checkShowStatisticsHeaderActionFilter.OnActionExecuting(actionExecutingContext);

        // Assert
        Assert.IsType<BadRequestResult>(actionExecutingContext.Result);
        actionExecutingContext.Result
            .Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public void OnActionExecuting_HeaderPresentAndTrue_NoBadRequest()
    {
        // Arrange 
        CheckShowStatisticsHeader checkShowStatisticsHeaderActionFilter = new();
        DefaultHttpContext httpContext = new();
        httpContext.Request.Headers["ShowStatistics"] = "true";

        ActionContext actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor(),
            new ModelStateDictionary());
        ActionExecutingContext actionExecutingContext = new(actionContext,
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            null);

        // Act
        checkShowStatisticsHeaderActionFilter.OnActionExecuting(actionExecutingContext);

        // Assert
        Assert.Null(actionExecutingContext.Result);
        actionExecutingContext.Result
            .Should().BeNull();
    }
}