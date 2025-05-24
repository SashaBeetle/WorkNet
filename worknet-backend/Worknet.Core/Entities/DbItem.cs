namespace Worknet.Core.Entities;
public class DbItem
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}