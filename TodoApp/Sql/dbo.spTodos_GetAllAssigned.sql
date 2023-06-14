USE [TodoDb]
GO

/****** Object: SqlProcedure [dbo].[spTodos_GetAllAssigned] Script Date: 6/18/2022 4:16:00 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[spTodos_GetAllAssigned]
	@AssignedTo int
as
begin
	select Id, Task, AssignedTo, IsComplete
	from dbo.Todos
	where AssignedTo = @AssignedTo;
end
