using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using IronDomeAPI.Services;
using IronDomeAPI.Models;
using IronDomeAPI.HttpUtils;
using IronDomeAPI.Enums;
using IronDomeAPI.Middleware;
using IronDomeAPI.Middleware.Attack;
using IronDomeAPI.Data;
using IronDomeAPI.Enums;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;


namespace IronDomeAPI.Controllers

{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AttacksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AttacksController> _logger;
        public AttacksController(ILogger<AttacksController> logger, ApplicationDbContext context) 
        {
            this._context = context;
            this._logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAttacks()
        {
            int status = StatusCodes.Status200OK;
            var attacks = await this._context.attacks.ToListAsync();
           
            return Ok(attacks);
        }
        [HttpPost]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateAttack(Attack attack)
        {
            //attack.id = Guid.NewGuid();
            attack.status = attackStatuses.PENDING;
            this._context.attacks.Add(attack);
            await this._context.SaveChangesAsync();
            return StatusCode(
                StatusCodes.Status201Created,
                new { success = true, attack = attack }
                );
        }


        [HttpPost("{id}/start")]
        public async Task< IActionResult> StartAttack(Guid id)
        {
            Attack attack = await this._context.attacks.FirstOrDefaultAsync(att => att.id == id);
            int status = StatusCodes.Status404NotFound;
            if (attack == null) return StatusCode(status, HttpUtils.HttpUtils.Response(status, "Attack not found"));
            if (attack.status == attackStatuses.COMPLETED)
            {
                status = StatusCodes.Status400BadRequest;
                return StatusCode(
                    status,
                    new
                    {
                        success = false,
                        error = "Cannot start an attack that hasalready beencompleted."
                    }
                );
            }
            attack.startedAt = DateTime.Now;
            attack.status = attackStatuses.IN_PROGRESS;
            Task attackTask = Task.Run(() =>
            {
                Task.Delay(10000);
            });
            return StatusCode(
                StatusCodes.Status200OK,
                new { message = "Attack Started.", TaskId = attackTask.Id }
            );

        }


        [HttpGet("{id}/status")]
        public async Task <IActionResult> AttackStatus(Guid id)
        {
            int status;
            Attack attack = await this._context.attacks.FirstOrDefaultAsync(att => att.id == id);

            if (attack == null)
            {
                status = StatusCodes.Status404NotFound;
                return StatusCode(status, HttpUtils.HttpUtils.Response(status, "attack not found"));
            }

            status = StatusCodes.Status200OK;
            return StatusCode(status, HttpUtils.HttpUtils.Response(status, new { attack = attack }));
        }


        [HttpPost("{id}/intercept")]
        public async Task <IActionResult>InterceptAttack(Guid id)
        {
            int status;
            Attack attack = await this._context.attacks.FirstOrDefaultAsync(att => att.id == id);
            if (attack.status != attackStatuses.INTERCEPTED)
            {
                status = StatusCodes.Status400BadRequest;
                return StatusCode(status, HttpUtils.HttpUtils.Response(status, "Cannot intercept an attack that has already been completed."));
                
            }
            status = StatusCodes.Status200OK;
            return StatusCode(status, HttpUtils.HttpUtils.Response(status, "Attack intercepted."));
           
        }
        [HttpPut("{id}/missiles")]
        public async Task <IActionResult> DefineAttackMissiles( Guid id, Attack newAttack)
        {
            int status;
            Attack attack = await this._context.attacks.FirstOrDefaultAsync(att => att.id == id);
            attack.missileCount = newAttack.missileCount;
            for (int i = 0; i < newAttack.missileTypes.Count; i++)
            {
                attack.missileTypes.Add(newAttack.missileTypes[i]);
            }
            
            status = StatusCodes.Status200OK;
            return StatusCode(status, HttpUtils.HttpUtils.Response(status, "Missiles Defined."));
        }



        //[HttpPost("{id}")]
        //public IActionResult DeleteAttack(Guid id)
        //{
        //    Attack attack = DbService.AttacksList.FirstOrDefault(att => att.id == id);
        //    attack.Remove();

        //    return StatusCode(NoContent, new { message = "Attack intercepted." });

        //}




    }

}
