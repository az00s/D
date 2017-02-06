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
            ViewBag.a = "none";

            //sorting----------------------------------------------------------------------------------------------

            ViewBag.UnpSortParm = sort == "unp_desc" ? "unp" : "unp_desc";
            ViewBag.NameSortParm=sort== "name_desc" ? "name" : "name_desc";



            switch (sort)
            {
                case "unp": return View(db.Поставщик.ToList().OrderBy(p => p.УНП_поставщика));
                case "unp_desc": return View(db.Поставщик.ToList().OrderByDescending(p => p.УНП_поставщика));
                case "name": return View(db.Поставщик.ToList().OrderBy(p => p.Название_организации));
                case "name_desc": return View(db.Поставщик.ToList().OrderByDescending(p => p.Название_организации));

                default: return View(db.Поставщик.ToList().OrderBy(p => p.УНП_поставщика));
            }
            //---------------------------------------------------------------------------------------------------------------
            
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

            ViewBag.p = поставщик;

            var query = from g in db.Поставщик_цена
                        where g.УНП_поставщика == id
                        select g;
            return View(query.ToList());
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed(/*[Bind(Include = "УНП_поставщика,Название_организации,Адрес,Телефон,Описание")] Поставщик поставщик*/)
        {
            if (ModelState.IsValid)
            {
                p.УНП_поставщика = Convert.ToInt32(Request.Form["УНП_поставщика"]);
                p.Название_организации = Request.Form["Название_организации"];
                p.Адрес = Request.Form["Адрес"];
                p.Телефон = Request.Form["Телефон"];
                p.Описание = Request.Form["Описание"];
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
            var поставщик = db.Поставщик.Find(id);
            if (поставщик == null)
            {
                return HttpNotFound();
            }
            return View(поставщик);
        }

        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "УНП_поставщика,Название_организации,Адрес,Телефон,Описание")] Поставщик поставщик*/)
        {
            if (ModelState.IsValid)
            {
                p.УНП_поставщика = Convert.ToInt32(Request.Form["УНП_поставщика"]);
                p.Название_организации = Request.Form["Название_организации"];
                p.Адрес = Request.Form["Адрес"];
                p.Телефон = Request.Form["Телефон"];
                p.Описание = Request.Form["Описание"];
              
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
            var поставщик = db.Поставщик.Find(id);
            if (поставщик == null)
            {
                return HttpNotFound();
            }
            return View(поставщик);
        }

        
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorProviders")]
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var поставщик = db.Поставщик.Find(id);
            db.Поставщик.Remove(поставщик);
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
            var queryGoods = from good in db.Поставщик
                             where good.Адрес.Contains("минск")
                             select good;

            if (queryGoods.Count() == 0)
            {
                ViewBag.a = "normal";
                ViewBag.b = "none";
            }
            else
            {
                ViewBag.a = "none";
                ViewBag.b = "normal";
            }
            return View("Index", queryGoods.ToList());
        }

        //export to excel----------------------------------------------------------------------
        public ActionResult ExpExcl()
        {

            var grid = new GridView();
            grid.DataSource = db.Поставщик.ToList();
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachement; filename=Поставщики.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }
        //-----------------------------------------------------------------------------------------
        //[HttpPost]
        //[Authorize(Roles = "admin")]
        //public ActionResult DeleteG(int id, int p)
        //{
        //    var query = from q in db.Поставщик_цена
        //                where q.ID_товара == id && q.УНП_поставщика == p
        //                select q;
        //    db.Поставщик_цена.Remove(query.First());
        //    db.SaveChanges();

        //    return RedirectToAction("Details", new { id=p});
        //}

        //autocomplete function for search field----------------------------------------

        public ActionResult AutocompleteSearch(string term)
        {
            var result = from N in db.Поставщик
                         where N.Название_организации.Contains(term)
                         select new { value = N.Название_организации };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //--------------------------------------------------------------------------------

        public ActionResult Search(string search)
        {
            var queryGoods = from good in db.Поставщик
                             where good.Название_организации.Contains(search) || good.УНП_поставщика.ToString().Contains(search) || good.Описание.Contains(search)
                             select good;

            if (queryGoods.Count() > 0)
            {
                return PartialView(queryGoods.ToList());
            }

            else return PartialView("NoResult");
        }
    }
}
