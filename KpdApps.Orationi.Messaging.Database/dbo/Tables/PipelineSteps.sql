CREATE TABLE [dbo].[PipelineSteps] (
    [Id]         UNIQUEIDENTIFIER CONSTRAINT [DF_PipelineSteps_Id] DEFAULT (newid()) NOT NULL,
    [AssemblyId] UNIQUEIDENTIFIER NOT NULL,
    [Class]      NVARCHAR (250)   NOT NULL,
    CONSTRAINT [PK_PipelineSteps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_PipelineSteps_PipelineAssemblies] FOREIGN KEY ([AssemblyId]) REFERENCES [dbo].[PipelineAssemblies] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

