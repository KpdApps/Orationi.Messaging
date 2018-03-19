CREATE TABLE [dbo].[ExternalSystems] (
    [Id]         UNIQUEIDENTIFIER CONSTRAINT [DF_ExternalSystems_Id] DEFAULT (newid()) NOT NULL,
    [SystemName] NVARCHAR (50)    NOT NULL,
    [Token]      NVARCHAR (50)    NOT NULL,
    CONSTRAINT [PK_ExternalSystems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [AK_ExternalSystems_SystemName] UNIQUE NONCLUSTERED ([SystemName] ASC)
);


