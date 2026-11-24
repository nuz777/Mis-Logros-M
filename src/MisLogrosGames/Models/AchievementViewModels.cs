using System.ComponentModel.DataAnnotations;
using MisLogrosGames.Services;

namespace MisLogrosGames.Models;

public static class AchievementIcons
{
    public static readonly string[] All = ["start", "login", "stone", "diamond", "creeper"];
}

public sealed class CreateAchievementViewModel
{
    [Required(ErrorMessage = "Escribe un nombre.")]
    [StringLength(128, ErrorMessage = "El nombre es demasiado largo.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Escribe una descripción.")]
    [StringLength(256, ErrorMessage = "La descripción es demasiado larga.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Elige un ícono.")]
    public string Icon { get; set; } = "stone";
}

public sealed class AchievementsPageViewModel
{
    public IReadOnlyList<AchievementListItem> Items { get; set; } = [];
    public CreateAchievementViewModel Create { get; set; } = new();
}
