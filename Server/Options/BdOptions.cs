namespace Server.Options;

public class BdOptions
{
    public const string SectionName = "Bd";

    public string ConnectionString { get; set; } = string.Empty;
}
