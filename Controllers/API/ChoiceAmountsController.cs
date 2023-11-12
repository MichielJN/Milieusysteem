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
    public class ChoiceAmountsController : ControllerBase
    {
        private readonly MilieuSysteemDb _context;

        public ChoiceAmountsController(MilieuSysteemDb context)
        {
            _context = context;
        }

        // GET: api/ChoiceAmounts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChoiceAmount>>> GetChoiceAmounts()
        {
          if (_context.ChoiceAmounts == null)
          {
              return NotFound();
          }
            return await _context.ChoiceAmounts.ToListAsync();
        }

        // GET: api/ChoiceAmounts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ChoiceAmount>> GetChoiceAmount(int id)
        {
          if (_context.ChoiceAmounts == null)
          {
              return NotFound();
          }
            var choiceAmount = await _context.ChoiceAmounts.FindAsync(id);

            if (choiceAmount == null)
            {
                return NotFound();
            }

            return choiceAmount;
        }

        // PUT: api/ChoiceAmounts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutChoiceAmount(int id, ChoiceAmount choiceAmount)
        {
            if (id != choiceAmount.Id)
            {
                return BadRequest();
            }

            _context.Entry(choiceAmount).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChoiceAmountExists(id))
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

        // POST: api/ChoiceAmounts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChoiceAmount>> PostChoiceAmount(ChoiceAmount choiceAmount)
        {
          if (_context.ChoiceAmounts == null)
          {
              return Problem("Entity set 'MilieuSysteemDb.ChoiceAmounts'  is null.");
          }
            _context.ChoiceAmounts.Add(choiceAmount);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetChoiceAmount", new { id = choiceAmount.Id }, choiceAmount);
        }

        // DELETE: api/ChoiceAmounts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteChoiceAmount(int id)
        {
            if (_context.ChoiceAmounts == null)
            {
                return NotFound();
            }
            var choiceAmount = await _context.ChoiceAmounts.FindAsync(id);
            if (choiceAmount == null)
            {
                return NotFound();
            }

            _context.ChoiceAmounts.Remove(choiceAmount);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChoiceAmountExists(int id)
        {
            return (_context.ChoiceAmounts?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
