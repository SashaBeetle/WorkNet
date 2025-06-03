namespace Worknet.Core.Entities;

public class Education : DbItem
{
    public string Degree { get; set; }
    public string Institution { get; set; }
    public string Domain { get; set; }
    public int GraduationYear {  get; set; }
    public string Major { get; set; }
    public string Description { get; set; }

    public string ProfileId { get; set; }
    public virtual Profile Profile { get; set; }
}

