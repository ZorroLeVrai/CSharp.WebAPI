namespace TodoLibrary.DataAccess
{
    public interface ISqlDataAccess
    {
        Task<List<T>> LoadData<T, U>(string storedProcedureName, U parameters, string connectionStringName);
        Task SaveData<T>(string storedProcedureName, T parameters, string connectionStringName);
    }
}