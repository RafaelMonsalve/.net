public class AppSettings
{
    public ApiSettings ApiSettings { get; set; } = new ApiSettings();
    public DatabaseSettings DatabaseSettings { get; set; } = new DatabaseSettings();
    public ServiceSettings ServiceSettings { get; set; } = new ServiceSettings();
}

public class ApiSettings
{
    public string Url { get; set; }
    public string WhoHeader { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class DatabaseSettings
{
    public string ConnectionString { get; set; }
}

public class ServiceSettings
{
    public int IntervalInMinutes { get; set; }
}