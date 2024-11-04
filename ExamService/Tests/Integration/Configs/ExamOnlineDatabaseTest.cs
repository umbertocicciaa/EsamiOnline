using MongoDB.Driver;
using Xunit;

namespace ExamService.Tests.Configs;

public class ExamOnlineDatabaseTest(MongoDbFixture fixture) : IClassFixture<MongoDbFixture>
{
    [Fact]
    public void TestDatabaseConnection()
    {
        // Arrange
        var client = new MongoClient(fixture.DbContextSettings.ConnectionString);
        // Act
        var databases = client.ListDatabaseNames().ToList();
        // Assert
        Assert.NotNull(databases);
    }
}