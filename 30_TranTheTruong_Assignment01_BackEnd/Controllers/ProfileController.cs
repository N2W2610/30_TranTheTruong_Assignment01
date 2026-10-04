using _30_TranTheTruong_Assignment01_BackEnd.DTOs;
using _30_TranTheTruong_Assignment01_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _30_TranTheTruong_Assignment01_BackEnd.Controllers
{
    [ApiController]
    [Route("api/profile")]
    [Authorize(Roles = "Staff")]
    public class ProfileController : ControllerBase
    {
        private readonly IAccountService _accounts;

        public ProfileController(IAccountService accounts)
        {
            _accounts = accounts;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var profile = _accounts.GetProfile(User.GetAccountId());
            return profile == null ? NotFound() : Ok(profile);
        }

        [HttpPut]
        public IActionResult Update([FromBody] ProfileRequest request)
        {
            var result = _accounts.UpdateProfile(User.GetAccountId(), request);
            if (result.NotFound) return NotFound(new { error = result.Error });
            if (!result.Success) return BadRequest(new { error = result.Error });
            return NoContent();
        }
    }

}
