public class SolutionGenerationOnError : ISolutionGenerationResult
{
    public required string ErrorOutput { get; init; }
    public required string StandardOutput { get; init; }
    public required int ExitCode { get; init; }
}