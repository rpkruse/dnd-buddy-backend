using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using dnd_buddy_backend.Models;
using System;
using Microsoft.AspNetCore.Authorization;

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
        [Authorize]
        public IEnumerable<Game> GetGame()
        {
            return _context.Game;
        }

        //Get: api/Games/1
        [Authorize]
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
        [Authorize]
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
        /// <summary>
        /// Games that the user is in or the DM of
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
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

        //Get: api/Games/open/1
        /// <summary>
        /// Games that the user can join (games they are not the DM AND aren't in)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("open/{userId}")]
        public async Task<IActionResult> GetGamesNotCurrentlyIn([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notGMGames = await _context.Game.Where(m => m.UserId != userId).AsNoTracking().ToListAsync(); //Get all games we do not own

            var characters = await _context.Character.Where(m => m.UserId == userId).AsNoTracking().ToListAsync(); //Get all of our characters


            notGMGames.RemoveAll(g => characters.Exists(c => c.GameId == g.GameId)); //Remove any game that we are currently in


            if (notGMGames == null)
            {
                return NotFound();
            }


            return Ok(notGMGames);
        }

        //Get: api/gm/
        [Authorize]
        [HttpGet("gm/{userId}")]
        public async Task<IActionResult> GetGMGames([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var games = await _context.Game.Where(m => m.UserId == userId).AsNoTracking().ToListAsync();

            if (games == null)
            {
                return NotFound();
            }

            return Ok(games);
        }


        //PUT: api/Games/1
        [Authorize]
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

            string authId = HttpContext.User.Claims.First().Value;
            User _authUser = _context.User.SingleOrDefault(u => u.UserId.ToString() == authId);

            if (_authUser != null && _authUser.UserId != game.UserId)
            {
                return Unauthorized();
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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostGame([FromBody] Game game)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string authId = HttpContext.User.Claims.First().Value;
            User _authUser = _context.User.SingleOrDefault(u => u.UserId.ToString() == authId);

            if (_authUser != null && _authUser.UserId != game.UserId)
            {
                return Unauthorized();
            }

            _context.Game.Add(game);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGame", new { id = game.GameId }, game);
        }

        //DELETE: api/Games/4
        [Authorize]
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

            string authId = HttpContext.User.Claims.First().Value;
            User _authUser = _context.User.SingleOrDefault(u => u.UserId.ToString() == authId);

            if (_authUser != null && _authUser.UserId != game.UserId)
            {
                return Unauthorized();
            }

            _context.Game.Remove(game);
            await _context.SaveChangesAsync();

            return Ok(game);
        }

        //api/Games/check/<gamename>
        [Authorize]
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

        //api/Games/state/<gameId>
        [Authorize]
        [HttpGet("state/{gameId}")]
        public async Task<IActionResult> GetGameState([FromRoute] int gameId)
        {
            Game game = _context.Game.SingleOrDefault(x => x.GameId == gameId);

            if (game == null) //If the game name isn't found, return error
            {
                ModelState.AddModelError("Error", "No game found");
                return BadRequest(ModelState);
            }
            else if (game.GameState == null) //If the game state hasn't been saved yet, return error
            {
                ModelState.AddModelError("Error", "No game save found");
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(game.GameState);
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Error", "Error reading save");
                return BadRequest(ModelState);
            }
        }


        private bool GameExists(int id)
        {
            return _context.Game.Any(e => e.GameId == id);
        }
    }
}
