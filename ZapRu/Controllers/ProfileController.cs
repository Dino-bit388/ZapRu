using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZapRu.Data;
using ZapRu.Entities;


[Authorize]
public class ProfileController : Controller
{
    private readonly ZapRuDbContext _context;
    public ProfileController(ZapRuDbContext context) => _context = context;

    public async Task<IActionResult> Index()
    {
        var userId = int.Parse(User.FindFirst("UserID").Value);
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return Challenge();
        return View(user);
    }
}