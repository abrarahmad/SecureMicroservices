using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Movies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(User.Claims.Select(s => new
            {
                s.Type,
                s.Value
            }));
        }
    }
}
