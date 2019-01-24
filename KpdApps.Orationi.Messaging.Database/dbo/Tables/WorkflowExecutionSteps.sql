CREATE TABLE [dbo].[WorkflowExecutionSteps] (
    [Id]                 UNIQUEIDENTIFIER CONSTRAINT [DF_WorkflowExecutionSteps_Id] DEFAULT (newid()) NOT NULL,
    [WorkflowId]         UNIQUEIDENTIFIER NOT NULL,
    [PluginActionSetItemId]  UNIQUEIDENTIFIER NULL,
    [StatusCode]         INT              CONSTRAINT [DF_WorkflowExecutionSteps_StatusCode] DEFAULT ((0)) NOT NULL,
    [RequestBody]        NVARCHAR (MAX)   NULL,
    [ResponseBody]       NVARCHAR (MAX)   NULL,
    [PipelineValues] NVARCHAR (MAX)   NULL,
    [MessageId] UNIQUEIDENTIFIER NULL, 
    [Created] DATETIME NOT NULL DEFAULT (getdate()), 
    CONSTRAINT [PK_WorkflowExecutionSteps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkflowExecutionSteps_PluginActionSetItems] FOREIGN KEY ([PluginActionSetItemId]) REFERENCES [dbo].[PluginActionSetItems] ([Id]),
    CONSTRAINT [FK_WorkflowExecutionSteps_WorkflowExecutionStepsStatusCodes] FOREIGN KEY ([StatusCode]) REFERENCES [dbo].[WorkflowExecutionStepsStatusCodes] ([Id]),
    CONSTRAINT [FK_WorkflowExecutionSteps_Workflows] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflows] ([Id]), 
    CONSTRAINT [FK_WorkflowExecutionSteps_Messages] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Messages]([Id])
);

