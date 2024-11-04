using JetBrains.Annotations;
using MongoDB.Driver;
using Testcontainers.MongoDb;
using Xunit;

namespace ExamService.Tests.Integration.Configs;

public class ExamOnlineDatabaseTest(MongoDbContainer container) : IAsyncLifetime
{
    [Fact]
    public void ConnectionStateReturnsOpen()
    {
        // Given
        var client = new MongoClient(container.GetConnectionString());

        // When
        using var databases = client.ListDatabases();

        // Then
        Assert.Contains(databases.ToEnumerable(),
            database => database.TryGetValue("name", out var name) && "admin".Equals(name.AsString));
    }

    [UsedImplicitly]
    public sealed class MongoDbNoAuthConfiguration()
        : ExamOnlineDatabaseTest(new MongoDbBuilder().WithUsername(string.Empty).WithPassword(string.Empty).Build());

    public Task InitializeAsync()
    {
        return container.StartAsync();
    }

    public Task DisposeAsync()
    {
        return container.DisposeAsync().AsTask();
    }
}