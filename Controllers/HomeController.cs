
using D.Infrastructure;
using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace D.Controllers
{
    
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class HomeController : Controller
    {
        private IdbInterface db;
        
        public HomeController(IdbInterface dbParam)
        {
            db = dbParam;
           
        }
        
        [CustomAuthorize]
        public  ActionResult Index()
        {

            ViewBag.ProductCount = db.Products.AsNoTracking().Count();
            ViewBag.CustomerCount = db.CustomerEnts.AsNoTracking().Count()+ db.CustomerInds.AsNoTracking().Count();
            ViewBag.SupplierCount = db.Suppliers.AsNoTracking().Count();
            ViewBag.EmployeeCount = db.Employees.AsNoTracking().Count();
            ViewBag.OrderCount = db.Orders.AsNoTracking().Count();
            

            
            ViewBag.ProductCost = db.Products
                .AsNoTracking()
                .Sum(g => g.Balance * g.Price).GetValueOrDefault().ToString("N");

            ViewBag.ProductWeight= (db.Products
                .AsNoTracking().AsEnumerable()
                .Sum(g => (decimal)(g.Balance)* g.Weight) / 1000000).GetValueOrDefault().ToString("N");


            ViewBag.OrderAmountLastYear = db.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate.Value.Year == DateTime.Now.Year - 1)
                .Select(o => o.AmountVat).Sum().GetValueOrDefault().ToString("N");

            ViewBag.OrderCountLastYear= db.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate.Value.Year == DateTime.Now.Year - 1)
                .Count();

            ViewBag.OrderAmountYear = db.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate.Value.Year == DateTime.Now.Year )
                .Select(o => o.AmountVat).Sum().GetValueOrDefault().ToString("N");

            ViewBag.OrderCountYear = db.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate.Value.Year == DateTime.Now.Year )
                .Count();


            ViewBag.OrderAmountLastMonth= db.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate.Value.Month == DateTime.Now.Month-1)
                .Select(o => o.AmountVat).Sum().GetValueOrDefault().ToString("N");

            ViewBag.OrderCountLastMonth= db.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate.Value.Month == DateTime.Now.Month - 1)
                .Count();

            ViewBag.OrderAmountMonth= db.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate.Value.Month == DateTime.Now.Month )
                .Select(o => o.AmountVat).Sum().GetValueOrDefault().ToString("N");
           
            ViewBag.OrderCountMonth = db.Orders
                .AsNoTracking()
                .Where(o => o.OrderDate.Value.Month == DateTime.Now.Month )
                .Count();

            ViewBag.ReceiptAmountLastMonth= db.MoneyReceipts
                .AsNoTracking()
                .Where(o => o.ReceiptDate.Value.Month == DateTime.Now.Month-1 && o.ReceiptDate.Value.Year == DateTime.Now.Year)
                .Select(o => o.Amount).Sum().GetValueOrDefault().ToString("N");

            
            ViewBag.ReceiptCountLastMonth = db.MoneyReceipts
                .AsNoTracking()
                .Where(o => o.ReceiptDate.Value.Month == DateTime.Now.Month - 1 && o.ReceiptDate.Value.Year == DateTime.Now.Year)
                .Count();


            ViewBag.o2m1= db.MoneyReceipts
                .AsNoTracking()
                .Where(o => o.ReceiptDate.Value.Month == DateTime.Now.Month && o.ReceiptDate.Value.Year == DateTime.Now.Year)
                .Select(o => o.Amount).Sum().GetValueOrDefault().ToString("N");

            ViewBag.ReceiptCountMonth = db.MoneyReceipts
                .AsNoTracking()
                .Where(o => o.ReceiptDate.Value.Month == DateTime.Now.Month && o.ReceiptDate.Value.Year == DateTime.Now.Year)
                .Count();


            ViewBag.ReceiptAmountLastYear = db.MoneyReceipts
                .AsNoTracking()
                .Where(o => o.ReceiptDate.Value.Year == DateTime.Now.Year-1)
                .Select(o => o.Amount).Sum().GetValueOrDefault().ToString("N");
            
            ViewBag.ReceiptCountLastYear = db.MoneyReceipts
                .AsNoTracking()
                .Where(o => o.ReceiptDate.Value.Year == DateTime.Now.Year - 1)
                .Count();

            ViewBag.ReceiptAmountYear = db.MoneyReceipts
                .AsNoTracking()
                .Where(o => o.ReceiptDate.Value.Year == DateTime.Now.Year)
                .Select(o => o.Amount).Sum().GetValueOrDefault().ToString("N");

            ViewBag.ReceiptCountYear = db.MoneyReceipts
                .AsNoTracking()
                .Where(o => o.ReceiptDate.Value.Year == DateTime.Now.Year )
                .Count();

            return View();
            
        }
        
    }
}