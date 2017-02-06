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
    public class PieChartItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

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

        //a simple chart about employees results----------------------------------------------------------------------------
        //public ActionResult CreateChart()
        //{
        //    string[] x =new String[30];
        //    int[] y = new Int32[30];
        //    var dbE = db.Сотрудник;
        //    var dbO = db.Заказ;
        //    int i = 0;
        //    foreach (var item in dbE)
        //    {
        //        x[i] = item.Фамилия;
        //        var query = from e in dbO where e.Табельный_номер == item.Табельный_номер select e;
        //        y[i]= query.Count();
        //        i++;                          
        //    }
   
        //        var chart = new SimpleChart.Chart(width: 500, height: 250)
        //          .AddTitle("График результативности продавцов")
        //          .AddSeries(
        //                 name: "Магазин 'Автозапчасти'",
        //                 chartType: "Pie",
        //                 xValue: x,
        //                 yValues: y)
        //          .AddLegend()
        //          .Write();

        //    return null;
        //}
        //----------------------------------------------------------------------------------------------------------
        

       
        public ActionResult Index(string sort)
        {

            //sorting----------------------------------------------------------------------------------------------

            ViewBag.a = "none";
            ViewBag.NumSortParm = sort == "num_desc" ? "num" : "num_desc";
            ViewBag.LastSortParm= sort == "last_desc" ? "last" : "last_desc";
            ViewBag.FirstSortParm = sort == "first_desc" ? "first" : "first_desc";
            ViewBag.PatSortParm = sort == "pat_desc" ? "pat" : "pat_desc";
            ViewBag.PosSortParm= sort == "pos_desc" ? "pos" : "pos_desc";
            ViewBag.BirthSortParm = sort == "birth_desc" ? "birth" : "birth_desc";

            switch (sort)
            {
                case "num": return View(db.Сотрудник.ToList().OrderBy(p => p.Табельный_номер));
                case "num_desc": return View(db.Сотрудник.ToList().OrderByDescending(p => p.Табельный_номер));
                case "last": return View(db.Сотрудник.ToList().OrderBy(p => p.Фамилия));
                case "last_desc": return View(db.Сотрудник.ToList().OrderByDescending(p => p.Фамилия));

                case "first": return View(db.Сотрудник.ToList().OrderBy(p => p.Имя));
                case "first_desc": return View(db.Сотрудник.ToList().OrderByDescending(p => p.Имя));
                case "pat": return View(db.Сотрудник.ToList().OrderBy(p => p.Отчество));
                case "pat_desc": return View(db.Сотрудник.ToList().OrderByDescending(p => p.Отчество));
                case "pos": return View(db.Сотрудник.ToList().OrderBy(p => p.Должность));
                case "pos_desc": return View(db.Сотрудник.ToList().OrderByDescending(p => p.Должность));
                case "birth": return View(db.Сотрудник.ToList().OrderBy(p => p.Дата_рождения));
                case "birth_desc": return View(db.Сотрудник.ToList().OrderByDescending(p => p.Дата_рождения));

                default: return View(db.Сотрудник.ToList().OrderBy(p => p.Табельный_номер));
            }
        //--------------------------------------------------------------------------------------------------------------
           
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

        
        public ActionResult Create()
        {
            return View();
        }

        
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed(/*[Bind(Include = "Табельный_номер,Фамилия,Имя,Отчество,Должность,Телефон,Дата_рождения")] Сотрудник сотрудник*/)
        {
            if (ModelState.IsValid)
            {
                s.Табельный_номер = Convert.ToInt32(Request.Form["Табельный_номер"]);
                s.Фамилия = Request.Form["Фамилия"];
                s.Имя = Request.Form["Имя"];
                s.Отчество = Request.Form["Отчество"];
                s.Должность = Request.Form["Должность"];
                s.Телефон = Request.Form["Телефон"];
                s.Дата_рождения = Convert.ToDateTime(Request.Form["Дата_рождения"]);
                
                s.AddtoTable(db, s);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(s);
        }

     
        public ActionResult Edit(int? id)
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

        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(/*[Bind(Include = "Табельный_номер,Фамилия,Имя,Отчество,Должность,Телефон,Дата_рождения")] Сотрудник сотрудник*/)
        {
            if (ModelState.IsValid)
            {

                s.Табельный_номер = Convert.ToInt32(Request.Form["Табельный_номер"]);
                s.Фамилия = Request.Form["Фамилия"];
                s.Имя = Request.Form["Имя"];
                s.Отчество = Request.Form["Отчество"];
                s.Должность = Request.Form["Должность"];
                s.Телефон = Request.Form["Телефон"];
                s.Дата_рождения = Convert.ToDateTime(Request.Form["Дата_рождения"]);
                db.Entry(s).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(s);
        }

       
        public ActionResult Delete(int? id)
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
        [Authorize(Roles = "admin")]
        
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorEmployee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var сотрудник = db.Сотрудник.Find(id);
            db.Сотрудник.Remove(сотрудник);
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
            var queryGoods = from good in db.Сотрудник
                             where good.Дата_рождения.Value.Year*365 + good.Дата_рождения.Value.Month*30<(DateTime.Now.Year*365+ DateTime.Now.Month*30) -(60*365)
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

        //export to excel----------------------------------------------------------------------------------------
        public ActionResult ExpExcl()
        {

            var grid = new GridView();
            grid.DataSource = db.Сотрудник.ToList();
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachement; filename=Сотрудники.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }
        //----------------------------------------------------------------------------------------------------

        //autocomplete function for search field----------------------------------------
        public ActionResult AutocompleteSearch(string term)
        {
            //Note : you can bind same list from database  

            //Searching records from list using LINQ query  
            var result = from N in db.Сотрудник
                         where N.Фамилия.Contains(term)
                         select new { value = N.Фамилия };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //--------------------------------------------------------------------------------
        public ActionResult Search(string search)
        {
            var queryGoods = from good in db.Сотрудник
                             where good.Фамилия.Contains(search) || good.Должность.Contains(search) || good.Имя.Contains(search) || good.Табельный_номер.ToString().Contains(search)
                             select good;

            if (queryGoods.Count() > 0)
            {
                return PartialView(queryGoods.ToList());
            }

            else return PartialView("NoResult");
        }

        public JsonResult GetDataForChart()
        {
            var result = new List<PieChartItem>();
            
            foreach (var item in db.Сотрудник.Where(e=>e.Должность== "Продавец"))
            {
                var query = from e in db.Заказ where e.Табельный_номер == item.Табельный_номер select e;

                result.Add(new PieChartItem { Name = item.Фамилия, Value = query.Count() });

            }

            return Json(new { Employees = result }, JsonRequestBehavior.AllowGet);
        }
    }
}

