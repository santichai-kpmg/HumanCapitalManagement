using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TraineeManagement.Controllers.CommonControllers
{
    public class UserGuideController : BaseController
    {
        // GET: UserGuide
        public ActionResult UserGuide()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            return View();
        }
    }
}