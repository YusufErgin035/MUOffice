using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProgProjc.Data;
using WebProgProjc.Models;

namespace WebProgProjc.Controllers;

[Authorize(Roles = "User,Admin")]
public class UserController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var reservations = await _context.Reservations
            .Include(r => r.Room)
            .Where(r => r.UserId == userId && !r.IsCancelled && r.EndTime > DateTime.UtcNow)
            .OrderBy(r => r.StartTime)
            .ToListAsync();

        return View(reservations);
    }
}
