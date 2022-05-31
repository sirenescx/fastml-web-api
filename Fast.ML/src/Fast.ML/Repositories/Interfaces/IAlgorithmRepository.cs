using Fast.ML.Models;
using Endpoint = Fast.ML.Models.Endpoint;

namespace Fast.ML.Registries.Interfaces;

public interface IAlgorithmRepository
{
    public Task<IEnumerable<Algorithm>> GetAlgorithmsAsync();
    
    public Task<IEnumerable<Endpoint>> GetEndpointsAsync();
    
    public Task<Endpoint> GetEndpointByAlgorithmAsync(Algorithm algorithm);
}