/*
Post-Deployment Script Template                            
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.        
 Use SQLCMD syntax to include a file in the post-deployment script.            
 Example:      :r .\myfile.sql                                
 Use SQLCMD syntax to reference a variable in the post-deployment script.        
 Example:      :setvar TableName MyTable                            
               SELECT * FROM [$(TableName)]                    
--------------------------------------------------------------------------------------
*/

insert into [dbo].[MessageStatusCode]([Id], [Name])
values (0, 'New'), (1000, 'Preparing'), (2000, 'InProgress'), (3000, 'Processed'), (9000, 'Error');
go
INSERT INTO [dbo].[WorkflowExecutionStepsStatusCodes] ([Id], [Name]) 
VALUES (0, 'New'), (1000, 'InProgress'), (3000, 'Finished'), (9000, 'Error');
go

-- job по запуску удаления данных из таблицы 
use msdb
go
EXEC dbo.sp_add_job
    @job_name = N'Delete old records from CacheRequestResponse',
    @enabled = enabled;
    
    
GO  
EXEC sp_add_jobstep
    @job_name = N'Delete old records from CacheRequestResponse',  
    @step_name = N'Delete records',
    @database_name = 'OrationiMessageBus',
    @command = N'exec [dbo].[sp_DeleteOldCacheRequestResponse]',
    @retry_attempts = 5,
    @retry_interval = 5;
GO  
EXEC dbo.sp_add_schedule
    @schedule_name = N'Every day',
    @freq_type = 4,
    @active_start_time = 233000;
USE msdb ;  
GO  
EXEC sp_attach_schedule
   @job_name = N'Delete old records from CacheRequestResponse',
   @schedule_name = N'Every day';
GO  
EXEC dbo.sp_add_jobserver
    @job_name = N'Delete old records from CacheRequestResponse';
GO  