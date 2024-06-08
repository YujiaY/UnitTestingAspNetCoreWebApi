using EmployeeManagement.Business;
using EmployeeManagement.DataAccess.DbContexts;
using EmployeeManagement.DataAccess.Entities;
using EmployeeManagement.DataAccess.Services;
using EmployeeManagement.Services.Test;
using EmployeeManagement.Test.HttpMessageHandlers;
using FluentAssertions;
// using EmployeeManagement.Test.HttpMessageHandlers;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Xunit.Sdk;

namespace EmployeeManagement.Test
{
    public class TestIsolationApproachesTests
    {
        [Fact]
        public async Task AttendCourseAsync_CourseAttended_SuggestedBonusMustCorrectlyBeRecalculated()
        {
            // Arrange
            SqliteConnection connection = new("Data Source=:memory:");
            connection.Open();

            DbContextOptionsBuilder<EmployeeDbContext> optionsBuilder = 
                new DbContextOptionsBuilder<EmployeeDbContext>().UseSqlite(connection);

            EmployeeDbContext dbContext = new(optionsBuilder.Options);
            await dbContext.Database.MigrateAsync();

            EmployeeManagementRepository employeeManagementDataRepository = new(dbContext);

            EmployeeService employeeService = new(
                employeeManagementDataRepository,
                new EmployeeFactory());

            // get course from database - "Dealing with Customers 101"
            Course? courseToAttend = await employeeManagementDataRepository
                .GetCourseAsync(Guid.Parse("844e14ce-c055-49e9-9610-855669c9859b"));

            // get existing employee - "Megan Jones"
            InternalEmployee? internalEmployee = await employeeManagementDataRepository
                .GetInternalEmployeeAsync(Guid.Parse("72f2f5fe-e50c-4966-8420-d50258aefdcb"));

            if (courseToAttend == null || internalEmployee == null)
            {
                throw new XunitException("Arranging the test failed");
            }

            // expected suggested bonus after attending the course
            var expectedSuggestedBonus = internalEmployee.YearsInService
                * (internalEmployee.AttendedCourses.Count + 1) * 100;

            // Act
            await employeeService.AttendCourseAsync(internalEmployee, courseToAttend);

            // Assert
            Assert.Equal(expectedSuggestedBonus, internalEmployee.SuggestedBonus);
            
            internalEmployee.SuggestedBonus.Should().Be(expectedSuggestedBonus);
        }
        
        [Fact]
        public async Task PromoteInternalEmployeeAsync_IsEligible_JobLevelMustBeIncreased()
        {
            // Arrange
            var httpClient = new HttpClient(
                new TestablePromotionEligibilityHandler(true));
            var internalEmployee = new InternalEmployee(
                "Brooklyn", "Cannon", 5, 3000, false, 1);
            var promotionService = new PromotionService(httpClient,
                new EmployeeManagementTestDataRepository());

            // Act
            await promotionService.PromoteInternalEmployeeAsync(internalEmployee);

            // Assert
            Assert.Equal(2, internalEmployee.JobLevel);
        }
    }
}
