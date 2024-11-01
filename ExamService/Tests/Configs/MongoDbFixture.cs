using ExamService.Configs;
using MongoDB.Driver;

namespace ExamService.Tests.Configs;

public class MongoDbFixture : IDisposable
{
    public ExamOnlineDatabaseSettings DbContextSettings { get; }

    public MongoDbFixture()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = config.GetSection("OnlineExamDatabaseTest:ConnectionString").Value ?? "";
       
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