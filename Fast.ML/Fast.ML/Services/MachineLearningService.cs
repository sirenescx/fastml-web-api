using Fast.ML.Extensions;
using Fast.ML.Hubs;
using Fast.ML.Preprocessor.Grpc;
using Fast.ML.Services.Interfaces;
using Fast.ML.Worker.Grpc;
using Microsoft.Extensions.Options;
using ServiceOptions = Fast.ML.Options.ServiceOptions;
using LinRegPredictionRequest =
    Fast.ML.Linear.Regression.Grpc.PredictionRequest;
using LinRegTrainingRequest =
    Fast.ML.Linear.Regression.Grpc.TrainingRequest;

// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace Fast.ML.Services;

public class MachineLearningService : IMachineLearningService
{
    private readonly ILogger<MachineLearningService> _logger;
    private readonly IOptionsMonitor<ServiceOptions> _options;
    private readonly IPreprocessingService _preprocessingService;
    private readonly Dictionary<string, IModelService> _modelServices = new();
    private readonly PageUpdateHub _hub;

    public MachineLearningService(
        ILogger<MachineLearningService> logger,
        IOptionsMonitor<ServiceOptions> options,
        IPreprocessingService preprocessingService,
        ILinearRegressionService linearRegressionService,
        ILinearClassificationService linearClassificationService,
        PageUpdateHub pageUpdateHub)
    {
        _logger = logger;
        _preprocessingService = preprocessingService;
        _options = options;
        _modelServices[MicroserviceType.LinearRegression] = linearRegressionService;
        _modelServices[MicroserviceType.LinearClassification] = linearClassificationService;
        _hub = pageUpdateHub;
    }

    public async Task<string> Train(TrainingRequest request, CancellationToken token)
    {
        try
        {
            request.Filename = MoveFile(request.Filename, request.TaskId);
        }
        catch (FileNotFoundException e)
        {
            _logger.LogError(e.Message);
        }

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
            request.Filename = await _preprocessingService
                .PreprocessDatasetAsync(preprocessingRequest, token);
            _logger.LogInformation("Training dataset was successfully preprocessed");
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }

        foreach (var algorithm in request.Algorithms)
        {
            using var timeoutCancellationToken = token
                .LinkWithTimeout(_options.CurrentValue.TrainingTimeoutMs);
            Console.WriteLine(_options.CurrentValue.TrainingTimeoutMs);
            var algorithmWithoutPrefix = algorithm.Replace("alg_", string.Empty);
            try
            {
                _logger.LogInformation($"Started training {algorithmWithoutPrefix}");
                await _modelServices[MicroserviceType.LinearRegression]
                    .TrainAsync(
                        GetTrainingRequest(algorithmWithoutPrefix, request.Filename),
                        timeoutCancellationToken.Token);
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                _logger.LogInformation(e.StackTrace);
                WriteErrorToLog(request.Filename, algorithmWithoutPrefix);
            }
        }

        await _hub.SendMessage(request.TaskId, "train");

