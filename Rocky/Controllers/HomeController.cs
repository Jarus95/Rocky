using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rocky.Data;
using Rocky.Models;
using Rocky.Models.ViewModels;
using Rocky.Utility;
using System.Diagnostics;

namespace Rocky.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext dbContext;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext _db)
        {
            _logger = logger;
            dbContext = _db;
        }

        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = dbContext.Product.Include(x => x.Category).Include(x => x.ApplicationType),
                Categories = dbContext.Category
            };
            return View(homeVM);
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            List<ShoppinCart> shoppinCarts = new List<ShoppinCart>();
            if (HttpContext.Session.Get<List<ShoppinCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppinCart>>(WC.SessionCart).Count() > 0)
            {
                shoppinCarts = HttpContext.Session.Get<List<ShoppinCart>>(WC.SessionCart);
            }

            DetailsVM detailsVM = new DetailsVM
            {
                Product = dbContext.Product.Include(y => y.Category).Include(y => y.ApplicationType).Where(x => x.Id == id).FirstOrDefault(),
                IsOnCart = false
            };

            foreach (var item in shoppinCarts)
            {
                if(item.ProductId==id)
                {
                    detailsVM.IsOnCart = true;
                }    
            }

            return View(detailsVM);
        }

        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id)
        {
            List<ShoppinCart> shoppinCarts = new List<ShoppinCart>();
            if(HttpContext.Session.Get<List<ShoppinCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<List<ShoppinCart>>(WC.SessionCart).Count() > 0)
            {
                shoppinCarts = HttpContext.Session.Get<List<ShoppinCart>>(WC.SessionCart);
            }
            shoppinCarts.Add(new ShoppinCart { ProductId = id});
            HttpContext.Session.Set(WC.SessionCart, shoppinCarts);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Privacy()
        {
            return View();
        }

     


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}