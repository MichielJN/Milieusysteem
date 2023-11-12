using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Milieusysteem.Data;
using Milieusysteem.Models;
using Newtonsoft.Json;

namespace Milieusysteem.Controllers
{
    public class ChoicesController : Controller
    {
        private readonly MilieuSysteemDb _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private List<Choice> choices;

        public ChoicesController(MilieuSysteemDb context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
            if (_contextAccessor.HttpContext.Session.Keys.Contains("Choices"))
            {
                choices = JsonConvert.DeserializeObject<List<Choice>>(_contextAccessor.HttpContext.Session.GetString("Choices"));
            }
        }

        // GET: Choices
        public async Task<IActionResult> Index()
        {
              return _context.choices != null ? 
                          View(await _context.choices.ToListAsync()) :
                          Problem("Entity set 'MilieuSysteemDb.choices'  is null.");
        }

        // GET: Choices/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.choices == null)
            {
                return NotFound();
            }

            var choice = await _context.choices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (choice == null)
            {
                return NotFound();
            }

            return View(choice);
        }

        // GET: Choices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Choices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ClimateSurveyId,ChoiceTekst,Advice,AmountOfVotes")] Choice choice)
        {
            if (ModelState.IsValid)
            {
                
                List<Choice> choices = _context.choices.ToList();
                choice.ClimateSurveyId = int.Parse(_contextAccessor.HttpContext.Session.GetString("SurveyId"));
                
                //List<Choice> choices = _context.choices.ToList();
               
                if (_context.choices.ToList().Any(_choice => _choice.ClimateSurveyId == choice.ClimateSurveyId & _choice.ChoiceTekst == choice.ChoiceTekst) == false)
                {
                    _context.Add(choice);
                    await _context.SaveChangesAsync();
                    List<Choice> currentChoices = new List<Choice>();
                    foreach (Choice choice1 in _context.choices.ToList())
                    {
                        if (choice1.ClimateSurveyId == int.Parse(_contextAccessor.HttpContext.Session.GetString("SurveyId")))
                        {
                            currentChoices.Add(choice1);
                        }
                    }
                    Choice choice2 = new Choice();
                    choice2.Choices = currentChoices;
                    return View("Create", choice2);
                }
                else
                { 
                
              //  _context.Add(choice);

               // await _context.SaveChangesAsync();
                List<Choice> currentChoices = new List<Choice>();
                foreach(Choice choice1 in _context.choices.ToList())
                    {
                        if(choice1.ClimateSurveyId == int.Parse(_contextAccessor.HttpContext.Session.GetString("SurveyId")))
                        {
                            currentChoices.Add(choice1);
                        }
                    }
                Choice choice2 = new Choice();
                    choice2.Choices = currentChoices;
                return View("Create", choice2);
                     }
            }
            return View(choice);
            
        }

        // GET: Choices/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.choices == null)
            {
                return NotFound();
            }

            var choice = await _context.choices.FindAsync(id);
            if (choice == null)
            {
                return NotFound();
            }
            return View(choice);
        }

        // POST: Choices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ClimateSurveyId,ChoiceTekst,Advice,AmountOfVotes")] Choice choice)
        {
            if (id != choice.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(choice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChoiceExists(choice.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(choice);
        }

        // GET: Choices/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.choices == null)
            {
                return NotFound();
            }

            var choice = await _context.choices
                .FirstOrDefaultAsync(m => m.Id == id);
            if (choice == null)
            {
                return NotFound();
            }

            return View(choice);
        }

        // POST: Choices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.choices == null)
            {
                return Problem("Entity set 'MilieuSysteemDb.choices'  is null.");
            }
            var choice = await _context.choices.FindAsync(id);
            if (choice != null)
            {
                _context.choices.Remove(choice);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChoiceExists(int id)
        {
          return (_context.choices?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IActionResult ConfirmSurvey()
        {
            Teacher teacher = _context.Teachers.FirstOrDefault(x => x.Id == int.Parse(_contextAccessor.HttpContext.Session.GetString("LoggedInTeacherId")));
            _contextAccessor.HttpContext.Session.Remove("SurveyId");
            return RedirectToAction("Log_in", "Home", teacher);
        }
    }
}
