using ExamService.Configs;
using ExamService.Mappers;
using ExamService.Repositories;
using ExamService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped<IExamRepository,ExamRepository>();
builder.Services.Configure<ExamOnlineDatabaseSettings>(builder.Configuration.GetSection("OnlineExamDatabase"));
builder.Services.AddAutoMapper(typeof(ExamMappingProfile).Assembly);

var app = builder.Build();

app.MapGrpcService<ExamsService>();

app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
app.Run();