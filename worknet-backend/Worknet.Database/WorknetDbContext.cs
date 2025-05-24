using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Worknet.Core.Entities;

namespace Worknet.DAL;
public class WorknetDbContext : IdentityDbContext<User>
{
    public WorknetDbContext(DbContextOptions options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}