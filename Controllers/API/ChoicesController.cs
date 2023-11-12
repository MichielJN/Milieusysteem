using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Milieusysteem.Data;
using Milieusysteem.Models;

namespace Milieusysteem.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChoicesController : ControllerBase
    {
        private readonly MilieuSysteemDb _context;

        public ChoicesController(MilieuSysteemDb context)
        {
            _context = context;
        }

        // GET: api/Choices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Choice>>> Getchoices()
        {
          if (_context.choices == null)
          {
              return NotFound();
          }
            return await _context.choices.ToListAsync();
        }

        // GET: api/Choices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Choice>> GetChoice(int id)
        {
          if (_context.choices == null)
          {
              return NotFound();
          }
            var choice = await _context.choices.FindAsync(id);

            if (choice == null)
            {
                return NotFound();
            }

            return choice;
        }

        // PUT: api/Choices/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChoice(int id, Choice choice)
        {
            if (id != choice.Id)
            {
                return BadRequest();
            }

            _context.Entry(choice).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChoiceExists(id))
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

        // POST: api/Choices
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Choice>> PostChoice(Choice choice)
        {
          if (_context.choices == null)
          {
              return Problem("Entity set 'MilieuSysteemDb.choices'  is null.");
          }
            _context.choices.Add(choice);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChoice", new { id = choice.Id }, choice);
        }

        // DELETE: api/Choices/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChoice(int id)
        {
            if (_context.choices == null)
            {
                return NotFound();
            }
            var choice = await _context.choices.FindAsync(id);
            if (choice == null)
            {
                return NotFound();
            }

            _context.choices.Remove(choice);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChoiceExists(int id)
        {
            return (_context.choices?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
