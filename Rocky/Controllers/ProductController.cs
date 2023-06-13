using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rocky.Data;
using Rocky.Models;

namespace Rocky.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;
        public ProductController(ApplicationDbContext _dbContext, IWebHostEnvironment _webHostEnvironment)
        {
            dbContext = _dbContext;
            webHostEnvironment = _webHostEnvironment;
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

            //IEnumerable<SelectListItem> CategoryDropDown = dbContext.Category.Select(i => new SelectListItem
            //{
            //    Text = i.Name,
            //    Value = i.Id.ToString()

            //});
            //ViewBag.CategoryDropDown = CategoryDropDown;
            ProductVM productVM = new ProductVM();
            productVM.Product = new Product();
            productVM.selectListItems = dbContext.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()

            });
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM pv)
        {
            
                var files = HttpContext.Request.Form.Files;
                string webRootPath = webHostEnvironment.WebRootPath;
                string upload = Path.Combine(webRootPath, WC.ImagePath);
                string fileName = Guid.NewGuid().ToString();
                string extension = Path.GetExtension(files[0].FileName);

                using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(fileStream);
                }

                pv.Product.Image = fileName + extension;
                dbContext.Product.Add(pv.Product);
                dbContext.SaveChanges();
                return RedirectToAction("Index");

  
        }
    }
}
