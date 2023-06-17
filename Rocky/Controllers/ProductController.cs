using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }


            var product = dbContext.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            ProductVM pv = new ProductVM();
            pv.selectListItems = dbContext.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()

            });
            pv.Product = product;

            return View(pv);
        }

        [HttpPost]
        public IActionResult Edit(ProductVM? productVM)
        {
            
                if (productVM == null)
                {
                    return NotFound();
                }

                var obj = dbContext.Product.AsNoTracking().FirstOrDefault(u=>u.Id == productVM.Product.Id);
                if (obj == null)
                {
                    return NotFound();
                }
                var files = HttpContext.Request.Form.Files;

                if (files.Count > 0)
                {
                   string webRootPath = webHostEnvironment.WebRootPath;
                   string upload = Path.Combine(webRootPath, WC.ImagePath);
                   string fileName = Guid.NewGuid().ToString();
                   string extension = Path.GetExtension(files[0].FileName);

                   var oldFile = Path.Combine(upload, obj.Image);
                   if (System.IO.File.Exists(oldFile))
                   {
                       System.IO.File.Delete(oldFile);
                   }
                   using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                   {
                       files[0].CopyTo(fileStream);
                   }

                   productVM.Product.Image = fileName + extension;
                }

              else
              {
                   productVM.Product.Image = obj.Image;
              }

                dbContext.Product.Update(productVM.Product);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            

         
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //var product = dbContext.Product.Include(u => u.CategoryId).FirstOrDefault(u => u.Id == id)
            var product = dbContext.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            ProductVM pv = new ProductVM();
            pv.selectListItems = dbContext.Category.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()

            });
            pv.Product = product;

            return View(product);
        }

        [HttpPost]
        public IActionResult Delete(ProductVM? productVM)
        {

            if (productVM == null)
            {
                return NotFound();
            }

            var obj = dbContext.Product.AsNoTracking().FirstOrDefault(u => u.Id == productVM.Product.Id);
            if (obj == null)
            {
                return NotFound();
            }


           
                string webRootPath = webHostEnvironment.WebRootPath;
                string upload = Path.Combine(webRootPath, WC.ImagePath);

                var oldFile = Path.Combine(upload, obj.Image);
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
                
            

          

            dbContext.Product.Remove(obj);
            dbContext.SaveChanges();
            return RedirectToAction("Index");



        }

    }
}
