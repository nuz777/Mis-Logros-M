namespace MisLogrosGames.Models;

public sealed class UserAchievement
{
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = null!;
    public Guid AchievementId { get; set; }
    public Achievement Achievement { get; set; } = null!;
    public DateTimeOffset UnlockedAt { get; set; } = DateTimeOffset.UtcNow;
}
