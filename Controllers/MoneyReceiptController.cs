using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using D.Models;
using D.Interfaces;
using D.Models.DataTableModel;

namespace D.Controllers
{
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [Authorize(Roles = "accountant,manager,admin")]
    public class MoneyReceiptController : Controller
    {
        
        private IdbInterface db;//dataContext
        
        public MoneyReceiptController(IdbInterface dbParam)//dependency injection via constructor
        {
            db = dbParam;
           
        }
        public ActionResult Index()
        {

            return View("Table");

        }
        public JsonResult Table(dtParam param)
        {

            dtMoneyReceipt res = new dtMoneyReceipt();
            res.data = res.GetData(param, db.MoneyReceipts.AsNoTracking());
            res.draw = param.Draw;
            res.recordsTotal = db.MoneyReceipts.AsNoTracking().Count();
            res.recordsFiltered = res.Count(param, db.MoneyReceipts.AsNoTracking());
            return Json(res);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.List = db.OrderPayments.Include("Order").AsNoTracking().Where(pay => pay.ReceiptID == id);
            return View(db.MoneyReceipts.Find(id));
        }
        
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed( MoneyReceipt receipt)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.MoneyReceipts.Add(receipt);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = receipt.ReceiptID });
                }
                return View("Error");
            }
            catch (Exception)
            { return View("Error"); }

           
        }

        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MoneyReceipt receipt)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    db.Entry(receipt).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details",new { id= receipt.ReceiptID});
                }
                return View("Error");
            }
            catch (Exception)
            { return View("Error"); }
            
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var receipt = db.MoneyReceipts.Find(id);
            db.MoneyReceipts.Remove(receipt);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

 
        [HttpPost]
        public ActionResult AddM(decimal sum, OrderPayment oPay)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    oPay.Amount = sum;
                    db.OrderPayments.Add(oPay);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = oPay.ReceiptID });
                }
                return View("Error");
            }
            catch (Exception)
            {  return View("Error"); }

        }



        [ChildActionOnly]
        public ActionResult AllOrders()
        {
            return PartialView("AllOrders",db.Orders.AsNoTracking().OrderBy(o=>o.OrderID));
        }

        [ChildActionOnly]
        public ActionResult AllCustomerEnts()
        {
            return PartialView("_AllCustomerEnts", db.CustomerEnts.AsNoTracking().OrderBy(o => o.ClientPAN));
        }

        public ActionResult PeriodReport()
        {
            return View();
        }
        public JsonResult ReportData(dtParam param)
        {

            dtMoneyReceipt res = new dtMoneyReceipt();
            var query = db.MoneyReceipts.Where(s => s.ReceiptDate <= param.repEnd && s.ReceiptDate >= param.repStart);
            res.data = res.GetData(param, query);
            res.draw = param.Draw;
            res.recordsTotal = db.MoneyReceipts.AsNoTracking().Count();
            res.recordsFiltered = res.Count(param, query);
            return Json(res);
        }


    }

    

    
}
