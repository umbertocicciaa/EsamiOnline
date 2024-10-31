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
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = config.GetSection("OnlineExamDatabase:ConnectionString").Value ?? "";
        
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