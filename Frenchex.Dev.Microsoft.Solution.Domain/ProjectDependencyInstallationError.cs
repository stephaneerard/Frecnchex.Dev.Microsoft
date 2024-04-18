public class ProjectDependencyInstallationError : IProjectDependencyInstallationResult
{
    public required string Output { get; set; }
    public required string Error { get; set; }
    public required int ExitCode { get; set; }
}