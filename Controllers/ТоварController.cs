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

       
        
       

        public ActionResult Index(string sort)
        {
            
            ViewBag.a = "none";
        //sorting----------------------------------------------------------------------------------------------
            ViewBag.NumSortParm = sort=="number_desc" ? "number" :"number_desc";
            ViewBag.NameSortParm = sort == "name_desc" ? "name" : "name_desc";
            ViewBag.OstSortParm= sort == "ost_desc" ? "ost" : "ost_desc";
            ViewBag.DaySortParm=sort == "day_desc" ? "day" : "day_desc";
            ViewBag.WeightSortParm= sort == "weight_desc" ? "weight" : "weight_desc";
            ViewBag.PriceSortParm= sort == "price_desc" ? "price" : "price_desc";
            ViewBag.PriceVatSortParm= sort == "pricevat_desc" ? "pricevat" : "pricevat_desc";


            switch (sort)
            {
                case "number_desc": return View(db.Товар.ToListAsync().Result.OrderByDescending(p=>p.Обозначение));
                case "number": return View(db.Товар.ToListAsync().Result.OrderBy(p => p.Обозначение));
                case "name": return View(db.Товар.ToListAsync().Result.OrderBy(p => p.Наименование));
                case "name_desc": return View(db.Товар.ToListAsync().Result.OrderByDescending(p => p.Наименование));            
                case "ost": return View(db.Товар.ToListAsync().Result.OrderBy(p => p.Остаток_на_складе));
                case "ost_desc": return View(db.Товар.ToListAsync().Result.OrderByDescending(p => p.Остаток_на_складе));
                case "day": return View(db.Товар.ToListAsync().Result.OrderBy(p => p.Срок_поставки));
                case "day_desc": return View(db.Товар.ToListAsync().Result.OrderByDescending(p => p.Срок_поставки));
                case "weight": return View(db.Товар.ToListAsync().Result.OrderBy(p => p.Вес));
                case "weight_desc": return View(db.Товар.ToListAsync().Result.OrderByDescending(p => p.Вес));
                case "price": return View(db.Товар.ToListAsync().Result.OrderBy(p => p.Цена));
                case "price_desc": return View(db.Товар.ToListAsync().Result.OrderByDescending(p => p.Цена));
                case "pricevat": return View(db.Товар.ToListAsync().Result.OrderBy(p => p.Цена_с_НДС));
                case "pricevat_desc": return View(db.Товар.ToListAsync().Result.OrderByDescending(p => p.Цена_с_НДС));

                default: return View(db.Товар.ToListAsync().Result.OrderBy(p => p.ID_товара));
            }
        //----------------------------------------------------------------------------------------------------
        }
        
       
        //autocomplete function for search field----------------------------------------
        public ActionResult AutocompleteSearch(string term)
        {
                         var result = from N in db.Товар.AsParallel()
                          where N.Наименование.Contains(term)
                            select new { value = N.Наименование };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        //------------------------------------------------------------------------------



    
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var товар = db.Товар.Find(id);
            if (товар == null)
            {
                return HttpNotFound();
            }
                var query = from g in db.Поставщик_цена
                where g.ID_товара == id
                select g;
            
            return View(query.ToList());
        }

        
        public ActionResult Create()
        {
            return View();
        }


        [ActionName("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed(/*[Bind(Include = "Обозначение,Наименование,Краткое_описание,Цена,Остаток_на_складе,Единица_измерения,Срок_поставки,Вес,Цена_с_НДС")] Товар товар*/)
        {
            if (ModelState.IsValid)
            {
                товар.Обозначение = Request.Form["Обозначение"];
                товар.Наименование = Request.Form["Наименование"];
                товар.Краткое_описание = Request.Form["Краткое_описание"];
                товар.Цена = Convert.ToDecimal(Request.Form["Цена"]);
                товар.Остаток_на_складе = Convert.ToInt32(Request.Form["Остаток_на_складе"]);
                товар.Единица_измерения = Request.Form["Единица_измерения"];
                товар.Срок_поставки = Convert.ToInt32(Request.Form["Срок_поставки"]);
                товар.Вес = Convert.ToInt32(Request.Form["Вес"]);

                товар.AddtoTable(db,товар);
                //Поставщик_цена p = new Поставщик_цена();
                p.ID_товара = товар.ID_товара;
                p.УНП_поставщика= Convert.ToInt32(Request.Form["УНП_поставщика"]);
                p.AddtoTable(db,p);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(товар);
        }

        
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var товар = db.Товар.Find(id);
            if (товар == null)
            {
                return HttpNotFound();
            }
            return View(товар);
        }

       
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit()
        {
  
            if (ModelState.IsValid)
            {
                товар=db.Товар.Find(Convert.ToInt32(Request.Form["ID_товара"]));
                товар.Обозначение = Request.Form["Обозначение"];
                товар.Наименование = Request.Form["Наименование"];
                товар.Краткое_описание = Request.Form["Краткое_описание"];
                товар.Цена = Convert.ToDecimal(Request.Form["Цена"]);
                товар.Остаток_на_складе = Convert.ToInt32(Request.Form["Остаток_на_складе"]);
                товар.Единица_измерения = Request.Form["Единица_измерения"];
                товар.Срок_поставки = Convert.ToInt32(Request.Form["Срок_поставки"]);
                товар.Вес = Convert.ToInt32(Request.Form["Вес"]);


                db.Entry(товар).State = EntityState.Modified;
                

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(товар);
        }

       
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var товар = db.Товар.Find(id);
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
            var товар = db.Товар.Find(id);
            db.Товар.Remove(товар);
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
           return PartialView("AllProviders", db.Поставщик.ToList());
        }

        //report about goods,which quantity equals 0       
        public ActionResult ROst_0()
        {
            
            var queryGoods = from good in db.Товар
                             where good.Остаток_на_складе==0
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

        //a report about quantities of goods
            public ActionResult ROst()
        {           
            ViewBag.a = "none";      
            return View("ROst", db.Товар.ToList());
        }

        //export to excel
        public ActionResult ExpExcl()
        {
            var grid = new GridView();
            grid.DataSource = db.Товар.ToList(); 
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachement; filename=Товары.xls");
            Response.ContentType = "application/excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
            return View();
        }

        //a report about prices
        public ActionResult Price()
        {
            ViewBag.a = "none";
            return View(db.Товар.ToList());
        }

        [HttpPost]
        //deleting provider for goods
        public ActionResult DeleteP(int id,int p)
        {
            var query = from q in db.Поставщик_цена
                               where q.ID_товара == id && q.УНП_поставщика == p
                               select q;
            db.Поставщик_цена.Remove(query.First());
            db.SaveChanges();


            var query1 = from g in db.Поставщик_цена
                        where g.ID_товара == id
                        select g;


            return View("Details",query1.ToList());
        }

        //adding provider for goods-------------------------------------------------
       
        public ActionResult AddP(int id)
        {
            var queryGoods = from good in db.Товар
                             where good.ID_товара == id
                             select good;

            ViewBag.number = (queryGoods.First()).Обозначение;
            ViewBag.name= (queryGoods.First()).Наименование;
            ViewBag.id = id;
            var query = from s in db.Поставщик_цена
                        where s.ID_товара == id
                        select s.Поставщик;

            
            var p= db.Поставщик.Except(query);

            return View("AddP",p );
        }
        [HttpPost]
        public ActionResult AddP(int id,int u,decimal? price)
        {
            //Поставщик_цена o = new Поставщик_цена();
            p.ID_товара = id;
            p.УНП_поставщика = u;
            p.Оптовая_цена = price;
            p.AddtoTable(db,p);
            db.SaveChanges();

            return RedirectToAction("Details",new { id = id });
        }
        //-------------------------------------------------------------------------------------------------

        
        public ActionResult Search(string search)
        {
            var queryGoods = from good in db.Товар
            where good.Наименование.Contains(search) || good.Обозначение.Contains(search)
            select good;

            if (queryGoods.Count() > 0)
            {
                return PartialView(queryGoods.ToList());
            }

            else return PartialView("NoResult");
        }

        

    }
}
