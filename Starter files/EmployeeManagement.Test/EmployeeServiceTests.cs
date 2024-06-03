using EmployeeManagement.Business;
using EmployeeManagement.Business.EventArguments;
using EmployeeManagement.Business.Exceptions;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.Services.Test;
using EmployeeManagement.Test.Fixtures;
using FluentAssertions;
using FluentAssertions.Execution;

namespace EmployeeManagement.Test;

public class EmployeeServiceTests: IClassFixture<EmployeeServiceFixture>
{
    private readonly EmployeeServiceFixture _employeeServiceFixture;

    public EmployeeServiceTests(EmployeeServiceFixture employeeServiceFixture)
    {
        _employeeServiceFixture = employeeServiceFixture;
    }
    
    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse_WithObject()
    {
        // Arrange      
        // var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        // var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());

        var obligatoryCourse = _employeeServiceFixture.EmployeeManagementTestDataRepository
            .GetCourse(Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        // Assuming GetCourse always returns a non-null value for valid IDs in the test setup
        obligatoryCourse.Should()
            .NotBeNull("the obligatory course must be initialized correctly for the test to be valid");
        
        // Act
        var internalEmployee = _employeeServiceFixture.EmployeeService
            .CreateInternalEmployee("Brooklyn", "Cannon");

        // Assert
        Assert.Contains(obligatoryCourse, internalEmployee.AttendedCourses);
        
        internalEmployee.AttendedCourses
            .Should().Contain(obligatoryCourse!);
    }
    
    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedFirstObligatoryCourse_WithPredicate()
    {
        // Arrange            
        // var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        // var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());

        // Act
        var internalEmployee = _employeeServiceFixture.EmployeeService
            .CreateInternalEmployee("Brooklyn", "Cannon");

        // Assert
        Assert.Contains(internalEmployee.AttendedCourses,
            course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
        
        internalEmployee.AttendedCourses.Should()
            .Contain(course => course.Id == Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"));
    }
    
    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_MustHaveAttendedSecondObligatoryCourse_WithPredicate()
    {
        // Arrange            
        // var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        // var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());

        // Act
        var internalEmployee = _employeeServiceFixture.EmployeeService
            .CreateInternalEmployee("Brooklyn", "Cannon");

        // Assert
        Assert.Contains(internalEmployee.AttendedCourses,
            course => course.Id == Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
        
        internalEmployee.AttendedCourses.Should()
            .Contain(course => course.Id == Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));
    }
    
    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourses()
    {
        // Arrange 
        // var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        // var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());

        var obligatoryCourses = _employeeServiceFixture.EmployeeManagementTestDataRepository
            .GetCourses(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

        // Act
        var internalEmployee = _employeeServiceFixture.EmployeeService
            .CreateInternalEmployee("Brooklyn", "Cannon");

        // Assert
        Assert.Equal(obligatoryCourses, internalEmployee.AttendedCourses);
        
        internalEmployee.AttendedCourses.Should()
            .BeEquivalentTo(obligatoryCourses);
    }
    
    [Fact]
    public void CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustNotBeNew()
    {
        // Arrange 
        // var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        // var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());
        // Act
        var internalEmployee = _employeeServiceFixture.EmployeeService.CreateInternalEmployee("Brooklyn", "Cannon");

        // Assert
        //foreach (var course in internalEmployee.AttendedCourses)
        //{
        //    Assert.False(course.IsNew);
        //}
        // Below will pass
        Assert.All(internalEmployee.AttendedCourses,
            course => Assert.False(course.IsNew));
        // Below will pass
        internalEmployee.AttendedCourses.Should()
            .NotBeEmpty("because we expect to have courses to check")
            .And
            .AllSatisfy(course => course.IsNew.Should().BeFalse());

        var result1 = new List<Course> {};

        // Below will pass
        Assert.All(result1,
            course => Assert.False(course.IsNew));
        
        // Below will fail
        // result1.Should().AllSatisfy(course => course.IsNew.Should().BeFalse());
    }
    
    [Fact]
    public async Task CreateInternalEmployee_InternalEmployeeCreated_AttendedCoursesMustMatchObligatoryCourses_Async()
    {
        // Arrange
        // var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        // var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());

        var obligatoryCourses = await _employeeServiceFixture.EmployeeManagementTestDataRepository
            .GetCoursesAsync(
                Guid.Parse("37e03ca7-c730-4351-834c-b66f280cdb01"),
                Guid.Parse("1fd115cf-f44c-4982-86bc-a8fe2e4ff83e"));

        // Act
        var internalEmployee = await _employeeServiceFixture.EmployeeService
            .CreateInternalEmployeeAsync("Brooklyn", "Cannon");

        // Assert
        Assert.Equal(obligatoryCourses, internalEmployee.AttendedCourses);
    }
    
    [Fact]
    public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown()
    {
        // Arrange  
        // var employeeManagementTestDataRepository = new EmployeeManagementTestDataRepository();
        // var employeeService = new EmployeeService(employeeManagementTestDataRepository, new EmployeeFactory());

        var internalEmployee = new InternalEmployee(
            "Brooklyn", "Cannon", 5, 3000, false, 1);

        // Act & Assert
        await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
            async () => 
                await _employeeServiceFixture.EmployeeService
                    .GiveRaiseAsync(internalEmployee, 50)
        );
        
        await FluentActions
            .Awaiting(async () => await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 50))
            .Should().ThrowAsync<EmployeeInvalidRaiseException>("because the raise amount is invalid");
    }
    
    [Fact]
    public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown_MistakexUnit()
    {
        // Arrange 
        // var employeeService = new EmployeeService(
        //     new EmployeeManagementTestDataRepository(),
        //     new EmployeeFactory());
        var internalEmployee = new InternalEmployee(
            "Brooklyn", "Cannon", 5, 3000, false, 1);
        
        // Act & Assert correct way with await
        EmployeeInvalidRaiseException task = await Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
            async () =>
                await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 50)
        );
        
        EmployeeInvalidRaiseException task2 = await Assert.ThrowsAnyAsync<EmployeeInvalidRaiseException>(
            async () =>
                await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 50)
        );
        
