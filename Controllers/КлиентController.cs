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
            ViewBag.a = "none";
            //sorting----------------------------------------------------------------------------

            ViewBag.UnpSortParm = sort == "unp_desc" ? "unp" : "unp_desc";
            ViewBag.NameSortParm = sort == "name_desc" ? "name" : "name_desc";
            ViewBag.LastSortParm = sort == "last_desc" ? "last" : "last_desc";
            ViewBag.FirstSortParm = sort == "first_desc" ? "first" : "first_desc";
            ViewBag.PatSortParm = sort == "pat_desc" ? "pat" : "pat_desc";
            ViewBag.NumSortParm = sort == "num_desc" ? "num" : "num_desc";

            switch (sort)
            {
                case "unp": return View(db.Клиент.ToList().OrderBy(p => p.УНП_Клиента));
                case "unp_desc": return View(db.Клиент.ToList().OrderByDescending(p => p.УНП_Клиента));
                case "name": return View(db.Клиент.ToList().OrderBy(p => p.Название_организации));
                case "name_desc": return View(db.Клиент.ToList().OrderByDescending(p => p.Название_организации));
                case "last": return View(db.Клиент.ToList().OrderBy(p => p.Фамилия));
                case "last_desc": return View(db.Клиент.ToList().OrderByDescending(p => p.Фамилия));
                case "first": return View(db.Клиент.ToList().OrderBy(p => p.Имя));
                case "first_desc": return View(db.Клиент.ToList().OrderByDescending(p => p.Имя));
                case "pat": return View(db.Клиент.ToList().OrderBy(p => p.Отчество));
                case "pat_desc": return View(db.Клиент.ToList().OrderByDescending(p => p.Отчество));
                case "num": return View(db.Клиент.ToList().OrderBy(p => p.Номер_паспорта));
                case "num_desc": return View(db.Клиент.ToList().OrderByDescending(p => p.Номер_паспорта));


                default: return View(db.Клиент.ToList().OrderBy(p => p.ID_клиента));
            }
            //-------------------------------------------------------------------------------------------------------
           
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
        public ActionResult CreateConfirmed(/*[Bind(Include = "ID_клиента,Номер_паспорта,УНП_Клиента,Название_организации,Телефон,Адрес,Фамилия,Имя,Отчество")] Клиент клиент*/)
        {
            if (ModelState.IsValid)
            {
                p.ID_клиента = Convert.ToInt32(Request.Form["ID_клиента"]);
                p.Номер_паспорта = Request.Form["Номер_паспорта"];
                p.УНП_Клиента = Convert.ToInt32(Request.Form["УНП_Клиента"]);
                p.Название_организации = Request.Form["Название_организации"];
                p.Телефон = Request.Form["Телефон"];
                p.Адрес = Request.Form["Адрес"];
                p.Фамилия = Request.Form["Фамилия"];
                p.Имя = Request.Form["Имя"];
                p.Отчество = Request.Form["Отчество"];
                
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
        public ActionResult Edit(/*[Bind(Include = "ID_клиента,Номер_паспорта,УНП_Клиента,Название_организации,Телефон,Адрес,Фамилия,Имя,Отчество")] Клиент клиент*/)
        {
            if (ModelState.IsValid)
            {
                p.ID_клиента = Convert.ToInt32(Request.Form["ID_клиента"]);
                p.Номер_паспорта = Request.Form["Номер_паспорта"];
                p.УНП_Клиента = Convert.ToInt32(Request.Form["УНП_Клиента"]);
                p.Название_организации = Request.Form["Название_организации"];
                p.Телефон = Request.Form["Телефон"];
                p.Адрес = Request.Form["Адрес"];
                p.Фамилия = Request.Form["Фамилия"];
                p.Имя = Request.Form["Имя"];
                p.Отчество = Request.Form["Отчество"];
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
            var клиент = db.Клиент.Find(id);
            db.Клиент.Remove(клиент);
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
            var queryGoods = from good in db.Клиент
                             where good.Адрес.Contains("могил")
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
        //-----------------------------------------------------------------------------
        
            
        //Export to excel
        public ActionResult ExpExcl()
        {
            var grid = new GridView();
            grid.DataSource = db.Клиент.ToList();
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachement; filename=Клиенты.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }

        //autocomplete function for search field----------------------------------------
        public ActionResult AutocompleteSearch(string term)
        {
            var result = from N in db.Клиент
                         where N.Название_организации.Contains(term)
                         select new { value = N.Название_организации };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //----------------------------------------------------------------------------------

        public ActionResult Search(string search)
        {
            var queryGoods = from good in db.Клиент
                             where good.Название_организации.Contains(search) || good.УНП_Клиента.ToString().Contains(search)
                             select good;

            if (queryGoods.Count() > 0)
            {
                return PartialView(queryGoods.ToList());
            }

            else return PartialView("NoResult");
        }

    }
}
