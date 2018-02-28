CREATE TABLE [dbo].[RequestCodes] (
    [Id]               INT            NOT NULL,
    [Name]             NVARCHAR (50)  NULL,
    [Description]      NVARCHAR (250) NULL,
    [NeedNotification] BIT            CONSTRAINT [DF_RequestCodes_NeedNotification] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RequestCodes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RequestCodes_RequestCodes] FOREIGN KEY ([Id]) REFERENCES [dbo].[RequestCodes] ([Id])
);



