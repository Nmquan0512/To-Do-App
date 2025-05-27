using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;

namespace ToDoApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<TodoItem> TodoItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(@"Server=XCBA2\SQLEXPRESS;Database=DemoToDo;Trusted_Connection=True;TrustServerCertificate=True;");
    }
}
