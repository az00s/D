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

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed([Bind(Include = "CustomerIndId,Description,BirstDate,Email,Telephone,Adress,LastName,FirstName,Patronymic,PassportId")] CustomerInd customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    customer.AddtoTable(db, customer);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = customer.CustomerIndId });
                }
                return View("Error");
            }
            catch (Exception)
            { return View("Error"); }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerIndId,PassportId,Email,Telephone,Adress,BirstDate,Description,FirstName,LastName,Patronymic,RegisteredDate")] CustomerInd customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(customer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = customer.CustomerIndId });
                }
                return View("Error");
            }
            catch (Exception)
            { return View("Error"); }

        }

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

    }
}