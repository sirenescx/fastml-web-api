using Fast.ML.Contracts;
using Fast.ML.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Fast.ML.Controllers;

[Route("configurations")]
public class AlgorithmController : ControllerBase
{
    private readonly IAlgorithmService _service;

    public AlgorithmController(IAlgorithmService service) => _service = service;
    
    /// <summary>
    /// Получение списка алгоритмов машинного обучения.
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("/algorithms")]
    public async Task<AlgorithmsResponse> GetAlgorithms(CancellationToken token) =>
        await _service.GetAlgorithmsAsync(token);
    
    // [HttpGet]
    /// <summary>
    /// Получение конфигураций всех микросервисов для машинного обучения.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("/endpoints")]
    public async Task<UserIdResponse> GetUserId([FromQuery] UserIdRequest request, CancellationToken token) =>
        throw new NotImplementedException();
    
    // [HttpGet]
    /// <summary>
    /// Получение конфигурации микросервиса по алгоритму машинного обучения.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("/endpoint/{algorithm_id}")]
    public async Task<UserIdResponse> GetUserId1([FromQuery] UserIdRequest request, CancellationToken token) =>
        throw new NotImplementedException();
}