using Worknet.Core.Entities;
using Worknet.Core.Enums;

namespace Worknet.Shared.Models.DTOs;
public class ProfileDto
{
    public string? Id { get; set; }
    public ProfileType? ProfileType { get; set; }
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public GenderEnum? Gender { get; set; } = GenderEnum.None;
    public string? Headline { get; set; } = string.Empty; // Title
    public string? About { get; set; } = string.Empty;
    public string? Location { get; set; } = string.Empty;

    public string? UserId { get; set; }
    public string? ProfilePhotoId { get; set; }
    public ICollection<SkillDto>? Skills { get; set; }
    public ICollection<EducationDto>? Educations { get; set; }
    public ICollection<ExperienceDto>? Experiences { get; set; }
    public ICollection<GoogleDriveFile>? Files { get; set; }
}