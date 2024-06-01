using EmployeeManagement.DataAccess.Entities;
using FluentAssertions;

namespace EmployeeManagement.Test;

public class CourseTests
{
    [Fact]
    public void CourseConstructor_ConstructCourse_IsNewMustBeTrue()
    {
        // Arrange
        // Nothing to arrange here
        
        // Act
        var course = new Course("C# Fundamentals");
        
        // Assert
        Assert.True(course.IsNew);
        course.IsNew.Should().Be(true);
    }
    
}