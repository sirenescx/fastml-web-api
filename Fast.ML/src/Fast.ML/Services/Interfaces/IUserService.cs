using Fast.ML.Contracts;

namespace Fast.ML.Services.Interfaces;

public interface IUserService
{
    public Task<UserIdResponse> GetUserIdAsync(UserIdRequest request, CancellationToken token);

    public Task<UserTaskResponse> PostUserTaskAsync(UserTaskRequest request, CancellationToken token);

    public Task<UserTasksResponse> GetUserTasksAsync(UserTasksRequest request, CancellationToken token);
    // public Task<UserTasksResponse> GetUserTasksAsync(int userId, CancellationToken token);
}
