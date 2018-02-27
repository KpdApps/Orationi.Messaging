CREATE TABLE [dbo].[GlobalSettings] (
    [Name]  NVARCHAR (50)  NOT NULL,
    [Value] NVARCHAR (250) NOT NULL,
    CONSTRAINT [PK_GlobalSettings] PRIMARY KEY CLUSTERED ([Name] ASC)
);

