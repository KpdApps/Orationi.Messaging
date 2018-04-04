using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KpdApps.Orationi.Messaging.DataAccess;
using KpdApps.Orationi.Messaging.DataAccess.Models;
using Newtonsoft.Json;
using WorkflowsInit.Infrastructure;

namespace WorkflowsInit
{
    class Program
    {
        static void Main(string[] args)
        {
            var workflowInfoPath = "WorkflowInfo.json";

            var workflowInfoJson = File.ReadAllText(workflowInfoPath, Encoding.UTF8);

            var serializer = Newtonsoft.Json.JsonSerializer.Create();
            var workflowInfos = serializer.Deserialize<List<WorkflowInfo>>(new JsonTextReader(new StringReader(workflowInfoJson)));

            try
            {
                Process(workflowInfos);
            }
            catch (Exception e)
            {
                Console.WriteLine("Что-то пошло не так.");
                Console.WriteLine($"Exception — {e.Message}{(e.InnerException is null ? "" : $", {e.InnerException.Message}")}");
                Console.WriteLine($"StackTrace — {e.StackTrace}");
            }
            

            Console.WriteLine("Для выхода нажмите любую клавишу...");
            Console.ReadKey();
        }

        static void Process(List<WorkflowInfo> workflowsInfo)
        {
            Console.WriteLine("Начало инициализации.");
            if (workflowsInfo is null)
            {
                throw new ArgumentNullException(nameof(workflowsInfo));
            }

            workflowsInfo.ForEach(workflowInfo =>
            {
                Console.WriteLine(
                    $"Обработка WorkFlow — {workflowInfo.Name} для RequestCode — {workflowInfo.RequestCode.Id}.");
                using (var dbContext = new OrationiDatabaseContext())
                {
                    var workflow = dbContext
                        .Workflows
                        .FirstOrDefault(wf => wf.Name == workflowInfo.Name);
                    Console.Write("Проверка WorkFlow: ");
                    if (workflow is null)
                    {
                        Console.WriteLine("не существует. Будет выполнено создание.");

                        if (workflowInfo.RequestCode is null)
                        {
                            Console.WriteLine($"Дальнейшая обработка WorkFlow — {workflowInfo.Name} невозможна, некорректные данные в узле json \"requestCode\".");
                        }

                        var requestCode = dbContext
                            .RequestCodes
                            .FirstOrDefault(rq => rq.Id == workflowInfo.RequestCode.Id);

                        if (requestCode is null)
                        {
                            Console.WriteLine($"Создание RequestCode — {workflowInfo.RequestCode.Id}.");

                            requestCode = new KpdApps.Orationi.Messaging.DataAccess.Models.RequestCode
                            {
                                Id = workflowInfo.RequestCode.Id,
                                Name = workflowInfo.RequestCode.Name,
                                Description = workflowInfo.RequestCode.Description,
                                NeedNotification = workflowInfo.RequestCode.NeedNotigication
                            };

                            dbContext
                                .RequestCodes
                                .Add(requestCode);
                            dbContext.SaveChanges();
                            Console.WriteLine($"RequestCode — {workflowInfo.RequestCode} — успешно создан.");
                        }

                        workflow = new Workflow
                        {
                            Name = workflowInfo.Name,
                            RequestCodeId = requestCode.Id
                        };

                        dbContext.Workflows.Add(workflow);

                        dbContext.SaveChanges();
                        Console.WriteLine(
                            $"WorkFlow — {workflowInfo.Name} для RequestCode — {workflowInfo.RequestCode.Id} — успешно создан.");
                    }
                    else
                    {
                        Console.WriteLine("существует. Будет выполнено обновление.");
                    }

                    workflowInfo.WorkflowActions.ForEach(workflowActionInfo =>
                    {
                        Console.WriteLine($"Проверка WorkFlowAction с порядковым номером — {workflowActionInfo.Order}: ");

                        var workflowAction = dbContext
                            .WorkflowActions
                            .FirstOrDefault(wa => wa.Order == workflowActionInfo.Order && wa.WorkflowId == workflow.Id);

                        if (workflowAction is null)
                        {
                            Console.Write("не существует. Будет выполнено создание.");
                            var pluginActionSet = new PluginActionSet
                            {
                                Name = $"{workflowActionInfo.Name}ActionSet-Order{workflowActionInfo.Order}"
                            };

                            workflowAction = new KpdApps.Orationi.Messaging.DataAccess.Models.WorkflowAction
                            {
                                WorkflowId = workflow.Id,
                                PluginActionSet = pluginActionSet,
                                Description = workflowActionInfo.Description ?? $"{workflowActionInfo.Name}Workflow",
                                Order = workflowActionInfo.Order
                            };

                            dbContext.WorkflowActions
                                .Add(workflowAction);

                            dbContext.SaveChanges();
                            Console.WriteLine(
                                $"WorkFlowAction с порядковым номером — {workflowActionInfo.Order} — успешно создан.");
                        }
                        else
                        {
                            Console.Write("существует. Будет выполнено обновление.");
                        }

                        workflowActionInfo.Plugins.ForEach(pluginAction =>
                        {
                            Console.WriteLine(
                                $"Обработка Plugin — {pluginAction.ClassName} из сборки — {pluginAction.AssemblyName}");
                            var pluginAssembly = dbContext
                                .PluginAsseblies
                                .FirstOrDefault(p => p.Name == pluginAction.AssemblyName);
                            if (pluginAssembly is null)
                            {
                                Console.WriteLine(
                                    $"Сборка с именем {pluginAction.AssemblyName} отсутствует в БД. Дальнейшая обработка плагина невозможна.");
                                return;
                            }

                            var registeredPlugin = dbContext
                                .RegisteredPlugins
                                .FirstOrDefault(rp =>
                                    rp.Class == pluginAction.ClassName && rp.AssemblyId == pluginAssembly.Id);
                            if (registeredPlugin is null)
                            {
                                Console.WriteLine($"Выполняется добавление Plugin — {pluginAction.ClassName} из сборки — {pluginAction.AssemblyName}");
                                registeredPlugin = new RegisteredPlugin
                                {
                                    Class = pluginAction.ClassName,
                                    AssemblyId = pluginAssembly.Id
                                };

                                dbContext
                                    .RegisteredPlugins
                                    .Add(registeredPlugin);
                                dbContext.SaveChanges();
                            }

                            Console.WriteLine($"Проверка PluginActionSetItem с порядковым номером — {pluginAction.Order}: ");

                            if (workflowAction
                                .PluginActionSet
                                .PluginActionSetItems.All(pasi => pasi.Order != pluginAction.Order))
                            {
                                Console.Write("не существует. Будет выполнено создание.");
                                workflowAction
                                    .PluginActionSet
                                    .PluginActionSetItems
                                    .Add(new PluginActionSetItem
                                    {
                                        PluginActionSetId = workflowAction.PluginActionSet.Id,
                                        RegisteredPluginId = registeredPlugin.Id,
                                        Order = pluginAction.Order,
                                        Configuration = pluginAction.Configuration
                                    });
                                dbContext.SaveChanges();
                                Console.WriteLine($"PluginActionSetItem с порядковым номером {pluginAction.Order} — успешно создан.");
                            }
                            else
                            {
                                Console.Write($"существует. Дальнейшая обработка невозможна, порядковый номер {pluginAction.Order} используется.");
                            }
                        });
                    });
                }
            });
            Console.WriteLine("Инициализация закончена.");
        }
    }
}
