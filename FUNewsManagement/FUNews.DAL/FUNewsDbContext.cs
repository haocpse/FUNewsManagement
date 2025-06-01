using FUNews.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNews.DAL
{
    public class FUNewsDbContext : DbContext
    {
        public FUNewsDbContext(DbContextOptions<FUNewsDbContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<SystemAccount> SystemAccounts { get; set; }

        public DbSet<NewsArticle> NewsArticles { get; set; }

        public DbSet<Tag> Tags { get; set; }
        public DbSet<NewsTag> NewsTags { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");

                entity.HasKey(e => e.CategoryId);

                entity.Property(e => e.CategoryId)
                      .HasColumnName("CategoryId")
                      .HasColumnType("smallint");

                entity.Property(e => e.CategoryName)
                      .HasMaxLength(100)
                      .IsRequired();

                entity.Property(e => e.CategoryDescription)
                      .HasMaxLength(250)
                      .IsRequired();

                entity.Property(e => e.ParentCategoryId)
                      .HasColumnType("smallint");

                entity.Property(e => e.IsActive)
                      .HasColumnType("bit");
            });

            modelBuilder.Entity<SystemAccount>(entity =>
            {
                entity.ToTable("SystemAccount");

                entity.HasKey(e => e.AccountId);

                entity.Property(e => e.AccountId)
                      .HasColumnType("smallint");

                entity.Property(e => e.AccountName)
                      .HasMaxLength(100);

                entity.Property(e => e.AccountEmail)
                      .HasMaxLength(70);

                entity.Property(e => e.AccountRole)
                      .HasColumnType("int");

                entity.Property(e => e.AccountPassword)
                      .HasMaxLength(70);

                // Quan hệ 1-n với NewsArticle (CreatedBy)
                entity.HasMany(e => e.CreatedNewsArticles)
                      .WithOne(a => a.CreatedBy)
                      .HasForeignKey(a => a.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);

                // Quan hệ 1-n với NewsArticle (UpdatedBy)
                entity.HasMany(e => e.UpdatedNewsArticles)
                      .WithOne(a => a.UpdatedBy)
                      .HasForeignKey(a => a.UpdatedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<NewsArticle>(entity =>
            {
                entity.ToTable("NewsArticle");

                entity.HasKey(e => e.NewsArticleId);

                entity.Property(e => e.NewsArticleId)
                      .HasMaxLength(20)
                      .IsRequired();

                entity.Property(e => e.NewsTitle)
                      .HasMaxLength(400);

                entity.Property(e => e.Headline)
                      .HasMaxLength(150)
                      .IsRequired();

                entity.Property(e => e.NewsContent)
                      .HasMaxLength(4000);

                entity.Property(e => e.NewsSource)
                      .HasMaxLength(400);

                entity.HasOne(e => e.Category)
                      .WithMany()
                      .HasForeignKey(e => e.CategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.CreatedBy)
                      .WithMany()
                      .HasForeignKey(e => e.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UpdatedBy)
                      .WithMany()
                      .HasForeignKey(e => e.UpdatedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<NewsTag>(entity =>
            {
                entity.ToTable("NewsTag");

                entity.HasKey(e => new { e.NewsArticleId, e.TagId }); // Composite key

                entity.HasOne(e => e.NewsArticle)
                      .WithMany(a => a.NewsTags)
                      .HasForeignKey(e => e.NewsArticleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.Tag)
                      .WithMany(t => t.NewsTags)
                      .HasForeignKey(e => e.TagId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
