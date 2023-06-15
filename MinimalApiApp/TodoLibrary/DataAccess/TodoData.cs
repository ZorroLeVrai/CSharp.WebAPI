using TodoLibrary.Models;

namespace TodoLibrary.DataAccess;

public class TodoData : ITodoData
{
    private ISqlDataAccess _sql;

    public TodoData(ISqlDataAccess sql)
    {
        _sql = sql;
    }

    public Task<List<TodoModel>> GetAllAssigned(int assignedTo)
    {
        //exec dbo.spTodos_GetAllAssigned @AssignedTo = assignedTo
        //Notice how AssignedTo matches the parameter name of the stored procedure
        return _sql.LoadData<TodoModel, dynamic>("dbo.spTodos_GetAllAssigned",
            new { AssignedTo = assignedTo },
            "Default");
    }

    public async Task<TodoModel?> GetOneAssigned(int assignedTo, int todoId)
    {
        //dynamic because is the only way to pass anonymous objects as a parameter
        var result = await _sql.LoadData<TodoModel, dynamic>("dbo.spTodos_GetOneAssigned",
            new { AssignedTo = assignedTo, TodoId = todoId },
            "Default");
        return result.FirstOrDefault();
    }

    public async Task<TodoModel?> Create(int assignedTo, string task)
    {
        //dynamic because is the only way to pass anonymous objects as a parameter
        var result = await _sql.LoadData<TodoModel, dynamic>("dbo.spTodos_Create",
            new { AssignedTo = assignedTo, Task = task },
            "Default");
        return result.FirstOrDefault();
    }

    public Task UpdateTask(int assignedTo, int todoId, string task)
    {
        return _sql.SaveData<dynamic>("dbo.spTodos_UpdateTask",
            new { AssignedTo = assignedTo, TodoId = todoId, Task = task },
            "Default");
    }

    public Task CompleteTodo(int assignedTo, int todoId)
    {
        return _sql.SaveData<dynamic>("dbo.spTodos_CompleteTodo",
            new { AssignedTo = assignedTo, TodoId = todoId },
            "Default");
    }

    public Task Delete(int assignedTo, int todoId)
    {
        return _sql.SaveData<dynamic>("dbo.spTodos_Delete",
            new { AssignedTo = assignedTo, TodoId = todoId },
            "Default");
    }
}
