namespace ExamService.Configs;

public class ExamOnlineDatabaseSettings
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ExamCollectionName { get; set; } = null!;
}