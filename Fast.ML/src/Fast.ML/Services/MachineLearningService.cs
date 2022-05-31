using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using Fast.ML.Hubs;
using Fast.ML.Services.Interfaces;
using Fast.ML.Worker.Grpc;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.AspNetCore.SignalR;
using ServiceStack;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Fast.ML.Services;

public class MachineLearningService : IMachineLearningService
{
    private readonly ILogger<MachineLearningService> _logger;
    private readonly IPreprocessingService _preprocessingService;
    private readonly Dictionary<string, IModelService> _modelServices = new();
    private readonly PageUpdateHub _hub;

    public MachineLearningService(
        ILogger<MachineLearningService> logger,
        IPreprocessingService preprocessingService,
        ILinearRegressionService linearRegressionService,
        IRidgeRegressionService ridgeRegressionService,
        ILassoRegressionService lassoRegressionService,
        IElasticNetRegressionService elasticNetRegressionService,
        ISGDRegressorService sgdRegressorService,
        ILogisticRegressionService logisticRegressionService,
        PageUpdateHub pageUpdateHub)
    {
        _logger = logger;
        _preprocessingService = preprocessingService;
        
        _modelServices[Algorithm.LinearRegression] = linearRegressionService;
        _modelServices[Algorithm.Ridge] = ridgeRegressionService;
        _modelServices[Algorithm.Lasso] = lassoRegressionService;
        _modelServices[Algorithm.ElasticNet] = elasticNetRegressionService;
        _modelServices[Algorithm.SGDRegressor] = sgdRegressorService;

        _modelServices[Algorithm.LogisticRegression] = logisticRegressionService;

        _hub = pageUpdateHub;
    }

    public async Task<string> Train(TrainingRequest request, CancellationToken token)
    {
        request.Filename = MoveFile(request.Filename, request.TaskId);
        var preprocessingRequest = new Preprocessor.Grpc.PreprocessingRequest
        {
            Filename = request.Filename,
            Separator = request.Separator,
            ProblemType = request.ProblemType,
            Target = request.Target,
            Index = request.Index,
            Mode = Mode.Train,
        };
        
        try
        {
            request.Filename = await _preprocessingService.PreprocessDatasetAsync(preprocessingRequest, token);
            _logger.LogInformation("Training dataset was successfully preprocessed");
        }
        catch (Exception)
        {
            _logger.LogError("Training dataset was preprocessed successfully");
        }

        foreach (var algorithm in request.Algorithms)
        {
            await _modelServices[algorithm]
                .TrainAsync(GetTrainingRequest(algorithm, request.Filename), token);
        }

        await _hub.SendMessage(request.TaskId, "train");

        return "OK";
    }

    public async Task<string> Predict(PredictionRequest request, CancellationToken token)
    {
        request.Filename = MoveFile(request.Filename, request.TaskId);
        var preprocessingRequest = new Preprocessor.Grpc.PreprocessingRequest
        {
            Filename = request.Filename,
            Separator = request.Separator,
            Index = request.Index,
            Mode = Mode.Predict
        };

        request.Filename = await _preprocessingService.PreprocessDatasetAsync(preprocessingRequest, token);

        foreach (var algorithm in request.Algorithms)
        {
            var predictionsPath = await _modelServices[algorithm]
                .PredictAsync(GetPredictionRequest(algorithm, request.Filename), token);
            await _hub.SendMessage(predictionsPath, "model");
        }

        return "OK";
    }

    private static string MoveFile(string filePath, string taskId)
    {
        var path = Path.GetDirectoryName(filePath);
        var taskPath = Path.Combine(path!, taskId);
        if (!Directory.Exists(taskPath))
            Directory.CreateDirectory(taskPath);
        var newFilePath = Path.Combine(taskPath, Path.GetFileName(filePath));
        File.Move(filePath, newFilePath);

        return newFilePath;
    }

    private static dynamic GetTrainingRequest(string algorithm, string filepath) =>
        algorithm switch
        {
            Algorithm.LinearRegression => new LinearRegression.Grpc.TrainingRequest {Filepath = filepath},
            Algorithm.Ridge => new Ridge.Grpc.TrainingRequest {Filepath = filepath},
            Algorithm.Lasso => new Lasso.Grpc.TrainingRequest {Filepath = filepath},
            Algorithm.ElasticNet => new ElasticNet.Grpc.TrainingRequest {Filepath = filepath},
            Algorithm.SGDRegressor => new SGDRegressor.Grpc.TrainingRequest {Filepath = filepath},
            
            Algorithm.LogisticRegression => new LogisticRegression.Grpc.TrainingRequest {Filepath = filepath},
            _ => new Ridge.Grpc.TrainingRequest {Filepath = filepath}
        };
    
    private static dynamic GetPredictionRequest(string algorithm, string filepath) =>
        algorithm switch
        {
            Algorithm.LinearRegression => new LinearRegression.Grpc.PredictionRequest {Filepath = filepath},
            Algorithm.Ridge => new Ridge.Grpc.PredictionRequest {Filepath = filepath},
            Algorithm.Lasso => new Lasso.Grpc.PredictionRequest {Filepath = filepath},
            Algorithm.ElasticNet => new ElasticNet.Grpc.PredictionRequest {Filepath = filepath},
            Algorithm.SGDRegressor => new SGDRegressor.Grpc.PredictionRequest {Filepath = filepath},
            
            Algorithm.LogisticRegression => new LogisticRegression.Grpc.PredictionRequest {Filepath = filepath},
            _ => new Ridge.Grpc.PredictionRequest {Filepath = filepath}
        };

    #region Constants

    private class Mode
    {
        public const string Train = "train";
        public const string Predict = "predict";
    }

    private class Algorithm
    {
        public const string LinearRegression = "alg_LinearRegression";
        public const string Ridge = "alg_Ridge";
        public const string SGDRegressor = "alg_SGDRegressor";
        public const string ElasticNet = "alg_ElasticNet";
        public const string Lars = "alg_Lars";
        public const string Lasso = "alg_Lasso";
        public const string LassoLars = "alg_LassoLars";
        public const string OrthogonalMatchingPursuit = "alg_OrthogonalMatchingPursuit";
        public const string ARDRegression = "alg_ARDRegression";
        public const string BayesianRidge = "alg_BayesianRidge";
        public const string HuberRegressor = "alg_HuberRegressor";
        public const string QuantileRegressor = "alg_QuantileRegressor";
        public const string RANSACRegressor = "alg_RANSACRegressor";

        public const string LogisticRegression = "alg_LogisticRegression";
    }

    #endregion
}