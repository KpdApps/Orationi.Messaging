CREATE TABLE [dbo].[ExternalSystemsRequestCodes] (
    [ExternalSystemId] UNIQUEIDENTIFIER NOT NULL,
    [RequestCodeId]    INT              NOT NULL,
    CONSTRAINT [PK_ExternalSystemsRequestCodes] PRIMARY KEY CLUSTERED ([ExternalSystemId] ASC, [RequestCodeId] ASC),
    CONSTRAINT [FK_ExternalSystemsRequestCodes_ExternalSystems] FOREIGN KEY ([ExternalSystemId]) REFERENCES [dbo].[ExternalSystems] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE,
    CONSTRAINT [FK_ExternalSystemsRequestCodes_RequestCode] FOREIGN KEY ([RequestCodeId]) REFERENCES [dbo].[RequestCodes] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

