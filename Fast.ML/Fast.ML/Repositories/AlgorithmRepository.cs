using Dapper;
using Fast.ML.Database;
using Fast.ML.Models;
using Fast.ML.Registries.Interfaces;
using Endpoint = Fast.ML.Models.Endpoint;

namespace Fast.ML.Registries;

public class AlgorithmRepository : IAlgorithmRepository
{
    private readonly DapperContext _context;

    public AlgorithmRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Algorithm>> GetAlgorithmsAsync()
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Algorithm>(GetAlgorithmsQuery);
    }

    public async Task<IEnumerable<Endpoint>> GetEndpointsAsync()
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<Endpoint>(GetEndpointsQuery);
    }

    public async Task<Endpoint> GetEndpointByAlgorithmAsync(Algorithm algorithm)
    {
        var parameters = new {AlgorithmId = algorithm.Id};
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Endpoint>(
            GetEndpointByAlgorithmQuery, parameters);
    }
    
    #region SqlQueries

    private const string GetEndpointsQuery = "select * from fastml.dbo.endpoints";

    private const string GetEndpointByAlgorithmQuery = 
        @"select * from fastml.dbo.endpoints 
            join fastml.dbo.algorithms_endpoints on 
                endpoints.id = algorithms_endpoints.endpoint_id 
            where algorithm_id = @AlgorithmId";

    private const string GetAlgorithmsQuery = "select * from fastml.dbo.algorithms";

    #endregion
}