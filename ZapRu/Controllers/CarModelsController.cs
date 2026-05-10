using Microsoft.AspNetCore.Mvc;
using ZapRu.Data;
using Microsoft.EntityFrameworkCore;

namespace ZapRu.Controllers
{
    [Route("api/carmodels")]
    [ApiController]
    public class CarModelsController : ControllerBase
    {
        private readonly ZapRuDbContext _context;
        public CarModelsController(ZapRuDbContext context) => _context = context;
        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _context.CarModels.ToListAsync());
    }
}
