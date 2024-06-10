using AutoMapper;
using EmployeeManagement.Business;
using EmployeeManagement.Controllers;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Models;
using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EmployeeManagement.Test
{
    public class InternalEmployeeControllerTests
    {
        private readonly InternalEmployeesController _internalEmployeesController;
        private readonly InternalEmployee _firstEmployee;

        public InternalEmployeeControllerTests()
        {
            _firstEmployee = new InternalEmployee(
                "Megan", "Jones", 2, 3000, false, 2)
            {
                Id = Guid.Parse("bfdd0acd-d314-48d5-a7ad-0e94dfdd9155"),
                SuggestedBonus = 400
            };

            var employeeServiceMock = new Mock<IEmployeeService>();
            employeeServiceMock
                .Setup(m => m.FetchInternalEmployeesAsync())
                .ReturnsAsync(new List<InternalEmployee>() {
                    _firstEmployee,
                    new InternalEmployee("Jaimy", "Johnson", 3, 3400, true, 1),
                    new InternalEmployee("Anne", "Adams", 3, 4000, false, 3)
                });
            //
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(m =>
                 m.Map<InternalEmployee, InternalEmployeeDto>(It.IsAny<InternalEmployee>()))
                 .Returns(new InternalEmployeeDto());
            var mapperConfiguration = new MapperConfiguration(
                cfg => cfg.AddProfile<MapperProfiles.EmployeeProfile>());
            var mapper = new Mapper(mapperConfiguration);
            
            _internalEmployeesController = new InternalEmployeesController(
                 employeeServiceMock.Object, mapper);
        }
        
        [Fact]
        public async Task GetInternalEmployees_GetAction_ReturnsOkObjectResultWithCorrectAmountOfInternalEmployees()
        {
            // Arrange

            // Act
            var result = await _internalEmployeesController.GetInternalEmployees();

            // Assert
            ActionResult<IEnumerable<InternalEmployeeDto>> actionResult = Assert
                .IsType<ActionResult<IEnumerable<InternalEmployeeDto>>>(result);
            OkObjectResult okObjectResult = Assert.IsType<OkObjectResult>(actionResult.Result);
            IEnumerable<InternalEmployeeDto> dtos = Assert.IsAssignableFrom<IEnumerable<InternalEmployeeDto>>
                (okObjectResult.Value);
            Assert.Equal(3,dtos.Count());
            InternalEmployeeDto firstEmployee = dtos.First();
            Assert.Equal(_firstEmployee.Id, firstEmployee.Id);
            Assert.Equal(_firstEmployee.FirstName, firstEmployee.FirstName);
            Assert.Equal(_firstEmployee.LastName, firstEmployee.LastName);
            Assert.Equal(_firstEmployee.Salary, firstEmployee.Salary);
            Assert.Equal(_firstEmployee.SuggestedBonus, firstEmployee.SuggestedBonus);
            Assert.Equal(_firstEmployee.YearsInService, firstEmployee.YearsInService);
            
            // FluentAssertions
            ActionResult<IEnumerable<InternalEmployeeDto>>? actionResult2 = result.Should()
                .BeOfType<ActionResult<IEnumerable<InternalEmployeeDto>>>().Subject;
            OkObjectResult okObjectResult2 = actionResult2.Result.Should().BeOfType<OkObjectResult>().Subject;
            IEnumerable<InternalEmployeeDto> dtos2 = okObjectResult2.Value
                .Should()
                .BeAssignableTo<IEnumerable<InternalEmployeeDto>>().Subject;
            dtos.Should().HaveCount(3, "because there should be exactly 3 employees returned");
            InternalEmployeeDto firstEmployee2 = dtos.First();
            firstEmployee2.Should().BeEquivalentTo(_firstEmployee, options => options
                .Including(e => e.Id)
                .Including(e => e.FirstName)
                .Including(e => e.LastName)
                .Including(e => e.Salary)
                .Including(e => e.SuggestedBonus)
                .Including(e => e.YearsInService)
            );

        }

        [Fact]
        public async Task GetInternalEmployees_GetAction_MustReturnOkObjectResult()
        {
            // Arrange
           
            // Act
            var result = await _internalEmployeesController.GetInternalEmployees();

            // Assert
            // Assert.IsType<OkObjectResult>(result.Result);

            using (new AssertionScope())
            {
                var actionResult = Assert
                 .IsType<ActionResult<IEnumerable<InternalEmployeeDto>>>(result);
                
                Assert.IsType<OkObjectResult>(actionResult.Result);
                
                // FluentAssertions
                actionResult.Should()
                    .BeOfType<ActionResult<IEnumerable<InternalEmployeeDto>>>()
                    .Which.Result
                    // actionResult.Result
                    .Should().BeOfType<OkObjectResult>();
            }

        }
        
        [Fact]
        public async Task GetInternalEmployees_GetAction_MustReturnIEnumerableOfInternalEmployeeDtoAsModelType()
        {
            // Arrange

            // Act 
            var result = await _internalEmployeesController.GetInternalEmployees();

            // Assert
            var actionResult = Assert
                .IsType<ActionResult<IEnumerable<InternalEmployeeDto>>>(result);

            Assert.IsAssignableFrom<IEnumerable<InternalEmployeeDto>>(
                ((OkObjectResult)actionResult.Result!).Value);
            
            // FluentAssertions
            var actionResult2 = result.Should()
                .BeOfType<ActionResult<IEnumerable<InternalEmployeeDto>>>().Subject;
            
            var okResult = actionResult2.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().BeAssignableTo<IEnumerable<InternalEmployeeDto>>();
        }
        
        [Fact]
        public async Task GetInternalEmployees_GetAction_MustReturnNumberOfInputtedInternalEmployees()
        {
            // Arrange

            // Act
            var result = await _internalEmployeesController.GetInternalEmployees();

            // Assert
            var actionResult = Assert
                .IsType<ActionResult<IEnumerable<InternalEmployeeDto>>>(result);
            
            Assert.Equal(3,
                ((IEnumerable<InternalEmployeeDto>)
                    ((OkObjectResult)actionResult.Result!).Value!).Count());
            
            // FluentAssertions
            var actionResult2 = result.Should()
                .BeOfType<ActionResult<IEnumerable<InternalEmployeeDto>>>().Subject;
            
            OkObjectResult? okResult = actionResult2.Result.Should().BeOfType<OkObjectResult>().Subject;

            ((IEnumerable<InternalEmployeeDto>)okResult.Value!).Should().HaveCount(3);
            
            // Correct use of Count with FluentAssertions
            var employees = okResult.Value as IEnumerable<InternalEmployeeDto>;
            employees.Should()
                .NotBeNull()
                .And
                .HaveCount(3);
            
        }
        
    }
}
