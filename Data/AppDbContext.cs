using Microsoft.EntityFrameworkCore;
using ToDoWebAPI.Models;

namespace ToDoWebAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<TaskItem>  TaskItems { get; set; }
    public DbSet<Label> Labels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Label 和 TaskItem 一对多关系
        modelBuilder.Entity<Label>()
            .HasMany(l => l.TaskItems)       // 一个 Label 有多个 TaskItem
            .WithOne(t => t.Label)           // 一个 TaskItem 属于一个 Label
            .HasForeignKey(t => t.LabelId)   // 外键是 LabelId
            .OnDelete(DeleteBehavior.SetNull); // 删除 Label 时，TaskItem 的 LabelId 设为 null
    }
}