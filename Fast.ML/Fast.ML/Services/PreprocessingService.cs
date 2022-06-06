using Fast.ML.Preprocessor.Grpc;
using Fast.ML.Services.Interfaces;
using PreprocessingServiceClient = 
    Fast.ML.Preprocessor.Grpc.PreprocessingService
    .PreprocessingServiceClient;

namespace Fast.ML.Services;

public class PreprocessingService : IPreprocessingService
{
    private readonly PreprocessingServiceClient _client;

    public PreprocessingService(PreprocessingServiceClient client) => _client = client;

    public async Task<string> PreprocessDatasetAsync(
        PreprocessingRequest request,
        CancellationToken cancellationToken)
    {
        return (await _client.PreprocessDatasetAsync(
            request, cancellationToken: cancellationToken)).Message;
    }
}