namespace Worknet.Core.Entities;
public class Experience : DbItem
{
    public string Position { get; set; }
    public string Company { get; set; }
    public string Location { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Description { get; set; }

    public string ProfileId { get; set; }
    public virtual Profile Profile { get; set; }
}