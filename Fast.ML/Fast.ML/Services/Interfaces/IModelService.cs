namespace Fast.ML.Services.Interfaces;

public interface IModelService
{
    public Task<string> TrainAsync<TTrainingRequest>(
        TTrainingRequest request, CancellationToken cancellationToken);
    
    public Task<string> PredictAsync<TPredictionRequest>(
        TPredictionRequest request, CancellationToken cancellationToken);
}

#region Interfaces

public interface ILinearRegressionService : IModelService
{
}

public interface ILinearClassificationService : IModelService
{
}

#endregion



