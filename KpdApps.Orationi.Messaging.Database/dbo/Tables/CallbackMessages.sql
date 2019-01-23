CREATE TABLE [dbo].[CallbackMessages]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newid(), 
    [MessageId] UNIQUEIDENTIFIER NOT NULL, 
    [Modified] DATETIME NOT NULL DEFAULT getdate(), 
    [CanBeSend] BIT NOT NULL DEFAULT 0, 
    [WasSend] BIT NOT NULL DEFAULT 0, 
    [StatusCode] INT NOT NULL DEFAULT 0, 
    [ErrorMessage] NVARCHAR(MAX) NULL, 
    CONSTRAINT [FK_CallbackMessages_Messages] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Messages]([Id])
)

GO

CREATE unique INDEX [IX_CallbackMessages_MessageId] ON [dbo].[CallbackMessages] ([MessageId])
