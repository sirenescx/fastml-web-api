using Fast.ML.Contracts;
using Fast.ML.Registries.Interfaces;
using Fast.ML.Services.Interfaces;

namespace Fast.ML.Services;

public class AlgorithmService : IAlgorithmService
{
    private readonly IAlgorithmRepository _repository;

    public AlgorithmService(IAlgorithmRepository repository) => _repository = repository;

    public async Task<AlgorithmsResponse> GetAlgorithmsAsync(CancellationToken token)
    {
        var algorithms = await _repository.GetAlgorithmsAsync();
        return new AlgorithmsResponse
        {
            Algorithms = algorithms
        };
    }
}