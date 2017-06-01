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
using System;

namespace D.Controllers
{
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [Authorize(Roles = "manager,admin,sklad")]
    public class ПоставщикController : Controller
    {
        private IdbInterface db;//dataContext
        private IПоставщикInterface p;

        public ПоставщикController(IdbInterface dbParam, IПоставщикInterface pParam)//dependency injection via constructor
        {
            db = dbParam;
            p = pParam;
        }      
        public ActionResult Index(string sort)
        {
            return RedirectToAction("Table");
        }

        public ActionResult Table()
        {
            return View("Table",db.Поставщик.AsNoTracking().OrderBy(o=>o.УНП_поставщика));
            
            //---------------------------------------------------------------------------------------------------------------}
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var поставщик = db.Поставщик.Find(id);
            if (поставщик == null)
            {
                return HttpNotFound();
            }

            ViewBag.List = db.Поставщик_цена.AsNoTracking().Where(pro => pro.УНП_поставщика == id);

            return View(поставщик);
        }


        //public ActionResult Create()
        //{
        //    return View();
        //}


        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed([Bind(Include = "УНП_поставщика,Название_организации,Адрес,Телефон,Описание")] Поставщик p)
        {
            if (ModelState.IsValid)
            {
                //p.УНП_поставщика = Convert.ToInt32(Request.Form["УНП_поставщика"]);
                //p.Название_организации = Request.Form["Название_организации"];
                //p.Адрес = Request.Form["Адрес"];
                //p.Телефон = Request.Form["Телефон"];
                //p.Описание = Request.Form["Описание"];
                p.AddtoTable(db, p);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Error");
        }

        
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var поставщик = db.Поставщик.Find(id);
        //    if (поставщик == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(поставщик);
        //}

        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "УНП_поставщика,Название_организации,Адрес,Телефон,Описание")] Поставщик p)
        {
            if (ModelState.IsValid)
            {
                //p.УНП_поставщика = Convert.ToInt32(Request.Form["УНП_поставщика"]);
                //p.Название_организации = Request.Form["Название_организации"];
                //p.Адрес = Request.Form["Адрес"];
                //p.Телефон = Request.Form["Телефон"];
                //p.Описание = Request.Form["Описание"];
              
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details",new { id=p.УНП_поставщика});
            }
            return View("Error");
        }

        
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var поставщик = db.Поставщик.Find(id);
        //    if (поставщик == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(поставщик);
        //}

        
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorProviders")]
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            db.Поставщик.Remove(db.Поставщик.Find(id));
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
        public ActionResult PMinsk()
        {
            
            return View("Table",db.Поставщик.AsNoTracking().Where(pro=> pro.Адрес.Contains("минск")));
        }

        // public ActionResult AutocompleteSearch(string term)
        //{
            
        //    return Json(db.Поставщик
        //        .AsNoTracking()
        //        .Where(pro=>pro.Название_организации.Contains(term))
        //        .Select(p => new { value = p.Название_организации })
        //        , JsonRequestBehavior.AllowGet);
        //}
        //--------------------------------------------------------------------------------

        //public ActionResult Search(string search)
        //{
        //    var query = db.Поставщик
        //        .AsNoTracking()
        //        .Where(pro => pro.Название_организации.Contains(search) || pro.УНП_поставщика.ToString().Contains(search) || pro.Описание.Contains(search));

        //    if (query.Count() > 0)
        //    {
        //        return PartialView(query);
        //    }

        //    else return PartialView("NoResult");
        //}
    }
}
