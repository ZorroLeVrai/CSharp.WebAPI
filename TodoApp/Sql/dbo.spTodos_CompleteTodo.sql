USE [TodoDb]
GO

/****** Object: SqlProcedure [dbo].[spTodos_CompleteTodo] Script Date: 6/18/2022 4:15:41 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[spTodos_CompleteTodo]
	@AssignedTo int,
	@TodoId int
as
begin
	update dbo.Todos
	set IsComplete = 1
	where Id = @TodoId 
		and AssignedTo = @AssignedTo;
end
