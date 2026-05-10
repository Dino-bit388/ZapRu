using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZapRu.Data;
using ZapRu.Entities;

namespace ZapRu.Controllers
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ZapRuDbContext _context;

        public AuthController(ZapRuDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRequest request) 
        {
            if (request == null) return BadRequest(new { message = "Некорректный запрос" });

            if (request.FirstName == null || request.LastName == null || request.Email == null)
            {
                return BadRequest(new { message = "Нужно заполнить обязательные поля: имя, фамилия, почта" });
            }

            if (await _context.Users.AnyAsync(u => u.Email == request.Email)) 
            {
                return BadRequest(new { message = "Этот email уже занят"});
            
            }

            var user = new User();
            if (request.Phone != null)
            {
                if (request.Phone.Length == 11 && request.Phone.All(ch => char.IsDigit(ch)))
                {
                    user.Phone = request.Phone;
                }
            }

            user.SetPassword(request.Password);
            user.RegisteredAt = DateTime.UtcNow;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;

            



            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Регистрация прошла успешно" });
        
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthRequest request) 
        {
            if (request == null) return BadRequest(new { message = "Некорректный запрос" });
            if (request.Email == null)
            {
                return BadRequest(new { message = "Нужно заполнить обязательные поля: Почта" });
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                return Unauthorized(new { message = "Неверный пароль"});


                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("UserID", user.UserID.ToString())
                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));
            return Ok(new { message = "Вход выполнен" });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return Ok(new { message = "Вы вышли из системы" });
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var userId = int.Parse(User.FindFirst("UserID").Value);
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return NotFound();
            return Ok(new
            {
                user.FirstName,
                user.LastName,
                user.Email
            });
        }


    }
}
