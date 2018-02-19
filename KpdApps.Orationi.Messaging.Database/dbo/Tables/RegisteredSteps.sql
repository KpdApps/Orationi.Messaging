CREATE TABLE [dbo].[RegisteredSteps] (
    [Id]             UNIQUEIDENTIFIER CONSTRAINT [DF_RegisteredSteps_Id] DEFAULT (newid()) NOT NULL,
    [RequestCode]    INT              NOT NULL,
    [Order]          INT              NOT NULL,
    [IsAsynchronous] BIT              CONSTRAINT [DF_RegisteredSteps_IsAsynchronous] DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_RegisteredSteps] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_RegisteredSteps_RequestCodes] FOREIGN KEY ([RequestCode]) REFERENCES [dbo].[RequestCodes] ([Id]) ON DELETE CASCADE ON UPDATE CASCADE
);

