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
            ViewBag.a = "none";
            var денежное_поступление = db.Денежное_поступление.Include(д => д.Клиент).ToList();
            ViewBag.NumSortParm = sort == "num_desc" ? "num" : "num_desc";
            ViewBag.DateSortParm = sort == "date_desc" ? "date" : "date_desc";
            ViewBag.UnpSortParm = sort == "unp_desc" ? "unp" : "unp_desc";
            ViewBag.NameSortParm = sort == "name_desc" ? "name" : "name_desc";
            ViewBag.AmountSortParm = sort == "amount_desc" ? "amount" : "amount_desc";



            switch (sort)
            {
                case "num": return View(денежное_поступление.ToList().OrderBy(p => p.ID_поступления));
                case "num_desc": return View(денежное_поступление.ToList().OrderByDescending(p => p.ID_поступления));
                case "date": return View(денежное_поступление.ToList().OrderBy(p => p.Дата_поступления));
                case "date_desc": return View(денежное_поступление.ToList().OrderByDescending(p => p.Дата_поступления));
                case "unp": return View(денежное_поступление.ToList().OrderBy(p => p.Клиент.УНП_Клиента));
                case "unp_desc": return View(денежное_поступление.ToList().OrderByDescending(p => p.Клиент.УНП_Клиента));
                case "name": return View(денежное_поступление.ToList().OrderBy(p => p.Клиент.Название_организации));
                case "name_desc": return View(денежное_поступление.ToList().OrderByDescending(p => p.Клиент.Название_организации));
                case "amount": return View(денежное_поступление.ToList().OrderBy(p => p.Сумма));
                case "amount_desc": return View(денежное_поступление.ToList().OrderByDescending(p => p.Сумма));

                default: return View(денежное_поступление.ToList().OrderBy(p => p.ID_поступления));
            }

            
        }

        
        public ActionResult Details(int? id)
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

            ViewBag.p = денежное_поступление;

            var query = from g in db.Оплата_заказа.Include("Заказ")
                        where g.ID_поступления == id
                        select g;

            //query.Distinct();
            
            return View(query.ToList());
        }

        
        public ActionResult Create()
        {
            ViewBag.ID_клиента = new SelectList(db.Клиент, "ID_клиента", "Название_организации");
            return View();
        }

        
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed(/*[Bind(Include = "ID_поступления,Сумма,Дата_поступления,ID_клиента")] Денежное_поступление денежное_поступление*/)
        {
            if (ModelState.IsValid)
            {
                p.ID_поступления = Convert.ToInt32(Request.Form["ID_поступления"]);
                p.Сумма = Convert.ToDecimal(Request.Form["Сумма"]);
                p.Дата_поступления = Convert.ToDateTime(Request.Form["Дата_поступления"]);
                p.ID_клиента = Convert.ToInt32(Request.Form["ID_клиента"]);
                
                p.AddtoTable(db, p);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ID_клиента = new SelectList(db.Клиент, "ID_клиента", "Номер_паспорта", p.ID_клиента);
            return View(p);
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
            ViewBag.ID_клиента = new SelectList(db.Клиент, "ID_клиента", "Название_организации", денежное_поступление.ID_клиента);
            return View(денежное_поступление);
        }

        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "ID_поступления,Сумма,Дата_поступления,ID_клиента")] Денежное_поступление денежное_поступление*/)
        {
            if (ModelState.IsValid)
            {
                p.ID_поступления = Convert.ToInt32(Request.Form["ID_поступления"]);
                p.Сумма = Convert.ToDecimal(Request.Form["Сумма"]);
                p.Дата_поступления = Convert.ToDateTime(Request.Form["Дата_поступления"]);
                p.ID_клиента = Convert.ToInt32(Request.Form["ID_клиента"]);
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ID_клиента = new SelectList(db.Клиент, "ID_клиента", "Номер_паспорта", p.ID_клиента);
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
            return PartialView("AllClients", db.Клиент.ToList());
            
        }

        public ActionResult ExpExcl()
        {

            var grid = new GridView();
            var list = db.Денежное_поступление.Select(d=>new {Номер=d.ID_поступления, Дата=d.Дата_поступления, Сумма=d.Сумма,Клиент=d.Клиент.Название_организации });
            
            grid.DataSource = list.ToList();
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachement; filename=Поступления.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }

        public ActionResult AddM(int id)
        {
            var queryGoods = from good in db.Денежное_поступление
                             where good.ID_поступления == id
                             select good;


            ViewBag.d = (queryGoods.First());
            
            var query = from s in db.Заказ
                        where s.Статус_заказа== "Оплачен"
                        select s;
            var p = db.Заказ.Except(query);
            
            return View(p);
        }
        
        [HttpPost]
        public ActionResult AddM(decimal Сумма)
        {
            o.ID_заказа = Convert.ToInt32(Request.Form["ID_заказа"]);
            o.ID_поступления = Convert.ToInt32(Request.Form["ID_поступления"]);
            o.Сумма = Сумма;
            o.AddtoTable(db,o);
            db.SaveChanges();
            

            return RedirectToAction("Details", new { id = o.ID_поступления });

            
        }

        public ActionResult AutocompleteSearch(string term)
        {
            
            var result = from N in db.Денежное_поступление
                         where N.Клиент.Название_организации.Contains(term)
                         select new { value = N.Клиент.Название_организации };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string search)
        {
            var queryGoods = from good in db.Денежное_поступление
                             where good.Клиент.Название_организации.Contains(search) || good.Клиент.УНП_Клиента.ToString().Contains(search) || good.Сумма.ToString().Contains(search) || good.ID_поступления.ToString().Contains(search)
                             select good;

            if (queryGoods.Count() > 0)
            {
                return PartialView(queryGoods.ToList());
            }

            else return PartialView("NoResult");
        }

        public ActionResult ROrders(DateTime start, DateTime end)
        {
            var queryGoods = from good in db.Денежное_поступление
                             where good.Дата_поступления >= start && good.Дата_поступления <= end
                             select good;

            if (queryGoods.Count() > 0)
            {
                return PartialView(queryGoods.ToList());
            }

            else return PartialView("NoResult");
        }


    }

    

    
}
