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
    public class SurveyCountersController : ControllerBase
    {
        private readonly MilieuSysteemDb _context;

        public SurveyCountersController(MilieuSysteemDb context)
        {
            _context = context;
        }

        // GET: api/SurveyCounters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SurveyCounter>>> GetsurveyCounters()
        {
          if (_context.surveyCounters == null)
          {
              return NotFound();
          }
            return await _context.surveyCounters.ToListAsync();
        }

        // GET: api/SurveyCounters/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SurveyCounter>> GetSurveyCounter(int id)
        {
          if (_context.surveyCounters == null)
          {
              return NotFound();
          }
            var surveyCounter = await _context.surveyCounters.FindAsync(id);

            if (surveyCounter == null)
            {
                return NotFound();
            }

            return surveyCounter;
        }

        // PUT: api/SurveyCounters/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSurveyCounter(int id, SurveyCounter surveyCounter)
        {
            if (id != surveyCounter.Id)
            {
                return BadRequest();
            }

            _context.Entry(surveyCounter).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SurveyCounterExists(id))
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

        // POST: api/SurveyCounters
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SurveyCounter>> PostSurveyCounter(SurveyCounter surveyCounter)
        {
          if (_context.surveyCounters == null)
          {
              return Problem("Entity set 'MilieuSysteemDb.surveyCounters'  is null.");
          }
            _context.surveyCounters.Add(surveyCounter);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSurveyCounter", new { id = surveyCounter.Id }, surveyCounter);
        }

        // DELETE: api/SurveyCounters/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSurveyCounter(int id)
        {
            if (_context.surveyCounters == null)
            {
                return NotFound();
            }
            var surveyCounter = await _context.surveyCounters.FindAsync(id);
            if (surveyCounter == null)
            {
                return NotFound();
            }

            _context.surveyCounters.Remove(surveyCounter);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SurveyCounterExists(int id)
        {
            return (_context.surveyCounters?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
