using Fast.ML.Models;

namespace Fast.ML.Contracts;

public class AlgorithmsResponse
{
    public IEnumerable<Algorithm> Algorithms { get; set; }
}