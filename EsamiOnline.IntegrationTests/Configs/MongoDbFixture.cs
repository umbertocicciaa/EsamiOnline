using EsamiOnline.Configs;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
namespace EsamiOnline.IntegrationTests.Configs;

public class MongoDbFixture : IDisposable
{
    public ExamOnlineDatabaseSettings DbContextSettings { get; }

    public MongoDbFixture()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config.GetSection("ConnectionStrings:db").Value ?? "";
       
        DbContextSettings = new ExamOnlineDatabaseSettings
        {
            ConnectionString = connectionString,
            DatabaseName = $"test_db_{Guid.NewGuid()}",
            ExamCollectionName = "Exams"
        };
    }

    public void Dispose()
    {
        var client = new MongoClient(DbContextSettings.ConnectionString);
        client.DropDatabase(DbContextSettings.DatabaseName);
    }
}