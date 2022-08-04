using HumanCapitalManagement.Controllers.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.StrengthFinderControllers
{
    public class StrengthFinderController : UploadFileController
    {
        // GET: StrengthFinder
        public ActionResult Index()
        {
            return View();
        }

        
    }
}