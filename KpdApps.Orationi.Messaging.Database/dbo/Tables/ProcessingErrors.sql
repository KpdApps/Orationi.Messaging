CREATE TABLE [dbo].[ProcessingErrors] (
    [Id]         UNIQUEIDENTIFIER CONSTRAINT [DF_ProcessingErrors_Id] DEFAULT (newid()) NOT NULL,
    [MessageId]  UNIQUEIDENTIFIER NOT NULL,
    [Created]    DATETIME2 (7)    CONSTRAINT [DF_ProcessingErrors_CreatedOn] DEFAULT (getdate()) NOT NULL,
    [Error]      NVARCHAR (MAX)   NOT NULL,
    [StackTrace] NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_ProcessingErrors] PRIMARY KEY CLUSTERED ([Id] ASC)
);

