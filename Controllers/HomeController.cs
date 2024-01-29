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
        public IActionResult Index(string sortOrder)
        {
            ViewData["WarehouseSortParam"] = string.IsNullOrEmpty(sortOrder) ? "warehouse_desc" : "";
            ViewData["ProductSortParam"] = sortOrder == "product" ? "product_desc" : "product";
            ViewData["TotalAmountSortParam"] = sortOrder == "totalAmount" ? "totalAmount_desc" : "totalAmount";

            var inventoryBalances = _context.inventoryBalances
                                    .Include(i => i.Warehouse)
                                    .Include(i => i.Product)
                                    .AsQueryable();

            switch (sortOrder)
            {
                case "warehouse_desc":
                    inventoryBalances = inventoryBalances.OrderByDescending(i => i.Warehouse.City);
                    break;
                case "product":
                    inventoryBalances = inventoryBalances.OrderBy(i => i.Product.ProductName);
                    break;
                case "product_desc":
                    inventoryBalances = inventoryBalances.OrderByDescending(i => i.Product.ProductName);
                    break;
                case "totalAmount":
                    inventoryBalances = inventoryBalances.OrderBy(i => i.TotalAmount);
                    break;
                case "totalAmount_desc":
                    inventoryBalances = inventoryBalances.OrderByDescending(i => i.TotalAmount);
                    break;
                default:
                    inventoryBalances = inventoryBalances.OrderBy(i => i.Warehouse.City);
                    break;
            }

            return View(inventoryBalances.ToList());
        }
        //public IActionResult Index(string sortOrder)
        //{
        //    ViewData["WarehouseSortParam"] = string.IsNullOrEmpty(sortOrder) ? "warehouse_desc" : "";
        //    ViewData["ProductSortParam"] = sortOrder == "product" ? "product_desc" : "product";
        //    ViewData["TotalAmountSortParam"] = sortOrder == "totalAmount" ? "totalAmount_desc" : "totalAmount";

        //    var inventoryBalances = _context.inventoryBalances.AsQueryable();

        //    switch (sortOrder)
        //    {
        //        case "warehouse_desc":
        //            inventoryBalances = inventoryBalances.OrderByDescending(i => i.Warehouse.City);
        //            break;
        //        case "product":
        //            inventoryBalances = inventoryBalances.OrderBy(i => i.Product.ProductName);
        //            break;
        //        case "product_desc":
        //            inventoryBalances = inventoryBalances.OrderByDescending(i => i.Product.ProductName);
        //            break;
        //        case "totalAmount":
        //            inventoryBalances = inventoryBalances.OrderBy(i => i.TotalAmount);
        //            break;
        //        case "totalAmount_desc":
        //            inventoryBalances = inventoryBalances.OrderByDescending(i => i.TotalAmount);
        //            break;
        //        default:
        //            inventoryBalances = inventoryBalances.OrderBy(i => i.Warehouse.City);
        //            break;
        //    }

        //    return View(inventoryBalances.ToList());
        //}

    }
}