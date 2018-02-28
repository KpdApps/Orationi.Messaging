CREATE TABLE [dbo].[PluginAssemblies] (
    [Id]         UNIQUEIDENTIFIER NOT NULL,
    [Name]       NVARCHAR (128)   NOT NULL,
    [Assembly]   VARBINARY (MAX)  NOT NULL,
    [Modified] DATETIME2 (7)    CONSTRAINT [DF_PluginAssemblies_ModifiedOn] DEFAULT (getdate()) NOT NULL,
    CONSTRAINT [PK_PipelineAssemblies] PRIMARY KEY CLUSTERED ([Id] ASC)
);






GO

CREATE TRIGGER dbo.PluginAssembliesModified ON dbo.PluginAssemblies
AFTER INSERT, UPDATE 
AS
  UPDATE pa set [Modified] = GETDATE()
  FROM 
  dbo.PluginAssemblies AS pa
  INNER JOIN inserted
  AS i
  ON pa.Id = i.Id;