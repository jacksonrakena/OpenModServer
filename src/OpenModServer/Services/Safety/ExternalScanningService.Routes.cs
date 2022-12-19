namespace OpenModServer.Services.Safety;

public partial class ExternalScanningService
{
    private readonly string _getLargeUploadUrl = "https://www.virustotal.com/api/v3/files/upload_url";
    private string _getFileReportUrl(string id) => $"https://www.virustotal.com/api/v3/files/{id}";
}