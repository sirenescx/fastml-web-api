using Newtonsoft.Json;

namespace Fast.ML.Models;

public class ModelConfiguration
{
    [JsonProperty("id")]
    public int Id { get; set; }
    
    [JsonProperty("algorithm_id")]
    public int AlgorithmId { get; set; }
    
    [JsonProperty("user_id")]
    public int UserId { get; set; }
    
    [JsonProperty("configuration")]
    public byte[] Configuration { get; set; }
}