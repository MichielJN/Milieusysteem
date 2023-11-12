using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Editing;
using Microsoft.EntityFrameworkCore;
using Milieusysteem.Data;
using Milieusysteem.Models;
using Newtonsoft.Json;

namespace Milieusysteem.Controllers
{
    public class ClimateSurveysController : Controller
    {
        private readonly MilieuSysteemDb _context;
        private readonly IHttpContextAccessor _contextAccessor;

        public ClimateSurveysController(MilieuSysteemDb context, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _contextAccessor = contextAccessor;
        }

        // GET: ClimateSurveys
        public async Task<IActionResult> Index()
        {
              return _context.climateSurveys != null ? 
                          View(await _context.climateSurveys.ToListAsync()) :
                          Problem("Entity set 'MilieuSysteemDb.climateSurveys'  is null.");
        }

        // GET: ClimateSurveys/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.climateSurveys == null)
            {
                return NotFound();
            }

            var climateSurvey = await _context.climateSurveys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (climateSurvey == null)
            {
                return NotFound();
            }

            return View(climateSurvey);
        }

        // GET: ClimateSurveys/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ClimateSurveys/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SurveyQuestion,TeacherId,ClassId,SurveyCountId")] ClimateSurvey climateSurvey)
        {
            if (ModelState.IsValid)
            {
                List<ClimateSurvey> climateSurveys = _context.climateSurveys.ToList();
                if (climateSurveys.Any(x => x.SurveyQuestion == climateSurvey.SurveyQuestion) == false)
                {
                    climateSurvey.TeacherId = int.Parse(_contextAccessor.HttpContext.Session.GetString("LoggedInTeacherId"));

                    _context.Add(climateSurvey);
                    await _context.SaveChangesAsync();
                    List<ClimateSurvey> climatesurveys = _context.climateSurveys.ToList();
                    ClimateSurvey _survey = _context.climateSurveys.FirstOrDefault(survey => survey.SurveyQuestion == climateSurvey.SurveyQuestion);
                    _contextAccessor.HttpContext.Session.SetString("SurveyId", _survey.Id.ToString());

                    return RedirectToAction("Create", "Choices");
                }
                else
                {
                    return View("Create");
                }
            }
            return View(climateSurvey);
        }

        // GET: ClimateSurveys/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.climateSurveys == null)
            {
                return NotFound();
            }

