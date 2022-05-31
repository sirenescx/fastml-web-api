using Fast.ML.Lasso.Grpc;
using Fast.ML.Services.Interfaces;
using LassoServiceClient = Fast.ML.Lasso.Grpc.LassoService.LassoServiceClient;

namespace Fast.ML.Services.ModelServices.Regression;

public class LassoRegressionService : ILassoRegressionService
{
    private readonly LassoServiceClient _client;

    public LassoRegressionService(LassoServiceClient client) => _client = client;

    private async Task<string> TrainAsync(TrainingRequest request, CancellationToken cancellationToken) =>
        (await _client.TrainAsync(request, cancellationToken: cancellationToken)).Configuration;

    private async Task<string> PredictAsync(PredictionRequest request, CancellationToken cancellationToken) =>
        (await _client.PredictAsync(request, cancellationToken: cancellationToken)).Message;

    public async Task<string> TrainAsync<TTrainingRequest>(
        TTrainingRequest request,
        CancellationToken cancellationToken) => await TrainAsync((request as TrainingRequest)!, cancellationToken);

    public async Task<string> PredictAsync<TPredictionRequest>(
        TPredictionRequest request,
        CancellationToken cancellationToken) => await PredictAsync((request as PredictionRequest)!, cancellationToken);
}