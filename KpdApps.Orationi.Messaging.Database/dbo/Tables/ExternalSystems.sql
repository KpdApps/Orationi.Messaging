CREATE TABLE [dbo].[ExternalSystems]
(
	[Id] INT NOT NULL PRIMARY KEY Identity(1, 1), 
	[SystemName] NVARCHAR(50) NOT NULL, 
	[Token] NVARCHAR(50) NOT NULL, 
	CONSTRAINT [AK_ExternalSystems_SystemName] UNIQUE ([SystemName]) 
)
