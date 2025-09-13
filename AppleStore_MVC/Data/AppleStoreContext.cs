using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using AppleStore_MVC.Models;

namespace AppleStore_MVC.Data;

public partial class AppleStoreContext : DbContext
{
    public AppleStoreContext()
    {
    }

    public AppleStoreContext(DbContextOptions<AppleStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public DbSet<AppleStore_MVC.Data.User> User { get; set; } = default!;

    public DbSet<AppleStore_MVC.Data.Cart> Cart { get; set; }

    public DbSet<AppleStore_MVC.Data.CartItem> CartItem { get; set; }



    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=AppleStore;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(500);
            entity.Property(e => e.Icon)
                .HasMaxLength(500)
                .HasColumnName("icon");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImageLink)
                .HasMaxLength(500)
                .HasColumnName("imageLink");
            entity.Property(e => e.ProductName).HasMaxLength(500);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Product_Category");

            //
            //entity.HasMany(c => c.CartItem)
            //.WithOne(cd => cd.Product)
            //.HasForeignKey(cd => cd.id)
            //.HasConstraintName("FK_CartItem_Cart");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.IsAdmin).HasColumnName("isAdmin");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsFixedLength();
            entity.Property(e => e.UserName).HasMaxLength(100);
        });

        modelBuilder.Entity<Cart>()
        .HasKey(c => c.id);

        //modelBuilder.Entity<CartItem>(entity =>
        //{
        //    entity.ToTable("CartItem");

        //    entity.HasKey(e => e.CartItemId); // PK riêng cho CartItem

        //    entity.Property(e => e.id).HasColumnName("id");
        //    entity.Property(e => e.pro_id).HasColumnName("pro_id");

        //    entity.HasOne(d => d.Cart)
        //        .WithMany(p => p.CartItems)
        //        .HasForeignKey(d => d.id)
        //        .OnDelete(DeleteBehavior.Cascade)
        //        .HasConstraintName("FK_CartItem_Cart");

        //    entity.HasOne(d => d.Product)
        //        .WithMany()
        //        .HasForeignKey(d => d.pro_id)
        //        .OnDelete(DeleteBehavior.Cascade)
        //        .HasConstraintName("FK_CartItem_Product");
        //});

        //modelBuilder.Entity<Cart>(entity =>
        //{
        //    entity.Property(e => e.id).HasColumnName("id");
        //    entity.Property(e => e.u_id).HasColumnName("u_id");
        //    entity.Property(e => e.buy_date).HasColumnName("buy_date");
        //    entity.HasMany(c => c.CartItem)
        //    .WithOne(cd => cd.Cart)
        //    .HasForeignKey(cd => cd.id)
        //    .HasConstraintName("FK_CartItem_Cart");
        //});

        //modelBuilder.Entity<CartItem>(entity =>
        //{
        //    entity.Property(e => e.id).HasColumnName("id");
        //    entity.Property(e => e.quantity).HasColumnName("quantity");
        //    entity.Property(e => e.unitPrice).HasColumnName("unitPrice");
        //    entity.Property(e => e.pro_id).HasColumnName("pro_id");

        //    //entity.HasOne(d => d.CartItem).WithMany(p => p.CartItem)
        //    //    .HasForeignKey(d => d.CategoryId)
        //    //    .OnDelete(DeleteBehavior.ClientSetNull)
        //    //    .HasConstraintName("FK_Product_Category");
        //});

        OnModelCreatingPartial(modelBuilder);      
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

