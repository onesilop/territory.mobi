using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace territory.mobi.Models
{
    public partial class TerritoryContext : DbContext
    {
        public TerritoryContext()
        {
        }

        public TerritoryContext(DbContextOptions<TerritoryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<Cong> Cong { get; set; }
        public virtual DbSet<Dncpword> Dncpword { get; set; }
        public virtual DbSet<DoNotCall> DoNotCall { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Map> Map { get; set; }
        public virtual DbSet<Section> Section { get; set; }
        public virtual DbSet<Token> Token { get; set; }
        public virtual DbSet<Setting> Setting { get; set; }
        public virtual DbSet<MapAssignment> MapAssignment { get; set; }
        public virtual DbSet<MapFeature> MapFeature { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string projectPath = AppDomain.CurrentDomain.BaseDirectory.Split(new String[] { @"bin\" }, StringSplitOptions.None)[0];
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(projectPath)
                    .AddJsonFile("appsettings.json")
                    .Build();

                optionsBuilder.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:DefaultSchema", "app");

            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.ToTable("AspNetRoleClaims", "dbo");

                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.ToTable("AspNetRoles", "dbo");

                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.ToTable("AspNetUserClaims", "dbo");

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.ToTable("AspNetUserLogins", "dbo");

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.ToTable("AspNetUserRoles", "dbo");

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.ToTable("AspNetUsers", "dbo");

                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique();

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.ToTable("AspNetUserTokens", "dbo");

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Cong>(entity =>
            {
                entity.Property(e => e.CongId)
                    .HasColumnName("CongID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CongName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
                
                entity.Property(e => e.ServId)
                    .HasColumnName("ServID")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDatetime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Dncpword>(entity =>
            {
                entity.HasKey(e => e.PwdId);

                entity.ToTable("dncpword");

                entity.Property(e => e.PwdId)
                    .HasColumnName("pwdID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CongId).HasColumnName("congID");

                entity.Property(e => e.Notinuse).HasColumnName("notinuse");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasColumnName("passwordHash")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<DoNotCall>(entity =>
            {
                entity.HasKey(e => e.DncId)
                    .HasName("PK_doNotCall");

                entity.Property(e => e.DncId)
                    .HasColumnName("dncID")
                    .ValueGeneratedNever();

                entity.Property(e => e.AptNo)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated)
                    .HasColumnName("dateCreated")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateValidated)
                    .HasColumnName("dateValidated")
                    .HasColumnType("datetime");

                entity.Property(e => e.Display).HasColumnName("display");

                entity.Property(e => e.MapId).HasColumnName("mapID");

                entity.Property(e => e.Note).IsUnicode(false);

                entity.Property(e => e.StreetName)
                    .IsRequired()
                    .HasColumnName("streetName")
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.StreetNo)
                    .IsRequired()
                    .HasColumnName("streetNo")
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Suburb)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDatetime)
                    .HasColumnName("updateDatetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.Geocode)
                    .HasColumnName("geocode")
                    .IsUnicode(false);

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.DoNotCall)
                    .HasForeignKey(d => d.MapId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_DoNotCall_Map");

            });

            modelBuilder.Entity<Images>(entity =>
            {
                entity.HasKey(e => e.ImgId);

                entity.ToTable("images");

                entity.Property(e => e.ImgId)
                    .HasColumnName("imgID")
                    .ValueGeneratedNever();

                entity.Property(e => e.ImgImage)
                    .HasColumnName("imgImage")
                    .HasColumnType("image");

                entity.Property(e => e.ImgPath)
                    .HasColumnName("imgPath")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.ImgText)
                    .IsRequired()
                    .HasColumnName("imgText")
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.MapId).HasColumnName("mapID");

                entity.Property(e => e.Updatedatetime)
                    .HasColumnName("updatedatetime")
                    .HasColumnType("datetime");

                entity.HasOne(d => d.Map)
                    .WithMany(p => p.Images)
                    .HasForeignKey(d => d.MapId)
                    .HasConstraintName("FK_images_Map");
            });

            modelBuilder.Entity<Map>(entity =>
            {
                entity.Property(e => e.MapId)
                    .HasColumnName("MapID")
                    .ValueGeneratedNever();

                entity.Property(e => e.CongId).HasColumnName("CongID");

                entity.Property(e => e.GoogleRef).IsUnicode(false);

                entity.Property(e => e.ImgId).HasColumnName("imgID");

                entity.Property(e => e.MapArea)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.MapDesc)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MapKey)
                    .HasMaxLength(5)
                    .IsUnicode(false);

                entity.Property(e => e.MapPolygon).IsUnicode(false);

                entity.Property(e => e.MapType)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Notes).IsUnicode(false);

                entity.Property(e => e.Parking).IsUnicode(false);

                entity.Property(e => e.SectionId).HasColumnName("SectionID");

                entity.Property(e => e.UpdateDatetime).HasColumnType("datetime");
            });

            modelBuilder.Entity<Section>(entity =>
            {
                entity.Property(e => e.SectionId).ValueGeneratedNever();

                entity.Property(e => e.SectionTitle)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CongId).HasColumnName("CongID");

                entity.Property(e => e.SortOrder).HasColumnName("SortOrder");

            });

            modelBuilder.Entity<Token>(entity =>
            {
                entity.Property(e => e.TokenId)
                    .HasColumnName("tokenId")
                    .ValueGeneratedNever();

                entity.Property(e => e.UpdateDateTime)
                    .HasColumnName("updateDateTime")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserCong)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserEmail)
                    .HasMaxLength(250)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Setting>(entity =>
            {
                entity.Property(e => e.SettingId).ValueGeneratedNever();

                entity.Property(e => e.SettingType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.SettingValue)
                    .IsRequired()
                    .IsUnicode(false);
            });

            modelBuilder.Entity<MapAssignment>(entity =>
            {
                entity.HasKey(e => e.AssignId)
                    .HasName("PK_mapAssignment");

                entity.Property(e => e.AssignId)
                    .HasColumnName("assignId")
                    .ValueGeneratedNever();

                entity.Property(e => e.DateAssigned)
                    .HasColumnName("dateAssigned")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateReturned)
                    .HasColumnName("dateReturned")
                    .HasColumnType("datetime");

                entity.Property(e => e.MapId).HasColumnName("mapId");

                entity.Property(e => e.NonUserName)
                    .HasColumnName("nonUserName")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Updatedatetime)
                    .HasColumnName("updatedatetime")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasColumnName("userId")
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<MapFeature>(entity =>
            {
                entity.Property(e => e.MapFeatureId).ValueGeneratedNever();

                entity.Property(e => e.ZIndex)
                    .HasColumnName("zIndex")
                    .IsRequired();

                entity.Property(e => e.Zoom)
                   .HasColumnName("zoom")
                   .IsRequired();

                entity.Property(e => e.Color)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.Position)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Updatedatetime)
                    .HasColumnName("updatedatetime")
                    .HasColumnType("datetime");
            });

        }
    }
}
