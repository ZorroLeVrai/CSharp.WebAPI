using Microsoft.Extensions.Configuration;
using Dapper;
using System.Data;
using System.Data.SqlClient;

namespace TodoLibrary.DataAccess;

public class SqlDataAccess : ISqlDataAccess
{
    private readonly IConfiguration _config;

    public SqlDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public async Task<List<T>> LoadData<T, U>(
        string storedProcedureName,
        U parameters,
        string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);

        //opens a connection string to SqlServer
        using IDbConnection connection = new SqlConnection(connectionString);

        //Dapper maps the result of the stored procedure to type T
        var rows = await connection.QueryAsync<T>(storedProcedureName, parameters,
            commandType: CommandType.StoredProcedure);

        return rows.ToList();
    }

    public async Task SaveData<T>(string storedProcedureName,
        T parameters,
        string connectionStringName)
    {
        string connectionString = _config.GetConnectionString(connectionStringName);

        //opens a connection string to SqlServer
        using IDbConnection connection = new SqlConnection(connectionString);

        await connection.ExecuteAsync(
            storedProcedureName,
            parameters,
            commandType: CommandType.StoredProcedure);
    }
}
