using Laba_ISTP_1.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Laba_ISTP_1.Context;

public class ProjectDbContext : IdentityDbContext<User>
{
    public ProjectDbContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Specialities>()
             .HasMany(s => s.Students)
             .WithOne(s => s.Speciality)
             .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Students>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
    public DbSet<Departments> Departments { get; set; }
    public DbSet<Groups> Groups { get; set; }
    public DbSet<Lessons> Lessons { get; set; }
    public DbSet<Specialities> Specialities { get; set; }
    public DbSet<Students> Students { get; set; }
    public DbSet<Teachers> Teachers { get; set; }
    public DbSet<Timetables> Timetables { get; set; }
}
