using _30_TranTheTruong_Assignment01_BackEnd.DTOs;
using _30_TranTheTruong_Assignment01_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _30_TranTheTruong_Assignment01_BackEnd.Controllers
{
    [ApiController]
    [Route("api/news")]
    [Authorize(Roles = "Staff")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _service;

        public NewsController(INewsService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Create([FromBody] NewsArticleRequest request)
        {
            var result = _service.Create(request, User.GetAccountId());
            if (!result.Success) return BadRequest(new { error = result.Error });
            return StatusCode(StatusCodes.Status201Created);
        }

        [HttpPut("{id}")]
        public IActionResult Update(string id, [FromBody] NewsArticleRequest request)
        {
            var result = _service.Update(id, request, User.GetAccountId());
            if (result.NotFound) return NotFound(new { error = result.Error });
            if (!result.Success) return BadRequest(new { error = result.Error });
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var result = _service.Delete(id);
            if (result.NotFound) return NotFound(new { error = result.Error });
            if (!result.Success) return BadRequest(new { error = result.Error });
            return NoContent();
        }
    }

}
