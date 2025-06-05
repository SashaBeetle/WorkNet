using Worknet.Core.Entities;

namespace Worknet.Shared.Models.DTOs;
public class PostDto
{
    public string? Id { get; set; }
    public DateTime? CreatedAt { get; set; }
    public string? Data { get; set; }
    public string? UserId { get; set; }
    public string? ProfilePhotoId { get; set; }
    public User? User { get; set; }
}