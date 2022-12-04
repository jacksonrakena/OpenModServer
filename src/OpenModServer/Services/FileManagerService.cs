using System.Configuration;
using OpenModServer.Structures;

namespace OpenModServer.Services;

public record FileCreationResult(string Path, string Name, ulong SizeKilobytes);
public class FileManagerService
{
    private static readonly Random _random = new Random();
    
    private readonly string _uploadPath;
    public FileManagerService(OmsConfig config)
    {
        _uploadPath =  config.Paths.Uploads;
        if (!Path.Exists(config.Paths.Uploads))
        {
            throw new ConfigurationErrorsException($"Paths::Upload path '{_uploadPath}' does not exist.");
        }
    }

    public async Task<FileCreationResult?> AcceptIncomingReleaseUploadAsync(IFormFile upload)
    {
        var name = GenerateRandomFileName(upload);
        var pathName = Path.Combine(_uploadPath, name);

        await using var fileStream = new FileStream(pathName, FileMode.Create);
        await upload.CopyToAsync(fileStream);
        ulong sizeKb = 0;
        try
        {
            sizeKb = (ulong) fileStream.Length / 1000;
        }
        catch
        {
            // ignored
        }

        return new FileCreationResult(pathName, name, sizeKb);
    }

    private string GenerateRandomFileName(IFormFile file)
    {
        var name = new byte[6];
        _random.NextBytes(name);
        return Convert.ToBase64String(name) + ".zip";
    }
}