using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using D.Models;
using D.Interfaces;
using System;
using D.Models.DataTableModel;

namespace D.Controllers
{
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [Authorize(Roles = "manager,admin,sklad")]
    public class SupplierController : Controller
    {
        private IdbInterface db;//dataContext

        public SupplierController(IdbInterface dbParam)//dependency injection via constructor
        {
            db = dbParam;
        }
        public ActionResult Index()
        {

            return View("Table");

        }
        public JsonResult Table(dtParam param)
        {
            var query = db.Suppliers.AsNoTracking();
            dtSupplier res = new dtSupplier();
            res.data = res.GetData(param, query);
            res.draw = param.Draw;
            res.recordsTotal = query.Count();
            res.recordsFiltered = res.Count(param, query);
            return Json(res);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }

            ViewBag.List = db.SupplierPrices.AsNoTracking().Where(pro => pro.SupplierPAN == id);

            return View(supplier);
        }

        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed(Supplier supplier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    supplier.AddtoTable(db, supplier);

                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = supplier.SupplierPAN });
                }
                return View("Error");
            }
            catch (Exception)
            { return View("Error"); }

           
        }

        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Supplier supplier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(supplier).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = supplier.SupplierPAN });
                }
                return View("Error");
            }
            catch (Exception)
            { return View("Error"); }
        }
        
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorProviders")]
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            db.Suppliers.Remove(db.Suppliers.Find(id));
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

        //a report about providers from Minsk
        public ActionResult Report()
        {
            return View();
        }
        public JsonResult ReportData(dtParam param)
        {
            var query = db.Suppliers.AsNoTracking().Where(s=>s.Address.Contains("минск"));
            dtSupplier res = new dtSupplier();
            res.data = res.GetData(param, query);
            res.draw = param.Draw;
            res.recordsTotal = db.Suppliers.AsNoTracking().Count();
            res.recordsFiltered = res.Count(param, query);
            return Json(res);
        }
    }
}
