using Dapper;
using Fast.ML.Database;
using Fast.ML.Registries.Interfaces;

namespace Fast.ML.Registries;

public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;

    public UserRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<int?> GetUserIdAsync(string email)
    {
        var parameters = new
        {
            Email = email
        };
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<int?>(GetUserIdQuery, parameters);
    }

    public async Task<int> CreateUserAsync(string email, string firstName, string lastName)
    {
        var parameters = new
        {
            Email = email, 
            FirstName = firstName, 
            LastName = lastName
        };
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(CreateUserQuery, parameters);
        return await connection.QuerySingleOrDefaultAsync<int>(GetUserIdQuery, parameters);
    }

    public async Task<string> CreateUserTaskAsync(int userId)
    {
        var taskId = GetTaskId(userId);
        var parameters = new
        {
            UserId = userId, 
            TaskId = taskId
        };
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(CreateUserTaskQuery, parameters);
        return taskId;
    }

    public async Task<IEnumerable<string>> GetUserTasksAsync(int userId)
    {
        var parameters = new {UserId = userId};
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<string>(GetUserTasksQuery, parameters);
    }

    #region SqlQueries

    private const string GetUserIdQuery = 
        @"select id from fastml.dbo.users 
          where email = @Email";

    private const string CreateUserQuery =
        @"insert into fastml.dbo.users (email, first_name, last_name) 
          values (@Email, @FirstName, @LastName)";

    private const string CreateUserTaskQuery =
        "insert into fastml.dbo.user_tasks (user_id, task_id) values (@UserId, @TaskId)";

    private const string GetUserTasksQuery =
        "select task_id from fastml.dbo.user_tasks where user_id = @UserId";

    #endregion

    #region Constants

    private const string DateTimeFormat = "yyyy'-'MM'-'dd'T'HH'-'mm'-'ss";

    #endregion
    
    #region Utilites

    private static string GetTaskId(int userId) =>
        string.Join('-', $"{userId:d10}", DateTime.Now.ToString(DateTimeFormat));

    #endregion
}