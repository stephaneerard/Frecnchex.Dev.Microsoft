#region Usings

using CommunityToolkit.Diagnostics;
using Frenchex.Dev.Microsoft.Solution.Domain;

#endregion

public class SolutionGenerator
{
    private SolutionGenerator()
    {
    }

    public required GlobalsGenerator GlobalsGenerator { get; init; }
    public required ProjectGenerator ProjectGenerator { get; init; }
    public required EnumGenerator EnumGenerator { get; init; }
    public required InterfaceGenerator InterfaceGenerator { get; init; }
    public required ClassGenerator ClassGenerator { get; init; }


    public static SolutionGenerator New(GlobalsGenerator globalsGenerator, ProjectGenerator projectGenerator, ClassGenerator classGenerator, InterfaceGenerator interfaceGenerator, EnumGenerator enumGenerator)
    {
        return new SolutionGenerator
        {
            GlobalsGenerator = globalsGenerator
                   ,
            ProjectGenerator = projectGenerator
                   ,
            ClassGenerator = classGenerator
                   ,
            InterfaceGenerator = interfaceGenerator
                   ,
            EnumGenerator = enumGenerator
        };
    }


    /// <summary>
    /// 
    /// </summary>
    /// <param name="solution"></param>
    /// <param name="solutionGenerationConfiguration"></param>
    /// <param name="fileSystem"></param>
    /// <param name="processExecutorFactory"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task<ISolutionGenerationResult> GenerateAsync(Solution solution, SolutionGenerationConfiguration solutionGenerationConfiguration, IFileSystem fileSystem, IProcessExecutorFactory processExecutorFactory, CancellationToken cancellationToken = default)
    {
        var dirInfo = new DirectoryInfo(solutionGenerationConfiguration.RootPath);

        if (dirInfo.Exists)
        {
            Directory.Delete(dirInfo.FullName);
        }

        dirInfo.Create();

        var process = processExecutorFactory.Factory();
        var startResult = process.Start(solutionGenerationConfiguration.RootPath
                                        , "dotnet"
                                        , "new sln"
                                       );

        switch (startResult)
        {
            case ProcessNotStarted processNotStarted:
                return new SolutionGenerationOnError
                {
                    StandardOutput = await processNotStarted.StandardOutput.ReadToEndAsync(CancellationToken.None)
                           ,
                    ErrorOutput = await processNotStarted.StandardError.ReadToEndAsync(CancellationToken.None)
                           ,
                    ExitCode = processNotStarted.ExitCode
                };
            case ProcessStarted processStarted:
                await processStarted.WaitForExitAsync(cancellationToken);
                break;
        }

        await GlobalsGenerator.GenerateAsync(solution.Globals
                                             , solutionGenerationConfiguration
                                             , fileSystem
                                             , cancellationToken
                                            );

        foreach (var projectSolutionAssociation in solution.Projects)
        {
            await ProjectGenerator.GenerateAsync(projectSolutionAssociation.Project
                                                 , ProjectGenerationConfiguration.New(solutionGenerationConfiguration)
                                                 , cancellationToken
                                                );
        }

        return new SolutionGeneratedSuccessfully();
    }
}

public interface ISolutionGenerationWorkflow
{
    ISolutionGenerationWorkflow Add<TResult>(ISolutionGenerationWorkflowStep<TResult> step);
    Task<ISolutionGenerationWorkflowResult<TResult>> GenerateAsync<TResult>(Solution solution, SolutionGenerationConfiguration solutionGenerationConfiguration, SolutionGenerationWorkflowDependencies dependencies, CancellationToken cancellationToken = default);
}

public class SolutionGenerationWorklowExecutionStack
{
    public List<ISolutionGenerationWorkflowStepResult<object>> StepsResults { get; } = new();
}

public interface ISolutionGenerationWorkflowStep<TResult>
{
    Task<ISolutionGenerationWorkflowStepResult<TResult>> ExecuteAsync(Solution solution, SolutionGenerationConfiguration solutionGenerationConfiguration, SolutionGenerationWorkflowDependencies dependencies, SolutionGenerationWorklowExecutionStack executionStackStack, CancellationToken cancellationToken = default);
}

public interface ISolutionGenerationWorkflowStepResult<TResult>
{
    ISolutionGenerationWorkflowStep<TResult> Step { get; }
}

public interface ISolutionGenerationWorkflowResult<out TResult>
{
    TResult Result { get; }
}

public class SolutionGenerationWorkflowRanSuccessfully
{

}

public class SolutionGenerationWorkflowDependencies
{
    public IFileSystem FileSystem { get; private set; }
    public IProcessExecutorFactory ProcessExecutorFactory { get; private set; }

    public SolutionGenerationWorkflowDependencies(IFileSystem fileSystem, IProcessExecutorFactory processExecutorFactory)
    {
        Guard.IsAssignableToType<IFileSystem>(fileSystem);
        Guard.IsAssignableToType<IProcessExecutorFactory>(processExecutorFactory);

        FileSystem = fileSystem;
        ProcessExecutorFactory = processExecutorFactory;
    }
}