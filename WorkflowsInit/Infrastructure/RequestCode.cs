using Newtonsoft.Json;

namespace WorkflowsInit.Infrastructure
{
    class RequestCode
    {
        [JsonProperty("code")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("description")]
        public string Description { get; set; }
        [JsonProperty("needNotification")]
        public bool NeedNotigication { get; set; }
    }
}
