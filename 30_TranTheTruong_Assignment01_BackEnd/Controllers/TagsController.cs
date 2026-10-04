using _30_TranTheTruong_Assignment01_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace _30_TranTheTruong_Assignment01_BackEnd.Controllers
{
    [Authorize(Roles = "Staff")]
    public class TagsController : ODataApiController
    {
        private readonly ITagService _service;

        public TagsController(ITagService service)
        {
            _service = service;
        }

        [EnableQuery]
        public IActionResult Get() => Ok(_service.GetAll().AsQueryable());
    }
}
