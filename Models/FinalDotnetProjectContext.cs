using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MyFinalProject.Models;

public partial class FinalDotnetProjectContext : DbContext
{
    public FinalDotnetProjectContext()
    {
    }

    public FinalDotnetProjectContext(DbContextOptions<FinalDotnetProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Adv> Advs { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryProduct> CategoryProducts { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Permision> Permisions { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Rating> Ratings { get; set; }

    public virtual DbSet<Slide> Slides { get; set; }

    public virtual DbSet<Tag> Tags { get; set; }

    public virtual DbSet<TagProduct> TagProducts { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserPermision> UserPermisions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(" Data Source=PHC\\PHC_1304;Initial Catalog=Final_Dotnet_Project;User ID=sa;Password=Phc1304.,..;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Adv>(entity =>
        {
            entity.HasKey(e => e.IdAdv);

            entity.ToTable("Adv");

            entity.Property(e => e.IdAdv).HasColumnName("id_adv");
            entity.Property(e => e.LinkAdv).HasColumnName("link_adv");
            entity.Property(e => e.NameAdv)
                .HasMaxLength(500)
                .HasColumnName("name_adv");
            entity.Property(e => e.PhotoAdv)
                .HasMaxLength(500)
                .HasColumnName("photo_adv");
            entity.Property(e => e.PositionAdv).HasColumnName("position_adv");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.IdCategory);

            entity.ToTable("Category");

            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.DisplayHomePage).HasColumnName("display_home_page");
            entity.Property(e => e.IconCategory)
                .HasMaxLength(500)
                .HasColumnName("icon_category");
            entity.Property(e => e.IdParent).HasColumnName("id_parent");
            entity.Property(e => e.NameCategory)
                .HasMaxLength(500)
                .HasColumnName("name_category");
        });

        modelBuilder.Entity<CategoryProduct>(entity =>
        {
            entity.ToTable("CategoryProduct");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdCategory).HasColumnName("id_category");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");

            entity.HasOne(d => d.IdCategoryNavigation).WithMany(p => p.CategoryProducts)
                .HasForeignKey(d => d.IdCategory)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CategoryProduct_Category");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.CategoryProducts)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_CategoryProduct_Product");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.IdCustomer);

            entity.ToTable("Customer");

            entity.Property(e => e.IdCustomer).HasColumnName("id_customer");
            entity.Property(e => e.AddressCustomer).HasColumnName("address_customer");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("date_created");
            entity.Property(e => e.EmailCustomer).HasColumnName("email_customer");
            entity.Property(e => e.NameCustomer).HasColumnName("name_customer");
            entity.Property(e => e.PasswordCustomer).HasColumnName("password_customer");
            entity.Property(e => e.PhoneCustomer).HasColumnName("phone_customer");
            entity.Property(e => e.StatusUser).HasColumnName("status_user");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.IdNews).HasName("PK_News_1");

            entity.Property(e => e.IdNews).HasColumnName("id_news");
            entity.Property(e => e.ContentNews)
                .HasColumnType("ntext")
                .HasColumnName("content_news");
            entity.Property(e => e.DescriptionNews)
                .HasColumnType("ntext")
                .HasColumnName("description_news");
            entity.Property(e => e.HotNews).HasColumnName("hot_news");
            entity.Property(e => e.NameNews)
                .HasMaxLength(500)
                .HasColumnName("name_news");
            entity.Property(e => e.PhotoNews)
                .HasMaxLength(500)
                .HasColumnName("photo_news");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder);

