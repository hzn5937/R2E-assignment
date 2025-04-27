using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAdmin()
        {
            return Ok(new { value = "this is an admin protected value" });
        }

        [HttpGet("user")]
        [Authorize(Roles = "User")]
        public IActionResult GetUser()
        {
            return Ok(new { value = "this is an user protected value" });
        }

        [HttpGet("general")]
        [Authorize]
        public IActionResult Get()
        {
            return Ok(new { value = "this is a general protected value" });
        }
    }
}
