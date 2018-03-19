CREATE TABLE [dbo].[Workflows] (
    [Id]            UNIQUEIDENTIFIER CONSTRAINT [DF_Workflows_Id] DEFAULT (newid()) NOT NULL,
    [Name]          NVARCHAR (128)   NOT NULL,
    [RequestCodeId] INT              NOT NULL,
    CONSTRAINT [PK_Workflows] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Workflows_RequestCodes] FOREIGN KEY ([RequestCodeId]) REFERENCES [dbo].[RequestCodes] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

