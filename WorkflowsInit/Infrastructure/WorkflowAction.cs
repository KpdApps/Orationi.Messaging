using System.Collections.Generic;
using Newtonsoft.Json;

namespace WorkflowsInit.Infrastructure
{
    public class WorkflowAction
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("plugins")]
        public List<Plugin> Plugins { get; set; }
    }
}
