namespace BbcTravelMs.Infrastructure.Settings;

public class AzureServiceBusSettings
{
    public string ConnectionString { get; set; }

    public List<string> Queues { get; set; }

    public List<string> Topics { get; set; }
}
