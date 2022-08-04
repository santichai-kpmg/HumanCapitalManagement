using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.ReportVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HumanCapitalManagement.App_Start.HCMClass;
using static HumanCapitalManagement.Controllers.Report.rHeadcountController;

namespace HumanCapitalManagement.Controllers.Report
{
    public class rStaffMovementController : BaseController
    {
        private DivisionService _DivisionService;
        private TM_FY_PlanService _TM_FY_PlanService;
        private TM_FY_DetailService _TM_FY_DetailService;
        private PersonnelRequestService _PersonnelRequestService;
        private wsHRIS.HRISSoapClient wsHRIS = new wsHRIS.HRISSoapClient();
        private New_HRISEntities dbHr = new New_HRISEntities();
        public rStaffMovementController(DivisionService DivisionService, TM_FY_PlanService TM_FY_PlanService, TM_FY_DetailService TM_FY_DetailService, PersonnelRequestService PersonnelRequestService)
        {
            _DivisionService = DivisionService;
            _TM_FY_PlanService = TM_FY_PlanService;
            _TM_FY_DetailService = TM_FY_DetailService;
            _PersonnelRequestService = PersonnelRequestService;
        }
      
        // GET: rStaffMovement
        public ActionResult StaffMovement()
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
            bool isAdmin = CGlobal.UserIsAdmin();


            vStaffMovement result = new vStaffMovement();
            result.full_last_year = (fy - 1) + "";
            result.fysh_year = dFyYear.Value.ToString("yy");
            result.fy_year = _TM_FY_PlanService.GetYear(fy) + "";
            result.mtd_start = dNow.DateTimebyCulture("MM/yyyy");
            var startDate = new DateTime(dNow.Year, dNow.Month, 1);


            List<vStaffMovementReportData> lstData = new List<vStaffMovementReportData>();
            List<vStaffMovementReportData> newlstDate = new List<vStaffMovementReportData>();
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
            //    newlstDate = _getNewUnitGroup.Select(s => new vStaffMovementReportData
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
            var _getPR = _PersonnelRequestService.GetPRListForStaffMovement("", aAlldivision, isAdmin);
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
            var empListNew = empList.Where(w => w.JoinDate <= startDate).ToList(); ;
            List<vClassEmployeeFromWS> lstEmplist = empListNew.Where(w => aAlldivision.Contains(w.UnitGroupID + ""))
             .Select(e => new vClassEmployeeFromWS
             {
                 unitgroup = e.UnitGroup + "",
                 emp_no = e.EmpNo + "",//e["EmpNo"].ToString(),
                 unit_name = e.Pool + "" != "Tax" ? (e.Pool + "" == "Shared Services" ? "Practice Support & Shared Services" : e.Pool + "") : "Tax & Legal",

             }).ToList();
            List<vClassEmployeeFromWS> lseEmpCur = (from row in empListNew.AsEnumerable().Where(w => aAlldivision.Contains(w.UnitGroupID + ""))
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
                       select new vStaffMovementReportData
                       {
                           division = grp.Key.Pool_name_en + "",
                           sgroup = grp.Key.division_name_en + "",
                           plan = lstFPlan.Where(w => w.unit_code == grp.Key.division_code).FirstOrDefault() != null ? lstFPlan.Where(w => w.unit_code == grp.Key.division_code).Select(s => s.nSum).FirstOrDefault() : 0,
                           ac_new_hires = lstEmplist.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                           current_hc = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                           ac_resign = lseEmpRes.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                           ac_starting = _getprevious.Where(w => w.group_code == grp.Key.division_code).Count(),
                           new_join = _getPR.Where(w => w.TM_Divisions.division_code == grp.Key.division_code && w.TM_PR_Candidate_Mapping.Any(a => a.active_status + "" == "Y"
                           &&
                           a.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y" && w2.action_date > startDate).Select(s => s.TM_Candidate_Status.Id).Contains((int)StatusCandidate.OnBoard)
                           &&
                           !a.TM_Candidate_Status_Cycle.Any(a2 => HCMClass.lstNotSelect().Contains(a2.TM_Candidate_Status.Id) && a2.active_status == "Y")
                           )).Count()
                       }).ToList();

            //var xxfex = lseEmpCur.Where(w => w.unitgroup == "T&L-Support").ToList();
            //DataTable dtxx = SystemFunction.LinqToDataTable(xxfex.OrderBy(o => o.unitgroup));

