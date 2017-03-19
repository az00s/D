using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using D.Models;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using D.Interfaces;

namespace D.Controllers
{
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [Authorize(Roles = "accountant,manager,admin")]
    public class Денежное_поступлениеController : Controller
    {
        
        private IdbInterface db;//dataContext
        private IДенежное_поступлениеInterface p;
        private IОплата_заказаInterface o;
        public Денежное_поступлениеController(IdbInterface dbParam, IДенежное_поступлениеInterface pParam, IОплата_заказаInterface oParam)//dependency injection via constructor
        {
            db = dbParam;
            p = pParam;
            o = oParam;
        }
        public ActionResult Index(string sort)
        {
            
            ViewBag.NumSortParm =  "num_desc";
            ViewBag.DateSortParm = "date" ;
            ViewBag.UnpSortParm ="unp" ;
            ViewBag.NameSortParm = "name";
            ViewBag.AmountSortParm ="amount";
            
             return View(db.Денежное_поступление.Include(д => д.CustomerEnt).AsNoTracking().OrderBy(p => p.ID_поступления));
           
        }

        public ActionResult Sorting(string sort)
        {

            var денежное_поступление = db.Денежное_поступление.Include(д => д.CustomerEnt);
            ViewBag.NumSortParm = sort == "num_desc" ? "num" : "num_desc";
            ViewBag.DateSortParm = sort == "date_desc" ? "date" : "date_desc";
            ViewBag.UnpSortParm = sort == "unp_desc" ? "unp" : "unp_desc";
            ViewBag.NameSortParm = sort == "name_desc" ? "name" : "name_desc";
            ViewBag.AmountSortParm = sort == "amount_desc" ? "amount" : "amount_desc";



            switch (sort)
            {
                case "num": return PartialView("Table",денежное_поступление.AsNoTracking().OrderBy(p => p.ID_поступления));
                case "num_desc": return PartialView("Table", денежное_поступление.AsNoTracking().OrderByDescending(p => p.ID_поступления));
                case "date": return PartialView("Table", денежное_поступление.AsNoTracking().OrderBy(p => p.Дата_поступления));
                case "date_desc": return PartialView("Table", денежное_поступление.AsNoTracking().OrderByDescending(p => p.Дата_поступления));
                case "unp": return PartialView("Table", денежное_поступление.AsNoTracking().OrderBy(p => p.CustomerEnt.УНП_Клиента));
                case "unp_desc": return PartialView("Table", денежное_поступление.AsNoTracking().OrderByDescending(p => p.CustomerEnt.УНП_Клиента));
                case "name": return PartialView("Table", денежное_поступление.AsNoTracking().OrderBy(p => p.CustomerEnt.Название_организации));
                case "name_desc": return PartialView("Table", денежное_поступление.AsNoTracking().OrderByDescending(p => p.CustomerEnt.Название_организации));
                case "amount": return PartialView("Table", денежное_поступление.AsNoTracking().OrderBy(p => p.Сумма));
                case "amount_desc": return PartialView("Table", денежное_поступление.AsNoTracking().OrderByDescending(p => p.Сумма));

                default: return PartialView("Table", денежное_поступление.AsNoTracking().OrderBy(p => p.ID_поступления));
            }
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.p = db.Денежное_поступление.Find(id);

            if (ViewBag.p == null)
            {
                return HttpNotFound();
            }

            return View(db.Оплата_заказа.Include("Заказ").AsNoTracking().Where(pay=>pay.ID_поступления==id));
        }

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed([Bind(Include = "ID_поступления,Сумма,Дата_поступления,ID_клиента")] Денежное_поступление p)
        {
            if (ModelState.IsValid)
            {
                //p.ID_поступления = Convert.ToInt32(Request.Form["ID_поступления"]);
                //p.Сумма = Convert.ToDecimal(Request.Form["Сумма"]);
                //p.Дата_поступления = Convert.ToDateTime(Request.Form["Дата_поступления"]);
                //p.ID_клиента = Convert.ToInt32(Request.Form["ID_клиента"]);
                
                p.AddtoTable(db, p);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View("Create",p);
        }

      
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var денежное_поступление = db.Денежное_поступление.Find(id);
            if (денежное_поступление == null)
            {
                return HttpNotFound();
            }
            return View(денежное_поступление);
        }

        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_поступления,Сумма,Дата_поступления,ID_клиента")] Денежное_поступление p)
        {
            if (ModelState.IsValid)
            {
                //p.ID_поступления = Convert.ToInt32(Request.Form["ID_поступления"]);
                //p.Сумма = Convert.ToDecimal(Request.Form["Сумма"]);
                //p.Дата_поступления = Convert.ToDateTime(Request.Form["Дата_поступления"]);
                //p.ID_клиента = Convert.ToInt32(Request.Form["ID_клиента"]);
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
            var денежное_поступление = db.Денежное_поступление.Find(id);
            if (денежное_поступление == null)
            {
                return HttpNotFound();
            }
            return View(денежное_поступление);
        }

        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var денежное_поступление = db.Денежное_поступление.Find(id);
            db.Денежное_поступление.Remove(денежное_поступление);
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

        [ChildActionOnly]
        public ActionResult AllClients()
        {
            return PartialView("AllClients", db.CustomerEnt.AsNoTracking());
            
        }
        public ActionResult AddM(int id)
        {
           ViewBag.d = db.Денежное_поступление
                .AsNoTracking()
                .Single(m => m.ID_поступления == id);
            
            
            
            var query = db.Заказ
                .AsNoTracking()
                .Where(o => o.Статус_заказа != "Оплачен");
            
            return View(query);
        }
        
        [HttpPost]
        public ActionResult AddM(decimal Сумма,Оплата_заказа o)
        {
            //o.ID_заказа = Convert.ToInt32(Request.Form["ID_заказа"]);
            //o.ID_поступления = Convert.ToInt32(Request.Form["ID_поступления"]);
            o.Сумма = Сумма;
            o.AddtoTable(db,o);
            db.SaveChanges();
            

            return RedirectToAction("Details", new { id = o.ID_поступления });

            
        }

        public ActionResult AutocompleteSearch(string term)
        {            
          return Json(db.Денежное_поступление
                .AsNoTracking()
                .Where(mo=>mo.CustomerEnt.Название_организации.Contains(term))
                .Select(s=>new { value = s.CustomerEnt.Название_организации })
                , JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string search)
        {
            var queryGoods = db.Денежное_поступление
                .AsNoTracking()
                .Where(mo => mo.CustomerEnt.Название_организации.Contains(search) || mo.CustomerEnt.УНП_Клиента.ToString().Contains(search) || mo.Сумма.ToString().Contains(search) || mo.ID_поступления.ToString().Contains(search));
            if (queryGoods.Count() > 0)
            {
                return PartialView(queryGoods.ToList());
            }

            else return PartialView("NoResult");
        }

        public ActionResult ROrders(DateTime start, DateTime end)
        {
            var query = db.Денежное_поступление.AsNoTracking().Where(mo=>mo.Дата_поступления >= start && mo.Дата_поступления <= end);
            if (query.Count() > 0)
            {
                return PartialView(query.AsNoTracking());
            }

            else return PartialView("NoResult");
        }


    }

    

    
}
