namespace Fast.ML.Services.Interfaces;

public interface IModelService
{
    public Task<string> TrainAsync<TTrainingRequest>(TTrainingRequest request, CancellationToken cancellationToken);
    
    public Task<string> PredictAsync<TPredictionRequest>(TPredictionRequest request, CancellationToken cancellationToken);
}

#region Interfaces

public interface ILinearRegressionService : IModelService
{
}

public interface IRidgeRegressionService : IModelService
{
}

public interface ILassoRegressionService : IModelService
{
}

public interface IElasticNetRegressionService : IModelService
{
}

public interface ISGDRegressorService : IModelService
{
}

public interface ILogisticRegressionService : IModelService
{
}

#endregion



