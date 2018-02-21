using KpdApps.Orationi.Messaging.DummyPlugins;
using KpdApps.Orationi.Messaging.ServerConsole.Pipeline;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace KpdApps.Orationi.Messaging.ServerTests
{
    [TestClass]
    public class PipelineTests
    {
        [TestMethod]
        public void RunTest()
        {
            DummyRequest dummyRequest = new DummyRequest();
            dummyRequest.MessageId = "{FB4DE941-323F-497C-91D5-4C95A303025E}";
            dummyRequest.RequestCode = 1;
            string request = dummyRequest.Serialize();

            dummyRequest = new DummyRequest();
            dummyRequest = DummyRequest.Deserialize(request);

            Pipeline pipeline = new Pipeline(Guid.Parse("C95F170F-2F29-42AE-A7ED-665DC58243EA"), 1);
            pipeline.Init();
            pipeline.Run();
        }
    }
}
