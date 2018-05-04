CREATE TABLE [dbo].[PluginActionSets] (
    [Id]   UNIQUEIDENTIFIER CONSTRAINT [DF_PluginSets_Id] DEFAULT (newid()) NOT NULL,
    [Name] NVARCHAR (128)   NOT NULL,
    CONSTRAINT [PK_PluginSets] PRIMARY KEY CLUSTERED ([Id] ASC)
);

