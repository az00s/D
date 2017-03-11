using System;
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
using System.Collections.Generic;
using System.Web.Helpers;
using System.Threading.Tasks;
using D.Infrastructure;
using System.Collections;

namespace D.Controllers
{
   
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [Authorize]
    [HandleError(ExceptionType = typeof(SystemException), View = "Error")]
    public class ТоварController : Controller
    {
        private IdbInterface db;//dataContext
        private IПоставщик_ценаInterface p;
        private IТоварInterface товар;
        public ТоварController(IdbInterface dbParam, IПоставщик_ценаInterface pParam, IТоварInterface товарParam)//dependency injection via constructor
        {
            
            db = dbParam;
            p = pParam;
            товар=товарParam;
           
        }
        public ActionResult Index()
        {
           
            ViewBag.NumSortParm ="number_desc";
            ViewBag.NameSortParm ="name" ;
            ViewBag.OstSortParm =  "ost";
            ViewBag.DaySortParm = "day";
            ViewBag.WeightSortParm =  "weight";
            ViewBag.PriceSortParm = "price";
            ViewBag.PriceVatSortParm = "pricevat";
           
            return View(db.Товар.AsNoTracking().OrderBy(p => p.ID_товара));
        }

        public ActionResult Sorting(string sort)
        {
            //sorting----------------------------------------------------------------------------------------------
            ViewBag.NumSortParm = sort == "number_desc" ? "number" : "number_desc";
            ViewBag.NameSortParm = sort == "name_desc" ? "name" : "name_desc";
            ViewBag.OstSortParm = sort == "ost_desc" ? "ost" : "ost_desc";
            ViewBag.DaySortParm = sort == "day_desc" ? "day" : "day_desc";
            ViewBag.WeightSortParm = sort == "weight_desc" ? "weight" : "weight_desc";
            ViewBag.PriceSortParm = sort == "price_desc" ? "price" : "price_desc";
            ViewBag.PriceVatSortParm = sort == "pricevat_desc" ? "pricevat" : "pricevat_desc";


            switch (sort)
            {
                case "number_desc": return PartialView("Table",db.Товар.AsNoTracking().OrderByDescending(p => p.Обозначение));
                case "number": return PartialView("Table", db.Товар.AsNoTracking().OrderBy(p => p.Обозначение));
                case "name": return PartialView("Table", db.Товар.AsNoTracking().OrderBy(p => p.Наименование));
                case "name_desc": return PartialView("Table", db.Товар.AsNoTracking().OrderByDescending(p => p.Наименование));
                case "ost": return PartialView("Table", db.Товар.AsNoTracking().OrderBy(p => p.Остаток_на_складе));
                case "ost_desc": return PartialView("Table", db.Товар.AsNoTracking().OrderByDescending(p => p.Остаток_на_складе));
                case "day": return PartialView("Table", db.Товар.AsNoTracking().OrderBy(p => p.Срок_поставки));
                case "day_desc": return PartialView("Table", db.Товар.AsNoTracking().OrderByDescending(p => p.Срок_поставки));
                case "weight": return PartialView("Table", db.Товар.AsNoTracking().OrderBy(p => p.Вес));
                case "weight_desc": return PartialView("Table", db.Товар.AsNoTracking().OrderByDescending(p => p.Вес));
                case "price": return PartialView("Table", db.Товар.AsNoTracking().OrderBy(p => p.Цена));
                case "price_desc": return PartialView("Table", db.Товар.AsNoTracking().OrderByDescending(p => p.Цена));
                case "pricevat": return PartialView("Table", db.Товар.AsNoTracking().OrderBy(p => p.Цена_с_НДС));
                case "pricevat_desc": return PartialView("Table", db.Товар.AsNoTracking().OrderByDescending(p => p.Цена_с_НДС));

                default: return PartialView("Table", db.Товар.AsNoTracking().OrderBy(p => p.ID_товара));
            }
        }
        
       
        //autocomplete function for search field----------------------------------------
        public ActionResult AutocompleteSearch(string term)
        {
            return Json(db.Товар.AsNoTracking().Where(s => s.Наименование.Contains(term)).Select(s => s.Наименование), JsonRequestBehavior.AllowGet);
        }
        //------------------------------------------------------------------------------



    
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            if (db.Товар.Find(id) == null)
            {
                return HttpNotFound();
            }
                            
            return View(db.Поставщик_цена.AsNoTracking().Where(s=>s.ID_товара==id));
        }

        
        public ActionResult Create()
        {
            return View();
        }


