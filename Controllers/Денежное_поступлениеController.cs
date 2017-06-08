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
        public ActionResult Index()
        {

            return RedirectToAction("Table");
           
        }

        public ActionResult Table()
        {
            return View("Table",db.Денежное_поступление.AsNoTracking().OrderBy(o=>o.ID_поступления));
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.List = db.Оплата_заказа.Include("Заказ").AsNoTracking().Where(pay => pay.ID_поступления == id);
            return View(db.Денежное_поступление.Find(id));
        }
        
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed([Bind(Include = "ID_поступления,Сумма,Дата_поступления,ID_клиента")] Денежное_поступление p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    p.AddtoTable(db, p);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = p.ID_поступления });
                }
                return View("Error");
            }
            catch (Exception)
            { return View("Error"); }

           
        }

        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_поступления,Сумма,Дата_поступления,ID_клиента")] Денежное_поступление p)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details",new { id=p.ID_поступления});
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


        [HttpPost]
        public ActionResult AddM(decimal sum, Оплата_заказа o)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    o.Сумма = sum;
                    o.AddtoTable(db, o);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = o.ID_поступления });
                }
                return View("Error");
            }
            catch (Exception)
            {  return View("Error"); }

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

        [ChildActionOnly]
        public ActionResult AllOrders()
        {
            return PartialView("AllOrders",db.Заказ.AsNoTracking().OrderBy(o=>o.ID_заказа));
        }


    }

    

    
}
