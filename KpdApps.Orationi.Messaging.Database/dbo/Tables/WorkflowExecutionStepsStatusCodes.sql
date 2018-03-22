CREATE TABLE [dbo].[WorkflowExecutionStepsStatusCodes] (
    [Id]          INT            NOT NULL,
    [Name]        NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (500) NULL,
    CONSTRAINT [PK_WorkflowExecutionStepsStatusCodes] PRIMARY KEY CLUSTERED ([Id] ASC)
);

