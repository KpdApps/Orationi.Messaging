using System;
using System.Diagnostics;
using System.IO;

namespace KpdApps.Orationi.WinNTHostService.Helpers
{
    public static class Helpers
    {
        public static void WriteToFile(string str)
        {
            var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
            var pathToRoot = Path.GetDirectoryName(pathToExe);
            var pathToLog = Path.Combine(pathToRoot, "logs");
            if (!Directory.Exists(pathToLog))
            {
                Directory.CreateDirectory(pathToLog);
            }

            var pathToLogFile = Path.Combine(pathToLog, $"KpdApps.Orationi.HostServices-{DateTime.Now.ToFileTime()}.log");
            using (var stream = new FileStream(pathToLogFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.Read))
            using (var streamWriter = new StreamWriter(stream))
            {
                streamWriter.WriteLine(str);
            }
        }
    }
}