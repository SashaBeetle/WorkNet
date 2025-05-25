using Worknet.Core.Enums;

namespace Worknet.Core.Entities;
public class GoogleDriveFile : DbItem
{
    public string Name { get; set; }
    public required MimeType ExtensionType { get; set; }
    public required string ViewLink { get; set; }

    public string? ProfileId { get; set; }
    public virtual Profile? Profile { get; set; }
}