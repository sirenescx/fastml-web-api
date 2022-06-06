using Fast.ML.Contracts;

namespace Fast.ML.Services.Interfaces;

public interface IAlgorithmService
{
    public Task<AlgorithmsResponse> GetAlgorithmsAsync(CancellationToken token);
}