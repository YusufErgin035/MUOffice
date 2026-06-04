using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProgProjc.Data;

namespace WebProgProjc.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var reservations = await _context.Reservations
            .Include(r => r.Room)
            .Include(r => r.User)
            .OrderByDescending(r => r.StartTime)
            .ToListAsync();

        var rooms = await _context.Rooms.ToListAsync();

        ViewBag.Rooms = rooms;
        return View(reservations);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CancelReservation(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null) return NotFound();

        reservation.IsCancelled = true;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Rezervasyon iptal edildi.";
        return RedirectToAction(nameof(Index));
    }
}
