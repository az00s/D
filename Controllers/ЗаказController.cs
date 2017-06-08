using System;
using System.Collections.Generic;
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
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Hosting;

namespace D.Controllers
{
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [Authorize(Roles = "accountant,manager,admin,seller")]
    public class ЗаказController : Controller
    {
        private IЗаказInterface p; 
        private IОформление_заказаInterface o;
        private IdbInterface db;
        public ЗаказController(IdbInterface dbParam, IОформление_заказаInterface oParam, IЗаказInterface pParam)//dependency injection via constructor
        {
            p = pParam;
            o = oParam;
            db = dbParam;
        }
        
        public ActionResult Index()
        {
            return RedirectToAction("Table");
            
        }

        public ActionResult Table()
        {
            return View("Table",db.Заказ.AsNoTracking().OrderBy(o=>o.ID_заказа));
        }
        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var заказ = db.Заказ.Find(id);
           
            if (заказ == null)
            {
                return HttpNotFound();
            }

            ViewBag.Amount = db.Заказ.AsNoTracking().Where(ord => ord.ID_заказа == id)
                            .GroupJoin(
                            db.Оформление_заказа,
                            o => o.ID_заказа,
                            of => of.ID_заказа,
                            (o, of) => new { amount = of.Sum(oa => oa.Количество * oa.Товар.Цена_с_НДС) })
                            .FirstOrDefault().amount.Value.ToString("0.00");

            ViewBag.Ord = заказ;
                        
            ViewBag.Paylist = db.Оплата_заказа
                .AsNoTracking()
                .Where(g=> g.ID_заказа == id);
            ViewBag.OfList = db.Оформление_заказа
                .AsNoTracking()
                .Where(d => d.ID_заказа == id);

            return View(заказ);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Заказ p)
        {
            try
            {
                //if (ModelState.IsValid)
                //{
                    p.AddtoTable(db, p);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = p.ID_заказа });
                //}
                //return View("Error");
            }
            catch (Exception)
            { return View("Error"); }            
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ICollection<Оформление_заказа> listOf, [Bind(Exclude = "Оформление_заказа")]Заказ p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Оформление_заказа.RemoveRange(db.Оформление_заказа.Where(o => o.ID_заказа == p.ID_заказа));
                    db.Entry(p).State = EntityState.Modified;
                    db.Оформление_заказа.AddRange(listOf);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = p.ID_заказа });
                }
                return View("Error");
            }
            catch (Exception)
            { return View("Error"); }
            
        }
        
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var заказ = db.Заказ.Find(id);
            if (заказ == null)
            {
                return HttpNotFound();
            }
            return View(заказ);
        }
        [Authorize(Roles = "admin")]
      
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorOrders")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.Заказ.Remove(db.Заказ.Find(id));
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

        //getting list of all clients------------------------------------
        [ChildActionOnly]
        public ActionResult AllClients()
        {
            return PartialView("AllClients", db.CustomerEnt.AsNoTracking().OrderBy(o=>o.УНП_Клиента));
        }
        //getting list of all goods--------------------------------------------
        //[ChildActionOnly]
        public ActionResult AllGoods()
        {

            return PartialView("AllGoods", db.Товар.AsNoTracking().OrderBy(o=>o.ID_товара));
        }
        [ChildActionOnly]
        public ActionResult AllEmployee()
        {
            return PartialView("AllEmployee", db.Сотрудник.AsNoTracking().OrderBy(o=>o.Фамилия));
        }


        //deleting money from order
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteP(Оплата_заказа p)
        {
            
            db.Оплата_заказа.Remove(db.Оплата_заказа.Find(p.ID));
            db.SaveChanges();
            return RedirectToAction("Details", new { id = p.ID_заказа });
        }

    }
}
