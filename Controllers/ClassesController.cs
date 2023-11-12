using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Milieusysteem.Data;
using Milieusysteem.Models;
using NuGet.DependencyResolver;

namespace Milieusysteem.Controllers
{
    public class ClassesController : Controller
    {
        private readonly MilieuSysteemDb _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public ClassesController(MilieuSysteemDb context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _contextAccessor = httpContextAccessor;
        }

        // GET: Classes
        public async Task<IActionResult> Index()
        {
              return _context.Class != null ? 
                          View(await _context.Class.ToListAsync()) :
                          Problem("Entity set 'MilieuSysteemDb.Class'  is null.");
        }

        // GET: Classes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Class == null)
            {
                return NotFound();
            }

            var @class = await _context.Class
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // GET: Classes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Classes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Class @class)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@class);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@class);
        }

        // GET: Classes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Class == null)
            {
                return NotFound();
            }

            var @class = await _context.Class.FindAsync(id);
            if (@class == null)
            {
                return NotFound();
            }
            return View(@class);
        }

        // POST: Classes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        

        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Class @class)
        {
            if (id != @class.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    @class.TeacherId = int.Parse(_contextAccessor.HttpContext.Session.GetString("LoggedInTeacherId"));
                    _context.Update(@class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClassExists(@class.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                //Teacher teacher = _context.Teachers.FirstOrDefault(t => t.Id == int.Parse(_contextAccessor.HttpContext.Session.GetString("LoggedInTeacherId")));
                Teacher teacher = _context.Teachers.FirstOrDefault(t => t.Id == int.Parse(_contextAccessor.HttpContext.Session.GetString("LoggedInTeacherId")));
                List<Class> classes = new List<Class>();
                foreach (var Class in _context.Classes.ToList())
                {
                    if (Class.TeacherId == teacher.Id)
                    {
                        classes.Add(Class);
                    }
                }
                teacher.Classes = classes;
                return RedirectToAction("Log_In", "Home", teacher);
                //return View("TeacherHome", teacher);
                //return RedirectToAction("Log_In", "Home", teacher);
                //return View("Views/Home/TeacherHome", teacher);
            }

            return View(@class);
        }

        // GET: Classes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Class == null)
            {
                return NotFound();
            }

            var @class = await _context.Class
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@class == null)
            {
                return NotFound();
            }

            return View(@class);
        }

        // POST: Classes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Class == null)
            {
                return Problem("Entity set 'MilieuSysteemDb.Class'  is null.");
            }
            var @class = await _context.Class.FindAsync(id);
            if (@class != null)
            {
                _context.Class.Remove(@class);
            }
            
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));\
            Teacher teacher = _context.Teachers.FirstOrDefault(t => t.Id == int.Parse(_contextAccessor.HttpContext.Session.GetString("LoggedInTeacherId")));
            List<Class> classes = new List<Class>();
            foreach (var Class in _context.Classes.ToList())
            {
                if (Class.TeacherId == teacher.Id)
                {
                    classes.Add(Class);
                }
            }
            teacher.Classes = classes;
            return RedirectToAction("Log_In", "Home", teacher);
        }

        private bool ClassExists(int id)
        {
          return (_context.Class?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> ViewSurveyOutcomes(int? id)
        {
            if(id != null)
            {
                Class @class = _context.Classes.FirstOrDefault(x => x.Id == id);
                List<SurveyCounter> surveyCounters = _context.surveyCounters.ToList();
                
                
                foreach(SurveyCounter surveyCounter in surveyCounters)
                {
                    List<ClimateSurvey> climateSurveys = _context.climateSurveys.ToList();
                    List<Choice> choices = _context.choices.ToList();
                    List<ChoiceAmount> choiceAmounts = _context.ChoiceAmounts.ToList();
                    ClimateSurvey climateSurvey = new ClimateSurvey();
                    List<ClimateSurvey> finishedSurveys = new List<ClimateSurvey>();
                    List<Choice> surveyChoices = new List<Choice>();
                    if (surveyCounter.ClassId == id)
                    {
                        foreach (ClimateSurvey survey in climateSurveys)
                        {
                            if(survey.Id == surveyCounter.ClimateSurveyId)
                            {
                                climateSurvey = survey;
                                climateSurvey.SurveyCountId = surveyCounter.Id;
                                climateSurvey.ClassId = id;

                                foreach (Choice choice in choices)
                                {
                                    if (choice.ClimateSurveyId == climateSurvey.Id)
                                    {
                                        foreach (ChoiceAmount amount in choiceAmounts)
                                        {
                                            if (amount.ChoiceId == choice.Id & amount.SurveyCounterId == climateSurvey.SurveyCountId)
                                            {
                                                choice.AmountOfVotes = amount.ChoiceCount;
                                                climateSurvey.SurveyChoices.Add(choice);
                                                
                                            }
                                        }
                                    }
                                }

                                @class.FinishedSurveys.Add(climateSurvey);
                                
                            }
                        }
                    }
                    
                }
                

                return View(@class); 
            }
            return View();
        }
    }
}
