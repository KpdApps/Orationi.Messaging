using System.Collections.Generic;
using Newtonsoft.Json;

namespace WorkflowsInit.Infrastructure
{
    class WorkflowInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("requestCode")]
        public RequestCode RequestCode { get; set; }
        [JsonProperty("workflowActions")]
        public List<WorkflowAction> WorkflowActions { get; set; }
    }
}
