using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.Entities;
using FluentAssertions;

namespace EmployeeManagement.Test;

public class EmployeeFactoryTests: IDisposable
{
    private EmployeeFactory _employeeFactory;
    public EmployeeFactoryTests()
    {
        _employeeFactory = new EmployeeFactory();
    }

    public void Dispose()
    {
        // clean up the setup code, if required
    }
    
    // [Fact]
    [Fact(Skip = "Skipping this one for demo reasons.")]
    [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
    public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500()
    {
        // Arrange
        

        // Act
        var employee = (InternalEmployee)_employeeFactory.CreateEmployee("John", "Doe");

        // Assert with xUnit
        Assert.Equal(2500, employee.Salary);
        // Assert with FluentAssertions
        employee.Salary.Should().Be(2500);
        // employee.FullName.Should().Be("John Doe");
    }
    
    [Fact]
    [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
    public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500()
    {
        // Arrange 
        

        // Act
        var employee = (InternalEmployee)_employeeFactory
            .CreateEmployee("Kevin", "Dockx");

        // Assert
        Assert.True(employee.Salary >= 2500 && employee.Salary <= 3500, 
            "Salary not in acceptable range.");
        
        employee.Salary.Should()
            .BeGreaterThanOrEqualTo(2500, "Salary not in acceptable range.")
            .And
            .BeLessThanOrEqualTo(3500, "Salary not in acceptable range.");
    }
    
    [Fact]
    [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
    public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500_Alternative()
    {
        // Arrange 
        

        // Act
        var employee = (InternalEmployee)_employeeFactory
            .CreateEmployee("Kevin", "Dockx");

        // Assert
        Assert.True(employee.Salary >= 2500);
        Assert.True(employee.Salary <= 3500);
    }
    
    [Fact]
    [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
    public void CreateEmployee_ConstructInternalEmployee_SalaryMustBeBetween2500And3500_AlternativeWithInRange()
    {
        // Arrange 
        

        // Act
        var employee = (InternalEmployee)_employeeFactory
            .CreateEmployee("Kevin", "Dockx");

        // Assert
        Assert.InRange(employee.Salary, 2500, 3500);
        employee.Salary.Should().BeInRange(2500, 3500);
    }
    
    [Fact]
    [Trait("Category", "EmployeeFactory_CreateEmployee_Salary")]
    public void CreateEmployee_ConstructInternalEmployee_SalaryMustBe2500_PrecisionExample()
    {
        // Arrange 
        

        // Act
        var employee = (InternalEmployee)_employeeFactory
            .CreateEmployee("Kevin", "Dockx");
        employee.Salary = 2500.323m;

        // Assert
        Assert.Equal(2500, employee.Salary, 0);
        employee.Salary.Should().BeApproximately(2500, 0.33m);
    }
    
    [Fact]
    [Trait("Category", "EmployeeFactory_CreateEmployee_ReturnType")]
    public void CreateEmployee_IsExternalIsTrue_ReturnTypeMustBeExternalEmployee()
    {
        // Arrange 
        

        // Act
        var employee = _employeeFactory
            .CreateEmployee("Kevin", "Dockx", "Marvin", true);

        // Assert
        Assert.IsType<ExternalEmployee>(employee);
        Assert.IsAssignableFrom<Employee>(employee);

        employee.Should().BeOfType<ExternalEmployee>();
        employee.Should().BeAssignableTo<Employee>();
    }
}