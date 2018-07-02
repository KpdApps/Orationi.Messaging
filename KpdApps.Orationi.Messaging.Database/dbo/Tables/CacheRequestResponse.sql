CREATE TABLE [dbo].[CacheRequestResponse]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT (newid()), 
    [Key] NVARCHAR(256) NOT NULL, 
    [Value] NVARCHAR(MAX) NOT NULL, 
    [ExpireDate] DATETIME NOT NULL
)
go
-- индекс по ключу
create index IX_CacheRequestResponse_Key on [dbo].[CacheRequestResponse] ([Key]);
go
-- индекс по дате (срок хранения)
create index IX_CacheRequestResponse_ExpireDate on [dbo].[CacheRequestResponse] ([ExpireDate]);
go
-- процедура удаления данных из таблицы CacheRequestResponse срок хранения которых истек
CREATE PROCEDURE [dbo].[sp_DeleteOldCacheRequestResponse]
as
begin
    declare @currentDateTime datetime = getdate();

    delete from [dbo].[CacheRequestREsponse]
        where [ExpireDate] <= @currentDateTime;
end
go