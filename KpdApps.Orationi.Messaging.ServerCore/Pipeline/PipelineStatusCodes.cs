using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KpdApps.Orationi.Tests")]
namespace KpdApps.Orationi.Messaging.ServerCore.Pipeline
{
    internal enum PipelineStatusCodes
    {
        New = 0,
        InProgress = 1000,
        Finished = 3000,
        Error = 9000
    }
}