        [ActionName("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed([Bind(Include = "Обозначение,Наименование,Краткое_описание,Цена,Остаток_на_складе,Единица_измерения,Срок_поставки,Вес")] Товар товар)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    //товар.Обозначение = Request.Form["Обозначение"];
                    //товар.Наименование = Request.Form["Наименование"];
                    //товар.Краткое_описание = Request.Form["Краткое_описание"];
                    //товар.Цена = Convert.ToDecimal(Request.Form["Цена"]);
                    //товар.Остаток_на_складе = Convert.ToInt32(Request.Form["Остаток_на_складе"]);
                    //товар.Единица_измерения = Request.Form["Единица_измерения"];
                    //товар.Срок_поставки = Convert.ToInt32(Request.Form["Срок_поставки"]);
                    //товар.Вес = Convert.ToInt32(Request.Form["Вес"]);

                    товар.AddtoTable(db, товар);

                    p.ID_товара = товар.ID_товара;
                    p.УНП_поставщика = Convert.ToInt32(Request.Form["УНП_поставщика"]);
                    p.AddtoTable(db, p);
                    db.SaveChanges();


                    return RedirectToAction("Index");
                }
            }
            catch (Exception)

            { return View("Error"); }
            

            return View(товар);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            товар = db.Товар.Find(id);
            if (товар == null)
            {
                return HttpNotFound();
            }
            return View(товар);
        }

       
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_товара,Обозначение,Наименование,Краткое_описание,Цена,Остаток_на_складе,Единица_измерения,Срок_поставки,Вес")] Товар товар)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //товар = db.Товар.Find(Convert.ToInt32(Request.Form["ID_товара"]));
                    //товар.Обозначение = Request.Form["Обозначение"];
                    //товар.Наименование = Request.Form["Наименование"];
                    //товар.Краткое_описание = Request.Form["Краткое_описание"];
                    //товар.Цена = Convert.ToDecimal(Request.Form["Цена"]);
                    //товар.Остаток_на_складе = Convert.ToInt32(Request.Form["Остаток_на_складе"]);
                    //товар.Единица_измерения = Request.Form["Единица_измерения"];
                    //товар.Срок_поставки = Convert.ToInt32(Request.Form["Срок_поставки"]);
                    //товар.Вес = Convert.ToInt32(Request.Form["Вес"]);


                    db.Entry(товар).State = EntityState.Modified;


                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }

            catch (Exception)

            { return View("Error"); }

            return View(товар);
        }

       
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            товар = db.Товар.Find(id);
            if (товар == null)
            {
                return HttpNotFound();
            }
            return View(товар);
        }

        [Authorize(Roles = "admin")]
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorGoods")]
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
                db.Товар.Remove(db.Товар.Find(id));
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

        
        //getting a list off all providers 
        [ChildActionOnly]
        public ActionResult AllProviders()
        {
           return PartialView("AllProviders", db.Поставщик);
        }

        //report about goods,which quantity equals 0       
        public ActionResult ROst_0()
        {                  
            return PartialView("Search", db.Товар.AsNoTracking().Where(s => s.Остаток_на_складе == 0));
        }

        //a report about quantities of goods
            public ActionResult ROst()
        {           
                
            return PartialView("ROst", db.Товар);
        }

               

        //a report about prices
        public ActionResult Price()
        {
            return PartialView(db.Товар);
        }

        [HttpPost]
        //deleting provider for goods
        public ActionResult DeleteP(int id,int p)
        {
            try
            {
                db.Поставщик_цена.Remove(db.Поставщик_цена.Single(q => q.ID_товара == id && q.УНП_поставщика == p));
                db.SaveChanges();
            }
            catch (Exception)
            {
                return View("Error");
            }
         

            return View("Details", db.Поставщик_цена.AsNoTracking().Where(g => g.ID_товара == id));
        }

        //adding provider for goods-------------------------------------------------
       
        public ActionResult AddP(int id)
        {
            ViewBag.Item= db.Товар.Find(id);
            var p= db.Поставщик.AsNoTracking().Except(
                
                db.Поставщик_цена.AsNoTracking().Where(s=>s.ID_товара==id).Select(s=>s.Поставщик)
                                    );

            return View("AddP",p );
        }
        [HttpPost]
        public ActionResult AddP(int id,int u,decimal? price=0)
        {
            
            try
            {
                p.ID_товара = id;
                p.УНП_поставщика = u;
                p.Оптовая_цена = price;
                p.AddtoTable(db, p);
                db.SaveChanges();
            }
            catch (Exception)
            { return View("Error"); }

            return RedirectToAction("Details",new { id = id });
        }
        //-------------------------------------------------------------------------------------------------

        
        public ActionResult Search(string search)
        {
            var queryGoods = db.Товар.AsNoTracking().Where(s => s.Наименование.Contains(search) || s.Обозначение.Contains(search));

            if (queryGoods.Count() > 0)
            {
                return PartialView(queryGoods);
            }

            else return PartialView("NoResult");
        }

        

    }
}