            entity.ToTable("Order");

            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.DateCreated)
                .HasColumnType("datetime")
                .HasColumnName("date_created");
            entity.Property(e => e.IdCustomer).HasColumnName("id_customer");
            entity.Property(e => e.PriceOrder).HasColumnName("price_order");
            entity.Property(e => e.StatusOrder)
                .HasDefaultValueSql("((0))")
                .HasColumnName("status_order");

            entity.HasOne(d => d.IdCustomerNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdCustomer)
                .HasConstraintName("FK_Order_Customer");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.ToTable("OrderDetail");

            entity.Property(e => e.IdOrder).HasColumnName("id_order");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.IdOrder)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_OrderDetail_Product");
        });

        modelBuilder.Entity<Permision>(entity =>
        {
            entity.HasKey(e => e.IdPer);

            entity.ToTable("Permision");

            entity.Property(e => e.IdPer)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("id_per");
            entity.Property(e => e.GroupPer)
                .HasMaxLength(500)
                .HasColumnName("group_per");
            entity.Property(e => e.NamePer)
                .HasMaxLength(500)
                .HasColumnName("name_per");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct);

            entity.ToTable("Product");

            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.ContentProduct)
                .HasColumnType("ntext")
                .HasColumnName("content_product");
            entity.Property(e => e.DescriptionProduct)
                .HasMaxLength(4000)
                .HasColumnName("description_product");
            entity.Property(e => e.DiscountProduct)
                .HasDefaultValueSql("((0))")
                .HasColumnName("discount_product");
            entity.Property(e => e.HotProduct)
                .HasDefaultValueSql("((0))")
                .HasColumnName("hot_product");
            entity.Property(e => e.NameProduct)
                .HasMaxLength(4000)
                .HasColumnName("name_product");
            entity.Property(e => e.PhotoProduct)
                .HasMaxLength(4000)
                .HasColumnName("photo_product");
            entity.Property(e => e.PriceProduct).HasColumnName("price_product");
        });

        modelBuilder.Entity<Rating>(entity =>
        {
            entity.HasKey(e => e.IdRating);

            entity.ToTable("Rating");

            entity.Property(e => e.IdRating).HasColumnName("id_rating");
            entity.Property(e => e.IdProduct).HasColumnName("id_product");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.Ratings)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_Rating_Product1");
        });

        modelBuilder.Entity<Slide>(entity =>
        {
            entity.HasKey(e => e.IdSlide);

            entity.ToTable("Slide");

            entity.Property(e => e.IdSlide).HasColumnName("id_slide");
            entity.Property(e => e.InfoSlide)
                .HasMaxLength(500)
                .HasColumnName("info_slide");
            entity.Property(e => e.Link).HasMaxLength(500);
            entity.Property(e => e.NameSlide)
                .HasMaxLength(500)
                .HasColumnName("name_slide");
            entity.Property(e => e.PhotoSlide)
                .HasMaxLength(500)
                .HasColumnName("photo_slide");
            entity.Property(e => e.SubTitleSlide)
                .HasMaxLength(500)
                .HasColumnName("subTitle_slide");
            entity.Property(e => e.TitleSlide)
                .HasMaxLength(500)
                .HasColumnName("title_slide");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.HasKey(e => e.IdTag);

            entity.ToTable("Tag");

            entity.Property(e => e.IdTag).HasColumnName("id_tag");
            entity.Property(e => e.NameTag)
                .HasMaxLength(500)
                .HasColumnName("name_tag");
            entity.Property(e => e.TypeTag)
                .HasMaxLength(50)
                .HasColumnName("type_tag");
        });

        modelBuilder.Entity<TagProduct>(entity =>
        {
            entity.ToTable("TagProduct");

            entity.Property(e => e.IdProduct).HasColumnName("id_product");
            entity.Property(e => e.IdTag).HasColumnName("id_tag");

            entity.HasOne(d => d.IdProductNavigation).WithMany(p => p.TagProducts)
                .HasForeignKey(d => d.IdProduct)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TagProduct_Product");

            entity.HasOne(d => d.IdTagNavigation).WithMany(p => p.TagProducts)
                .HasForeignKey(d => d.IdTag)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_TagProduct_Tag");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.DateCreated)
                .HasColumnType("date")
                .HasColumnName("date_created");
            entity.Property(e => e.NameAccount)
                .HasMaxLength(200)
                .HasColumnName("name_account");
            entity.Property(e => e.NameUser)
                .HasMaxLength(200)
                .HasColumnName("name_user");
            entity.Property(e => e.PasswordUser)
                .HasMaxLength(200)
                .HasColumnName("password_user");
            entity.Property(e => e.StatusUser).HasColumnName("status_user");
        });

        modelBuilder.Entity<UserPermision>(entity =>
        {
            entity.HasKey(e => new { e.IdUser, e.IdPer });

            entity.ToTable("User_Permision");

            entity.Property(e => e.IdUser).HasColumnName("id_user");
            entity.Property(e => e.IdPer)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("id_per");
            entity.Property(e => e.Note)
                .HasMaxLength(200)
                .HasColumnName("note");

            entity.HasOne(d => d.IdPerNavigation).WithMany(p => p.UserPermisions)
                .HasForeignKey(d => d.IdPer)
                .HasConstraintName("FK_User_Permision_Permision");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.UserPermisions)
                .HasForeignKey(d => d.IdUser)
                .HasConstraintName("FK_User_Permision_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
