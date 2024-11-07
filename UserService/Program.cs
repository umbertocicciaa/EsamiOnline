using MongoDB.Driver;
using UserService.Configs;
using UserService.Mappers;
using UserService.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));
builder.Services.Configure<UserDatabasesSetting>(builder.Configuration.GetSection("UserDatabase"));
builder.Services.AddAutoMapper(typeof(UserMapperProfile).Assembly);

var connectionString = builder.Configuration.GetSection("OnlineExamDatabase:ConnectionString").Value;
var databaseName = builder.Configuration.GetSection("OnlineExamDatabase:DatabaseName").Value;

var client = new MongoClient(connectionString);
var database = client.GetDatabase(databaseName);
builder.Services.AddSingleton(database);

var app = builder.Build();

app.MapGrpcService<UserService.Services.UserService>();
// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();