using Healthcare.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Healthcare.Core.DB
{

    public partial class HealthcareDbContext : DbContext
    {
        public HealthcareDbContext()
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public HealthcareDbContext(DbContextOptions<HealthcareDbContext> options)
            : base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }

        public virtual DbSet<Appointment> Appointment { get; set; }

        public virtual DbSet<Patient> Patient { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>(entity =>
            {

                entity.Property(e => e.AppointmentDate).HasColumnType("datetime");
                entity.Property(e => e.RemindingDate).HasColumnType("datetime");

                entity.HasOne(d => d.Patient).WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.PatientId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Appointment_Patient");

                entity.HasOne(d => d.Location).WithMany(p => p.Appointment)
                    .HasForeignKey(d => d.LocationId)
                     .OnDelete(DeleteBehavior.Cascade)
                     .HasConstraintName("FK_Appointment_Location");
            });

            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.PatientId).HasDefaultValueSql("(newid())");
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
                entity.Property(e => e.Surname)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.LocationId).ValueGeneratedOnAdd();
                entity.Property(e => e.LocationName)
                    .HasMaxLength(5250)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
