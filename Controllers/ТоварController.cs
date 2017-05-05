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
        public ActionResult Index(int pagesize = 10)
        { 
            return RedirectToAction("Table");
        }

       
        public ActionResult Table(string sort,int page = 1, int pagesize = 10,string search=null)
        {
            IQueryable<Товар> request = db.Товар.AsNoTracking();

            //searching----------------------------------------------------------------------------------------------
            if (search != null)
            {
                request = db.Товар.AsNoTracking().Where(s => s.Наименование.Contains(search) || s.Обозначение.Contains(search));
                if (request.LongCount() <1)
                { return PartialView("NoResult"); }
            }

            ViewBag.SearhRequest = search;
            //sorting----------------------------------------------------------------------------------------------

            ViewBag.SortParam = sort;
            ViewBag.NumSortParm = sort == "number_desc" ? "number" : "number_desc";
            ViewBag.NameSortParm = sort == "name_desc" ? "name" : "name_desc";
            ViewBag.OstSortParm = sort == "ost_desc" ? "ost" : "ost_desc";
            ViewBag.DaySortParm = sort == "day_desc" ? "day" : "day_desc";
            ViewBag.WeightSortParm = sort == "weight_desc" ? "weight" : "weight_desc";
            ViewBag.PriceSortParm = sort == "price_desc" ? "price" : "price_desc";
            ViewBag.PriceVatSortParm = sort == "pricevat_desc" ? "pricevat" : "pricevat_desc";
            //Paging--------------------------------------------------------------------------------------------------
            ViewBag.CurrentPage = page;
            ViewBag.PageCount = request.LongCount() % pagesize == 0 ?
                                        request.LongCount() / pagesize : 
                                       (request.LongCount() / pagesize) + 1;

            ViewBag.PageCountResult = ViewBag.PageCount < 11 ? ViewBag.PageCount : 7;
            //----------------------------------------------------------------------------------------------------------


            switch (sort)
            {
                case "number_desc":
                    {
                        ViewBag.Num = "&#9650;";
                        return View("Table", request.OrderByDescending(p => p.Обозначение).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "number":
                    {
                        ViewBag.Num = "&#9660;";
                        return View("Table", request.OrderBy(p => p.Обозначение).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "name":
                    {
                        ViewBag.Name = "&#9650;";
                        return View("Table", request.OrderBy(p => p.Наименование).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "name_desc":
                    {
                        ViewBag.Name = "&#9660;";
                        return View("Table", request.OrderByDescending(p => p.Наименование).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "ost":
                    {
                        ViewBag.Ost= "&#9650;";
                        return View("Table", request.OrderBy(p => p.Остаток_на_складе).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "ost_desc":
                    {
                        ViewBag.Ost = "&#9660;";
                        return View("Table", request.OrderByDescending(p => p.Остаток_на_складе).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "day":
                    {
                        ViewBag.Day = "&#9650;";
                        return View("Table", request.OrderBy(p => p.Срок_поставки).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "day_desc":
                    {
                        ViewBag.Day = "&#9660;";
                        return View("Table", request.OrderByDescending(p => p.Срок_поставки).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "weight":
                    {
                        ViewBag.Weight = "&#9650;";
                        return View("Table", request.OrderBy(p => p.Вес).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "weight_desc":
                    {
                        ViewBag.Weight = "&#9660;";
                        return View("Table", request.OrderByDescending(p => p.Вес).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "price":
                    {
                        ViewBag.Price = "&#9650;";
                        return View("Table", request.OrderBy(p => p.Цена).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "price_desc":
                    {
                        ViewBag.Price = "&#9660;";
                        return View("Table", request.OrderByDescending(p => p.Цена).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "pricevat":
                    {
                        ViewBag.PriceVat = "&#9650;";
                        return View("Table", request.OrderBy(p => p.Цена_с_НДС).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                case "pricevat_desc":
                    {
                        ViewBag.PriceVat = "&#9660;";
                        return View("Table", request.OrderByDescending(p => p.Цена_с_НДС).Skip((page - 1) * pagesize).Take(pagesize));
                    }
                default: return View("Table", request.OrderBy(p => p.ID_товара).Skip((page - 1) * pagesize).Take(pagesize));
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
        public ActionResult ROst_0(int page = 1, int pagesize = 10)

        {
            var request = db.Товар.AsNoTracking().Where(s => s.Остаток_на_складе == 0);
            //Paging--------------------------------------------------------------------------------------------------
            ViewBag.CurrentPage = page;
            ViewBag.PageCount = request.LongCount() % pagesize == 0 ?
                                        request.LongCount() / pagesize :
                                       (request.LongCount() / pagesize) + 1;

            ViewBag.PageCountResult = ViewBag.PageCount < 11 ? ViewBag.PageCount : 7;

            return View(request.OrderBy(p => p.ID_товара).Skip((page - 1) * pagesize).Take(pagesize));
        }

        //a report about quantities of goods
            public ActionResult ROst(int page = 1, int pagesize = 10)
        {
            var request = db.Товар.AsNoTracking();
            //Paging--------------------------------------------------------------------------------------------------
            ViewBag.CurrentPage = page;
            ViewBag.PageCount = request.LongCount() % pagesize == 0 ?
                                        request.LongCount() / pagesize :
                                       (request.LongCount() / pagesize) + 1;

            ViewBag.PageCountResult = ViewBag.PageCount < 11 ? ViewBag.PageCount : 7;

            return View(request.OrderBy(p => p.ID_товара).Skip((page - 1) * pagesize).Take(pagesize));

            
        }

               

        //a report about prices
        public ActionResult Price(int page = 1, int pagesize = 10)
        {
            var request = db.Товар.AsNoTracking();
            //Paging--------------------------------------------------------------------------------------------------
            ViewBag.CurrentPage = page;
            ViewBag.PageCount = request.LongCount() % pagesize == 0 ?
                                        request.LongCount() / pagesize :
                                       (request.LongCount() / pagesize) + 1;

            ViewBag.PageCountResult = ViewBag.PageCount < 11 ? ViewBag.PageCount : 7;

            return View(request.OrderBy(p => p.ID_товара).Skip((page - 1) * pagesize).Take(pagesize));
        
            
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

        
        //public ActionResult Search(string search, int page = 1, int pagesize = 10)
        //{
        //    var queryGoods = db.Товар.AsNoTracking().Where(s => s.Наименование.Contains(search) || s.Обозначение.Contains(search));
        //    ViewBag.SearhRequest = search;
        //    if (queryGoods.LongCount() > 0)
        //    {
        //        ViewBag.CurrentPage = page;
        //        ViewBag.PageCount = queryGoods.LongCount() % pagesize == 0 ?
        //                                    queryGoods.LongCount() / pagesize :
        //                                   (queryGoods.LongCount() / pagesize) + 1;

        //        ViewBag.PageCountResult = ViewBag.PageCount < 11 ? ViewBag.PageCount : 7;
        //        return              View(queryGoods
        //                                .OrderBy(p => p.ID_товара)
        //                                .Skip((page - 1) * pagesize)
        //                                .Take(pagesize));
        //    }

        //    else return PartialView("NoResult");
        //}

        

    }
}
