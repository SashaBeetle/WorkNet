namespace Worknet.Core.Entities;
public class Skill : DbItem
{
    public string Name { get; set; } = string.Empty;

    public string ProfileId { get; set; }
    public virtual Profile Profile { get; set; } = null!;
}

