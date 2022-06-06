using Fast.ML.Linear.Classification.Grpc;
using Fast.ML.Services.Interfaces;

namespace Fast.ML.Services.ModelServices.Classification;

using LinearClassificationServiceClient =
    Linear.Classification.Grpc.LinearClassificationService
    .LinearClassificationServiceClient;

public class LinearClassificationService : ILinearClassificationService
{
    private readonly LinearClassificationServiceClient _client;

    public LinearClassificationService(
        LinearClassificationServiceClient client) =>
        _client = client;

    private async Task<string> TrainAsync(
        TrainingRequest request, CancellationToken cancellationToken) =>
        (await _client.TrainAsync(request, cancellationToken: cancellationToken))
        .Configuration;

    private async Task<string> PredictAsync(
        PredictionRequest request, CancellationToken cancellationToken) =>
        (await _client.PredictAsync(request, cancellationToken: cancellationToken))
        .Message;

    public async Task<string> TrainAsync<TTrainingRequest>(
        TTrainingRequest request,
        CancellationToken cancellationToken) =>
        await TrainAsync((request as TrainingRequest)!, cancellationToken);

    public async Task<string> PredictAsync<TPredictionRequest>(
        TPredictionRequest request,
        CancellationToken cancellationToken) =>
        await PredictAsync((request as PredictionRequest)!, cancellationToken);
}