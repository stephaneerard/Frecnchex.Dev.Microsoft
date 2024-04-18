namespace Frenchex.Dev.Microsoft.Solution.Domain.Seed
{
    public interface INamed
    {
        string Name { get; }
    }

    public interface INameable<out TReturn>
    {
        string Name { get; }
        TReturn Named(string name);
    }
}
