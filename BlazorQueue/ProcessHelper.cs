using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ClipHost
{
    public class ProcessHelper
    {
        public static Process? StartProcess(string programPath, params string[] args)
        {
            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = programPath,
                        Arguments = string.Join(" ", args.Select(s => s = "\"" + Regex.Replace(s, @"(\\+)$", @"$1$1") + "\"")),
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        CreateNoWindow = true
                    }
                };


                return process;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error starting streamlink: {0} {1}", e.Message, e.StackTrace);
            }

            return null;
        }
    }
}