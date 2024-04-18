#region Usings

using Frenchex.Dev.Microsoft.Solution.Domain;

#endregion

public class DependencyInstaller : IDependencyInstaller
{
    private readonly PackageDependencyInstaller _packageDependencyInstaller;
    private readonly ProjectDependencyInstaller _projectDependencyInstaller;

    public DependencyInstaller(
        PackageDependencyInstaller packageDependencyInstaller,
        ProjectDependencyInstaller projectDependencyInstaller
    )
    {
        _packageDependencyInstaller = packageDependencyInstaller;
        _projectDependencyInstaller = projectDependencyInstaller;
    }

    public async Task InstallAsync(Project project, Project.IDependency dependency, ProjectGenerationConfiguration projectGenerationConfiguration, IProcessExecutorFactory processExecutorFactory, CancellationToken cancellationToken = default)
    {
        var targetProjectPath = Path.Join(projectGenerationConfiguration.SolutionGenerationConfiguration.RootPath, project.Name);

        switch (dependency)
        {
            case Project.PackageDependency packageDependency:
                await _packageDependencyInstaller.InstallAsync(targetProjectPath, packageDependency.Name, packageDependency.Version, processExecutorFactory, cancellationToken);
                break;
            case Project.ProjectDependency projectDependency:
                await _projectDependencyInstaller.InstallAsync(targetProjectPath, projectDependency.Name, processExecutorFactory, cancellationToken);
                break;
        }
    }
}