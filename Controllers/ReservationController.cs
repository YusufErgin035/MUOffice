using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebProgProjc.Data;
using WebProgProjc.Models;
using WebProgProjc.Models.ViewModels;

namespace WebProgProjc.Controllers;

[Authorize]
public class ReservationController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ReservationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var reservations = await _context.Reservations
            .Include(r => r.Room)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.StartTime)
            .ToListAsync();
        return View(reservations);
    }

    [HttpGet]
    public async Task<IActionResult> Create(int? roomId)
    {
        var rooms = await _context.Rooms.Where(r => r.IsActive).ToListAsync();
        ViewBag.Rooms = new SelectList(rooms, "Id", "Name", roomId);
        var model = new ReservationViewModel { RoomId = roomId ?? 0 };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ReservationViewModel model)
    {
        if (model.EndTime <= model.StartTime)
            ModelState.AddModelError("EndTime", "Bitiş saati başlangıç saatinden sonra olmalıdır.");

        if (model.StartTime < DateTime.Now.AddMinutes(30))
            ModelState.AddModelError("StartTime", "Rezervasyon en az 30 dakika önceden yapılmalıdır.");

        var duration = model.EndTime - model.StartTime;
        if (duration.TotalHours > 8)
            ModelState.AddModelError("EndTime", "Rezervasyon süresi maksimum 8 saat olabilir.");

        if (duration.TotalMinutes < 30)
            ModelState.AddModelError("EndTime", "Rezervasyon süresi en az 30 dakika olmalıdır.");

        if (ModelState.IsValid)
        {
            var startUtc = model.StartTime.ToUniversalTime();
            var endUtc = model.EndTime.ToUniversalTime();

            var conflict = await _context.Reservations.AnyAsync(r =>
                r.RoomId == model.RoomId &&
                !r.IsCancelled &&
                r.StartTime < endUtc &&
                r.EndTime > startUtc);

            if (conflict)
            {
                ModelState.AddModelError("", "Bu oda seçilen saatler için zaten rezerve edilmiş. Lütfen farklı bir saat aralığı seçin.");
            }
            else
            {
                var reservation = new Reservation
                {
                    RoomId = model.RoomId,
                    UserId = _userManager.GetUserId(User)!,
                    StartTime = startUtc,
                    EndTime = endUtc,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Rezervasyonunuz başarıyla oluşturuldu.";
                return RedirectToAction(nameof(Index));
            }
        }

        var rooms = await _context.Rooms.Where(r => r.IsActive).ToListAsync();
        ViewBag.Rooms = new SelectList(rooms, "Id", "Name", model.RoomId);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = _userManager.GetUserId(User);
        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == id && r.UserId == userId);

        if (reservation == null)
            return NotFound();

        reservation.IsCancelled = true;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Rezervasyon iptal edildi.";
        return RedirectToAction(nameof(Index));
    }
}
