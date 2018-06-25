﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dnd_buddy_backend.Models
{
    public partial class DataContext : DbContext
    {
        public virtual DbSet<Character> Character { get; set; }
        public virtual DbSet<Game> Game { get; set; }
        public virtual DbSet<User> User { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>(entity =>
            {
                entity.ToTable("character");

                entity.HasIndex(e => e.CharacterId)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.CharacterId)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GameId)
                    .HasColumnName("gameId")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Game)
                    .WithMany(g => g.Character)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("f_gid");

                entity.HasOne(d => d.User)
                    .WithMany(g => g.Character)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Game>(entity =>
            {
                entity.ToTable("game");

                entity.HasIndex(e => e.GameId)
                    .HasName("id");

                entity.Property(e => e.GameId)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GM)
                    .HasColumnName("gm")
                    .HasColumnType("bit(1)");

                entity.HasOne(d => d.User)
                    .WithMany(g => g.Game)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("f_uid");
            });
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
