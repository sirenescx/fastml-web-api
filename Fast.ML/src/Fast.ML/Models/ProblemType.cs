using System.Text.Json.Serialization;

namespace Fast.ML.Models;

public class ProblemType
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    
    [JsonPropertyName("problem_name")]
    public int ProblemName { get; set; }
}