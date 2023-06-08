using Microsoft.AspNetCore.Mvc;
using Rocky.Data;

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
    }
}
