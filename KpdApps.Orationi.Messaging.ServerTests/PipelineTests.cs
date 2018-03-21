using KpdApps.Orationi.Messaging.DummyPlugins;
using KpdApps.Orationi.Messaging.ServerCore.Pipeline;
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
            /*DummyRequest dummyRequest = new DummyRequest();
            dummyRequest.MessageId = "{FB4DE941-323F-497C-91D5-4C95A303025E}";
            dummyRequest.RequestCode = 1;
            string request = dummyRequest.Serialize();

            dummyRequest = new DummyRequest();
            dummyRequest = DummyRequest.Deserialize(request);*/

            PipelineProcessor pipeline = new PipelineProcessor(Guid.Parse("E3F6F3EC-792F-494F-92C4-2E7E02708E4B"), 1);
            pipeline.Init();
            pipeline.Run();
        }
    }
}
