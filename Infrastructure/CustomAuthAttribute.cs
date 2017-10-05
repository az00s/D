using Microsoft.ApplicationInsights.Extensibility.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using D.Models;
using System.Web.Mvc.Filters;
using System.Web.Routing;

namespace D.Infrastructure
{
    
    public class CustomAuthAttribute:FilterAttribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)

        {
           
            if (HttpContext.Current.Session["sessionid"] ==null)
                HttpContext.Current.Session["sessionid"] = "empty";

            // check to see if your ID in the Logins table has LoggedIn = true - if so, continue, otherwise, redirect to Login page.
            if (IsYourLoginStillTrue(System.Web.HttpContext.Current.User.Identity.Name, HttpContext.Current.Session["sessionid"].ToString()))
            {
                // check to see if your user ID is being used elsewhere under a different session ID
                if (!IsUserLoggedOnElsewhere(System.Web.HttpContext.Current.User.Identity.Name, HttpContext.Current.Session["sessionid"].ToString()))
                {
                //    context.Result= new HttpUnauthorizedResult() RedirectToRouteResult(new RouteValueDictionary {
                //    {"controller", "GoogleAccount"},
                //    {"action",  "Login"},
                //    {"returnUrl", context.HttpContext.Request.RawUrl}
                //}); ;
                
                }
                else
                {
                    // if it is being used elsewhere, update all their Logins records to LoggedIn = false, except for your session ID
                    LogEveryoneElseOut(System.Web.HttpContext.Current.User.Identity.Name, HttpContext.Current.Session["sessionid"].ToString());
                    //return View();
                }
            }
            else
            {
                FormsAuthentication.SignOut();
                context.Result=new RedirectToRouteResult(new RouteValueDictionary {
                    {"controller", "Account"},
                    {"action",  "Login"},
                    //{"returnUrl", context.HttpContext.Request.RawUrl}
                }); 
            }
        }

        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // не реализован
        }

        public static bool IsYourLoginStillTrue(string userId, string sid)
        {
            db context=new db();

            IEnumerable<AspNetUser> logins = (from i in context.AspNetUsers
                                          where i.LoggedIn == true && i.Email == userId && i.SessionId == sid
                                          select i).AsEnumerable();
            return logins.Any();
        }

        public static bool IsUserLoggedOnElsewhere(string userId, string sid)
        {
            db context = new db();

            IEnumerable<AspNetUser> logins = (from i in context.AspNetUsers
                                              where i.LoggedIn == true && i.Email == userId && i.SessionId != sid
                                          select i).AsEnumerable();
            return logins.Any();
        }

        public static void LogEveryoneElseOut(string userId, string sid)
        {
            db context = new db();

            IEnumerable<AspNetUser> logins = (from i in context.AspNetUsers
                                              where i.LoggedIn == true && i.Email == userId && i.SessionId != sid // need to filter by user ID
                                          select i).AsEnumerable();

            foreach (AspNetUser item in logins)
            {
                item.LoggedIn = false;
            }

            context.SaveChanges();
        }
    }
}