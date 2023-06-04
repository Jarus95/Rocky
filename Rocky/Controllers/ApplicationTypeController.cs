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
            if (ModelState.IsValid)
            {
               dbContext.ApplicationType.Add(applicationType);
               dbContext.SaveChanges();
               return RedirectToAction("Index");

            }

            return View(applicationType);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var type = dbContext.ApplicationType.Find(id);
            if (type == null)
            {
                return NotFound();
            }

            return View(type);
        }

        [HttpPost]
        public IActionResult Edit(ApplicationType? appType)
        {
            if (ModelState.IsValid)
            {
                if (appType == null)
                {
                    return NotFound();
                }

                dbContext.ApplicationType.Update(appType);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appType);
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var appType = dbContext.ApplicationType.Find(id);
            if (appType == null)
            {
                return NotFound();
            }

            return View(appType);
        }


        [HttpPost]
        public IActionResult Delete(ApplicationType appType)
        {
            if (appType == null)
            {
                return NotFound();
            }

            dbContext.ApplicationType.Remove(appType);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
