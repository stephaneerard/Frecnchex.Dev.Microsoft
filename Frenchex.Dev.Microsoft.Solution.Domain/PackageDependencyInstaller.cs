#region Usings

using Frenchex.Dev.Microsoft.Solution.Domain;

#endregion

public class PackageDependencyInstaller
{
    public async Task InstallAsync(string projectPath, string name, string version, IProcessExecutorFactory processExecutorFactory, CancellationToken cancellationToken = default)
    {
        var processExecutor = processExecutorFactory.Factory();
        var started = processExecutor.Start(projectPath, "dotnet", $"add package {name} -v {version}");

        switch (started)
        {
            case ProcessStarted processStarted:
                await processStarted.WaitForExitAsync(cancellationToken);

                break;
            case ProcessNotStarted processNotStarted:
                break;
        }
    }
}