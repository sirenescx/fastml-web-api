using Fast.ML.Services.Interfaces;
using Fast.ML.SGDRegressor.Grpc;
using SGDRegressorServiceClient = Fast.ML.SGDRegressor.Grpc.SGDRegressorService.SGDRegressorServiceClient;

namespace Fast.ML.Services.ModelServices.Regression;

public class SGDRegressorService : ISGDRegressorService
{
    private readonly SGDRegressorServiceClient _client;

    public SGDRegressorService(SGDRegressorServiceClient client) => _client = client;

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