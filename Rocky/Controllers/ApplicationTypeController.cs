using Microsoft.AspNetCore.Mvc;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class ApplicationTypeController : Controller
    {
        public readonly  ApplicationDbContext dbContext;

        public ApplicationTypeController(ApplicationDbContext applicationDb)
        {
            dbContext = applicationDb;
        }
        public IActionResult Index()
        {
            var appdb = dbContext.ApplicationType.ToList();
            return View(appdb);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(ApplicationType applicationType)
        {
            dbContext.ApplicationType.Add(applicationType);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
