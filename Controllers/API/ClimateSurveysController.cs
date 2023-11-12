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
    public class ClimateSurveysController : ControllerBase
    {
        private readonly MilieuSysteemDb _context;

        public ClimateSurveysController(MilieuSysteemDb context)
        {
            _context = context;
        }

        // GET: api/ClimateSurveys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClimateSurvey>>> GetclimateSurveys()
        {
          if (_context.climateSurveys == null)
          {
              return NotFound();
          }
            return await _context.climateSurveys.ToListAsync();
        }

        // GET: api/ClimateSurveys/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClimateSurvey>> GetClimateSurvey(int id)
        {
          if (_context.climateSurveys == null)
          {
              return NotFound();
          }
            var climateSurvey = await _context.climateSurveys.FindAsync(id);

            if (climateSurvey == null)
            {
                return NotFound();
            }

            return climateSurvey;
        }

        // PUT: api/ClimateSurveys/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClimateSurvey(int id, ClimateSurvey climateSurvey)
        {
            if (id != climateSurvey.Id)
            {
                return BadRequest();
            }

            _context.Entry(climateSurvey).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClimateSurveyExists(id))
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

        // POST: api/ClimateSurveys
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ClimateSurvey>> PostClimateSurvey(ClimateSurvey climateSurvey)
        {
          if (_context.climateSurveys == null)
          {
              return Problem("Entity set 'MilieuSysteemDb.climateSurveys'  is null.");
          }
            _context.climateSurveys.Add(climateSurvey);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClimateSurvey", new { id = climateSurvey.Id }, climateSurvey);
        }

        // DELETE: api/ClimateSurveys/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClimateSurvey(int id)
        {
            if (_context.climateSurveys == null)
            {
                return NotFound();
            }
            var climateSurvey = await _context.climateSurveys.FindAsync(id);
            if (climateSurvey == null)
            {
                return NotFound();
            }

            _context.climateSurveys.Remove(climateSurvey);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClimateSurveyExists(int id)
        {
            return (_context.climateSurveys?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
