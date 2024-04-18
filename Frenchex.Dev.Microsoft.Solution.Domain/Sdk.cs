using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Frenchex.Dev.Microsoft.Solution.Domain;

public class Sdk
{
    public enum RollForwardPolicies
    {
        [EnumMember(Value = "patch")] Patch
        , [EnumMember(Value = "feature")] Feature
        , [EnumMember(Value = "minor")] Minor
        , [EnumMember(Value = "major")] Major
        , [EnumMember(Value = "latestPatch")] LatestPatch
        , [EnumMember(Value = "latestFeatures")] LatestFeatures
        , [EnumMember(Value = "latestMinor")] LatestMinor
        , [EnumMember(Value = "latestMajor")] LatestMajor
        , [EnumMember(Value = "disable")] Disable
    }

    private Sdk(string version, RollForwardPolicies rollForwardPolicy, bool allowPrerelease)
    {
        Version = version;
        RollForwardPolicy = rollForwardPolicy;
        AllowPrerelease = allowPrerelease;
    }

    [JsonPropertyName("version")]
    public required string Version { get; init; }

    [JsonPropertyName("rollForward")]
    public required RollForwardPolicies RollForwardPolicy { get; init; }

    [JsonPropertyName("allowPrerelease")]
    public required bool AllowPrerelease { get; init; }

    public static Sdk New(string version, RollForwardPolicies rollForwardPolicy, bool allowPrereleases)
    {
        return new Sdk(version, rollForwardPolicy, allowPrereleases)
               {
                   Version = version
                   , RollForwardPolicy = rollForwardPolicy
                   , AllowPrerelease = allowPrereleases
               };
    }
}