            var climateSurvey = await _context.climateSurveys.FindAsync(id);
            if (climateSurvey == null)
            {
                return NotFound();
            }
            return View(climateSurvey);
        }

        // POST: ClimateSurveys/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SurveyQuestion,TeacherId,ClassId,SurveyCountId")] ClimateSurvey climateSurvey)
        {
            if (id != climateSurvey.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(climateSurvey);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClimateSurveyExists(climateSurvey.Id))
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
            return View(climateSurvey);
        }

        // GET: ClimateSurveys/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.climateSurveys == null)
            {
                return NotFound();
            }

            var climateSurvey = await _context.climateSurveys
                .FirstOrDefaultAsync(m => m.Id == id);
            if (climateSurvey == null)
            {
                return NotFound();
            }

            return View(climateSurvey);
        }

        // POST: ClimateSurveys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.climateSurveys == null)
            {
                return Problem("Entity set 'MilieuSysteemDb.climateSurveys'  is null.");
            }
            var climateSurvey = await _context.climateSurveys.FindAsync(id);
            if (climateSurvey != null)
            {
                _context.climateSurveys.Remove(climateSurvey);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClimateSurveyExists(int id)
        {
          return (_context.climateSurveys?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        public async Task<IActionResult> ViewSurveys()
        {
            Teacher teacher = _context.Teachers.FirstOrDefault(x => x.Id == int.Parse(_contextAccessor.HttpContext.Session.GetString("LoggedInTeacherId")));
            List<ClimateSurvey> climateSurveys = new List<ClimateSurvey>();
            foreach(ClimateSurvey _climateSurvey in _context.climateSurveys.ToList())
            {
                if(_climateSurvey.TeacherId == teacher.Id)
                {
                    climateSurveys.Add(_climateSurvey);
                }
            }
            teacher.ClimateSurveys = climateSurveys;
            return View(teacher);
        }
        
        public async Task<IActionResult> ViewQuestions()
        {
            return View();
        }

        public async Task<IActionResult> SelectSurvey(int? id)
        {
            if(id != null)
            {
                _contextAccessor.HttpContext.Session.SetString("ClassId", id.ToString());
                return View(_context.climateSurveys.ToList());
            }

            return View();
        }
        public async Task<IActionResult> DoSurvey(int? id)
        {
            if(id != null)
            {
                _contextAccessor.HttpContext.Session.SetInt32("VoteAmount", 0);
                ClimateSurvey climateSurvey = _context.climateSurveys.FirstOrDefault(x => x.Id == id);
                List<Choice> choices = new List<Choice>();
                foreach(Choice choice in _context.choices.ToList())
                {
                    if (choice.ClimateSurveyId == id)
                    {
                        choices.Add(choice);  
                    }
                }
                _contextAccessor.HttpContext.Session.SetString("ChoiceCounter", JsonConvert.SerializeObject(choices));
                SurveyCounter surveyCounter = new SurveyCounter();
                surveyCounter.ClimateSurveyId = climateSurvey.Id;
                surveyCounter.ClassId = int.Parse(_contextAccessor.HttpContext.Session.GetString("ClassId"));
                _context.surveyCounters.Add(surveyCounter);
                await _context.SaveChangesAsync();
                climateSurvey.SurveyCountId = _context.surveyCounters.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                foreach(Choice choice in choices)
                {
                    ChoiceAmount choiceAmount = new ChoiceAmount();
                    choiceAmount.SurveyCounterId = (int)climateSurvey.SurveyCountId;
                    choiceAmount.ChoiceId = choice.Id;
                    _context.ChoiceAmounts.Add(choiceAmount);
                    await _context.SaveChangesAsync();
                }
                climateSurvey.SurveyChoices = choices;
                return View(climateSurvey);
            }
            return View();
        }

        public async Task<IActionResult> Vote(int? id)
        {
            if(id != null)
            {
                int voteAmount = (int)_contextAccessor.HttpContext.Session.GetInt32("VoteAmount") + 1;
                _contextAccessor.HttpContext.Session.SetInt32("VoteAmount", voteAmount);
                int climateSurveyCountId = _context.surveyCounters.OrderByDescending(x => x.Id).FirstOrDefault().Id;
                foreach(ChoiceAmount choiceAmount in _context.ChoiceAmounts)
                {
                    if (choiceAmount.SurveyCounterId == climateSurveyCountId && choiceAmount.ChoiceId == id)
                    {
                        choiceAmount.ChoiceCount += 1;
                        _context.ChoiceAmounts.Update(choiceAmount);
                        
                    }
                }
                await _context.SaveChangesAsync();
                List<SurveyCounter> surveyCounters = _context.surveyCounters.ToList();
                surveyCounters.OrderByDescending(x => x.Id);

                SurveyCounter surveyCounter = surveyCounters[surveyCounters.Count - 1];
                ClimateSurvey climateSurvey = _context.climateSurveys.FirstOrDefault(x => x.Id == surveyCounter.ClimateSurveyId);
                List<Choice> choices = new List<Choice>();
                foreach (Choice choice in _context.choices.ToList())
                {
                    
                    if (choice.ClimateSurveyId == climateSurvey.Id)
                    {
                        choices.Add(choice);
                    }
                }
                climateSurvey.SurveyChoices = choices;
                return View("DoSurvey", climateSurvey);
            }
            return View();
        }
        public async Task<IActionResult> ViewVoteResults()
        {
            ClimateSurvey climateSurvey = _context.climateSurveys.FirstOrDefault(x => x.Id == _context.surveyCounters.OrderByDescending(X => X.Id).FirstOrDefault().ClimateSurveyId);
            climateSurvey.SurveyCountId = _context.surveyCounters.OrderByDescending(X => X.Id).FirstOrDefault().Id;
            List<Choice> choices = new List<Choice>();
            foreach (Choice choice in _context.choices.ToList())
            {

                if (choice.ClimateSurveyId == climateSurvey.Id)
                {
                    foreach (ChoiceAmount choiceAmount in _context.ChoiceAmounts)
                    {
                        if (choiceAmount.SurveyCounterId == climateSurvey.SurveyCountId && choiceAmount.ChoiceId == choice.Id)
                        {
                            choice.AmountOfVotes = choiceAmount.ChoiceCount;
                        }
                    }
                    choices.Add(choice);
                }
            }
            climateSurvey.SurveyChoices = choices;
            return View(climateSurvey);
        }
        public async Task<IActionResult> SurveyOutcomesToTeacherHome()
        {
            Teacher teacher = _context.Teachers.FirstOrDefault(x => x.Id == int.Parse(_contextAccessor.HttpContext.Session.GetString("LoggedInTeacherId")));
            return RedirectToAction("Log_in", "Home", teacher);
        }
    }
}
