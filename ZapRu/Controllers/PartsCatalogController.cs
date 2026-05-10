using Microsoft.AspNetCore.Mvc;
using ZapRu.Data;
using Microsoft.EntityFrameworkCore;
namespace ZapRu.Controllers
{
    [Route("api/partscatalog")]
    [ApiController]
    public class PartsCatalogController : ControllerBase
    {
        private readonly ZapRuDbContext _context;
        public PartsCatalogController(ZapRuDbContext context) => _context = context;
        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _context.PartsCatalogs.ToListAsync());
    }
}
