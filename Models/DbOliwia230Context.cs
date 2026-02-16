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

    public virtual DbSet<CheaperUser> CheaperUsers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=mysql.mikr.us;database=db_oliwia230;user=oliwia230;password=C239_0b2c9f", Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.6.22-mariadb"));

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
            entity.Property(e => e.Price)
                .HasMaxLength(200)
                .HasColumnName("price");
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
            entity.Property(e => e.EmailCodeAttempts)
                .HasColumnType("tinyint(4)")
                .HasColumnName("emailCodeAttempts");
            entity.Property(e => e.EmailCodeExpiresAt)
                .HasColumnType("datetime")
                .HasColumnName("emailCodeExpiresAt");
            entity.Property(e => e.EmailCode)
                .HasColumnType("int(150)")
                .HasColumnName("emailCodeHash");
            entity.Property(e => e.EmailConfirmed)
                .HasColumnType("int(11)")
                .HasColumnName("emailConfirmed");
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
