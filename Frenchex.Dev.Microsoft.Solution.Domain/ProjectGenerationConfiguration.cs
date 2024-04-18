public class ProjectGenerationConfiguration
{
    private ProjectGenerationConfiguration()
    {
    }

    public required SolutionGenerationConfiguration SolutionGenerationConfiguration { get; init; }

    public static ProjectGenerationConfiguration New(SolutionGenerationConfiguration solutionGenerationConfiguration)
    {
        return new ProjectGenerationConfiguration
               {
                   SolutionGenerationConfiguration = solutionGenerationConfiguration
               };
    }
}