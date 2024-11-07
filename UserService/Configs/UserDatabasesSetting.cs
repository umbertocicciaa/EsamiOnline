namespace UserService.Configs;

public class UserDatabasesSetting
{
    public string ConnectionString { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;

    public string ExamCollectionName { get; set; } = null!;
}