using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DevAssessementWebsite.Models
{
    public partial class DevAssessmentContext : DbContext
    {
        public DevAssessmentContext()
        {
        }

        public DevAssessmentContext(DbContextOptions<DevAssessmentContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserInformation> UserInformations { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<UserInformation>(entity =>
            {
                entity.HasKey(e => e.PersonId);

                entity.ToTable("UserInformation");

                entity.HasOne(d => d.Person)
                    .WithOne(p => p.UserInformation)
                    .HasForeignKey<UserInformation>(d => d.PersonId);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
