using EmployeeManagement.DataAccess.Entities;
using FluentAssertions;

namespace EmployeeManagement.Test;

public class EmployeeTests
{
    [Fact]
    public void EmployeeFullNamePropertyGetters_InputFirstNameAndLastName_FullNameIsConcatenation()
    {
        // Arrange
        var employee = new InternalEmployee("John", "Doe",0, 2500, false, 1);

        // Act
        employee.FirstName = "Lucia";
        employee.LastName = "ShElToN";
        

        // Assert
        Assert.Equal("Lucia Shelton", employee.FullName, ignoreCase: true);
        employee.FullName.ToLower().Should().Be("Lucia Shelton".ToLower());
        employee.FullName.Should().BeEquivalentTo("Lucia Shelton");

        // // Assert
        // var fullName = employee.FullName;

        // Assert.Equal("John Doe", fullName);
        // fullName.Should().Be("John Doe");
    }
    
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameStartsWithFirstName()
    {
        // Arrange
        var employee = new InternalEmployee("Kevin", "Dockx", 0, 2500, false, 1);

        // Act
        employee.FirstName = "Lucia";
        employee.LastName = "Shelton";

        // Assert
        Assert.StartsWith(employee.FirstName, employee.FullName);
        employee.FullName.Should().StartWithEquivalentOf(employee.FirstName);
    }
    
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameEndsWithLastName()
    {
        // Arrange
        var employee = new InternalEmployee("Kevin", "Dockx", 0, 2500, false, 1);

        // Act
        employee.FirstName = "Lucia";
        employee.LastName = "Shelton";

        // Assert
        Assert.EndsWith(employee.LastName, employee.FullName, StringComparison.OrdinalIgnoreCase);
        employee.FullName.Should().EndWithEquivalentOf(employee.LastName);
    }
    
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameContainsPartOfConcatenation()
    {
        // Arrange
        var employee = new InternalEmployee("Kevin", "Dockx", 0, 2500, false, 1);

        // Act
        employee.FirstName = "Lucia";
        employee.LastName = "Shelton";

        // Assert
        Assert.Contains("ia Sh", employee.FullName);
        employee.FullName.Should().Contain("ia Sh");
    }
    
    [Fact]
    public void EmployeeFullNamePropertyGetter_InputFirstNameAndLastName_FullNameSoundsLikeConcatenation()
    {
        // Arrange
        var employee = new InternalEmployee("Kevin", "Dockx", 0, 2500, false, 1);

        // Act
        employee.FirstName = "Lusia";
        employee.LastName = "Shelton";

        // Assert
        Assert.Matches("Lu(c|s|z)ia Shel(t|d)on", employee.FullName);
        employee.FullName.Should().MatchRegex("Lu(c|s|z)ia Shel(t|d)on");
    }
}