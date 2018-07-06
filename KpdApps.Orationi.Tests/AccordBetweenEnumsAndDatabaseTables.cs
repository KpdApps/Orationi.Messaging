using System;
using System.Collections.Generic;
using System.Linq;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.ServerCore.Pipeline;
using KpdApps.Orationi.Messaging.ServerCore.Workflow;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace KpdApps.Orationi.Tests
{
    [TestClass]
    public class AccordBetweenEnumsAndDatabaseTables
    {
        [TestMethod]
        public void СоответствиеМеждуPipelineStatusCodesИWorkflowExecutionStepsStatusCodes()
        {
            using (var dbContext = new OrationiDatabaseContext())
            {
                var workflowExecutionStepsStatusCodes = dbContext
                    .WorkflowExecutionStepsStatusCodes
                    .ToList();

                List<(string Name, int Value)> pipelineStatusCodes = Enum
                    .GetNames(typeof(PipelineStatusCodes))
                    .Select(n => (n, (int)Enum.Parse(typeof(PipelineStatusCodes), n)))
                    .ToList();

                Assert.AreEqual(workflowExecutionStepsStatusCodes.Count, 
                    pipelineStatusCodes.Count, 
                    "В таблице WorkflowExecutionStepsStatusCodes и в типе PipelineStatusCodes должно быть одинаковое количество элементов");

                var errorEnumValues = new List<string>();

                foreach (var pipelineStatusCode in pipelineStatusCodes)
                {
                    if (!workflowExecutionStepsStatusCodes
                        .Any(wessc => wessc.Id == pipelineStatusCode.Value && wessc.Name == pipelineStatusCode.Name))
                    {
                        errorEnumValues.Add($"{pipelineStatusCode.Name}({pipelineStatusCode.Value})");
                    }
                }
                Assert.IsFalse(errorEnumValues.Any(), "В таблице WorkflowExecutionStepsStatusCodes для следующих значений типа PipelineStatusCodes: " +
                    $"{string.Join(", ", errorEnumValues)} не найдено соответствий");
            }
        }

        [TestMethod]
        public void СоответствиеМеждуMessageStatusCodesИMessageStatusCode()
        {
            using (var dbContext = new OrationiDatabaseContext())
            {
                var messageStatusCodes = dbContext
                    .MessageStatusCodes
                    .ToList();

                List<(string Name, int Value)> enumMessageStatusCodes = Enum
                    .GetNames(typeof(MessageStatusCodes))
                    .Select(n => (n, (int)Enum.Parse(typeof(MessageStatusCodes), n)))
                    .ToList();

                Assert.AreEqual(messageStatusCodes.Count,
                    enumMessageStatusCodes.Count,
                    "В таблице MessageStatusCodes и в типе MessageStatusCodes должно быть одинаковое количество элементов");

                var errorEnumValues = new List<string>();

                foreach (var enumMessageStatusCode in enumMessageStatusCodes)
                {
                    if (!messageStatusCodes
                        .Any(wessc => wessc.Id == enumMessageStatusCode.Value && wessc.Name == enumMessageStatusCode.Name))
                    {
                        errorEnumValues.Add($"{enumMessageStatusCode.Name}({enumMessageStatusCode.Value})");
                    }
                }
                Assert.IsFalse(errorEnumValues.Any(), "В таблице MessageStatusCodes для следующих значений типа MessageStatusCodes: " +
                                                      $"{string.Join(", ", errorEnumValues)} не найдено соответствий");
            }
        }
    }
}
