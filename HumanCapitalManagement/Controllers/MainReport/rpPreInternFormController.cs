using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Report.DataSet.FormDataset;
using HumanCapitalManagement.Report.DevReport.Form;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.PreInternAssessment;
using HumanCapitalManagement.Service.TIFForm;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.MainVM;
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
    public class rpPreInternFormController : BaseController
    {
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;


        private TM_Candidate_MassTIFService _TM_Candidate_MassTIFService;
        private TM_Candidate_Status_CycleService _TM_Candidate_Status_CycleService;
        private TM_Candidate_TIFService _TM_Candidate_TIFService;
        private TM_TIF_RatingService _TM_TIF_RatingService;

        private TM_PIntern_FormService _TM_PIntern_FormService;
        private TM_PIntern_RatingService _TM_PIntern_RatingService;
        private TM_PIntern_StatusService _TM_PIntern_StatusService;
        private TM_PInternAssessment_ActivitiesService _TM_PInternAssessment_ActivitiesService;
        private TM_Candidate_PIntern_AnswerService _TM_Candidate_PIntern_AnswerService;
        private TM_Candidate_PIntern_ApprovService _TM_Candidate_PIntern_ApprovService;
        private TM_Candidate_PInternService _TM_Candidate_PInternService;
        private TM_Candidate_PIntern_MassService _TM_Candidate_PIntern_MassService;
        private TM_Candidate_PIntern_Mass_AnswerService _TM_Candidate_PIntern_Mass_AnswerService;
        private TM_Candidate_PIntern_Mass_ApprovService _TM_Candidate_PIntern_Mass_ApprovService;
        private TM_PIntern_RatingFormService _TM_PIntern_RatingFormService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public rpPreInternFormController(
         TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService


            , TM_Candidate_MassTIFService TM_Candidate_MassTIFService
             , TM_Candidate_Status_CycleService TM_Candidate_Status_CycleService
             , TM_TIF_RatingService TM_TIF_RatingService
            , TM_Candidate_TIFService TM_Candidate_TIFService
            , TM_PIntern_FormService TM_PIntern_FormService
          , TM_PIntern_RatingService TM_PIntern_RatingService
          , TM_PIntern_StatusService TM_PIntern_StatusService
          , TM_PInternAssessment_ActivitiesService TM_PInternAssessment_ActivitiesService
          , TM_Candidate_PIntern_AnswerService TM_Candidate_PIntern_AnswerService
          , TM_Candidate_PIntern_ApprovService TM_Candidate_PIntern_ApprovService
          , TM_Candidate_PInternService TM_Candidate_PInternService
            , TM_PIntern_RatingFormService TM_PIntern_RatingFormService
            , TM_Candidate_PIntern_Mass_AnswerService TM_Candidate_PIntern_Mass_AnswerService
          , TM_Candidate_PIntern_Mass_ApprovService TM_Candidate_PIntern_Mass_ApprovService
          , TM_Candidate_PIntern_MassService TM_Candidate_PIntern_MassService)

        {
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;

            _TM_Candidate_MassTIFService = TM_Candidate_MassTIFService;
            _TM_Candidate_Status_CycleService = TM_Candidate_Status_CycleService;
            _TM_TIF_RatingService = TM_TIF_RatingService;
            _TM_Candidate_TIFService = TM_Candidate_TIFService;
            //Pre Intern
            _TM_PIntern_FormService = TM_PIntern_FormService;
            _TM_PIntern_RatingService = TM_PIntern_RatingService;
            _TM_PIntern_StatusService = TM_PIntern_StatusService;
            _TM_PInternAssessment_ActivitiesService = TM_PInternAssessment_ActivitiesService;
            _TM_Candidate_PIntern_AnswerService = TM_Candidate_PIntern_AnswerService;
            _TM_Candidate_PIntern_ApprovService = TM_Candidate_PIntern_ApprovService;
            _TM_Candidate_PInternService = TM_Candidate_PInternService;
            _TM_PIntern_RatingFormService = TM_PIntern_RatingFormService;
            _TM_Candidate_PIntern_Mass_AnswerService = TM_Candidate_PIntern_Mass_AnswerService;
            _TM_Candidate_PIntern_Mass_ApprovService = TM_Candidate_PIntern_Mass_ApprovService;
            _TM_Candidate_PIntern_MassService = TM_Candidate_PIntern_MassService;
        }
        // GET: rpTIFForm
        public ActionResult Index()
        {
            return View();
        }
        #region export PDF
        public ActionResult ExpPreInternForm(string qryStr, string sMode = "pdf")
        {
            var sCheck = acCheckLoginAndPermissionExport("PreInternReportForm", "PreIntern");
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;


            if (qryStr != "")
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {


                    var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getData.PersonnelRequest.type_of_TIFForm + "" == "N")
                    {
                        var _getDataEva = _TM_Candidate_PInternService.FindByMappingID(nId);
                        if (_getDataEva != null && _getData != null)
                        {
                            _getDataEva.TM_PR_Candidate_Mapping = _getData;
                            try
                            {
                                List<string> lstUser = new List<string>();
                                if (_getDataEva.TM_Candidate_PIntern_Approv != null && _getDataEva.TM_Candidate_PIntern_Approv.Any())
                                {
                                    lstUser.AddRange(_getDataEva.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y").Select(s => s.Req_Approve_user).ToArray());
                                }
                                lstUser.Add(_getDataEva.acknowledge_user);
                                var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();
                                List<rpvtif_list_question> lstAns = new List<rpvtif_list_question>();
                                int nNO = 1;
                                //Dynamic Question and Rating
                                if (_getDataEva.TM_Candidate_PIntern_Answer != null && _getDataEva.TM_Candidate_PIntern_Answer.Any(a => a.active_status == "Y"))
                                {
                                    //Question and answer  
                                    lstAns = (from lstQ in _getDataEva.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y")
                                              select new rpvtif_list_question
                                              {
                                                  squestion = lstQ.TM_PIntern_Form_Question.question.Replace("<br/>", Environment.NewLine + ""),
                                                  sheader = lstQ.TM_PIntern_Form_Question.header.Replace("<br/>", Environment.NewLine + ""),
                                                  nSeq = lstQ.TM_PIntern_Form_Question.seq,
                                                  sremark = "  " + lstQ.answer.TexttoReportnewline() + "",
                                                  srating = lstQ.TM_PIntern_Rating != null ? lstQ.TM_PIntern_Rating.rating_name_en + "" : "",
                                                  stopic = lstQ.TM_PIntern_Form_Question.topic + "",
                                              }).ToList();
                                }
                                //Rating Citeria show 
                                var _getratings = new List<rpvtif_list_rating>();
                                var _getPIARating = _TM_PIntern_RatingFormService.GetActivePInternForm(_getDataEva.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);
                                _getratings = (from lstQ in _getPIARating.TM_PIntern_Rating
                                               orderby lstQ.seq
                                               select new rpvtif_list_rating
                                               {
                                                   sratingname = lstQ.rating_name_en + "",
                                                   sratingdes = lstQ.rating_description + "",
                                                   snSeq = lstQ.seq + "",
                                               }).ToList();
                                var stream = new MemoryStream();
                                XrTemp xrTemp = new XrTemp();
                                if (!string.IsNullOrEmpty(sMode) && sMode == "pdf")
                                {
                                    //link Var Report Design and Data
                                    #region varible Code link with varible Report Design
                                    xrTemp.CreateDocument();
                                    ReportPreInternForm report = new ReportPreInternForm();
                                    XRLabel xrFirstEvaName = (XRLabel)report.FindControl("xrFirstEvaName", true);
                                    XRLabel xrFirstEvaRank = (XRLabel)report.FindControl("xrFirstEvaRank", true);
                                    XRLabel xrFirstEvaDate = (XRLabel)report.FindControl("xrFirstEvaDate", true);
                                    XRLabel xrFirstEvaGroup = (XRLabel)report.FindControl("xrFirstEvaGroup", true);
                                    XRLabel xrSecEvaName = (XRLabel)report.FindControl("xrSecEvaName", true);
                                    XRLabel xrSecEvaRank = (XRLabel)report.FindControl("xrSecEvaRank", true);
                                    XRLabel xrSecEvaDate = (XRLabel)report.FindControl("xrSecEvaDate", true);
                                    XRLabel xrSecEvaGroup = (XRLabel)report.FindControl("xrSecEvaGroup", true);
                                    XRLabel xrCandidateName = (XRLabel)report.FindControl("xrCandidateName", true);
                                    XRLabel xrGroup = (XRLabel)report.FindControl("xrGroup", true);
                                    XRLabel xrSubGroup = (XRLabel)report.FindControl("xrSubGroup", true);
                                    XRLabel xrPosition = (XRLabel)report.FindControl("xrPosition", true);
                                    XRLabel xrActivity = (XRLabel)report.FindControl("xrActivity", true);
                                    XRLabel xrRecRank = (XRLabel)report.FindControl("xrRecRank", true);
                                    XRLabel xrResult = (XRLabel)report.FindControl("xrResult", true);
                                    XRTableCell xrComment = (XRTableCell)report.FindControl("xrComment", true);
                                    XRPictureBox xrPicCheck = (XRPictureBox)report.FindControl("xrPicCheck", true);
                                    XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);
                                    #endregion
                                    //Check Human submit
                                    if (_getDataEva.TM_Candidate_PIntern_Approv != null && _getDataEva.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y"))
                                    {
                                        var _getFirst = _getDataEva.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                        var _getSecond = _getDataEva.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
                                        if (_getFirst != null)
                                        {
                                            var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();
                                            if (_getEmpFirst != null)
                                            {
                                                xrFirstEvaName.Text = _getEmpFirst.EmpFullName + "";
                                                xrFirstEvaDate.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                xrFirstEvaRank.Text = _getEmpFirst.Rank;
                                                xrFirstEvaGroup.Text = _getEmpFirst.UnitGroupName;
                                            }
                                        }
                                        if (_getSecond != null)
                                        {
                                            var _getEmpSecond = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getSecond.Req_Approve_user).FirstOrDefault();
                                            if (_getEmpSecond != null)
                                            {
                                                xrSecEvaName.Text = _getEmpSecond.EmpFullName + "";
                                                xrSecEvaDate.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                                xrSecEvaRank.Text = _getEmpSecond.Rank;
                                                xrSecEvaGroup.Text = _getEmpSecond.UnitGroupName;
                                            }
                                        }
                                    }
                                    //Detail Candidate
                                    var _getRequest = _getData.PersonnelRequest;
                                    xrCandidateName.Text = "Candidate Name : " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                    xrPosition.Text = "Position : " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                    xrActivity.Text = "Activity : " + (_getRequest.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities != null ? _getRequest.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities.Activities_name_en + "" : "");
                                    xrRecRank.Text = "Recommended Rank (After interview) : " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                    xrResult.Text = "Pre-Intern Assessment Result : " + (_getDataEva.TM_PIntern_Status != null ? _getDataEva.TM_PIntern_Status.PIntern_short_name_en + "" : "");
                                    xrGroup.Text = "Group : " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                    xrSubGroup.Text = "Sub Group : " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_SubGroup != null ? _getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_SubGroup.sub_group_name_en + "" : "-");
                                    xrComment.Text = _getDataEva.comments.TexttoReportnewline();
                                    if (_getDataEva.confidentiality_agreement + "" == "Y")
                                    {
                                        xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                    }
                                    xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();

                                    //Send Data to dataset
                                    DataTable dtR = SystemFunction.LinqToDataTable(lstAns);
                                    DataTable dtRa = SystemFunction.LinqToDataTable(_getratings);
                                    dsReportPreInternForm ds = new dsReportPreInternForm();
                                    ds.dsMain.Merge(dtR);
                                    ds.dsRating.Merge(dtRa);
                                    report.DataSource = ds;
                                    report.CreateDocument();
                                    PdfExportOptions options = new PdfExportOptions();
                                    options.ShowPrintDialogOnOpen = true;

                                    //Add page
                                    xrTemp.Pages.AddRange(report.Pages);

                                    xrTemp.ExportToPdf(stream);
                                    return File(stream.GetBuffer(), "application/pdf", _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name() + "Pre-Intern Assessment Form" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                                }
                                else
                                {
                                    xrTemp.CreateDocument();
                                    DataTable dtR = SystemFunction.LinqToDataTable(lstAns);
                                    dsReportPreInternForm ds = new dsReportPreInternForm();
                                    ReportPreInternForm report = new ReportPreInternForm();
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
                                    return File(breport, "application/excel", _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name() + "Pre-Intern Assessment Form" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");
                                }
                            }
                            catch (Exception e)
                            {
                                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                                //throw;
                            }
                        }
                        else
                        {
                            return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                        }
                    }
                    else if (_getData.PersonnelRequest.type_of_TIFForm + "" == "M")
                    {
                        var _getDataEva = _TM_Candidate_PIntern_MassService.FindByMappingID(nId);
                        if (_getDataEva != null && _getData != null)
                        {
                            _getDataEva.TM_PR_Candidate_Mapping = _getData;
                            try
                            {
                                List<string> lstUser = new List<string>();
                                if (_getDataEva.TM_Candidate_PIntern_Mass_Approv != null && _getDataEva.TM_Candidate_PIntern_Mass_Approv.Any())
                                {
                                    lstUser.AddRange(_getDataEva.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y").Select(s => s.Req_Approve_user).ToArray());
                                }
                                lstUser.Add(_getDataEva.acknowledge_user);
                                var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();
                                List<rpvtif_list_question> lstAns = new List<rpvtif_list_question>();
                                int nNO = 1;
                                //Dynamic Question and Rating
                                if (_getDataEva.TM_Candidate_PIntern_Mass_Answer != null && _getDataEva.TM_Candidate_PIntern_Mass_Answer.Any(a => a.active_status == "Y"))
                                {
                                    //Question and answer  
                                    lstAns = (from lstQ in _getDataEva.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y")
                                              select new rpvtif_list_question
                                              {
                                                  squestion = lstQ.TM_PIntern_Mass_Question.question.Replace("<br/>", Environment.NewLine + ""),
                                                  sheader = lstQ.TM_PIntern_Mass_Question.header.Replace("<br/>", Environment.NewLine + ""),
                                                  nSeq = lstQ.TM_PIntern_Mass_Question.seq,
                                                  sremark = "  " + lstQ.answer.TexttoReportnewline() + "",
                                                  srating = lstQ.TM_PIntern_Rating != null ? lstQ.TM_PIntern_Rating.rating_name_en + "" : "",
                                                  stopic = lstQ.TM_PIntern_Mass_Question.topic + "",
                                              }).ToList();
                                }
                                //Rating Citeria show 
                                var _getratings = new List<rpvtif_list_rating>();
                                var _getPIARating = _TM_PIntern_RatingFormService.GetActivePInternForm(_getDataEva.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);
                                _getratings = (from lstQ in _getPIARating.TM_PIntern_Rating
                                               orderby lstQ.seq
                                               select new rpvtif_list_rating
                                               {
                                                   sratingname = lstQ.rating_name_en + "",
                                                   sratingdes = lstQ.rating_description + "",
                                                   snSeq = lstQ.seq + "",
                                               }).ToList();
                                var stream = new MemoryStream();
                                XrTemp xrTemp = new XrTemp();
                                if (!string.IsNullOrEmpty(sMode) && sMode == "pdf")
                                {
                                    //link Var Report Design and Data
                                    #region varible Code link with varible Report Design
                                    xrTemp.CreateDocument();
                                    ReportPreInternForm report = new ReportPreInternForm();
                                    XRLabel xrFirstEvaName = (XRLabel)report.FindControl("xrFirstEvaName", true);
                                    XRLabel xrFirstEvaRank = (XRLabel)report.FindControl("xrFirstEvaRank", true);
                                    XRLabel xrFirstEvaDate = (XRLabel)report.FindControl("xrFirstEvaDate", true);
                                    XRLabel xrFirstEvaGroup = (XRLabel)report.FindControl("xrFirstEvaGroup", true);
                                    XRLabel xrSecEvaName = (XRLabel)report.FindControl("xrSecEvaName", true);
                                    XRLabel xrSecEvaRank = (XRLabel)report.FindControl("xrSecEvaRank", true);
                                    XRLabel xrSecEvaDate = (XRLabel)report.FindControl("xrSecEvaDate", true);
                                    XRLabel xrSecEvaGroup = (XRLabel)report.FindControl("xrSecEvaGroup", true);
                                    XRLabel xrCandidateName = (XRLabel)report.FindControl("xrCandidateName", true);
                                    XRLabel xrGroup = (XRLabel)report.FindControl("xrGroup", true);
                                    XRLabel xrSubGroup = (XRLabel)report.FindControl("xrSubGroup", true);
                                    XRLabel xrPosition = (XRLabel)report.FindControl("xrPosition", true);
                                    XRLabel xrActivity = (XRLabel)report.FindControl("xrActivity", true);
                                    XRLabel xrRecRank = (XRLabel)report.FindControl("xrRecRank", true);
                                    XRLabel xrResult = (XRLabel)report.FindControl("xrResult", true);
                                    XRTableCell xrComment = (XRTableCell)report.FindControl("xrComment", true);
                                    XRPictureBox xrPicCheck = (XRPictureBox)report.FindControl("xrPicCheck", true);
                                    XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);
                                    #endregion
                                    //Check Human submit
                                    if (_getDataEva.TM_Candidate_PIntern_Mass_Approv != null && _getDataEva.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y"))
                                    {
                                        var _getFirst = _getDataEva.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                        var _getSecond = _getDataEva.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
                                        if (_getFirst != null)
                                        {
                                            var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();
                                            if (_getEmpFirst != null)
                                            {
                                                xrFirstEvaName.Text = _getEmpFirst.EmpFullName + "";
                                                xrFirstEvaDate.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                xrFirstEvaRank.Text = _getEmpFirst.Rank;
                                                xrFirstEvaGroup.Text = _getEmpFirst.UnitGroupName;
                                            }
                                        }
                                        if (_getSecond != null)
                                        {
                                            var _getEmpSecond = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getSecond.Req_Approve_user).FirstOrDefault();
                                            if (_getEmpSecond != null)
                                            {
                                                xrSecEvaName.Text = _getEmpSecond.EmpFullName + "";
                                                xrSecEvaDate.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                                xrSecEvaRank.Text = _getEmpSecond.Rank;
                                                xrSecEvaGroup.Text = _getEmpSecond.UnitGroupName;
                                            }
                                        }
                                    }
                                    //Detail Candidate
                                    var _getRequest = _getData.PersonnelRequest;
                                    xrCandidateName.Text = "Candidate Name : " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                    xrPosition.Text = "Position : " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                    xrActivity.Text = "Activity : " + (_getRequest.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities != null ? _getRequest.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities.Activities_name_en + "" : "");
                                    xrRecRank.Text = "Recommended Rank (After interview) : " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                    xrResult.Text = "Pre-Intern Assessment Result : " + (_getDataEva.TM_PIntern_Status != null ? _getDataEva.TM_PIntern_Status.PIntern_short_name_en + "" : "");
                                    xrGroup.Text = "Group : " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                    xrSubGroup.Text = "Sub Group : " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_SubGroup != null ? _getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_SubGroup.sub_group_name_en + "" : "-");
                                    xrComment.Text = _getDataEva.comments.TexttoReportnewline();
                                    if (_getDataEva.confidentiality_agreement + "" == "Y")
                                    {
                                        xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                    }
                                    xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();

                                    //Send Data to dataset
                                    DataTable dtR = SystemFunction.LinqToDataTable(lstAns);
                                    DataTable dtRa = SystemFunction.LinqToDataTable(_getratings);
                                    dsReportPreInternForm ds = new dsReportPreInternForm();
                                    ds.dsMain.Merge(dtR);
                                    ds.dsRating.Merge(dtRa);
                                    report.DataSource = ds;
                                    report.CreateDocument();
                                    PdfExportOptions options = new PdfExportOptions();
                                    options.ShowPrintDialogOnOpen = true;

                                    //Add page
                                    xrTemp.Pages.AddRange(report.Pages);

                                    xrTemp.ExportToPdf(stream);
                                    return File(stream.GetBuffer(), "application/pdf", _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name() + "Pre-Intern Assessment Form" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                                }
                                else
                                {
                                    xrTemp.CreateDocument();
                                    DataTable dtR = SystemFunction.LinqToDataTable(lstAns);
                                    dsReportPreInternForm ds = new dsReportPreInternForm();
                                    ReportPreInternForm report = new ReportPreInternForm();
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
                                    return File(breport, "application/excel", _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name() + "Pre-Intern Assessment Form" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");
                                }
                            }
                            catch (Exception e)
                            {
                                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                                //throw;
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

                    //  return View();
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

        public ActionResult ExpPreInternFormList(string qryStr, string session)
        {
            var sCheck = acCheckLoginAndPermissionExport("PreInternReportForm", "PreIntern");
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;

            if (qryStr != "" && !string.IsNullOrEmpty(session))
            {
                rpvTIFReport_Session objSession = Session[session] as rpvTIFReport_Session; //as List<ListAdvanceTransaction>;
                //List<ListAdvanceTransaction> lstData = Session[qryStr] as List<ListAdvanceTransaction>;
                if (objSession != null || objSession.lstDataPIntern != null || objSession.lstDataPIntern.Any()
                    || objSession.lstDataPInternMass != null || objSession.lstDataPInternMass.Any())
                {
                    try
                    {
                        string sMode = "pdf";
                        string[] aID = qryStr.Split(',');
                        aID = aID.Take(aID.Length - 1).ToArray();

                        if (aID != null && aID.Length > 0)
                        {

                            XrTemp xrTemp = new XrTemp();
                            var stream = new MemoryStream();
                            xrTemp.CreateDocument();
                            foreach (var xID in aID)
                            {
                                XrTemp xrTemp2 = new XrTemp();
                                xrTemp2.CreateDocument();
                                int nId = SystemFunction.GetIntNullToZero(xID);
                                var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                                //if (!string.IsNullOrEmpty(_getData.PersonnelRequest.type_of_TIFForm)&& _getData.PersonnelRequest.type_of_TIFForm + "" == "")
                                //{
                                //    //if nid is " "
                                //}
                                 if (!string.IsNullOrEmpty(_getData.PersonnelRequest.type_of_TIFForm) && _getData.PersonnelRequest.type_of_TIFForm + "" == "N")
                                {
                                    var _getReport = objSession.lstDataPIntern.Where(w => nId == w.TM_PR_Candidate_Mapping.Id/* && w.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm == "M"*/).FirstOrDefault();
                                    var _getDataEva = _TM_Candidate_PInternService.FindByMappingID(nId);
                                    if (_getReport != null && _getDataEva != null && _getData != null)
                                    {
                                        _getDataEva.TM_PR_Candidate_Mapping = _getData;
                                        try
                                        {
                                            int nNO = 1;
                                            if (!string.IsNullOrEmpty(sMode))
                                            {
                                                var _getRequest = _getData.PersonnelRequest;
                                                //link Var Report Design and Data
                                                #region varible Code link with varible Report Design
                                                ReportPreInternForm report = new ReportPreInternForm();
                                                XRLabel xrFirstEvaName = (XRLabel)report.FindControl("xrFirstEvaName", true);
                                                XRLabel xrFirstEvaRank = (XRLabel)report.FindControl("xrFirstEvaRank", true);
                                                XRLabel xrFirstEvaDate = (XRLabel)report.FindControl("xrFirstEvaDate", true);
                                                XRLabel xrFirstEvaGroup = (XRLabel)report.FindControl("xrFirstEvaGroup", true);
                                                XRLabel xrSecEvaName = (XRLabel)report.FindControl("xrSecEvaName", true);
                                                XRLabel xrSecEvaRank = (XRLabel)report.FindControl("xrSecEvaRank", true);
                                                XRLabel xrSecEvaDate = (XRLabel)report.FindControl("xrSecEvaDate", true);
                                                XRLabel xrSecEvaGroup = (XRLabel)report.FindControl("xrSecEvaGroup", true);
                                                XRLabel xrCandidateName = (XRLabel)report.FindControl("xrCandidateName", true);
                                                XRLabel xrGroup = (XRLabel)report.FindControl("xrGroup", true);
                                                XRLabel xrSubGroup = (XRLabel)report.FindControl("xrSubGroup", true);
                                                XRLabel xrPosition = (XRLabel)report.FindControl("xrPosition", true);
                                                XRLabel xrActivity = (XRLabel)report.FindControl("xrActivity", true);
                                                XRLabel xrRecRank = (XRLabel)report.FindControl("xrRecRank", true);
                                                XRLabel xrResult = (XRLabel)report.FindControl("xrResult", true);
                                                XRTableCell xrComment = (XRTableCell)report.FindControl("xrComment", true);
                                                XRPictureBox xrPicCheck = (XRPictureBox)report.FindControl("xrPicCheck", true);
                                                XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);
                                                #endregion
                                                if (_getDataEva.TM_Candidate_PIntern_Approv != null && _getDataEva.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y"))
                                                {
                                                    var _getFirst = _getDataEva.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                                    var _getSecond = _getDataEva.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
                                                    xrCandidateName.Text = "Candidate Name : " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                                    xrPosition.Text = "Position : " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                                    xrActivity.Text = "Activity : " + (_getRequest.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities != null ? _getRequest.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities.Activities_short_name_en + "" : "");
                                                    xrRecRank.Text = "Recommended Rank (After interview) : " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                                    xrResult.Text = "Pre-Intern Assessment Result : " + (_getDataEva.TM_PIntern_Status != null ? _getDataEva.TM_PIntern_Status.PIntern_short_name_en + "" : "");
                                                    xrGroup.Text = "Group : " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                                    xrSubGroup.Text = "Sub Group : " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_SubGroup != null ? _getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_SubGroup.sub_group_name_en + "" : "-");
                                                    xrComment.Text = _getDataEva.comments.TexttoReportnewline();
                                                    if (_getDataEva.confidentiality_agreement + "" == "Y")
                                                    {
                                                        xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                                    }
                                                    xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                                    //Check human submit
                                                    if (_getFirst != null)
                                                    {
                                                        var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();
                                                        if (_getEmpFirst != null)
                                                        {
                                                            xrFirstEvaName.Text = _getEmpFirst.EmpFullName + "";
                                                            xrFirstEvaDate.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                            xrFirstEvaRank.Text = _getEmpFirst.Rank;
                                                            xrFirstEvaGroup.Text = _getEmpFirst.UnitGroupName;
                                                        }
                                                    }
                                                    if (_getSecond != null)
                                                    {
                                                        var _getEmpSecond = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getSecond.Req_Approve_user).FirstOrDefault();
                                                        if (_getEmpSecond != null)
                                                        {
                                                            xrSecEvaName.Text = _getEmpSecond.EmpFullName + "";
                                                            xrSecEvaDate.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                                            xrSecEvaRank.Text = _getEmpSecond.Rank;
                                                            xrSecEvaGroup.Text = _getEmpSecond.UnitGroupName;
                                                        }

                                                    }
                                                }
                                                //Dynamic Question and Rating
                                                List<rpvtif_list_question> lstAns = new List<rpvtif_list_question>();
                                                if (_getDataEva.TM_Candidate_PIntern_Answer != null && _getDataEva.TM_Candidate_PIntern_Answer.Any(a => a.active_status == "Y"))
                                                {
                                                    var _CheckData = _TM_Candidate_PInternService.FindByMappingID(_getData.Id);

                                                    //Question and answer  
                                                    lstAns = (from lstQ in _getDataEva.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y")
                                                              select new rpvtif_list_question
                                                              {
                                                                  squestion = lstQ.TM_PIntern_Form_Question.question.Replace("<br/>", Environment.NewLine + ""),
                                                                  sheader = lstQ.TM_PIntern_Form_Question.header.Replace("<br/>", Environment.NewLine + ""),
                                                                  nSeq = lstQ.TM_PIntern_Form_Question.seq,
                                                                  sremark = "  " + lstQ.answer.TexttoReportnewline() + "",
                                                                  srating = lstQ.TM_PIntern_Rating != null ? lstQ.TM_PIntern_Rating.rating_name_en + "" : "",
                                                                  stopic = lstQ.TM_PIntern_Form_Question.topic + "",
                                                              }).ToList();

                                                }

                                                //Rating Citeria show 
                                                var _getratings = new List<rpvtif_list_rating>();
                                                var _getPIARating = _TM_PIntern_RatingFormService.GetActivePInternForm(_getDataEva.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);
                                                _getratings = (from lstQ in _getPIARating.TM_PIntern_Rating
                                                               orderby lstQ.seq
                                                               select new rpvtif_list_rating
                                                               {
                                                                   sratingname = lstQ.rating_name_en + "",
                                                                   sratingdes = lstQ.rating_description + "",
                                                                   snSeq = lstQ.seq + "",
                                                               }).ToList();


                                                //Send To dataset
                                                DataTable dtR = SystemFunction.LinqToDataTable(lstAns);
                                                DataTable dtRa = SystemFunction.LinqToDataTable(_getratings);
                                                dsReportPreInternForm ds = new dsReportPreInternForm();
                                                ds.dsMain.Merge(dtR);
                                                ds.dsRating.Merge(dtRa);
                                                report.DataSource = ds;
                                                report.CreateDocument();
                                                xrTemp.Pages.AddRange(report.Pages);

                                                //if (_getDataEva.confidentiality_agreement + "" == "Y")
                                                //{
                                                //    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                                //}
                                                //xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();

                                                //XlsxExportOptions xlsxOptions = xrTemp.ExportOptions.Xlsx;
                                                //xlsxOptions.ShowGridLines = true;
                                                //xlsxOptions.SheetName = "sheet_";
                                                //xlsxOptions.TextExportMode = TextExportMode.Text;
                                                //xlsxOptions.ExportMode = XlsxExportMode.SingleFilePageByPage;

                                                //xrTemp.ExportToXlsx(stream, xlsxOptions);
                                                //stream.Seek(0, SeekOrigin.Begin);

                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                                        }
                                        PdfExportOptions options = new PdfExportOptions();
                                        options.ShowPrintDialogOnOpen = true;
                                        xrTemp.PrintingSystem.ContinuousPageNumbering = false;
                                    }
                                }
                                else if (!string.IsNullOrEmpty(_getData.PersonnelRequest.type_of_TIFForm) && _getData.PersonnelRequest.type_of_TIFForm + "" == "M")
                                {
                                    var _getReportMass = objSession.lstDataPInternMass.Where(w => nId == w.TM_PR_Candidate_Mapping.Id/* && w.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm == "M"*/).FirstOrDefault();
                                    var _getDataEvaMass = _TM_Candidate_PIntern_MassService.FindByMappingID(nId);
                                    if (_getReportMass != null && _getDataEvaMass != null && _getData != null)
                                    {
                                        _getDataEvaMass.TM_PR_Candidate_Mapping = _getData;
                                        try
                                        {
                                            int nNO = 1;
                                            if (!string.IsNullOrEmpty(sMode))
                                            {
                                                var _getRequest = _getData.PersonnelRequest;
                                                //link Var Report Design and Data
                                                #region varible Code link with varible Report Design
                                                ReportPreInternForm report = new ReportPreInternForm();
                                                XRLabel xrFirstEvaName = (XRLabel)report.FindControl("xrFirstEvaName", true);
                                                XRLabel xrFirstEvaRank = (XRLabel)report.FindControl("xrFirstEvaRank", true);
                                                XRLabel xrFirstEvaDate = (XRLabel)report.FindControl("xrFirstEvaDate", true);
                                                XRLabel xrFirstEvaGroup = (XRLabel)report.FindControl("xrFirstEvaGroup", true);
                                                XRLabel xrSecEvaName = (XRLabel)report.FindControl("xrSecEvaName", true);
                                                XRLabel xrSecEvaRank = (XRLabel)report.FindControl("xrSecEvaRank", true);
                                                XRLabel xrSecEvaDate = (XRLabel)report.FindControl("xrSecEvaDate", true);
                                                XRLabel xrSecEvaGroup = (XRLabel)report.FindControl("xrSecEvaGroup", true);
                                                XRLabel xrCandidateName = (XRLabel)report.FindControl("xrCandidateName", true);
                                                XRLabel xrGroup = (XRLabel)report.FindControl("xrGroup", true);
                                                XRLabel xrSubGroup = (XRLabel)report.FindControl("xrSubGroup", true);
                                                XRLabel xrPosition = (XRLabel)report.FindControl("xrPosition", true);
                                                XRLabel xrActivity = (XRLabel)report.FindControl("xrActivity", true);
                                                XRLabel xrRecRank = (XRLabel)report.FindControl("xrRecRank", true);
                                                XRLabel xrResult = (XRLabel)report.FindControl("xrResult", true);
                                                XRTableCell xrComment = (XRTableCell)report.FindControl("xrComment", true);
                                                XRPictureBox xrPicCheck = (XRPictureBox)report.FindControl("xrPicCheck", true);
                                                XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);
                                                #endregion
                                                if (_getDataEvaMass.TM_Candidate_PIntern_Mass_Approv != null && _getDataEvaMass.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y"))
                                                {
                                                    var _getFirst = _getDataEvaMass.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                                    var _getSecond = _getDataEvaMass.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
                                                    xrCandidateName.Text = "Candidate Name : " + (_getDataEvaMass.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                                    xrPosition.Text = "Position : " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                                    xrActivity.Text = "Activity : " + (_getRequest.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities != null ? _getRequest.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities.Activities_short_name_en + "" : "");
                                                    xrRecRank.Text = "Recommended Rank (After interview) : " + (_getDataEvaMass.Recommended_Rank != null ? _getDataEvaMass.Recommended_Rank.Pool_rank_name_en + "" : "");
                                                    xrResult.Text = "Pre-Intern Assessment Result : " + (_getDataEvaMass.TM_PIntern_Status != null ? _getDataEvaMass.TM_PIntern_Status.PIntern_short_name_en + "" : "");
                                                    xrGroup.Text = "Group : " + (_getDataEvaMass.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                                    xrSubGroup.Text = "Sub Group : " + (_getDataEvaMass.TM_PR_Candidate_Mapping.PersonnelRequest.TM_SubGroup != null ? _getDataEvaMass.TM_PR_Candidate_Mapping.PersonnelRequest.TM_SubGroup.sub_group_name_en + "" : "-");
                                                    xrComment.Text = _getDataEvaMass.comments.TexttoReportnewline();
                                                    if (_getDataEvaMass.confidentiality_agreement + "" == "Y")
                                                    {
                                                        xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                                    }
                                                    xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                                    //Check human submit
                                                    if (_getFirst != null)
                                                    {
                                                        var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();
                                                        if (_getEmpFirst != null)
                                                        {
                                                            xrFirstEvaName.Text = _getEmpFirst.EmpFullName + "";
                                                            xrFirstEvaDate.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                            xrFirstEvaRank.Text = _getEmpFirst.Rank;
                                                            xrFirstEvaGroup.Text = _getEmpFirst.UnitGroupName;
                                                        }
                                                    }
                                                    if (_getSecond != null)
                                                    {
                                                        var _getEmpSecond = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getSecond.Req_Approve_user).FirstOrDefault();
                                                        if (_getEmpSecond != null)
                                                        {
                                                            xrSecEvaName.Text = _getEmpSecond.EmpFullName + "";
                                                            xrSecEvaDate.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                                            xrSecEvaRank.Text = _getEmpSecond.Rank;
                                                            xrSecEvaGroup.Text = _getEmpSecond.UnitGroupName;
                                                        }

                                                    }
                                                }
                                                //Dynamic Question and Rating
                                                List<rpvtif_list_question> lstAns = new List<rpvtif_list_question>();
                                                if (_getDataEvaMass.TM_Candidate_PIntern_Mass_Answer != null && _getDataEvaMass.TM_Candidate_PIntern_Mass_Answer.Any(a => a.active_status == "Y"))
                                                {
                                                    var _CheckData = _TM_Candidate_PIntern_MassService.FindByMappingID(_getData.Id);

                                                    //Question and answer  
                                                    lstAns = (from lstQ in _getDataEvaMass.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y")
                                                              select new rpvtif_list_question
                                                              {
                                                                  squestion = lstQ.TM_PIntern_Mass_Question.question.Replace("<br/>", Environment.NewLine + ""),
                                                                  sheader = lstQ.TM_PIntern_Mass_Question.header.Replace("<br/>", Environment.NewLine + ""),
                                                                  nSeq = lstQ.TM_PIntern_Mass_Question.seq,
                                                                  sremark = "  " + lstQ.answer.TexttoReportnewline() + "",
                                                                  srating = lstQ.TM_PIntern_Rating != null ? lstQ.TM_PIntern_Rating.rating_name_en + "" : "",
                                                                  stopic = lstQ.TM_PIntern_Mass_Question.topic + "",
                                                              }).ToList();

                                                }

                                                //Rating Citeria show 
                                                var _getratings = new List<rpvtif_list_rating>();
                                                var _getPIARating = _TM_PIntern_RatingFormService.GetActivePInternForm(_getDataEvaMass.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);
                                                _getratings = (from lstQ in _getPIARating.TM_PIntern_Rating
                                                               orderby lstQ.seq
                                                               select new rpvtif_list_rating
                                                               {
                                                                   sratingname = lstQ.rating_name_en + "",
                                                                   sratingdes = lstQ.rating_description + "",
                                                                   snSeq = lstQ.seq + "",
                                                               }).ToList();


                                                //Send To dataset
                                                DataTable dtR = SystemFunction.LinqToDataTable(lstAns);
                                                DataTable dtRa = SystemFunction.LinqToDataTable(_getratings);
                                                dsReportPreInternForm ds = new dsReportPreInternForm();
                                                ds.dsMain.Merge(dtR);
                                                ds.dsRating.Merge(dtRa);
                                                report.DataSource = ds;
                                                report.CreateDocument();
                                                xrTemp.Pages.AddRange(report.Pages);

                                                //if (_getDataEva.confidentiality_agreement + "" == "Y")
                                                //{
                                                //    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                                //}
                                                //xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();

                                                //XlsxExportOptions xlsxOptions = xrTemp.ExportOptions.Xlsx;
                                                //xlsxOptions.ShowGridLines = true;
                                                //xlsxOptions.SheetName = "sheet_";
                                                //xlsxOptions.TextExportMode = TextExportMode.Text;
                                                //xlsxOptions.ExportMode = XlsxExportMode.SingleFilePageByPage;

                                                //xrTemp.ExportToXlsx(stream, xlsxOptions);
                                                //stream.Seek(0, SeekOrigin.Begin);

                                            }

                                        }
                                        catch (Exception e)
                                        {
                                            return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                                        }
                                        PdfExportOptions options = new PdfExportOptions();
                                        options.ShowPrintDialogOnOpen = true;
                                        xrTemp.PrintingSystem.ContinuousPageNumbering = false;
                                    }
                                }
                                

                            }
                            xrTemp.ExportToPdf(stream);
                            return File(stream.GetBuffer(), "application/pdf", "Pre-Intern List" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                        }
                        else
                        {
                            return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session Error Length" });
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
                //  return View();
            }
            else
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
            }

        }
        public ActionResult ExpPreInternFormListExcel(string qryStr, string session)
        {
            var sCheck = acCheckLoginAndPermissionExport("PreInternReportFormList", "PreIntern");
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;

            if (qryStr != "" && !string.IsNullOrEmpty(session))
            {
                rpvTIFReport_Session objSession = Session[session] as rpvTIFReport_Session; //as List<ListAdvanceTransaction>;
                //List<ListAdvanceTransaction> lstData = Session[qryStr] as List<ListAdvanceTransaction>;
                if (objSession != null && objSession.lstDataPIntern != null && objSession.lstDataPIntern.Any())
                {
                    try
                    {
                        string[] aID = qryStr.Split(',');
                        aID = aID.Take(aID.Length - 1).ToArray();
                        if (aID != null && aID.Length > 0)
                        {
                            int[] nID = aID.Select(s => SystemFunction.GetIntNullToZero(s + "")).ToArray();
                            nID = nID.Where(w => w != 0).ToArray();
                            var _getReport = objSession.lstDataPIntern.Where(w => nID.Contains(w.TM_PR_Candidate_Mapping.Id)).ToList();
                            if (_getReport.Any())
                            {
                                try
                                {
                                    // var _getData = _TM_Candidate_MassTIFService.FindByMappingArrayID(_getReport.Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray());
                                    List<rpvPIAList_lst> lstReport = new List<rpvPIAList_lst>();
                                    string[] aAckHr = _getReport.Select(s => s.acknowledge_user).ToArray();
                                    var _getUser = dbHr.AllInfo_WS.Where(w => aAckHr.Contains(w.EmpNo)).ToList();
                                    int nNo = 1;
                                    lstReport = (from s in _getReport.AsEnumerable().OrderBy(o => o.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en)
                                                 from lstHR in _getUser.Where(w => w.EmpNo == s.acknowledge_user).DefaultIfEmpty(new AllInfo_WS())
                                                 select new rpvPIAList_lst
                                                 {
                                                     seq = nNo++,
                                                     no = s.Id,
                                                     fistname = s.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "",
                                                     lastname = s.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "",
                                                     email = s.TM_PR_Candidate_Mapping.TM_Candidates.candidate_Email + "",
                                                     id_card = s.TM_PR_Candidate_Mapping.TM_Candidates.id_card + "",
                                                     mobile = s.TM_PR_Candidate_Mapping.TM_Candidates.candidate_phone + "",
                                                     bu_result = s.TM_PIntern_Status != null ? s.TM_PIntern_Status.PIntern_short_name_en : "",
                                                     hr_acknow = lstHR != null ? lstHR.EmpFullName : "",
                                                     bu_comment = s.comments + "",
                                                     //start_date = s.can_start_date,
                                                     interview_date = s.create_date,
                                                     full_name = s.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name(),
                                                     hr_setinterview = _TM_Candidate_Status_CycleService.FindInterviewDate(s.TM_PR_Candidate_Mapping.Id),
                                                 }).ToList();
                                    lstReport.ForEach(ed =>
                                    {
                                        //var _getTif = _getReport.Where(w => w.Id == ed.no).FirstOrDefault();
                                        var _getData = _TM_Candidate_PInternService.Find(ed.no.Value);
                                        if (_getData != null && _getData.TM_Candidate_PIntern_Approv != null && _getData.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y"))
                                        {
                                            var _getFirst = _getData.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                            var _getSecond = _getData.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
                                            if (_getFirst != null)
                                            {
                                                var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();
                                                var _getmail = dbHr.AllInfo_WS.FirstOrDefault().Email;
                                                if (_getEmpFirst != null)
                                                {
                                                    ed.first_eva = _getEmpFirst.EmpFullName + "";
                                                    ed.mailfirst_eva = _getEmpFirst.Email;
                                                    // ed.first_eva_date = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                }

                                            }
                                            if (_getSecond != null)
                                            {
                                                var _getEmpSecond = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getSecond.Req_Approve_user).FirstOrDefault();
                                                if (_getEmpSecond != null)
                                                {
                                                    ed.second_eva = _getEmpSecond.EmpFullName + "";
                                                    ed.mailsecond_eva = _getEmpSecond.Email;
                                                    //ed.second_eva_date = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                                }

                                            }
                                            //Question and answer  
                                            if (_getData.TM_Candidate_PIntern_Answer != null && _getData.TM_Candidate_PIntern_Answer.Any(a => a.active_status == "Y"))
                                            {
                                                ed.Q_total = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y").Sum(s2 => s2.TM_PIntern_Rating.point);

                                                ed.Q1 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 1).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q2 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 2).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q3 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 3).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q4 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 4).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q5 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 5).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q6 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 6).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q7 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 7).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q8 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 8).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q9 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 9).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q10 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 10).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q11 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 11).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q12 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 12).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q13 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 13).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q14 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 14).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q15 = _getData.TM_Candidate_PIntern_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Form_Question.seq == 15).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.bu_result = _getData.TM_PIntern_Status.PIntern_short_name_en + "";
                                                ed.bu_comment = _getData.TM_Candidate_PIntern_Answer.FirstOrDefault().TM_Candidate_PIntern.comments;
                                            }
                                        }



                                    });




                                    XrTemp xrTemp = new XrTemp();
                                    var stream = new MemoryStream();
                                    xrTemp.CreateDocument();
                                    DataTable dtR = SystemFunction.LinqToDataTable(lstReport);
                                    dsReportPIAListData ds = new dsReportPIAListData();
                                    ds.dsMain.Merge(dtR);
                                    ReportExcelPIA report = new ReportExcelPIA();
                                    XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);
                                    xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
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
                                    return File(breport, "application/excel", "PIAListReport" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");

                                }
                                catch (Exception e)
                                {
                                    return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                                    throw;
                                }
                            }
                            else
                            {
                                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session ErrorData" });
                            }

                        }
                        else
                        {
                            return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session Error Length" });
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
                //  return View();
            }
            else
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
            }

        }

        public ActionResult ExpPreIntern_Mass_FormListExcel(string qryStr, string session)
        {
            var sCheck = acCheckLoginAndPermissionExport("PreInternReportFormList", "PreIntern");
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;

            if (qryStr != "" && !string.IsNullOrEmpty(session))
            {
                rpvTIFReport_Session objSession = Session[session] as rpvTIFReport_Session; //as List<ListAdvanceTransaction>;
                //List<ListAdvanceTransaction> lstData = Session[qryStr] as List<ListAdvanceTransaction>;
                if (objSession != null && objSession.lstDataPInternMass != null && objSession.lstDataPInternMass.Any())
                {
                    try
                    {
                        string[] aID = qryStr.Split(',');
                        //aID = aID.Take(aID.Length - 1).ToArray();
                        if (aID != null && aID.Length > 0)
                        {
                            int[] nID = aID.Select(s => SystemFunction.GetIntNullToZero(s + "")).ToArray();
                            nID = nID.Where(w => w != 0).ToArray();
                            var _getReport = objSession.lstDataPInternMass.Where(w => nID.Contains(w.TM_PR_Candidate_Mapping.Id)).ToList();
                            if (_getReport.Any())
                            {
                                try
                                {
                                    // var _getData = _TM_Candidate_MassTIFService.FindByMappingArrayID(_getReport.Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray());
                                    List<rpvPIAList_lst> lstReport = new List<rpvPIAList_lst>();
                                    string[] aAckHr = _getReport.Select(s => s.acknowledge_user).ToArray();
                                    var _getUser = dbHr.AllInfo_WS.Where(w => aAckHr.Contains(w.EmpNo)).ToList();
                                    int nNo = 1;
                                    lstReport = (from s in _getReport.AsEnumerable().OrderBy(o => o.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en)
                                                 from lstHR in _getUser.Where(w => w.EmpNo == s.acknowledge_user).DefaultIfEmpty(new AllInfo_WS())
                                                 select new rpvPIAList_lst
                                                 {
                                                     seq = nNo++,
                                                     no = s.Id,
                                                     fistname = s.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "",
                                                     lastname = s.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "",
                                                     email = s.TM_PR_Candidate_Mapping.TM_Candidates.candidate_Email + "",
                                                     id_card = s.TM_PR_Candidate_Mapping.TM_Candidates.id_card + "",
                                                     mobile = s.TM_PR_Candidate_Mapping.TM_Candidates.candidate_phone + "",
                                                     bu_result = s.TM_PIntern_Status != null ? s.TM_PIntern_Status.PIntern_short_name_en : "",
                                                     hr_acknow = lstHR != null ? lstHR.EmpFullName : "",
                                                     bu_comment = s.comments + "",
                                                     //start_date = s.can_start_date,
                                                     interview_date = s.create_date,
                                                     full_name = s.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name(),
                                                     hr_setinterview = _TM_Candidate_Status_CycleService.FindInterviewDate(s.TM_PR_Candidate_Mapping.Id),
                                                 }).ToList();
                                    lstReport.ForEach(ed =>
                                    {
                                        //var _getTif = _getReport.Where(w => w.Id == ed.no).FirstOrDefault();
                                        var _getData = _TM_Candidate_PIntern_MassService.Find(ed.no.Value);
                                        if (_getData != null && _getData.TM_Candidate_PIntern_Mass_Approv != null && _getData.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y"))
                                        {
                                            var _getFirst = _getData.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                            var _getSecond = _getData.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
                                            if (_getFirst != null)
                                            {
                                                var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();
                                                var _getmail = dbHr.AllInfo_WS.FirstOrDefault().Email;
                                                if (_getEmpFirst != null)
                                                {
                                                    ed.first_eva = _getEmpFirst.EmpFullName + "";
                                                    ed.mailfirst_eva = _getEmpFirst.Email;
                                                    // ed.first_eva_date = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                }

                                            }
                                            if (_getSecond != null)
                                            {
                                                var _getEmpSecond = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getSecond.Req_Approve_user).FirstOrDefault();
                                                if (_getEmpSecond != null)
                                                {
                                                    ed.second_eva = _getEmpSecond.EmpFullName + "";
                                                    ed.mailsecond_eva = _getEmpSecond.Email;
                                                    //ed.second_eva_date = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                                }

                                            }
                                            //Question and answer  
                                            if (_getData.TM_Candidate_PIntern_Mass_Answer != null && _getData.TM_Candidate_PIntern_Mass_Answer.Any(a => a.active_status == "Y"))
                                            {
                                                ed.Q_total = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y").Sum(s2 => s2.TM_PIntern_Rating.point);

                                                ed.Q1 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 1).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q2 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 2).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q3 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 3).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q4 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 4).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q5 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 5).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q6 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 6).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q7 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 7).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q8 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 8).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q9 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 9).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q10 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 10).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q12 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 11).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q14 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 12).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.Q15 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 13).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                //ed.Q14 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 14).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                //ed.Q15 = _getData.TM_Candidate_PIntern_Mass_Answer.Where(a => a.active_status == "Y" && a.TM_PIntern_Mass_Question.seq == 15).Select(s => (s.TM_PIntern_Rating != null ? s.TM_PIntern_Rating.rating_name_en : "")).FirstOrDefault();
                                                ed.bu_result = _getData.TM_PIntern_Status.PIntern_short_name_en + "";
                                                ed.bu_comment = _getData.TM_Candidate_PIntern_Mass_Answer.FirstOrDefault().TM_Candidate_PIntern_Mass.comments;
                                            }
                                        }



                                    });




                                    XrTemp xrTemp = new XrTemp();
                                    var stream = new MemoryStream();
                                    xrTemp.CreateDocument();
                                    DataTable dtR = SystemFunction.LinqToDataTable(lstReport);
                                    dsReportPIAListData ds = new dsReportPIAListData();
                                    ds.dsMain.Merge(dtR);
                                    ReportExcelPIA report = new ReportExcelPIA();
                                    XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);
                                    xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
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
                                    return File(breport, "application/excel", "PIAListReport" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");

                                }
                                catch (Exception e)
                                {
                                    return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                                    throw;
                                }
                            }
                            else
                            {
                                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session ErrorData" });
                            }

                        }
                        else
                        {
                            return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session Error Length" });
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
                //  return View();
            }
            else
            {
                return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
            }

        }
        #endregion
    }
}