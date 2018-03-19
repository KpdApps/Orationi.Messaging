CREATE TABLE [dbo].[ExternalSystems]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY default(newid()), 
	[SystemName] NVARCHAR(50) NOT NULL, 
	[Token] NVARCHAR(50) NOT NULL, 
	CONSTRAINT [AK_ExternalSystems_SystemName] UNIQUE ([SystemName]) 
)
