using Fast.ML.Preprocessor.Grpc;

namespace Fast.ML.Services.Interfaces;

public interface IPreprocessingService
{
    public Task<string> PreprocessDatasetAsync(PreprocessingRequest request, CancellationToken cancellationToken);
}