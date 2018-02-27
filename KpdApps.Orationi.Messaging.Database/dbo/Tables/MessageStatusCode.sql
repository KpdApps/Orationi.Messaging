CREATE TABLE [dbo].[MessageStatusCode] (
    [Id]          INT            NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (300) NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

