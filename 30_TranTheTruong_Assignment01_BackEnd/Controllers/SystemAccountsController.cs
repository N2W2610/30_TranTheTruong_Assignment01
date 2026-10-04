using _30_TranTheTruong_Assignment01_BackEnd.Models;
using _30_TranTheTruong_Assignment01_BackEnd.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;

namespace _30_TranTheTruong_Assignment01_BackEnd.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SystemAccountsController : ODataApiController
    {
        private readonly IAccountService _service;

        public SystemAccountsController(IAccountService service)
        {
            _service = service;
        }

        [EnableQuery]
        public IActionResult Get() => Ok(_service.GetAll().AsQueryable());

        public IActionResult Get(short key)
        {
            var account = _service.Get(key);
            return account == null ? NotFound() : Ok(account);
        }

        public IActionResult Post([FromBody] SystemAccount account)
        {
            if (!ModelState.IsValid) return InvalidModel();

            var result = _service.Create(account);
            return FromResult(result, () =>
            {
                account.AccountPassword = null;
                return Created(account);
            });
        }

        public IActionResult Put(short key, [FromBody] SystemAccount account)
        {
            if (!ModelState.IsValid) return InvalidModel();

            var result = _service.Update(key, account);
            return FromResult(result, () => NoContent());
        }

        public IActionResult Delete(short key)
        {
            var result = _service.Delete(key);
            return FromResult(result, () => NoContent());
        }
    }
}
