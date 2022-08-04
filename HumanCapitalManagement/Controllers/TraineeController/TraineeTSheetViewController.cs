using HumanCapitalManagement.Controllers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.TraineeController
{
    public class TraineeTSheetViewController : BaseController
    {
        // GET: TraineeTSheetView
        public ActionResult TraineeTSheetView()
        {
            return View();
        }
    }
}