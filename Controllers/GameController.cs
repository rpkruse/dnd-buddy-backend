using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using dnd_buddy_backend.Models;

namespace dnd_buddy_backend.Controllers
{
    [Produces("application/json")]
    [Route("api/Games")]
    public class GameController : Controller
    {
        private readonly DataContext _context;

        public GameController(DataContext context)
        {
            _context = context;
        }

        //GET: api/Games
        public IEnumerable<Game> GetGame()
        {
            return _context.Game;
        }

        //Get: api/Games/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGame([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = await _context.Game.SingleOrDefaultAsync(m => m.GameId == id);

            if(game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        //Get: api/Games/detials/2
        [HttpGet("details/{id}")]
        public async Task<IActionResult> GetGameDetails([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var game = await _context.Game
                            .Include(c => c.Character)
                            .AsNoTracking()
                            .SingleOrDefaultAsync(m => m.GameId == id);

            if(game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        //Get: api/Games/user/1
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUsersGame([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var games = new List<Game>();
            var characters = await _context.Character.Where(m => m.UserId == userId).AsNoTracking().ToListAsync();
            var gmGames = await _context.Game.Where(m => m.UserId == userId).AsNoTracking().ToListAsync();

            foreach(Character c in characters)
            {
                var game = _context.Game.SingleOrDefault(x => x.GameId == c.GameId);
                games.Add(game);
            }

            //var games = await _context.Game.Where(m => m.UserId == userId).AsNoTracking().ToListAsync();

            if (games == null)
            {
                return NotFound();
            }

            foreach(Game g in gmGames)
            {
                games.Add(g);
            }

            return Ok(games);
        }

        //Get: api/Games/user/1
        [HttpGet("open/{userId}")]
        public async Task<IActionResult> GetGamesNotCurrentlyIn([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var games = await _context.Game.Where(m => m.UserId != userId).AsNoTracking().ToListAsync();

            if (games == null)
            {
                return NotFound();
            }

            return Ok(games);
        }


        //PUT: api/Games/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame([FromRoute] int id, [FromBody] Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(id != game.GameId)
            {
                return BadRequest();
            }

            _context.Entry(game).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!GameExists(id))
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

        //POST: api/Games
        [HttpPost]
        public async Task<IActionResult> PostGame([FromBody] Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Game.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.GameId }, game);
        }

        //DELETE: api/Games/4
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var game = await _context.Game.SingleOrDefaultAsync(m => m.GameId == id);
            if(game == null)
            {
                return NotFound();
            }

            _context.Game.Remove(game);
            await _context.SaveChangesAsync();

            return Ok(game);
        }

        //api/Games/check/<gamename>
        [HttpGet("check/{gamename}")]
        public async Task<IActionResult> CheckForGameName([FromRoute] string gamename)
        {
            Game _game = _context.Game.SingleOrDefault(x => x.Name == gamename);

            if (_game != null) //If the game name is already taken we return an error
            {
                ModelState.AddModelError("Error", "Game name already taken");
                return BadRequest(ModelState);
            }

            return Ok();
        }


        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.GameId == id);
        }
    }
}
