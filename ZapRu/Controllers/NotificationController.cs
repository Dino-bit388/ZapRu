using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZapRu.Data;
using ZapRu.Entities;

namespace ZapRu.Controllers
{
    [Route("api/notifications")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly ZapRuDbContext _context;
        public NotificationController(ZapRuDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = int.Parse(User.FindFirst("UserID").Value);
            var notifications = await _context.Notifications
                .Where(n => n.UserID == userId)
                .OrderByDescending(n => n.CreatedAt)
                .Select(n => new
                {
                    n.NotificationID,
                    n.Type,
                    n.Message,
                    n.IsRead,
                    n.CreatedAt,
                    n.ResponseStatus,
                    n.PartID
                })
                .ToListAsync();
            return Ok(notifications);
        }

        [HttpPost("{id}/respond")]
        public async Task<IActionResult> RespondToNotification(int id, [FromBody] RespondRequest request)
        {
            var userId = int.Parse(User.FindFirst("UserID").Value);
            var notif = await _context.Notifications
                .Include(n => n.Part)
                .ThenInclude(p => p.Car)
                .FirstOrDefaultAsync(n => n.NotificationID == id && n.UserID == userId);

            if (notif == null) return NotFound();
            if (notif.Type != "bid") return BadRequest("Неподдерживаемый тип уведомления");
            if (notif.ResponseStatus != null) return BadRequest("Уведомление уже обработано");

            notif.ResponseStatus = request.Accept;
            notif.IsRead = true;

            if (request.Accept)
            {
                var transaction = new Transaction
                {
                    PartID = notif.PartID.Value,
                    BuyerID = notif.FromUserID.Value,         
                    SellerID = userId,
                    AgreedPrice = request.Price ?? 0,
                    Status = "pending",
                    CreatedAt = DateTime.UtcNow
                };
                _context.Transactions.Add(transaction);
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = request.Accept ? "Сделка создана" : "Запрос отклонён" });
        }
    }

    public class RespondRequest
    {
        public bool Accept { get; set; }
        public decimal? Price { get; set; }
    }
}