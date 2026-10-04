using _30_TranTheTruong_Assignment01_FrontEnd.Filters;
using _30_TranTheTruong_Assignment01_FrontEnd.Models;
using _30_TranTheTruong_Assignment01_FrontEnd.Services;
using Microsoft.AspNetCore.Mvc;

namespace _30_TranTheTruong_Assignment01_FrontEnd.Controllers
{
    [RequireRole("Admin")]
    public class ReportController : BaseController
    {
        private readonly ApiClient _api;

        public ReportController(ApiClient api)
        {
            _api = api;
        }

        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var start = (startDate ?? DateTime.Today.AddDays(-30)).Date;
            var end = (endDate ?? DateTime.Today).Date;

            ViewBag.Start = start.ToString("yyyy-MM-dd");
            ViewBag.End = end.ToString("yyyy-MM-dd");

            if (start > end)
            {
                ViewBag.Error = "Start date must be earlier than or equal to end date.";
                return View(new ReportVM { StartDate = start, EndDate = end });
            }

            var result = await _api.GetReportAsync(start, end);
            if (!result.Success) ViewBag.Error = result.Error;

            return View(result.Data ?? new ReportVM { StartDate = start, EndDate = end });
        }
    }

}
