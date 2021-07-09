using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace Valuta
{
    public partial class moneyContext : DbContext
    {
        public moneyContext()
        {
        }

        public moneyContext(DbContextOptions<moneyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<DateCourse> DateCourses { get; set; }
        public virtual DbSet<Valute> Valutes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost;Database=money;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Cyrillic_General_CI_AS");

            modelBuilder.Entity<DateCourse>(entity =>
            {
                entity.ToTable("dateCourse");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Value)
                    .HasColumnType("money")
                    .HasColumnName("value");

                entity.Property(e => e.ValuteId)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("valuteID");

                entity.HasOne(d => d.Valute)
                    .WithMany(p => p.DateCourses)
                    .HasPrincipalKey(p => p.Code)
                    .HasForeignKey(d => d.ValuteId)
                    .HasConstraintName("FK__dateCours__valut__2E1BDC42");
            });

            modelBuilder.Entity<Valute>(entity =>
            {
                entity.ToTable("Valute");

                entity.HasIndex(e => e.Code, "UQ__Valute__A25C5AA7BDF0F060")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Code)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
