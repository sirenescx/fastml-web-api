using System.Text.Json.Serialization;

namespace Fast.ML.Models;

public class Endpoint
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("service_name")]
    public string ServiceName { get; set; }
    
    [JsonPropertyName("uri")]
    public string Uri { get; set; }
    
    [JsonPropertyName("timeout_ms")]
    public int? TimeoutMs { get; set; }
}