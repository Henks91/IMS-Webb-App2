using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PäronWebbApp.Data;
using PäronWebbApp.Models;
using System.Diagnostics;

namespace PäronWebbApp.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}
        private readonly AppDbContext _context;
                
        public HomeController(AppDbContext context)
        {
            _context = context;
        }


        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Intranet()
        {
            return View();
        }
        public IActionResult Product()
        {
            return View();
        }
        public IActionResult Transactions()
        {
            return View();
        }
        public IActionResult IntentoryBalance()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.inventoryBalances.Include(i => i.Product).Include(i => i.Warehouse);
            return View(await appDbContext.ToListAsync());
        }
    }
}