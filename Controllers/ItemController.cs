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
    [Route("api/Items")]
    public class ItemController : Controller
    {
        private readonly DataContext _context;

        public ItemController(DataContext context)
        {
            _context = context;
        }

        //GET: api/Items
        /// <summary>
        /// Gets all items
        /// </summary>
        /// <returns>All items</returns>
        [Authorize]
        [HttpGet("")]
        public IEnumerable<Item> GetItem()
        {
            return _context.Item;
        }

        //Get: api/Items/1
        /// <summary>
        /// Gets a specific item
        /// </summary>
        /// <param name="id">The id of the item to get</param>
        /// <returns>A specific item</returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _context.Item.SingleOrDefaultAsync(m => m.ItemId == id);

            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        //Get: api/Items/character/1
        /// <summary>
        /// Gets all items for a specific character
        /// </summary>
        /// <param name="id">The id of the character</param>
        /// <returns>All items for a specific user</returns>
        [Authorize]
        [HttpGet("character/{id}")]
        public async Task<IActionResult> GetCharacterItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var items = new List<Item>();
            items = await _context.Item.Where(i => i.CharacterId == id).ToListAsync();

            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }

        //PUT: api/Items/1
        /// <summary>
        /// Updates an item
        /// </summary>
        /// <param name="id">The id of the item to update</param>
        /// <param name="item">The updated item object</param>
        /// <returns>None</returns>
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem([FromRoute] int id, [FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != item.ItemId)
            {
                return BadRequest();
            }

            if (!IsValidUser(item))
            {
                return Unauthorized();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (!ItemExists(id))
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

        //POST: api/Items
        /// <summary>
        /// Adds a new item to the database
        /// </summary>
        /// <param name="item">A new item object</param>
        /// <returns>The added item</returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PostItem([FromBody] Item item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!IsValidUser(item))
            {
                return Unauthorized();
            }

            _context.Item.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.ItemId }, item);
        }

        //DELETE: api/Items/4
        /// <summary>
        /// Removes a specific item from the database
        /// </summary>
        /// <param name="id">The id of the item to remove</param>
        /// <returns>The removed item</returns>
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = await _context.Item.SingleOrDefaultAsync(m => m.ItemId == id);
            if (item == null)
            {
                return NotFound();
            }

            if (!IsValidUser(item))
            {
                return Unauthorized();
            }

            _context.Item.Remove(item);
            await _context.SaveChangesAsync();

            return Ok(item);
        }


        private bool ItemExists(int id)
        {
            return _context.Item.Any(e => e.ItemId == id);
        }

        private bool IsValidUser(Item item)
        {
            string authId = HttpContext.User.Claims.First().Value;
            User _user = _context.User.SingleOrDefault(u => u.UserId.ToString() == authId);

            if (_user == null)
            {
                return false;
            }

            Character _character = _context.Character.SingleOrDefault(c => c.CharacterId == item.CharacterId);
            Game _game = _context.Game.SingleOrDefault(g => g.GameId == _character.CharacterId);

            if (_character == null && (_user.UserId != _character.CharacterId && _user.UserId != _game.GameId)) //User || DM
            {
                return false;
            }

            return true;
        }
    }
}
