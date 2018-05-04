namespace KpdApps.Orationi.Messaging.ClientConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            //var configuration = new ConfigurationBuilder()
            //    .SetBasePath(Environment.CurrentDirectory)
            //    .AddJsonFile("appsettings.json")
            //    .Build();

            //var dbContext = new OrationiMessagingContext(new OrationiContextOptionsBuilder(configuration));

            //var httpContext = new DefaultHttpContext();
            //httpContext.Request.Headers["Token"] = "Secure";

            //IncomingMessageProcessor imp = new IncomingMessageProcessor(dbContext, httpContext);

            //while (true)
            //{
            //    DummyRequest dummyRequest = new DummyRequest
            //    {
            //        MessageId = Guid.NewGuid().ToString(),
            //        RequestCode = 1
            //    };

            //    Request request = new Request()
            //    {
            //        Body = dummyRequest.Serialize(),
            //        Code = 1,
            //        UserName = "Dummy"
            //    };

            //    Console.WriteLine($" ==> {JsonConvert.SerializeObject(request)}");
            //    Response response = imp.Execute(request);

            //    Console.WriteLine($" <== {JsonConvert.SerializeObject(response)}");
            //    Thread.Sleep(1000);
            //}
        }
    }
}
