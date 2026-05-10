using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ZapRu.Data;

[Route("api/market")]
[ApiController]
public class MarketApiController : ControllerBase
{
    private readonly ZapRuDbContext _context;
    public MarketApiController(ZapRuDbContext context) => _context = context;

    [HttpGet("users")]
    public async Task<IActionResult> GetUsersWithCars()
    {
        var users = await _context.Users
        .Where(u => u.Cars.Any(c => c.Status == "active" && !c.Parts.Any(p => p.Bids.Any(b => b.Status == "pending"))))
        .Select(u => new
        {
            u.UserID,
            u.FirstName,
            u.LastName,
            u.Phone,
            Cars = u.Cars
                .Where(c => c.Status == "active" && !c.Parts.Any(p => p.Bids.Any(b => b.Status == "pending")))
                .Select(c => new
                {
                    c.CarID,
                    c.CarModel.Brand,
                    c.CarModel.Model,
                    c.ManufactureYear,
                    c.Mileage,
                    c.Condition,
                    c.Region,
                    c.Description,
                    Photos = c.Photos.Select(p => p.FilePath),
                    Parts = c.Parts.Select(p => new
                    {
                        p.PartID,
                        p.PartsCatalog.Name,
                        p.Price,
                        p.Condition,
                        p.IsSold
                    })
                })
        })
        .ToListAsync();
        return Ok(users);
    }
}
