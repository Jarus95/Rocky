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


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            dbContext.Category.Add(category);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var catergories = dbContext.Category.ToList();
            return View(catergories);
        }
    }
}
