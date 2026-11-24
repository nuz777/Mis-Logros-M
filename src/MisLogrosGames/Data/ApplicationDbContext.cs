using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MisLogrosGames.Models;

namespace MisLogrosGames.Data;

public sealed class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Achievement> Achievements => Set<Achievement>();

    public DbSet<UserAchievement> UserAchievements => Set<UserAchievement>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>().ToTable("users");
        builder.Entity<IdentityRole<Guid>>().ToTable("roles");
        builder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
        builder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        builder.Entity<IdentityUserRole<Guid>>().ToTable("user_roles");
        builder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        builder.Entity<IdentityRoleClaim<Guid>>().ToTable("role_claims");

        builder.Entity<UserAchievement>(entity =>
        {
            entity.HasKey(userAchievement => new { userAchievement.UserId, userAchievement.AchievementId });

            entity.HasOne(userAchievement => userAchievement.User)
                .WithMany(user => user.UserAchievements)
                .HasForeignKey(userAchievement => userAchievement.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(userAchievement => userAchievement.Achievement)
                .WithMany(achievement => achievement.UserAchievements)
                .HasForeignKey(userAchievement => userAchievement.AchievementId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(userAchievement => userAchievement.UnlockedAt)
                .HasDefaultValueSql("now()");
        });

        builder.Entity<Achievement>(entity =>
        {
            entity.HasIndex(achievement => achievement.Code).IsUnique();
            entity.Property(achievement => achievement.Code).HasMaxLength(64);
            entity.Property(achievement => achievement.Name).HasMaxLength(128);
            entity.Property(achievement => achievement.Description).HasMaxLength(256);
            entity.Property(achievement => achievement.Icon).HasMaxLength(64);
            entity.HasOne(achievement => achievement.Owner)
                .WithMany(user => user.CustomAchievements)
                .HasForeignKey(achievement => achievement.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasData(AchievementSeed.Items);
        });
    }
}
