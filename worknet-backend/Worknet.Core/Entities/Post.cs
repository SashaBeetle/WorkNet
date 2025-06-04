namespace Worknet.Core.Entities;
public class Post : DbItem
{
    public string Data { get; set; }
    public string UserId { get; set; }
    public virtual User User { get; set; }
}

