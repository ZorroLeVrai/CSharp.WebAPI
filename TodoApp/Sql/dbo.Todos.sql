USE [TodoDb]
GO

/****** Object: Table [dbo].[Todos] Script Date: 6/18/2022 4:15:03 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Todos] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Task]       NVARCHAR (50) NOT NULL,
    [AssignedTo] INT           NOT NULL,
    [IsComplete] BIT           NOT NULL
);


