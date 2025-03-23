using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1.Models;

public partial class DrivingSkillCertContext : DbContext
{
    public DrivingSkillCertContext()
    {
    }

    public DrivingSkillCertContext(DbContextOptions<DrivingSkillCertContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Answer> Answers { get; set; }

    public virtual DbSet<BankQuestion> BankQuestions { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Exam> Exams { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Question> Questions { get; set; }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<Result> Results { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LilyStoner;uid=sa;password=123456;database=DrivingSkillCert;Encrypt=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PK__Answers__D4825024325D3A3D");

            entity.Property(e => e.AnswerId).HasColumnName("AnswerID");
            entity.Property(e => e.Answer1).HasColumnName("Answer");
            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

            entity.HasOne(d => d.Question).WithMany(p => p.Answers)
                .HasForeignKey(d => d.QuestionId)
                .HasConstraintName("FK__Answers__Questio__45F365D3");
        });

        modelBuilder.Entity<BankQuestion>(entity =>
        {
            entity.HasKey(e => e.BankId).HasName("PK__BankQues__AA08CB3339B43527");

            entity.Property(e => e.BankId).HasColumnName("BankID");
            entity.Property(e => e.BankName).HasMaxLength(100);
            entity.Property(e => e.CourseId).HasColumnName("CourseID");

            entity.HasOne(d => d.Course).WithMany(p => p.BankQuestions)
                .HasForeignKey(d => d.CourseId)
                .HasConstraintName("FK__BankQuest__Cours__403A8C7D");
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.CertificateId).HasName("PK__Certific__BBF8A7E1A82D4877");

            entity.HasIndex(e => e.CertificateCode, "UQ__Certific__9B85583084D76606").IsUnique();

            entity.Property(e => e.CertificateId).HasColumnName("CertificateID");
            entity.Property(e => e.CertificateCode).HasMaxLength(50);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Certifica__UserI__59063A47");
        });

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseId).HasName("PK__Courses__C92D7187C8A197BC");

            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.CourseName).HasMaxLength(100);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.TeacherId).HasColumnName("TeacherID");

            entity.HasOne(d => d.Teacher).WithMany(p => p.Courses)
                .HasForeignKey(d => d.TeacherId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Courses__Teacher__3C69FB99");
        });

        modelBuilder.Entity<Exam>(entity =>
        {
            entity.HasKey(e => e.ExamId).HasName("PK__Exams__297521A7C8FE0ED5");

            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Room).HasMaxLength(50);

            entity.HasOne(d => d.Course).WithMany(p => p.Exams)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Exams__CourseID__4F7CD00D");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.NotificationId).HasName("PK__Notifica__20CF2E326FCF6713");

            entity.Property(e => e.NotificationId).HasColumnName("NotificationID");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.IsRead).HasDefaultValue(false);
            entity.Property(e => e.SentDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__5EBF139D");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.QuestionId).HasName("PK__Question__0DC06F8CC2DD0524");

            entity.Property(e => e.QuestionId).HasColumnName("QuestionID");
            entity.Property(e => e.BankId).HasColumnName("BankID");
            entity.Property(e => e.Question1).HasColumnName("Question");

            entity.HasOne(d => d.Bank).WithMany(p => p.Questions)
                .HasForeignKey(d => d.BankId)
                .HasConstraintName("FK__Questions__BankI__4316F928");
        });

        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.RegistrationId).HasName("PK__Registra__6EF588309FC9D9E2");

            entity.Property(e => e.RegistrationId).HasColumnName("RegistrationID");
            entity.Property(e => e.CourseId).HasColumnName("CourseID");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .HasDefaultValue("Pending");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Course).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.CourseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__Cours__4BAC3F29");

            entity.HasOne(d => d.User).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Registrat__UserI__4AB81AF0");
        });

        modelBuilder.Entity<Result>(entity =>
        {
            entity.HasKey(e => e.ResultId).HasName("PK__Results__976902280C8A1C19");

            entity.Property(e => e.ResultId).HasColumnName("ResultID");
            entity.Property(e => e.ExamId).HasColumnName("ExamID");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Score).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Exam).WithMany(p => p.Results)
                .HasForeignKey(d => d.ExamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Results__ExamID__534D60F1");

            entity.HasOne(d => d.User).WithMany(p => p.Results)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Results__UserID__5441852A");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CCACAC673BC3");

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105349A03FAE7").IsUnique();

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Class).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Role).HasMaxLength(20);
            entity.Property(e => e.School).HasMaxLength(100);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
