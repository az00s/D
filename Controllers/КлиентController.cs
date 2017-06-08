using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using D.Interfaces;
using System;

namespace D.Models
{
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    [Authorize(Roles = "seller,manager,admin")]
    public class КлиентController : Controller
    {
        private IdbInterface db;//dataContext
        private IКлиентInterface p;

        public КлиентController(IdbInterface dbParam, IКлиентInterface pParam)//dependency injection via constructor
        {

            db = dbParam;
            p = pParam;


        }
        public ActionResult Index()
        {
            return RedirectToAction("Table");
                               
         }
        public ActionResult Table()
        {
            return View("Table",db.CustomerEnt.AsNoTracking().OrderBy(o=>o.ID_клиента));
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var клиент = db.CustomerEnt.Find(id);
            if (клиент == null)
            {
                return HttpNotFound();
            }
            return View(клиент);
        }



        [HttpPost,ActionName("Create")]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConfirmed([Bind(Include = "ID_клиента,УНП_Клиента,Название_организации,Телефон,Адрес")] CustomerEnt p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    p.AddtoTable(db, p);
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = p.ID_клиента });
                }
                return View("Error");
            }
            catch (Exception)
            { return View("Error"); }

           
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID_клиента,УНП_Клиента,Название_организации,Телефон,Адрес")] CustomerEnt p)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(p).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details", new { id = p.ID_клиента });
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
            db.CustomerEnt.Remove(db.CustomerEnt.Find(id));
            db.SaveChanges();
            return RedirectToAction("Table");
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
            return View("Table", db.CustomerEnt.AsNoTracking().Where(cl=>cl.Адрес.Contains("могил")));
        }
        //-----------------------------------------------------------------------------
        
    }
}
