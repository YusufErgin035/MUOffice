using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProgProjc.Data;
using WebProgProjc.Models;

namespace WebProgProjc.Controllers;

public class RoomController : Controller
{
    private readonly ApplicationDbContext _context;

    public RoomController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var rooms = await _context.Rooms.Where(r => r.IsActive).ToListAsync();
        return View(rooms);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Room room)
    {
        if (ModelState.IsValid)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Oda başarıyla eklendi.";
            return RedirectToAction("Index", "Admin");
        }
        return View(room);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return NotFound();
        return View(room);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Room room)
    {
        if (id != room.Id) return NotFound();

        if (ModelState.IsValid)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Oda güncellendi.";
            return RedirectToAction("Index", "Admin");
        }
        return View(room);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var room = await _context.Rooms.FindAsync(id);
        if (room == null) return NotFound();

        room.IsActive = false;
        await _context.SaveChangesAsync();
        TempData["Success"] = "Oda devre dışı bırakıldı.";
        return RedirectToAction("Index", "Admin");
    }
}
