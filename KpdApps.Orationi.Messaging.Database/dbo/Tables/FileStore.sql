﻿CREATE TABLE [dbo].[FileStore]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT (newid()), 
    [MessageId] UNIQUEIDENTIFIER NOT NULL, 
    [FileName] NVARCHAR(250) NOT NULL, 
    [Data] VARBINARY(MAX) NOT NULL, 
    [CreatedOn] DATETIME NOT NULL, 
    CONSTRAINT [FK_FileStore_Messages] FOREIGN KEY ([MessageId]) REFERENCES [Messages]([Id])
)
