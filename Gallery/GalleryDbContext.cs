using Gallery.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Gallery
{
    public class GalleryDbContext : IdentityDbContext
    {
        private IConfiguration Configuration => new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
           .AddJsonFile("appsettings.json")
           .Build();
        //public GalleryDbContext() { }
        public GalleryDbContext(DbContextOptions<GalleryDbContext> options) : base(options) { }
        public virtual DbSet<Images> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("GalleryDbConnection"));
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Images>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id);

                entity.Property(e => e.ImageName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasColumnType("nvarchar(MAX)")
                    .IsUnicode(false);

                entity.Property(e => e.CreatedBy)
                .IsRequired()
                .HasColumnType("nvarchar(100)");

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");
            });
        }
    }
}
