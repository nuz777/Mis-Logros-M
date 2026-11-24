using Microsoft.EntityFrameworkCore;
using MisLogrosGames.Data;
using MisLogrosGames.Models;

namespace MisLogrosGames.Services;

public sealed record AchievementListItem(
    Guid Id,
    string Code,
    string Name,
    string Description,
    string Icon,
    DateTimeOffset? UnlockedAt,
    bool IsCustom);

public sealed class AchievementService
{
    private readonly ApplicationDbContext _dbContext;

    public AchievementService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<AchievementListItem>> GetForUserAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var unlocked = await _dbContext.UserAchievements
            .AsNoTracking()
            .Where(item => item.UserId == userId)
            .ToDictionaryAsync(
                item => item.AchievementId,
                item => item.UnlockedAt,
                cancellationToken);

        var achievements = await _dbContext.Achievements
            .AsNoTracking()
            .Where(achievement => achievement.OwnerId == null || achievement.OwnerId == userId)
            .OrderBy(achievement => achievement.Name)
            .ToListAsync(cancellationToken);

        return achievements
            .Select(achievement => new AchievementListItem(
                achievement.Id,
                achievement.Code,
                achievement.Name,
                achievement.Description,
                achievement.Icon,
                unlocked.TryGetValue(achievement.Id, out var unlockedAt) ? unlockedAt : null,
                achievement.OwnerId is not null))
            .ToList();
    }

    public async Task<bool> UnlockAsync(
        Guid userId,
        Guid achievementId,
        CancellationToken cancellationToken = default)
    {
        var exists = await _dbContext.Achievements
            .AnyAsync(achievement => achievement.Id == achievementId, cancellationToken);

        if (!exists)
        {
            return false;
        }

        var alreadyUnlocked = await _dbContext.UserAchievements
            .AnyAsync(
                item => item.UserId == userId && item.AchievementId == achievementId,
                cancellationToken);

        if (alreadyUnlocked)
        {
            return false;
        }

        _dbContext.UserAchievements.Add(new UserAchievement
        {
            UserId = userId,
            AchievementId = achievementId,
            UnlockedAt = DateTimeOffset.UtcNow
        });

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<bool> CreateCustomAsync(
        Guid userId,
        string name,
        string description,
        string icon,
        CancellationToken cancellationToken = default)
    {
        var achievement = new Achievement
        {
            Id = Guid.NewGuid(),
            Code = $"custom_{Guid.NewGuid():N}",
            Name = name.Trim(),
            Description = description.Trim(),
            Icon = icon,
            OwnerId = userId
        };

        _dbContext.Achievements.Add(achievement);
        _dbContext.UserAchievements.Add(new UserAchievement
        {
            UserId = userId,
            AchievementId = achievement.Id,
            UnlockedAt = DateTimeOffset.UtcNow
        });

        try
        {
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }
        catch (DbUpdateException)
        {
            return false;
        }
    }

    public async Task<bool> DeleteCustomAsync(
        Guid userId,
        Guid achievementId,
        CancellationToken cancellationToken = default)
    {
        var achievement = await _dbContext.Achievements
            .FirstOrDefaultAsync(
                item => item.Id == achievementId && item.OwnerId == userId,
                cancellationToken);

        if (achievement is null)
        {
            return false;
        }

        _dbContext.Achievements.Remove(achievement);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
