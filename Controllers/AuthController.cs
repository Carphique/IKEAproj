using ikea.DTO.Requests;
using ikea.Models;
using ikea.Sources;
using Microsoft.AspNetCore.Mvc;

namespace ikea.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthSource _authSource;

        public AuthController(AuthSource authSource)
        {
            _authSource = authSource;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto dto)
        {
            var token = await _authSource.LoginAsync(dto.Email, dto.Password);
            if (token == null)
                return Unauthorized(new { message = "Неправильний логін або пароль" });

            return Ok(new { token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto dto)
        {
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = dto.Password, 
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Role = "Customer"
            };

            var token = await _authSource.RegisterAsync(user);
            if (token == null)
                return BadRequest(new { message = "Користувач з таким email вже існує" });

            return Ok(new { token });
        }
    }
}