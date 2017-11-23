using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using D.Models;
using D.Infrastructure;

namespace D.Controllers
{
    [CustomAuthorize]
    [HandleError(ExceptionType = typeof(SystemException), View = "Error")]
    public class ProductController : Controller
    {
        private IdbInterface db;//dataContext
       
        public ProductController(IdbInterface dbParam)//dependency injection via constructor
        {
            
            db = dbParam;
            
           
        }
        public ActionResult Index()
        {
           
            
            return View("Table");
        }


        public JsonResult Table(dtParam param)
        {
            var query = db.Products.AsNoTracking();
            dtResult<IQueryable<Product>,Product> res = new dtResult<IQueryable<Product>, Product>();
            res.GetData(param, query,query);
            

            return Json(res);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (db.Products.Find(id) == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProvPrice = db.SupplierPrices.AsNoTracking().Where(s => s.ProductID == id);
            ViewBag.Providers= db.Suppliers.AsNoTracking().Except(

                 db.SupplierPrices.AsNoTracking().Where(s => s.ProductID == id).Select(s => s.Supplier)
                                     );

            return View(db.Products.AsNoTracking().Single(s => s.ProductID == id));
        }

        [ActionName("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed(Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                    return RedirectToAction("Details",new { id=product.ProductID});
                }
                return View("Error");
            }
            catch (Exception)

            { return View("Error"); }
        }

               
        [CustomAuthorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Product product)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(product).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details",new { id= product.ProductID});
                }
                return View("Error");
            }

            catch (Exception)

            { return View("Error"); }

            
        }

        [CustomAuthorize(Roles = "admin")]
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorGoods")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                db.Products.Remove(db.Products.Find(id));
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (System.Data.SqlClient.SqlException)
            { return View("ErrorGoods-Supplier"); }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        
        //getting a list off all providers 
        [ChildActionOnly]
        public ActionResult AllProviders()
        {
           return PartialView("AllProviders", db.Suppliers);
        }

        //report about goods,which quantity equals 0 
           
        public ActionResult ROst_0()
        {
          
            return View("ReportBalance0");
        }
        public JsonResult Report0(dtParam param)
        {
            var query = db.Products.AsNoTracking().Where(p=>p.Balance==0);
            dtResult<IQueryable<Product>, Product> res = new dtResult<IQueryable<Product>, Product>();
            res.GetData(param, db.Products,query);
            return Json(res);
        }

        //a report about quantities of goods
        public ActionResult ROst()
        {
            return View("ReportBalance");
        }
        //a report about prices
        public ActionResult Price()
        {
            return View("ReportPrice");
        }

        [HttpPost]
        //deleting provider for goods
        public ActionResult DeleteP(int id,int p)
        {
            try
            {
                db.SupplierPrices.Remove(db.SupplierPrices.Single(q => q.ProductID == id && q.SupplierPAN == p));
                db.SaveChanges();
            }
            catch (Exception)
            {
                return View("Error");
            }
         

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public ActionResult AddP(SupplierPrice Sprice)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    db.SupplierPrices.Add(Sprice);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = Sprice.ProductID });
                }
                return View("Error");

            }
            catch (Exception)
            { return View("Error"); }

            
        }

        

    }
}
