using CommunityToolkit.Diagnostics;
using Frenchex.Dev.Microsoft.Solution.Domain.Seed;
using System.Collections.ObjectModel;
using static Frenchex.Dev.Microsoft.Solution.Domain.IProject;

namespace Frenchex.Dev.Microsoft.Solution.Domain
{

    public interface ISolution : INameable<ISolution>
    {
        ISolution Add(IProject project);
    }
    public class Solution : ISolution
    {
        private Solution()
        {

        }

        public static ISolution New(string name)
        {
            return new Solution().Named(name);
        }

        public string Name { get; private set; }
        public ISolution Named(string name)
        {
            Guard.IsNotNullOrEmpty(name, "name is not empty");
            Name = name;
            return this;
        }


        private List<IProject> _projects { get; } = new();
        public IReadOnlyCollection<IProject> Projects => new ReadOnlyCollection<IProject>(_projects);

        public ISolution Add(IProject project)
        {
            Guard.IsNotNull(project);
            _projects.Add(project);
            return this;
        }
    }


    public interface IProject
    {
        public interface IDataType
        {

        }

        public interface IType
        {

        }

        public interface ITypeDeclarationHolder<out TProject> where TProject : IProject
        {
            public TProject Add(IType type);
        }

        public interface IDependencyHolder<out TProject> where TProject : IProject
        {
            public TProject Add(IDependency dependency);
        }

        public interface ILibraryProject : IProject, ITypeDeclarationHolder<ILibraryProject>
        {
            
        }

        public interface ICommmandHolder<TSub> where TSub : IProject
        {
            TSub AddCommandHandler<TCommand>(IHandler<TCommand> handlerOfCommand) where TCommand : ICommand;
            TSub AddAsyncCommandHandler<TCommand>(IAsyncHandler<TCommand> handlerOfCommand) where TCommand : ICommand;
        }
        public interface ICommand
        {

        }

        public interface IHandler<ICommand>
        {

        }

        public interface IAsyncHandler<ICommand>
        {

        }
        public interface IConsoleProject : IProject, ITypeDeclarationHolder<IConsoleProject>, ICommmandHolder<IConsoleProject>
        {
           

        }

        public interface IWebApiProject : IProject, ITypeDeclarationHolder<IWebApiProject>, ICommmandHolder<IConsoleProject>
        {

        }

        public interface ITestProject : IProject, ITypeDeclarationHolder<ITestProject>
        {

        }

        public interface IDependency
        {
            public interface IPackage
            {
                string Name { get; }
                string Version { get; }
            }

            public interface IProject
            {
                string Name { get; }
                IProject Project { get; }
            }
        }
    }

    public abstract class AbstractProject<TProjectKind> where TProjectKind : class
    {
        public string Name { get; private set; }
        public TProjectKind Named(string name)
        {
            Name = name;
            var cast = this as TProjectKind;
            Guard.IsAssignableToType<TProjectKind>(cast);
            return cast;
        }

        private readonly List<IType> _types = new();
        public IReadOnlyCollection<IType> Types => new ReadOnlyCollection<IType>(_types);
        public TProjectKind Add(IType type)
        {
            _types.Add(type);
            var cast = this as TProjectKind;
            Guard.IsAssignableToType<TProjectKind>(cast);
            return cast;
        }


        private readonly List<IProject> _projects = new();
        public IReadOnlyCollection<IProject> ReferencedProjects => new ReadOnlyCollection<IProject>(_projects);

        public TProjectKind References(IProject project)
        {
            _projects.Add(project);
            var cast = this as TProjectKind;
            Guard.IsAssignableToType<TProjectKind>(cast);
            return cast;
        }
    }

    public class LibraryProject : AbstractProject<LibraryProject>, IProject.ILibraryProject, INameable<LibraryProject>
    {
        private LibraryProject()
        {

        }

        public static ILibraryProject New(string name)
        {
            return new LibraryProject().Named(name);
        }


        public ILibraryProject Add(IType type)
        {
            return base.Add(type);
        }
    }

    public class ConsoleProject : AbstractProject<ConsoleProject>, IProject.IConsoleProject, INameable<ConsoleProject>
    {
        private ConsoleProject() { }

        public static ConsoleProject New(string name)
        {
            return new ConsoleProject().Named(name);
        }

        public IConsoleProject Add(IType type)
        {
            return base.Add(type);
        }

        public IConsoleProject AddCommandHandler<TCommand>(IHandler<TCommand> handlerOfCommand) where TCommand : ICommand
        {
            throw new NotImplementedException();
        }

        public IConsoleProject AddAsyncCommandHandler<TCommand>(IAsyncHandler<TCommand> handlerOfCommand) where TCommand : ICommand
        {
            throw new NotImplementedException();
        }
    }

    public class WebApiProject : AbstractProject<WebApiProject>, IProject.IWebApiProject, INameable<WebApiProject>
    {
        public WebApiProject()
        {
        }

        public static WebApiProject New(string name)
        {
            return new WebApiProject().Named(name);
        }

        public IWebApiProject Add(IType type)
        {
            return base.Add(type);
        }
    }

    public class BaseTestProject<TSub> : AbstractProject<TSub>, INameable<TSub> where TSub : class
    {
        public TSub Add(IType type)
        {
            return base.Add(type);
        }
    }

    public class NUnitTestProject : BaseTestProject<NUnitTestProject>, IProject.ITestProject
    {
        private NUnitTestProject()
        {

        }

        public static NUnitTestProject New(string name)
        {
            return new NUnitTestProject().Named(name);
        }

        public ITestProject Add(IType type)
        {
            return base.Add(type);
        }
    }
}
