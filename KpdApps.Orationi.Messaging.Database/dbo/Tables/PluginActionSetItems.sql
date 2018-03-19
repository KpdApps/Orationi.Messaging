CREATE TABLE [dbo].[PluginActionSetItems] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_RegisteredSteps_Id] DEFAULT (newid()) NOT NULL,
    [PluginActionSetId] UNIQUEIDENTIFIER NULL,
    [RequestCode]       INT              NOT NULL,
    [PluginTypeId]      UNIQUEIDENTIFIER NOT NULL,
    [Order]             INT              NOT NULL,
    [IsAsynchronous]    BIT              CONSTRAINT [DF_RegisteredSteps_IsAsynchronous] DEFAULT ((0)) NOT NULL,
    [Configuration]     NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_RegisteredSteps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PluginActionSetItems_PluginActionSets] FOREIGN KEY ([PluginActionSetId]) REFERENCES [dbo].[PluginActionSets] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_PluginRegisteredSteps_PluginTypes] FOREIGN KEY ([PluginTypeId]) REFERENCES [dbo].[RegisteredPlugins] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_RegisteredSteps_RequestCodes] FOREIGN KEY ([RequestCode]) REFERENCES [dbo].[RequestCodes] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

