CREATE TABLE [dbo].[CallbackSettings]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT (newid()), 
    [MethodType] NVARCHAR(4) NOT NULL, 
    [RequestTargetUrl] NVARCHAR(255) NOT NULL, 
    [UrlAccessUserName] NVARCHAR(50) NULL, 
    [UrlAccessUserPassword] NVARCHAR(50) NULL, 
    [NeedAuthentification] BIT NOT NULL DEFAULT 0
)
