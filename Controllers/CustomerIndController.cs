using D.Models;
using D.Models.DataTableModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace D.Controllers
{
    public class CustomerIndController : Controller
    {
        private IdbInterface db;//dataContext
        

        public CustomerIndController(IdbInterface dbParam)//dependency injection via constructor
        {
            db = dbParam;
         
        }
        // GET: CustomerInd
        public ActionResult Index()
        {
            return View("Table");

        }
        public JsonResult Table(dtParam param)
        {

            dtCustomerInd res = new dtCustomerInd();
            res.data = res.GetData(param, db.CustomerInds.AsNoTracking());
            res.draw = param.Draw;
            res.recordsTotal = db.CustomerInds.AsNoTracking().Count();
            res.recordsFiltered = res.Count(param, db.CustomerInds.AsNoTracking());
            return Json(res);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var customer = db.CustomerInds.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        [HttpPost, ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed( CustomerInd customer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.CustomerInds.Add(customer);
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
        public ActionResult Edit( CustomerInd customer)
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
            db.CustomerInds.Remove(db.CustomerInds.Find(id));
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

        public ActionResult ReportMogilev()
        {
            return View("ReportMogilev");
        }


        public JsonResult CMogilev(dtParam param)
        {
            var query = db.CustomerInds.AsNoTracking().Where(cus => cus.Address.Contains("могил"));
            dtCustomerInd res = new dtCustomerInd();
            res.data = res.GetData(param, query);
            res.draw = param.Draw;
            res.recordsTotal = db.CustomerInds.AsNoTracking().Count();
            res.recordsFiltered = res.Count(param, query);
            return Json(res);
        }



    }
}