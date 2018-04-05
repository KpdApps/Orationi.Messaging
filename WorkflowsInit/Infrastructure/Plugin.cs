﻿using Newtonsoft.Json;

namespace WorkflowsInit.Infrastructure
{
    public class Plugin
    {
        [JsonProperty("assemblyName")]
        public string AssemblyName { get; set; }
        [JsonProperty("className")]
        public string ClassName { get; set; }
        [JsonProperty("order")]
        public int Order { get; set; }
        [JsonProperty("configuration")]
        public string Configuration { get; set; }
    }
}