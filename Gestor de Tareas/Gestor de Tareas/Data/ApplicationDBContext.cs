using Microsoft.EntityFrameworkCore;
using Gestor_de_Tareas.Models;

namespace Gestor_de_Tareas.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                object value = entity.ToTable("users");
                entity.HasKey(e => e.id);

                entity.Property(e => e.name).HasColumnName("name");
                entity.Property(e => e.lastName).HasColumnName("lastName");
                entity.Property(e => e.email).HasColumnName("email");
                entity.Property(e => e.password).HasColumnName("password");
            });
        }
    }
}
