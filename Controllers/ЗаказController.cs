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
        
      
        public ActionResult Index(string sort)
        {
            ViewBag.a = "none";
            var заказ = db.Заказ.Include(з => з.Клиент).Include(з => з.Сотрудник);

            //sorting----------------------------------------------------------------------------

            ViewBag.NumSortParm = sort == "num_desc" ? "num" : "num_desc";
            ViewBag.DateSortParm = sort == "date_desc" ? "date" : "date_desc";
            ViewBag.NameSortParm = sort == "name_desc" ? "name" : "name_desc";
            ViewBag.AmountSortParm = sort == "amount_desc" ? "amount" : "amount_desc";
            ViewBag.StatusSortParm = sort == "status_desc" ? "statdistinus" : "status_desc";
            ViewBag.EmpSortParm= sort == "emp_desc" ? "emp" : "emp_desc";

            switch (sort)
            {
                case "num": return View(заказ.ToList().OrderBy(p => p.ID_заказа));
                case "num_desc": return View(заказ.ToList().OrderByDescending(p => p.ID_заказа));
                case "date": return View(заказ.ToList().OrderBy(p => p.Дата_заказа));
                case "date_desc": return View(заказ.ToList().OrderByDescending(p => p.Дата_заказа));

                case "name": return View(заказ.ToList().OrderBy(p => p.Клиент.Название_организации));
                case "name_desc": return View(заказ.ToList().OrderByDescending(p => p.Клиент.Название_организации));

                case "amount": return View(заказ.ToList().OrderBy(p => p.Сумма_заказа_с_НДС));
                case "amount_desc": return View(заказ.ToList().OrderByDescending(p => p.Сумма_заказа_с_НДС));
                case "status": return View(заказ.ToList().OrderBy(p => p.Статус_заказа));
                case "status_desc": return View(заказ.ToList().OrderByDescending(p => p.Статус_заказа));
                case "emp": return View(заказ.ToList().OrderBy(p => p.Сотрудник.Фамилия));
                case "emp_desc": return View(заказ.ToList().OrderByDescending(p => p.Сотрудник.Фамилия));


                default: return View(заказ.ToList().OrderBy(p => p.ID_заказа));
            }
            //-----------------------------------------------------------------------------------------------------------------
            
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

            var b = db.Заказ.Where(ord => ord.ID_заказа == id)
                .GroupJoin(
                db.Оформление_заказа,
                o => o.ID_заказа,
                of => of.ID_заказа,
                (o, of) => new { amount = of.Sum(oa => oa.Количество * oa.Товар.Цена_с_НДС) });

            var queryGoods = from good in db.Оформление_заказа
                             where good.ID_заказа == заказ.ID_заказа
                             select good;
            //decimal? amount = 0;
            //foreach (var i in queryGoods)
            //{              
            //     amount+= i.Количество * i.Товар.Цена_с_НДС;
            //}         
            ViewBag.Amount = b.First().amount.Value.ToString("0.00");
            ViewBag.ID = заказ.ID_заказа;
            ViewBag.Date = заказ.Дата_заказа.Value.ToShortDateString();
            ViewBag.Client = заказ.Клиент.Название_организации;
            ViewBag.Employee = заказ.Сотрудник.Фамилия;

            var query = from g in db.Оплата_заказа
                             where g.ID_заказа == id
                             select g;
            ViewBag.list = query.ToList();
            return View(queryGoods);
        }
        //getting an invoice for order---------------------------------------------------------------------
        public ActionResult Invoice(int? id)
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
            
            var queryGoods = from good in db.Оформление_заказа
                             where good.ID_заказа == заказ.ID_заказа
                             select good;
            decimal? amount = 0;
            int? timeD = 0;
            foreach (var i in queryGoods)
            {
                amount += i.Количество * i.Товар.Цена_с_НДС;
                timeD = timeD < i.Товар.Срок_поставки ? i.Товар.Срок_поставки : timeD;
            }
            ViewBag.Amount = amount.Value.ToString("0.00");
            ViewBag.ID = заказ.ID_заказа;
            ViewBag.Date = заказ.Дата_заказа.Value.ToShortDateString();
            ViewBag.Client = заказ.Клиент.Название_организации+", "+ "УНП: "+заказ.Клиент.УНП_Клиента+", "+ заказ.Клиент.Адрес+", "+ заказ.Клиент.Телефон+".";
            ViewBag.Employee = заказ.Сотрудник.Фамилия;
            ViewBag.time = timeD;
            return View(queryGoods);
        }     
        //-----------------------------------------------------------------------------------------------------
       
        public ActionResult Create()
        {
            
            ViewBag.D = "none";
            ViewBag.ID_клиента = new SelectList(db.Клиент, "ID_клиента", "Название_организации");
            ViewBag.Табельный_номер = new SelectList(db.Сотрудник, "Табельный_номер", "Фамилия");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(List<int> Количество, List<int> ID)
        {
            if (ModelState.IsValid)
            {
                p.ID_клиента = Convert.ToInt32(Request.Form["ID_заказа"]);
                p.Дата_заказа = Convert.ToDateTime(Request.Form["Дата_заказа"]);
                p.Сумма_заказа_с_НДС = Convert.ToDecimal(Request.Form["Сумма_заказа_с_НДС"]);
                p.ID_клиента = Convert.ToInt32(Request.Form["ID_клиента"]);
                p.Табельный_номер = Convert.ToInt32(Request.Form["Табельный_номер"]);
                

                p.AddtoTable(db, p);
                           
                for (int i=0;i<Количество.Count;i++)
                {
                    //Оформление_заказа o = new Оформление_заказа();
                    o.ID_заказа = p.ID_заказа;
                    o.ID_товара = ID[i];
                    o.Количество = Количество[i];
                    o.AddtoTable(db,o);
                    db.SaveChanges();
                }             
                
                return RedirectToAction("Index");
            }
            ViewBag.ID_клиента = new SelectList(db.Клиент, "ID_клиента", "Название_организации", p.ID_клиента);
            ViewBag.Табельный_номер = new SelectList(db.Сотрудник, "Табельный_номер", "Фамилия", p.Табельный_номер);
            return View(p);
        }
        
        public ActionResult Edit(int? id)
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
            
            var Queryg = from p in db.Оформление_заказа
                         where p.ID_заказа == заказ.ID_заказа
                         select p;
            List<IТоварInterface> list=new List<IТоварInterface>();
            //List<int> list1 = new List<int>();
            foreach(var i in Queryg)
            {
                var t = db.Товар.Find(i.ID_товара);
                t.Остаток_на_складе = i.Количество;
                list.Add(t);              
            }
            ViewBag.List = list;        
            ViewBag.ID_клиента = new SelectList(db.Клиент, "ID_клиента", "Название_организации", заказ.ID_клиента);
            ViewBag.Табельный_номер = new SelectList(db.Сотрудник, "Табельный_номер", "Фамилия", заказ.Табельный_номер);
            return View(заказ);
        }
        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(List<int> Количество, List<int> ID)
        {
            if (ModelState.IsValid)
            {
                p.ID_заказа = Convert.ToInt32(Request.Form["ID_заказа"]);
                p.Дата_заказа = Convert.ToDateTime(Request.Form["Дата_заказа"]);
                p.Сумма_заказа_с_НДС = Convert.ToDecimal(Request.Form["Сумма_заказа_с_НДС"]);
                p.ID_клиента = Convert.ToInt32(Request.Form["ID_клиента"]);
                p.Табельный_номер = Convert.ToInt32(Request.Form["Табельный_номер"]);
                db.Entry(p).State = EntityState.Modified;
                

                var Query = from f in db.Оформление_заказа
                            where f.ID_заказа == p.ID_заказа
                            select f;
                foreach(var i in Query)
                { db.Оформление_заказа.Remove(i); }
                db.SaveChanges();

                for (int i = 0; i<Количество.Count; i++)
                {
                    o.ID_заказа = p.ID_заказа;
                    o.ID_товара = ID[i];
                    o.Количество = Количество[i];
                    o.AddtoTable(db,o);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            ViewBag.ID_клиента = new SelectList(db.Клиент, "ID_клиента", "Номер_паспорта", p.ID_клиента);
            ViewBag.Табельный_номер = new SelectList(db.Сотрудник, "Табельный_номер", "Фамилия", p.Табельный_номер);
            return View(p);
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
            var заказ = db.Заказ.Find(id);
            db.Заказ.Remove(заказ);
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
            return PartialView("AllClients", db.Клиент.ToList());
        }
        //getting list of all goods--------------------------------------------
        [ChildActionOnly]
        public ActionResult AllGoods()
        {
            return PartialView("AllGoods", db.Товар.ToList());
        }
        
        //Export to excel---------------------------------------------------------------
        public ActionResult ExpExcl()
        {
            var grid = new GridView();
            var list = db.Заказ.Select(o => new {Номер=o.ID_заказа,Дата=o.Дата_заказа,Клиент=o.Клиент.Название_организации,Сумма=o.Сумма_заказа_с_НДС,Ответственный=o.Сотрудник.Фамилия });
            grid.DataSource = list.ToList();
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachement; filename=Заказы.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }

        //deleting money from order
        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult DeleteP(int id, int p)
        {
            var query = from q in db.Оплата_заказа
                        where q.ID_заказа == id && q.ID_поступления == p
                        select q;
            db.Оплата_заказа.Remove(query.First());
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }


        //autocomplete function for search field----------------------------------------

        public ActionResult AutocompleteSearch(string term)
        { 
            var result = from N in db.Заказ
                         where N.Клиент.Название_организации.Contains(term)
                         select new { value = N.Клиент.Название_организации };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string search)
        {
            var queryGoods = from good in db.Заказ
                             where good.ID_заказа.ToString().Contains(search) || good.Клиент.Название_организации.Contains(search) || good.Клиент.Фамилия.Contains(search) || good.Сотрудник.Фамилия.Contains(search) || good.Сумма_заказа_с_НДС.Value.ToString().Contains(search)
                             select good;

            if (queryGoods.Count() > 0)
            {
                return PartialView(queryGoods.ToList());
            }

            else return PartialView("NoResult");
        }


        //a report for period----------------------------------------------------------------------
        public ActionResult ROrders(DateTime start, DateTime end)
        {
            var queryGoods = from good in db.Заказ
                             where good.Дата_заказа >= start && good.Дата_заказа <= end
                             select good;

            if (queryGoods.Count() > 0)
            {
                return PartialView(queryGoods.ToList());
            }

            else return PartialView("NoResult");
        }

        
    }
}
