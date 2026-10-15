using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Persistence
{
    public class ApplicationDbContext : DbContext 
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<PersonEntity> Persons { get; set; }
        public DbSet<VisitEntity> Visits { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure the PersonEntity table
            modelBuilder.Entity<PersonEntity>(entity =>
            {
                entity.ToTable("Persons");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(e => e.Code)
                    .IsUnique();

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PhoneNumber)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Ignore(e => e.FullName); // Ignore FullName property as it's computed

                entity.Property<DateTime>("CreatedDate")
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property<DateTime>("UpdatedAt")
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()");
            });

            modelBuilder.Entity<VisitEntity>(entity =>
            {
                entity.ToTable("Visits");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .IsRequired()
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.PersonId)
                    .IsRequired();

                entity.Property(e => e.EntryTime)
                    .IsRequired();

                entity.Property(e => e.ExitTime)
                    .IsRequired(false);

                entity.Ignore(e => e.IsActive);
                entity.Ignore(e => e.Duration);

                entity.HasOne(e => e.Person)
                      .WithMany()
                      .HasForeignKey(e => e.PersonId)
                      .OnDelete(DeleteBehavior.Cascade);
                    
                entity.HasIndex(e => e.PersonId);
                entity.HasIndex(e => e.EntryTime);
                entity.HasIndex(e => new { e.PersonId, e.EntryTime });

                entity.Property<DateTime>("CreatedAt")
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()");

                entity.Property<DateTime>("UpdatedAt")
                      .IsRequired()
                      .HasDefaultValueSql("GETUTCDATE()");
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateTimestamps();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void UpdateTimestamps()
        { 
            var entries = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                //if (entry.Metadata.FindProperty("UpdatedAt") != null) { 
                    entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
                //}
            }
        }
    }
}
