using Worknet.Core.Enums;

namespace Worknet.Shared.Models.DTOs;
public class GoogleDriveFileDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public MimeType? ExtensionType { get; set; }
    public string? ViewLink { get; set; }
}