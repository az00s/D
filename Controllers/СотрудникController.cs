using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using D.Models;
using SimpleChart = System.Web.Helpers;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using D.Interfaces;
using System.Collections.Generic;

namespace D.Controllers
{
    
     //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [Authorize(Roles = "accountant,manager,admin")]
    public class СотрудникController : Controller
    {
        private IdbInterface db;//dataContext
        private IСотрудникInterface s;
        
        public СотрудникController(IdbInterface dbParam, IСотрудникInterface sParam)//dependency injection via constructor
        {

            db = dbParam;
            s = sParam;
            

        }

        //----------------------------------------------------------------------------------------------------------
           
        public ActionResult Index()
        {

            return RedirectToAction("Table");
        }

        public ActionResult Table ()
        {
            return View("Table",db.Сотрудник.AsNoTracking().OrderBy(o=>o.Табельный_номер));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var сотрудник = db.Сотрудник.Find(id);
            if (сотрудник == null)
            {
                return HttpNotFound();
            }
            return View(сотрудник);
        }

        
        //public ActionResult Create()
        //{
        //    return View();
        //}

        
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed([Bind(Include = "Табельный_номер,Фамилия,Имя,Отчество,Должность,Телефон,Дата_рождения,Адрес,Номер_паспорта")] Сотрудник s)
        {
            if (ModelState.IsValid)
            {
                //s.Табельный_номер = Convert.ToInt32(Request.Form["Табельный_номер"]);
                //s.Фамилия = Request.Form["Фамилия"];
                //s.Имя = Request.Form["Имя"];
                //s.Отчество = Request.Form["Отчество"];
                //s.Должность = Request.Form["Должность"];
                //s.Телефон = Request.Form["Телефон"];
                //s.Дата_рождения = Convert.ToDateTime(Request.Form["Дата_рождения"]);
                
                s.AddtoTable(db, s);
                
                db.SaveChanges();
                return RedirectToAction("Details",new { id=s.Табельный_номер});
            }

            return RedirectToAction("Table");
        }

     
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var сотрудник = db.Сотрудник.Find(id);
        //    if (сотрудник == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(сотрудник);
        //}

        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Табельный_номер,Фамилия,Имя,Отчество,Должность,Телефон,Дата_рождения,Адрес,Номер_паспорта")] Сотрудник s)
        {
            if (ModelState.IsValid)
            {

                //s.Табельный_номер = Convert.ToInt32(Request.Form["Табельный_номер"]);
                //s.Фамилия = Request.Form["Фамилия"];
                //s.Имя = Request.Form["Имя"];
                //s.Отчество = Request.Form["Отчество"];
                //s.Должность = Request.Form["Должность"];
                //s.Телефон = Request.Form["Телефон"];
                //s.Дата_рождения = Convert.ToDateTime(Request.Form["Дата_рождения"]);
                db.Entry(s).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details",new { id=s.Табельный_номер});
            }
            return View(s);
        }

       
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var сотрудник = db.Сотрудник.Find(id);
        //    if (сотрудник == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(сотрудник);
        //}
        [Authorize(Roles = "admin")]
        
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorEmployee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
                db.Сотрудник.Remove(db.Сотрудник.Find(id));
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


        //a report about employees, who older than 60 years--------------------------------------- 
        public ActionResult REmployee60()
        {
            
            var queryGoods = db.Сотрудник.AsNoTracking().Where(emp => (DateTime.Now.Year - emp.Дата_рождения.Value.Year) * 8755.2 > 525312);
            
            if (queryGoods.Count() > 0)
            {
                return View("Table", queryGoods);
            }

            else return View("NoResult");
        }

        

        //autocomplete function for search field----------------------------------------
        //public ActionResult AutocompleteSearch(string term)
        //{
            
        //    return Json(db.Сотрудник
        //        .AsNoTracking().Where(emp=>emp.Фамилия.Contains(term))
        //        .Select(s => new { value=s.Фамилия }), 
        //        JsonRequestBehavior.AllowGet);
        //}
        //--------------------------------------------------------------------------------
        //public ActionResult Search(string search)
        //{
        //    var queryGoods = db.Сотрудник
        //        .AsNoTracking()
        //        .Where(good => good.Фамилия.Contains(search) || good.Должность.Contains(search) || good.Имя.Contains(search) || good.Табельный_номер.ToString().Contains(search));
                             
        //    if (queryGoods.Count() > 0)
        //    {
        //        return PartialView(queryGoods);
        //    }

        //    else return PartialView("NoResult");
        //}

        public JsonResult GetDataForChart()
        {
            var result=db.Сотрудник
                .Where(e=>e.Должность== "Продавец")
                .GroupJoin(
                db.Заказ,emp=>emp.Табельный_номер,ord=>ord.Табельный_номер,(emp,ord)=> new PieChartItem {
                    Name = emp.Фамилия, Value = db.Заказ.Where(s=>s.Табельный_номер==emp.Табельный_номер).Count() });
            

            return Json(new { Employees = result }, JsonRequestBehavior.AllowGet);
        }
    }
    public class PieChartItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

}

