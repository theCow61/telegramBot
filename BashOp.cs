using System.Diagnostics;

namespace ShittyTea
{
    class BashOp
    {
        public string cmd { get; set; }
        public string Bash()
        {
            string escapedArgs = cmd.Replace("\"", "\\\"");
            Process process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            string refResult = result.Replace("-", "");
            process.WaitForExit();
            return refResult;
        }
    }
}
