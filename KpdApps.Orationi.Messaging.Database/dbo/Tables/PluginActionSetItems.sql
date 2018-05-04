CREATE TABLE [dbo].[PluginActionSetItems] (
    [Id]                 UNIQUEIDENTIFIER CONSTRAINT [DF_RegisteredSteps_Id] DEFAULT (newid()) NOT NULL,
    [PluginActionSetId]  UNIQUEIDENTIFIER NOT NULL,
    [RegisteredPluginId] UNIQUEIDENTIFIER NOT NULL,
    [Order]              INT              NOT NULL,
    [Configuration]      NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_RegisteredSteps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PluginActionSetItems_PluginActionSets] FOREIGN KEY ([PluginActionSetId]) REFERENCES [dbo].[PluginActionSets] ([Id]),
    CONSTRAINT [FK_PluginActionSetItems_RegisteredPlugins] FOREIGN KEY ([RegisteredPluginId]) REFERENCES [dbo].[RegisteredPlugins] ([Id])
);





