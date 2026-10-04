using _30_TranTheTruong_Assignment01_BackEnd.Models;
using _30_TranTheTruong_Assignment01_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace _30_TranTheTruong_Assignment01_BackEnd.Controllers
{
    [Authorize(Roles = "Staff")]
    public class CategoriesController : ODataApiController
    {
        private readonly ICategoryService _service;

        public CategoriesController(ICategoryService service)
        {
            _service = service;
        }

        [EnableQuery]
        public IActionResult Get() => Ok(_service.GetAll().AsQueryable());

        public IActionResult Get(short key)
        {
            var category = _service.Get(key);
            return category == null ? NotFound() : Ok(category);
        }

        public IActionResult Post([FromBody] Category category)
        {
            if (!ModelState.IsValid) return InvalidModel();

            var result = _service.Create(category);
            return FromResult(result, () => Created(category));
        }

        public IActionResult Put(short key, [FromBody] Category category)
        {
            if (!ModelState.IsValid) return InvalidModel();

            var result = _service.Update(key, category);
            return FromResult(result, () => NoContent());
        }

        public IActionResult Delete(short key)
        {
            var result = _service.Delete(key);
            return FromResult(result, () => NoContent());
        }
    }

}
