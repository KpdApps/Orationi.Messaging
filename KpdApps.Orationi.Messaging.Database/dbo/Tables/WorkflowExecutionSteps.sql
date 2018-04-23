CREATE TABLE [dbo].[WorkflowExecutionSteps] (
    [Id]                 UNIQUEIDENTIFIER CONSTRAINT [DF_WorkflowExecutionSteps_Id] DEFAULT (newid()) NOT NULL,
    [WorkflowId]         UNIQUEIDENTIFIER NOT NULL,
    [PluginActionSetId]  UNIQUEIDENTIFIER NOT NULL,
    [StatusCode]         INT              CONSTRAINT [DF_WorkflowExecutionSteps_StatusCode] DEFAULT ((0)) NOT NULL,
    [RequestBody]        NVARCHAR (MAX)   NULL,
    [ResponseBody]       NVARCHAR (MAX)   NULL,
    [ExecutionVariables] NVARCHAR (MAX)   NULL,
    [MessageId] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [PK_WorkflowExecutionSteps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WorkflowExecutionSteps_PluginActionSets] FOREIGN KEY ([PluginActionSetId]) REFERENCES [dbo].[PluginActionSets] ([Id]),
    CONSTRAINT [FK_WorkflowExecutionSteps_WorkflowExecutionStepsStatusCodes] FOREIGN KEY ([StatusCode]) REFERENCES [dbo].[WorkflowExecutionStepsStatusCodes] ([Id]),
    CONSTRAINT [FK_WorkflowExecutionSteps_Workflows] FOREIGN KEY ([WorkflowId]) REFERENCES [dbo].[Workflows] ([Id]), 
    CONSTRAINT [FK_WorkflowExecutionSteps_Messages] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Messages]([Id])
);

