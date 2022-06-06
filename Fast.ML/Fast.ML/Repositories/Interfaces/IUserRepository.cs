namespace Fast.ML.Registries.Interfaces;

public interface IUserRepository
{
    public Task<int?> GetUserIdAsync(string email);

    public Task<int> CreateUserAsync(string email, string firstName, string lastName);

    public Task<string> CreateUserTaskAsync(int userId);

    public Task<IEnumerable<string>> GetUserTasksAsync(int userId);
}