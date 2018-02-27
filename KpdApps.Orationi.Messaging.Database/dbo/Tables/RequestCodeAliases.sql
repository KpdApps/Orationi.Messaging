CREATE TABLE [dbo].[RequestCodeAliases] (
    [Id]          UNIQUEIDENTIFIER CONSTRAINT [DF_RequestCodeAliases_Id] DEFAULT (newid()) NOT NULL,
    [RequestCode] INT              NOT NULL,
    [Alias]       NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_RequestCodeAliases] PRIMARY KEY CLUSTERED ([Id] ASC)
);

