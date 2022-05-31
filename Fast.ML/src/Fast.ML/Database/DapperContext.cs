using System.Data;
using Microsoft.Data.SqlClient;

namespace Fast.ML.Database;

public class DapperContext
{
    private readonly string _connectionString;

    private const string ConnectionStringJsonFieldName = "FastMLSqlConnection";
    
    public DapperContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString(ConnectionStringJsonFieldName);
    }
    
    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}