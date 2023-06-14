USE [TodoDb]
GO

/****** Object: SqlProcedure [dbo].[spTodos_GetOneAssigned] Script Date: 6/18/2022 4:16:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[spTodos_GetOneAssigned]
	@AssignedTo int,
	@TodoId int
as
begin
	select Id, Task, AssignedTo, IsComplete
	from dbo.Todos
	where AssignedTo = @AssignedTo 
		and Id = @TodoId;
end
