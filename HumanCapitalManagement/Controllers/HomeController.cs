using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        
        {
            vHome result = new vHome();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;

            }
            result.is_pes = "N";
            if (CGlobal.UserInfo.RankID == "12" || CGlobal.UserInfo.RankID == "20" || CGlobal.UserIsAdminPES())
            {
                result.is_pes = "Y";
            }
            return View(result);

        }
        public ActionResult IndexTest()
        {
            vHome result = new vHome();
            result.is_pes = "S";
            return View(result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}