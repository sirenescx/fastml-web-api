using System.Text.Json.Serialization;

namespace Fast.ML.Models;

public class Algorithm
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    [JsonPropertyName("json_configuration")]
    public string JsonConfiguration { get; set; }
    
    [JsonPropertyName("problem_type_id")]
    public int ProblemTypeId { get; set; }
}