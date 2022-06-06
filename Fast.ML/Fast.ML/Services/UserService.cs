using Fast.ML.Contracts;
using Fast.ML.Registries.Interfaces;
using Fast.ML.Services.Interfaces;

namespace Fast.ML.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<UserIdResponse> GetUserIdAsync(
        UserIdRequest request, CancellationToken token)
    {
        var userId =
            await _repository.GetUserIdAsync(request.Email) ??
            await _repository.CreateUserAsync(
                request.Email, request.FirstName, request.LastName);

        return new UserIdResponse
        {
            Id = userId
        };
    }

    public async Task<UserTaskResponse> PostUserTaskAsync(
        UserTaskRequest request, CancellationToken token)
    {
        var taskId = await _repository.CreateUserTaskAsync(request.UserId);

        return new UserTaskResponse
        {
            TaskId = taskId
        };
    }

    public async Task<UserTasksResponse> GetUserTasksAsync(
        UserTasksRequest request, CancellationToken token)
    {
        var tasksIds = await _repository.GetUserTasksAsync(request.UserId);
    
        return new UserTasksResponse
        {
            TaskIds = tasksIds
        };
    }
    // public async Task<UserTasksResponse> GetUserTasksAsync(int userId, CancellationToken token)
    // {
    //     var tasksIds = await _registry.GetUserTasksAsync(userId);
    //
    //     return new UserTasksResponse
    //     {
    //         TaskIds = tasksIds
    //     };
    // }
}