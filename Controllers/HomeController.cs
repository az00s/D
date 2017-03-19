
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
            ViewBag.t = db.Товар.AsNoTracking().Count();
            ViewBag.c = db.CustomerEnt.AsNoTracking().Count()+ db.CustomerInd.AsNoTracking().Count();
            ViewBag.p = db.Поставщик.AsNoTracking().Count();
            ViewBag.e = db.Сотрудник.AsNoTracking().Count();
            ViewBag.o = db.Заказ.AsNoTracking().Count();
            

            ViewBag.amount = db.Товар
                .AsNoTracking()
                .Sum(g=>g.Остаток_на_складе*g.Цена);

            ViewBag.w= db.Товар
                .AsNoTracking()
                .Sum(g => g.Остаток_на_складе * g.Вес)/1000;


            ViewBag.o1mY = db.Заказ
                .AsNoTracking()
                .Where(o => o.Дата_заказа.Value.Year == DateTime.Now.Year - 1)
                .Select(o => o.Сумма_заказа_с_НДС).Sum();

            ViewBag.o1Y= db.Заказ
                .AsNoTracking()
                .Where(o => o.Дата_заказа.Value.Year == DateTime.Now.Year - 1)
                .Count();

            ViewBag.o1mYN = db.Заказ
                .AsNoTracking()
                .Where(o => o.Дата_заказа.Value.Year == DateTime.Now.Year )
                .Select(o => o.Сумма_заказа_с_НДС).Sum();

            ViewBag.o1YN = db.Заказ
                .AsNoTracking()
                .Where(o => o.Дата_заказа.Value.Year == DateTime.Now.Year )
                .Count();


            ViewBag.o1m= db.Заказ
                .AsNoTracking()
                .Where(o => o.Дата_заказа.Value.Month == DateTime.Now.Month-1)
                .Select(o => o.Сумма_заказа_с_НДС).Sum();

            ViewBag.o1= db.Заказ
                .AsNoTracking()
                .Where(o => o.Дата_заказа.Value.Month == DateTime.Now.Month - 1)
                .Count();

            ViewBag.o2m= db.Заказ
                .AsNoTracking()
                .Where(o => o.Дата_заказа.Value.Month == DateTime.Now.Month )
                .Select(o => o.Сумма_заказа_с_НДС).Sum();
           
            ViewBag.o2 = db.Заказ
                .AsNoTracking()
                .Where(o => o.Дата_заказа.Value.Month == DateTime.Now.Month )
                .Count();

            ViewBag.o1m1= db.Денежное_поступление
                .AsNoTracking()
                .Where(o => o.Дата_поступления.Value.Month == DateTime.Now.Month-1)
                .Select(o => o.Сумма).Sum();

            
            ViewBag.o11 = db.Денежное_поступление
                .AsNoTracking()
                .Where(o => o.Дата_поступления.Value.Month == DateTime.Now.Month - 1)
                .Count();


            ViewBag.o2m1= db.Денежное_поступление
                .AsNoTracking()
                .Where(o => o.Дата_поступления.Value.Month == DateTime.Now.Month )
                .Select(o => o.Сумма).Sum();

            ViewBag.o21= db.Денежное_поступление
                .AsNoTracking()
                .Where(o => o.Дата_поступления.Value.Month == DateTime.Now.Month)
                .Count();


            ViewBag.o1m1Y= db.Денежное_поступление
                .AsNoTracking()
                .Where(o => o.Дата_поступления.Value.Year == DateTime.Now.Year-1)
                .Select(o => o.Сумма).Sum();
            
            ViewBag.o11Y = db.Денежное_поступление
                .AsNoTracking()
                .Where(o => o.Дата_поступления.Value.Year == DateTime.Now.Year - 1)
                .Count();

            ViewBag.o2m1Y = db.Денежное_поступление
                .AsNoTracking()
                .Where(o => o.Дата_поступления.Value.Year == DateTime.Now.Year)
                .Select(o => o.Сумма).Sum();

            ViewBag.o21Y = db.Денежное_поступление
                .AsNoTracking()
                .Where(o => o.Дата_поступления.Value.Year == DateTime.Now.Year )
                .Count();

            return View();
            
        }
        
    }
}