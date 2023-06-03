using Microsoft.AspNetCore.Mvc;
using Rocky.Data;

namespace Rocky.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryController(ApplicationDbContext _dbContext)
        {
             dbContext = _dbContext;
        }


        //Get
        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Index()
        {
            var catergories = dbContext.Category.ToList();
            return View(catergories);
        }
    }
}
