using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MisLogrosGames.Models;
using MisLogrosGames.Services;

namespace MisLogrosGames.Controllers;

[Authorize]
public sealed class AchievementsController : Controller
{
    private readonly AchievementService _achievements;
    private readonly UserManager<ApplicationUser> _userManager;

    public AchievementsController(
        AchievementService achievements,
        UserManager<ApplicationUser> userManager)
    {
        _achievements = achievements;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        return View(await BuildPageAsync(new CreateAchievementViewModel()));
    }

    private async Task<AchievementsPageViewModel> BuildPageAsync(CreateAchievementViewModel create)
    {
        var userId = Guid.Parse(_userManager.GetUserId(User)!);
        var items = await _achievements.GetForUserAsync(userId);
        return new AchievementsPageViewModel { Items = items, Create = create };
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unlock(Guid id)
    {
        var userId = Guid.Parse(_userManager.GetUserId(User)!);
        await _achievements.UnlockAsync(userId, id);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind(Prefix = "Create")] CreateAchievementViewModel model)
    {
        if (!AchievementIcons.All.Contains(model.Icon))
        {
            ModelState.AddModelError(nameof(model.Icon), "Elige un ícono válido.");
        }

        if (!ModelState.IsValid)
        {
            return View(nameof(Index), await BuildPageAsync(model));
        }

        var userId = Guid.Parse(_userManager.GetUserId(User)!);
        var created = await _achievements.CreateCustomAsync(userId, model.Name, model.Description, model.Icon);

        if (!created)
        {
            ModelState.AddModelError(string.Empty, "No se pudo guardar el logro. Inténtalo de nuevo.");
            return View(nameof(Index), await BuildPageAsync(model));
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteCustom(Guid id)
    {
        var userId = Guid.Parse(_userManager.GetUserId(User)!);
        await _achievements.DeleteCustomAsync(userId, id);
        return RedirectToAction(nameof(Index));
    }
}
