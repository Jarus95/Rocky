using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cms.Ecc;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryController(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public IActionResult Index()
        {
            var catergories = dbContext.Category.ToList();
            return View(catergories);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
               dbContext.Category.Add(category);
               dbContext.SaveChanges();
               return RedirectToAction("Index");

            }
            return View(category);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var category = dbContext.Category.Find(id);
            if(category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(Category? _category)
        {
            if (_category == null)
            {
                return NotFound();
            }

            dbContext.Category.Update(_category);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if(id == null || id == 0)
            {
                return NotFound();
            }

            var category = dbContext.Category.Find(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }


        [HttpPost]
        public IActionResult Delete(Category _category)
        {
            if (_category == null)
            {
                return NotFound();
            }

            dbContext.Category.Remove(_category);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}
