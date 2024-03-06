using DotNetAdvance.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetAdvance.DatabaseContext
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options) : base(options){}

        public DbSet<ToDoItem> ToDoItems { get; set; }
        public DbSet<ToDoItemUpdated> ToDoItemsUpdated { get; set; }
    }
}
