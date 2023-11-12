using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Milieusysteem.Data;
using Milieusysteem.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using NuGet.DependencyResolver;

namespace Milieusysteem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly MilieuSysteemDb _context;
        private readonly IHttpContextAccessor _sessionStorage;
        

        public HomeController(ILogger<HomeController> logger, MilieuSysteemDb context, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _sessionStorage = httpContextAccessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CreateAccount()
        {
            return View();
        }

        public async Task<IActionResult> Create([Bind("Id,Email,Password")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                List<Teacher> teachers = _context.Teachers.ToList();
                if (teachers.Any(contextTeacher => contextTeacher.Email == teacher.Email))
                {
                    return View("CreateAccount");
                }
            }
            _context.Add(teacher);
            await _context.SaveChangesAsync();
            //return RedirectToAction(nameof(Index));
            
            
            return View("Index");
            
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Log_in([Bind("Id,Email,Password")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                List<Teacher> teachers = _context.Teachers.ToList();
                if (teachers.Any(contextTeacher => contextTeacher.Email == teacher.Email & contextTeacher.Password == teacher.Password))
                {
                    Teacher _teacher = _context.Teachers.FirstOrDefault(x => x.Email == teacher.Email);
                    _sessionStorage.HttpContext.Session.SetString("LoggedInTeacherId", _teacher.Id.ToString());
                    List<Class> classes = new List<Class>();
                    foreach(var Class in _context.Classes.ToList())
                    {
                        if(Class.TeacherId == _teacher.Id)
                        {
                            classes.Add(Class);
                        }
                    }
                    _teacher.Classes = classes;
                    return View("TeacherHome", _teacher);
                }
            }
            return View("index");
        }
        public IActionResult CreateClass()
        {
            return View();
        }

        public async Task<IActionResult> CreateClassButton([Bind("Id,Name")] Class @class)
        {
            if (ModelState.IsValid)
            {
                @class.TeacherId = int.Parse(_sessionStorage.HttpContext.Session.GetString("LoggedInTeacherId"));
                _context.Add(@class);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                Teacher teacher = _context.Teachers.FirstOrDefault(t => t.Id == int.Parse(_sessionStorage.HttpContext.Session.GetString("LoggedInTeacherId")));
                teacher.Classes = _context.Classes.ToList();
                return View("TeacherHome", teacher);
            }
            Teacher _teacher = _context.Teachers.FirstOrDefault(t => t.Id == int.Parse(_sessionStorage.HttpContext.Session.GetString("LoggedInTeacherId")));
            _teacher.Classes = _context.Classes.ToList();
            return View("TeacherHome", _teacher);
            //return View(@class);
        }
        public IActionResult ClassPage()
        {
            return View();
        }

        public IActionResult CreateSurvey()
        {
            return RedirectToAction("Create", "ClimateSurveys");
        }

        public async Task<IActionResult> DoSurvey(int? id)
        {
            if (id != null)
            {

                Class _class = _context.Classes.FirstOrDefault(t => t.Id == id);
                _sessionStorage.HttpContext.Session.SetString("ClassId", id.ToString());
                return RedirectToAction("SelectSurvey", "ClimateSurveys", _class);
            }
            else { return View(); }
        }
    }
}