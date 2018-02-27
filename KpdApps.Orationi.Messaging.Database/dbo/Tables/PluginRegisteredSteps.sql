CREATE TABLE [dbo].[PluginRegisteredSteps] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_RegisteredSteps_Id] DEFAULT (newid()) NOT NULL,
    [RequestCode]    INT              NOT NULL,
    [PluginTypeId]   UNIQUEIDENTIFIER NOT NULL,
    [Order]          INT              NOT NULL,
    [IsAsynchronous] BIT              CONSTRAINT [DF_RegisteredSteps_IsAsynchronous] DEFAULT ((0)) NOT NULL,
    [Configuration]  NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_RegisteredSteps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PluginRegisteredSteps_PluginTypes] FOREIGN KEY ([PluginTypeId]) REFERENCES [dbo].[PluginTypes] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_RegisteredSteps_RequestCodes] FOREIGN KEY ([RequestCode]) REFERENCES [dbo].[RequestCodes] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);



