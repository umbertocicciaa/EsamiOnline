using MongoDB.Driver;
using Testcontainers.MongoDb;
using Xunit;

namespace ExamService.Tests.Integration.Configs;

public class ExamOnlineDatabaseTest : IAsyncLifetime
{
    private readonly MongoDbContainer _container = new MongoDbBuilder().Build();

    public Task InitializeAsync()
    {
        return _container.StartAsync();
    }

    public Task DisposeAsync()
    {
        return _container.DisposeAsync().AsTask();
    }
    
    [Fact]
    public void ConnectionStateReturnsOpen()
    {
        // Given
        var client = new MongoClient(_container.GetConnectionString());

        // When
        using var databases = client.ListDatabases();

        // Then
        Assert.Contains(databases.ToEnumerable(),
            database => database.TryGetValue("name", out var name) && "admin".Equals(name.AsString));
    }
    
}