#region Usings

using System.Diagnostics;

#endregion

namespace Frenchex.Dev.Microsoft.Solution.Domain;

public interface IProcessExecutorFactory
{
    IProcessExecutor Factory();
}

public class ProcessExecutorFactory : IProcessExecutorFactory
{
    public IProcessExecutor Factory()
    {
        return new ProcessExecutor();
    }
}

public interface IProcessExecutor
{
    IProcessExecutionResult Start(string workingDirectory, string fileName, string arguments);
}

public class ProcessExecutor : IProcessExecutor
{
    public IProcessExecutionResult Start(string workingDirectory, string fileName, string arguments)
    {
        var process = new Process
                      {
                          StartInfo = new ProcessStartInfo
                                      {
                                          WorkingDirectory = workingDirectory
                                          , FileName = fileName
                                          , Arguments = arguments
                                          , RedirectStandardError = true
                                          , RedirectStandardInput = true
                                          , RedirectStandardOutput = true
                                          , CreateNoWindow = true
                                          , UseShellExecute = false
                                      }
                      };

        var started = process.Start();

        if (!started)
            return new ProcessNotStarted
                   {
                       StandardOutput = process.StandardOutput
                       , StandardError = process.StandardError
                       , ExitCode = process.ExitCode
                       , ExitTime = process.ExitTime
                   };

        var processStarted = new ProcessStarted(process)
                             {
                                 StandardOutput = process.StandardOutput
                                 , StandardError = process.StandardError
                                 , StandardInput = process.StandardInput
                             };

        return processStarted;
    }
}

public interface IProcessExecutionResult
{
}

public class ProcessStarted : IProcessExecutionResult
{
    public ProcessStarted(Process process)
    {
        WaitForExitAsync = async ct =>
                           {
                               await process.WaitForExitAsync(ct);
                               ExitCode = process.ExitCode;
                           };
    }

    public required StreamReader StandardOutput { get; set; }
    public required StreamReader StandardError { get; set; }
    public required StreamWriter StandardInput { get; set; }
    public Func<CancellationToken, Task> WaitForExitAsync { get; private set; }
    public int ExitCode { get; set; }
}

public class ProcessNotStarted : IProcessExecutionResult
{
    public required StreamReader StandardOutput { get; set; }
    public required StreamReader StandardError { get; set; }
    public required int ExitCode { get; set; }
    public required DateTime ExitTime { get; set; }
}