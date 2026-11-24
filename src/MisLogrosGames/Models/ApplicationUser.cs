using Microsoft.AspNetCore.Identity;

namespace MisLogrosGames.Models;

public sealed class ApplicationUser : IdentityUser<Guid>
{
    public ICollection<UserAchievement> UserAchievements { get; set; } = new List<UserAchievement>();
    public ICollection<Achievement> CustomAchievements { get; set; } = new List<Achievement>();
}
