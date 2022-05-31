using Fast.ML.LogisticRegression.Grpc;
using Fast.ML.Services.Interfaces;

namespace Fast.ML.Services.ModelServices.Classification;

using LogisticRegressionServiceClient =
    LogisticRegression.Grpc.LogisticRegressionService.LogisticRegressionServiceClient;

public class LogisticRegressionService : ILogisticRegressionService
{
    private readonly LogisticRegressionServiceClient _client;

    public LogisticRegressionService(LogisticRegressionServiceClient client) => _client = client;

    private async Task<string> TrainAsync(TrainingRequest request, CancellationToken cancellationToken) =>
        (await _client.TrainAsync(request, cancellationToken: cancellationToken)).Configuration;

    private async Task<string> PredictAsync(PredictionRequest request, CancellationToken cancellationToken) =>
        (await _client.PredictAsync(request, cancellationToken: cancellationToken)).Message;

    public async Task<string> TrainAsync<TTrainingRequest>(
        TTrainingRequest request,
        CancellationToken cancellationToken) =>
        await TrainAsync((request as TrainingRequest)!, cancellationToken);

    public async Task<string> PredictAsync<TPredictionRequest>(
        TPredictionRequest request,
        CancellationToken cancellationToken) =>
        await PredictAsync((request as PredictionRequest)!, cancellationToken);
}