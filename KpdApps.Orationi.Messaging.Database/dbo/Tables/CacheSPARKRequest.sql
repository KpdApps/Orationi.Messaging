CREATE TABLE [dbo].[CacheSPARKRequest]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT (newid()), 
    [Key] NVARCHAR(MAX) NOT NULL, 
    [Value] NVARCHAR(MAX) NOT NULL, 
    [Created] DATETIME NOT NULL DEFAULT (getdate())
)
go
-- индекс
create index IX_CacheSPARKRequest_Key on [dbo].[CacheSPARKRequest] ([Key]);
go
-- процедура удаления данных из таблицы CacheSPARKRequest старше 21 дня
CREATE PROCEDURE [dbo].[sp_DeleteOldCacheSPARKRequests]
as
begin
    declare @currentDateTime datetime = DATEADD(DAY, 21, curDate());

    delete from [dbo].[CacheSPARKRequest]
        where [Created] <= @currentDateTime;
end
go
