USE [TodoDb]
GO

/****** Object: SqlProcedure [dbo].[spTodos_Delete] Script Date: 6/18/2022 4:15:54 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[spTodos_Delete]
	@AssignedTo int,
	@TodoId int
as
begin
	delete from dbo.Todos
	where Id = @TodoId 
		and AssignedTo = @AssignedTo;
end
