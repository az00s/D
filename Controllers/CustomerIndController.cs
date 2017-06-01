using D.Interfaces;
using D.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
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
            return RedirectToAction("Table");
        }
        
        public ActionResult Table()
        {
            return View("Table",db.CustomerInd.AsNoTracking().OrderBy(o=>o.CustomerIndId));
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


        //public ActionResult Create()
        //{
        //    return View();
        //}


        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed([Bind(Include = "CustomerIndId,Description,BirstDate,Email,Telephone,Adress,LastName,FirstName,Patronymic,PassportId")] CustomerInd customer)
        {
            //if (ModelState.IsValid)
            //{
            //p.ID_клиента = Convert.ToInt32(Request.Form["ID_клиента"]);
            //p.Номер_паспорта = Request.Form["Номер_паспорта"];
            //p.УНП_Клиента = Convert.ToInt32(Request.Form["УНП_Клиента"]);
            //p.Название_организации = Request.Form["Название_организации"];
            //p.Телефон = Request.Form["Телефон"];
            //p.Адрес = Request.Form["Адрес"];
            //p.Фамилия = Request.Form["Фамилия"];
            //p.Имя = Request.Form["Имя"];
            //p.Отчество = Request.Form["Отчество"];
           
                customer.AddtoTable(db, customer);

                db.SaveChanges();
                return RedirectToAction("Table");
            //}

            return View("Error");
        }


        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    var клиент = db.CustomerInd.Find(id);
        //    if (клиент == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(клиент);
        //}


        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerIndId,PassportId,Email,Telephone,Adress,BirstDate,Description,FirstName,LastName,Patronymic,RegisteredDate")] CustomerInd customer)
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
                //pp.BirstDate = DateTime.Parse(Request.Form["BirstDate"]);
                //pp.RegisteredDate= DateTime.Parse(Request.Form["RegisteredDate"]);

                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Details", new { id=p.CustomerIndId});
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
            return View("Table", db.CustomerInd.AsNoTracking().Where(cl => cl.Adress.Contains("могил")));
        }
        //-----------------------------------------------------------------------------

        //autocomplete function for search field----------------------------------------
        //public ActionResult AutocompleteSearch(string term)
        //{
        //    return Json(db.CustomerEnt
        //        .AsNoTracking()
        //        .Where(cl => cl.Название_организации.Contains(term))
        //        .Select(c => new { value = c.Название_организации })
        //        , JsonRequestBehavior.AllowGet);
        //}
        //----------------------------------------------------------------------------------

        //public ActionResult Search(string search)
        //{
        //    var query = db.CustomerEnt
        //        .AsNoTracking()
        //        .Where(c => c.Название_организации.Contains(search) || c.УНП_Клиента.ToString().Contains(search));


        //    if (query.Count() > 0)
        //    {
        //        return PartialView(query);
        //    }

        //    else return PartialView("NoResult");
        //}

    }
}