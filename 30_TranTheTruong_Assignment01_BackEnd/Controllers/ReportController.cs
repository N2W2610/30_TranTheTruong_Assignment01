using _30_TranTheTruong_Assignment01_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _30_TranTheTruong_Assignment01_BackEnd.Controllers
{
    [ApiController]
    [Route("api/report")]
    [Authorize(Roles = "Admin")]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportController(IReportService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            if (startDate.Date > endDate.Date)
                return BadRequest(new { error = "Start date must be earlier than or equal to end date." });

            return Ok(_service.Build(startDate, endDate));
        }
    }
}
