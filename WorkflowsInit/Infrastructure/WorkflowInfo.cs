using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WorkflowsInit.Infrastructure
{
    class WorkflowInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("requestCode")]
        public int RequestCode { get; set; }
        [JsonProperty("workflowActions")]
        public List<WorkflowAction> WorkflowActions { get; set; }

        public void Add()
        {
            /*
            var workflow = Workflows.FirstOrDefault(wf => wf.Name == this.Name);
            if (workflow is null)
            {
                Workflows.InsertOnSubmit(new Workflows
                {
                    Name = this.Name,
                    RequestCode = this.RequestCode
                });
                SubmitChanges();
            }
            */
        }
    }
}
