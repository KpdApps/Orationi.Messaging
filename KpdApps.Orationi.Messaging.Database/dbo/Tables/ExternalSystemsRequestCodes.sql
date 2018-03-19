CREATE TABLE [dbo].[ExternalSystemsRequestCodes]
(
	[ExternalSystemId] UNIQUEIDENTIFIER NOT NULL,
	[RequestCodeId] INT NOT NULL,
	CONSTRAINT [FK_ExternalSystemsRequestCodes_RequestCode] FOREIGN KEY ([RequestCodeId]) REFERENCES [dbo].[RequestCodes]([Id]), 
	CONSTRAINT [FK_ExternalSystemsRequestCodes_ExternalSystems] FOREIGN KEY ([ExternalSystemId]) REFERENCES [dbo].[ExternalSystems]([Id]), 
	CONSTRAINT [PK_ExternalSystemsRequestCodes] PRIMARY KEY ([ExternalSystemId], [RequestCodeId])
	
)