using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Xunit;

namespace EmployeeManagement.Test
{
    public class DemoInternalEmployeesControllerTests
    {
        [Fact]
        public async Task CreateInternalEmployee_InvalidInput_MustReturnBadRequest()
        { 
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var mapperMock = new Mock<IMapper>();
            var demoInternalEmployeesController = new DemoInternalEmployeesController(
                employeeServiceMock.Object, mapperMock.Object);

            // var internalEmployeeForCreationDto = new InternalEmployeeForCreationDto()
            //     { FirstName = string. Empty,LastName = "Smith" };
                
            var internalEmployeeForCreationDto = new InternalEmployeeForCreationDto();

            demoInternalEmployeesController.ModelState
                .AddModelError("FirstName", "FirstName is Required");
            
            demoInternalEmployeesController.ModelState
                .AddModelError("FirstNameAgain", "FirstName Again is Required");
            demoInternalEmployeesController.ModelState
                .AddModelError("LastName", "LastName is also Required");

            // Act 
            var result = await demoInternalEmployeesController
                .CreateInternalEmployee(internalEmployeeForCreationDto);

            // Assert
            var actionResult = Assert
                .IsType<ActionResult<InternalEmployeeDto>>(result);
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.IsType<SerializableError>(badRequestResult.Value);
            
            // Fluent Assertions
            ActionResult<InternalEmployeeDto>? actionResult2 = result.Should()
                .BeOfType<ActionResult<InternalEmployeeDto>>().Subject;

            BadRequestObjectResult? badRequestResult2 = actionResult2.Result.Should()
                .BeOfType<BadRequestObjectResult>().Subject;
            SerializableError? serializableError = badRequestResult2.Value.Should()
                .BeOfType<SerializableError>().Subject;
        }

        [Fact]
        public void GetProtectedInternalEmployees_GetActionForUserInAdminRole_MustRedirectToGetInternalEmployeesOnProtectedInternalEmployees()
        {
            // Arrange
            Mock<IEmployeeService> employeeServiceMock = new Mock<IEmployeeService>();
            Mock<IMapper> mapperMock = new Mock<IMapper>();
            DemoInternalEmployeesController demoInternalEmployeesController = 
                new(employeeServiceMock.Object, mapperMock.Object);
           
            List<Claim> userClaims =
            [
                new Claim(ClaimTypes.Name, "Karen"),
                new Claim(ClaimTypes.Role, "Admin")
            ];
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(userClaims, "UnitTest");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            DefaultHttpContext defaultHttpContext = new()
            {
                User = claimsPrincipal
            };

            demoInternalEmployeesController.ControllerContext = 
                new ControllerContext()
                {
                    HttpContext = defaultHttpContext
                };

            // Act
            IActionResult result = demoInternalEmployeesController.GetProtectedInternalEmployees();

            using (new AssertionScope())
            {
                
                // Assert using Fluent Assertions, combined assertions
                RedirectToActionResult? redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
                redirectResult.ActionName.Should().Be("GetInternalEmployees");
                redirectResult.ControllerName.Should().Be("ProtectedInternalEmployees");
                
                // Assert 
                IActionResult actionResult = Assert.IsAssignableFrom<IActionResult>(result);
                // This one below is more specific:
                RedirectToActionResult redirectoToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("GetInternalEmployees", redirectoToActionResult.ActionName);
                Assert.Equal("ProtectedInternalEmployees", redirectoToActionResult.ControllerName);
            }
        }

        [Fact]
        public void GetProtectedInternalEmployees_GetActionForUserInAdminRole_MustRedirectToGetInternalEmployeesOnProtectedInternalEmployees_WithMoq()
        {
            // Arrange
            var employeeServiceMock = new Mock<IEmployeeService>();
            var mapperMock = new Mock<IMapper>();
            var demoInternalEmployeesController =
                new DemoInternalEmployeesController(
                    employeeServiceMock.Object, mapperMock.Object);

            var mockPrincipal = new Mock<ClaimsPrincipal>();
            mockPrincipal.Setup(x => 
                x.IsInRole(It.Is<string>(s => s == "Admin"))).Returns(true);

            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(c => c.User)
                .Returns(mockPrincipal.Object); 

            demoInternalEmployeesController.ControllerContext =
                new ControllerContext()
                {
                    HttpContext = httpContextMock.Object
                };

            // Act
            var result = demoInternalEmployeesController.GetProtectedInternalEmployees();

            using (new AssertionScope())
            {
                
                // Assert using Fluent Assertions, combined assertions
                RedirectToActionResult? redirectResult = result.Should().BeOfType<RedirectToActionResult>().Subject;
                redirectResult.ActionName.Should().Be("GetInternalEmployees");
                redirectResult.ControllerName.Should().Be("ProtectedInternalEmployees");
                
                // Assert 
                IActionResult actionResult = Assert.IsAssignableFrom<IActionResult>(result);
                // This one below is more specific:
                RedirectToActionResult redirectoToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("GetInternalEmployees", redirectoToActionResult.ActionName);
                Assert.Equal("ProtectedInternalEmployees", redirectoToActionResult.ControllerName);
            }
        }
    }
}
