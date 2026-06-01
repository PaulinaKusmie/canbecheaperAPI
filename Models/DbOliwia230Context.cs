using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace canbecheaperAPI.Models;

public partial class DbOliwia230Context : DbContext
{
    public DbOliwia230Context()
    {
    }

    public DbOliwia230Context(DbContextOptions<DbOliwia230Context> options)
        : base(options)
    {
    }

    public virtual DbSet<CheaperPrice> CheaperPrices { get; set; }

    public virtual DbSet<CheaperProduct> CheaperProducts { get; set; }

    public virtual DbSet<CheaperProductPrice> CheaperProductPrices { get; set; }

    public virtual DbSet<CheaperType> CheaperTypes { get; set; }

    public virtual DbSet<CheaperUnit> CheaperUnits { get; set; }

    public virtual DbSet<CheaperUser> CheaperUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseMySql("name=ConnectionStrings:MySQLConnection", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.6.22-mariadb"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<CheaperPrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cheaper_price");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Price).HasColumnName("price");
        });

        modelBuilder.Entity<CheaperProduct>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cheaper_product");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
        });

        modelBuilder.Entity<CheaperProductPrice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cheaper_product_price");

            entity.HasIndex(e => e.PriceId, "priceId");

            entity.HasIndex(e => e.ProductId, "productId");

            entity.HasIndex(e => e.TypeId, "typeId");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("createdAt");
            entity.Property(e => e.PriceId)
                .HasColumnType("int(11)")
                .HasColumnName("priceId");
            entity.Property(e => e.ProductId)
                .HasColumnType("int(11)")
                .HasColumnName("productId");
            entity.Property(e => e.TypeId)
                .HasColumnType("int(11)")
                .HasColumnName("typeId");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("userId");

            entity.HasOne(d => d.Price).WithMany(p => p.CheaperProductPrices)
                .HasForeignKey(d => d.PriceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cheaper_product_price_ibfk_2");

            entity.HasOne(d => d.Product).WithMany(p => p.CheaperProductPrices)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cheaper_product_price_ibfk_1");

            entity.HasOne(d => d.Type).WithMany(p => p.CheaperProductPrices)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cheaper_product_price_ibfk_3");
        });

        modelBuilder.Entity<CheaperType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cheaper_type");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .HasColumnName("name");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("userId");
        });

        modelBuilder.Entity<CheaperUnit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cheaper_unit");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.LengthUnit)
                .HasColumnType("int(11)")
                .HasColumnName("length_unit");
            entity.Property(e => e.PieceUnit)
                .HasColumnType("int(11)")
                .HasColumnName("piece_unit");
            entity.Property(e => e.UserId)
                .HasColumnType("int(11)")
                .HasColumnName("user_id");
            entity.Property(e => e.VolumeUnit)
                .HasColumnType("int(11)")
                .HasColumnName("volume_unit");
            entity.Property(e => e.WeightUnit)
                .HasColumnType("int(11)")
                .HasColumnName("weight_unit");

            entity.HasOne(d => d.User).WithMany(p => p.CheaperUnits)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("cheaper_unit_ibfk_1");
        });

        modelBuilder.Entity<CheaperUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("cheaper_user");

            entity.HasIndex(e => e.Email, "email").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("current_timestamp()")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Email)
                .HasMaxLength(150)
                .HasColumnName("email");
            entity.Property(e => e.EmailCode)
                .HasColumnType("int(150)")
                .HasColumnName("emailCode");
            entity.Property(e => e.EmailCodeAttempts)
                .HasColumnType("tinyint(4)")
                .HasColumnName("emailCodeAttempts");
            entity.Property(e => e.EmailCodeExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("emailCodeExpiresAt");
            entity.Property(e => e.EmailConfirmed).HasColumnName("emailConfirmed");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
        });

    
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
