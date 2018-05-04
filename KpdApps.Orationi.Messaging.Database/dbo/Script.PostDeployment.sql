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
