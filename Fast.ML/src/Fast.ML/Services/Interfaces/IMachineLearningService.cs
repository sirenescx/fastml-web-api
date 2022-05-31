using Fast.ML.Worker.Grpc;

namespace Fast.ML.Services.Interfaces;

public interface IMachineLearningService
{
    public Task<string> Train(TrainingRequest request, CancellationToken token);

    public Task<string> Predict(PredictionRequest request, CancellationToken token);
}