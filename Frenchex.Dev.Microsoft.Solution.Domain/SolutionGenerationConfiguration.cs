public class SolutionGenerationConfiguration
{
    private SolutionGenerationConfiguration(string rootPath)
    {
        RootPath = rootPath;
    }

    public string RootPath { get; set; }

    public static SolutionGenerationConfiguration New(string rootPath)
    {
        return new SolutionGenerationConfiguration(rootPath);
    }
}