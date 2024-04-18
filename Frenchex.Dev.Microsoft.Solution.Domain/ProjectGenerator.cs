#region Usings

using System.Diagnostics;
using Frenchex.Dev.Microsoft.Solution.Domain;

#endregion

public class ProjectGenerator
{
    private ProjectGenerator()
    {
    }

    public static ProjectGenerator New()
    {
        return new ProjectGenerator();
    }

    public async Task<IProjectGenerationResult> GenerateAsync(Project project, ProjectGenerationConfiguration projectGenerationConfiguration, CancellationToken cancellationToken = default)
    {
        var dirPath = project.GetPath(projectGenerationConfiguration.SolutionGenerationConfiguration.RootPath);

        var dirInfo = new DirectoryInfo(dirPath);

        if (dirInfo.Exists)
        {
            Directory.Delete(dirInfo.FullName);
        }

        dirInfo.Create();

        var process = new Process
                      {
                          StartInfo = new ProcessStartInfo
                                      {
                                          FileName = "dotnet"
                                          , ArgumentList =
                                          {
                                              "new"
                                              , "project"
                                          }
                                          , WorkingDirectory = dirPath
                                      }
                      };

        var started = process.Start();

        if (!started)
            return new ProjectGeneratedErrors
                   {
                       Process = process
                   };

        return new ProjectGeneratedSuccessfully
               {
                   Output = await process.StandardOutput.ReadToEndAsync(cancellationToken)
               };
    }
}