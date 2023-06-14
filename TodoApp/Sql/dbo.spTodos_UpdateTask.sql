USE [TodoDb]
GO

/****** Object: SqlProcedure [dbo].[spTodos_UpdateTask] Script Date: 6/18/2022 4:16:13 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[spTodos_UpdateTask]
	@Task nvarchar(50),
	@AssignedTo int,
	@TodoId int
as
begin
	update dbo.Todos
	set Task = @Task
	where Id = @TodoId 
		and AssignedTo = @AssignedTo;
end
