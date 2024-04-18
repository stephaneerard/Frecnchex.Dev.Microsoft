#region Usings

#endregion

namespace Frenchex.Dev.Microsoft.Solution.Domain;

public class Solution
{
    public static string DefaultNetVersion = "net8.0";
    public static Sdk.RollForwardPolicies DefaultRollForwardPolicy = Sdk.RollForwardPolicies.Disable;
    public static bool DefaultAllowPrereleases = false;

    private Solution(string name, Globals? globals, List<SolutionProjectAssociation>? projects = null)
    {
        Name = name;
        Globals = globals ?? Globals.New(Sdk.New(DefaultNetVersion
                                                 , DefaultRollForwardPolicy
                                                 , DefaultAllowPrereleases
                                                )
                                        );
        Projects = projects ?? new List<SolutionProjectAssociation>();
    }

    public string Name { get; init; }
    public List<SolutionProjectAssociation> Projects { get; init; }
    public Globals Globals { get; init; }

    public static Solution New(string name, Globals? globals, List<SolutionProjectAssociation>? projects = null)
    {
        return new Solution(name
                            , globals
                            , projects
                           )
               {
                   Globals = globals!
                   , Name = name
                   , Projects = projects ?? new List<SolutionProjectAssociation>()
               };
    }

    public Solution Add(Project project)
    {
        var association = new SolutionProjectAssociation
                          {
                              Solution = this
                              , Project = project
                          };

        Projects.Add(association);

        return this;
    }
}