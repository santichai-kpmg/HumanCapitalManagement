using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using HumanCapitalManagement.Report.DataSet.PESDataset;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.PartnerEvaluation;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.PESVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HumanCapitalManagement.App_Start.PESClass;

namespace HumanCapitalManagement.Controllers.PartnerEvaluation
{
    public class rpPESExportController : BaseController
    {
        private PTR_Evaluation_YearService _PTR_Evaluation_YearService;
        private PTR_EvaluationService _PTR_EvaluationService;
        private TM_PTR_Eva_ApproveStepService _TM_PTR_Eva_ApproveStepService;
        private TM_KPIs_BaseService _TM_KPIs_BaseService;
        private PTR_Evaluation_KPIsService _PTR_Evaluation_KPIsService;
        private PTR_Evaluation_FileService _PTR_Evaluation_FileService;
        private TM_PTR_Eva_StatusService _TM_PTR_Eva_StatusService;
        private TM_Partner_EvaluationService _TM_Partner_EvaluationService;
        private PTR_Evaluation_ApproveService _PTR_Evaluation_ApproveService;
        private TM_Annual_RatingService _TM_Annual_RatingService;
        private PTR_Evaluation_AnswerService _PTR_Evaluation_AnswerService;
        private TM_Feedback_RatingService _TM_Feedback_RatingService;
        private DivisionService _DivisionService;
        private PTR_Feedback_EmpService _PTR_Feedback_EmpService;
        private PTR_Feedback_UnitGroupService _PTR_Feedback_UnitGroupService;
        private PTR_Evaluation_Incidents_ScoreService _PTR_Evaluation_Incidents_ScoreService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public rpPESExportController(PTR_Evaluation_YearService PTR_Evaluation_YearService
            , PTR_EvaluationService PTR_EvaluationService
            , TM_PTR_Eva_ApproveStepService TM_PTR_Eva_ApproveStepService
            , TM_KPIs_BaseService TM_KPIs_BaseService
            , PTR_Evaluation_KPIsService PTR_Evaluation_KPIsService
            , PTR_Evaluation_FileService PTR_Evaluation_FileService
            , TM_PTR_Eva_StatusService TM_PTR_Eva_StatusService
            , TM_Partner_EvaluationService TM_Partner_EvaluationService
            , PTR_Evaluation_ApproveService PTR_Evaluation_ApproveService
            , TM_Annual_RatingService TM_Annual_RatingService
            , PTR_Evaluation_AnswerService PTR_Evaluation_AnswerService
            , TM_Feedback_RatingService TM_Feedback_RatingService
            , DivisionService DivisionService
            , PTR_Feedback_EmpService PTR_Feedback_EmpService
            , PTR_Feedback_UnitGroupService PTR_Feedback_UnitGroupService
            , PTR_Evaluation_Incidents_ScoreService PTR_Evaluation_Incidents_ScoreService
           )
        {
            _PTR_Evaluation_YearService = PTR_Evaluation_YearService;
            _PTR_EvaluationService = PTR_EvaluationService;
            _TM_PTR_Eva_ApproveStepService = TM_PTR_Eva_ApproveStepService;
            _TM_KPIs_BaseService = TM_KPIs_BaseService;
            _PTR_Evaluation_KPIsService = PTR_Evaluation_KPIsService;
            _PTR_Evaluation_FileService = PTR_Evaluation_FileService;
            _TM_PTR_Eva_StatusService = TM_PTR_Eva_StatusService;
            _TM_Partner_EvaluationService = TM_Partner_EvaluationService;
            _PTR_Evaluation_ApproveService = PTR_Evaluation_ApproveService;
            _TM_Annual_RatingService = TM_Annual_RatingService;
            _PTR_Evaluation_AnswerService = PTR_Evaluation_AnswerService;
            _TM_Feedback_RatingService = TM_Feedback_RatingService;
            _DivisionService = DivisionService;
            _PTR_Feedback_EmpService = PTR_Feedback_EmpService;
            _PTR_Feedback_UnitGroupService = PTR_Feedback_UnitGroupService;
            _PTR_Evaluation_Incidents_ScoreService = PTR_Evaluation_Incidents_ScoreService;
        }
        // GET: rpPESExport
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ExpEvaluationForm(string qryStr, string sMode = "pdf")
        {
            if (CGlobal.IsUserExpired())
            {
                return RedirectToAction("Login", "Login");
            }
            DateTime dNow = DateTime.Now;

            if (!string.IsNullOrEmpty(qryStr))
            {

                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                var _getData = _PTR_EvaluationService.FindReport(nId);
                bool checkAuthor = false;
                if (_getData != null)
                {
                    if (_getData.PTR_Evaluation_AuthorizedEva != null)
                    {
                        checkAuthor = _getData.PTR_Evaluation_AuthorizedEva.Any(a => a.active_status == "Y" && a.authorized_user == CGlobal.UserInfo.EmployeeNo);
                    }
                }

                if (_getData != null
                    && _getData.PTR_Evaluation_Approve != null
                    && (_getData.PTR_Evaluation_Approve.Any(a => a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G")
                    || CGlobal.UserIsAdminPES())
                    || checkAuthor
                    )
                {

                    var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                    if (_checkActiv != null)
                    {

                        try
                        {
                            XrTemp xrTemp = new XrTemp();
                            var stream = new MemoryStream();
                            xrTemp.CreateDocument();


                            rPESEvaluation18 report = new rPESEvaluation18();

                            rPESEvaQuestion18 report2 = new rPESEvaQuestion18();

                            rPESEvaKPIs18 report3 = new rPESEvaKPIs18();

                            XRLabel xrlblName = (XRLabel)report.FindControl("xrlblName", true);
                            XRLabel xrlblPosition = (XRLabel)report.FindControl("xrlblPosition", true);
                            XRLabel xrlblPool = (XRLabel)report.FindControl("xrlblPool", true);
                            XRLabel xrlblGroup = (XRLabel)report.FindControl("xrlblGroup", true);
                            XRLabel xrlblOtherRole = (XRLabel)report.FindControl("xrlblOtherRole", true);
                            XRLabel xrlblSelfRate = (XRLabel)report.FindControl("xrlblSelfRate", true);
                            XRLabel xrlblFinalRate = (XRLabel)report.FindControl("xrlblFinalRate", true);


                            if (_getData.PTR_Evaluation_Approve != null && _getData.PTR_Evaluation_Approve.Any(a => a.active_status == "Y"))
                            {
                                var _getRate = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self).Select(s => s.Annual_Rating != null ? s.Annual_Rating : null).FirstOrDefault();
                                if (_getRate != null)
                                {
                                    xrlblSelfRate.Text = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? _getRate.rating_name_en + "" : _getRate.rating_name_en;
                                }
                                var _getRateCEO = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Ceo).Select(s => s.Annual_Rating != null ? s.Annual_Rating : null).FirstOrDefault();
                                if (_getRateCEO != null)
                                {
                                    xrlblFinalRate.Text = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? _getRateCEO.rating_name_en + "" : _getRateCEO.rating_name_en;
                                }
                            }
                            List<vrpEvaluation> lstApprover = new List<vrpEvaluation>();
                            List<vrpEvaluation_Answer> lstQuestions = new List<vrpEvaluation_Answer>();
                            List<vPTREvaluation_KPIs> lstKPIs = new List<vPTREvaluation_KPIs>();

                            if (_getData.PTR_Evaluation_Approve != null && _getData.PTR_Evaluation_Approve.Any(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G"))
                            {
                                string[] empNO = _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G").Select(s => s.Req_Approve_user).ToArray();
                                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                lstApprover = (from lst in _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G")
                                               from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                               select new vrpEvaluation
                                               {
                                                   sStepName = lst.TM_PTR_Eva_ApproveStep.Step_name_en,
                                                   sName = lstEmpReq.EmpFullName,
                                                   nOrder = lst.TM_PTR_Eva_ApproveStep.seq.HasValue ? lst.TM_PTR_Eva_ApproveStep.seq.Value : 0,
                                                   dApproved = lst.Approve_date,
                                                   sStatus = ((HCMFunc.DataDecryptPES(lst.responses + "") + "").Replace("<br/>", Environment.NewLine + "")).TexttoReportnewline() + "",
                                               }).ToList();

                            }
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null)
                            {
                                lstQuestions = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.questions_type == "G")
                                                select new vrpEvaluation_Answer
                                                {
                                                    id = lstQ.Id + "",
                                                    question = lstQ.question,
                                                    header = lstQ.header,
                                                    nOrder = lstQ.seq,
                                                    remark = "",
                                                    sgroup = lstQ.questions_type + "" == "P" ? "Personal Performance Plan" : "1. Top 5 Priorities and Achievement",

                                                }).ToList();
                                if (_getData.PTR_Evaluation_Answer != null && _getData.PTR_Evaluation_Answer.Any(a => a.active_status == "Y"))
                                {
                                    lstQuestions.ForEach(ed =>
                                    {
                                        var GetAns = _getData.PTR_Evaluation_Answer.Where(w => w.TM_PTR_Eva_Questions.Id + "" == ed.id).FirstOrDefault();
                                        if (GetAns != null)
                                        {
                                            ed.remark = ((HCMFunc.DataDecryptPES(GetAns.answer + "") + "").Replace("<br/>", Environment.NewLine + "")).TexttoReportnewline() + "";
                                        }
                                    });
                                }
                            }

                            if (_getData.PTR_Evaluation_KPIs != null && _getData.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                            {
                                var _getDataYOne = _PTR_EvaluationService.FindFor3YearKPI(_getData.user_no, _getData.PTR_Evaluation_Year.evaluation_year.Value.AddYears(-1).Year);
                                var _getDataYTwo = _PTR_EvaluationService.FindFor3YearKPI(_getData.user_no, _getData.PTR_Evaluation_Year.evaluation_year.Value.AddYears(-2).Year);
                                var _getDataYThree = _PTR_EvaluationService.FindFor3YearKPI(_getData.user_no, _getData.PTR_Evaluation_Year.evaluation_year.Value.AddYears(-3).Year);
                                List<PTR_Evaluation_KPIs> lstKPI1 = new List<PTR_Evaluation_KPIs>();
                                List<PTR_Evaluation_KPIs> lstKPI2 = new List<PTR_Evaluation_KPIs>();
                                List<PTR_Evaluation_KPIs> lstKPI3 = new List<PTR_Evaluation_KPIs>();
                                if (_getDataYOne != null && _getDataYOne.PTR_Evaluation_KPIs != null && _getDataYOne.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                                {
                                    lstKPI1 = _getDataYOne.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").ToList();
                                }
                                if (_getDataYTwo != null && _getDataYTwo.PTR_Evaluation_KPIs != null && _getDataYTwo.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                                {
                                    lstKPI2 = _getDataYTwo.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").ToList();
                                }
                                if (_getDataYThree != null && _getDataYThree.PTR_Evaluation_KPIs != null && _getDataYThree.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                                {
                                    lstKPI3 = _getDataYThree.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").ToList();
                                }

                                lstKPIs = (from lst in _getData.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y")
                                           from lstKPIOne in lstKPI1.Where(w => w.TM_KPIs_Base.Id == lst.TM_KPIs_Base.Id).DefaultIfEmpty(new PTR_Evaluation_KPIs())
                                           from lstKPITwo in lstKPI2.Where(w => w.TM_KPIs_Base.Id == lst.TM_KPIs_Base.Id).DefaultIfEmpty(new PTR_Evaluation_KPIs())
                                           from lstKPIThree in lstKPI3.Where(w => w.TM_KPIs_Base.Id == lst.TM_KPIs_Base.Id).DefaultIfEmpty(new PTR_Evaluation_KPIs())
                                           select new vPTREvaluation_KPIs
                                           {
                                               sname = lst.TM_KPIs_Base.kpi_name_en,
                                               target_data = lst.TM_KPIs_Base.type_of_kpi == "P" ? SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target)).NodecimalFormatHasvalue() + "-" + SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)).NodecimalFormatHasvalue() + "%" : (lst.TM_KPIs_Base.type_of_kpi == "N" ? SystemFunction.GetNumberNullToZero(FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)))).NodecimalFormatHasvalue() : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)).NodecimalFormatHasvalue()),
                                               remark = ((HCMFunc.DataDecryptPES(lst.final_remark + "") + "").Replace("<br/>", Environment.NewLine + "")).TexttoReportnewline() + "",
                                               actual = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.actual)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                               yearone = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.actual))) : (SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.actual)).NodecimalFormatHasvalue())) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIOne.actual),
                                               yeartwo = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.actual))) : (SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.actual)).NodecimalFormatHasvalue())) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPITwo.actual),
                                               yearthree = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIThree.actual),

                                               group_yearone = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.group_actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIOne.actual),
                                               group_yeartwo = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.group_actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPITwo.actual),
                                               group_yearthree = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.group_actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIThree.actual),

                                               sdisable = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? "N" : "Y",
                                               IdEncrypt = lst.Id + "",
                                               stype = lst.TM_KPIs_Base.type_of_kpi,
                                               target_group = lst.TM_KPIs_Base.type_of_kpi == "P" ? SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target)).NodecimalFormatHasvalue() + "-" + SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max)).NodecimalFormatHasvalue() + "%" : (lst.TM_KPIs_Base.type_of_kpi == "N" ? SystemFunction.GetNumberNullToZero(FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max)))).NodecimalFormatHasvalue() : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max)).NodecimalFormatHasvalue()),
                                               group_actual = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_actual)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                               howto = HCMFunc.DataDecryptPES(lst.how_to),
                                               sbu = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.sbu))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.sbu)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                               su = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.su))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.su)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                           }).ToList();



                                //_getData.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").Select(s => new vPTREvaluation_KPIs
                                //{
                                //    sname = s.TM_KPIs_Base.kpi_name_en,
                                //    target = s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : (s.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(s.target_max))) : HCMFunc.DataDecryptPES(s.target_max)),
                                //    howto = s.how_to + "",
                                //    actual = HCMFunc.DataDecryptPES(s.actual),


                                //}).ToList();
                            }


                            xrlblName.Text = _checkActiv.EmpFullName;
                            xrlblPosition.Text = _checkActiv.Rank;
                            xrlblPool.Text = _checkActiv.Pool;
                            xrlblGroup.Text = _checkActiv.UnitGroupName;
                            xrlblOtherRole.Text = _getData.other_roles + "";


                            //  xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();

                            DataTable dtApprover = SystemFunction.LinqToDataTable(lstApprover);
                            DataTable dtQuestions = SystemFunction.LinqToDataTable(lstQuestions);
                            DataTable dtKPIs = SystemFunction.LinqToDataTable(lstKPIs);


                            dsPESEvaluationForm ds = new dsPESEvaluationForm();
                            ds.dsApprover.Merge(dtApprover);
                            ds.dsQuestions.Merge(dtQuestions);
                            ds.dsKPIs.Merge(dtKPIs);


                            report.DataSource = ds;
                            report2.DataSource = ds;
                            report3.DataSource = ds;

                            report.CreateDocument();
                            report2.CreateDocument();
                            report3.CreateDocument();


                            PdfExportOptions options = new PdfExportOptions();
                            options.ShowPrintDialogOnOpen = true;

                            //Add page

                            xrTemp.Pages.AddRange(report.Pages);
                            xrTemp.Pages.AddRange(report2.Pages);
                            xrTemp.Pages.AddRange(report3.Pages);
                            xrTemp.ExportToPdf(stream);
                            return File(stream.GetBuffer(), "application/pdf", _checkActiv.EmpFullName + "_" + dNow.ToString("MMddyyyyHHmm") + ".pdf");

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

                }
                else
                {
                    return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                }

                //rpvPTREvaluation_Session objSession = Session[qryStr] as rpvPTREvaluation_Session;
                //if (objSession != null && objSession.lstData != null && objSession.lstData.Any())
                //{

                //}

                //else
                //{
                //    return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                //}
                //  return View();
            }
            else
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
            }
        }

        public ActionResult ExpEvaluationForm2019(string qryStr, string sMode = "pdf")
        {
            if (CGlobal.IsUserExpired())
            {
                return RedirectToAction("Login", "Login");
            }
            DateTime dNow = DateTime.Now;

            if (!string.IsNullOrEmpty(qryStr))
            {

                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                var _getData = _PTR_EvaluationService.FindReport(nId);
                bool checkAuthor = false;
                if (_getData != null)
                {
                    if (_getData.PTR_Evaluation_AuthorizedEva != null)
                    {
                        checkAuthor = _getData.PTR_Evaluation_AuthorizedEva.Any(a => a.active_status == "Y" && a.authorized_user == CGlobal.UserInfo.EmployeeNo);
                    }
                }

                if (_getData != null
                    && _getData.PTR_Evaluation_Approve != null
                    && (_getData.PTR_Evaluation_Approve.Any(a => a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G")
                    || CGlobal.UserIsAdminPES())
                    || checkAuthor
                    )
                {

                    var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                    if (_checkActiv != null)
                    {

                        try
                        {
                            XrTemp xrTemp = new XrTemp();
                            var stream = new MemoryStream();
                            xrTemp.CreateDocument();


                            rPESEvaluation19 report = new rPESEvaluation19();

                            rPESEvaQuestion19 report2 = new rPESEvaQuestion19();

                            rPESEvaKPIs19 report3 = new rPESEvaKPIs19();

                            rPESEvaIncidents19 report4 = new rPESEvaIncidents19();
                            rPESEvaIncidentScore19 report5 = new rPESEvaIncidentScore19();

                            XRLabel xrlblName = (XRLabel)report.FindControl("xrlblName", true);
                            XRLabel xrlblPosition = (XRLabel)report.FindControl("xrlblPosition", true);
                            XRLabel xrlblPool = (XRLabel)report.FindControl("xrlblPool", true);
                            XRLabel xrlblGroup = (XRLabel)report.FindControl("xrlblGroup", true);
                            XRLabel xrlblOtherRole = (XRLabel)report.FindControl("xrlblOtherRole", true);
                            XRLabel xrlblSelfRate = (XRLabel)report.FindControl("xrlblSelfRate", true);
                            XRLabel xrlblFinalRate = (XRLabel)report.FindControl("xrlblFinalRate", true);


                            if (_getData.PTR_Evaluation_Approve != null && _getData.PTR_Evaluation_Approve.Any(a => a.active_status == "Y"))
                            {
                                var _getRate = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self).Select(s => s.Annual_Rating != null ? s.Annual_Rating : null).FirstOrDefault();
                                if (_getRate != null)
                                {
                                    xrlblSelfRate.Text = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? _getRate.rating_name_en + "" : _getRate.rating_name_en;
                                }
                                var _getRateCEO = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Ceo).Select(s => s.Annual_Rating != null ? s.Annual_Rating : null).FirstOrDefault();
                                if (_getRateCEO != null)
                                {
                                    xrlblFinalRate.Text = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? _getRateCEO.rating_name_en + "" : _getRateCEO.rating_name_en;
                                }
                            }
                            List<vrpEvaluation> lstApprover = new List<vrpEvaluation>();
                            List<vrpEvaluation_Answer> lstQuestions = new List<vrpEvaluation_Answer>();
                            List<vPTREvaluation_KPIs> lstKPIs = new List<vPTREvaluation_KPIs>();

                            List<vPTREvaluation_Answer> lstIncidents = new List<vPTREvaluation_Answer>();
                            List<vPTREvaluation_Incidents_Score> lstIncidents_score = new List<vPTREvaluation_Incidents_Score>();

                            if (_getData.PTR_Evaluation_Approve != null && _getData.PTR_Evaluation_Approve.Any(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G"))
                            {
                                string[] empNO = _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G").Select(s => s.Req_Approve_user).ToArray();
                                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                lstApprover = (from lst in _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G").OrderBy(o => o.TM_PTR_Eva_ApproveStep.seq)
                                               from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                               select new vrpEvaluation
                                               {
                                                   sStepName = lst.TM_PTR_Eva_ApproveStep.Step_name_en,
                                                   sName = lstEmpReq.EmpFullName,
                                                   nOrder = lst.TM_PTR_Eva_ApproveStep.seq.HasValue ? lst.TM_PTR_Eva_ApproveStep.seq.Value : 0,
                                                   dApproved = lst.Approve_date,
                                                   sStatus = ((HCMFunc.DataDecryptPES(lst.responses + "") + "").Replace("<br/>", Environment.NewLine + "")).TexttoReportnewline() + "",
                                                   sRating = lst.Annual_Rating != null ? lst.Annual_Rating.rating_name_en : "",

                                               }).ToList();

                            }
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null)
                            {
                                lstQuestions = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions
                                                select new vrpEvaluation_Answer
                                                {
                                                    id = lstQ.Id + "",
                                                    question = lstQ.question,
                                                    header = lstQ.header,
                                                    nOrder = lstQ.seq,
                                                    remark = "",
                                                    sgroup = lstQ.qgroup + "",

                                                }).ToList();
                                if (_getData.PTR_Evaluation_Answer != null && _getData.PTR_Evaluation_Answer.Any(a => a.active_status == "Y"))
                                {
                                    lstQuestions.ForEach(ed =>
                                    {
                                        var GetAns = _getData.PTR_Evaluation_Answer.Where(w => w.TM_PTR_Eva_Questions.Id + "" == ed.id).FirstOrDefault();
                                        if (GetAns != null)
                                        {
                                            ed.remark = ((HCMFunc.DataDecryptPES(GetAns.answer + "") + "").Replace("<br/>", Environment.NewLine + "")).TexttoReportnewline() + "";
                                        }
                                    });
                                }
                            }

                            if (_getData.PTR_Evaluation_KPIs != null && _getData.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                            {
                                var _getDataYOne = _PTR_EvaluationService.FindFor3YearKPI(_getData.user_no, _getData.PTR_Evaluation_Year.evaluation_year.Value.AddYears(-1).Year);
                                var _getDataYTwo = _PTR_EvaluationService.FindFor3YearKPI(_getData.user_no, _getData.PTR_Evaluation_Year.evaluation_year.Value.AddYears(-2).Year);
                                var _getDataYThree = _PTR_EvaluationService.FindFor3YearKPI(_getData.user_no, _getData.PTR_Evaluation_Year.evaluation_year.Value.AddYears(-3).Year);
                                List<PTR_Evaluation_KPIs> lstKPI1 = new List<PTR_Evaluation_KPIs>();
                                List<PTR_Evaluation_KPIs> lstKPI2 = new List<PTR_Evaluation_KPIs>();
                                List<PTR_Evaluation_KPIs> lstKPI3 = new List<PTR_Evaluation_KPIs>();
                                if (_getDataYOne != null && _getDataYOne.PTR_Evaluation_KPIs != null && _getDataYOne.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                                {
                                    lstKPI1 = _getDataYOne.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").ToList();
                                }
                                if (_getDataYTwo != null && _getDataYTwo.PTR_Evaluation_KPIs != null && _getDataYTwo.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                                {
                                    lstKPI2 = _getDataYTwo.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").ToList();
                                }
                                if (_getDataYThree != null && _getDataYThree.PTR_Evaluation_KPIs != null && _getDataYThree.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                                {
                                    lstKPI3 = _getDataYThree.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").ToList();
                                }

                                lstKPIs = (from lst in _getData.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y")
                                           from lstKPIOne in lstKPI1.Where(w => w.TM_KPIs_Base.Id == lst.TM_KPIs_Base.Id).DefaultIfEmpty(new PTR_Evaluation_KPIs())
                                           from lstKPITwo in lstKPI2.Where(w => w.TM_KPIs_Base.Id == lst.TM_KPIs_Base.Id).DefaultIfEmpty(new PTR_Evaluation_KPIs())
                                           from lstKPIThree in lstKPI3.Where(w => w.TM_KPIs_Base.Id == lst.TM_KPIs_Base.Id).DefaultIfEmpty(new PTR_Evaluation_KPIs())
                                           select new vPTREvaluation_KPIs
                                           {
                                               sname = lst.TM_KPIs_Base.kpi_name_en,
                                               target_data = lst.TM_KPIs_Base.type_of_kpi == "P" ? SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target)).NodecimalFormatHasvalue() + "-" + SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)).NodecimalFormatHasvalue() + "%" : (lst.TM_KPIs_Base.type_of_kpi == "N" ? SystemFunction.GetNumberNullToZero(FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)))).NodecimalFormatHasvalue() : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)).NodecimalFormatHasvalue()),
                                               remark = "Howto : " + ((HCMFunc.DataDecryptPES(lst.how_to + "") + "").Replace("<br/>", Environment.NewLine + "")).TexttoReportnewline() + Environment.NewLine + "Remark : " + ((HCMFunc.DataDecryptPES(lst.final_remark + "") + "").Replace("<br/>", Environment.NewLine + "")).TexttoReportnewline() + "",
                                               actual = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.actual)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                               yearone = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.actual))) : (SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.actual)).NodecimalFormatHasvalue())) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIOne.actual),
                                               yeartwo = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.actual))) : (SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.actual)).NodecimalFormatHasvalue())) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPITwo.actual),
                                               yearthree = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIThree.actual),

                                               group_yearone = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.group_actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIOne.actual),
                                               group_yeartwo = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.group_actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPITwo.actual),
                                               group_yearthree = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.group_actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIThree.actual),

                                               sdisable = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? "N" : "Y",
                                               IdEncrypt = lst.Id + "",
                                               stype = lst.TM_KPIs_Base.type_of_kpi,
                                               target_group = lst.TM_KPIs_Base.type_of_kpi == "P" ? SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target)).NodecimalFormatHasvalue() + "-" + SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max)).NodecimalFormatHasvalue() + "%" : (lst.TM_KPIs_Base.type_of_kpi == "N" ? SystemFunction.GetNumberNullToZero(FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max)))).NodecimalFormatHasvalue() : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max)).NodecimalFormatHasvalue()),
                                               group_actual = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_actual)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                               howto = HCMFunc.DataDecryptPES(lst.how_to),
                                               sbu = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.sbu))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.sbu)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                               su = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.su))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.su)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                           }).ToList();



                                //_getData.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").Select(s => new vPTREvaluation_KPIs
                                //{
                                //    sname = s.TM_KPIs_Base.kpi_name_en,
                                //    target = s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : (s.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(s.target_max))) : HCMFunc.DataDecryptPES(s.target_max)),
                                //    howto = s.how_to + "",
                                //    actual = HCMFunc.DataDecryptPES(s.actual),


                                //}).ToList();
                            }
                            var _getApprovalforIncident = _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G" && a.TM_PTR_Eva_ApproveStep.Id != (int)StepApproveEvaluate.Self).ToList();
                            if (_getEvaQuestion.TM_PTR_Eva_Incidents != null && _getApprovalforIncident.Any())
                            {

                                List<vPTREvaluation_Incidents_Score> lstInScore = new List<vPTREvaluation_Incidents_Score>();
                                var GetRatescore = _PTR_Evaluation_Incidents_ScoreService.GetScored(_getApprovalforIncident.Select(s => s.Id).ToArray(), true).ToList();
                                foreach (var item in _getApprovalforIncident)
                                {
                                    lstInScore.AddRange(
                                        (from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents
                                         select new vPTREvaluation_Incidents_Score
                                         {
                                             isCurrent = "N",

                                             nstep = item.TM_PTR_Eva_ApproveStep != null ? item.TM_PTR_Eva_ApproveStep.seq : 0,
                                             id = lstQ.Id + "",
                                             question = lstQ.question,
                                             header = lstQ.seq + ". " + lstQ.header,
                                             nSeq = lstQ.seq,
                                             sgroup = item.TM_PTR_Eva_ApproveStep != null ? item.TM_PTR_Eva_ApproveStep.Step_name_en + "" : "",
                                             sExcellent = "N",
                                             sHigh = "N",
                                             sNI = "N",
                                             sLow = "N",
                                             sMeet = "N",
                                             nscore = lstQ.nscore + "%",
                                             nrate = GetRatescore.Where(w => w.active_status == "Y" && w.TM_PTR_Eva_Incidents_Id == lstQ.Id && w.PTR_Evaluation_Approve_Id == item.Id).Select(s => s.TM_PTR_Eva_Incidents_Score_Id).FirstOrDefault(),
                                             sdisable = ((_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate)) ? "N" : "Y",
                                             user = item.Req_Approve_user
                                         }).ToList()
                                        );


                                }

                                lstInScore.ForEach(ed =>
                                {
                                    if (ed.nrate == (int)PESClass.Incidents_Score.Excellent)
                                    {
                                        ed.sExcellent = "Y";
                                    }
                                    else if (ed.nrate == (int)PESClass.Incidents_Score.High)
                                    {
                                        ed.sHigh = "Y";
                                    }
                                    else if (ed.nrate == (int)PESClass.Incidents_Score.Low)
                                    {
                                        ed.sLow = "Y";
                                    }
                                    else if (ed.nrate == (int)PESClass.Incidents_Score.NI)
                                    {
                                        ed.sNI = "Y";
                                    }
                                    else if (ed.nrate == (int)PESClass.Incidents_Score.Meet)
                                    {
                                        ed.sMeet = "Y";
                                    }
                                });

                                lstIncidents_score = lstInScore.ToList();
                            }

                            if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Incidents != null)
                            {
                                if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019)
                                {
                                    lstIncidents = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents
                                                    select new vPTREvaluation_Answer
                                                    {
                                                        id = lstQ.Id + "",
                                                        question = lstQ.question,
                                                        header = lstQ.header,
                                                        nSeq = lstQ.seq,
                                                        remark = "",
                                                        sgroup = lstQ.qgroup + "",

                                                    }).ToList();


                                    if (_getData.PTR_Evaluation_Incidents != null && _getData.PTR_Evaluation_Incidents.Any(a => a.active_status == "Y"))
                                    {
                                        lstIncidents.ForEach(ed =>
                                         {
                                             var GetAns = _getData.PTR_Evaluation_Incidents.Where(w => w.TM_PTR_Eva_Incidents.Id + "" == ed.id).FirstOrDefault();
                                             if (GetAns != null)
                                             {
                                                 ed.remark = HCMFunc.DataDecryptPES(GetAns.answer + "").TexttoReportnewline();
                                             }
                                         });

                                    }
                                }
                            }

                            xrlblName.Text = _checkActiv.EmpFullName;
                            xrlblPosition.Text = _checkActiv.Rank;
                            xrlblPool.Text = _checkActiv.Pool;
                            xrlblGroup.Text = _checkActiv.UnitGroupName;
                            xrlblOtherRole.Text = _getData.other_roles + "";


                            //  xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();

                            DataTable dtApprover = SystemFunction.LinqToDataTable(lstApprover);
                            DataTable dtQuestions = SystemFunction.LinqToDataTable(lstQuestions);
                            DataTable dtKPIs = SystemFunction.LinqToDataTable(lstKPIs);

                            DataTable dsIncidents = SystemFunction.LinqToDataTable(lstIncidents);
                            DataTable dsIncidentsScore = SystemFunction.LinqToDataTable(lstIncidents_score);
                            dsPESEvaluationForm ds = new dsPESEvaluationForm();
                            ds.dsApprover.Merge(dtApprover);
                            ds.dsQuestions.Merge(dtQuestions);
                            ds.dsKPIs.Merge(dtKPIs);
                            ds.dsIncidents.Merge(dsIncidents);
                            ds.dsIncidents_score.Merge(dsIncidentsScore);


                            report.DataSource = ds;
                            report2.DataSource = ds;
                            report3.DataSource = ds;
                            report4.DataSource = ds;
                            //  report5.DataSource = ds;


                            report.CreateDocument();
                            report2.CreateDocument();
                            report3.CreateDocument();
                            report4.CreateDocument();
                            // report5.CreateDocument();
                            PdfExportOptions options = new PdfExportOptions();
                            options.ShowPrintDialogOnOpen = true;

                            //Add page

                            xrTemp.Pages.AddRange(report.Pages);
                            xrTemp.Pages.AddRange(report2.Pages);
                            xrTemp.Pages.AddRange(report3.Pages);
                            xrTemp.Pages.AddRange(report4.Pages);
                            // xrTemp.Pages.AddRange(report5.Pages);

                            if (sMode == "pdf")
                            {
                                xrTemp.ExportToPdf(stream);
                                return File(stream.GetBuffer(), "application/pdf", _checkActiv.EmpFullName + "_" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                            }
                            else
                            {

                                DocxExportOptions xlsxOptions = xrTemp.ExportOptions.Docx;

                                xlsxOptions.ExportMode = DocxExportMode.SingleFilePageByPage;

                                xrTemp.ExportToDocx(stream, xlsxOptions);
                                return File(stream.GetBuffer().ToArray(), "application/octet-stream", _checkActiv.EmpFullName + "_" + dNow.ToString("MMddyyyyHHmm") + ".docx");
                            }



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

                }
                else
                {
                    return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                }

                //rpvPTREvaluation_Session objSession = Session[qryStr] as rpvPTREvaluation_Session;
                //if (objSession != null && objSession.lstData != null && objSession.lstData.Any())
                //{

                //}

                //else
                //{
                //    return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                //}
                //  return View();
            }
            else
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
            }
        }

        public ActionResult ExpEvaluationForm2020(string qryStr, string sMode = "pdf")
        {
            if (CGlobal.IsUserExpired())
            {
                return RedirectToAction("Login", "Login");
            }
            DateTime dNow = DateTime.Now;

            if (!string.IsNullOrEmpty(qryStr))
            {

                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                var _getData = _PTR_EvaluationService.FindReport(nId);
                bool checkAuthor = false;
                if (_getData != null)
                {
                    if (_getData.PTR_Evaluation_AuthorizedEva != null)
                    {
                        checkAuthor = _getData.PTR_Evaluation_AuthorizedEva.Any(a => a.active_status == "Y" && a.authorized_user == CGlobal.UserInfo.EmployeeNo);
                    }
                }

                if (_getData != null
                    && _getData.PTR_Evaluation_Approve != null
                    && (_getData.PTR_Evaluation_Approve.Any(a => a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G")
                    || CGlobal.UserIsAdminPES())
                    || checkAuthor
                    )
                {

                    var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                    if (_checkActiv != null)
                    {

                        try
                        {
                            XrTemp xrTemp = new XrTemp();
                            var stream = new MemoryStream();
                            xrTemp.CreateDocument();


                            rPESEvaluation20 report = new rPESEvaluation20();

                            rPESEvaQuestion20 report2 = new rPESEvaQuestion20();

                            rPESEvaKPIs20 report3 = new rPESEvaKPIs20();

                            rPESEvaIncidents20 report4 = new rPESEvaIncidents20();
                            rPESEvaIncidentScore20 report5 = new rPESEvaIncidentScore20();

                            XRLabel xrlblName = (XRLabel)report.FindControl("xrlblName", true);
                            XRLabel xrlblPosition = (XRLabel)report.FindControl("xrlblPosition", true);
                            XRLabel xrlblPool = (XRLabel)report.FindControl("xrlblPool", true);
                            XRLabel xrlblGroup = (XRLabel)report.FindControl("xrlblGroup", true);
                            XRLabel xrlblOtherRole = (XRLabel)report.FindControl("xrlblOtherRole", true);
                            XRLabel xrlblSelfRate = (XRLabel)report.FindControl("xrlblSelfRate", true);
                            XRLabel xrlblFinalRate = (XRLabel)report.FindControl("xrlblFinalRate", true);


                            if (_getData.PTR_Evaluation_Approve != null && _getData.PTR_Evaluation_Approve.Any(a => a.active_status == "Y"))
                            {
                                var _getRate = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self).Select(s => s.Annual_Rating != null ? s.Annual_Rating : null).FirstOrDefault();
                                if (_getRate != null)
                                {
                                    xrlblSelfRate.Text = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? _getRate.rating_name_en + "" : _getRate.rating_name_en;
                                }
                                var _getRateCEO = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Ceo).Select(s => s.Annual_Rating != null ? s.Annual_Rating : null).FirstOrDefault();
                                if (_getRateCEO != null)
                                {
                                    xrlblFinalRate.Text = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? _getRateCEO.rating_name_en + "" : _getRateCEO.rating_name_en;
                                }
                            }
                            List<vrpEvaluation> lstApprover = new List<vrpEvaluation>();
                            List<vrpEvaluation_Answer> lstQuestions = new List<vrpEvaluation_Answer>();
                            List<vPTREvaluation_KPIs> lstKPIs = new List<vPTREvaluation_KPIs>();

                            List<vPTREvaluation_Answer> lstIncidents = new List<vPTREvaluation_Answer>();
                            List<vPTREvaluation_Incidents_Score> lstIncidents_score = new List<vPTREvaluation_Incidents_Score>();

                            if (_getData.PTR_Evaluation_Approve != null && _getData.PTR_Evaluation_Approve.Any(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G"))
                            {
                                string[] empNO = _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G").Select(s => s.Req_Approve_user).ToArray();
                                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                lstApprover = (from lst in _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G").OrderBy(o => o.TM_PTR_Eva_ApproveStep.seq)
                                               from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                               select new vrpEvaluation
                                               {
                                                   sStepName = lst.TM_PTR_Eva_ApproveStep.Step_name_en,
                                                   sName = lstEmpReq.EmpFullName,
                                                   nOrder = lst.TM_PTR_Eva_ApproveStep.seq.HasValue ? lst.TM_PTR_Eva_ApproveStep.seq.Value : 0,
                                                   dApproved = lst.Approve_date,
                                                   sStatus = ((HCMFunc.DataDecryptPES(lst.responses + "") + "").Replace("<br/>", Environment.NewLine + "")).TexttoReportnewline() + "",
                                                   sRating = lst.Annual_Rating != null ? lst.Annual_Rating.rating_name_en : "",

                                               }).ToList();

                            }
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null)
                            {
                                lstQuestions = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions
                                                select new vrpEvaluation_Answer
                                                {
                                                    id = lstQ.Id + "",
                                                    question = lstQ.question,
                                                    header = lstQ.header,
                                                    nOrder = lstQ.seq,
                                                    remark = "",
                                                    sgroup = lstQ.qgroup + "",

                                                }).ToList();
                                if (_getData.PTR_Evaluation_Answer != null && _getData.PTR_Evaluation_Answer.Any(a => a.active_status == "Y"))
                                {
                                    lstQuestions.ForEach(ed =>
                                    {
                                        var GetAns = _getData.PTR_Evaluation_Answer.Where(w => w.TM_PTR_Eva_Questions.Id + "" == ed.id).FirstOrDefault();
                                        if (GetAns != null)
                                        {
                                            ed.remark = ((HCMFunc.DataDecryptPES(GetAns.answer + "") + "").Replace("<br/>", Environment.NewLine + "")).TexttoReportnewline() + "";
                                        }
                                    });
                                }
                            }

                            if (_getData.PTR_Evaluation_KPIs != null && _getData.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                            {
                                var _getDataYOne = _PTR_EvaluationService.FindFor3YearKPI(_getData.user_no, _getData.PTR_Evaluation_Year.evaluation_year.Value.AddYears(-1).Year);
                                var _getDataYTwo = _PTR_EvaluationService.FindFor3YearKPI(_getData.user_no, _getData.PTR_Evaluation_Year.evaluation_year.Value.AddYears(-2).Year);
                                var _getDataYThree = _PTR_EvaluationService.FindFor3YearKPI(_getData.user_no, _getData.PTR_Evaluation_Year.evaluation_year.Value.AddYears(-3).Year);
                                List<PTR_Evaluation_KPIs> lstKPI1 = new List<PTR_Evaluation_KPIs>();
                                List<PTR_Evaluation_KPIs> lstKPI2 = new List<PTR_Evaluation_KPIs>();
                                List<PTR_Evaluation_KPIs> lstKPI3 = new List<PTR_Evaluation_KPIs>();
                                if (_getDataYOne != null && _getDataYOne.PTR_Evaluation_KPIs != null && _getDataYOne.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                                {
                                    lstKPI1 = _getDataYOne.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").ToList();
                                }
                                if (_getDataYTwo != null && _getDataYTwo.PTR_Evaluation_KPIs != null && _getDataYTwo.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                                {
                                    lstKPI2 = _getDataYTwo.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").ToList();
                                }
                                if (_getDataYThree != null && _getDataYThree.PTR_Evaluation_KPIs != null && _getDataYThree.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                                {
                                    lstKPI3 = _getDataYThree.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").ToList();
                                }

                                lstKPIs = (from lst in _getData.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y")
                                           from lstKPIOne in lstKPI1.Where(w => w.TM_KPIs_Base.Id == lst.TM_KPIs_Base.Id).DefaultIfEmpty(new PTR_Evaluation_KPIs())
                                           from lstKPITwo in lstKPI2.Where(w => w.TM_KPIs_Base.Id == lst.TM_KPIs_Base.Id).DefaultIfEmpty(new PTR_Evaluation_KPIs())
                                           from lstKPIThree in lstKPI3.Where(w => w.TM_KPIs_Base.Id == lst.TM_KPIs_Base.Id).DefaultIfEmpty(new PTR_Evaluation_KPIs())
                                           select new vPTREvaluation_KPIs
                                           {
                                               sname = lst.TM_KPIs_Base.kpi_name_en,
                                               target_data = lst.TM_KPIs_Base.type_of_kpi == "P" ? SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target)).NodecimalFormatHasvalue() + "-" + SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)).NodecimalFormatHasvalue() + "%" : (lst.TM_KPIs_Base.type_of_kpi == "N" ? SystemFunction.GetNumberNullToZero(FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)))).NodecimalFormatHasvalue() : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)).NodecimalFormatHasvalue()),
                                               remark = "Howto : " + ((HCMFunc.DataDecryptPES(lst.how_to + "") + "").Replace("<br/>", Environment.NewLine + "")).TexttoReportnewline() + Environment.NewLine + "Remark : " + ((HCMFunc.DataDecryptPES(lst.final_remark + "") + "").Replace("<br/>", Environment.NewLine + "")).TexttoReportnewline() + "",
                                               actual = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.actual)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                               yearone = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.actual))) : (SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.actual)).NodecimalFormatHasvalue())) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIOne.actual),
                                               yeartwo = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.actual))) : (SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.actual)).NodecimalFormatHasvalue())) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPITwo.actual),
                                               yearthree = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIThree.actual),

                                               group_yearone = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.group_actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIOne.actual),
                                               group_yeartwo = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.group_actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPITwo.actual),
                                               group_yearthree = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.group_actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIThree.actual),

                                               sdisable = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? "N" : "Y",
                                               IdEncrypt = lst.Id + "",
                                               stype = lst.TM_KPIs_Base.type_of_kpi,
                                               target_group = lst.TM_KPIs_Base.type_of_kpi == "P" ? SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target)).NodecimalFormatHasvalue() + "-" + SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max)).NodecimalFormatHasvalue() + "%" : (lst.TM_KPIs_Base.type_of_kpi == "N" ? SystemFunction.GetNumberNullToZero(FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max)))).NodecimalFormatHasvalue() : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max)).NodecimalFormatHasvalue()),
                                               group_actual = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_actual)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                               howto = HCMFunc.DataDecryptPES(lst.how_to),
                                               sbu = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.sbu))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.sbu)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                               su = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.su))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.su)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                           }).ToList();



                                //_getData.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").Select(s => new vPTREvaluation_KPIs
                                //{
                                //    sname = s.TM_KPIs_Base.kpi_name_en,
                                //    target = s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : (s.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(s.target_max))) : HCMFunc.DataDecryptPES(s.target_max)),
                                //    howto = s.how_to + "",
                                //    actual = HCMFunc.DataDecryptPES(s.actual),


                                //}).ToList();
                            }
                            var _getApprovalforIncident = _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G" && a.TM_PTR_Eva_ApproveStep.Id != (int)StepApproveEvaluate.Self).ToList();
                            if (_getEvaQuestion.TM_PTR_Eva_Incidents != null && _getApprovalforIncident.Any())
                            {

                                List<vPTREvaluation_Incidents_Score> lstInScore = new List<vPTREvaluation_Incidents_Score>();
                                var GetRatescore = _PTR_Evaluation_Incidents_ScoreService.GetScored(_getApprovalforIncident.Select(s => s.Id).ToArray(), true).ToList();
                                foreach (var item in _getApprovalforIncident)
                                {
                                    lstInScore.AddRange(
                                        (from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents
                                         select new vPTREvaluation_Incidents_Score
                                         {
                                             isCurrent = "N",

                                             nstep = item.TM_PTR_Eva_ApproveStep != null ? item.TM_PTR_Eva_ApproveStep.seq : 0,
                                             id = lstQ.Id + "",
                                             question = lstQ.question,
                                             header = lstQ.seq + ". " + lstQ.header,
                                             nSeq = lstQ.seq,
                                             sgroup = item.TM_PTR_Eva_ApproveStep != null ? item.TM_PTR_Eva_ApproveStep.Step_name_en + "" : "",
                                             sExcellent = "N",
                                             sHigh = "N",
                                             sNI = "N",
                                             sLow = "N",
                                             sMeet = "N",
                                             nscore = lstQ.nscore + "%",
                                             nrate = GetRatescore.Where(w => w.active_status == "Y" && w.TM_PTR_Eva_Incidents_Id == lstQ.Id && w.PTR_Evaluation_Approve_Id == item.Id).Select(s => s.TM_PTR_Eva_Incidents_Score_Id).FirstOrDefault(),
                                             sdisable = ((_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate)) ? "N" : "Y",
                                             user = item.Req_Approve_user
                                         }).ToList()
                                        );


                                }

                                lstInScore.ForEach(ed =>
                                {
                                    if (ed.nrate == (int)PESClass.Incidents_Score.Excellent)
                                    {
                                        ed.sExcellent = "Y";
                                    }
                                    else if (ed.nrate == (int)PESClass.Incidents_Score.High)
                                    {
                                        ed.sHigh = "Y";
                                    }
                                    else if (ed.nrate == (int)PESClass.Incidents_Score.Low)
                                    {
                                        ed.sLow = "Y";
                                    }
                                    else if (ed.nrate == (int)PESClass.Incidents_Score.NI)
                                    {
                                        ed.sNI = "Y";
                                    }
                                    else if (ed.nrate == (int)PESClass.Incidents_Score.Meet)
                                    {
                                        ed.sMeet = "Y";
                                    }
                                });

                                lstIncidents_score = lstInScore.ToList();
                            }

                            if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Incidents != null)
                            {
                                if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019 || _getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2020)
                                {
                                    lstIncidents = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents
                                                    select new vPTREvaluation_Answer
                                                    {
                                                        id = lstQ.Id + "",
                                                        question = lstQ.question,
                                                        header = lstQ.header,
                                                        nSeq = lstQ.seq,
                                                        remark = "",
                                                        sgroup = lstQ.qgroup + "",

                                                    }).ToList();


                                    if (_getData.PTR_Evaluation_Incidents != null && _getData.PTR_Evaluation_Incidents.Any(a => a.active_status == "Y"))
                                    {
                                        lstIncidents.ForEach(ed =>
                                        {
                                            var GetAns = _getData.PTR_Evaluation_Incidents.Where(w => w.TM_PTR_Eva_Incidents.Id + "" == ed.id).FirstOrDefault();
                                            if (GetAns != null)
                                            {
                                                ed.remark = HCMFunc.DataDecryptPES(GetAns.answer + "").TexttoReportnewline();
                                            }
                                        });

                                    }
                                }
                            }

                            xrlblName.Text = _checkActiv.EmpFullName;
                            xrlblPosition.Text = _checkActiv.Rank;
                            xrlblPool.Text = _checkActiv.Pool;
                            xrlblGroup.Text = _checkActiv.UnitGroupName;
                            xrlblOtherRole.Text = _getData.other_roles + "";


                            //  xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();

                            DataTable dtApprover = SystemFunction.LinqToDataTable(lstApprover);
                            DataTable dtQuestions = SystemFunction.LinqToDataTable(lstQuestions);
                            DataTable dtKPIs = SystemFunction.LinqToDataTable(lstKPIs);

                            DataTable dsIncidents = SystemFunction.LinqToDataTable(lstIncidents);
                            DataTable dsIncidentsScore = SystemFunction.LinqToDataTable(lstIncidents_score);
                            dsPESEvaluationForm ds = new dsPESEvaluationForm();
                            ds.dsApprover.Merge(dtApprover);
                            ds.dsQuestions.Merge(dtQuestions);
                            ds.dsKPIs.Merge(dtKPIs);
                            ds.dsIncidents.Merge(dsIncidents);
                            ds.dsIncidents_score.Merge(dsIncidentsScore);


                            report.DataSource = ds;
                            report2.DataSource = ds;
                            report3.DataSource = ds;
                            report4.DataSource = ds;
                            report5.DataSource = ds;


                            report.CreateDocument();
                            report2.CreateDocument();
                            report3.CreateDocument();
                            report4.CreateDocument();
                            report5.CreateDocument();
                            PdfExportOptions options = new PdfExportOptions();
                            options.ShowPrintDialogOnOpen = true;

                            //Add page

                            xrTemp.Pages.AddRange(report.Pages);
                            xrTemp.Pages.AddRange(report2.Pages);
                            xrTemp.Pages.AddRange(report3.Pages);
                            xrTemp.Pages.AddRange(report4.Pages);
                            xrTemp.Pages.AddRange(report5.Pages);

                            if (sMode == "pdf")
                            {
                                xrTemp.ExportToPdf(stream);
                                return File(stream.GetBuffer(), "application/pdf", _checkActiv.EmpFullName + "_" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                            }
                            else
                            {

                                DocxExportOptions xlsxOptions = xrTemp.ExportOptions.Docx;

                                xlsxOptions.ExportMode = DocxExportMode.SingleFilePageByPage;

                                xrTemp.ExportToDocx(stream, xlsxOptions);
                                return File(stream.GetBuffer().ToArray(), "application/octet-stream", _checkActiv.EmpFullName + "_" + dNow.ToString("MMddyyyyHHmm") + ".docx");
                            }



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

                }
                else
                {
                    return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                }

                //rpvPTREvaluation_Session objSession = Session[qryStr] as rpvPTREvaluation_Session;
                //if (objSession != null && objSession.lstData != null && objSession.lstData.Any())
                //{

                //}

                //else
                //{
                //    return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                //}
                //  return View();
            }
            else
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
            }
        }

        private static string FormatNumber(double num)
        {
            // Ensure number has max 3 significant digits (no rounding up can happen)
            double i = (double)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 3));
            num = num / i * i;

            if (num >= 1000000000)
                return (num / 1000000000D).ToString("0.##") + "";
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.##") + "";
            if (num >= 1000)
                return (num / 1000D).ToString("0.##") + "";

            return num.ToString("#,0");
        }
    }
}