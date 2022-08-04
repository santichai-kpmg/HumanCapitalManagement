using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Report.DataSet;
using HumanCapitalManagement.Report.DevReport.Candidate;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Report
{
    public class rTraineeAnnouncementController : BaseController
    {
        private DivisionService _DivisionService;

        private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();
        private New_HRISEntities dbHr = new New_HRISEntities();
        public rTraineeAnnouncementController(DivisionService DivisionService)
        {
            _DivisionService = DivisionService;
        }
        // GET: rHeadcount
        public ActionResult Index()
        {
            return View();
        }
    }


}
