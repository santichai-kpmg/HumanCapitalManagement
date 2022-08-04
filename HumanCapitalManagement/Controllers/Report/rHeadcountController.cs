using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Report.DataSet;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.Report
{
    public class rHeadcountController : BaseController
    {
        private DivisionService _DivisionService;
        private TM_FY_PlanService _TM_FY_PlanService;
        private TM_FY_DetailService _TM_FY_DetailService;
        private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();
        private New_HRISEntities dbHr = new New_HRISEntities();
        public rHeadcountController(DivisionService DivisionService, TM_FY_PlanService TM_FY_PlanService, TM_FY_DetailService TM_FY_DetailService)
        {
            _DivisionService = DivisionService;
            _TM_FY_PlanService = TM_FY_PlanService;
            _TM_FY_DetailService = TM_FY_DetailService;
        }
        // GET: rHeadcount
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult HCPlanAndSummary()
        {
            StoreDb db = new StoreDb();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            int fy;
            DateTime? dFyYear = null;
            var dNow = DateTime.Now;
            if (dNow.Month >= 10)
            {
                fy = dNow.Year + 1;
                dFyYear = dNow.AddYears(1);
            }
            else
            {
                fy = dNow.Year;
                dFyYear = dNow;
            }

            vHCPlanAndSummaryReport_Result result = new vHCPlanAndSummaryReport_Result();
            result.full_last_year = (fy - 1) + "";
            result.fysh_year = dFyYear.Value.ToString("yy");
            result.fy_year = _TM_FY_PlanService.GetYear(fy) + "";
            List<vHCPlanAndSummaryReportData> lstData = new List<vHCPlanAndSummaryReportData>();
            List<vHCPlanAndSummaryReportData> newlstDate = new List<vHCPlanAndSummaryReportData>();
            var _getNewUnitGroup = _DivisionService.GetDivisionForReport();
            var _GetGroupInPermission = CGlobal.GetDivision();

            //if (_GetGroupInPermission.Any())
            //{
            //    string[] aUni = _GetGroupInPermission.Where(w => w.sCompany_code == "4100").Select(s => s.sID).ToArray();
            //    _getNewUnitGroup = _getNewUnitGroup.Where(w => aUni.Contains(w.division_code)).ToList();
            //}
            //else
            //{
            //    _getNewUnitGroup = _getNewUnitGroup.Take(0);
            //}
            //if (_getNewUnitGroup.Any())
            //{
            //    newlstDate = _getNewUnitGroup.Select(s => new vHCPlanAndSummaryReportData
            //    {
            //        division = s.TM_Pool.Pool_name_en,
            //        sgroup = s.division_name_en,


            //    }).ToList();
            //}
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
         
            var _newPlan = _TM_FY_DetailService.GetDataNow(fy);
            List<vHCFYPlan> lstFPlan = new List<vHCFYPlan>();
            var _getdivision = _getNewUnitGroup.Where(w => aDivisionPermission.Contains(w.division_code)).ToList();
            string[] aAlldivision = _getNewUnitGroup.Select(s => s.division_code).ToArray();
            if (_newPlan != null && _newPlan.Any())
            {
                lstFPlan = _newPlan.Select(s => new vHCFYPlan
                {
                    LOB = s.TM_Divisions.division_name_en,
                    Pool = s.TM_Divisions.TM_Pool.Pool_name_en,
                    unit_code = s.TM_Divisions.division_code,
                    nSum = ((s.para.HasValue ? s.para.Value : 0) +
                (s.aa.HasValue ? s.aa.Value : 0) +
                 (s.sr.HasValue ? s.sr.Value : 0) +
                  (s.am.HasValue ? s.am.Value : 0) +
                 (s.mgr.HasValue ? s.mgr.Value : 0) +
                 (s.ad.HasValue ? s.ad.Value : 0) +
                  (s.dir.HasValue ? s.dir.Value : 0) +
                 (s.ptr.HasValue ? s.ptr.Value : 0)),

                }).ToList();


            }

            var _Udivision = CGlobal.GetDivision();
            var _getprevious = db.PreviousFYs.Where(w => w.nYear == fy && w.Company +"" != "KPMG Lao" && w.Company + "" != "KPMG MM").ToList();

            lstFPlan.Where(w => w.Pool == "Tax").ToList().ForEach(ed =>
            {
                ed.Pool = "Tax & Legal";
            });
            _getprevious.Where(w => w.Pool == "Tax").ToList().ForEach(ed =>
            {
                ed.Pool = "Tax & Legal";
            });
            lstFPlan.Where(w => w.Pool == "Shared Services").ToList().ForEach(ed =>
            {
                ed.Pool = "Practice Support & Shared Services";
            });
            _getprevious.Where(w => w.Pool == "Shared Services").ToList().ForEach(ed =>
            {
                ed.Pool = "Practice Support & Shared Services";
            });
            var empList = dbHr.AllInfo_WS.Where(w => (w.Status == 1 || w.Status == 3) && w.CompanyCode != "KPMG Lao" && w.CompanyCode != "KPMG MM").ToList();
            //   DataTable empList = wsHRIS.getEmployeeInfo("k", "", "3,1", "", "", "", "");
            DateTime JoinDate = SystemFunction.ConvertStringToDateTime("01/10/" + (fy - 1), "", "dd/MM/yyyy");
            var empListNew = empList.Where(w => w.JoinDate >= JoinDate).ToList(); ;
            List<vClassEmployeeFromWS> lstEmplist = empListNew.Where(w => aAlldivision.Contains(w.UnitGroupID + ""))
             .Select(e => new vClassEmployeeFromWS
             {
                 unitgroup = e.UnitGroup + "",
                 emp_no = e.EmpNo + "",//e["EmpNo"].ToString(),
                 unit_name = e.Pool + "" != "Tax" ? (e.Pool + "" == "Shared Services" ? "Practice Support & Shared Services" : e.Pool + "") : "Tax & Legal",

             }).ToList();
            List<vClassEmployeeFromWS> lseEmpCur = (from row in empList.AsEnumerable().Where(w => aAlldivision.Contains(w.UnitGroupID + ""))
                                                    group row by new { ID = row.EmpNo, UnitGroup = row.UnitGroup, Pool = row.Pool, row.UnitGroupID } into grp
                                                    select new vClassEmployeeFromWS
                                                    {
                                                        emp_no = grp.Key.ID,
                                                        unitgroup = grp.Key.UnitGroup,
                                                        unitgroup_code = grp.Key.UnitGroupID + "",
                                                        // unit_name = grp.Key.Pool + "" != "Tax" ? grp.Key.Pool + "" : "Tax & Legal",
                                                        unit_name = grp.Key.Pool + "" != "Tax" ? (grp.Key.Pool + "" == "Shared Services" ? "Practice Support & Shared Services" : grp.Key.Pool + "") : "Tax & Legal",
                                                    }).ToList();
            var empListOut = dbHr.AllInfo_WS.Where(w => (w.Status == 0) && w.CompanyCode != "KPMG Lao" && w.CompanyCode != "KPMG MM").ToList();
            // DataTable empListOut = wsHRIS.getEmployeeInfo("k", "", "0", "", "", "", "");

            List<string> preFY = _getprevious.Select(x => x.EmployeeNo).ToList();
            List<vClassEmployeeFromWS> lseEmpRes = (from row in empListOut.AsEnumerable().Where(x => preFY.Contains(x.EmpNo) && aAlldivision.Contains(x.UnitGroupID + ""))
                                                    group row by new { ID = row.EmpNo, UnitGroup = row.UnitGroup, Pool = row.Pool, row.UnitGroupID } into grp
                                                    select new vClassEmployeeFromWS
                                                    {
                                                        emp_no = grp.Key.ID,
                                                        unitgroup = grp.Key.UnitGroup,
                                                        unitgroup_code = grp.Key.UnitGroupID + "",
                                                        //    unit_name = grp.Key.Pool + "" != "Tax" ? grp.Key.Pool + "" : "Tax & Legal",
                                                        unit_name = grp.Key.Pool + "" != "Tax" ? (grp.Key.Pool + "" == "Shared Services" ? "Practice Support & Shared Services" : grp.Key.Pool + "") : "Tax & Legal",
                                                    }).ToList();

            lstData = (from lstdivision in _getdivision.OrderBy(o => o.division_name_en)
                           //from lstplan in lstFPlan.Where(w => w.LOB == lstdivision.division_name_en).DefaultIfEmpty(new vHCFYPlan())
                           //from lstprevious in _getprevious.Where(w => w.UnitGroup == lstdivision.division_name_en).DefaultIfEmpty(new Models.OldTable.PreviousFY())
                           //from lstEmp in lstEmplist.Where(w => w.unitgroup == lstdivision.division_name_en).DefaultIfEmpty(new vClassEmployeeFromWS())
                       group new { lstdivision /*, lstplan, lstprevious ,lstEmp*/ } by new
                       {
                           lstdivision.TM_Pool.Pool_name_en,
                           lstdivision.division_name_en,
                           lstdivision.division_code,
                           //lstplan.NewHire,
                           //lstplan.EstimatePreviousFY,

                       } into grp
                       select new vHCPlanAndSummaryReportData
                       {
                           division = grp.Key.Pool_name_en + "",
                           sgroup = grp.Key.division_name_en + "",
                           plan = lstFPlan.Where(w => w.unit_code == grp.Key.division_code).FirstOrDefault() != null ? lstFPlan.Where(w => w.unit_code == grp.Key.division_code).Select(s => s.nSum).FirstOrDefault() : 0,
                           ac_new_hires = lstEmplist.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                           current_hc = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                           ac_resign = lseEmpRes.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                           ac_starting = _getprevious.Where(w => w.group_code == grp.Key.division_code).Count(),
                       }).ToList();

            //var xxfex = lseEmpCur.Where(w => w.unitgroup == "T&L-Support").ToList();
            //DataTable dtxx = SystemFunction.LinqToDataTable(xxfex.OrderBy(o => o.unitgroup));
            result.lstDataTotal = (from lstplan in _getNewUnitGroup//lstFPlan
                                   group new { lstplan } by new
                                   {
                                       lstplan.TM_Pool.Pool_name_en
                                   } into grp
                                   select new vHCPlanAndSummaryReportData_Total
                                   {
                                       division = grp.Key.Pool_name_en + "",
                                       plan = lstFPlan.Where(w => w.Pool == grp.Key.Pool_name_en).Sum(s => s.nSum),
                                       ac_new_hires = lstEmplist.Where(w => w.unit_name == grp.Key.Pool_name_en).Count(),
                                       current_hc = lseEmpCur.Where(w => w.unit_name == grp.Key.Pool_name_en).Count(),
                                       ac_resign = lseEmpRes.Where(w => w.unit_name == grp.Key.Pool_name_en).Count(),
                                       ac_starting = _getprevious.Where(w => w.Pool == grp.Key.Pool_name_en).Count(),
                                   }).ToList();

            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "rpHCPlanAndSummary" + unixTimestamps;
            result.session = sSession;

            Session[sSession] = new List<vHCPlanAndSummaryReportData>();
            if (lstData.Any())
            {
                result.lstData = lstData.ToList();
                Session[sSession] = lstData.ToList();
            }

            return View(result);
        }
        public ActionResult CurrentStaff()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            StoreDb db = new StoreDb();
            vCurrentStaffReport_Result result = new vCurrentStaffReport_Result();
            List<vCurrentStaffReportData> lstData = new List<vCurrentStaffReportData>();
            int fy;
            if (DateTime.Now.Month >= 10)
            {
                fy = DateTime.Now.Year;
            }
            else
            {
                fy = DateTime.Now.Year - 1;
            }
            // var _getPlan = db.FYPlans.ToList();
            var _getNewUnitGroup = _DivisionService.GetDivisionForReport();
            var _GetGroupInPermission = CGlobal.GetDivision();

            if (_GetGroupInPermission.Any())
            {
                string[] aUni = _GetGroupInPermission.Where(w => w.sCompany_code == "4100").Select(s => s.sID).ToArray();
                _getNewUnitGroup = _getNewUnitGroup.Where(w => aUni.Contains(w.division_code)).ToList();
            }
            else
            {
                _getNewUnitGroup = _getNewUnitGroup.Take(0);
            }

            //  var _getdivision = db.Divisions.ToList();
            // var _getprevious = db.PreviousFYs.ToList();
            var _getEmp = dbHr.AllInfo_WS.Where(w => (w.Status == 1 || w.Status == 3) && w.CompanyCode != "KPMG Lao" && w.CompanyCode != "KPMG MM").ToList();
            // DataTable empList = wsHRIS.getEmployeeInfo("k", "", "3,1", "", "", "", "");
            //List<vClassEmployeeFromWS> lseEmpCur = (from row in empList.AsEnumerable()
            //                                        group row by new { ID = row.Field<string>("EmpNo"), UnitGroup = row.Field<string>("UnitGroup"), Rank = row.Field<string>("RankCode") } into grp
            //                                        select new vClassEmployeeFromWS
            //                                        {
            //                                            emp_no = grp.Key.ID,
            //                                            unitgroup = grp.Key.UnitGroup,
            //                                            rank = grp.Key.Rank,
            //                                        }).ToList();
            List<vClassEmployeeFromWS> lseEmpCur = (from row in _getEmp
                                                    group row by new { ID = row.EmpNo, UnitGroup = row.UnitGroup, Rank = row.RankCode, row.UnitGroupID } into grp
                                                    select new vClassEmployeeFromWS
                                                    {
                                                        emp_no = grp.Key.ID,
                                                        unitgroup = grp.Key.UnitGroup,
                                                        rank = grp.Key.Rank,
                                                        unitgroup_code = grp.Key.UnitGroupID + "",
                                                    }).ToList();
            lstData = (from lstdivision in _getNewUnitGroup.OrderBy(o => o.seq)
                       group new { lstdivision } by new
                       {
                           lstdivision.TM_Pool.Pool_name_en,
                           lstdivision.division_name_en,
                           lstdivision.division_code,
                       } into grp
                       select new vCurrentStaffReportData
                       {
                           division = grp.Key.Pool_name_en + "",
                           sgroup = grp.Key.division_name_en + "",
                           para = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "ASS ASSOC" || w.rank == "EA")).Count(),
                           aa = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "ASSOC" || w.rank == "STAFF" || w.rank == "ASS")).Count(),
                           sr = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "SR")).Count(),
                           am = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "AM")).Count(),
                           mgr = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "MGR")).Count(),
                           ad = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "ASSOC D")).Count(),
                           dir = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "EXEC DI")).Count(),
                           ptr = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "PTR" || w.rank == "ADV")).Count(),
                           total = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                       }).ToList();

            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "rpCurrentStaff" + unixTimestamps;
            result.session = sSession;
            Session[sSession] = new List<vHCPlanAndSummaryReportData>();
            if (lstData.Any())
            {
                result.lstData = lstData.ToList();
                Session[sSession] = lstData.ToList();
            }


            return View(result);

        }
        #region Export Pdf and Excel by devexpress
        public ActionResult ExpHCPlanAndSummary(string qryStr, string sMode)
        {
            var sCheck = acCheckLoginAndPermission(true);
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            List<vHCPlanAndSummaryReportData> lstData_resutl = new List<vHCPlanAndSummaryReportData>();

            if (qryStr != "")
            {
                if (Session[qryStr] != null)
                {
                    List<vHCPlanAndSummaryReportData> lstData = Session[qryStr] as List<vHCPlanAndSummaryReportData>;
                    if (lstData.Any())
                    {
                        try
                        {
                            if (lstData.Any())
                            {
                                var stream = new MemoryStream();
                                XrTemp xrTemp = new XrTemp();
                                if (!string.IsNullOrEmpty(sMode) && sMode == "pdf")
                                {

                                    xrTemp.CreateDocument();

                                    DataTable dtR = SystemFunction.LinqToDataTable(lstData);
                                    dsHCPlanAndSummary ds = new dsHCPlanAndSummary();
                                    ds.tbMain.Merge(dtR);
                                    ReportHCPlanAndSummary report = new ReportHCPlanAndSummary();
                                    XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                                    xrLabel1.Text = dNow.DateTimeWithTimebyCulture() + " By " + CGlobal.UserInfo.FullName;
                                    report.DataSource = ds;
                                    report.CreateDocument();
                                    PdfExportOptions options = new PdfExportOptions();
                                    options.ShowPrintDialogOnOpen = true;

                                    //Add page
                                    xrTemp.Pages.AddRange(report.Pages);

                                    xrTemp.ExportToPdf(stream);
                                    return File(stream.GetBuffer(), "application/pdf", "HCPlanAndSummary" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                                }
                                else
                                {
                                    xrTemp.CreateDocument();


                                    DataTable dtR = SystemFunction.LinqToDataTable(lstData);
                                    dsHCPlanAndSummary2 ds = new dsHCPlanAndSummary2();
                                    ds.tbMain.Merge(dtR);
                                    ReportHCPlanAndSummaryXls report = new ReportHCPlanAndSummaryXls();
                                    //XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                                    //xrLabel1.Text = dNow.DateTimeWithTimebyCulture() + " By " + CGlobal.UserInfo.FullName;
                                    report.DataSource = ds;
                                    report.CreateDocument();
                                    //Add page
                                    xrTemp.Pages.AddRange(report.Pages);

                                    XlsxExportOptions xlsxOptions = xrTemp.ExportOptions.Xlsx;
                                    xlsxOptions.ShowGridLines = true;
                                    xlsxOptions.SheetName = "sheet_";
                                    xlsxOptions.TextExportMode = TextExportMode.Text;
                                    xlsxOptions.ExportMode = XlsxExportMode.SingleFilePageByPage;

                                    xrTemp.ExportToXlsx(stream, xlsxOptions);
                                    stream.Seek(0, SeekOrigin.Begin);
                                    byte[] breport = stream.ToArray();
                                    return File(breport, "application/excel", "HCPlanAndSummary" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");
                                }
                            }
                            return View();
                        }
                        catch (Exception e)
                        {
                            return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                            throw;
                        }
                    }
                    else
                    {
                        return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error Data not found." });
                    }
                }
                else
                {
                    return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                }
            }
            else
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
            }
        }
        #endregion
        public class vHCFYPlan
        {
            public int Id { get; set; }
            public string Pool { get; set; }
            public string LOB { get; set; }
            public string FY { get; set; }
            public decimal? nSum { get; set; }
            public string unit_code { get; set; }

        }
        #region Ajax

        [HttpPost]
        public ActionResult HCPlanAndSummaryByFYPlanYear(CSearchHCPlanAndSummary SearchItem)
        {
            StoreDb db = new StoreDb();
            int nid = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
            var _getData = _TM_FY_PlanService.Find(nid);
            DateTime? dFyYear = null;
            var dNow = DateTime.Now;
            int fy = 0;
            if (_getData != null)
            {
                fy = _getData.fy_year.Value.Year;
                dFyYear = _getData.fy_year;
            }
            else
            {
                fy = dNow.Year;
                dFyYear = dNow;
            }

            vHCPlanAndSummaryReport_Result result = new vHCPlanAndSummaryReport_Result();
            result.full_last_year = (fy - 1) + "";
            result.fysh_year = dFyYear.Value.ToString("yy");
            result.fy_year = _TM_FY_PlanService.GetYear(fy) + "";
            List<vHCPlanAndSummaryReportData> lstData = new List<vHCPlanAndSummaryReportData>();
            var _getNewUnitGroup = _DivisionService.GetDivisionForReport();
            var _GetGroupInPermission = CGlobal.GetDivision();

            //if (_GetGroupInPermission.Any())
            //{
            //    string[] aUni = _GetGroupInPermission.Where(w => w.sCompany_code == "4100").Select(s => s.sID).ToArray();
            //    _getNewUnitGroup = _getNewUnitGroup.Where(w => aUni.Contains(w.division_code)).ToList();
            //}
            //else
            //{
            //    _getNewUnitGroup = _getNewUnitGroup.Take(0);
            //}

            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();

            //var _newPlan = _TM_FY_DetailService.GetDataNow(fy);
            List<vHCFYPlan> lstFPlan = new List<vHCFYPlan>();
            if (_getData.TM_FY_Detail != null && _getData.TM_FY_Detail.Any())
            {
                lstFPlan = _getData.TM_FY_Detail.Select(s => new vHCFYPlan
                {
                    LOB = s.TM_Divisions.division_name_en,
                    Pool = s.TM_Divisions.TM_Pool.Pool_name_en,
                    unit_code = s.TM_Divisions.division_code,
                    nSum = ((s.para.HasValue ? s.para.Value : 0) +
                (s.aa.HasValue ? s.aa.Value : 0) +
                 (s.sr.HasValue ? s.sr.Value : 0) +
                  (s.am.HasValue ? s.am.Value : 0) +
                 (s.mgr.HasValue ? s.mgr.Value : 0) +
                 (s.ad.HasValue ? s.ad.Value : 0) +
                  (s.dir.HasValue ? s.dir.Value : 0) +
                 (s.ptr.HasValue ? s.ptr.Value : 0)),

                }).ToList();


            }

            var _getdivision = _getNewUnitGroup.Where(w => aDivisionPermission.Contains(w.division_code)).ToList();
            string[] aAlldivision = _getNewUnitGroup.Select(s => s.division_code).ToArray();
            var _Udivision = CGlobal.GetDivision();
            var _getprevious = db.PreviousFYs.Where(w => w.nYear == fy && w.Company + "" != "KPMG Lao" && w.Company + "" != "KPMG MM").ToList();
            lstFPlan.Where(w => w.Pool == "Tax").ToList().ForEach(ed =>
            {
                ed.Pool = "Tax & Legal";
            });
            _getprevious.Where(w => w.Pool == "Tax").ToList().ForEach(ed =>
            {
                ed.Pool = "Tax & Legal";
            });
            lstFPlan.Where(w => w.Pool == "Shared Services").ToList().ForEach(ed =>
            {
                ed.Pool = "Practice Support & Shared Services";
            });
            _getprevious.Where(w => w.Pool == "Shared Services").ToList().ForEach(ed =>
            {
                ed.Pool = "Practice Support & Shared Services";
            });
            var empList = dbHr.AllInfo_WS.Where(w => (w.Status == 1 || w.Status == 3) && w.CompanyCode != "KPMG Lao" && w.CompanyCode != "KPMG MM").ToList();
            //   DataTable empList = wsHRIS.getEmployeeInfo("k", "", "3,1", "", "", "", "");
            DateTime JoinDate = SystemFunction.ConvertStringToDateTime("01/10/" + (fy - 1), "", "dd/MM/yyyy");
            var empListNew = empList.Where(w => w.JoinDate >= JoinDate).ToList(); ;
            List<vClassEmployeeFromWS> lstEmplist = empListNew.Where(w => aAlldivision.Contains(w.UnitGroupID + ""))
             .Select(e => new vClassEmployeeFromWS
             {
                 unitgroup = e.UnitGroup + "",
                 emp_no = e.EmpNo + "",//e["EmpNo"].ToString(),
                 unit_name = e.Pool + "" != "Tax" ? (e.Pool + "" == "Shared Services" ? "Practice Support & Shared Services" : e.Pool + "") : "Tax & Legal",

             }).ToList();
            List<vClassEmployeeFromWS> lseEmpCur = (from row in empList.AsEnumerable().Where(w => aAlldivision.Contains(w.UnitGroupID + ""))
                                                    group row by new { ID = row.EmpNo, UnitGroup = row.UnitGroup, Pool = row.Pool, row.UnitGroupID } into grp
                                                    select new vClassEmployeeFromWS
                                                    {
                                                        emp_no = grp.Key.ID,
                                                        unitgroup = grp.Key.UnitGroup,
                                                        unitgroup_code = grp.Key.UnitGroupID + "",
                                                        // unit_name = grp.Key.Pool + "" != "Tax" ? grp.Key.Pool + "" : "Tax & Legal",
                                                        unit_name = grp.Key.Pool + "" != "Tax" ? (grp.Key.Pool + "" == "Shared Services" ? "Practice Support & Shared Services" : grp.Key.Pool + "") : "Tax & Legal",
                                                    }).ToList();
            var empListOut = dbHr.AllInfo_WS.Where(w => (w.Status == 0) && w.CompanyCode != "KPMG Lao" && w.CompanyCode != "KPMG MM").ToList();
            // DataTable empListOut = wsHRIS.getEmployeeInfo("k", "", "0", "", "", "", "");

            List<string> preFY = _getprevious.Select(x => x.EmployeeNo).ToList();
            List<vClassEmployeeFromWS> lseEmpRes = (from row in empListOut.AsEnumerable().Where(x => preFY.Contains(x.EmpNo) && aAlldivision.Contains(x.UnitGroupID + ""))
                                                    group row by new { ID = row.EmpNo, UnitGroup = row.UnitGroup, Pool = row.Pool, row.UnitGroupID } into grp
                                                    select new vClassEmployeeFromWS
                                                    {
                                                        emp_no = grp.Key.ID,
                                                        unitgroup = grp.Key.UnitGroup,
                                                        unitgroup_code = grp.Key.UnitGroupID + "",
                                                        //    unit_name = grp.Key.Pool + "" != "Tax" ? grp.Key.Pool + "" : "Tax & Legal",
                                                        unit_name = grp.Key.Pool + "" != "Tax" ? (grp.Key.Pool + "" == "Shared Services" ? "Practice Support & Shared Services" : grp.Key.Pool + "") : "Tax & Legal",
                                                    }).ToList();

            lstData = (from lstdivision in _getdivision.OrderBy(o => o.division_name_en)
                           //from lstplan in lstFPlan.Where(w => w.LOB == lstdivision.division_name_en).DefaultIfEmpty(new vHCFYPlan())
                           //from lstprevious in _getprevious.Where(w => w.UnitGroup == lstdivision.division_name_en).DefaultIfEmpty(new Models.OldTable.PreviousFY())
                           //from lstEmp in lstEmplist.Where(w => w.unitgroup == lstdivision.division_name_en).DefaultIfEmpty(new vClassEmployeeFromWS())
                       group new { lstdivision/*, lstplan, lstprevious lstEmp*/ } by new
                       {
                           lstdivision.TM_Pool.Pool_name_en,
                           lstdivision.division_name_en,
                           lstdivision.division_code,
                           //lstplan.NewHire,
                           //lstplan.EstimatePreviousFY,

                       } into grp
                       select new vHCPlanAndSummaryReportData
                       {
                           division = grp.Key.Pool_name_en + "",
                           sgroup = grp.Key.division_name_en + "",
                           plan = lstFPlan.Where(w => w.unit_code == grp.Key.division_code).FirstOrDefault() != null ? lstFPlan.Where(w => w.unit_code == grp.Key.division_code).Select(s => s.nSum).FirstOrDefault() : 0,
                           ac_new_hires = lstEmplist.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                           current_hc = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                           ac_resign = lseEmpRes.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                           ac_starting = _getprevious.Where(w => w.group_code == grp.Key.division_code).Count(),
                       }).ToList();

            //var xxfex = lseEmpCur.Where(w => w.unitgroup == "T&L-Support").ToList();
            //DataTable dtxx = SystemFunction.LinqToDataTable(xxfex.OrderBy(o => o.unitgroup));
            result.lstDataTotal = (from lstplan in _getNewUnitGroup//lstFPlan
                                   group new { lstplan } by new
                                   {
                                       lstplan.TM_Pool.Pool_name_en
                                   } into grp
                                   select new vHCPlanAndSummaryReportData_Total
                                   {
                                       division = grp.Key.Pool_name_en + "",
                                       plan = lstFPlan.Where(w => w.Pool == grp.Key.Pool_name_en).Sum(s => s.nSum),
                                       ac_new_hires = lstEmplist.Where(w => w.unit_name == grp.Key.Pool_name_en).Count(),
                                       current_hc = lseEmpCur.Where(w => w.unit_name == grp.Key.Pool_name_en).Count(),
                                       ac_resign = lseEmpRes.Where(w => w.unit_name == grp.Key.Pool_name_en).Count(),
                                       ac_starting = _getprevious.Where(w => w.Pool == grp.Key.Pool_name_en).Count(),
                                   }).ToList();
            //result.lstDataTotal = (from lstplan in _getdivision.OrderBy(o => o.division_name_en)
            //                       group new { lstplan } by new
            //                       {
            //                           lstplan.TM_Pool.
            //                       } into grp
            //                       select new vHCPlanAndSummaryReportData_Total
            //                       {
            //                           division = grp.Key.Pool + "",
            //                           plan = lstFPlan.Where(w => w.Pool == grp.Key.Pool).Sum(s => s.nSum),
            //                           ac_new_hires = lstEmplist.Where(w => w.unit_name == grp.Key.Pool).Count(),
            //                           current_hc = lseEmpCur.Where(w => w.unit_name == grp.Key.Pool).Count(),
            //                           ac_resign = lseEmpRes.Where(w => w.unit_name == grp.Key.Pool).Count(),
            //                           ac_starting = _getprevious.Where(w => w.Pool == grp.Key.Pool).Count(),

            //                       }).ToList();
            //string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            //string sSession = "rpHCPlanAndSummary" + unixTimestamps;
            //result.session = sSession;

            //Session[sSession] = new List<vHCPlanAndSummaryReportData>();
            if (lstData.Any())
            {
                result.lstData = lstData.ToList();
                Session[SearchItem.session] = lstData.ToList();

            }

            return Json(new { result });
        }
        public class CSearchHCPlanAndSummary
        {
            [DefaultValue("")]
            public string fy_year { get; set; }
            [DefaultValue("")]
            public string n_month { get; set; }
            [DefaultValue("")]
            public string session { get; set; }
        }
        #endregion
    }
}