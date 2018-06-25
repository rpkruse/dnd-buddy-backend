using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnd_buddy_backend.Models
{
    public partial class DataContext : DbContext
    {
        public virtual DbSet<User> User { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.UserId)
                    .HasName("id");

                entity.HasIndex(e => e.Username)
                    .HasName("username")
                    .IsUnique();

                entity.Property(e => e.UserId)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Username)
                    .HasColumnName("username")
                    .HasColumnType("varchar(100)")
                    .IsRequired();

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasColumnType("varchar(100)")
                    .IsRequired();
            });
        }
    }
}
