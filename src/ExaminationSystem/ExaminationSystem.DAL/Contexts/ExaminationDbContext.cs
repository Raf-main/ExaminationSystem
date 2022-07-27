using ExaminationSystem.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ExaminationSystem.DAL.Contexts;

public class ExaminationDbContext : IdentityDbContext<ApplicationUser>
{
    public ExaminationDbContext()
    {
    }

    public ExaminationDbContext(DbContextOptions<ExaminationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<RefreshTokenDbEntry> RefreshTokens { get; set; } = null!;
    public virtual DbSet<RoomDbEntry> Rooms { get; set; } = null!;
    public virtual DbSet<RoomUserDbEntry> RoomUsers { get; set; } = null!;
    public virtual DbSet<ExamDbEntry> Exams { get; set; } = null!;
    public virtual DbSet<MultipleQuestionDbEntry> MultipleQuestions { get; set; } = null!;
    public virtual DbSet<MultipleQuestionOptionDbEntry> MultipleQuestionOptions { get; set; } = null!;
    public virtual DbSet<MultipleQuestionAnswerDbEntry> MultipleQuestionAnswers { get; set; } = null!;
    public virtual DbSet<MultipleQuestionOptionAnswerDbEntry> MultipleQuestionOptionAnswers { get; set; } = null!;
    public virtual DbSet<OpenQuestionDbEntry> OpenQuestions { get; set; } = null!;
    public virtual DbSet<OpenQuestionOptionDbEntry> OpenQuestionOptions { get; set; } = null!;
    public virtual DbSet<MultipleQuestionAnswerDbEntry> OpenQuestionAnswers { get; set; } = null!;
    public virtual DbSet<MessageDbEntry> Messages { get; set; } = null!;
    public virtual DbSet<RoomMessageDbEntry> RoomMessages { get; set; } = null!;
    public virtual DbSet<RoomInviteDbEntry> RoomInvites { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<RoomUserDbEntry>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoomId });

            entity.HasOne(p => p.User)
                .WithMany(u => u.Rooms)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(p => p.Room)
                .WithMany(u => u.Members)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.Property(p => p.Permission)
                .HasConversion<int>();
        });

        modelBuilder.Entity<RoomMessageDbEntry>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoomId, e.MessageId });

            entity.HasOne(p => p.Room)
                .WithMany(u => u.UserMessages)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(p => p.User)
                .WithMany(u => u.RoomMessages)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<MultipleQuestionOptionAnswerDbEntry>(entity =>
        {
            entity.HasOne(p => p.Option)
                .WithMany(u => u.Answers)
                .OnDelete(DeleteBehavior.ClientSetNull);

            entity.HasOne(p => p.Answer)
                .WithMany(u => u.ChosenAnswers)
                .OnDelete(DeleteBehavior.ClientSetNull);
        });

        modelBuilder.Entity<ExamDbEntry>(entity =>
        {
            entity.Property(p => p.Status)
                .HasConversion<int>();
        });

        modelBuilder.Entity<ExamResultDbEntry>(entity =>
            {
                entity.HasOne(p => p.Exam)
                    .WithMany(r => r.ExamResults)
                    .OnDelete(DeleteBehavior.ClientCascade);
            }
        );

        base.OnModelCreating(modelBuilder);
    }
}