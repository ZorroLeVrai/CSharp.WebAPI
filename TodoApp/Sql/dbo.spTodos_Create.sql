USE [TodoDb]
GO

/****** Object: SqlProcedure [dbo].[spTodos_Create] Script Date: 6/18/2022 4:15:49 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[spTodos_Create]
	@Task nvarchar(50),
	@AssignedTo int
as
begin
	insert into dbo.Todos (Task, AssignedTo)
	values (@Task, @AssignedTo);

	select Id, Task, AssignedTo, IsComplete
	from dbo.Todos
	where Id = scope_identity();
end
