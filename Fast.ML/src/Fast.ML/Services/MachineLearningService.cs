using Fast.ML.Hubs;
using Fast.ML.Preprocessor.Grpc;
using Fast.ML.Services.Interfaces;
using Fast.ML.Worker.Grpc;

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
        PageUpdateHub pageUpdateHub)
    {
        _logger = logger;
        _preprocessingService = preprocessingService;
        
        _modelServices[MicroserviceType.LinearRegression] = linearRegressionService;

        _hub = pageUpdateHub;
    }

    public async Task<string> Train(TrainingRequest request, CancellationToken token)
    {
        request.Filename = MoveFile(request.Filename, request.TaskId);
        var preprocessingRequest = new PreprocessingRequest
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
        catch (Exception e)
        {
            _logger.LogError($"Dataset preprocessing failed {e.StackTrace}");
        }

        foreach (var algorithm in request.Algorithms)
        {
            var algorithmWithoutPrefix = algorithm.Replace("alg_", string.Empty);
            await _modelServices[MicroserviceType.LinearRegression]
                .TrainAsync(GetTrainingRequest(algorithmWithoutPrefix, request.Filename), token);
        }

        await _hub.SendMessage(request.TaskId, "train");

        return "OK";
    }

    public async Task<string> Predict(PredictionRequest request, CancellationToken token)
    {
        request.Filename = MoveFile(request.Filename, request.TaskId);
        var preprocessingRequest = new PreprocessingRequest
        {
            Filename = request.Filename,
            Separator = request.Separator,
            Index = request.Index,
            Mode = Mode.Predict
        };

        request.Filename = await _preprocessingService.PreprocessDatasetAsync(preprocessingRequest, token);

        foreach (var algorithm in request.Algorithms)
        {
            var algorithmWithoutPrefix = algorithm.Replace("alg_", string.Empty);
            var predictionsPath = await _modelServices[MicroserviceType.LinearRegression]
                .PredictAsync(GetPredictionRequest(algorithmWithoutPrefix, request.Filename), token);
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

    // private static dynamic GetTrainingRequest(string algorithm, string filepath) =>
    //     algorithm switch
    //     {
    //         Algorithm.LinearRegression => new LinearRegression.Grpc.TrainingRequest {Filepath = filepath},
    //         Algorithm.Ridge => new Ridge.Grpc.TrainingRequest {Filepath = filepath},
    //         Algorithm.Lasso => new Lasso.Grpc.TrainingRequest {Filepath = filepath},
    //         Algorithm.ElasticNet => new ElasticNet.Grpc.TrainingRequest {Filepath = filepath},
    //         Algorithm.SGDRegressor => new SGDRegressor.Grpc.TrainingRequest {Filepath = filepath},
    //         
    //         Algorithm.LogisticRegression => new LogisticRegression.Grpc.TrainingRequest {Filepath = filepath},
    //         _ => new Ridge.Grpc.TrainingRequest {Filepath = filepath}
    //     };
    
    private static dynamic GetTrainingRequest(string algorithm, string filepath) =>
        algorithm switch
        {
            LinearRegressionAlgorithms.LinearRegression => new Linear.Regression.Grpc.TrainingRequest {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.LinearRegression},
            LinearRegressionAlgorithms.Ridge => new Linear.Regression.Grpc.TrainingRequest {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.Ridge},
            LinearRegressionAlgorithms.Lasso => new Linear.Regression.Grpc.TrainingRequest {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.Lasso},
            LinearRegressionAlgorithms.Lars => new Linear.Regression.Grpc.TrainingRequest {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.Lars},
            LinearRegressionAlgorithms.ElasticNet => new Linear.Regression.Grpc.TrainingRequest {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.ElasticNet},
            LinearRegressionAlgorithms.SGDRegressor => new Linear.Regression.Grpc.TrainingRequest {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.SGDRegressor},
            LinearRegressionAlgorithms.HuberRegressor => new Linear.Regression.Grpc.TrainingRequest {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.HuberRegressor},
            LinearRegressionAlgorithms.RANSACRegressor => new Linear.Regression.Grpc.TrainingRequest {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.RANSACRegressor},
            
            // Algorithm.LogisticRegression => new LogisticRegression.Grpc.TrainingRequest {Filepath = filepath},
            // _ => new Ridge.Grpc.TrainingRequest {Filepath = filepath}
        };
    // private static dynamic GetPredictionRequest(string algorithm, string filepath) =>
    //     algorithm switch
    //     {
    //         Algorithm.LinearRegression => new LinearRegression.Grpc.PredictionRequest {Filepath = filepath},
    //         Algorithm.Ridge => new Ridge.Grpc.PredictionRequest {Filepath = filepath},
    //         Algorithm.Lasso => new Lasso.Grpc.PredictionRequest {Filepath = filepath},
    //         Algorithm.ElasticNet => new ElasticNet.Grpc.PredictionRequest {Filepath = filepath},
    //         Algorithm.SGDRegressor => new SGDRegressor.Grpc.PredictionRequest {Filepath = filepath},
    //         
    //         Algorithm.LogisticRegression => new LogisticRegression.Grpc.PredictionRequest {Filepath = filepath},
    //         _ => new Ridge.Grpc.PredictionRequest {Filepath = filepath}
    //     };
    
    private static dynamic GetPredictionRequest(string algorithm, string filepath) =>
        algorithm switch
        {
            LinearRegressionAlgorithms.LinearRegression => new Linear.Regression.Grpc.PredictionRequest {Filepath = filepath, Algorithm =  LinearRegressionAlgorithms.LinearRegression},
            LinearRegressionAlgorithms.Ridge => new Linear.Regression.Grpc.PredictionRequest {Filepath = filepath, Algorithm =  LinearRegressionAlgorithms.Ridge},
            LinearRegressionAlgorithms.Lasso => new Linear.Regression.Grpc.PredictionRequest {Filepath = filepath, Algorithm =  LinearRegressionAlgorithms.Lasso},
            LinearRegressionAlgorithms.ElasticNet => new Linear.Regression.Grpc.PredictionRequest {Filepath = filepath, Algorithm =  LinearRegressionAlgorithms.ElasticNet},
            LinearRegressionAlgorithms.SGDRegressor => new Linear.Regression.Grpc.PredictionRequest {Filepath = filepath, Algorithm =  LinearRegressionAlgorithms.SGDRegressor},
            
            // Algorithm.LogisticRegression => new LogisticRegression.Grpc.PredictionRequest {Filepath = filepath},
            // _ => new Ridge.Grpc.PredictionRequest {Filepath = filepath}
        };

    #region Constants

    private class Mode
    {
        public const string Train = "train";
        public const string Predict = "predict";
    }

    private class LinearRegressionAlgorithms
    {
        public const string LinearRegression = "LinearRegression";
        public const string Ridge = "Ridge";
        public const string SGDRegressor = "SGDRegressor";
        public const string ElasticNet = "ElasticNet";
        public const string Lars = "Lars";
        public const string Lasso = "Lasso";
        public const string LassoLars = "LassoLars";
        public const string OrthogonalMatchingPursuit = "OrthogonalMatchingPursuit";
        public const string ARDRegression = "ARDRegression";
        public const string BayesianRidge = "BayesianRidge";
        public const string HuberRegressor = "HuberRegressor";
        public const string QuantileRegressor = "QuantileRegressor";
        public const string RANSACRegressor = "RANSACRegressor";
    }

    private class MicroserviceType
    {
        public const string LinearRegression = "LinearRegression";
    }

    #endregion
}