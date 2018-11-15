CREATE TABLE [dbo].[ExternalSystems]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY default(newid()), 
    [SystemName] NVARCHAR(50) NOT NULL, 
    [Token] NVARCHAR(50) NOT NULL, 
    [CallbackSettingId] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [AK_ExternalSystems_SystemName] UNIQUE ([SystemName]), 
    CONSTRAINT [FK_ExternalSystems_CallbackSettings] FOREIGN KEY (CallbackSettingId) REFERENCES [dbo].[CallbackSettings]([Id])
)
