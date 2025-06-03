using Worknet.Core.Enums;

namespace Worknet.Core.Entities;
public class Profile : DbItem
{
    public required ProfileType ProfileType { get; set; }
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public GenderEnum Gender { get; set; } = GenderEnum.None;
    public string? Headline { get; set; } = string.Empty; // Title
    public string? About { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public string? ProfilePhotoId { get; set; }
    public string UserId {  get; set; }
    public virtual User User { get; set; }

    public virtual ICollection<Skill> Skills { get; set; }
    public virtual ICollection<Education> Educations { get; set; }
    public virtual ICollection<Experience> Experiences { get; set; }
    public virtual ICollection<GoogleDriveFile> Files { get; set; }
}