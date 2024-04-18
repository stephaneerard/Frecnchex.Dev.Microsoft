namespace Frenchex.Dev.Microsoft.Solution.Domain.Tests;

public class Tests
{
    [Test]
    public async Task Test1()
    {
        var mySolution = Solution.New(
                                      "MySolution"
                                      , Globals.New(
                                                    Sdk.New(
                                                            "net8.0"
                                                            , Sdk.RollForwardPolicies.Disable
                                                            , false
                                                           )
                                                   )
                                     );

        var myFirstProject = Project.New("myFirstProject", Project.ProjectTypesDict["Library"]);
        var mySecondProject = Project.New("mySecondProject", Project.ProjectTypesDict["Library"]);

        const string MsExDiAbstractions = "Microsoft.Extensions.DependencyInjection.Abstractions";
        const string MsExDi = "Microsoft.Extensions.DependencyInjection";
        const string MsPackagesVersion = "8.0.0";

        var packages = new Dictionary<string, Project.PackageDependency>
                       {
                           { MsExDiAbstractions, Project.PackageDependency.New(MsExDiAbstractions, MsPackagesVersion) }
                           , { MsExDi, Project.PackageDependency.New(MsExDi, MsPackagesVersion) }
                       };


        myFirstProject.Add(mySecondProject.AsDependency());
        myFirstProject.Add(packages[MsExDi]);

        mySecondProject.Add(packages[MsExDiAbstractions]);

        var ns1 = Modeling.Namespace.New("My.Fucking.Project");

        var stringType = new Modeling.String();

        var interface0 = Modeling.Interface
                                 .New("MyInterface0")
                                 .AsDeclaredIn(ns1)
            ;
        var interface1 = Modeling.Interface
                                 .New("MyInterface1")
                                 .AsDeclaredIn(ns1)
                                 .Add(Modeling.MethodDeclaration
                                              .New("MyMethod1")
                                              .AsTask()
                                              .ReturnsType(interface0)
                                     )
            ;

        interface1.Add(Modeling.MethodDeclaration
                               .New("MyMethod2")
                               .ReturnsType(interface1)
                      );

        var class1 = Modeling.Class
                             .New("MyClass")
                             .AsDeclaredIn(ns1)
                             .AsAbstract()
                             .AsStatic()
                             .AsFinal()
                             .Add(interface1)
                             .Add(Modeling.Method
                                          .New("MyMethod1")
                                          .ReturnsType(interface0)
                                          .AsAsync()
                                          .AsTask()
                                          .WithParameter(Modeling.Method.Parameter.New("myParameter1").OfType(stringType))
                                          .WithBody((ns, @class, method) => "Console.WriteLine(myParameter1);"))
                             ;

        var class2 = Modeling.Class
                             .New("MyClass2")
                             .AsDeclaredIn(ns1)
                             .Add(interface1)
                             .Add(Modeling.Method
                                          .New("MyMethod1")
                                          .ReturnsType(interface0)
                                          .AsAsync()
                                          .AsTask()
                                          .WithParameter(Modeling.Method.Parameter.New("myParameter1").OfType(stringType))
                                          .WithBody((ns, @class, method) => "Console.WriteLine(myParameter1);"))
                             ;

        myFirstProject.Add(interface0);
        myFirstProject.Add(interface1);
        myFirstProject.Add(class1);
        myFirstProject.Add(class2);

        mySolution.Add(myFirstProject);
        mySolution.Add(mySecondProject);

        var processExecutorFactory = new ProcessExecutorFactory();
        var fs = new FileSystem();

        var solutionGenerator = SolutionGenerator.New(
                                                      GlobalsGenerator.New()
                                                      , ProjectGenerator.New()
                                                      , ClassGenerator.New()
                                                      , InterfaceGenerator.New()
                                                      , EnumGenerator.New()
                                                     );

        var solutionGenerationConfiguration = SolutionGenerationConfiguration.New(@"c:\code\tests\t_" + DateTime.Now.ToString("hhmmss"));
            
        var solutionGenerationResult = await solutionGenerator.GenerateAsync(mySolution
                                                                             , solutionGenerationConfiguration
                                                                             , fs
                                                                             , processExecutorFactory
                                                                            );
    }
}