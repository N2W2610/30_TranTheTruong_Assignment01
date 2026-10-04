using _30_TranTheTruong_Assignment01_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Results;

namespace _30_TranTheTruong_Assignment01_BackEnd.Controllers
{
    [AllowAnonymous]
    public class NewsArticlesController : ODataApiController
    {
        private readonly INewsService _service;

        public NewsArticlesController(INewsService service)
        {
            _service = service;
        }

        private bool OnlyActive => !User.IsInRole("Staff");

        [EnableQuery]
        public IActionResult Get() => Ok(_service.GetArticles(OnlyActive).AsQueryable());

        [EnableQuery]
        public IActionResult Get(string key)
        {
            var matches = _service.GetArticles(OnlyActive)
                .Where(n => n.NewsArticleId == key)
                .AsQueryable();
            return Ok(SingleResult.Create(matches));
        }
    }

}
