using System;
using System.Collections.Generic;
using System.Linq;
using KpdApps.Orationi.Messaging.DataAccess.EF.Models;

namespace KpdApps.Orationi.Messaging.DataAccess.EF.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Проверка обращения к таблице GlobalSettings

            WriteCaption("Проверка обращения к таблице GlobalSettings");
            List<GlobalSetting> globalSettings;
            using (var context = new OrationiDatabaseContext())
            {
                globalSettings = context.GlobalSettings.ToList();
            }

            foreach (var globalSetting in globalSettings)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"GlobalSettings: {globalSetting.Name} = {globalSetting.Value};");
            }
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("Для продолжения нажмите любою клавишу...");
            Console.ReadKey();

            #endregion

            #region Проверка обращения к таблице ProcessingErrors

            WriteCaption("Проверка обращения к таблице ProcessingErrors");
            List<ProcessingError> processingErrors;
            using (var context = new OrationiDatabaseContext())
            {
                processingErrors = context.ProcessingErrors.ToList();
            }

            foreach (var processingError in processingErrors)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"ProcessingErrors: {processingError.Id} | {processingError.MessageId} | {processingError.Created} | {processingError.Error} | {processingError.StackTrace};");
            }
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("Для продолжения нажмите любою клавишу...");
            Console.ReadKey();

            #endregion

            #region Проверка обращения к таблице RequestCodeAliases

            WriteCaption("Проверка обращения к таблице RequestCodeAliases");
            List<RequestCodeAlias> requestCodeAliases;
            using (var context = new OrationiDatabaseContext())
            {
                requestCodeAliases = context.RequestCodeAliases.ToList();
            }

            foreach (var requestCodeAlias in requestCodeAliases)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine($"RequestCodeAliases: {requestCodeAlias.Id} | {requestCodeAlias.RequestCode} | {requestCodeAlias.Alias};");
            }
            Console.ForegroundColor = ConsoleColor.Gray;

            Console.WriteLine("Для продолжения нажмите любою клавишу...");
            Console.ReadKey();

            #endregion

            #region Проверка обращения к таблице WorkflowExecutionSteps

            WriteCaption("Проверка обращения к таблице WorkflowExecutionSteps");
            List<WorkflowExecutionStep> workflowExecutionSteps;
            using (var context = new OrationiDatabaseContext())
            {
                workflowExecutionSteps = context
                    .WorkflowExecutionSteps
                    .ToList();


                foreach (var workflowExecutionStep in workflowExecutionSteps)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(
                        $"WorkflowExecutionStep: {workflowExecutionStep.Id} | {workflowExecutionStep.WorkflowId} | {workflowExecutionStep.PluginActionSetId} | {workflowExecutionStep.StatusCode} | {workflowExecutionStep.RequestBody} | {workflowExecutionStep.ResponseBody} | {workflowExecutionStep.ExecutionVariables};");
                    var workflow = workflowExecutionStep.Workflow;
                    var pluginActionSet = workflowExecutionStep.PluginActionSet;
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"\tWorkflow: {workflow.Id} | {workflow.Name} | {workflow.RequestCodeId};");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine($"\tPluginActionSet: {pluginActionSet.Id} | {pluginActionSet.Name};");
                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine("Для продолжения нажмите любою клавишу...");
            Console.ReadKey();

            #endregion

            #region Проверка обращения к таблице ExternalSystems

            WriteCaption("Проверка обращения к таблице ExternalSystems");
            using (var context = new OrationiDatabaseContext())
            {
                var externalSystems = context
                    .ExternalSystems
                    .ToList();


                foreach (var externalSystem in externalSystems)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(
                        $"ExternalSystem: {externalSystem.Id} | {externalSystem.SystemName} | {externalSystem.Token};");
                    var requestCodes = externalSystem.RequestCodes;
                    var messages = externalSystem.Messages;
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    foreach (var requestCode in requestCodes)
                    {
                        Console.WriteLine(
                            $"\tRequestCode: {requestCode.Id} | {requestCode.Name} | {requestCode.Description} | {requestCode.NeedNotification};");
                    }

                    foreach (var message in messages)
                    {
                        Console.WriteLine($"\tMessage: {message.Id} | {message.StatusCode};");
                    }
                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine("Для продолжения нажмите любою клавишу...");
            Console.ReadKey();

            #endregion

            #region Проверка обращения к таблице Messages

            WriteCaption("Проверка обращения к таблице Messages");
            using (var context = new OrationiDatabaseContext())
            {
                var messages = context
                    .Messages
                    .ToList();

                foreach (var message in messages)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(
                        $"ExternalSystem: {message.Id} | {message.StatusCode};");
                    var messageStatusCode = message.MessageStatusCode;
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.WriteLine($"\tMessageStatusCode: {messageStatusCode.Id} | {messageStatusCode.Name} | {messageStatusCode.Description};");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;

                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine("Для продолжения нажмите любою клавишу...");
            Console.ReadKey();

            #endregion

            #region Проверка обращения к таблице WorkflowExecutionStepsStatusCodes

            WriteCaption("Проверка обращения к таблице WorkflowExecutionStepsStatusCodes");
            using (var context = new OrationiDatabaseContext())
            {
                var statuses = context
                    .WorkflowExecutionStepsStatusCodes
                    .ToList();

                foreach (var status in statuses)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    Console.WriteLine(
                        $"WorkflowExecutionStepsStatusCode: {status.Id} | {status.Name} | {status.Description};");
                    var workFlowExecutionSteps = status.WorkflowExecutionSteps;
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    foreach (var workflowExecutionStep in workFlowExecutionSteps)
                    {
                        Console.WriteLine($"\tWorkFlowExecutionSteps: {workflowExecutionStep.Id} | {workflowExecutionStep.WorkflowId} | {workflowExecutionStep.StatusCode};");
                    }

                }
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine("Для продолжения нажмите любою клавишу...");
            Console.ReadKey();

            #endregion
        }

        private static void WriteCaption(string caption)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine(caption);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
    }
}
