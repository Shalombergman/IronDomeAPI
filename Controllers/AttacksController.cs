using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IronDomeAPI.Services;
using IronDomeAPI.Models;
namespace IronDomeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttacksController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateAttack([FromBody] object attack)
        {
            Guid newAttackId = Guid.NewGuid();       
            attack.id = newAttackId;
            DbService.AttacksList.Add(attack);
            return Ok(DbService.AttacksList.ToArray());
        }
    }
}
