CREATE TABLE [dbo].[PipelineAssemblies] (
    [Id]       UNIQUEIDENTIFIER NOT NULL,
    [Assembly] VARBINARY (MAX)  NOT NULL,
    CONSTRAINT [PK_PipelineAssemblies] PRIMARY KEY CLUSTERED ([Id] ASC)
);

