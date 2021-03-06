﻿using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using D.Models;
using System;
using D.Infrastructure;

namespace D.Controllers
{
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [CustomAuthorize(Roles = "manager,admin,sklad")]
    public class SupplierController : Controller
    {
        private IdbInterface db;//dataContext

        public SupplierController(IdbInterface dbParam)//dependency injection via constructor
        {
            db = dbParam;
        }
        public ActionResult Index()
        {

            return View("Table");

        }
        public JsonResult Table(dtParam param)
        {
            var query = db.Suppliers.AsNoTracking();
            dtResult<IQueryable<Supplier>,Supplier> res = new dtResult<IQueryable<Supplier>, Supplier>();
            res.GetData(param, query,query);
            return Json(res);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }

            ViewBag.List = db.SupplierPrices.AsNoTracking().Where(pro => pro.SupplierPAN == id);

            return View(supplier);
        }

        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed(Supplier supplier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Suppliers.Add(supplier);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = supplier.SupplierPAN });
                }
                return View("Error");
            }
            catch (Exception)
            { return View("Error"); }

           
        }

        
        [CustomAuthorize(Roles = "admin")]
        
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Supplier supplier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(supplier).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = supplier.SupplierPAN });
                }
                return View("Error");
            }
            catch (Exception)
            { return View("Error"); }
        }
        
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorProviders")]
        [CustomAuthorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            db.Suppliers.Remove(db.Suppliers.Find(id));
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

        //a report about providers from Minsk
        public ActionResult Report()
        {
            return View();
        }
        public JsonResult ReportData(dtParam param)
        {
            var query = db.Suppliers.AsNoTracking().Where(s=>s.Address.Contains("минск"));
            dtResult<IQueryable<Supplier>, Supplier> res = new dtResult<IQueryable<Supplier>, Supplier>();
            res.GetData(param,db.Suppliers, query);
            return Json(res);
        }
    }
}
