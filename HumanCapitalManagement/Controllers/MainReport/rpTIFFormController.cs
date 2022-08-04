using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.TIFForm;
using HumanCapitalManagement.Report.DataSet.FormDataset;
using HumanCapitalManagement.Report.DevReport.Form;
using HumanCapitalManagement.Service.MainService;
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
    public class rpTIFFormController : BaseController
    {
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;


        private TM_Candidate_MassTIFService _TM_Candidate_MassTIFService;
        private TM_Candidate_Status_CycleService _TM_Candidate_Status_CycleService;
        private TM_Candidate_TIFService _TM_Candidate_TIFService;
        private TM_TIF_RatingService _TM_TIF_RatingService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public rpTIFFormController(
         TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , TM_Candidate_MassTIFService TM_Candidate_MassTIFService
             , TM_Candidate_Status_CycleService TM_Candidate_Status_CycleService
             , TM_TIF_RatingService TM_TIF_RatingService
            , TM_Candidate_TIFService TM_Candidate_TIFService)
        {
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;

            _TM_Candidate_MassTIFService = TM_Candidate_MassTIFService;
            _TM_Candidate_Status_CycleService = TM_Candidate_Status_CycleService;
            _TM_TIF_RatingService = TM_TIF_RatingService;
            _TM_Candidate_TIFService = TM_Candidate_TIFService;
        }
        // GET: rpTIFForm
        public ActionResult Index()
        {
            return View();
        }
        #region export PDF
        public ActionResult ExpTIFForm(string qryStr, string sMode = "pdf")
        {
            var sCheck = acCheckLoginAndPermissionExport("TIFFormReportView", "AcKnowledge");
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
                    var _getDataEva = _TM_Candidate_TIFService.FindByMappingID(nId);
                    var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getDataEva != null && _getData != null)
                    {
                        _getDataEva.TM_PR_Candidate_Mapping = _getData;
                        try
                        {
                            List<string> lstUser = new List<string>();
                            if (_getDataEva.TM_Candidate_TIF_Approv != null && _getDataEva.TM_Candidate_TIF_Approv.Any())
                            {
                                lstUser.AddRange(_getDataEva.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y").Select(s => s.Req_Approve_user).ToArray());
                            }

                            lstUser.Add(_getDataEva.acknowledge_user);
                            var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();
                            List<rpvtif_list_question> lstAns = new List<rpvtif_list_question>();

                            int nNO = 1;
                            //if (_getDataEva.TM_Trainee_Eva_Answer != null && _getDataEva.TM_Trainee_Eva_Answer.Any(a => a.active_status == "Y"))
                            //{
                            //    lstAns = _getDataEva.TM_Trainee_Eva_Answer.Where(w => w.active_status == "Y").Select(s => new rpvEva_list_question
                            //    {

                            //        seq = s.TM_Evaluation_Question.seq,
                            //        no = nNO++,
                            //        description = s.TM_Evaluation_Question.question + "",
                            //        sgroup = s.TM_Evaluation_Question.header,
                            //        comment = s.inchage_comment,
                            //        trainee_rating = s.trainee_rating != null ? s.trainee_rating.point + " : " + s.trainee_rating.rating_name_en + "" : "",
                            //        eva_ratine = s.inchage_rating != null ? s.inchage_rating.point + " : " + s.inchage_rating.rating_name_en + "" : "",
                            //    }).ToList();
                            //}
                            if (_getDataEva.TM_Candidate_TIF_Answer != null && _getDataEva.TM_Candidate_TIF_Answer.Any(a => a.active_status == "Y"))
                            {
                                lstAns = (from lstQ in _getDataEva.TM_Candidate_TIF_Answer.Where(a => a.active_status == "Y")
                                          select new rpvtif_list_question
                                          {
                                              squestion = lstQ.TM_TIF_Form_Question.question.Replace("<b>", "").Replace("</b>", "").Replace("<br>", " : ").Replace("<br/>", Environment.NewLine + ""),
                                              sheader = lstQ.TM_TIF_Form_Question.header.Replace("<br/>", Environment.NewLine + ""),
                                              nSeq = lstQ.TM_TIF_Form_Question.seq,
                                              sremark = lstQ.answer.TexttoReportnewline() + "",
                                              srating = lstQ.TM_TIF_Rating != null ? lstQ.TM_TIF_Rating.rating_name_en + "" : "",

                                          }).ToList();

                            }
                            //rpvtif_list_rating
                            var _getrating = _TM_TIF_RatingService.GetDataForSelectAll();

                            var _CheckDataF = _TM_Candidate_TIFService.FindByMappingID(_getData.Id);
                            var formId = _CheckDataF.TM_Candidate_TIF_Answer.Where(w => w.TM_TIF_Form_Question != null).FirstOrDefault();
                            var _CheckF = formId.TM_TIF_Form_Question.TM_TIF_Form.Id;

                            var _getratings = new List<rpvtif_list_rating>();

                            if (_CheckF == 1)
                            {
                                _getratings = _getrating.Where(w => w.active_status == "N").Select(s => new rpvtif_list_rating()

                                {
                                    sratingname = s.rating_name_en,
                                    sratingdes = s.rating_description
                                }).ToList();
                            }
                            else
                            {
                                _getratings = _getrating.Where(w => w.active_status == "Y").Select(s => new rpvtif_list_rating()

                                {
                                    sratingname = s.rating_name_en,
                                    sratingdes = s.rating_description
                                }).ToList();
                            }



                            var stream = new MemoryStream();
                            XrTemp xrTemp = new XrTemp();
                            if (!string.IsNullOrEmpty(sMode) && sMode == "pdf")
                            {

                                xrTemp.CreateDocument();
                                ReportTIFForm report = new ReportTIFForm();
                                //Evaluator
                                XRLabel xrFirstEvaName = (XRLabel)report.FindControl("xrFirstEvaName", true);
                                XRLabel xrFirstEvaRank = (XRLabel)report.FindControl("xrFirstEvaRank", true);
                                XRLabel xrFirstEvaDate = (XRLabel)report.FindControl("xrFirstEvaDate", true);
                                XRLabel xrFirstEvaGroup = (XRLabel)report.FindControl("xrFirstEvaGroup", true);
                                XRLabel xrSecEvaName = (XRLabel)report.FindControl("xrSecEvaName", true);
                                XRLabel xrSecEvaRank = (XRLabel)report.FindControl("xrSecEvaRank", true);
                                XRLabel xrSecEvaDate = (XRLabel)report.FindControl("xrSecEvaDate", true);
                                XRLabel xrSecEvaGroup = (XRLabel)report.FindControl("xrSecEvaGroup", true);
                                if (_getDataEva.TM_Candidate_TIF_Approv != null && _getDataEva.TM_Candidate_TIF_Approv.Any(a => a.active_status == "Y"))
                                {
                                    var _getFirst = _getDataEva.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                    var _getSecond = _getDataEva.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
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




                                XRLabel xrCandidateName = (XRLabel)report.FindControl("xrCandidateName", true);
                                XRLabel xrGroup = (XRLabel)report.FindControl("xrGroup", true);
                                XRLabel xrPosition = (XRLabel)report.FindControl("xrPosition", true);
                                XRLabel xrRecRank = (XRLabel)report.FindControl("xrRecRank", true);
                                XRLabel xrResult = (XRLabel)report.FindControl("xrResult", true);
                                XRTableCell xrComment = (XRTableCell)report.FindControl("xrComment", true);


                                XRPictureBox xrPicCheck = (XRPictureBox)report.FindControl("xrPicCheck", true);
                                XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);
                                var _getRequest = _getData.PersonnelRequest;
                                xrCandidateName.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                xrPosition.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                xrRecRank.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                xrResult.Text = "Interview Result: " + (_getDataEva.TM_TIF_Status != null ? _getDataEva.TM_TIF_Status.tif_status_name_en + "" : "");
                                xrGroup.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                xrComment.Text = _getDataEva.comments.TexttoReportnewline();
                                if (_getDataEva.confidentiality_agreement + "" == "Y")
                                {
                                    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                }
                                xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();


                                DataTable dtR = SystemFunction.LinqToDataTable(lstAns);
                                DataTable dtRa = SystemFunction.LinqToDataTable(_getratings);
                                dsReportTIFForm ds = new dsReportTIFForm();
                                ds.dsMain.Merge(dtR);
                                ds.dsRating.Merge(dtRa);



                                report.DataSource = ds;
                                report.CreateDocument();
                                PdfExportOptions options = new PdfExportOptions();
                                options.ShowPrintDialogOnOpen = true;

                                //Add page
                                xrTemp.Pages.AddRange(report.Pages);

                                xrTemp.ExportToPdf(stream);
                                return File(stream.GetBuffer(), "application/pdf", _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name() + "TIFForm" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                            }
                            else
                            {
                                xrTemp.CreateDocument();


                                DataTable dtR = SystemFunction.LinqToDataTable(lstAns);
                                dsReportTIFForm ds = new dsReportTIFForm();
                                //   ds.dsMain.Merge(dtR);
                                ReportTIFForm report = new ReportTIFForm();
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
                                return File(breport, "application/excel", _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name() + "TIFForm" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");
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

        public ActionResult ExpTIFFormList(string qryStr, string session)
        {
            var sCheck = acCheckLoginAndPermissionExport("TIFFormReportList", "AcKnowledge");
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;

            if (qryStr != "" && !string.IsNullOrEmpty(session))
            {
                rpvTIFReport_Session objSession = Session[session] as rpvTIFReport_Session; //as List<ListAdvanceTransaction>;
                //List<ListAdvanceTransaction> lstData = Session[qryStr] as List<ListAdvanceTransaction>;
                if (objSession != null && objSession.lstDataMass != null && objSession.lstDataMass.Any())
                {
                    try
                    {
                        string[] aID = qryStr.Split(',');

                        if (aID != null && aID.Length > 0)
                        {
                            int[] nID = aID.Select(s => SystemFunction.GetIntNullToZero(s + "")).ToArray();
                            nID = nID.Where(w => w != 0).ToArray();
                            var _getReport = objSession.lstDataMass.Where(w => nID.Contains(w.TM_PR_Candidate_Mapping.Id) && w.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm == "M").ToList();
                            if (_getReport.Any())
                            {
                                try
                                {
                                    // var _getData = _TM_Candidate_MassTIFService.FindByMappingArrayID(_getReport.Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray());
                                    List<rpvTIFList_lst> lstReport = new List<rpvTIFList_lst>();
                                    string[] aAckHr = _getReport.Select(s => s.acknowledge_user).ToArray();
                                    var _getUser = dbHr.AllInfo_WS.Where(w => aAckHr.Contains(w.EmpNo)).ToList();
                                    int nNo = 1;
                                    lstReport = (from s in _getReport.AsEnumerable().OrderBy(o => o.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en)
                                                 from lstHR in _getUser.Where(w => w.EmpNo == s.acknowledge_user).DefaultIfEmpty(new AllInfo_WS())
                                                 select new rpvTIFList_lst
                                                 {
                                                     seq = nNo++,
                                                     no = s.Id,
                                                     fistname = s.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "",
                                                     lastname = s.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "",
                                                     email = s.TM_PR_Candidate_Mapping.TM_Candidates.candidate_Email + "",
                                                     id_card = s.TM_PR_Candidate_Mapping.TM_Candidates.id_card + "",
                                                     mobile = s.TM_PR_Candidate_Mapping.TM_Candidates.candidate_phone + "",
                                                     bu_result = s.TM_MassTIF_Status != null ? s.TM_MassTIF_Status.masstif_status_name_en : "",
                                                     hr_acknow = lstHR != null ? lstHR.EmpFullName : "",
                                                     bu_comment = s.comments + "",
                                                     start_date = s.can_start_date,
                                                     interview_date = s.create_date,
                                                     full_name = s.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name(),
                                                     hr_setinterview = _TM_Candidate_Status_CycleService.FindInterviewDate(s.TM_PR_Candidate_Mapping.Id),
                                                     //  core_total = (s.TM_Candidate_MassTIF_Core != null && s.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y")) ? s.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y").Sum(s2 => s2.TM_Mass_Scoring.point) : 0,
                                                     //  audit_total = (s.TM_Candidate_MassTIF_Audit_Qst != null && s.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y")) ? s.TM_Candidate_MassTIF_Audit_Qst.Where(a => a.active_status == "Y").Sum(s2 => s2.TM_Mass_Scoring.point) : 0,

                                                 }).ToList();
                                    lstReport.ForEach(ed =>
                                    {
                                        //var _getTif = _getReport.Where(w => w.Id == ed.no).FirstOrDefault();
                                        var _getData = _TM_Candidate_MassTIFService.Find(ed.no.Value);
                                        if (_getData != null && _getData.TM_Candidate_MassTIF_Approv != null && _getData.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y"))
                                        {
                                            var _getFirst = _getData.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                            var _getSecond = _getData.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
                                            if (_getFirst != null)
                                            {
                                                var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();
                                                if (_getEmpFirst != null)
                                                {
                                                    ed.first_eva = _getEmpFirst.EmpFullName + "";
                                                    // ed.first_eva_date = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                }

                                            }
                                            if (_getSecond != null)
                                            {
                                                var _getEmpSecond = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getSecond.Req_Approve_user).FirstOrDefault();
                                                if (_getEmpSecond != null)
                                                {
                                                    ed.second_eva = _getEmpSecond.EmpFullName + "";
                                                    //ed.second_eva_date = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                                }

                                            }
                                            if (_getData.TM_Candidate_MassTIF_Core != null && _getData.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                            {
                                                ed.core_total = _getData.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y" && a.TM_Mass_Scoring != null).Sum(s2 => s2.TM_Mass_Scoring.point);
                                                ed.core1 = _getData.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y" && a.TM_MassTIF_Form_Question.seq == 1).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : s.TM_TIF_Rating.seq.ToString())).FirstOrDefault();
                                                ed.core2 = _getData.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y" && a.TM_MassTIF_Form_Question.seq == 2).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : s.TM_TIF_Rating.seq.ToString())).FirstOrDefault();
                                                ed.core3 = _getData.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y" && a.TM_MassTIF_Form_Question.seq == 3).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : s.TM_TIF_Rating.seq.ToString())).FirstOrDefault();
                                                ed.core4 = _getData.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y" && a.TM_MassTIF_Form_Question.seq == 4).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : s.TM_TIF_Rating.seq.ToString())).FirstOrDefault();
                                                ed.core5 = _getData.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y" && a.TM_MassTIF_Form_Question.seq == 5).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : s.TM_TIF_Rating.seq.ToString())).FirstOrDefault();
                                                ed.core6 = _getData.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y" && a.TM_MassTIF_Form_Question.seq == 6).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : s.TM_TIF_Rating.seq.ToString())).FirstOrDefault();
                                                ed.core7 = _getData.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y" && a.TM_MassTIF_Form_Question.seq == 7).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : s.TM_TIF_Rating.seq.ToString())).FirstOrDefault();
                                                ed.core8 = _getData.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y" && a.TM_MassTIF_Form_Question.seq == 8).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : s.TM_TIF_Rating.seq.ToString())).FirstOrDefault();
                                                ed.core9 = _getData.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y" && a.TM_MassTIF_Form_Question.seq == 9).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : s.TM_TIF_Rating.seq.ToString())).FirstOrDefault();

                                            }
                                            if (_getData.TM_Candidate_MassTIF_Audit_Qst != null && _getData.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                            {
                                                ed.audit1 = _getData.TM_Candidate_MassTIF_Audit_Qst.Where(a => a.active_status == "Y" && a.seq == 1).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : "")).FirstOrDefault();
                                                ed.audit2 = _getData.TM_Candidate_MassTIF_Audit_Qst.Where(a => a.active_status == "Y" && a.seq == 2).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : "")).FirstOrDefault();
                                                ed.audit3 = _getData.TM_Candidate_MassTIF_Audit_Qst.Where(a => a.active_status == "Y" && a.seq == 3).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : "")).FirstOrDefault();
                                                ed.audit4 = _getData.TM_Candidate_MassTIF_Audit_Qst.Where(a => a.active_status == "Y" && a.seq == 4).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : "")).FirstOrDefault();
                                                ed.audit5 = _getData.TM_Candidate_MassTIF_Audit_Qst.Where(a => a.active_status == "Y" && a.seq == 5).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.seq.ToString() : "")).FirstOrDefault();

                                                //add condition for cal result 28/03/2022
                                                //Easy  = 1,2 poin * 1
                                                //Moderate  = 3,4 poin * 2
                                                //Hard  = 5 poin * 3
                                                int? summarys = 0;
                                                for (int i = 1; i <= 5; i++)
                                                    summarys += _getData.TM_Candidate_MassTIF_Audit_Qst.Where(a => a.active_status == "Y" && a.seq == i).Select(s => (s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0)).FirstOrDefault() * (i == 5 ? 3 : i == 4 || i == 3 ? 2 : 1);

                                                ed.audit_total = summarys;
                                            }
                                            if (_getData.TM_Candidate_MassTIF_Additional != null && _getData.TM_Candidate_MassTIF_Additional.Any(a => a.active_status == "Y"))
                                            {
                                                ed.add_info7 = _getData.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y" && a.TM_Additional_Questions.seq == 7).Select(s => s.other_answer).FirstOrDefault();
                                                var _getAdinfo1 = _getData.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y" && a.TM_Additional_Questions.seq == 1).FirstOrDefault();
                                                var _getAdinfo2 = _getData.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y" && a.TM_Additional_Questions.seq == 2).FirstOrDefault();
                                                var _getAdinfo3 = _getData.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y" && a.TM_Additional_Questions.seq == 3).FirstOrDefault();
                                                var _getAdinfo4 = _getData.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y" && a.TM_Additional_Questions.seq == 4).FirstOrDefault();
                                                var _getAdinfo5 = _getData.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y" && a.TM_Additional_Questions.seq == 5).FirstOrDefault();
                                                var _getAdinfo6 = _getData.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y" && a.TM_Additional_Questions.seq == 6).FirstOrDefault();
                                                var _getAdinfo8 = _getData.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y" && a.TM_Additional_Questions.seq == 8).FirstOrDefault();
                                                var _getAdinfo9 = _getData.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y" && a.TM_Additional_Questions.seq == 9).FirstOrDefault();

                                                if (_getAdinfo1 != null)
                                                {
                                                    if (_getAdinfo1.TM_Candidate_MassTIF_Adnl_Answer != null && _getAdinfo1.TM_Candidate_MassTIF_Adnl_Answer.Any(a => a.active_status == "Y"))
                                                    {
                                                        foreach (var item in _getAdinfo1.TM_Candidate_MassTIF_Adnl_Answer.Where(a => a.active_status == "Y"))
                                                        {
                                                            ed.add_info1 += item.TM_Additional_Answers.answers + ",";
                                                        }

                                                    }
                                                    if (!string.IsNullOrEmpty(_getAdinfo1.other_answer))
                                                    {
                                                        ed.add_info1 += "(" + _getAdinfo1.other_answer + ")";
                                                    }
                                                }
                                                if (_getAdinfo2 != null)
                                                {
                                                    if (_getAdinfo2.TM_Candidate_MassTIF_Adnl_Answer != null && _getAdinfo2.TM_Candidate_MassTIF_Adnl_Answer.Any(a => a.active_status == "Y"))
                                                    {
                                                        foreach (var item in _getAdinfo2.TM_Candidate_MassTIF_Adnl_Answer.Where(a => a.active_status == "Y"))
                                                        {
                                                            ed.add_info2 += item.TM_Additional_Answers.answers + ",";
                                                        }

                                                    }
                                                    if (!string.IsNullOrEmpty(_getAdinfo2.other_answer))
                                                    {
                                                        ed.add_info2 += "(" + _getAdinfo2.other_answer + ")";
                                                    }
                                                }
                                                if (_getAdinfo3 != null)
                                                {
                                                    if (_getAdinfo3.TM_Candidate_MassTIF_Adnl_Answer != null && _getAdinfo3.TM_Candidate_MassTIF_Adnl_Answer.Any(a => a.active_status == "Y"))
                                                    {
                                                        foreach (var item in _getAdinfo3.TM_Candidate_MassTIF_Adnl_Answer.Where(a => a.active_status == "Y"))
                                                        {
                                                            ed.add_info3 += item.TM_Additional_Answers.answers + ",";
                                                        }

                                                    }
                                                    if (!string.IsNullOrEmpty(_getAdinfo3.other_answer))
                                                    {
                                                        ed.add_info3 += "(" + _getAdinfo3.other_answer + ")";
                                                    }
                                                }
                                                if (_getAdinfo4 != null)
                                                {
                                                    if (_getAdinfo4.TM_Candidate_MassTIF_Adnl_Answer != null && _getAdinfo4.TM_Candidate_MassTIF_Adnl_Answer.Any(a => a.active_status == "Y"))
                                                    {
                                                        foreach (var item in _getAdinfo4.TM_Candidate_MassTIF_Adnl_Answer.Where(a => a.active_status == "Y"))
                                                        {
                                                            ed.add_info4 += item.TM_Additional_Answers.answers + ",";
                                                        }

                                                    }
                                                    if (!string.IsNullOrEmpty(_getAdinfo4.other_answer))
                                                    {
                                                        ed.add_info4 += "(" + _getAdinfo4.other_answer + ")";
                                                    }
                                                }
                                                if (_getAdinfo5 != null)
                                                {
                                                    if (_getAdinfo5.TM_Candidate_MassTIF_Adnl_Answer != null && _getAdinfo5.TM_Candidate_MassTIF_Adnl_Answer.Any(a => a.active_status == "Y"))
                                                    {
                                                        foreach (var item in _getAdinfo5.TM_Candidate_MassTIF_Adnl_Answer.Where(a => a.active_status == "Y"))
                                                        {
                                                            ed.add_info5 += item.TM_Additional_Answers.answers + ",";
                                                        }

                                                    }
                                                    if (!string.IsNullOrEmpty(_getAdinfo5.other_answer))
                                                    {
                                                        ed.add_info5 += "(" + _getAdinfo5.other_answer + ")";
                                                    }
                                                }
                                                if (_getAdinfo6 != null)
                                                {
                                                    if (_getAdinfo6.TM_Candidate_MassTIF_Adnl_Answer != null && _getAdinfo6.TM_Candidate_MassTIF_Adnl_Answer.Any(a => a.active_status == "Y"))
                                                    {
                                                        foreach (var item in _getAdinfo6.TM_Candidate_MassTIF_Adnl_Answer.Where(a => a.active_status == "Y"))
                                                        {
                                                            ed.add_info6 += item.TM_Additional_Answers.answers + ",";
                                                        }

                                                    }
                                                    if (!string.IsNullOrEmpty(_getAdinfo6.other_answer))
                                                    {
                                                        ed.add_info6 += "(" + _getAdinfo6.other_answer + ")";
                                                    }
                                                }
                                                if (_getAdinfo8 != null)
                                                {
                                                    if (_getAdinfo8.TM_Candidate_MassTIF_Adnl_Answer != null && _getAdinfo8.TM_Candidate_MassTIF_Adnl_Answer.Any(a => a.active_status == "Y"))
                                                    {
                                                        foreach (var item in _getAdinfo8.TM_Candidate_MassTIF_Adnl_Answer.Where(a => a.active_status == "Y"))
                                                        {
                                                            ed.add_info8 += item.TM_Additional_Answers.answers + ",";
                                                        }

                                                    }
                                                    if (!string.IsNullOrEmpty(_getAdinfo8.other_answer))
                                                    {
                                                        ed.add_info8 += "(" + _getAdinfo8.other_answer + ")";
                                                    }
                                                }
                                                if (_getAdinfo9 != null)
                                                {
                                                    if (_getAdinfo9.TM_Candidate_MassTIF_Adnl_Answer != null && _getAdinfo9.TM_Candidate_MassTIF_Adnl_Answer.Any(a => a.active_status == "Y"))
                                                    {
                                                        foreach (var item in _getAdinfo9.TM_Candidate_MassTIF_Adnl_Answer.Where(a => a.active_status == "Y"))
                                                        {
                                                            ed.add_info9 += item.TM_Additional_Answers.answers + ",";
                                                        }

                                                    }
                                                    if (!string.IsNullOrEmpty(_getAdinfo9.other_answer))
                                                    {
                                                        ed.add_info9 += "(" + _getAdinfo9.other_answer + ")";
                                                    }
                                                }

                                            }
                                        }



                                    });




                                    XrTemp xrTemp = new XrTemp();
                                    var stream = new MemoryStream();
                                    xrTemp.CreateDocument();
                                    DataTable dtR = SystemFunction.LinqToDataTable(lstReport);
                                    dsReportTIFListData ds = new dsReportTIFListData();
                                    ds.dsMain.Merge(dtR);
                                    ReportTIFListData report = new ReportTIFListData();
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
                                    return File(breport, "application/excel", "TIFListReport" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");

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

        public ActionResult ExpMASSTIFForm(string qryStr, string sMode = "pdf")
        {
            var sCheck = acCheckLoginAndPermissionExport("TIFFormReportView", "AcKnowledge");
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
                    var _getDataEva = _TM_Candidate_MassTIFService.FindByMappingID(nId);
                    var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getDataEva != null && _getData != null)
                    {
                        _getDataEva.TM_PR_Candidate_Mapping = _getData;
                        try
                        {
                            List<string> lstUser = new List<string>();
                            if (_getDataEva.TM_Candidate_MassTIF_Approv != null && _getDataEva.TM_Candidate_MassTIF_Approv.Any())
                            {
                                lstUser.AddRange(_getDataEva.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y").Select(s => s.Req_Approve_user).ToArray());
                            }

                            lstUser.Add(_getDataEva.acknowledge_user);
                            var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();

                            int nNO = 1;

                            var stream = new MemoryStream();
                            XrTemp xrTemp = new XrTemp();
                            if (!string.IsNullOrEmpty(sMode) && sMode == "pdf")
                            {
                                xrTemp.CreateDocument();
                                var _getRequest = _getData.PersonnelRequest;
                                decimal? nCoreScore = 0, nAuditingScore = 0, nTotalScore = 0;
                                //Check Form Mass (New or Old)
                                var _CheckDataF = _TM_Candidate_MassTIFService.FindByMappingID(_getData.Id);
                                var _formId = _CheckDataF.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question != null).FirstOrDefault();
                                var _chkM = _formId.TM_MassTIF_Form_Question.TM_Mass_TIF_Form.Id;

                                //new
                                #region Report New Form Core
                                ReportMASSTIFFormNew reportnew = new ReportMASSTIFFormNew();
                                //Evaluator
                                XRLabel xrFirstEvaNamenew = (XRLabel)reportnew.FindControl("xrFirstEvaName", true);
                                XRLabel xrFirstEvaRanknew = (XRLabel)reportnew.FindControl("xrFirstEvaRank", true);
                                XRLabel xrFirstEvaDatenew = (XRLabel)reportnew.FindControl("xrFirstEvaDate", true);
                                XRLabel xrFirstEvaGroupnew = (XRLabel)reportnew.FindControl("xrFirstEvaGroup", true);
                                XRLabel xrSecEvaNamenew = (XRLabel)reportnew.FindControl("xrSecEvaName", true);
                                XRLabel xrSecEvaRanknew = (XRLabel)reportnew.FindControl("xrSecEvaRank", true);
                                XRLabel xrSecEvaDatenew = (XRLabel)reportnew.FindControl("xrSecEvaDate", true);
                                XRLabel xrSecEvaGroupnew = (XRLabel)reportnew.FindControl("xrSecEvaGroup", true);
                                XRLabel xrCandidateNamenew = (XRLabel)reportnew.FindControl("xrCandidateName", true);
                                XRLabel xrGroupnew = (XRLabel)reportnew.FindControl("xrGroup", true);
                                XRLabel xrPositionnew = (XRLabel)reportnew.FindControl("xrPosition", true);
                                XRLabel xrRecRanknew = (XRLabel)reportnew.FindControl("xrRecRank", true);
                                XRLabel xrResultnew = (XRLabel)reportnew.FindControl("xrResult", true);

                                //XRLabel xrCoreScorenew = (XRLabel)reportnew.FindControl("xrCoreScore", true);
                                XRLabel xrAuditingScorenew = (XRLabel)reportnew.FindControl("xrAuditingScore", true);
                                //XRLabel xrTotalScorenew = (XRLabel)reportnew.FindControl("xrTotalScore", true);
                                XRLabel xrlblPrintbtnew = (XRLabel)reportnew.FindControl("xrlblPrintbt", true);
                                xrCandidateNamenew.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                xrPositionnew.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                xrRecRanknew.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                xrResultnew.Text = "Interview Result: " + (_getDataEva.TM_MassTIF_Status != null ? _getDataEva.TM_MassTIF_Status.masstif_status_name_en + "" : "");
                                xrGroupnew.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                //xrCoreScorenew.Text = "Core Competency score: " + nCoreScore.NodecimalFormat();
                                xrAuditingScorenew.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                //xrTotalScorenew.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat();
                                //xrComment.Text = _getDataEva.comments;
                                //if (_getDataEva.confidentiality_agreement + "" == "Y")
                                //{
                                //    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                //}
                                xrlblPrintbtnew.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                //if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                //{
                                //    nCoreScore = _getDataEva.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => s.TM_TIF_Rating != null ? s.TM_TIF_Rating.point : 0).Sum();
                                //}
                                if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                {
                                    nAuditingScore = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                }
                                //xrCoreScorenew.Text = "Core Competency score: " + nCoreScore.NodecimalFormat() + "/36";
                                xrAuditingScorenew.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                //xrTotalScorenew.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat() + "/72";
                                #endregion
                                #region Report Page 2 New Form
                                ReportMASSCAuditingNew report2N = new ReportMASSCAuditingNew();
                                //Evaluator
                                XRLabel xrFirstEvaName2N = (XRLabel)report2N.FindControl("xrFirstEvaName", true);
                                XRLabel xrFirstEvaRank2N = (XRLabel)report2N.FindControl("xrFirstEvaRank", true);
                                XRLabel xrFirstEvaDate2N = (XRLabel)report2N.FindControl("xrFirstEvaDate", true);
                                XRLabel xrFirstEvaGroup2N = (XRLabel)report2N.FindControl("xrFirstEvaGroup", true);
                                XRLabel xrSecEvaName2N = (XRLabel)report2N.FindControl("xrSecEvaName", true);
                                XRLabel xrSecEvaRank2N = (XRLabel)report2N.FindControl("xrSecEvaRank", true);
                                XRLabel xrSecEvaDate2N = (XRLabel)report2N.FindControl("xrSecEvaDate", true);
                                XRLabel xrSecEvaGroup2N = (XRLabel)report2N.FindControl("xrSecEvaGroup", true);
                                XRLabel xrCandidateName2N = (XRLabel)report2N.FindControl("xrCandidateName", true);
                                XRLabel xrGroup2N = (XRLabel)report2N.FindControl("xrGroup", true);
                                XRLabel xrPosition2N = (XRLabel)report2N.FindControl("xrPosition", true);
                                XRLabel xrRecRank2N = (XRLabel)report2N.FindControl("xrRecRank", true);
                                XRLabel xrResult2N = (XRLabel)report2N.FindControl("xrResult", true);

                                //XRLabel xrCoreScore2N = (XRLabel)report2N.FindControl("xrCoreScore", true);
                                XRLabel xrAuditingScore2N = (XRLabel)report2N.FindControl("xrAuditingScore", true);
                                //XRLabel xrTotalScore2N = (XRLabel)report2N.FindControl("xrTotalScore", true);
                                XRLabel xrlblPrintbt2N = (XRLabel)report2N.FindControl("xrlblPrintbt", true);
                                xrCandidateName2N.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                xrPosition2N.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                xrRecRank2N.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                xrResult2N.Text = "Interview Result: " + (_getDataEva.TM_MassTIF_Status != null ? _getDataEva.TM_MassTIF_Status.masstif_status_name_en + "" : "");
                                xrGroup2N.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                //xrComment.Text = _getDataEva.comments;
                                //if (_getDataEva.confidentiality_agreement + "" == "Y")
                                //{
                                //    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                //}
                                xrlblPrintbt2N.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                //if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                //{
                                //    nCoreScore = _getDataEva.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                //}
                                if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                {
                                    nAuditingScore = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                }
                                //xrCoreScore2N.Text = "Core Competency score: " + nCoreScore.NodecimalFormat();
                                xrAuditingScore2N.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                //xrTotalScore2N.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat();
                                #endregion
                                #region Report Page 3 NEW Form
                                ReportMASSCADInfoNew report3N = new ReportMASSCADInfoNew();
                                //Evaluator
                                XRLabel xrFirstEvaName3N = (XRLabel)report3N.FindControl("xrFirstEvaName", true);
                                XRLabel xrFirstEvaRank3N = (XRLabel)report3N.FindControl("xrFirstEvaRank", true);
                                XRLabel xrFirstEvaDate3N = (XRLabel)report3N.FindControl("xrFirstEvaDate", true);
                                XRLabel xrFirstEvaGroup3N = (XRLabel)report3N.FindControl("xrFirstEvaGroup", true);
                                XRLabel xrSecEvaName3N = (XRLabel)report3N.FindControl("xrSecEvaName", true);
                                XRLabel xrSecEvaRank3N = (XRLabel)report3N.FindControl("xrSecEvaRank", true);
                                XRLabel xrSecEvaDate3N = (XRLabel)report3N.FindControl("xrSecEvaDate", true);
                                XRLabel xrSecEvaGroup3N = (XRLabel)report3N.FindControl("xrSecEvaGroup", true);
                                XRLabel xrCandidateName3N = (XRLabel)report3N.FindControl("xrCandidateName", true);
                                XRLabel xrGroup3N = (XRLabel)report3N.FindControl("xrGroup", true);
                                XRLabel xrPosition3N = (XRLabel)report3N.FindControl("xrPosition", true);
                                XRLabel xrRecRank3N = (XRLabel)report3N.FindControl("xrRecRank", true);
                                XRLabel xrResult3N = (XRLabel)report3N.FindControl("xrResult", true);

                                //XRLabel xrCoreScore3N = (XRLabel)report3N.FindControl("xrCoreScore", true);
                                XRLabel xrAuditingScore3N = (XRLabel)report3N.FindControl("xrAuditingScore", true);
                                //XRLabel xrTotalScore3N = (XRLabel)report3N.FindControl("xrTotalScore", true);

                                XRTableCell xrCommentN = (XRTableCell)report3N.FindControl("xrComment", true);
                                XRPictureBox xrPicCheckN = (XRPictureBox)report3N.FindControl("xrPicCheck", true);
                                XRLabel xrlblPrintbt3N = (XRLabel)report3N.FindControl("xrlblPrintbt", true);

                                xrCandidateName3N.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                xrPosition3N.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                xrRecRank3N.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                xrResult3N.Text = "Interview Result: " + (_getDataEva.TM_MassTIF_Status != null ? _getDataEva.TM_MassTIF_Status.masstif_status_name_en + "" : "");
                                xrGroup3N.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                xrCommentN.Text = _getDataEva.comments.TexttoReportnewline();
                                if (_getDataEva.confidentiality_agreement + "" == "Y")
                                {
                                    xrPicCheckN.ImageUrl = "~\\Image\\check.png";
                                }
                                xrlblPrintbt3N.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                //if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                //{
                                //    nCoreScore = _getDataEva.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                //}
                                if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                {
                                    nAuditingScore = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                }
                                //xrCoreScore3N.Text = "Core Competency score: " + nCoreScore.NodecimalFormat();
                                xrAuditingScore3N.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                //xrTotalScore3N.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat();
                                #endregion
                                //old
                                #region Report Page 1
                                ReportMASSTIFForm report = new ReportMASSTIFForm();

                                //Evaluator
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
                                XRLabel xrPosition = (XRLabel)report.FindControl("xrPosition", true);
                                XRLabel xrRecRank = (XRLabel)report.FindControl("xrRecRank", true);
                                XRLabel xrResult = (XRLabel)report.FindControl("xrResult", true);

                                XRLabel xrCoreScore = (XRLabel)report.FindControl("xrCoreScore", true);
                                XRLabel xrAuditingScore = (XRLabel)report.FindControl("xrAuditingScore", true);
                                XRLabel xrTotalScore = (XRLabel)report.FindControl("xrTotalScore", true);
                                XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);
                                xrCandidateName.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                xrPosition.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                xrRecRank.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                xrResult.Text = "Interview Result: " + (_getDataEva.TM_MassTIF_Status != null ? _getDataEva.TM_MassTIF_Status.masstif_status_name_en + "" : "");
                                xrGroup.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                //xrComment.Text = _getDataEva.comments;
                                //if (_getDataEva.confidentiality_agreement + "" == "Y")
                                //{
                                //    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                //}
                                xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                {
                                    nCoreScore = _getDataEva.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                }
                                if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                {
                                    nAuditingScore = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                }
                                xrCoreScore.Text = "Core Competency score: " + nCoreScore.NodecimalFormat();
                                xrAuditingScore.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                xrTotalScore.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat();
                                #endregion
                                #region Report Page 2
                                ReportMASSCAuditing report2 = new ReportMASSCAuditing();
                                //Evaluator
                                XRLabel xrFirstEvaName2 = (XRLabel)report2.FindControl("xrFirstEvaName", true);
                                XRLabel xrFirstEvaRank2 = (XRLabel)report2.FindControl("xrFirstEvaRank", true);
                                XRLabel xrFirstEvaDate2 = (XRLabel)report2.FindControl("xrFirstEvaDate", true);
                                XRLabel xrFirstEvaGroup2 = (XRLabel)report2.FindControl("xrFirstEvaGroup", true);
                                XRLabel xrSecEvaName2 = (XRLabel)report2.FindControl("xrSecEvaName", true);
                                XRLabel xrSecEvaRank2 = (XRLabel)report2.FindControl("xrSecEvaRank", true);
                                XRLabel xrSecEvaDate2 = (XRLabel)report2.FindControl("xrSecEvaDate", true);
                                XRLabel xrSecEvaGroup2 = (XRLabel)report2.FindControl("xrSecEvaGroup", true);
                                XRLabel xrCandidateName2 = (XRLabel)report2.FindControl("xrCandidateName", true);
                                XRLabel xrGroup2 = (XRLabel)report2.FindControl("xrGroup", true);
                                XRLabel xrPosition2 = (XRLabel)report2.FindControl("xrPosition", true);
                                XRLabel xrRecRank2 = (XRLabel)report2.FindControl("xrRecRank", true);
                                XRLabel xrResult2 = (XRLabel)report2.FindControl("xrResult", true);

                                XRLabel xrCoreScore2 = (XRLabel)report2.FindControl("xrCoreScore", true);
                                XRLabel xrAuditingScore2 = (XRLabel)report2.FindControl("xrAuditingScore", true);
                                XRLabel xrTotalScore2 = (XRLabel)report2.FindControl("xrTotalScore", true);
                                XRLabel xrlblPrintbt2 = (XRLabel)report2.FindControl("xrlblPrintbt", true);
                                xrCandidateName2.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                xrPosition2.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                xrRecRank2.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                xrResult2.Text = "Interview Result: " + (_getDataEva.TM_MassTIF_Status != null ? _getDataEva.TM_MassTIF_Status.masstif_status_name_en + "" : "");
                                xrGroup2.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                //xrComment.Text = _getDataEva.comments;
                                //if (_getDataEva.confidentiality_agreement + "" == "Y")
                                //{
                                //    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                //}
                                xrlblPrintbt2.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                {
                                    nCoreScore = _getDataEva.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                }
                                if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                {
                                    nAuditingScore = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                }
                                xrCoreScore2.Text = "Core Competency score: " + nCoreScore.NodecimalFormat();
                                xrAuditingScore2.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                xrTotalScore2.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat();
                                #endregion
                                #region Report Page 3
                                ReportMASSCADInfo report3 = new ReportMASSCADInfo();
                                //Evaluator
                                XRLabel xrFirstEvaName3 = (XRLabel)report3.FindControl("xrFirstEvaName", true);
                                XRLabel xrFirstEvaRank3 = (XRLabel)report3.FindControl("xrFirstEvaRank", true);
                                XRLabel xrFirstEvaDate3 = (XRLabel)report3.FindControl("xrFirstEvaDate", true);
                                XRLabel xrFirstEvaGroup3 = (XRLabel)report3.FindControl("xrFirstEvaGroup", true);
                                XRLabel xrSecEvaName3 = (XRLabel)report3.FindControl("xrSecEvaName", true);
                                XRLabel xrSecEvaRank3 = (XRLabel)report3.FindControl("xrSecEvaRank", true);
                                XRLabel xrSecEvaDate3 = (XRLabel)report3.FindControl("xrSecEvaDate", true);
                                XRLabel xrSecEvaGroup3 = (XRLabel)report3.FindControl("xrSecEvaGroup", true);
                                XRLabel xrCandidateName3 = (XRLabel)report3.FindControl("xrCandidateName", true);
                                XRLabel xrGroup3 = (XRLabel)report3.FindControl("xrGroup", true);
                                XRLabel xrPosition3 = (XRLabel)report3.FindControl("xrPosition", true);
                                XRLabel xrRecRank3 = (XRLabel)report3.FindControl("xrRecRank", true);
                                XRLabel xrResult3 = (XRLabel)report3.FindControl("xrResult", true);

                                XRLabel xrCoreScore3 = (XRLabel)report3.FindControl("xrCoreScore", true);
                                XRLabel xrAuditingScore3 = (XRLabel)report3.FindControl("xrAuditingScore", true);
                                XRLabel xrTotalScore3 = (XRLabel)report3.FindControl("xrTotalScore", true);

                                XRTableCell xrComment = (XRTableCell)report3.FindControl("xrComment", true);
                                XRPictureBox xrPicCheck = (XRPictureBox)report3.FindControl("xrPicCheck", true);
                                XRLabel xrlblPrintbt3 = (XRLabel)report3.FindControl("xrlblPrintbt", true);

                                xrCandidateName3.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                xrPosition3.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                xrRecRank3.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                xrResult3.Text = "Interview Result: " + (_getDataEva.TM_MassTIF_Status != null ? _getDataEva.TM_MassTIF_Status.masstif_status_name_en + "" : "");
                                xrGroup3.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                xrComment.Text = _getDataEva.comments.TexttoReportnewline();
                                if (_getDataEva.confidentiality_agreement + "" == "Y")
                                {
                                    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                }
                                xrlblPrintbt3.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                {
                                    nCoreScore = _getDataEva.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                }
                                if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                {
                                    nAuditingScore = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                }
                                xrCoreScore3.Text = "Core Competency score: " + nCoreScore.NodecimalFormat();
                                xrAuditingScore3.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                xrTotalScore3.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat();
                                #endregion


                                if (_getDataEva.TM_Candidate_MassTIF_Approv != null && _getDataEva.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y"))
                                {
                                    var _getFirst = _getDataEva.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                    var _getSecond = _getDataEva.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
                                    if (_getFirst != null)
                                    {
                                        var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();
                                        if (_getEmpFirst != null)
                                        {
                                            xrFirstEvaName.Text = _getEmpFirst.EmpFullName + "";
                                            xrFirstEvaDate.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                            xrFirstEvaRank.Text = _getEmpFirst.Rank;
                                            xrFirstEvaGroup.Text = _getEmpFirst.UnitGroupName;
                                            //Form New Page1
                                            xrFirstEvaNamenew.Text = _getEmpFirst.EmpFullName + "";
                                            xrFirstEvaRanknew.Text = _getEmpFirst.Rank;
                                            xrFirstEvaDatenew.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                            xrFirstEvaGroupnew.Text = _getEmpFirst.UnitGroupName;

                                            xrFirstEvaName2.Text = _getEmpFirst.EmpFullName + "";
                                            xrFirstEvaDate2.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                            xrFirstEvaRank2.Text = _getEmpFirst.Rank;
                                            xrFirstEvaGroup2.Text = _getEmpFirst.UnitGroupName;
                                            //Form New Page 2
                                            xrFirstEvaName2N.Text = _getEmpFirst.EmpFullName + "";
                                            xrFirstEvaDate2N.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                            xrFirstEvaRank2N.Text = _getEmpFirst.Rank;
                                            xrFirstEvaGroup2N.Text = _getEmpFirst.UnitGroupName;

                                            xrFirstEvaName3.Text = _getEmpFirst.EmpFullName + "";
                                            xrFirstEvaDate3.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                            xrFirstEvaRank3.Text = _getEmpFirst.Rank;
                                            xrFirstEvaGroup3.Text = _getEmpFirst.UnitGroupName;

                                            //Form New Page 3
                                            xrFirstEvaName3N.Text = _getEmpFirst.EmpFullName + "";
                                            xrFirstEvaDate3N.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                            xrFirstEvaRank3N.Text = _getEmpFirst.Rank;
                                            xrFirstEvaGroup3N.Text = _getEmpFirst.UnitGroupName;
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
                                            //Form New Page1
                                            xrSecEvaNamenew.Text = _getEmpSecond.EmpFullName + "";
                                            xrSecEvaDatenew.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                            xrSecEvaRanknew.Text = _getEmpSecond.Rank;
                                            xrSecEvaGroupnew.Text = _getEmpSecond.UnitGroupName;

                                            xrSecEvaName2.Text = _getEmpSecond.EmpFullName + "";
                                            xrSecEvaDate2.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                            xrSecEvaRank2.Text = _getEmpSecond.Rank;
                                            xrSecEvaGroup2.Text = _getEmpSecond.UnitGroupName;
                                            //Form New Page2
                                            xrSecEvaName2N.Text = _getEmpSecond.EmpFullName + "";
                                            xrSecEvaDate2N.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                            xrSecEvaRank2N.Text = _getEmpSecond.Rank;
                                            xrSecEvaGroup2N.Text = _getEmpSecond.UnitGroupName;

                                            xrSecEvaName3.Text = _getEmpSecond.EmpFullName + "";
                                            xrSecEvaDate3.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                            xrSecEvaRank3.Text = _getEmpSecond.Rank;
                                            xrSecEvaGroup3.Text = _getEmpSecond.UnitGroupName;

                                            //Form New Page 3
                                            xrSecEvaName3N.Text = _getEmpSecond.EmpFullName + "";
                                            xrSecEvaDate3N.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                            xrSecEvaRank3N.Text = _getEmpSecond.Rank;
                                            xrSecEvaGroup3N.Text = _getEmpSecond.UnitGroupName;
                                        }

                                    }
                                }

                                #region Core 
                                List<vdsCore> lstCore = new List<vdsCore>();

                                if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                {
                                    var _CheckData = _TM_Candidate_MassTIFService.FindByMappingID(_getData.Id);

                                    if (_chkM == 1)
                                    {
                                        lstCore = (from lstQ in _getDataEva.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y").OrderBy(o => o.TM_MassTIF_Form_Question.seq)
                                                   select new vdsCore
                                                   {
                                                       sheader = lstQ.TM_MassTIF_Form_Question.seq + ". " + lstQ.TM_MassTIF_Form_Question.header,
                                                       nSeq = lstQ.TM_MassTIF_Form_Question.seq,
                                                       sevidence = lstQ.evidence.TexttoReportnewline() + "",
                                                       sscoring = lstQ.TM_Mass_Scoring != null ? lstQ.TM_Mass_Scoring.scoring_code + "" : "",
                                                   }).ToList();
                                    }
                                    else
                                    {
                                        lstCore = _CheckData.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => new vdsCore
                                        {
                                            sheader = s.TM_MassTIF_Form_Question.header + "",
                                            question = s.TM_MassTIF_Form_Question.question.Replace("<b>", "").Replace("</b>", "").Replace("<br>", " : ") + "",
                                            sevidence = s.evidence.TexttoReportnewline() + "",
                                            srating = s.TM_TIF_Rating != null ? s.TM_TIF_Rating.rating_name_en + "" : "",
                                        }).ToList();
                                    }
                                }
                                #endregion

                                List<vMasstif_list_Auditing> lstAuditing = new List<vMasstif_list_Auditing>();
                                if (_chkM == 2)
                                {
                                    //New 
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Easy Question",
                                        nSeq = 1,
                                        id = 1 + "",
                                        answer = "",


                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Easy Question",
                                        nSeq = 2,
                                        id = 2 + "",
                                        answer = "",

                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Moderate Questions",
                                        nSeq = 3,
                                        id = 3 + "",
                                        answer = "",

                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Moderate Questions",
                                        nSeq = 4,
                                        id = 4 + "",
                                        answer = "",

                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Hard Questions",
                                        nSeq = 5,
                                        id = 5 + "",
                                        answer = "",

                                    });


                                }
                                else
                                {

                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Fixed Question",
                                        nSeq = 1,
                                        id = 1 + "",
                                        answer = "",

                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Fixed Question",
                                        nSeq = 2,
                                        id = 2 + "",
                                        answer = "",
                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Auditing Knowledge",
                                        nSeq = 3,
                                        id = 3 + "",
                                        answer = "",
                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Audit Procedures",
                                        nSeq = 4,
                                        id = 4 + "",
                                        answer = "",
                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Skepticism / Analytical",
                                        nSeq = 5,
                                        id = 5 + "",
                                        answer = "",
                                    });
                                }
                                if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                {

                                    if (_chkM == 2)
                                    {
                                        lstAuditing.ForEach(ed =>
                                        {
                                            var GetAns = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.seq + "" == ed.id).FirstOrDefault();
                                            if (GetAns != null)
                                            {
                                                ed.answer = GetAns.answer.TexttoReportnewline(50) + "";
                                                ed.scoring = GetAns.TM_Mass_Scoring != null ? GetAns.TM_Mass_Scoring.seq + "" : "";
                                                ed.Qst = GetAns.TM_Mass_Auditing_Question != null ? GetAns.TM_Mass_Auditing_Question.header.TexttoReportnewline() + "" : "";
                                            }
                                        });
                                    }
                                    else
                                    {
                                        lstAuditing.ForEach(ed =>
                                        {
                                            var GetAns = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.seq + "" == ed.id).FirstOrDefault();
                                            if (GetAns != null)
                                            {
                                                ed.answer = GetAns.answer.TexttoReportnewline(50) + "";
                                                ed.scoring = GetAns.TM_Mass_Scoring != null ? GetAns.TM_Mass_Scoring.scoring_code + "" : "";
                                                ed.Qst = GetAns.TM_Mass_Auditing_Question != null ? GetAns.TM_Mass_Auditing_Question.header.TexttoReportnewline() + "" : "";
                                            }
                                        });
                                    }
                                }
                                #region Additional

                                List<vdsAdInfo_Question> lstAdIn = new List<vdsAdInfo_Question>();

                                if (_getDataEva.TM_Candidate_MassTIF_Additional != null && _getDataEva.TM_Candidate_MassTIF_Additional.Any(a => a.active_status == "Y"))
                                {
                                    if (_chkM == 2)
                                    {
                                        lstAdIn = _getDataEva.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y").OrderBy(o => o.TM_Additional_Questions.seq).Select(s => new vdsAdInfo_Question
                                        {

                                            question = s.TM_Additional_Questions.header + "     " + s.TM_Additional_Questions.question + "",
                                            seq = s.TM_Additional_Questions.seq,
                                            other_answer = s.other_answer.TexttoReportnewline() + "",
                                            multi_answer = s.TM_Candidate_MassTIF_Adnl_Answer != null ? string.Join(Environment.NewLine + "", s.TM_Candidate_MassTIF_Adnl_Answer.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Additional_Answers.seq).Select((s2, index) => (index + 1) + ". " + s2.TM_Additional_Answers.answers).ToList()) : "",
                                            //lstAnswers = s.TM_Candidate_MassTIF_Adnl_Answer != null ? s.TM_Candidate_MassTIF_Adnl_Answer.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Additional_Answers.seq).Select(s2 => new vMassAdInfo_Answers
                                            //{
                                            //    nID = s2.Id + "",
                                            //    answer = s2.TM_Additional_Answers.answers,
                                            //    seq = s2.TM_Additional_Answers.seq,

                                            //}).ToList() : new List<vMassAdInfo_Answers>(),
                                        }).ToList();

                                    }
                                    else
                                    {
                                        lstAdIn = _getDataEva.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y").OrderBy(o => o.TM_Additional_Questions.seq).Select(s => new vdsAdInfo_Question
                                        {

                                            question = s.TM_Additional_Questions.question.Replace("<b>", "").Replace("</b>", "").Replace("<br>", " : ") + "",
                                            seq = s.TM_Additional_Questions.seq,
                                            other_answer = s.other_answer.TexttoReportnewline() + "",
                                            multi_answer = s.TM_Candidate_MassTIF_Adnl_Answer != null ? string.Join(Environment.NewLine + "", s.TM_Candidate_MassTIF_Adnl_Answer.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Additional_Answers.seq).Select((s2, index) => (index + 1) + ". " + s2.TM_Additional_Answers.answers).ToList()) : "",
                                            //lstAnswers = s.TM_Candidate_MassTIF_Adnl_Answer != null ? s.TM_Candidate_MassTIF_Adnl_Answer.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Additional_Answers.seq).Select(s2 => new vMassAdInfo_Answers
                                            //{
                                            //    nID = s2.Id + "",
                                            //    answer = s2.TM_Additional_Answers.answers,
                                            //    seq = s2.TM_Additional_Answers.seq,

                                            //}).ToList() : new List<vMassAdInfo_Answers>(),
                                        }).ToList();
                                    }



                                }
                                #endregion

                                DataTable dtR = SystemFunction.LinqToDataTable(lstCore);
                                DataTable dtAd = SystemFunction.LinqToDataTable(lstAuditing);
                                DataTable dtAdInfo = SystemFunction.LinqToDataTable(lstAdIn);

                                dsReportMASSTIFForm ds = new dsReportMASSTIFForm();

                                if (_chkM == 2)
                                {
                                    ds.dsCore.Merge(dtR);
                                    ds.dsAuditing.Merge(dtAd);
                                    ds.dsAdInfo.Merge(dtAdInfo);
                                    //Get Data report New
                                    reportnew.DataSource = ds;
                                    report2N.DataSource = ds;
                                    report3N.DataSource = ds;


                                    //create report New
                                    reportnew.CreateDocument();
                                    report2N.CreateDocument();
                                    report3N.CreateDocument();
                                    //Add page
                                    xrTemp.Pages.AddRange(reportnew.Pages);
                                    xrTemp.Pages.AddRange(report2N.Pages);
                                    xrTemp.Pages.AddRange(report3N.Pages);

                                }
                                else if (_chkM == 1)
                                {
                                    ds.dsCore.Merge(dtR);
                                    ds.dsAuditing.Merge(dtAd);
                                    ds.dsAdInfo.Merge(dtAdInfo);
                                    //Get Data report old
                                    report.DataSource = ds;
                                    report2.DataSource = ds;
                                    report3.DataSource = ds;

                                    //Create Data Report old
                                    report.CreateDocument();
                                    report2.CreateDocument();
                                    report3.CreateDocument();
                                    //Add page
                                    xrTemp.Pages.AddRange(report.Pages);
                                    xrTemp.Pages.AddRange(report2.Pages);
                                    xrTemp.Pages.AddRange(report3.Pages);

                                }
                                else
                                {
                                    ds.dsCore.Merge(dtR);
                                    ds.dsAuditing.Merge(dtAd);
                                    ds.dsAdInfo.Merge(dtAdInfo);
                                    //Get Data report New
                                    reportnew.DataSource = ds;
                                    report2N.DataSource = ds;
                                    report3N.DataSource = ds;


                                    //create report New
                                    reportnew.CreateDocument();
                                    report2N.CreateDocument();
                                    report3N.CreateDocument();
                                    //Add page
                                    xrTemp.Pages.AddRange(reportnew.Pages);
                                    xrTemp.Pages.AddRange(report2N.Pages);
                                    xrTemp.Pages.AddRange(report3N.Pages);
                                }
                                xrTemp.ExportToPdf(stream);
                                PdfExportOptions options = new PdfExportOptions();
                                options.ShowPrintDialogOnOpen = true;


                                return File(stream.GetBuffer(), "application/pdf", _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name() + "MassForm" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                            }
                            else
                            {
                                xrTemp.CreateDocument();




                                //   ds.dsMain.Merge(dtR);
                                ReportTIFForm report = new ReportTIFForm();
                                //XRLabel xrLabel1 = (XRLabel)report.FindControl("xrLabel1", true);
                                //xrLabel1.Text = dNow.DateTimeWithTimebyCulture() + " By " + CGlobal.UserInfo.FullName;

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
                                return File(breport, "application/excel", _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name() + "TIFForm" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");
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
        public ActionResult ExpMASSTIFFormList(string qryStr, string session)
        {
            var sCheck = acCheckLoginAndPermissionExport("TIFFormReportView", "AcKnowledge");
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            string sMode2 = "pdf";

            if (qryStr != "" && !string.IsNullOrEmpty(session))
            {
                rpvTIFReport_Session objSession = Session[session] as rpvTIFReport_Session; //as List<ListAdvanceTransaction>;
                if (objSession != null && objSession.lstDataMass != null && objSession.lstDataMass.Any())
                {
                    try
                    {
                        string[] aID = qryStr.Split(',');
                        if (aID != null && aID.Length > 0)
                        {
                            XrTemp xrTemp = new XrTemp();
                            var stream = new MemoryStream();
                            xrTemp.CreateDocument();
                            foreach (var xID in aID)
                            {
                                XrTemp xrTemp2 = new XrTemp();
                                xrTemp2.CreateDocument();
                                int nId = SystemFunction.GetIntNullToZero(xID + "");
                                var _getReport = objSession.lstDataMass.Where(w => nId == w.TM_PR_Candidate_Mapping.Id && w.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm == "M").FirstOrDefault();
                                var _getDataEva = _TM_Candidate_MassTIFService.FindByMappingID(nId);
                                var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                                if (_getReport != null && _getDataEva != null && _getData != null)
                                {
                                    _getDataEva.TM_PR_Candidate_Mapping = _getData;
                                    try
                                    {
                                        List<string> lstUser = new List<string>();
                                        if (_getDataEva.TM_Candidate_MassTIF_Approv != null && _getDataEva.TM_Candidate_MassTIF_Approv.Any())
                                        {
                                            lstUser.AddRange(_getDataEva.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y").Select(s => s.Req_Approve_user).ToArray());
                                        }

                                        lstUser.Add(_getDataEva.acknowledge_user);
                                        var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();

                                        int nNO = 1;


                                        if (!string.IsNullOrEmpty(session))
                                        {

                                            var _getRequest = _getData.PersonnelRequest;
                                            decimal? nCoreScore = 0, nAuditingScore = 0, nTotalScore = 0;
                                            //Check Form Mass (New or Old)
                                            var _CheckDataF = _TM_Candidate_MassTIFService.FindByMappingID(_getData.Id);
                                            var _formId = _CheckDataF.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question != null).FirstOrDefault();
                                            var _chkM = _formId.TM_MassTIF_Form_Question.TM_Mass_TIF_Form.Id;

                                            //new
                                            #region Report New Form Core
                                            ReportMASSTIFFormNew reportnew = new ReportMASSTIFFormNew();
                                            //Evaluator
                                            XRLabel xrFirstEvaNamenew = (XRLabel)reportnew.FindControl("xrFirstEvaName", true);
                                            XRLabel xrFirstEvaRanknew = (XRLabel)reportnew.FindControl("xrFirstEvaRank", true);
                                            XRLabel xrFirstEvaDatenew = (XRLabel)reportnew.FindControl("xrFirstEvaDate", true);
                                            XRLabel xrFirstEvaGroupnew = (XRLabel)reportnew.FindControl("xrFirstEvaGroup", true);
                                            XRLabel xrSecEvaNamenew = (XRLabel)reportnew.FindControl("xrSecEvaName", true);
                                            XRLabel xrSecEvaRanknew = (XRLabel)reportnew.FindControl("xrSecEvaRank", true);
                                            XRLabel xrSecEvaDatenew = (XRLabel)reportnew.FindControl("xrSecEvaDate", true);
                                            XRLabel xrSecEvaGroupnew = (XRLabel)reportnew.FindControl("xrSecEvaGroup", true);
                                            XRLabel xrCandidateNamenew = (XRLabel)reportnew.FindControl("xrCandidateName", true);
                                            XRLabel xrGroupnew = (XRLabel)reportnew.FindControl("xrGroup", true);
                                            XRLabel xrPositionnew = (XRLabel)reportnew.FindControl("xrPosition", true);
                                            XRLabel xrRecRanknew = (XRLabel)reportnew.FindControl("xrRecRank", true);
                                            XRLabel xrResultnew = (XRLabel)reportnew.FindControl("xrResult", true);

                                            //XRLabel xrCoreScorenew = (XRLabel)reportnew.FindControl("xrCoreScore", true);
                                            XRLabel xrAuditingScorenew = (XRLabel)reportnew.FindControl("xrAuditingScore", true);
                                            //XRLabel xrTotalScorenew = (XRLabel)reportnew.FindControl("xrTotalScore", true);
                                            XRLabel xrlblPrintbtnew = (XRLabel)reportnew.FindControl("xrlblPrintbt", true);
                                            xrCandidateNamenew.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                            xrPositionnew.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                            xrRecRanknew.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                            xrResultnew.Text = "Interview Result: " + (_getDataEva.TM_MassTIF_Status != null ? _getDataEva.TM_MassTIF_Status.masstif_status_name_en + "" : "");
                                            xrGroupnew.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                            //xrCoreScorenew.Text = "Core Competency score: " + nCoreScore.NodecimalFormat();
                                            xrAuditingScorenew.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                            //xrTotalScorenew.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat();
                                            //xrComment.Text = _getDataEva.comments;
                                            //if (_getDataEva.confidentiality_agreement + "" == "Y")
                                            //{
                                            //    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                            //}
                                            xrlblPrintbtnew.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                            //if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                            //{
                                            //    nCoreScore = _getDataEva.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => s.TM_TIF_Rating != null ? s.TM_TIF_Rating.point : 0).Sum();
                                            //}
                                            if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                            {
                                                nAuditingScore = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                            }
                                            //xrCoreScorenew.Text = "Core Competency score: " + nCoreScore.NodecimalFormat() + "/36";
                                            xrAuditingScorenew.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                            //xrTotalScorenew.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat() + "/72";
                                            #endregion
                                            #region Report Page 2 New Form
                                            ReportMASSCAuditingNew report2N = new ReportMASSCAuditingNew();
                                            //Evaluator
                                            XRLabel xrFirstEvaName2N = (XRLabel)report2N.FindControl("xrFirstEvaName", true);
                                            XRLabel xrFirstEvaRank2N = (XRLabel)report2N.FindControl("xrFirstEvaRank", true);
                                            XRLabel xrFirstEvaDate2N = (XRLabel)report2N.FindControl("xrFirstEvaDate", true);
                                            XRLabel xrFirstEvaGroup2N = (XRLabel)report2N.FindControl("xrFirstEvaGroup", true);
                                            XRLabel xrSecEvaName2N = (XRLabel)report2N.FindControl("xrSecEvaName", true);
                                            XRLabel xrSecEvaRank2N = (XRLabel)report2N.FindControl("xrSecEvaRank", true);
                                            XRLabel xrSecEvaDate2N = (XRLabel)report2N.FindControl("xrSecEvaDate", true);
                                            XRLabel xrSecEvaGroup2N = (XRLabel)report2N.FindControl("xrSecEvaGroup", true);
                                            XRLabel xrCandidateName2N = (XRLabel)report2N.FindControl("xrCandidateName", true);
                                            XRLabel xrGroup2N = (XRLabel)report2N.FindControl("xrGroup", true);
                                            XRLabel xrPosition2N = (XRLabel)report2N.FindControl("xrPosition", true);
                                            XRLabel xrRecRank2N = (XRLabel)report2N.FindControl("xrRecRank", true);
                                            XRLabel xrResult2N = (XRLabel)report2N.FindControl("xrResult", true);

                                            //XRLabel xrCoreScore2N = (XRLabel)report2N.FindControl("xrCoreScore", true);
                                            XRLabel xrAuditingScore2N = (XRLabel)report2N.FindControl("xrAuditingScore", true);
                                            //XRLabel xrTotalScore2N = (XRLabel)report2N.FindControl("xrTotalScore", true);
                                            XRLabel xrlblPrintbt2N = (XRLabel)report2N.FindControl("xrlblPrintbt", true);
                                            xrCandidateName2N.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                            xrPosition2N.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                            xrRecRank2N.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                            xrResult2N.Text = "Interview Result: " + (_getDataEva.TM_MassTIF_Status != null ? _getDataEva.TM_MassTIF_Status.masstif_status_name_en + "" : "");
                                            xrGroup2N.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                            //xrComment.Text = _getDataEva.comments;
                                            //if (_getDataEva.confidentiality_agreement + "" == "Y")
                                            //{
                                            //    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                            //}
                                            xrlblPrintbt2N.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                            //if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                            //{
                                            //    nCoreScore = _getDataEva.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                            //}
                                            if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                            {
                                                nAuditingScore = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                            }
                                            //xrCoreScore2N.Text = "Core Competency score: " + nCoreScore.NodecimalFormat();
                                            xrAuditingScore2N.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                            //xrTotalScore2N.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat();
                                            #endregion
                                            #region Report Page 3 NEW Form
                                            ReportMASSCADInfoNew report3N = new ReportMASSCADInfoNew();
                                            //Evaluator
                                            XRLabel xrFirstEvaName3N = (XRLabel)report3N.FindControl("xrFirstEvaName", true);
                                            XRLabel xrFirstEvaRank3N = (XRLabel)report3N.FindControl("xrFirstEvaRank", true);
                                            XRLabel xrFirstEvaDate3N = (XRLabel)report3N.FindControl("xrFirstEvaDate", true);
                                            XRLabel xrFirstEvaGroup3N = (XRLabel)report3N.FindControl("xrFirstEvaGroup", true);
                                            XRLabel xrSecEvaName3N = (XRLabel)report3N.FindControl("xrSecEvaName", true);
                                            XRLabel xrSecEvaRank3N = (XRLabel)report3N.FindControl("xrSecEvaRank", true);
                                            XRLabel xrSecEvaDate3N = (XRLabel)report3N.FindControl("xrSecEvaDate", true);
                                            XRLabel xrSecEvaGroup3N = (XRLabel)report3N.FindControl("xrSecEvaGroup", true);
                                            XRLabel xrCandidateName3N = (XRLabel)report3N.FindControl("xrCandidateName", true);
                                            XRLabel xrGroup3N = (XRLabel)report3N.FindControl("xrGroup", true);
                                            XRLabel xrPosition3N = (XRLabel)report3N.FindControl("xrPosition", true);
                                            XRLabel xrRecRank3N = (XRLabel)report3N.FindControl("xrRecRank", true);
                                            XRLabel xrResult3N = (XRLabel)report3N.FindControl("xrResult", true);

                                            //XRLabel xrCoreScore3N = (XRLabel)report3N.FindControl("xrCoreScore", true);
                                            XRLabel xrAuditingScore3N = (XRLabel)report3N.FindControl("xrAuditingScore", true);
                                            //XRLabel xrTotalScore3N = (XRLabel)report3N.FindControl("xrTotalScore", true);

                                            XRTableCell xrCommentN = (XRTableCell)report3N.FindControl("xrComment", true);
                                            XRPictureBox xrPicCheckN = (XRPictureBox)report3N.FindControl("xrPicCheck", true);
                                            XRLabel xrlblPrintbt3N = (XRLabel)report3N.FindControl("xrlblPrintbt", true);

                                            xrCandidateName3N.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                            xrPosition3N.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                            xrRecRank3N.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                            xrResult3N.Text = "Interview Result: " + (_getDataEva.TM_MassTIF_Status != null ? _getDataEva.TM_MassTIF_Status.masstif_status_name_en + "" : "");
                                            xrGroup3N.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                            xrCommentN.Text = _getDataEva.comments.TexttoReportnewline();
                                            if (_getDataEva.confidentiality_agreement + "" == "Y")
                                            {
                                                xrPicCheckN.ImageUrl = "~\\Image\\check.png";
                                            }
                                            xrlblPrintbt3N.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                            //if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                            //{
                                            //    nCoreScore = _getDataEva.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                            //}
                                            if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                            {
                                                nAuditingScore = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                            }
                                            //xrCoreScore3N.Text = "Core Competency score: " + nCoreScore.NodecimalFormat();
                                            xrAuditingScore3N.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                            //xrTotalScore3N.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat();
                                            #endregion
                                            #region Report Page 1
                                            ReportMASSTIFForm report = new ReportMASSTIFForm();
                                            //Evaluator
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
                                            XRLabel xrPosition = (XRLabel)report.FindControl("xrPosition", true);
                                            XRLabel xrRecRank = (XRLabel)report.FindControl("xrRecRank", true);
                                            XRLabel xrResult = (XRLabel)report.FindControl("xrResult", true);

                                            XRLabel xrCoreScore = (XRLabel)report.FindControl("xrCoreScore", true);
                                            XRLabel xrAuditingScore = (XRLabel)report.FindControl("xrAuditingScore", true);
                                            XRLabel xrTotalScore = (XRLabel)report.FindControl("xrTotalScore", true);
                                            XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);
                                            xrCandidateName.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                            xrPosition.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                            xrRecRank.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                            xrResult.Text = "Interview Result: " + (_getDataEva.TM_MassTIF_Status != null ? _getDataEva.TM_MassTIF_Status.masstif_status_name_en + "" : "");
                                            xrGroup.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                            //xrComment.Text = _getDataEva.comments;
                                            //if (_getDataEva.confidentiality_agreement + "" == "Y")
                                            //{
                                            //    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                            //}
                                            xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                            if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                            {
                                                nCoreScore = _getDataEva.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                            }
                                            if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                            {
                                                nAuditingScore = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                            }
                                            xrCoreScore.Text = "Core Competency score: " + nCoreScore.NodecimalFormat();
                                            xrAuditingScore.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                            xrTotalScore.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat();
                                            #endregion
                                            #region Report Page 2
                                            ReportMASSCAuditing report2 = new ReportMASSCAuditing();
                                            //Evaluator
                                            XRLabel xrFirstEvaName2 = (XRLabel)report2.FindControl("xrFirstEvaName", true);
                                            XRLabel xrFirstEvaRank2 = (XRLabel)report2.FindControl("xrFirstEvaRank", true);
                                            XRLabel xrFirstEvaDate2 = (XRLabel)report2.FindControl("xrFirstEvaDate", true);
                                            XRLabel xrFirstEvaGroup2 = (XRLabel)report2.FindControl("xrFirstEvaGroup", true);
                                            XRLabel xrSecEvaName2 = (XRLabel)report2.FindControl("xrSecEvaName", true);
                                            XRLabel xrSecEvaRank2 = (XRLabel)report2.FindControl("xrSecEvaRank", true);
                                            XRLabel xrSecEvaDate2 = (XRLabel)report2.FindControl("xrSecEvaDate", true);
                                            XRLabel xrSecEvaGroup2 = (XRLabel)report2.FindControl("xrSecEvaGroup", true);
                                            XRLabel xrCandidateName2 = (XRLabel)report2.FindControl("xrCandidateName", true);
                                            XRLabel xrGroup2 = (XRLabel)report2.FindControl("xrGroup", true);
                                            XRLabel xrPosition2 = (XRLabel)report2.FindControl("xrPosition", true);
                                            XRLabel xrRecRank2 = (XRLabel)report2.FindControl("xrRecRank", true);
                                            XRLabel xrResult2 = (XRLabel)report2.FindControl("xrResult", true);

                                            XRLabel xrCoreScore2 = (XRLabel)report2.FindControl("xrCoreScore", true);
                                            XRLabel xrAuditingScore2 = (XRLabel)report2.FindControl("xrAuditingScore", true);
                                            XRLabel xrTotalScore2 = (XRLabel)report2.FindControl("xrTotalScore", true);
                                            XRLabel xrlblPrintbt2 = (XRLabel)report2.FindControl("xrlblPrintbt", true);
                                            xrCandidateName2.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                            xrPosition2.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                            xrRecRank2.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                            xrResult2.Text = "Interview Result: " + (_getDataEva.TM_MassTIF_Status != null ? _getDataEva.TM_MassTIF_Status.masstif_status_name_en + "" : "");
                                            xrGroup2.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                            //xrComment.Text = _getDataEva.comments;
                                            //if (_getDataEva.confidentiality_agreement + "" == "Y")
                                            //{
                                            //    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                            //}
                                            xrlblPrintbt2.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                            if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                            {
                                                nCoreScore = _getDataEva.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                            }
                                            if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                            {
                                                nAuditingScore = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                            }
                                            xrCoreScore2.Text = "Core Competency score: " + nCoreScore.NodecimalFormat();
                                            xrAuditingScore2.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                            xrTotalScore2.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat();
                                            #endregion
                                            #region Report Page 3
                                            ReportMASSCADInfo report3 = new ReportMASSCADInfo();
                                            //Evaluator
                                            XRLabel xrFirstEvaName3 = (XRLabel)report3.FindControl("xrFirstEvaName", true);
                                            XRLabel xrFirstEvaRank3 = (XRLabel)report3.FindControl("xrFirstEvaRank", true);
                                            XRLabel xrFirstEvaDate3 = (XRLabel)report3.FindControl("xrFirstEvaDate", true);
                                            XRLabel xrFirstEvaGroup3 = (XRLabel)report3.FindControl("xrFirstEvaGroup", true);
                                            XRLabel xrSecEvaName3 = (XRLabel)report3.FindControl("xrSecEvaName", true);
                                            XRLabel xrSecEvaRank3 = (XRLabel)report3.FindControl("xrSecEvaRank", true);
                                            XRLabel xrSecEvaDate3 = (XRLabel)report3.FindControl("xrSecEvaDate", true);
                                            XRLabel xrSecEvaGroup3 = (XRLabel)report3.FindControl("xrSecEvaGroup", true);
                                            XRLabel xrCandidateName3 = (XRLabel)report3.FindControl("xrCandidateName", true);
                                            XRLabel xrGroup3 = (XRLabel)report3.FindControl("xrGroup", true);
                                            XRLabel xrPosition3 = (XRLabel)report3.FindControl("xrPosition", true);
                                            XRLabel xrRecRank3 = (XRLabel)report3.FindControl("xrRecRank", true);
                                            XRLabel xrResult3 = (XRLabel)report3.FindControl("xrResult", true);

                                            XRLabel xrCoreScore3 = (XRLabel)report3.FindControl("xrCoreScore", true);
                                            XRLabel xrAuditingScore3 = (XRLabel)report3.FindControl("xrAuditingScore", true);
                                            XRLabel xrTotalScore3 = (XRLabel)report3.FindControl("xrTotalScore", true);

                                            XRTableCell xrComment = (XRTableCell)report3.FindControl("xrComment", true);
                                            XRPictureBox xrPicCheck = (XRPictureBox)report3.FindControl("xrPicCheck", true);
                                            XRLabel xrlblPrintbt3 = (XRLabel)report3.FindControl("xrlblPrintbt", true);

                                            xrCandidateName3.Text = "Candidate Name: " + (_getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name());
                                            xrPosition3.Text = "Position: " + (_getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "");
                                            xrRecRank3.Text = "Recommended Rank (After interview): " + (_getDataEva.Recommended_Rank != null ? _getDataEva.Recommended_Rank.Pool_rank_name_en + "" : "");
                                            xrResult3.Text = "Interview Result: " + (_getDataEva.TM_MassTIF_Status != null ? _getDataEva.TM_MassTIF_Status.masstif_status_name_en + "" : "");
                                            xrGroup3.Text = "Group: " + (_getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en);
                                            xrComment.Text = _getDataEva.comments.TexttoReportnewline();
                                            if (_getDataEva.confidentiality_agreement + "" == "Y")
                                            {
                                                xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                            }
                                            xrlblPrintbt3.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();
                                            if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                            {
                                                nCoreScore = _getDataEva.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                            }
                                            if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                            {
                                                nAuditingScore = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.active_status == "Y").Select(s => s.TM_Mass_Scoring != null ? s.TM_Mass_Scoring.point : 0).Sum();
                                            }
                                            xrCoreScore3.Text = "Core Competency score: " + nCoreScore.NodecimalFormat();
                                            xrAuditingScore3.Text = "Auditing Question score: " + nAuditingScore.NodecimalFormat();
                                            xrTotalScore3.Text = "Total score: " + (nCoreScore + nAuditingScore).NodecimalFormat();
                                            #endregion


                                            if (_getDataEva.TM_Candidate_MassTIF_Approv != null && _getDataEva.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y"))
                                            {
                                                var _getFirst = _getDataEva.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                                var _getSecond = _getDataEva.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
                                                if (_getFirst != null)
                                                {
                                                    var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();
                                                    if (_getEmpFirst != null)
                                                    {
                                                        xrFirstEvaName.Text = _getEmpFirst.EmpFullName + "";
                                                        xrFirstEvaDate.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                        xrFirstEvaRank.Text = _getEmpFirst.Rank;
                                                        xrFirstEvaGroup.Text = _getEmpFirst.UnitGroupName;
                                                        //Form New Page1
                                                        xrFirstEvaNamenew.Text = _getEmpFirst.EmpFullName + "";
                                                        xrFirstEvaRanknew.Text = _getEmpFirst.Rank;
                                                        xrFirstEvaDatenew.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                        xrFirstEvaGroupnew.Text = _getEmpFirst.UnitGroupName;

                                                        xrFirstEvaName2.Text = _getEmpFirst.EmpFullName + "";
                                                        xrFirstEvaDate2.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                        xrFirstEvaRank2.Text = _getEmpFirst.Rank;
                                                        xrFirstEvaGroup2.Text = _getEmpFirst.UnitGroupName;
                                                        //Form New Page2
                                                        xrFirstEvaName2N.Text = _getEmpFirst.EmpFullName + "";
                                                        xrFirstEvaDate2N.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                        xrFirstEvaRank2N.Text = _getEmpFirst.Rank;
                                                        xrFirstEvaGroup2N.Text = _getEmpFirst.UnitGroupName;

                                                        xrFirstEvaName3.Text = _getEmpFirst.EmpFullName + "";
                                                        xrFirstEvaDate3.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                        xrFirstEvaRank3.Text = _getEmpFirst.Rank;
                                                        xrFirstEvaGroup3.Text = _getEmpFirst.UnitGroupName;

                                                        //Form New Page 3
                                                        xrFirstEvaName3N.Text = _getEmpFirst.EmpFullName + "";
                                                        xrFirstEvaDate3N.Text = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                                        xrFirstEvaRank3N.Text = _getEmpFirst.Rank;
                                                        xrFirstEvaGroup3N.Text = _getEmpFirst.UnitGroupName;
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

                                                        //Form New Page1
                                                        xrSecEvaNamenew.Text = _getEmpSecond.EmpFullName + "";
                                                        xrSecEvaDatenew.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                                        xrSecEvaRanknew.Text = _getEmpSecond.Rank;
                                                        xrSecEvaGroupnew.Text = _getEmpSecond.UnitGroupName;

                                                        xrSecEvaName2.Text = _getEmpSecond.EmpFullName + "";
                                                        xrSecEvaDate2.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                                        xrSecEvaRank2.Text = _getEmpSecond.Rank;
                                                        xrSecEvaGroup2.Text = _getEmpSecond.UnitGroupName;
                                                        //Form New Page2
                                                        xrSecEvaName2N.Text = _getEmpSecond.EmpFullName + "";
                                                        xrSecEvaDate2N.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                                        xrSecEvaRank2N.Text = _getEmpSecond.Rank;
                                                        xrSecEvaGroup2N.Text = _getEmpSecond.UnitGroupName;

                                                        xrSecEvaName3.Text = _getEmpSecond.EmpFullName + "";
                                                        xrSecEvaDate3.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                                        xrSecEvaRank3.Text = _getEmpSecond.Rank;
                                                        xrSecEvaGroup3.Text = _getEmpSecond.UnitGroupName;

                                                        //Form New Page 3
                                                        xrSecEvaName3N.Text = _getEmpSecond.EmpFullName + "";
                                                        xrSecEvaDate3N.Text = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                                        xrSecEvaRank3N.Text = _getEmpSecond.Rank;
                                                        xrSecEvaGroup3N.Text = _getEmpSecond.UnitGroupName;
                                                    }

                                                }
                                            }

                                            #region Core 
                                            List<vdsCore> lstCore = new List<vdsCore>();

                                            if (_getDataEva.TM_Candidate_MassTIF_Core != null && _getDataEva.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                            {
                                                var _CheckData = _TM_Candidate_MassTIFService.FindByMappingID(_getData.Id);

                                                if (_chkM == 1)
                                                {
                                                    lstCore = (from lstQ in _getDataEva.TM_Candidate_MassTIF_Core.Where(a => a.active_status == "Y").OrderBy(o => o.TM_MassTIF_Form_Question.seq)
                                                               select new vdsCore
                                                               {
                                                                   sheader = lstQ.TM_MassTIF_Form_Question.seq + ". " + lstQ.TM_MassTIF_Form_Question.header,
                                                                   nSeq = lstQ.TM_MassTIF_Form_Question.seq,
                                                                   sevidence = lstQ.evidence.TexttoReportnewline() + "",
                                                                   sscoring = lstQ.TM_Mass_Scoring != null ? lstQ.TM_Mass_Scoring.scoring_code + "" : "",
                                                               }).ToList();
                                                }
                                                else
                                                {
                                                    lstCore = _CheckData.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").Select(s => new vdsCore
                                                    {
                                                        sheader = s.TM_MassTIF_Form_Question.header + "",
                                                        question = s.TM_MassTIF_Form_Question.question.Replace("<b>", "").Replace("</b>", "").Replace("<br>", " : ") + "",
                                                        sevidence = s.evidence.TexttoReportnewline() + "",
                                                        srating = s.TM_TIF_Rating != null ? s.TM_TIF_Rating.rating_name_en + "" : "",
                                                    }).ToList();
                                                }

                                            }
                                            #endregion


                                            #region Audting
                                            List<vMasstif_list_Auditing> lstAuditing = new List<vMasstif_list_Auditing>();
                                            if (_chkM == 2)
                                            {
                                                //New 
                                                lstAuditing.Add(new vMasstif_list_Auditing
                                                {
                                                    header = "Easy Question",
                                                    nSeq = 1,
                                                    id = 1 + "",
                                                    answer = "",


                                                });
                                                lstAuditing.Add(new vMasstif_list_Auditing
                                                {
                                                    header = "Easy Question",
                                                    nSeq = 2,
                                                    id = 2 + "",
                                                    answer = "",

                                                });
                                                lstAuditing.Add(new vMasstif_list_Auditing
                                                {
                                                    header = "Moderate Questions",
                                                    nSeq = 3,
                                                    id = 3 + "",
                                                    answer = "",

                                                });
                                                lstAuditing.Add(new vMasstif_list_Auditing
                                                {
                                                    header = "Moderate Questions",
                                                    nSeq = 4,
                                                    id = 4 + "",
                                                    answer = "",

                                                });
                                                lstAuditing.Add(new vMasstif_list_Auditing
                                                {
                                                    header = "Hard Questions",
                                                    nSeq = 5,
                                                    id = 5 + "",
                                                    answer = "",

                                                });


                                            }
                                            else
                                            {

                                                lstAuditing.Add(new vMasstif_list_Auditing
                                                {
                                                    header = "Fixed Question",
                                                    nSeq = 1,
                                                    id = 1 + "",
                                                    answer = "",

                                                });
                                                lstAuditing.Add(new vMasstif_list_Auditing
                                                {
                                                    header = "Fixed Question",
                                                    nSeq = 2,
                                                    id = 2 + "",
                                                    answer = "",
                                                });
                                                lstAuditing.Add(new vMasstif_list_Auditing
                                                {
                                                    header = "Auditing Knowledge",
                                                    nSeq = 3,
                                                    id = 3 + "",
                                                    answer = "",
                                                });
                                                lstAuditing.Add(new vMasstif_list_Auditing
                                                {
                                                    header = "Audit Procedures",
                                                    nSeq = 4,
                                                    id = 4 + "",
                                                    answer = "",
                                                });
                                                lstAuditing.Add(new vMasstif_list_Auditing
                                                {
                                                    header = "Skepticism / Analytical",
                                                    nSeq = 5,
                                                    id = 5 + "",
                                                    answer = "",
                                                });
                                            }

                                            if (_getDataEva.TM_Candidate_MassTIF_Audit_Qst != null && _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                            {
                                                if (_chkM == 2)
                                                {
                                                    lstAuditing.ForEach(ed =>
                                                    {
                                                        var GetAns = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.seq + "" == ed.id).FirstOrDefault();
                                                        if (GetAns != null)
                                                        {
                                                            ed.answer = GetAns.answer.TexttoReportnewline(50) + "";
                                                            ed.scoring = GetAns.TM_Mass_Scoring != null ? GetAns.TM_Mass_Scoring.seq + "" : "";
                                                            ed.Qst = GetAns.TM_Mass_Auditing_Question != null ? GetAns.TM_Mass_Auditing_Question.header.TexttoReportnewline() + "" : "";
                                                        }
                                                    });
                                                }
                                                else
                                                {
                                                    lstAuditing.ForEach(ed =>
                                                    {
                                                        var GetAns = _getDataEva.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.seq + "" == ed.id).FirstOrDefault();
                                                        if (GetAns != null)
                                                        {
                                                            ed.answer = GetAns.answer.TexttoReportnewline(50) + "";
                                                            ed.scoring = GetAns.TM_Mass_Scoring != null ? GetAns.TM_Mass_Scoring.scoring_code + "" : "";
                                                            ed.Qst = GetAns.TM_Mass_Auditing_Question != null ? GetAns.TM_Mass_Auditing_Question.header.TexttoReportnewline() + "" : "";
                                                        }
                                                    });
                                                }
                                            }
                                            #endregion

                                            #region Additional

                                            List<vdsAdInfo_Question> lstAdIn = new List<vdsAdInfo_Question>();

                                            if (_getDataEva.TM_Candidate_MassTIF_Additional != null && _getDataEva.TM_Candidate_MassTIF_Additional.Any(a => a.active_status == "Y"))
                                            {

                                                lstAdIn = _getDataEva.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y").OrderBy(o => o.TM_Additional_Questions.seq).Select(s => new vdsAdInfo_Question
                                                {

                                                    question = s.TM_Additional_Questions.question.Replace("<b>", "").Replace("</b>", "").Replace("<br>", " : ") + "",
                                                    seq = s.TM_Additional_Questions.seq,
                                                    other_answer = s.other_answer.TexttoReportnewline() + "",
                                                    multi_answer = s.TM_Candidate_MassTIF_Adnl_Answer != null ? string.Join(Environment.NewLine + "", s.TM_Candidate_MassTIF_Adnl_Answer.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Additional_Answers.seq).Select((s2, index) => (index + 1) + ". " + s2.TM_Additional_Answers.answers).ToList()) : "",
                                                    //lstAnswers = s.TM_Candidate_MassTIF_Adnl_Answer != null ? s.TM_Candidate_MassTIF_Adnl_Answer.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Additional_Answers.seq).Select(s2 => new vMassAdInfo_Answers
                                                    //{
                                                    //    nID = s2.Id + "",
                                                    //    answer = s2.TM_Additional_Answers.answers,
                                                    //    seq = s2.TM_Additional_Answers.seq,

                                                    //}).ToList() : new List<vMassAdInfo_Answers>(),
                                                }).ToList();

                                            }
                                            #endregion

                                            DataTable dtR = SystemFunction.LinqToDataTable(lstCore);
                                            DataTable dtAd = SystemFunction.LinqToDataTable(lstAuditing);
                                            DataTable dtAdInfo = SystemFunction.LinqToDataTable(lstAdIn);
                                            dsReportMASSTIFForm ds = new dsReportMASSTIFForm();

                                            if (_chkM >= 2)
                                            {
                                                ds.dsCore.Merge(dtR);
                                                ds.dsAuditing.Merge(dtAd);
                                                ds.dsAdInfo.Merge(dtAdInfo);
                                                //Get Data report New
                                                reportnew.DataSource = ds;
                                                report2N.DataSource = ds;
                                                report3N.DataSource = ds;


                                                //create report New
                                                reportnew.CreateDocument();
                                                report2N.CreateDocument();
                                                report3N.CreateDocument();
                                                //Add page
                                                xrTemp.Pages.AddRange(reportnew.Pages);
                                                xrTemp.Pages.AddRange(report2N.Pages);
                                                xrTemp.Pages.AddRange(report3N.Pages);

                                            }
                                            else if (_chkM == 1)
                                            {
                                                ds.dsCore.Merge(dtR);
                                                ds.dsAuditing.Merge(dtAd);
                                                ds.dsAdInfo.Merge(dtAdInfo);
                                                //Get Data report old
                                                report.DataSource = ds;
                                                report2.DataSource = ds;
                                                report3.DataSource = ds;

                                                //Create Data Report old
                                                report.CreateDocument();
                                                report2.CreateDocument();
                                                report3.CreateDocument();
                                                //Add page
                                                xrTemp.Pages.AddRange(report.Pages);
                                                xrTemp.Pages.AddRange(report2.Pages);
                                                xrTemp.Pages.AddRange(report3.Pages);

                                            }

                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
                                        //throw;
                                    }

                                    PdfExportOptions options = new PdfExportOptions();
                                    options.ShowPrintDialogOnOpen = true;
                                    xrTemp.PrintingSystem.ContinuousPageNumbering = false;

                                }
                            }
                            xrTemp.ExportToPdf(stream);
                            return File(stream.GetBuffer(), "application/pdf", "MASSTIFForm" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                        }
                        else
                        {
                            return RedirectToAction("Error404", "MasterPage", new vErrorObject { msg = "error session expired" });
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