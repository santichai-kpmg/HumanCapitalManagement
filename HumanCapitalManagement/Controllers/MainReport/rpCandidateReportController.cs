using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Report.DataSet.Candidate;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.ReportVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HumanCapitalManagement.App_Start.HCMClass;

namespace HumanCapitalManagement.Controllers.MainReport
{
    public class rpCandidateReportController : BaseController
    {
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;


        private TM_Candidate_MassTIFService _TM_Candidate_MassTIFService;
        private TM_Candidate_Status_CycleService _TM_Candidate_Status_CycleService;
        private TM_Candidate_TIFService _TM_Candidate_TIFService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public rpCandidateReportController(
         TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService


            , TM_Candidate_MassTIFService TM_Candidate_MassTIFService
             , TM_Candidate_Status_CycleService TM_Candidate_Status_CycleService
            , TM_Candidate_TIFService TM_Candidate_TIFService)
        {
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;

            _TM_Candidate_MassTIFService = TM_Candidate_MassTIFService;
            _TM_Candidate_Status_CycleService = TM_Candidate_Status_CycleService;
            _TM_Candidate_TIFService = TM_Candidate_TIFService;
        }
        // GET: rpCandidateReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ExpCandidateStatus(string qryStr, string session)
        {
            var sCheck = acCheckLoginAndPermissionExport("rCandidateStatusList", "CandidateStatusReport");
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;

            if (!string.IsNullOrEmpty(session))
            {
                vrCandidateStatus_Session objSession = Session[session] as vrCandidateStatus_Session; //as List<ListAdvanceTransaction>;
                //List<ListAdvanceTransaction> lstData = Session[qryStr] as List<ListAdvanceTransaction>;
                if (objSession != null && objSession.lstData != null && objSession.lstData.Any())
                {
                    try
                    {
                        // var _getData = _TM_Candidate_MassTIFService.FindByMappingArrayID(_getReport.Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray());
                        XrTemp xrTemp = new XrTemp();
                        var stream = new MemoryStream();
                        xrTemp.CreateDocument();
                        DataTable dtR = SystemFunction.LinqToDataTable(objSession.lstData);
                        dsCandidateStatus ds = new dsCandidateStatus();
                        ds.dsMain.Merge(dtR);
                        ReportCandidateStatus report = new ReportCandidateStatus();

                        report.DataSource = ds;
                        report.CreateDocument();
                        XlsxExportOptions xlsxOptions = xrTemp.ExportOptions.Xlsx;
                        xlsxOptions.ShowGridLines = true;
                        xlsxOptions.SheetName = "sheet_";
                        xlsxOptions.TextExportMode = TextExportMode.Text;
                        xlsxOptions.ExportMode = XlsxExportMode.SingleFilePageByPage;

                        report.ExportToXlsx(stream, xlsxOptions);
                        stream.Seek(0, SeekOrigin.Begin);
                        byte[] breport = stream.ToArray();
                        return File(breport, "application/excel", "CandidateStatus" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");

                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                        throw;
                    }
                }

                else
                {
                    return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                }
                //  return View();
            }
            else
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
            }

        }
        public ActionResult ExpLeadTime(string qryStr, string session)
        {
            var sCheck = acCheckLoginAndPermissionExport("PRCandidateList", "PRCandidate");
            if (sCheck != null)
            {
                return sCheck;
            }
            XrTemp xrTemp = new XrTemp();
            xrTemp.CreateDocument();
            DateTime dNow = DateTime.Now;
            bool isAdmin = CGlobal.UserIsAdmin();

            DateTime dTarget = SystemFunction.ConvertStringToDateTime("01-Jan-2018", "");
            var _getData = _TM_PR_Candidate_MappingService.GetForLeadtime(dTarget, CGlobal.UserInfo.EmployeeNo, isAdmin);
            try
            {
                DateTime d16May = SystemFunction.ConvertStringToDateTime("16-May-2019", "");
                wsHRIS.HRISSoap Hris = new wsHRIS.HRISSoapClient();
                var holiday2018 = Hris.Get_Holiday("2018");
                var holiday2019 = Hris.Get_Holiday("2019");
                List<DateTime> lstDateHoliday = new List<DateTime>();
                if (holiday2018 != null)
                {
                    lstDateHoliday.AddRange(holiday2018.AsEnumerable().Select(s => s.Field<DateTime>("Date")).ToList());


                }
                if (holiday2019 != null)
                {
                    lstDateHoliday.AddRange(holiday2019.AsEnumerable().Select(s => s.Field<DateTime>("Date")).ToList());


                }

                List<int> lstNotSelect = new List<int>();
                lstNotSelect.Add((int)StatusCandidate.Turndown);
                lstNotSelect.Add((int)StatusCandidate.NoShow);
                lstNotSelect.Add((int)StatusCandidate.Reject_Before_Sending_Hiring);
                lstNotSelect.Add((int)StatusCandidate.Withdraw_the_Offer_KPMG);
                lstNotSelect.Add((int)StatusCandidate.Reject_Before_Offer_Date);
                lstNotSelect.Add((int)StatusCandidate.Candidate_Withdrawn_Recruit);
                lstNotSelect.Add((int)StatusCandidate.Blacklist);
                lstNotSelect.Add((int)StatusCandidate.Offer_Rejected);
                lstNotSelect.Add((int)StatusCandidate.OnBoard);


                // var _getData = _TM_Candidate_MassTIFService.FindByMappingArrayID(_getReport.Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray());
                #region Onboard List 
                var _getOnboard = _getData.Where(w => w.TM_Candidate_Status_Cycle.Any(a => a.active_status == "Y" && a.TM_Candidate_Status.Id == (int)StatusCandidate.OnBoard)).ToList();
                if (_getOnboard.Any())
                {
                    string[] aRequestUser = _getOnboard.Select(s => s.TM_Recruitment_Team.user_no).ToArray();
                    var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                    List<vLeadTime> lstLead = new List<vLeadTime>();
                    lstLead = (from item in _getOnboard

                               select new vLeadTime
                               {
                                   spool = item.Pool_name(),
                                   group_name = item.Group_name(),
                                   request_type = item.RequestType_name(),
                                   position_name = item.Position_name(),
                                   request_date = item.PersonnelRequest.request_date,
                                   approve_date = item.PersonnelRequest.Approved_Date(),
                                   accepte_date = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.Accepted).Select(s => s.action_date).FirstOrDefault() : null,
                                   date_offer = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.OfferAccepted).Select(s => s.action_date).FirstOrDefault() : null,
                                   send_mgr = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.sendtoMGRHR).Select(s => s.action_date).FirstOrDefault() : null,
                                   onboard = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.OnBoard).Select(s => s.action_date).FirstOrDefault() : null,
                                   //  hiring_a = null,
                                   bu_pass = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.BuPass).Select(s => s.action_date).FirstOrDefault() : null,
                                   approval_status = "Y",
                                   candidate_name = item.Candidate_name(),
                                   nfilled = 1,
                                   no = "",
                                   target = item.PersonnelRequest.no_of_headcount,
                                   lead_date = 0,
                                   n_group = item.PersonnelRequest.TM_Pool_Rank.TM_Rank.piority <= 5 ? "AM up" : "SR down",
                                   owner_name = item.TM_Recruitment_Team != null ? _getRequestUser.Where(w => w.EmpNo == item.TM_Recruitment_Team.user_no + "").Select(s => s.EmpFullName).FirstOrDefault() : "",
                                   owner_no = item.TM_Recruitment_Team != null ? item.TM_Recruitment_Team.user_no + "" : "",
                                   rank_grade = item.TM_Candidate_Rank != null ? item.TM_Candidate_Rank.crank_short_name_en + "" : "",
                                   remark = item.description + "",
                                   sourcing = "",
                                   verbal_date = "",
                                   refno = item.PersonnelRequest.RefNo,
                               }).ToList();

                    if (lstLead.Any())
                    {
                        lstLead.ForEach(ed =>
                        {
                            if (ed.approve_date.HasValue)
                            {
                                if (ed.approve_date.HasValue && ed.bu_pass.HasValue)
                                {
                                    ed.lead_date = SHA.fwGetWorkingDays(ed.approve_date.Value, ed.bu_pass.Value, lstDateHoliday);
                                }
                            }
                            else
                            {
                                ed.approval_status = "N";
                            }
                            if (ed.send_mgr.HasValue && ed.date_offer.HasValue)
                            {
                                ed.lead_offer = SHA.fwGetWorkingDays(ed.send_mgr.Value, ed.date_offer.Value, lstDateHoliday);
                            }
                        });

                        var _getBefore16 = lstLead.Where(w => w.onboard <= d16May).ToList();
                        var _getAfter16 = lstLead.Where(w => w.onboard > d16May).ToList();

                        if (_getBefore16.Any())
                        {
                            DataTable dtR = SystemFunction.LinqToDataTable(_getBefore16);
                            dsLeadTime ds = new dsLeadTime();
                            ds.dsMain.Merge(dtR);
                            ReportLeadTime report = new ReportLeadTime();
                            XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                            xrLabel1.Text = "Successful Candidates";

                            report.DataSource = ds;
                            report.CreateDocument();
                            xrTemp.Pages.AddRange(report.Pages);
                        }
                        if (_getAfter16.Any())
                        {
                            DataTable dtR = SystemFunction.LinqToDataTable(_getAfter16);
                            dsLeadTime ds2 = new dsLeadTime();
                            ds2.dsMain.Merge(dtR);
                            ReportLeadTime report = new ReportLeadTime();
                            XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                            xrLabel1.Text = "To be on-board (After 16th May 2018 -- Already accepted the offer)";

                            report.DataSource = ds2;
                            report.CreateDocument();
                            xrTemp.Pages.AddRange(report.Pages);
                        }


                    }
                }

                #endregion

                #region on Hiring

                var _getOnHiring = _getData.Where(w => !w.TM_Candidate_Status_Cycle.Any(a => a.active_status == "Y" && lstNotSelect.Contains(a.TM_Candidate_Status.Id))
                && w.TM_Candidate_Status_Cycle.Any(a => a.active_status == "Y" && a.TM_Candidate_Status.Id == (int)StatusCandidate.BuPass)
                ).ToList();
                if (_getOnHiring.Any())
                {
                    string[] aRequestUser = _getOnHiring.Select(s => s.TM_Recruitment_Team.user_no).ToArray();
                    var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                    List<vLeadTime> lstLead = new List<vLeadTime>();
                    lstLead = (from item in _getOnHiring

                               select new vLeadTime
                               {
                                   spool = item.Pool_name(),
                                   group_name = item.Group_name(),
                                   request_type = item.RequestType_name(),
                                   position_name = item.Position_name(),
                                   request_date = item.PersonnelRequest.request_date,
                                   approve_date = item.PersonnelRequest.Approved_Date(),
                                   accepte_date = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.Accepted).Select(s => s.action_date).FirstOrDefault() : null,
                                   date_offer = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.OfferAccepted).Select(s => s.action_date).FirstOrDefault() : null,
                                   send_mgr = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.sendtoMGRHR).Select(s => s.action_date).FirstOrDefault() : null,
                                   onboard = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.OnBoard).Select(s => s.action_date).FirstOrDefault() : null,
                                   //  hiring_a = null,
                                   bu_pass = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.BuPass).Select(s => s.action_date).FirstOrDefault() : null,
                                   approval_status = "Y",
                                   candidate_name = item.Candidate_name(),
                                   nfilled = 1,
                                   no = "",
                                   target = item.PersonnelRequest.no_of_headcount,
                                   lead_date = 0,
                                   n_group = item.PersonnelRequest.TM_Pool_Rank.TM_Rank.piority <= 5 ? "AM up" : "SR down",
                                   owner_name = item.TM_Recruitment_Team != null ? _getRequestUser.Where(w => w.EmpNo == item.TM_Recruitment_Team.user_no + "").Select(s => s.EmpFullName).FirstOrDefault() : "",
                                   owner_no = item.TM_Recruitment_Team != null ? item.TM_Recruitment_Team.user_no + "" : "",
                                   rank_grade = item.TM_Candidate_Rank != null ? item.TM_Candidate_Rank.crank_short_name_en + "" : "",
                                   remark = item.description + "",
                                   sourcing = "",
                                   verbal_date = "",
                                   refno = item.PersonnelRequest.RefNo,
                               }).ToList();

                    if (lstLead.Any())
                    {
                        lstLead.ForEach(ed =>
                        {
                            if (ed.approve_date.HasValue)
                            {
                                if (ed.approve_date.HasValue && ed.bu_pass.HasValue)
                                {
                                    ed.lead_date = SHA.fwGetWorkingDays(ed.approve_date.Value, ed.bu_pass.Value, lstDateHoliday);
                                }
                            }
                            else
                            {
                                ed.approval_status = "N";
                            }
                        });
                        DataTable dtR = SystemFunction.LinqToDataTable(lstLead);
                        dsLeadTime ds = new dsLeadTime();
                        ds.dsMain.Merge(dtR);
                        ReportLeadTime report = new ReportLeadTime();
                        XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                        xrLabel1.Text = "On Hiring Process";

                        report.DataSource = ds;
                        report.CreateDocument();
                        xrTemp.Pages.AddRange(report.Pages);
                    }

                }


                #endregion


                #region on Reject
                List<int> lstNotSelectReject = new List<int>();
                lstNotSelectReject.Add((int)StatusCandidate.Turndown);
                lstNotSelectReject.Add((int)StatusCandidate.Offer_Rejected);

                var _getReject = _getData.Where(w => w.TM_Candidate_Status_Cycle.Any(a => a.active_status == "Y" && lstNotSelectReject.Contains(a.TM_Candidate_Status.Id))).ToList();
                if (_getReject.Any())
                {
                    string[] aRequestUser = _getReject.Select(s => s.TM_Recruitment_Team.user_no).ToArray();
                    var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                    List<vLeadTime> lstLead = new List<vLeadTime>();
                    lstLead = (from item in _getReject

                               select new vLeadTime
                               {
                                   spool = item.Pool_name(),
                                   group_name = item.Group_name(),
                                   request_type = item.RequestType_name(),
                                   position_name = item.Position_name(),
                                   request_date = item.PersonnelRequest.request_date,
                                   approve_date = item.PersonnelRequest.Approved_Date(),
                                   accepte_date = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && (w.TM_Candidate_Status.Id == (int)StatusCandidate.Turndown || w.TM_Candidate_Status.Id == (int)StatusCandidate.Offer_Rejected)).Select(s => s.action_date).FirstOrDefault() : null,
                                   date_offer = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.OfferAccepted).Select(s => s.action_date).FirstOrDefault() : null,
                                   send_mgr = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.sendtoMGRHR).Select(s => s.action_date).FirstOrDefault() : null,
                                   onboard = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.OnBoard).Select(s => s.action_date).FirstOrDefault() : null,
                                   //  hiring_a = null,
                                   bu_pass = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.BuPass).Select(s => s.action_date).FirstOrDefault() : null,
                                   approval_status = "Y",
                                   candidate_name = item.Candidate_name(),
                                   nfilled = 1,
                                   no = "",
                                   target = item.PersonnelRequest.no_of_headcount,
                                   lead_date = 0,
                                   n_group = item.PersonnelRequest.TM_Pool_Rank.TM_Rank.piority <= 5 ? "AM up" : "SR down",
                                   owner_name = item.TM_Recruitment_Team != null ? _getRequestUser.Where(w => w.EmpNo == item.TM_Recruitment_Team.user_no + "").Select(s => s.EmpFullName).FirstOrDefault() : "",
                                   owner_no = item.TM_Recruitment_Team != null ? item.TM_Recruitment_Team.user_no + "" : "",
                                   rank_grade = item.TM_Candidate_Rank != null ? item.TM_Candidate_Rank.crank_short_name_en + "" : "",
                                   remark = item.description + "",
                                   sourcing = "",
                                   verbal_date = "",
                                   refno = item.PersonnelRequest.RefNo,
                               }).ToList();

                    if (lstLead.Any())
                    {
                        lstLead.ForEach(ed =>
                        {
                            if (ed.approve_date.HasValue)
                            {
                                if (ed.approve_date.HasValue && ed.bu_pass.HasValue)
                                {
                                    ed.lead_date = SHA.fwGetWorkingDays(ed.approve_date.Value, ed.bu_pass.Value, lstDateHoliday);
                                }
                            }
                            else
                            {
                                ed.approval_status = "N";
                            }
                            if (ed.send_mgr.HasValue && ed.date_offer.HasValue)
                            {
                                ed.lead_offer = SHA.fwGetWorkingDays(ed.send_mgr.Value, ed.date_offer.Value, lstDateHoliday);
                            }
                        });
                        DataTable dtR = SystemFunction.LinqToDataTable(lstLead);
                        dsLeadTime ds = new dsLeadTime();
                        ds.dsMain.Merge(dtR);
                        ReportLeadTime report = new ReportLeadTime();
                        XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                        xrLabel1.Text = "Rejected / Turn Down an Offer";

                        report.DataSource = ds;
                        report.CreateDocument();
                        xrTemp.Pages.AddRange(report.Pages);
                    }
                }


                #endregion



                #region Active Job Vacancy listed



                var _getActive = _getData.Where(w => !w.TM_Candidate_Status_Cycle.Any(a => a.active_status == "Y" && lstNotSelect.Contains(a.TM_Candidate_Status.Id))
             && !w.TM_Candidate_Status_Cycle.Any(a => a.active_status == "Y" && a.TM_Candidate_Status.Id == (int)StatusCandidate.BuPass)
             ).ToList();
                if (_getActive.Any())
                {
                    string[] aRequestUser = _getActive.Select(s => s.TM_Recruitment_Team.user_no).ToArray();
                    var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                    List<vLeadTime> lstLead = new List<vLeadTime>();
                    lstLead = (from item in _getActive

                               select new vLeadTime
                               {
                                   spool = item.Pool_name(),
                                   group_name = item.Group_name(),
                                   request_type = item.RequestType_name(),
                                   position_name = item.Position_name(),
                                   request_date = item.PersonnelRequest.request_date,
                                   approve_date = item.PersonnelRequest.Approved_Date(),
                                   accepte_date = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.Accepted).Select(s => s.action_date).FirstOrDefault() : null,
                                   date_offer = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.OfferAccepted).Select(s => s.action_date).FirstOrDefault() : null,
                                   send_mgr = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.sendtoMGRHR).Select(s => s.action_date).FirstOrDefault() : null,
                                   onboard = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.OnBoard).Select(s => s.action_date).FirstOrDefault() : null,
                                   //  hiring_a = null,
                                   bu_pass = item.TM_Candidate_Status_Cycle != null ? item.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y" && w.TM_Candidate_Status.Id == (int)StatusCandidate.BuPass).Select(s => s.action_date).FirstOrDefault() : null,
                                   approval_status = "Y",
                                   candidate_name = item.Candidate_name(),
                                   nfilled = 1,
                                   no = "",
                                   target = item.PersonnelRequest.no_of_headcount,
                                   lead_date = 0,
                                   n_group = item.PersonnelRequest.TM_Pool_Rank.TM_Rank.piority <= 5 ? "AM up" : "SR down",
                                   owner_name = item.TM_Recruitment_Team != null ? _getRequestUser.Where(w => w.EmpNo == item.TM_Recruitment_Team.user_no + "").Select(s => s.EmpFullName).FirstOrDefault() : "",
                                   owner_no = item.TM_Recruitment_Team != null ? item.TM_Recruitment_Team.user_no + "" : "",
                                   rank_grade = item.TM_Candidate_Rank != null ? item.TM_Candidate_Rank.crank_short_name_en + "" : "",
                                   remark = item.description + "",
                                   sourcing = "",
                                   verbal_date = "",
                                   refno = item.PersonnelRequest.RefNo,
                               }).ToList();

                    if (lstLead.Any())
                    {
                        lstLead.ForEach(ed =>
                        {
                            if (ed.approve_date.HasValue)
                            {
                                if (ed.approve_date.HasValue && ed.bu_pass.HasValue)
                                {
                                    ed.lead_date = SHA.fwGetWorkingDays(ed.approve_date.Value, ed.bu_pass.Value, lstDateHoliday);
                                }
                            }
                            else
                            {
                                ed.approval_status = "N";
                            }
                            if (ed.send_mgr.HasValue && ed.date_offer.HasValue)
                            {
                                ed.lead_offer = SHA.fwGetWorkingDays(ed.send_mgr.Value, ed.date_offer.Value, lstDateHoliday);
                            }
                        });
                        DataTable dtR = SystemFunction.LinqToDataTable(lstLead);
                        dsLeadTime ds = new dsLeadTime();
                        ds.dsMain.Merge(dtR);
                        ReportLeadTime report = new ReportLeadTime();
                        XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                        xrLabel1.Text = "Active Job Vacancy listed";

                        report.DataSource = ds;
                        report.CreateDocument();
                        xrTemp.Pages.AddRange(report.Pages);
                    }
                }


                #endregion

                var stream = new MemoryStream();


                XlsxExportOptions xlsxOptions = xrTemp.ExportOptions.Xlsx;
                xlsxOptions.ShowGridLines = true;
                xlsxOptions.SheetName = "sheet_";

                // xlsxOptions.TextExportMode = TextExportMode.Text;
                xlsxOptions.TextExportMode = TextExportMode.Value;
                xlsxOptions.ExportMode = XlsxExportMode.SingleFilePageByPage;

                xrTemp.ExportToXlsx(stream, xlsxOptions);
                stream.Seek(0, SeekOrigin.Begin);
                byte[] breport = stream.ToArray();
                return File(breport, "application/excel", "Leadtime_" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");

            }
            catch (Exception e)
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                throw;
            }

            //  return View();

        }

    }

}