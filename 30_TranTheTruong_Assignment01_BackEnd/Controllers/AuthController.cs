using _30_TranTheTruong_Assignment01_BackEnd.DTOs;
using _30_TranTheTruong_Assignment01_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _30_TranTheTruong_Assignment01_BackEnd.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAccountService _accounts;
        private readonly ITokenService _tokens;

        public AuthController(IAccountService accounts, ITokenService tokens)
        {
            _accounts = accounts;
            _tokens = tokens;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var user = _accounts.Authenticate(request.Email, request.Password);
            if (user == null)
                return Unauthorized(new { error = "Invalid email or password." });

            if (user.Role == "Lecturer")
                return StatusCode(StatusCodes.Status403Forbidden, new { error = "Only Admin and Staff members can sign in to this system." });

            return Ok(new LoginResponse
            {
                Token = _tokens.CreateToken(user),
                AccountId = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role
            });
        }
    }

}
