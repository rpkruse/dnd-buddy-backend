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
    [Route("api/Users")]
    public class UserController : Controller
    {
        private readonly DataContext _context;

        public UserController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            return Unauthorized(); //Don't allow anyone to pull all of users
            //return _context.User;
        }

        //GET: api/Users/1
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.UserId == id);

            if(user == null)
            {
                return NotFound();
            }

            user.Password = null;
            return Ok(user);
        }

        //PUT: api/Users/2
        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser([FromRoute] int id, [FromBody] User user) //Maybe change to patch?
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if(id != user.UserId){
                return BadRequest();
            }

            if(user.Username == null || user.Username.Length <= 0)
            {
                user.Username = _context.User.AsNoTracking().SingleOrDefault(m => m.UserId == id).Username;
            }
            if (user.Password == null || user.Password.Length <= 0)
            {
                user.Password = _context.User.AsNoTracking().SingleOrDefault(m => m.UserId == id).Password;
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }catch (DbUpdateConcurrencyException)
            {
                if (!UserExits(id))
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

        //POST: api/Users
        [HttpPost]
        public async Task<IActionResult> PostUser([FromBody] User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User _user = _context.User.SingleOrDefault(x => x.Username == user.Username); //Since usernames are unqiue we can do this

            //If the username is already taken we return 400 with an error message
            if(_user != null)
            {
                ModelState.AddModelError("Error", "Username already taken");
                return BadRequest(ModelState);
            }

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        }

        //DELETE: api/Users/1
        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.User.SingleOrDefaultAsync(m => m.UserId == id);
            if(user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        //api/Users/Check/<username>
        [Authorize]
        [HttpGet("check/{username}")]
        public async Task<IActionResult> CheckForUsername([FromRoute] string username)
        {
            User _user = _context.User.SingleOrDefault(x => x.Username == username);

            if(_user != null) //If the username is already taken we return an error
            {
                ModelState.AddModelError("Error", "Username already taken");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        private bool UserExits(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }
    }
}
