
using D.Infrastructure;
using D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace D.Controllers
{
    
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Disabled)]
    public class HomeController : Controller
    {
        private IdbInterface db;
        
        public HomeController(IdbInterface dbParam)
        {
            db = dbParam;
           
        }
        
        private void Sending()
        {
            try
            {
                WebMail.SmtpServer = "smtp.yandex.ru";
                WebMail.SmtpPort = 587;
                WebMail.EnableSsl = true;
                WebMail.UserName = "azoos@tut.by";
                WebMail.Password = "azoosgoogle11";
                WebMail.From = "azoos@tut.by";


                string os = HttpContext.Request.Browser.Platform;
                string browser = HttpContext.Request.Browser.Type;
                string useragent = HttpContext.Request.UserAgent;
                string ip = HttpContext.Request.UserHostAddress;
                string userhostname = HttpContext.Request.UserHostName;
                WebMail.Send("azoos@tut.by", "Посещение сайта",
                             "Браузер: " + browser + "," + "useragent: " + useragent + "," + "ip: " + ip + "," + "userhostname: " + userhostname);

            }
            catch (Exception)
            {
                

            }
        }
        [Authorize]
        public  ActionResult Index()
        {
            ViewBag.t = db.Товар.Count();
            ViewBag.c = db.Клиент.Count();
            ViewBag.p = db.Поставщик.Count();
            ViewBag.e = db.Сотрудник.Count();
            ViewBag.o = db.Заказ.Count();
            ViewBag.amount = 0;
            ViewBag.w = 0;
            foreach (var t in db.Товар)
            {
                if (t.Цена != null)
                { ViewBag.amount += t.Остаток_на_складе * t.Цена; }
                if (t.Вес != null)
                { ViewBag.w += t.Остаток_на_складе * t.Вес; }
            }
            ViewBag.w = ViewBag.w / 1000;

            var queryY = from o in db.Заказ
                        where o.Дата_заказа.Value.Year > DateTime.Now.Year - 2 && o.Дата_заказа.Value.Year < DateTime.Now.Year
                         select o.Сумма_заказа_с_НДС;
            ViewBag.o1mY = queryY.Sum();
            ViewBag.o1Y = queryY.Count();

            var queryYN = from o in db.Заказ
                         where o.Дата_заказа.Value.Year== DateTime.Now.Year
                         select o.Сумма_заказа_с_НДС;
            ViewBag.o1mYN = queryYN.Sum();
            ViewBag.o1YN = queryYN.Count();

            var query = from o in db.Заказ
                          where o.Дата_заказа.Value.Month > DateTime.Now.Month - 2 && o.Дата_заказа.Value.Month < DateTime.Now.Month
                          select o.Сумма_заказа_с_НДС;
            ViewBag.o1m = query.Sum();
            ViewBag.o1 = query.Count();

            var query1 = from o in db.Заказ
                        where o.Дата_заказа.Value.Month == DateTime.Now.Month
                        select o.Сумма_заказа_с_НДС;
            ViewBag.o2m = query1.Sum();
            ViewBag.o2 = query1.Count();

            var query2 = from o in db.Денежное_поступление
                        where o.Дата_поступления.Value.Month > DateTime.Now.Month - 2 && o.Дата_поступления.Value.Month < DateTime.Now.Month
                        select o.Сумма;
            ViewBag.o1m1 = query2.Sum();
            ViewBag.o11 = query2.Count();

            var query3 = from o in db.Денежное_поступление
                         where o.Дата_поступления.Value.Month == DateTime.Now.Month
                         select o.Сумма;
            ViewBag.o2m1 = query3.Sum();
            ViewBag.o21 = query3.Count();

            var query4 = from o in db.Денежное_поступление
                         where o.Дата_поступления.Value.Year > DateTime.Now.Year - 2 && o.Дата_поступления.Value.Year < DateTime.Now.Year
                         select o.Сумма;
            ViewBag.o1m1Y = query4.Sum();
            ViewBag.o11Y = query4.Count();

            var query5 = from o in db.Денежное_поступление
                         where o.Дата_поступления.Value.Year == DateTime.Now.Year
                         select o.Сумма;
            ViewBag.o2m1Y = query5.Sum();
            ViewBag.o21Y = query5.Count();

           
            //Sending();
               
            return View();
            
        }
        
    }
}