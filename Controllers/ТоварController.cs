using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using D.Models;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using D.Interfaces;
using System.Collections.Generic;
using System.Web.Helpers;
using System.Threading.Tasks;
using D.Infrastructure;
using System.Collections;

namespace D.Controllers
{
   
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [Authorize]
    [HandleError(ExceptionType = typeof(SystemException), View = "Error")]
    public class ТоварController : Controller
    {
        private IdbInterface db;//dataContext
        private IПоставщик_ценаInterface p;
        private IТоварInterface товар;
        public ТоварController(IdbInterface dbParam, IПоставщик_ценаInterface pParam, IТоварInterface товарParam)//dependency injection via constructor
        {
            
            db = dbParam;
            p = pParam;
            товар=товарParam;
           
        }
        public ActionResult Index(int pagesize = 10)
        { 
            return RedirectToAction("Table");
        }

       
        public ActionResult Table()
        {
            IQueryable<Товар> request = db.Товар.AsNoTracking();

            return View("Table", request.OrderBy(p => p.ID_товара));
        }
 
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (db.Товар.Find(id) == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProvPrice = db.Поставщик_цена.AsNoTracking().Where(s => s.ID_товара == id);
            ViewBag.Providers= db.Поставщик.AsNoTracking().Except(

                 db.Поставщик_цена.AsNoTracking().Where(s => s.ID_товара == id).Select(s => s.Поставщик)
                                     );

            return View(db.Товар.AsNoTracking().Single(s => s.ID_товара == id));
        }

        [ActionName("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed(Товар товар)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    товар.AddtoTable(db, товар);
                    db.SaveChanges();
                    return RedirectToAction("Details",new { id=товар.ID_товара});
                }
                return View("Error");
            }
            catch (Exception)

            { return View("Error"); }
        }

               
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_товара,Обозначение,Наименование,Краткое_описание,Цена,Остаток_на_складе,Единица_измерения,Срок_поставки,Вес")] Товар товар)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(товар).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details",new { id=товар.ID_товара});
                }
                return View("Error");
            }

            catch (Exception)

            { return View("Error"); }

            
        }

        [Authorize(Roles = "admin")]
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorGoods")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
                db.Товар.Remove(db.Товар.Find(id));
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

        
        //getting a list off all providers 
        [ChildActionOnly]
        public ActionResult AllProviders()
        {
           return PartialView("AllProviders", db.Поставщик);
        }

        //report about goods,which quantity equals 0       
        public ActionResult ROst_0(int page = 1, int pagesize = 10)

        {
            var request = db.Товар.AsNoTracking().Where(s => s.Остаток_на_складе == 0);
            //Paging--------------------------------------------------------------------------------------------------
            ViewBag.CurrentPage = page;
            ViewBag.PageCount = request.LongCount() % pagesize == 0 ?
                                        request.LongCount() / pagesize :
                                       (request.LongCount() / pagesize) + 1;

            ViewBag.PageCountResult = ViewBag.PageCount < 11 ? ViewBag.PageCount : 7;

            return View("Table",request.OrderBy(p => p.ID_товара));
        }

        //a report about quantities of goods
            public ActionResult ROst()
        {
            var request = db.Товар.AsNoTracking();
            @ViewBag.Ost = "hidden";

            return View("Table",request.OrderBy(p => p.ID_товара));
        }
        //a report about prices
        public ActionResult Price(int page = 1, int pagesize = 10)
        {
            var request = db.Товар.AsNoTracking();
            //Paging--------------------------------------------------------------------------------------------------
            ViewBag.CurrentPage = page;
            ViewBag.PageCount = request.LongCount() % pagesize == 0 ?
                                        request.LongCount() / pagesize :
                                       (request.LongCount() / pagesize) + 1;

            ViewBag.PageCountResult = ViewBag.PageCount < 11 ? ViewBag.PageCount : 7;
            @ViewBag.Price = "hidden";
            return View("Table",request.OrderBy(p => p.ID_товара));
        
            
        }

        [HttpPost]
        //deleting provider for goods
        public ActionResult DeleteP(int id,int p)
        {
            try
            {
                db.Поставщик_цена.Remove(db.Поставщик_цена.Single(q => q.ID_товара == id && q.УНП_поставщика == p));
                db.SaveChanges();
            }
            catch (Exception)
            {
                return View("Error");
            }
         

            return RedirectToAction("Details", new { id = id });
        }

        [HttpPost]
        public ActionResult AddP(Поставщик_цена p)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                    p.AddtoTable(db, p);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = p.ID_товара });
                }
                return View("Error");

            }
            catch (Exception)
            { return View("Error"); }

            
        }

        

    }
}
