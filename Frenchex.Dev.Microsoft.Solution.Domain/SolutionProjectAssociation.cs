namespace Frenchex.Dev.Microsoft.Solution.Domain;

public class SolutionProjectAssociation
{
    public required Solution Solution { get; init; }
    public required Project Project { get; init; }
}