            #region Move Plan 
            List<vClassEmployeeFromWS> lseEmpCurMove = (from row in empListNew
                                                        group row by new { ID = row.EmpNo, UnitGroup = row.UnitGroup, Rank = row.RankCode, row.UnitGroupID } into grp
                                                        select new vClassEmployeeFromWS
                                                        {
                                                            emp_no = grp.Key.ID,
                                                            unitgroup = grp.Key.UnitGroup,
                                                            rank = grp.Key.Rank,
                                                            unitgroup_code = grp.Key.UnitGroupID + "",
                                                        }).ToList();
            result.lstPlan = (from lstdivision in _getdivision.OrderBy(o => o.seq)
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
                                  para = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "ASS ASSOC" || w.rank == "EA")).Count(),
                                  aa = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "ASSOC" || w.rank == "STAFF" || w.rank == "ASS")).Count(),
                                  sr = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "SR")).Count(),
                                  am = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "AM")).Count(),
                                  mgr = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "MGR")).Count(),
                                  ad = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "ASSOC D")).Count(),
                                  dir = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "EXEC DI")).Count(),
                                  ptr = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "PTR" || w.rank == "ADV")).Count(),
                                  total = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                                  type_name = "Current HC",
                              }).ToList();
            List<vCurrentStaffReportData> PlanResult = new List<vCurrentStaffReportData>();

            PlanResult = (from lstdivision in _getdivision.OrderBy(o => o.seq)
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
                              para = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.para.HasValue ? (int)s.para.Value : 0).FirstOrDefault(),
                              aa = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.aa.HasValue ? (int)s.aa.Value : 0).FirstOrDefault(),
                              sr = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.sr.HasValue ? (int)s.sr.Value : 0).FirstOrDefault(),
                              am = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.am.HasValue ? (int)s.am.Value : 0).FirstOrDefault(),
                              mgr = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.mgr.HasValue ? (int)s.mgr.Value : 0).FirstOrDefault(),
                              ad = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.ad.HasValue ? (int)s.ad.Value : 0).FirstOrDefault(),
                              dir = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.dir.HasValue ? (int)s.dir.Value : 0).FirstOrDefault(),
                              ptr = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.ptr.HasValue ? (int)s.ptr.Value : 0).FirstOrDefault(),
                              total = lstFPlan.Where(w => w.unit_code == grp.Key.division_code).FirstOrDefault() != null ? (int)lstFPlan.Where(w => w.unit_code == grp.Key.division_code).Select(s => s.nSum).FirstOrDefault() : 0,
                              type_name = "HC Plan",
                          }).ToList();
            if (PlanResult.Any())
            {
                result.lstPlan.AddRange(PlanResult);
            }
            #endregion


            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "rpStaffMovement" + unixTimestamps;
            result.session = sSession;
            vStaffMovement objSession = new vStaffMovement();
            Session[sSession] = new vStaffMovement();

            if (lstData.Any())
            {
                objSession.lstData = lstData.ToList();
                objSession.lstPlan = result.lstPlan.ToList();
                result.lstData = lstData.ToList();
                Session[sSession] = objSession;

            }

            return View(result);
        }

        #region Ajax

        [HttpPost]
        public ActionResult LoadStaffMovement(CSearchHCPlanAndSummary SearchItem)
        {
            StoreDb db = new StoreDb();
            var dNow = SystemFunction.ConvertStringToDateTime("01/" + SearchItem.n_month, "", "dd/MM/yyyy");
            int fy;
            DateTime? dFyYear = null;
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
            bool isAdmin = CGlobal.UserIsAdmin();


            vStaffMovement result = new vStaffMovement();
            result.full_last_year = (fy - 1) + "";
            result.fysh_year = dFyYear.Value.ToString("yy");
            result.fy_year = _TM_FY_PlanService.GetYear(fy) + "";
            result.mtd_start = dNow.DateTimebyCulture("MM/yyyy");
            var startDate = new DateTime(dNow.Year, dNow.Month, 1);
            vStaffMovement objSession = new vStaffMovement();




            List<vStaffMovementReportData> lstData = new List<vStaffMovementReportData>();
            List<vStaffMovementReportData> newlstDate = new List<vStaffMovementReportData>();
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
            //    newlstDate = _getNewUnitGroup.Select(s => new vStaffMovementReportData
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
            var _getPR = _PersonnelRequestService.GetPRListForStaffMovement("", aAlldivision, isAdmin);
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
            var empListNew = empList.Where(w => w.JoinDate <= startDate).ToList(); ;
            List<vClassEmployeeFromWS> lstEmplist = empListNew.Where(w => aAlldivision.Contains(w.UnitGroupID + ""))
             .Select(e => new vClassEmployeeFromWS
             {
                 unitgroup = e.UnitGroup + "",
                 emp_no = e.EmpNo + "",//e["EmpNo"].ToString(),
                 unit_name = e.Pool + "" != "Tax" ? (e.Pool + "" == "Shared Services" ? "Practice Support & Shared Services" : e.Pool + "") : "Tax & Legal",

             }).ToList();
            List<vClassEmployeeFromWS> lseEmpCur = (from row in empListNew.AsEnumerable().Where(w => aAlldivision.Contains(w.UnitGroupID + ""))
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
                       select new vStaffMovementReportData
                       {
                           division = grp.Key.Pool_name_en + "",
                           sgroup = grp.Key.division_name_en + "",
                           plan = lstFPlan.Where(w => w.unit_code == grp.Key.division_code).FirstOrDefault() != null ? lstFPlan.Where(w => w.unit_code == grp.Key.division_code).Select(s => s.nSum).FirstOrDefault() : 0,
                           ac_new_hires = lstEmplist.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                           current_hc = lseEmpCur.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                           ac_resign = lseEmpRes.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                           ac_starting = _getprevious.Where(w => w.group_code == grp.Key.division_code).Count(),
                           new_join = _getPR.Where(w => w.TM_Divisions.division_code == grp.Key.division_code && w.TM_PR_Candidate_Mapping.Any(a => a.active_status + "" == "Y"
                           &&
                           a.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y" && w2.action_date > startDate).Select(s => s.TM_Candidate_Status.Id).Contains((int)StatusCandidate.OnBoard)
                           &&
                           !a.TM_Candidate_Status_Cycle.Any(a2 => HCMClass.lstNotSelect().Contains(a2.TM_Candidate_Status.Id) && a2.active_status == "Y")
                           )).Count()
                       }).ToList();

            //var xxfex = lseEmpCur.Where(w => w.unitgroup == "T&L-Support").ToList();
            //DataTable dtxx = SystemFunction.LinqToDataTable(xxfex.OrderBy(o => o.unitgroup));

            #region Move Plan 
            List<vClassEmployeeFromWS> lseEmpCurMove = (from row in empList
                                                        group row by new { ID = row.EmpNo, UnitGroup = row.UnitGroup, Rank = row.RankCode, row.UnitGroupID } into grp
                                                        select new vClassEmployeeFromWS
                                                        {
                                                            emp_no = grp.Key.ID,
                                                            unitgroup = grp.Key.UnitGroup,
                                                            rank = grp.Key.Rank,
                                                            unitgroup_code = grp.Key.UnitGroupID + "",
                                                        }).ToList();
            result.lstPlan = (from lstdivision in _getdivision.OrderBy(o => o.seq)
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
                                  para = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "ASS ASSOC" || w.rank == "EA")).Count(),
                                  aa = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "ASSOC" || w.rank == "STAFF" || w.rank == "ASS")).Count(),
                                  sr = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "SR")).Count(),
                                  am = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "AM")).Count(),
                                  mgr = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "MGR")).Count(),
                                  ad = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "ASSOC D")).Count(),
                                  dir = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "EXEC DI")).Count(),
                                  ptr = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code && (w.rank == "PTR" || w.rank == "ADV")).Count(),
                                  total = lseEmpCurMove.Where(w => w.unitgroup_code == grp.Key.division_code).Count(),
                                  type_name = "Current HC",
                              }).ToList();
            List<vCurrentStaffReportData> PlanResult = new List<vCurrentStaffReportData>();

            PlanResult = (from lstdivision in _getdivision.OrderBy(o => o.seq)
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
                              para = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.para.HasValue ? (int)s.para.Value : 0).FirstOrDefault(),
                              aa = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.aa.HasValue ? (int)s.aa.Value : 0).FirstOrDefault(),
                              sr = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.sr.HasValue ? (int)s.sr.Value : 0).FirstOrDefault(),
                              am = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.am.HasValue ? (int)s.am.Value : 0).FirstOrDefault(),
                              mgr = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.mgr.HasValue ? (int)s.mgr.Value : 0).FirstOrDefault(),
                              ad = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.ad.HasValue ? (int)s.ad.Value : 0).FirstOrDefault(),
                              dir = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.dir.HasValue ? (int)s.dir.Value : 0).FirstOrDefault(),
                              ptr = _newPlan.Where(w => w.TM_Divisions.division_code == grp.Key.division_code).Select(s => s.ptr.HasValue ? (int)s.ptr.Value : 0).FirstOrDefault(),
                              total = lstFPlan.Where(w => w.unit_code == grp.Key.division_code).FirstOrDefault() != null ? (int)lstFPlan.Where(w => w.unit_code == grp.Key.division_code).Select(s => s.nSum).FirstOrDefault() : 0,
                              type_name = "HC Plan",
                          }).ToList();
            if (PlanResult.Any())
            {
                result.lstPlan.AddRange(PlanResult);
            }
            #endregion

            if (lstData.Any())
            {
                objSession.lstData = lstData.ToList();
                objSession.lstPlan = result.lstPlan.ToList();
                result.lstData = lstData.ToList();
                Session[SearchItem.session] = objSession;
            }

            return Json(new { result });
        }

        #endregion

    }
}