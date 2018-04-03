using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WorkflowsInit.Infrastructure;

namespace WorkflowsInit
{
    class Program
    {
        static void Main(string[] args)
        {
            var workflowInfoPath = "WorkflowInfo.json";

            var workflowInfoJson = File.ReadAllText(workflowInfoPath);

            var serializer = Newtonsoft.Json.JsonSerializer.Create();
            var workflowInfos = serializer.Deserialize<List<WorkflowInfo>>(new JsonTextReader(new StringReader(workflowInfoJson)));
            workflowInfos.ForEach(wf => wf.Add());
        }
    }
}
