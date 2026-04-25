using Microsoft.EntityFrameworkCore;
using Gestor_de_Tareas.Models;
using Task = Gestor_de_Tareas.Models.Task;

namespace Gestor_de_Tareas.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(e => e.id);

                entity.Property(e => e.name).HasColumnName("name");
                entity.Property(e => e.lastName).HasColumnName("lastName");
                entity.Property(e => e.email).HasColumnName("email");
                entity.Property(e => e.password).HasColumnName("password");
            });

            modelBuilder.Entity<Task>(entity =>
            {
                entity.ToTable("tasks");
                entity.HasKey(t => t.id);
                entity.Property(t => t.Title).HasColumnName("title");
                entity.Property(t => t.Description).HasColumnName("description");
                entity.Property(t => t.Tags).HasColumnName("tags");
                entity.Property(t => t.Priority).HasColumnName("priority");
                entity.Property(t => t.userId).HasColumnName("user_id");
            });
        }
    }
}