        await Assert.ThrowsAnyAsync<Exception>(
            async () =>
                await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 50)
        );
        
        // await Assert.ThrowsAsync<Exception>(
        //     async () =>
        //         await employeeService.GiveRaiseAsync(internalEmployee, 50)
        // );
        
        // Act & Assert as wrong way without outer await
        //   Assert.ThrowsAsync<EmployeeInvalidRaiseException>(
        //         async () =>
        //       await employeeService.GiveRaiseAsync(internalEmployee, 50)
        //     );
    }
    
    [Fact]
    public async Task GiveRaise_RaiseBelowMinimumGiven_EmployeeInvalidRaiseExceptionMustBeThrown_MistakeFluent()
    {
        // Arrange 
        // var employeeService = new EmployeeService(
        //     new EmployeeManagementTestDataRepository(),
        //     new EmployeeFactory());
        var internalEmployee = new InternalEmployee(
            "Brooklyn", "Cannon", 5, 3000, false, 1);
        
        // Act & Assert as correct way with async Task
        using (new AssertionScope())
        {
            // await FluentActions
            //     .Awaiting(async () => await employeeService.GiveRaiseAsync(internalEmployee, 10))
            //     .Should().ThrowAsync<EmployeeInvalidRaiseException>("because the raise amount is invalid");
            //
            // await FluentActions
            // .Awaiting(async () => await employeeService.GiveRaiseAsync(internalEmployee, 10))
            // .Should().ThrowExactlyAsync<EmployeeInvalidRaiseException>("because the raise amount is invalid");
        
            // await FluentActions
            // .Awaiting(async () => await employeeService.GiveRaiseAsync(internalEmployee, 10))
            // .Should().ThrowAsync<Exception>("because the raise amount is invalid");
        
            await FluentActions
            .Awaiting(async () => await _employeeServiceFixture.EmployeeService.GiveRaiseAsync(internalEmployee, 10))
            .Should().ThrowExactlyAsync<EmployeeInvalidRaiseException>("because the raise amount is invalid");
        }
    }
    
    [Fact]
    public void NotifyOfAbsence_EmployeeIsAbsent_OnEmployeeIsAbsentMustBeTriggered()
    {
        // Arrange
        // var employeeService = new EmployeeService(
        //     new EmployeeManagementTestDataRepository(),
        //     new EmployeeFactory());
        var internalEmployee = new InternalEmployee(
            "Brooklyn", "Cannon", 5, 3000, false, 1);
        var newId = Guid.NewGuid();
        internalEmployee.Id = newId;
        
        // Act & Assert
        Assert.Raises<EmployeeIsAbsentEventArgs>(
            handler => _employeeServiceFixture.EmployeeService
                .EmployeeIsAbsent += handler,
            handler => _employeeServiceFixture.EmployeeService
                .EmployeeIsAbsent -= handler,
            () => _employeeServiceFixture.EmployeeService
                .NotifyOfAbsence(internalEmployee));
        
        Assert.RaisesAny<EmployeeIsAbsentEventArgs>(
            handler => _employeeServiceFixture.EmployeeService
                .EmployeeIsAbsent += handler,
            handler => _employeeServiceFixture.EmployeeService
                .EmployeeIsAbsent -= handler,
            () => _employeeServiceFixture.EmployeeService
                .NotifyOfAbsence(internalEmployee));
        
        // Using FluentAssertions
        // Arrange for FluentAssertions
        var monitor = _employeeServiceFixture.EmployeeService.Monitor();
        
        // Act
        _employeeServiceFixture.EmployeeService.NotifyOfAbsence(internalEmployee);

        // Assert
        monitor.Should().Raise("EmployeeIsAbsent")
            .WithSender(_employeeServiceFixture.EmployeeService)//; // Optionally check the sender
            .WithArgs<EmployeeIsAbsentEventArgs>(args => args.EmployeeId == internalEmployee.Id);
    }
}