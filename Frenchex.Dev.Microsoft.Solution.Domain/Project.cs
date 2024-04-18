#region Usings

using System.Collections.ObjectModel;

#endregion

namespace Frenchex.Dev.Microsoft.Solution.Domain;

public class Project
{
    public enum ProjectTypes
    {
        Library,
        Console,
        WebApi
    }

    public static Dictionary<string, string> ProjectTypesDict = new Dictionary<string, string>()
                                                            {
                                                                { "Library", "lib" }
                                                                , { "Console", "console" }
                                                                , { "WebApi", "api" }
                                                                ,
                                                            };
    private Project
    (
        string name,
        string type
    )
    {
        Name = name;
        Type = type;
    }

    public required string Name { get; set; }
    public string? Type { get; set; }


    public ReadOnlyCollection<PackageDependency> Packages => new(_packageDependencies);

    private List<PackageDependency> _packageDependencies { get; } = new();

    public ReadOnlyCollection<ProjectDependency> Projects => new(_projectDependencies);
    private List<ProjectDependency> _projectDependencies { get; } = new();

    public ReadOnlyCollection<IDependency> Dependencies
    {
        get
        {
            var jointDependencies = new List<IDependency>();
            jointDependencies.AddRange(_packageDependencies);
            jointDependencies.AddRange(_projectDependencies);

            return new ReadOnlyCollection<IDependency>(jointDependencies);
        }
    }

    public static Project New
    (
        string name
        , string type
    )
    {
        return new Project(name
                           , type
                          )
               {
                   Name = name
                   , Type = type
               };
    }

    public Project Add(IDependency dependency)
    {
        switch (dependency)
        {
            case PackageDependency package:
                _packageDependencies.Add(package);
                break;
            case ProjectDependency project:
                _projectDependencies.Add(project);
                break;
        }

        return this;
    }

    private List<Modeling.IType> _types = new();
    public IReadOnlyCollection<Modeling.IType> Types => new ReadOnlyCollection<Modeling.IType>(_types);

    public Project Add(Modeling.IType type)
    {
        _types.Add(type);
        return this;
    }

    public ProjectDependency AsDependency()
    {
        return ProjectDependency.New(Name);
    }

    public string GetPath(string rootPath)
    {
        return Path.Combine(rootPath, Name);
    }

    public interface IDependency
    {
        string Name { get; }
    }

    public class PackageDependency : IDependency
    {
        private PackageDependency()
        {
        }

        public required string Version { get; init; }

        public required string Name { get; init; }

        public static PackageDependency New(string name, string version)
        {
            return new PackageDependency
                   {
                       Name = name
                       , Version = version
                   };
        }
    }

    public class ProjectDependency : IDependency
    {
        private ProjectDependency(string name)
        {
            Name = name;
        }

        public required string Name { get; init; }

        public static ProjectDependency New(string name)
        {
            return new ProjectDependency(name)
                   {
                       Name = name
                   };
        }
    }
}