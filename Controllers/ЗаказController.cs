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
                case "num": return View(заказ.AsNoTracking().OrderBy(p => p.ID_заказа));
                case "num_desc": return View(заказ.AsNoTracking().OrderByDescending(p => p.ID_заказа));
                case "date": return View(заказ.AsNoTracking().OrderBy(p => p.Дата_заказа));
                case "date_desc": return View(заказ.AsNoTracking().OrderByDescending(p => p.Дата_заказа));

                case "name": return View(заказ.AsNoTracking().OrderBy(p => p.Клиент.Название_организации));
                case "name_desc": return View(заказ.AsNoTracking().OrderByDescending(p => p.Клиент.Название_организации));

                case "amount": return View(заказ.AsNoTracking().OrderBy(p => p.Сумма_заказа_с_НДС));
                case "amount_desc": return View(заказ.AsNoTracking().OrderByDescending(p => p.Сумма_заказа_с_НДС));
                case "status": return View(заказ.AsNoTracking().OrderBy(p => p.Статус_заказа));
                case "status_desc": return View(заказ.AsNoTracking().OrderByDescending(p => p.Статус_заказа));
                case "emp": return View(заказ.AsNoTracking().OrderBy(p => p.Сотрудник.Фамилия));
                case "emp_desc": return View(заказ.AsNoTracking().OrderByDescending(p => p.Сотрудник.Фамилия));


                default: return View(заказ.AsNoTracking().OrderBy(p => p.ID_заказа));
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

            ViewBag.Amount = db.Заказ.AsNoTracking().Where(ord => ord.ID_заказа == id)
                            .GroupJoin(
                            db.Оформление_заказа,
                            o => o.ID_заказа,
                            of => of.ID_заказа,
                            (o, of) => new { amount = of.Sum(oa => oa.Количество * oa.Товар.Цена_с_НДС) })
                            .FirstOrDefault().amount.Value.ToString("0.00");
                  
            ViewBag.ID = заказ.ID_заказа;
            ViewBag.Date = заказ.Дата_заказа.Value.ToShortDateString();
            ViewBag.Client = заказ.Клиент.Название_организации;
            ViewBag.Employee = заказ.Сотрудник.Фамилия;

            
            ViewBag.list = db.Оплата_заказа.AsNoTracking().Where(g=> g.ID_заказа == id).ToList();

            return View(db.Оформление_заказа.AsNoTracking().Where(d=>d.ID_заказа==id));
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

            var query = db.Оформление_заказа.AsNoTracking().Where(a=>a.ID_заказа==id).Join(db.Товар, of => of.ID_товара, g => g.ID_товара, (of, g) => new { oform = of, good=g   }  );

            ViewBag.Amount = query.Sum(obj=>obj.good.Цена_с_НДС*obj.oform.Количество).Value.ToString("0.00");
            ViewBag.time = query.Max(obj=>obj.good.Срок_поставки);
            ViewBag.ID = заказ.ID_заказа;
            ViewBag.Date = заказ.Дата_заказа.Value.ToShortDateString();
            ViewBag.Client = заказ.Клиент.Название_организации+", "+ "УНП: "+заказ.Клиент.УНП_Клиента+", "+ заказ.Клиент.Адрес+", "+ заказ.Клиент.Телефон+".";
            ViewBag.Employee = заказ.Сотрудник.Фамилия;
          
            return View(db.Оформление_заказа.AsNoTracking().Where(a => a.ID_заказа == id));
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
                    Оформление_заказа o = new Оформление_заказа();
                    o.ID_заказа = p.ID_заказа;
                    o.ID_товара = ID[i];
                    o.Количество = Количество[i];
                    o.AddtoTable(db,o);
                }

                db.SaveChanges();

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

            ViewBag.List = db.Оформление_заказа.AsNoTracking()
                           .Where(p => p.ID_заказа == id)
                           .AsEnumerable()
                           .Join(db.Товар, of => of.ID_товара, g => g.ID_товара, (of, g) => new Товар{
                                                                                                        ID_товара = g.ID_товара,
                                                                                                        Обозначение = g.Обозначение,
                                                                                                        Наименование =g.Наименование,
                                                                                                        Цена =g.Цена,
                                                                                                        Цена_с_НДС =g.Цена_с_НДС,
                                                                                                        Остаток_на_складе =of.Количество
                                                                                                    } );
           
            
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


                var Orderlist = db.Оформление_заказа.Where(o => o.ID_заказа == p.ID_заказа);
                db.Оформление_заказа.RemoveRange(Orderlist);

                for (int i = 0; i < Количество.Count; i++)
                {
                    Оформление_заказа of = new Оформление_заказа();
                    of.ID_заказа = p.ID_заказа;
                    of.ID_товара = ID[i];
                    of.Количество = Количество[i];
                    db.Оформление_заказа.Add(of);
                }
                                
                db.SaveChanges();
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
            return PartialView("AllClients", db.Клиент);
        }
        //getting list of all goods--------------------------------------------
        [ChildActionOnly]
        public ActionResult AllGoods()
        {
            return PartialView("AllGoods", db.Товар);
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
            db.Оплата_заказа.Remove(db.Оплата_заказа.Single(q=> q.ID_заказа == id && q.ID_поступления == p));
            db.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }


        //autocomplete function for search field----------------------------------------

        public ActionResult AutocompleteSearch(string term)
        { 
            return Json(
                db.Заказ.AsNoTracking()
                .Where(N=> N.Клиент.Название_организации.Contains(term))
                .Select(N=> N.Клиент.Название_организации ), JsonRequestBehavior.AllowGet);
        }

        public ActionResult Search(string search)
        {
            var queryGoods = 
                db.Заказ.AsNoTracking()
                .Where(good => good.ID_заказа.ToString().Contains(search) || good.Клиент.Название_организации.Contains(search) || good.Клиент.Фамилия.Contains(search) || good.Сотрудник.Фамилия.Contains(search) || good.Сумма_заказа_с_НДС.Value.ToString().Contains(search));
                

            if (queryGoods.Count() > 0)
            {
                return PartialView(queryGoods);
            }

            else return PartialView("NoResult");
        }


        //a report for period----------------------------------------------------------------------
        public ActionResult ROrders(DateTime start, DateTime end)
        {
            var queryGoods = db.Заказ.AsNoTracking().Where(good => good.Дата_заказа >= start && good.Дата_заказа <= end);
                

            if (queryGoods.Count() > 0)
            {
                return PartialView(queryGoods);
            }

            else return PartialView("NoResult");
        }

        
    }
}
