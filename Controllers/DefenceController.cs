using IronDomeAPI.Models;
using IronDomeAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IronDomeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DefenceController : ControllerBase
    {
        [HttpPut("missiles")]
        public IActionResult Missiles(Defence defence)
        {
            DefenceService.MissileCount = defence.MissileCount;
            DefenceService.MissileTypes = defence.MissileTypes;
            return Ok();
        }
    }
}
