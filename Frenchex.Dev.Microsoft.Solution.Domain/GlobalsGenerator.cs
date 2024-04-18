#region Usings

using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Frenchex.Dev.Microsoft.Solution.Domain;

#endregion

public class GlobalsGenerator
{
    private GlobalsGenerator()
    {
    }

    public static GlobalsGenerator New()
    {
        return new GlobalsGenerator();
    }

    public async Task GenerateAsync(Globals globals, SolutionGenerationConfiguration solutionGenerationConfiguration, IFileSystem fileSystem, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(globals
                                            , new JsonSerializerOptions
                                              {
                                                  DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                                                  PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                                  Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
                                              }
                                           );

        var globalsPath = Path.Join(solutionGenerationConfiguration.RootPath
                                    , "globals.json"
                                   );

        await fileSystem.WriteFileAsync(globalsPath
                                        , new StringContent(json
                                                            , Encoding.UTF8
                                                           )
                                        , cancellationToken
                                       );
    }
}

public class ClassGenerator
{
    private ClassGenerator()
    {

    }

    public static ClassGenerator New()
    {
        return new ClassGenerator();
    }
}

public class EnumGenerator
{
    private EnumGenerator()
    {

    }

    public static EnumGenerator New()
    {
        return new EnumGenerator();
    }
}



public class InterfaceGenerator
{
    private InterfaceGenerator()
    {

    }

    public static InterfaceGenerator New()
    {
        return new InterfaceGenerator();
    }
}