        return "OK";
    }

    public async Task<string> Predict(PredictionRequest request, CancellationToken token)
    {
        try
        {
            request.Filename = MoveFile(request.Filename, request.TaskId);
        }
        catch (Exception e)
        {
            _logger.LogError(e.Message);
        }

        var preprocessingRequest = new PreprocessingRequest
        {
            Filename = request.Filename,
            Separator = request.Separator,
            Index = request.Index,
            Mode = Mode.Predict
        };

        try
        {
            request.Filename = await _preprocessingService
                .PreprocessDatasetAsync(preprocessingRequest, token);
            _logger.LogInformation("Training dataset was successfully preprocessed");
        }
        catch (Exception e)
        {
            _logger.LogError($"Dataset preprocessing failed {e.StackTrace}");
            await _hub.SendMessage(request.Algorithms.First(), "model", true);
            return "NOT OK";
        }

        foreach (var algorithm in request.Algorithms)
        {
            using var timeoutCancellationToken = token
                .LinkWithTimeout(_options.CurrentValue.TrainingTimeoutMs);
            try
            {
                var algorithmWithoutPrefix = algorithm.Replace("alg_", string.Empty);
                var predictionsPath =
                    await _modelServices[MicroserviceType.LinearRegression]
                        .PredictAsync(GetPredictionRequest(algorithmWithoutPrefix, request.Filename),
                            timeoutCancellationToken.Token);
                await _hub.SendMessage(predictionsPath, "model");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                _logger.LogError($"Prediction with {algorithm} failed");
            }
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

    private static void WriteErrorToLog(string filePath, string algorithm)
    {
        var path = Path.GetDirectoryName(filePath);
        var logFilePath = Path.Combine(path!, "log");
        File.AppendAllLines(logFilePath,
            new[]
            {
                @$"INFO {DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss,fff")} - 
[{algorithm}] Training was not finished because timeout exceeded or some other error"
            });
    }

    private static LinRegTrainingRequest GetTrainingRequest(string algorithm, string filepath)
    {
        return new LinRegTrainingRequest
        {
            Filepath = filepath,
            Algorithm = algorithm
        };
    }

    // private static dynamic GetTrainingRequest(string algorithm, string filepath) =>
    //     algorithm switch
    //     {
    //         LinearRegressionAlgorithms.LinearRegression => new LinRegTrainingRequest
    //             {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.LinearRegression},
    //         LinearRegressionAlgorithms.Ridge => new LinRegTrainingRequest
    //             {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.Ridge},
    //         LinearRegressionAlgorithms.Lasso => new LinRegTrainingRequest
    //             {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.Lasso},
    //         LinearRegressionAlgorithms.Lars => new LinRegTrainingRequest
    //             {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.Lars},
    //         LinearRegressionAlgorithms.ElasticNet => new LinRegTrainingRequest
    //             {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.ElasticNet},
    //         LinearRegressionAlgorithms.SGDRegressor => new LinRegTrainingRequest
    //             {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.SGDRegressor},
    //         LinearRegressionAlgorithms.HuberRegressor => new LinRegTrainingRequest
    //             {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.HuberRegressor},
    //         LinearRegressionAlgorithms.RANSACRegressor => new LinRegTrainingRequest
    //             {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.RANSACRegressor},
    //         LinearRegressionAlgorithms.LassoLars => new LinRegTrainingRequest
    //             {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.LassoLars},
    //         LinearRegressionAlgorithms.OrthogonalMatchingPursuit => new LinRegTrainingRequest
    //             {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.OrthogonalMatchingPursuit},
    //         LinearRegressionAlgorithms.TheilSenRegressor => new LinRegTrainingRequest
    //             {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.TheilSenRegressor},
    //         LinearRegressionAlgorithms.QuantileRegressor => new LinRegTrainingRequest
    //             {Filepath = filepath, Algorithm = LinearRegressionAlgorithms.QuantileRegressor},
    //     };

    private static dynamic GetPredictionRequest(string algorithm, string filepath) =>
        algorithm switch
        {
            LinearRegressionAlgorithms.LinearRegression =>
                new LinRegPredictionRequest
                {
                    Filepath = filepath,
                    Algorithm = LinearRegressionAlgorithms.LinearRegression
                },
            LinearRegressionAlgorithms.Ridge =>
                new LinRegPredictionRequest
                {
                    Filepath = filepath,
                    Algorithm = LinearRegressionAlgorithms.Ridge
                },
            LinearRegressionAlgorithms.Lasso =>
                new LinRegPredictionRequest
                {
                    Filepath = filepath,
                    Algorithm = LinearRegressionAlgorithms.Lasso
                },
            LinearRegressionAlgorithms.Lars =>
                new LinRegPredictionRequest
                {
                    Filepath = filepath,
                    Algorithm = LinearRegressionAlgorithms.Lars
                },
            LinearRegressionAlgorithms.ElasticNet =>
                new LinRegPredictionRequest
                {
                    Filepath = filepath,
                    Algorithm = LinearRegressionAlgorithms.ElasticNet
                },
            LinearRegressionAlgorithms.SGDRegressor =>
                new LinRegPredictionRequest
                {
                    Filepath = filepath,
                    Algorithm = LinearRegressionAlgorithms.SGDRegressor
                },
            LinearRegressionAlgorithms.HuberRegressor =>
                new LinRegPredictionRequest
                {
                    Filepath = filepath,
                    Algorithm = LinearRegressionAlgorithms.HuberRegressor
                },
            LinearRegressionAlgorithms.RANSACRegressor => 
                new LinRegPredictionRequest
                {
                    Filepath = filepath,
                    Algorithm = LinearRegressionAlgorithms.RANSACRegressor
                },
            LinearRegressionAlgorithms.LassoLars => 
                new LinRegPredictionRequest
                {
                    Filepath = filepath,
                    Algorithm = LinearRegressionAlgorithms.LassoLars
                },
            LinearRegressionAlgorithms.OrthogonalMatchingPursuit => 
                new LinRegPredictionRequest
                {
                    Filepath = filepath,
                    Algorithm = LinearRegressionAlgorithms.OrthogonalMatchingPursuit
                },
            LinearRegressionAlgorithms.TheilSenRegressor => 
                new LinRegPredictionRequest
                {
                    Filepath = filepath,
                    Algorithm = LinearRegressionAlgorithms.TheilSenRegressor
                },
            LinearRegressionAlgorithms.QuantileRegressor => 
                new LinRegPredictionRequest
            {
                Filepath = filepath,
                Algorithm = LinearRegressionAlgorithms.QuantileRegressor
            },
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
        public const string BayesianRidge = "BayesianRidge";
        public const string HuberRegressor = "HuberRegressor";
        public const string QuantileRegressor = "QuantileRegressor";
        public const string TheilSenRegressor = "TheilSenRegressor";
        public const string RANSACRegressor = "RANSACRegressor";
    }

    private class MicroserviceType
    {
        public const string LinearRegression = "LinearRegression";

        public const string LinearClassification = "LinearClassification";
    }

    #endregion
}