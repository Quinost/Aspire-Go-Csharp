namespace API.Shared.Config;

public sealed class ConnectionStringCfg(string connectionString)
{
    public string ConnectionString { get; } = connectionString;
}
