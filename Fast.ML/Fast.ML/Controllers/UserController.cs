using Fast.ML.Contracts;
using Fast.ML.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fast.ML.Controllers;

[Route("/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _service;

    public UserController(IUserService service)
    {
        _service = service;
    }

    [HttpGet]
    /// <summary>
    /// Обновление информации о пользователе.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    // [HttpPost]
    public async Task<UserIdResponse> GetUserId([FromQuery] UserIdRequest request, CancellationToken token) =>
        await _service.GetUserIdAsync(request, token);

    /// <summary>
    /// Получение списка задач пользователя.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    // [HttpGet]
    // [Route("/tasks/{user_id}")]
    // public async Task<UserTasksResponse> GetUserTasks([FromQuery(Name = "user_id")] int userId, CancellationToken token) =>
    //     throw new NotImplementedException();
    [HttpGet]
    [Route("/tasks")]
    public async Task<UserTasksResponse> GetUserTasks([FromQuery] UserTasksRequest request, CancellationToken token) =>
        await _service.GetUserTasksAsync(request, token);

    /// <summary>
    /// Создание задачи.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("/task")]
    public async Task<UserTasksResponse> Wtf([FromQuery] UserTasksRequest request, CancellationToken token) =>
        throw new NotImplementedException();

    /// <summary>
    /// Получение списка доступных пользователю алгоритмов.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [Route("/algorithms/{user_id}")]
    public async Task<UserTasksResponse> Wtf2([FromQuery] UserTasksRequest request, CancellationToken token) =>
        throw new NotImplementedException();
    
    // /// <summary>
    // /// Получение задачи.
    // /// </summary>
    // /// <param name="request"></param>
    // /// <param name="token"></param>
    // /// <returns></returns>
    // [HttpGet]
    // [Route("/task/{task_id}")]
    // public async Task<UserTasksResponse> Wtf3([FromQuery(Name = "task_id")] int userId, CancellationToken token) =>
    //     throw new NotImplementedException();
}