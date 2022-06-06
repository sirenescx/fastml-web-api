using Fast.ML.Contracts;
using Fast.ML.Services.Interfaces;
using Fast.ML.Worker.Grpc;
using Grpc.Core;
using TrainingRequest = Fast.ML.Worker.Grpc.TrainingRequest;
using TrainingResponse = Fast.ML.Worker.Grpc.TrainingResponse;

namespace Fast.ML.Controllers;

public class ModelController : WorkerService.WorkerServiceBase
{
    private readonly ILogger<ModelController> _logger;
    private readonly IUserService _userService;
    private readonly IMachineLearningService _machineLearningService;

    public ModelController(
        ILogger<ModelController> logger,
        IUserService userService,
        IMachineLearningService machineLearningService
    )
    {
        _logger = logger;
        _userService = userService;
        _machineLearningService = machineLearningService;
    }

    public override async Task<TaskResponse> Run(TaskRequest request, ServerCallContext context)
    {
        var userTaskRequest = new UserTaskRequest
        {
            UserId = request.UserId
        };
    
        var response = await _userService.PostUserTaskAsync(userTaskRequest, context.CancellationToken);
    
        return new TaskResponse
        {
            TaskId = response.TaskId
        };
    }
    
    /// <summary>
    /// Запуск обучения моделей.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public override async Task<TrainingResponse> Train(
        TrainingRequest request,
        ServerCallContext context)
    {
        return new TrainingResponse
        {
            Message = await _machineLearningService.Train(request, context.CancellationToken)
        };
    }

    public override async Task<PredictionResponse> Predict(
        PredictionRequest request,
        ServerCallContext context)
    {
        return new PredictionResponse
        {
            Message = await _machineLearningService.Predict(request, context.CancellationToken)
        };
    }
}