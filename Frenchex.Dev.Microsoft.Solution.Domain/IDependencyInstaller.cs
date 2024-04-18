#region Usings

using Frenchex.Dev.Microsoft.Solution.Domain;

#endregion

public interface IDependencyInstaller
{
    Task InstallAsync(Project project, Project.IDependency dependency, ProjectGenerationConfiguration projectGenerationConfiguration, IProcessExecutorFactory processExecutorFactory, CancellationToken cancellationToken = default);
}