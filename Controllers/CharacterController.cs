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
    [Route("api/Characters")]
    public class CharacterController : Controller
    {
        private readonly DataContext _context;

        public CharacterController(DataContext context)
        {
            _context = context;
        }

        //GET: api/Games
        [Authorize]
        public IEnumerable<Character> GetCharacter()
        {
            return _context.Character;
        }

        //Get: api/Characters/1
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCharacter([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var character = await _context.Character.SingleOrDefaultAsync(m => m.CharacterId == id);

            if (character == null)
            {
                return NotFound();
            }

            return Ok(character);
        }

        //Get: api/Characters/user/1
        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUsersCharacter([FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var characters = await _context.Character.Where(m => m.UserId == userId).AsNoTracking().ToListAsync();

            if (characters == null)
            {
                return NotFound();
            }

            return Ok(characters);
        }

        //PUT: api/Characters/1
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCharacter([FromRoute] int id, [FromBody] Character character)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != character.CharacterId)
            {
                return BadRequest();
            }

            string authId = HttpContext.User.Claims.First().Value;
            User _user = _context.User.SingleOrDefault(u => u.UserId.ToString() == authId);

            if (_user != null && _user.UserId != character.UserId)
            {
                return Unauthorized();
            }

            _context.Entry(character).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!CharacterExists(id))
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

        //POST: api/Characters
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostCharacter([FromBody] Character character)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string authId = HttpContext.User.Claims.First().Value;
            User _user = _context.User.SingleOrDefault(u => u.UserId.ToString() == authId);

            if (_user != null && _user.UserId != character.UserId)
            {
                return Unauthorized();
            }

            _context.Character.Add(character);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCharacter", new { id = character.CharacterId}, character);
        }

        //DELETE: api/Characters/4
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCharacter([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var character = await _context.Character.SingleOrDefaultAsync(m => m.CharacterId == id);
            if (character == null)
            {
                return NotFound();
            }

            string authId = HttpContext.User.Claims.First().Value;
            User _user = _context.User.SingleOrDefault(u => u.UserId.ToString() == authId);

            if(_user != null && _user.UserId != character.UserId)
            {
                return Unauthorized();
            }
            
            _context.Character.Remove(character);
            await _context.SaveChangesAsync();

            return Ok(character);
        }


        private bool CharacterExists(int id)
        {
            return _context.Character.Any(e => e.CharacterId == id);
        }
    }
}
