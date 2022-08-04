using HumanCapitalManagement.Service.TIFForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TraineeManagement.Controllers.CommonControllers;

namespace TraineeManagement.Controllers
{
    public class HomeController : BaseController
    {
        private TM_TIF_FormService _TM_TIF_FormService;
        public HomeController(TM_TIF_FormService TM_TIF_FormService)
        {
            _TM_TIF_FormService = TM_TIF_FormService;
        }
        public ActionResult Index()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            return View();
        }

        public ActionResult About()
        {
            return RedirectToAction("Error404", "Login");
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            return RedirectToAction("Error404", "Login");
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}