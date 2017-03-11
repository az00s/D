using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using D.Interfaces;
using System;

namespace D.Models
{
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [Authorize(Roles = "seller,manager,admin")]
    public class КлиентController : Controller
    {
        private IdbInterface db;//dataContext
        private IКлиентInterface p;

        public КлиентController(IdbInterface dbParam, IКлиентInterface pParam)//dependency injection via constructor
        {

            db = dbParam;
            p = pParam;


        }
        public ActionResult Index(string sort)
        {
            
            ViewBag.UnpSortParm = "unp";
            ViewBag.NameSortParm = "name";
            ViewBag.LastSortParm = "last";
            ViewBag.FirstSortParm = "first";
            ViewBag.PatSortParm = "pat";
            ViewBag.NumSortParm = "num";

           return View(db.Клиент.AsNoTracking().OrderBy(p => p.ID_клиента));       
         }

        public ActionResult Sorting(string sort)
        {
            ViewBag.UnpSortParm = sort == "unp_desc" ? "unp" : "unp_desc";
            ViewBag.NameSortParm = sort == "name_desc" ? "name" : "name_desc";
            ViewBag.LastSortParm = sort == "last_desc" ? "last" : "last_desc";
            ViewBag.FirstSortParm = sort == "first_desc" ? "first" : "first_desc";
            ViewBag.PatSortParm = sort == "pat_desc" ? "pat" : "pat_desc";
            ViewBag.NumSortParm = sort == "num_desc" ? "num" : "num_desc";

            switch (sort)
            {
                case "unp": return PartialView("Table",db.Клиент.AsNoTracking().OrderBy(p => p.УНП_Клиента));
                case "unp_desc": return PartialView("Table", db.Клиент.AsNoTracking().OrderByDescending(p => p.УНП_Клиента));
                case "name": return PartialView("Table", db.Клиент.AsNoTracking().OrderBy(p => p.Название_организации));
                case "name_desc": return PartialView("Table", db.Клиент.AsNoTracking().OrderByDescending(p => p.Название_организации));


                default: return PartialView("Table", db.Клиент.AsNoTracking().OrderBy(p => p.ID_клиента));
            }
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var клиент = db.Клиент.Find(id);
            if (клиент == null)
            {
                return HttpNotFound();
            }
            return View(клиент);
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed([Bind(Include = "ID_клиента,Номер_паспорта,УНП_Клиента,Название_организации,Телефон,Адрес,Фамилия,Имя,Отчество")] Клиент p)
        {
            if (ModelState.IsValid)
            {
                //p.ID_клиента = Convert.ToInt32(Request.Form["ID_клиента"]);
                //p.Номер_паспорта = Request.Form["Номер_паспорта"];
                //p.УНП_Клиента = Convert.ToInt32(Request.Form["УНП_Клиента"]);
                //p.Название_организации = Request.Form["Название_организации"];
                //p.Телефон = Request.Form["Телефон"];
                //p.Адрес = Request.Form["Адрес"];
                //p.Фамилия = Request.Form["Фамилия"];
                //p.Имя = Request.Form["Имя"];
                //p.Отчество = Request.Form["Отчество"];
                
                p.AddtoTable(db, p);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(p);
        }

       
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var клиент = db.Клиент.Find(id);
            if (клиент == null)
            {
                return HttpNotFound();
            }
            return View(клиент);
        }

        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_клиента,Номер_паспорта,УНП_Клиента,Название_организации,Телефон,Адрес,Фамилия,Имя,Отчество")] Клиент p)
        {
            if (ModelState.IsValid)
            {
                //p.ID_клиента = Convert.ToInt32(Request.Form["ID_клиента"]);
                //p.Номер_паспорта = Request.Form["Номер_паспорта"];
                //p.УНП_Клиента = Convert.ToInt32(Request.Form["УНП_Клиента"]);
                //p.Название_организации = Request.Form["Название_организации"];
                //p.Телефон = Request.Form["Телефон"];
                //p.Адрес = Request.Form["Адрес"];
                //p.Фамилия = Request.Form["Фамилия"];
                //p.Имя = Request.Form["Имя"];
                //p.Отчество = Request.Form["Отчество"];
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(p);
        }

      
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var клиент = db.Клиент.Find(id);
            if (клиент == null)
            {
                return HttpNotFound();
            }
            return View(клиент);
        }


        [Authorize(Roles = "admin")]
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorClients")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Клиент.Remove(db.Клиент.Find(id));
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

        //a report about clients from Mogilev
        public ActionResult CMogilev()
        {   
            return PartialView("Table", db.Клиент.AsNoTracking().Where(cl=>cl.Адрес.Contains("могил")));
        }
        //-----------------------------------------------------------------------------
        
        //autocomplete function for search field----------------------------------------
        public ActionResult AutocompleteSearch(string term)
        {           
            return Json(db.Клиент
                .AsNoTracking()
                .Where(cl=>cl.Название_организации.Contains(term))
                .Select(c=>new { value=c.Название_организации})
                , JsonRequestBehavior.AllowGet);
        }
        //----------------------------------------------------------------------------------

        public ActionResult Search(string search)
        {
            var query = db.Клиент
                .AsNoTracking()
                .Where(c => c.Название_организации.Contains(search) || c.УНП_Клиента.ToString().Contains(search));
                             

            if (query.Count() > 0)
            {
                return PartialView(query);
            }

            else return PartialView("NoResult");
        }

    }
}
