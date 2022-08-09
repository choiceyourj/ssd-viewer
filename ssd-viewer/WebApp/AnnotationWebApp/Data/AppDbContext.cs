using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AnnotationWebApp.Models.Account;
using AnnotationWebApp.Models.TumorImage;

namespace AnnotationWebApp.Data
{
    public class AppDbContext : IdentityDbContext<SsdUser, SsdRole, string>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<EndoscopeVideo> EndoscopeVideos { get; set; }
        public DbSet<StillCutImage> StillCutImages { get; set; }
        public DbSet<TumorPosition> TumorPositions { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<EndoscopeVideo>(b =>
            {
                b.Property(i => i.Id).HasColumnType("varchar(256)").HasMaxLength(256).IsRequired();

                b.Property(i => i.DisplayName).HasColumnType("text").IsRequired();
                b.Property(i => i.Description).HasColumnType("text").IsRequired();
                b.Property(i => i.VideoFileLocation).HasColumnType("text").IsRequired();
                b.Property(i => i.TotalNumberOfFrame).HasColumnType("integer");
                b.Property(i => i.IsAllImageTreated).HasColumnType("boolean");
                b.Property(i => i.UploadTime).HasColumnType("timestamp without time zone");
                b.Property(i => i.UserId).HasColumnType("varchar(256)").HasMaxLength(256);

                b.HasKey(i => i.Id);
                b.ToTable("EndoscopeVideos");


                // Define relation
                b.HasOne(i => i.User).WithMany(j => j.EndoscopeVideos).HasForeignKey(k => k.UserId);
            });
            
            builder.Entity<StillCutImage>(b =>
            {
                b.Property(i => i.Id).HasColumnType("varchar(256)").HasMaxLength(256).IsRequired();
                b.Property(i => i.DisplayName).HasColumnType("text").IsRequired();
                b.Property(i => i.ImageFileLocation).HasColumnType("text").IsRequired();
                b.Property(i => i.LastUpdateTime).HasColumnType("timestamp without time zone");
                b.Property(i => i.ImageCreatedTime).HasColumnType("timestamp without time zone");
                b.Property(i => i.IsCropComplete).HasColumnType("boolean");

                b.Property(i => i.VideoId).HasColumnType("varchar(256)").HasMaxLength(256);
                b.Property(i => i.UserId).HasColumnType("varchar(256)").HasMaxLength(256).IsRequired(false);


                b.HasKey(i => i.Id);
                b.ToTable("StillCutImages");


                b.HasIndex(i => i.VideoId);

                // Define relation
                b.HasOne(i => i.User).WithMany(j => j.StillCutImages).HasForeignKey(k => k.UserId);
                b.HasOne(i => i.Video).WithMany(j => j.StillCutImages).HasForeignKey(k => k.VideoId);
            });

            builder.Entity<TumorPosition>(b =>
            {
                b.Property(i => i.Id).HasColumnType("varchar(256)").HasMaxLength(256).IsRequired();
                b.Property(i => i.Order).HasColumnType("integer");
                b.Property(i => i.Width).HasColumnType("integer");
                b.Property(i => i.Height).HasColumnType("integer");
                b.Property(i => i.StartX).HasColumnType("integer");
                b.Property(i => i.StartY).HasColumnType("integer");
                b.Property(i => i.EndX).HasColumnType("integer");
                b.Property(i => i.EndY).HasColumnType("integer");


                b.Property(i => i.ImageId).HasColumnType("varchar(256)").HasMaxLength(256).IsRequired();

                b.HasKey(i => i.Id);
                b.ToTable("TumorPositions");


                b.HasIndex(i => i.ImageId);

                // Define relation
                b.HasOne(i => i.Image).WithMany(j => j.TumorPositions).HasForeignKey(k => k.ImageId);
            });



            #region Begin region FluentAPI for Identity Model

            builder.Entity<SsdUser>(b =>
            {
                b.Property(i => i.UserDisplayName).HasColumnType("varchar(256)").HasMaxLength(256);

                b.ToTable("SsdUsers");

            });

            builder.Entity<SsdRole>(b =>
            {
                b.Property(i => i.RoleDescription).HasColumnType("text").IsRequired(false);
                b.Property(i => i.CreationDate).HasColumnType("timestamp without time zone");
                b.ToTable("SsdRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(b =>
            {
                b.ToTable("SsdUserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(b =>
            {
                b.ToTable("SsdUserLogins");
            });

            builder.Entity<IdentityUserToken<string>>(b =>
            {
                b.ToTable("SsdUserTokens");
            });


            builder.Entity<IdentityRoleClaim<string>>(b =>
            {
                b.ToTable("SsdRoleClaims");
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("SsdUserRoles");
            });

            #endregion End region for FluentAPI for Identity Model

        }
    }
}
