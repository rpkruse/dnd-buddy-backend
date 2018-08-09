using Microsoft.EntityFrameworkCore;
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
        public virtual DbSet<Item> Item { get; set; }
        public virtual DbSet<Monster> Monster { get; set; }
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

                entity.Property(e => e.Class)
                    .HasColumnName("class")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Race)
                    .HasColumnName("race")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Abil_Score_Str)
                    .HasColumnName("abil_score_str")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Abil_Score_Dex)
                    .HasColumnName("abil_score_dex")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Abil_Score_Con)
                    .HasColumnName("abil_score_con")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Abil_Score_Int)
                    .HasColumnName("abil_score_int")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Abil_Score_Wis)
                    .HasColumnName("abil_score_wis")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Abil_Score_Cha)
                    .HasColumnName("abil_score_cha")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Max_HP)
                    .HasColumnName("max_hp")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HP)
                    .HasColumnName("hp")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Profs)
                    .HasColumnName("profs")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Level)
                    .HasColumnName("level")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Armor)
                    .HasColumnName("armor")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Weapon)
                    .HasColumnName("weapon")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Shield)
                    .HasColumnName("shield")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Neck)
                    .HasColumnName("neck")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Ring_1)
                    .HasColumnName("ring_1")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Ring_2)
                    .HasColumnName("ring_2")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Xp)
                     .HasColumnName("xp")
                     .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GameId)
                    .HasColumnName("gameId")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Game)
                    .WithMany(g => g.Character)
                    .HasForeignKey(d => d.GameId)
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

                entity.Property(e => e.GameState)
                    .HasColumnName("gameState")
                    .HasColumnType("varchar(2000)");

                entity.HasOne(d => d.User)
                    .WithMany(g => g.Game)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("f_uid");
            });
            modelBuilder.Entity<Item>(entity =>
            {
                entity.ToTable("item");

                entity.HasIndex(e => e.ItemId)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.ItemId)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .HasColumnType("varchar(45)");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasColumnType("varchar(100)");

                entity.Property(e => e.Count)
                    .HasColumnName("count")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CanEquip)
                    .HasColumnName("canEquip")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.CharacterId)
                    .HasColumnName("characterId")
                    .HasColumnType("int(11)");

                entity.HasOne(e => e.Character)
                    .WithMany(i => i.Item)
                    .HasConstraintName("c_uid");
            });
            modelBuilder.Entity<Monster>(entity =>
            {
                entity.ToTable("monster");

                entity.HasIndex(e => e.MonsterId)
                    .HasName("id")
                    .IsUnique();

                entity.Property(e => e.MonsterId)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Max_HP)
                    .HasColumnName("max_hp")
                    .HasColumnType("int(11)");

                entity.Property(e => e.HP)
                    .HasColumnName("hp")
                    .HasColumnType("int(11)");

                entity.Property(e => e.GameId)
                    .HasColumnName("gameId")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Url)
                    .HasColumnName("url")
                    .HasColumnType("varchar(100)");

                entity.HasOne(d => d.Game)
                    .WithMany(g => g.Monster)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("f_mtgid");
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
