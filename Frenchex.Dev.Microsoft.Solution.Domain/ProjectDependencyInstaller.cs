#region Usings

using Frenchex.Dev.Microsoft.Solution.Domain;

#endregion

public class ProjectDependencyInstaller
{
    public async Task<IProjectDependencyInstallationResult> InstallAsync(string projectPath, string name, IProcessExecutorFactory processExecutorFactory, CancellationToken cancellationToken = default)
    {
        var processExecutor = processExecutorFactory.Factory();
        var started = processExecutor.Start(projectPath, "dotnet", $"add project {name}");

        switch (started)
        {
            case ProcessStarted processStarted:
                await processStarted.WaitForExitAsync(cancellationToken);
                if (processStarted.ExitCode != 0)
                    return new ProjectDependencyInstallationError
                           {
                               Output = await processStarted.StandardOutput.ReadToEndAsync(cancellationToken)
                               , Error = await processStarted.StandardError.ReadToEndAsync(cancellationToken)
                               , ExitCode = processStarted.ExitCode
                           };

                return new ProjectDependencyInstalledSuccessfully
                       {
                           Output = await processStarted.StandardOutput.ReadToEndAsync(cancellationToken)
                       };
            case ProcessNotStarted processNotStarted:

                return new ProjectDependencyInstallationError
                       {
                           Output = await processNotStarted.StandardOutput.ReadToEndAsync(cancellationToken)
                           , Error = await processNotStarted.StandardError.ReadToEndAsync(cancellationToken)
                           , ExitCode = processNotStarted.ExitCode
                       };
            default:
                throw new NotImplementedException();
        }
    }
}