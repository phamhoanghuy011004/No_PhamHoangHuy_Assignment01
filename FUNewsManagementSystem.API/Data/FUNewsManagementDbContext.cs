using FUNewsManagementSystem.API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.API.Data
{
    public class FUNewsManagementDbContext : DbContext
    {
        public FUNewsManagementDbContext(DbContextOptions<FUNewsManagementDbContext> options)
            : base(options)
        {
        }

        public DbSet<SystemAccount> SystemAccounts { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;
        public DbSet<NewsArticle> NewsArticles { get; set; } = null!;
        public DbSet<Tag> Tags { get; set; } = null!;
        public DbSet<NewsTag> NewsTags { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ========== SystemAccount ==========
            modelBuilder.Entity<SystemAccount>(entity =>
            {
                entity.ToTable("SystemAccount");
                entity.HasKey(e => e.AccountID);
                entity.Property(e => e.AccountID).ValueGeneratedNever();
                entity.Property(e => e.AccountName).HasMaxLength(100);
                entity.Property(e => e.AccountEmail).HasMaxLength(70);
                entity.Property(e => e.AccountPassword).HasMaxLength(70);
            });

            // ========== Category ==========
            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
                entity.HasKey(e => e.CategoryID);
                entity.Property(e => e.CategoryID).ValueGeneratedOnAdd();
                entity.Property(e => e.CategoryName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.CategoryDesciption).IsRequired().HasMaxLength(250);

                // Self-reference
                entity.HasOne(e => e.ParentCategory)
                      .WithMany(e => e.SubCategories)
                      .HasForeignKey(e => e.ParentCategoryID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // ========== Tag ==========
            modelBuilder.Entity<Tag>(entity =>
            {
                entity.ToTable("Tag");
                entity.HasKey(e => e.TagID);
                entity.Property(e => e.TagID).ValueGeneratedNever();
                entity.Property(e => e.TagName).HasMaxLength(50);
                entity.Property(e => e.Note).HasMaxLength(400);
            });

            // ========== NewsArticle ==========
            modelBuilder.Entity<NewsArticle>(entity =>
            {
                entity.ToTable("NewsArticle");
                entity.HasKey(e => e.NewsArticleID);
                entity.Property(e => e.NewsArticleID).HasMaxLength(20);
                entity.Property(e => e.NewsTitle).HasMaxLength(400);
                entity.Property(e => e.Headline).IsRequired().HasMaxLength(150);
                entity.Property(e => e.NewsContent).HasMaxLength(4000);
                entity.Property(e => e.NewsSource).HasMaxLength(400);

                // FK to Category
                entity.HasOne(e => e.Category)
                      .WithMany(c => c.NewsArticles)
                      .HasForeignKey(e => e.CategoryID)
                      .OnDelete(DeleteBehavior.Cascade);

                // FK to SystemAccount (CreatedBy)
                entity.HasOne(e => e.CreatedBy)
                      .WithMany(a => a.CreatedNewsArticles)
                      .HasForeignKey(e => e.CreatedByID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ========== NewsTag (Composite Key) ==========
            modelBuilder.Entity<NewsTag>(entity =>
            {
                entity.ToTable("NewsTag");
                entity.HasKey(e => new { e.NewsArticleID, e.TagID });

                entity.Property(e => e.NewsArticleID).HasMaxLength(20);

                entity.HasOne(e => e.NewsArticle)
                      .WithMany(n => n.NewsTags)
                      .HasForeignKey(e => e.NewsArticleID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Tag)
                      .WithMany(t => t.NewsTags)
                      .HasForeignKey(e => e.TagID)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}