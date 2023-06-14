CREATE PROCEDURE [dbo].[spTodos_GetAllAssigned]
	@AssignedTo int
AS
BEGIN
	select Id,  Task, AssignedTo, IsComplete
	from dbo.Todos
	where AssignedTo = @AssignedTo;
END
