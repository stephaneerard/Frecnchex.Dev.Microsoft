using System.Text.Json.Serialization;

namespace Frenchex.Dev.Microsoft.Solution.Domain;

public class Globals
{
    private Globals(Sdk sdk)
    {
        Sdk = sdk;
    }

    [JsonPropertyName("sdk")]
    public required Sdk Sdk { get; set; }

    public static Globals New(Sdk sdk)
    {
        return new Globals(sdk)
               {
                   Sdk = sdk
               };
    }
}