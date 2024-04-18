using System.Text;

namespace Frenchex.Dev.Microsoft.Solution.Domain;

public interface IFileSystem
{
    Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default);
    Task WriteFileAsync(string path, StringContent stringContent, CancellationToken cancellationToken = default);
    Task<StringContent> ReadFileAsync(string path, Encoding encoding, CancellationToken cancellationToken = default);
}

public class FileSystem : IFileSystem
{
    public Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Path.Exists(path));
    }

    public async Task WriteFileAsync(string path, StringContent stringContent, CancellationToken cancellationToken = default)
    {
        await File.WriteAllBytesAsync(path, await stringContent.ReadAsByteArrayAsync(cancellationToken), cancellationToken);
    }

    public async Task<StringContent> ReadFileAsync(string path, Encoding encoding, CancellationToken cancellationToken = default)
    {
        var content = await File.ReadAllBytesAsync(path, cancellationToken);
        var encodedContent = new StringContent(Encoding.UTF8.GetString(content), encoding);
        return encodedContent;
    }
}