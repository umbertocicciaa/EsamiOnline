using ExamService.Configs;
using ExamService.Mappers;
using ExamService.Repositories;
using ExamService.Services;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.Configure<ExamOnlineDatabaseSettings>(builder.Configuration.GetSection("OnlineExamDatabase"));
builder.Services.AddAutoMapper(typeof(ExamMappingProfile).Assembly);

var connectionString = builder.Configuration.GetSection("OnlineExamDatabase:ConnectionString").Value;
var databaseName = builder.Configuration.GetSection("OnlineExamDatabase:DatabaseName").Value;

var client = new MongoClient(connectionString);
var database = client.GetDatabase(databaseName);

// Register the MongoDB context or direct collections
builder.Services.AddSingleton(database);
builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

var app = builder.Build();

app.MapGrpcService<ExamsService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run();