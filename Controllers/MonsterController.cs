using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using dnd_buddy_backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace dnd_buddy_backend.Controllers
{
    [Produces("application/json")]
    [Route("api/Monsters")]
    public class MonsterController : Controller
    {
        private readonly DataContext _context;

        public MonsterController(DataContext context)
        {
            _context = context;
        }

        //GET: api/Monsters
        /// <summary>
        /// Gets a list of Monsters
        /// </summary>
        /// <returns>A list of Monsters</returns>
        [Authorize]
        [HttpGet("")]
        public IEnumerable<Monster> GetMonster()
        {
            return _context.Monster;
        }

        //Get: api/Monsters/1
        /// <summary>
        /// Gets a specific Monster
        /// </summary>
        /// <param name="id">The id of the Monster to get</param>
        /// <returns>A specific Monster</returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMonster([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var monster = await _context.Monster.SingleOrDefaultAsync(m => m.MonsterId == id);

            if (monster == null)
            {
                return NotFound();
            }

            return Ok(monster);
        }

        //Get: api/Monsters/game/1
        /// <summary>
        /// Gets a list of Monsters in a given game
        /// </summary>
        /// <param name="gameId">The id of the game</param>
        /// <returns>A list of Monsters in a given game</returns>
        [Authorize]
        [HttpGet("game/{gameId}")]
        public async Task<IActionResult> GetUsersCharacter([FromRoute] int gameId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var monsters = await _context.Monster.Where(m => m.GameId == gameId).AsNoTracking().ToListAsync();

            if (monsters == null)
            {
                return NotFound();
            }

            return Ok(monsters);
        }

        //PUT: api/Monsters/1
        /// <summary>
        /// Updates a given Monster
        /// </summary>
        /// <param name="id">The id of the Monster</param>
        /// <param name="monster">The updated Monster object</param>
        /// <returns>None</returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter([FromRoute] int id, [FromBody] Monster monster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != monster.MonsterId)
            {
                return BadRequest();
            }

            string authId = HttpContext.User.Claims.First().Value;
            User _user = _context.User.SingleOrDefault(u => u.UserId.ToString() == authId);


            if (_user != null && !this.isProperUser(_user, id))
            {
                return Unauthorized();
            }


            _context.Entry(monster).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!MonsterExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //POST: api/Monsters
        /// <summary>
        /// Adds a new Monster to the database
        /// </summary>
        /// <param name="monster">The new Monster to add</param>
        /// <returns>A new character object</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostCharacter([FromBody] Monster monster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string authId = HttpContext.User.Claims.First().Value;
            User _user = _context.User.SingleOrDefault(u => u.UserId.ToString() == authId);

            if (_user != null && !this.isProperUser(_user, monster.MonsterId))
            {
                return Unauthorized();
            }

            _context.Monster.Add(monster);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMonster", new { id = monster.MonsterId }, monster);
        }

        //DELETE: api/Monsters/4
        /// <summary>
        /// Removes a specific Monster from the database
        /// </summary>
        /// <param name="id">The id of the Monster to remove</param>
        /// <returns>The deleted Monster</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMonster([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var monster = await _context.Monster.SingleOrDefaultAsync(m => m.MonsterId == id);
            if (monster == null)
            {
                return NotFound();
            }

            string authId = HttpContext.User.Claims.First().Value;
            User _user = _context.User.SingleOrDefault(u => u.UserId.ToString() == authId);


            if (_user != null && !this.isProperUser(_user, id))
            {
                return Unauthorized();
            }

            _context.Monster.Remove(monster);
            await _context.SaveChangesAsync();

            return Ok(monster);
        }

        private bool isProperUser(User user, int monsterId)
        {
            Monster monster = _context.Monster.SingleOrDefault(m => m.MonsterId == monsterId);

            if (monster == null) return false;

            //If the game and monster's id match, we can continue
            Game game = _context.Game.SingleOrDefault(g => g.GameId == monster.GameId);

            if (game == null) return false;


            return game.UserId == user.UserId; //If the user calling is the owner of the game with the given monster and it exists
        }

        private bool MonsterExists(int id)
        {
            return _context.Monster.Any(e => e.MonsterId == id);
        }
    }
}
