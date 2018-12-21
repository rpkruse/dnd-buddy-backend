using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using dnd_buddy_backend.Models;
using System;

namespace dnd_buddy_backend.Controllers
{
    [Produces("application/json")]
    [Route("api/Grids")]
    public class GridController : Controller
    {
        private readonly DataContext _context;

        public GridController(DataContext context)
        {
            _context = context;
        }

        //GET: api/Grid
        [HttpGet("")]
        public IEnumerable<Grid> GetGrid()
        {
            var grids = _context.Grid;

            foreach (Grid grid in grids)
            {
                grid.GridData = null;
            }

            return grids;
        }

        //Get: api/Grid/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGrid([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var grid = await _context.Grid.SingleOrDefaultAsync(m => m.GridId == id);

            if (grid == null)
            {
                return NotFound();
            }

            return Ok(grid);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutGrid([FromRoute] int id, [FromBody] Grid grid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != grid.GridId)
            {
                return BadRequest();
            }

            _context.Entry(grid).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            } catch (DbUpdateException)
            {
                if (!GridExists(id))
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

        [HttpPost]
        public async Task<IActionResult> PostGrid([FromBody] Grid grid)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Grid.Add(grid);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGrid", new { id = grid.GridId }, grid);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGrid([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var grid = await _context.Grid.SingleOrDefaultAsync(m => m.GridId == id);
            if (grid == null)
            {
                return NotFound();
            }

            _context.Grid.Remove(grid);
            await _context.SaveChangesAsync();

            return Ok(grid);
        }

        private bool GridExists(int id)
        {
            return _context.Grid.Any(e => e.GridId == id);
        }
    }
}
