using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WorkflowsInit.Infrastructure
{
    public class WorkflowAction
    {
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("plugins")]
        public List<Plugin> Plugins { get; set; }
    }
}
