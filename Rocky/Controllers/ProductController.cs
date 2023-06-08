using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public ProductController(ApplicationDbContext _dbContext)
        {
            dbContext = _dbContext;
        }


        public IActionResult Index()
        {
            var product = dbContext.Product.ToList();

            foreach (var item in product)
            {
                item.Category = dbContext.Category.FirstOrDefault(p=>p.Id == item.CategoryId);
            }

            return View(product);
        }


        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CategoryDropDown = dbContext.Category.Select(i=> new SelectListItem 
            { 
                Text = i.Name, 
                Value = i.Id.ToString()

            });
            ViewBag.CategoryDropDown = CategoryDropDown;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                dbContext.Product.Add(product);
                dbContext.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(product);
        }
    }
}
