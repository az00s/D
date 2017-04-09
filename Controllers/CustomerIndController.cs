using D.Interfaces;
using D.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace D.Controllers
{
    public class CustomerIndController : Controller
    {
        private IdbInterface db;//dataContext
        private ICustomerIndInterface p;

        public CustomerIndController(IdbInterface dbParam, ICustomerIndInterface pParam)//dependency injection via constructor
        {

            db = dbParam;
            p = pParam;


        }
        // GET: CustomerInd
        public ActionResult Index()
        {
            ViewBag.LastNameSortParm = "last";
            ViewBag.NameSortParm = "name";
            ViewBag.PatronimycSortParm = "pat";
            ViewBag.BirstDateSortParm = "birst";
            

            return View(db.CustomerInd.AsNoTracking().OrderBy(p => p.CustomerIndId));
        }
        
        public ActionResult Sorting(string sort)
        {
            ViewBag.LastNameSortParm = sort == "last_desc" ? "last" : "last_desc";
            ViewBag.NameSortParm = sort == "name_desc" ? "name" : "name_desc";
            ViewBag.PatronimycSortParm = sort == "pat_desc" ? "pat" : "pat_desc";
            ViewBag.BirstDateSortParm = sort == "birst_desc" ? "birst" : "birst_desc";

            switch (sort)
            {
                case "last": return PartialView("Table", db.CustomerInd.AsNoTracking().OrderBy(p => p.LastName));
                case "last_desc": return PartialView("Table", db.CustomerInd.AsNoTracking().OrderByDescending(p => p.LastName));
                case "name": return PartialView("Table", db.CustomerInd.AsNoTracking().OrderBy(p => p.FirstName));
                case "name_desc": return PartialView("Table", db.CustomerInd.AsNoTracking().OrderByDescending(p => p.FirstName));
                case "pat": return PartialView("Table", db.CustomerInd.AsNoTracking().OrderBy(p => p.Patronymic));
                case "pat_desc": return PartialView("Table", db.CustomerInd.AsNoTracking().OrderByDescending(p => p.Patronymic));
                case "birst": return PartialView("Table", db.CustomerInd.AsNoTracking().OrderBy(p => p.BirstDate));
                case "birst_desc": return PartialView("Table", db.CustomerInd.AsNoTracking().OrderByDescending(p => p.BirstDate));


                default: return PartialView("Table", db.CustomerInd.AsNoTracking().OrderBy(p => p.CustomerIndId));
            }
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var клиент = db.CustomerInd.Find(id);
            if (клиент == null)
            {
                return HttpNotFound();
            }
            return View(клиент);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed([Bind(Include = "ID_клиента,Номер_паспорта,УНП_Клиента,Название_организации,Телефон,Адрес,Фамилия,Имя,Отчество")] CustomerEnt p)
        {
            if (ModelState.IsValid)
            {
                //p.ID_клиента = Convert.ToInt32(Request.Form["ID_клиента"]);
                //p.Номер_паспорта = Request.Form["Номер_паспорта"];
                //p.УНП_Клиента = Convert.ToInt32(Request.Form["УНП_Клиента"]);
                //p.Название_организации = Request.Form["Название_организации"];
                //p.Телефон = Request.Form["Телефон"];
                //p.Адрес = Request.Form["Адрес"];
                //p.Фамилия = Request.Form["Фамилия"];
                //p.Имя = Request.Form["Имя"];
                //p.Отчество = Request.Form["Отчество"];

                p.AddtoTable(db, p);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(p);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var клиент = db.CustomerInd.Find(id);
            if (клиент == null)
            {
                return HttpNotFound();
            }
            return View(клиент);
        }


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerIndId,PassportId,Email,Telephone,Adress,BirstDate,Description,FirstName,LastName,Patronymic,RegisteredDate")] CustomerInd p)
        {
            if (ModelState.IsValid)
            {
                //CustomerInd pp = new CustomerInd();
                //pp.CustomerIndId = Convert.ToInt32(Request.Form["CustomerIndId"]);
                //pp.PassportId = Request.Form["PassportId"];
                //pp.Email = Request.Form["Email"];
                //pp.Telephone = Request.Form["Telephone"];
                //pp.Adress = Request.Form["Adress"];
                //pp.Description = Request.Form["Description"];
                //pp.FirstName = Request.Form["FirstName"];
                //pp.LastName = Request.Form["LastName"];
                //pp.Patronymic = Request.Form["Patronymic"];
                //pp.BirstDate= DateTime.Parse(Request.Form["BirstDate"]);
                //pp.RegisteredDate= DateTime.Parse(Request.Form["RegisteredDate"]);
                db.Entry(p).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(p);
        }


        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var клиент = db.CustomerEnt.Find(id);
        //    if (клиент == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(клиент);
        //}


        [Authorize(Roles = "admin")]
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorClients")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            db.CustomerInd.Remove(db.CustomerInd.Find(id));
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

        //a report about clients from Mogilev
        public ActionResult CMogilev()
        {
            return PartialView("Table", db.CustomerEnt.AsNoTracking().Where(cl => cl.Адрес.Contains("могил")));
        }
        //-----------------------------------------------------------------------------

        //autocomplete function for search field----------------------------------------
        public ActionResult AutocompleteSearch(string term)
        {
            return Json(db.CustomerEnt
                .AsNoTracking()
                .Where(cl => cl.Название_организации.Contains(term))
                .Select(c => new { value = c.Название_организации })
                , JsonRequestBehavior.AllowGet);
        }
        //----------------------------------------------------------------------------------

        public ActionResult Search(string search)
        {
            var query = db.CustomerEnt
                .AsNoTracking()
                .Where(c => c.Название_организации.Contains(search) || c.УНП_Клиента.ToString().Contains(search));


            if (query.Count() > 0)
            {
                return PartialView(query);
            }

            else return PartialView("NoResult");
        }

    }
}