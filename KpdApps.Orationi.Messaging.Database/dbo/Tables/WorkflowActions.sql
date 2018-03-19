CREATE TABLE [dbo].[WorkflowActions] (
    [Id]                UNIQUEIDENTIFIER CONSTRAINT [DF_WorkflowActions_Id] DEFAULT (newid()) NOT NULL,
    [WorkflowId]        UNIQUEIDENTIFIER NOT NULL,
    [PluginActionSetId] UNIQUEIDENTIFIER NOT NULL,
    [Description]       NVARCHAR (250)   NOT NULL,
    [Order]             INT              NOT NULL,
    CONSTRAINT [PK_WorkflowActions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkflowActions_PluginActionSets] FOREIGN KEY ([Id]) REFERENCES [dbo].[PluginActionSets] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_WorkflowActions_Workflows] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflows] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

