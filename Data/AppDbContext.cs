using Microsoft.EntityFrameworkCore;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<TaskItem>  TaskItems { get; set; }
    public DbSet<Label> Labels { get; set; }
    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Label>()
            .HasMany(l => l.TaskItems)
            .WithOne(t => t.Label)
            .HasForeignKey(t => t.LabelId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Category>().HasData(new Category
            {
                Id = 2,
                Name = "COMP1511",
                Description = "A node.js course",
                Color = "Blue"
            },
            new Category
            {
                Id = 1,
                Name = "Default",
                Description = "None",
                Color = "None"
            }
        );
    }
}