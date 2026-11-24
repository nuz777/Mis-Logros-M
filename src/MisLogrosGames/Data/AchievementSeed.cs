using MisLogrosGames.Models;

namespace MisLogrosGames.Data;

internal static class AchievementSeed
{
    public static readonly Guid FirstStepsId = Guid.Parse("11111111-1111-1111-1111-111111111111");

    public static readonly Achievement[] Items =
    [
        new()
        {
            Id = FirstStepsId,
            Code = "first_steps",
            Name = "Primeros pasos",
            Description = "Crea tu cuenta y entra en el mundo.",
            Icon = "start"
        },
        new()
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Code = "returning_hero",
            Name = "Héroe que regresa",
            Description = "Inicia sesión otra vez para seguir la aventura.",
            Icon = "login"
        },
        new()
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Code = "stone_collector",
            Name = "Coleccionista de piedra",
            Description = "El primer bloque de cualquier gran construcción.",
            Icon = "stone"
        },
        new()
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Code = "diamond_finder",
            Name = "Buscador de diamantes",
            Description = "Brilla más que un cofre lleno de botín.",
            Icon = "diamond"
        },
        new()
        {
            Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            Code = "creeper_survivor",
            Name = "Sobreviviente de creepers",
            Description = "Escuchaste el siseo y saliste corriendo a tiempo.",
            Icon = "creeper"
        }
    ];
}
