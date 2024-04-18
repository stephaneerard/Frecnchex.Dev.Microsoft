namespace Frenchex.Dev.Microsoft.Solution.Domain.Tests
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var solution = Solution.New("mySolution");

            var lib1Project = LibraryProject.New("Library1");
            var console1Project = ConsoleProject.New("Console1");

            var webapi1Project = WebApiProject.New("WebApi1");
            var testLib1Project = NUnitTestProject.New("Library1.Tests");
            var testConsole1Project = NUnitTestProject.New("Console1.Tests");
            var testWebApi1Project = NUnitTestProject.New("WebApi1.Tests");

            console1Project.References(lib1Project);
            webapi1Project.References(lib1Project);
            testLib1Project.References(lib1Project);
            testConsole1Project.References(console1Project);
            testWebApi1Project.References(webapi1Project);

            solution.Add(lib1Project);
            solution.Add(console1Project);
            solution.Add(webapi1Project);
            solution.Add(testLib1Project);
            solution.Add(testConsole1Project);
            solution.Add(testWebApi1Project);

            var generationPlanBuilder = SolutionGenerationPlanBuilder.New();
            var generationPlan = solution.BuildGenerationPlanAsync();


            switch (generationPlan)
            {
                case ErrorBuildingGenerationPlan error:
                    break;
                case SolutionGenerationPlanWorkflow generationPlanWorkflow:
                    break;
            }

            var generationPlanExecutor = SolutionGenerationPlanExecutor.New();
            var generationPlanExecutionResult = generationPlanExecutor.ExecuteAsync(generationPlan);


        }
    }
}