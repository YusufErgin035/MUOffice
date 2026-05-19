using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebProgProjc.Controllers;

[Authorize(Roles = "User,Admin")]
public class UserController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
