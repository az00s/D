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
using D.Models.DataTableModel;

namespace D.Controllers
{
    
     //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [Authorize(Roles = "accountant,manager,admin")]
    public class EmployeeController : Controller
    {
        private IdbInterface db;//dataContext
  
        
        public EmployeeController(IdbInterface dbParam)//dependency injection via constructor
        {

            db = dbParam;
           
            

        }

        //----------------------------------------------------------------------------------------------------------

        public ActionResult Index()
        {

            return View("Table");

        }
        public JsonResult Table(dtParam param)
        {

            dtEmployee res = new dtEmployee();
            res.data = res.GetData(param, db.Employees.AsNoTracking());
            res.draw = param.Draw;
            res.recordsTotal = db.Employees.AsNoTracking().Count();
            res.recordsFiltered = res.Count(param, db.Employees);
            return Json(res);
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var emp = db.Employees.Find(id);
            if (emp == null)
            {
                return HttpNotFound();
            }
            return View(emp);
        }

        
        

        
        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed(Employee emp)
        {
            if (ModelState.IsValid)
            {
                db.Employees.Add(emp);
               db.SaveChanges();
               return RedirectToAction("Details",new { id= emp.PersonnelNumber});
            }

            return RedirectToAction("Table");
        }

     

        
        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Employee emp)
        {
            if (ModelState.IsValid)
            {

                
                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details",new { id= emp.PersonnelNumber});
            }
            return View(emp);
        }

       
        
        [Authorize(Roles = "admin")]
        
        [HandleError(ExceptionType = typeof(System.Data.Entity.Infrastructure.DbUpdateException), View = "ErrorEmployee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
           
                db.Employees.Remove(db.Employees.Find(id));
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
        public ActionResult Report()
        {
            return View();
        }
        public JsonResult ReportData(dtParam param)
        {

            dtEmployee res = new dtEmployee();
            var query = db.Employees.AsNoTracking().Where(emp => (DateTime.Now.Year - emp.BirstDate.Value.Year) * 8755.2 > 525312);
            res.data = res.GetData(param, query);
            res.draw = param.Draw;
            res.recordsTotal = db.Employees.AsNoTracking().Count();
            res.recordsFiltered = res.Count(param, query);
            return Json(res);
        }



        public JsonResult GetDataForChart()
        {
            var result=db.Employees
                .Where(e=>e.Position== "Продавец")
                .GroupJoin(
                    db.Orders,emp=>emp.PersonnelNumber,ord=>ord.PersonnelNumber,(emp,ord)=> new PieChartItem {
                    Name = emp.LastName, Value = db.Orders.Where(s=>s.PersonnelNumber==emp.PersonnelNumber).Count() }
                );
            

            return Json(new { Employees = result }, JsonRequestBehavior.AllowGet);
        }
    }
    public class PieChartItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

}

