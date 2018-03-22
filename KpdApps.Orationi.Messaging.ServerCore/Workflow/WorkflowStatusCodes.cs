namespace KpdApps.Orationi.Messaging.ServerCore.Workflow
{
    public enum WorkflowStatusCodes
    {
        New = 0,
        Preparing = 1000, 
        InProgress = 2000,
        Processed = 3000,
        Error = 9000
    }
}
