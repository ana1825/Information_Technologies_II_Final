using System;
using DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DAL.Context
{
    public partial class OrganizationsDirectoryDbContext : IdentityDbContext<IdentityUser>
    {
        public OrganizationsDirectoryDbContext()
        {
        }

        public OrganizationsDirectoryDbContext(DbContextOptions<OrganizationsDirectoryDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonInOrganization> PersonInOrganization { get; set; }
        public virtual DbSet<Positions> Positions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("@Server =./LAPTOP-IRG1FTCI; Database = OrgDb; Trusted_Connection = True; ");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Organization>(entity =>
            {
                entity.Property(e => e.Activities)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Address)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.Property(e => e.BirthDate).HasColumnType("date");

                entity.Property(e => e.City)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PersonalId)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Picture)
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.Sex)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PersonInOrganization>(entity =>
            {
                entity.HasOne(d => d.Organization)
                    .WithMany(p => p.PersonInOrganization)
                    .HasForeignKey(d => d.OrganizationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__PersonInO__Organ__3C69FB99");

                entity.HasOne(d => d.Person)
                    .WithMany(p => p.PersonInOrganization)
                    .HasForeignKey(d => d.PersonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__PersonInO__Perso__3D5E1FD2");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.PersonInOrganization)
                    .HasForeignKey(d => d.PositionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK__PersonInO__Posit__3E52440B");
            });

            modelBuilder.Entity<Positions>(entity =>
            {
                entity.HasKey(e => e.PositionId)
                    .HasName("PK__Position__60BB9A79F6EF5514");

                entity.Property(e => e.Position)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
            modelBuilder.Seed();
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
