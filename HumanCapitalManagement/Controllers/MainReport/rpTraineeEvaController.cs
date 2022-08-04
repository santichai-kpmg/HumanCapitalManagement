using DevExpress.XtraPrinting;
using DevExpress.XtraReports.UI;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Report.DataSet.FormDataset;
using HumanCapitalManagement.Report.DataSet.Trainee;
using HumanCapitalManagement.Report.DevReport.Form;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TIFForm;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.ReportVM;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.MainReport
{
    public class rpTraineeEvaController : BaseController
    {
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private TM_Evaluation_FormService _TM_Evaluation_FormService;
        private TM_Eva_RatingService _TM_Eva_RatingService;
        private TM_Trainee_EvaService _TM_Trainee_EvaService;
        private TM_Trainee_Eva_AnswerService _TM_Trainee_Eva_AnswerService;
        private TM_TraineeEva_StatusService _TM_TraineeEva_StatusService;
        private MailContentService _MailContentService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        
        public rpTraineeEvaController(
         TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , TM_Evaluation_FormService TM_Evaluation_FormService
            , TM_Eva_RatingService TM_Eva_RatingService
            , TM_Trainee_EvaService TM_Trainee_EvaService
            , TM_Trainee_Eva_AnswerService TM_Trainee_Eva_AnswerService, TM_TraineeEva_StatusService TM_TraineeEva_StatusService
            , MailContentService MailContentService)
        {
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TM_Evaluation_FormService = TM_Evaluation_FormService;
            _TM_Eva_RatingService = TM_Eva_RatingService;
            _TM_Trainee_EvaService = TM_Trainee_EvaService;
            _TM_Trainee_Eva_AnswerService = TM_Trainee_Eva_AnswerService;
            _TM_TraineeEva_StatusService = TM_TraineeEva_StatusService;
            _MailContentService = MailContentService;
        }
        // GET: rpTraineeEva
        public ActionResult Index()
        {
            return View();
        }

        #region export PDF
        public ActionResult ExptraineeEvaForm(string qryStr, string sMode = "pdf")
        {
            var sCheck = acCheckLoginAndPermissionExport("TraineeEvaReportView", "TraineeEva");
            
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
                    var _getDataEva = _TM_Trainee_EvaService.Find(nId);
                    var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getDataEva != null && _getDataEva.TM_PR_Candidate_Mapping != null)
                    {
                        try
                        {
                            List<string> lstUser = new List<string>();
                            lstUser.Add(_getDataEva.req_incharge_Approve_user);
                            lstUser.Add(_getDataEva.req_mgr_Approve_user);
                            lstUser.Add(_getDataEva.acknowledge_user);
                            var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();
                            var _getratings = new List<rpvEva_Rating>();
                            List<rpvEva_list_question> lstAns = new List<rpvEva_list_question>();
                            int nNO = 1;

                            //rpvEva_Rating
                            var _getrating = _TM_Eva_RatingService.GetDataForSelectAll();
                            var _CheckDataF = _TM_Trainee_EvaService.Find(_getData.Id);
                            var formId = _CheckDataF.TM_Trainee_Eva_Answer.Where(w => w.TM_Evaluation_Question != null).FirstOrDefault();
                            var _CheckF = formId.TM_Evaluation_Question.TM_Evaluation_Form.Id;

                            if (_getDataEva.TM_Trainee_Eva_Answer != null && _getDataEva.TM_Trainee_Eva_Answer.Any(a => a.active_status == "Y"))
                            {
                                if (_CheckF == 7)
                                {
                                    lstAns = _getDataEva.TM_Trainee_Eva_Answer.Where(w => w.active_status == "Y").Select(s => new rpvEva_list_question
                                    {

                                        seq = s.TM_Evaluation_Question.seq,
                                        no = nNO++,
                                        description = s.TM_Evaluation_Question.question + "",
                                        sgroup = s.TM_Evaluation_Question.header,
                                        comment = s.inchage_comment.TexttoReportnewline(48) + "",
                                        trainee_rating = s.trainee_rating != null ? s.trainee_rating.point + " : " + s.trainee_rating.rating_name_en + "" : "",
                                        eva_ratine = s.inchage_rating != null ? s.inchage_rating.point + " : " + s.inchage_rating.rating_name_en + "" : "",
                                    }).ToList();
                                }
                                else
                                {
                                    lstAns = _getDataEva.TM_Trainee_Eva_Answer.Select(s => new rpvEva_list_question
                                    {

                                        seq = s.TM_Evaluation_Question.seq,
                                        no = nNO++,
                                        description = s.TM_Evaluation_Question.question + "",
                                        sgroup = s.TM_Evaluation_Question.header,
                                        comment = s.inchage_comment.TexttoReportnewline(48) + "",
                                        trainee_rating = s.trainee_rating != null ? s.trainee_rating.point + " : " + s.trainee_rating.rating_name_en + "" : "",
                                        eva_ratine = s.inchage_rating != null ? s.inchage_rating.point + " : " + s.inchage_rating.rating_name_en + "" : "",
                                    }).ToList();
                                }

                                //check Form
                                var _getActEva = _TM_Trainee_Eva_AnswerService.findByTraineeEvaId(nId); // for get info of eva form 

                                var chkFormId = _getActEva.TM_Evaluation_Question.TM_Evaluation_Form.Id;
                                //Add Code New Question 
                                var _getEva = _TM_Evaluation_FormService.Find(chkFormId);

                                if (_getEva.Id >= 7)
                                {
                                    var lstTmp = _getrating.ToList();

                                    _getratings = lstTmp.Where(w=> w.TM_Evaluation_Form.Id == _getEva.Id).Select(s => new rpvEva_Rating()
                                    {
                                        sratingname = s.rating_name_en,
                                        sratingdes = s.rating_description,
                                    }).ToList();
                                    
                                    
                                }
                                else
                                {
                                    _getratings = _getrating.Where(w=> w.TM_Evaluation_Form.Id == _getEva.Id).Select(s => new rpvEva_Rating()
                                    {
                                        sratingname = s.rating_name_en,
                                        sratingdes = s.rating_description
                                    }).ToList();
                                }

                            }
                            string sTraineeFullname = _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "_" + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                            var stream = new MemoryStream();
                            XrTemp xrTemp = new XrTemp();
                            if (!string.IsNullOrEmpty(sMode) && sMode == "pdf")
                            {

                                xrTemp.CreateDocument();
                                //lstAns.ForEach(ed =>
                                //{
                                //    if (!string.IsNullOrEmpty(ed.comment))
                                //    {

                                //        List<string> newList = ed.comment.Split(' ').ToList();
                                //        var sText = "";
                                //        foreach (var item in newList)
                                //        {
                                //            if (item.Length > 40)
                                //            {
                                //                sText += (String.Join(" ", (item.SplitBy(40)).ToArray())) + " ";
                                //            }
                                //            else
                                //            {
                                //                sText += item + " ";
                                //            }
                                //        }


                                //        ed.comment = sText;
                                //    }
                                //});
                                
                                DataTable dtR = SystemFunction.LinqToDataTable(lstAns);
                                DataTable dtRa = SystemFunction.LinqToDataTable(_getratings);
                                dsReportTraineeEvaForm ds = new dsReportTraineeEvaForm();
                                ds.dsMain.Merge(dtR);
                                ds.dsRating.Merge(dtRa);
                                ReportTraineeEvaForm report = new ReportTraineeEvaForm();
                                //XRTableCell xrTraineeName = (XRTableCell)report.FindControl("xrTraineeName", true);
                                //XRTableCell xrTraineeNo = (XRTableCell)report.FindControl("xrTraineeNo", true);
                                //XRTableCell xrTraineeStart = (XRTableCell)report.FindControl("xrTraineeStart", true);
                                //XRTableCell xrTraineeEnd = (XRTableCell)report.FindControl("xrTraineeEnd", true);
                                //XRTableCell xrTraineeGroup = (XRTableCell)report.FindControl("xrTraineeGroup", true);
                                //XRTableCell xrManager = (XRTableCell)report.FindControl("xrManager", true);
                                //XRTableCell xrInCharge = (XRTableCell)report.FindControl("xrInCharge", true);
                                //XRTableCell xrKeyCE = (XRTableCell)report.FindControl("xrKeyCE", true);
                                XRLabel xrTraineeName = (XRLabel)report.FindControl("xrTraineeName", true);
                                XRLabel xrTraineeNo = (XRLabel)report.FindControl("xrTraineeNo", true);
                                XRLabel xrNickname = (XRLabel)report.FindControl("xrNickname", true);
                                XRLabel xrTraineeStart = (XRLabel)report.FindControl("xrTraineeStart", true);
                                XRLabel xrTraineeEnd = (XRLabel)report.FindControl("xrTraineeEnd", true);
                                XRLabel xrTraineeGroup = (XRLabel)report.FindControl("xrTraineeGroup", true);
                                XRLabel xrManager = (XRLabel)report.FindControl("xrManager", true);
                                XRLabel xrInCharge = (XRLabel)report.FindControl("xrInCharge", true);
                                XRLabel xrKeyCE = (XRLabel)report.FindControl("xrKeyCE", true);


                                XRTableCell xrCellLeaned = (XRTableCell)report.FindControl("xrCellLeaned", true);
                                XRTableCell xrCellWell = (XRTableCell)report.FindControl("xrCellWell", true);
                                XRTableCell xrCellDevelop = (XRTableCell)report.FindControl("xrCellDevelop", true);
                                XRTableCell xrCellHiring = (XRTableCell)report.FindControl("xrCellHiring", true);
                                XRTableCell xrCellComment = (XRTableCell)report.FindControl("xrCellComment", true);


                                XRTableCell xrTraineeProposed = (XRTableCell)report.FindControl("xrTraineeProposed", true);
                                XRTableCell xrTraineeProposedDate = (XRTableCell)report.FindControl("xrTraineeProposedDate", true);
                                XRTableCell xrEvaluatorby = (XRTableCell)report.FindControl("xrEvaluatorby", true);
                                XRTableCell xrEvaluatorbyDate = (XRTableCell)report.FindControl("xrEvaluatorbyDate", true);
                                XRPictureBox xrPicCheck = (XRPictureBox)report.FindControl("xrPicCheck", true);
                                XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);

                                xrTraineeName.Text = "Trainee Name: " + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                                xrTraineeNo.Text = "Trainee No: " + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_TraineeNumber;
                                xrNickname.Text = "Nickname: " + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_NickName + "";
                                xrTraineeStart.Text = "Start Date: " + (_getDataEva.TM_PR_Candidate_Mapping.trainee_start.HasValue ? _getDataEva.TM_PR_Candidate_Mapping.trainee_start.Value.DateTimebyCulture() : "");
                                xrTraineeEnd.Text = "End Date: " + (_getDataEva.TM_PR_Candidate_Mapping.trainee_end.HasValue ? _getDataEva.TM_PR_Candidate_Mapping.trainee_end.Value.DateTimebyCulture() : "");
                                xrTraineeGroup.Text = "Group: " + _getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en;
                                xrManager.Text = "Performance Manager Name: " + _getUser.Where(w => w.EmpNo == _getDataEva.req_mgr_Approve_user).Select(s => s.EmpFullName).FirstOrDefault();
                                xrInCharge.Text = "In-Charge / Engagement Manager Name: " + _getUser.Where(w => w.EmpNo == _getDataEva.req_incharge_Approve_user).Select(s => s.EmpFullName).FirstOrDefault();
                                if (_getDataEva.approve_type + "" == "M")
                                {
                                    xrKeyCE.Text = "Key Contact Evaluator Name: " + _getUser.Where(w => w.EmpNo == _getDataEva.req_mgr_Approve_user).Select(s => s.EmpFullName).FirstOrDefault();
                                }
                                else
                                {
                                    xrKeyCE.Text = "Key Contact Evaluator Name: " + _getUser.Where(w => w.EmpNo == _getDataEva.req_incharge_Approve_user).Select(s => s.EmpFullName).FirstOrDefault();
                                }
                                //XRLabel xrCellLeaned2 = (XRLabel)report.FindControl("xrCellLeaned2", true);

                                xrCellLeaned.Text = _getDataEva.trainee_learned.TexttoReportnewline();
                                // xrCellLeaned2.Text = _getDataEva.trainee_learned.TexttoReport();

                                xrCellWell.Text = _getDataEva.trainee_done_well.TexttoReportnewline();
                                xrCellDevelop.Text = _getDataEva.trainee_developmental.TexttoReportnewline();

                                //Check Hiring Form
                                var _getActEva = _TM_Trainee_Eva_AnswerService.findByTraineeEvaId(nId); // for get info of eva form 

                                var chkFormId = _getActEva.TM_Evaluation_Question.TM_Evaluation_Form.Id;
                                //Add Code New Question 
                                var _getEva = _TM_Evaluation_FormService.Find(chkFormId);
                                if(_getEva.Id == 7)
                                {
                                   
                                    xrCellHiring.Text = _getDataEva.TM_Trainee_HiringRating.Trainee_HiringRating_name_en +"";
                                }
                                else if(_getEva.Id != 7)
                                {
                                    xrCellHiring.Text = _getDataEva.hiring_status + "" == "Y" ? "Yes" : "No";
                                }
                                xrCellComment.Text = _getDataEva.incharge_comments.TexttoReportnewline();
                                xrTraineeProposed.Text = "Proposed by: " + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "(Trainee)";
                                xrTraineeProposedDate.Text = "Date: " + (_getDataEva.update_date.HasValue ? _getDataEva.update_date.Value.DateTimebyCulture() : "");
                                //xrEvaluatorby.Text = "Evaluated by: ";
                                if (_getDataEva.approve_type + "" == "M")
                                {
                                    xrEvaluatorby.Text = "Evaluated by: " + _getUser.Where(w => w.EmpNo == _getDataEva.req_mgr_Approve_user).Select(s => s.EmpFullName).FirstOrDefault();
                                }
                                else
                                {
                                    xrEvaluatorby.Text = "Evaluated by: " + _getUser.Where(w => w.EmpNo == _getDataEva.req_incharge_Approve_user).Select(s => s.EmpFullName).FirstOrDefault();
                                }
                                xrEvaluatorbyDate.Text = "Date: " + (_getDataEva.incharge_update_date.HasValue ? _getDataEva.incharge_update_date.Value.DateTimebyCulture() : "");
                                if (_getDataEva.confidentiality_agreement + "" == "Y")
                                {
                                    xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                }
                                xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();


                                report.DataSource = ds;
                                report.CreateDocument();
                                PdfExportOptions options = new PdfExportOptions();
                                options.ShowPrintDialogOnOpen = true;

                                //Add page
                                xrTemp.Pages.AddRange(report.Pages);

                                xrTemp.ExportToPdf(stream);
                                return File(stream.GetBuffer(), "application/pdf", sTraineeFullname + "Evaluation" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
                            }
                            else
                            {
                                xrTemp.CreateDocument();


                                DataTable dtR = SystemFunction.LinqToDataTable(lstAns);
                                dsReportTraineeEvaForm ds = new dsReportTraineeEvaForm();
                                ds.dsMain.Merge(dtR);
                                ReportTraineeEvaForm report = new ReportTraineeEvaForm();
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
                                return File(breport, "application/excel", sTraineeFullname + "Evaluation" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");
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
        public ActionResult ExptraineeEvaFormList(string qryStr, string sMode)
        {
            var sCheck = acCheckLoginAndPermissionExport("TraineeEvaReportList", "TraineeEva");
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            string sMode2 = "pdf";

            if (qryStr != "" && !string.IsNullOrEmpty(sMode))
            {
                rpvEva_Session objSession = Session[sMode] as rpvEva_Session; //as List<ListAdvanceTransaction>;
                                                                              //List<ListAdvanceTransaction> lstData = Session[qryStr] as List<ListAdvanceTransaction>;
                if (objSession != null && objSession.lstData != null && objSession.lstData.Any())
                {
                    try
                    {
                        string[] aID = qryStr.Split(',');
                        if (aID != null && aID.Length > 0)
                        {
                            XrTemp xrTemp = new XrTemp();
                            var stream = new MemoryStream();
                            xrTemp.CreateDocument();
                            List<int> aNid = aID.Select(s => SystemFunction.GetIntNullToZero(s + "")).ToList();
                            var getList = objSession.lstData.Where(w => aNid.Contains(w.Id)).ToList();
                            //foreach (var xID in aID)
                            foreach (var xID in getList.OrderBy(o => o.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en))
                            {
                                //int nId = SystemFunction.GetIntNullToZero(xID + "");
                                int nId = xID.Id;
                                var _CheckData = xID;//objSession.lstData.Where(w => w.Id == nId).FirstOrDefault();
                                var _getDataEva = _TM_Trainee_EvaService.Find(nId);
                                if (_CheckData != null && _getDataEva != null && _getDataEva.TM_PR_Candidate_Mapping != null)
                                {
                                    List<string> lstUser = new List<string>();
                                    lstUser.Add(_getDataEva.req_incharge_Approve_user);
                                    lstUser.Add(_getDataEva.req_mgr_Approve_user);
                                    lstUser.Add(_getDataEva.acknowledge_user);
                                    var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();
                                    List<rpvEva_list_question> lstAns = new List<rpvEva_list_question>();
                                    int nNO = 1;
                                    if (_getDataEva.TM_Trainee_Eva_Answer != null && _getDataEva.TM_Trainee_Eva_Answer.Any(a => a.active_status == "Y"))
                                    {
                                        lstAns = _getDataEva.TM_Trainee_Eva_Answer.Where(w => w.active_status == "Y").Select(s => new rpvEva_list_question
                                        {

                                            seq = s.TM_Evaluation_Question.seq,
                                            no = nNO++,
                                            description = s.TM_Evaluation_Question.question + "",
                                            sgroup = s.TM_Evaluation_Question.header,
                                            comment = s.inchage_comment.TexttoReportnewline(48) + "",
                                            trainee_rating = s.trainee_rating != null ? s.trainee_rating.point + " : " + s.trainee_rating.rating_name_en + "" : "",
                                            eva_ratine = s.inchage_rating != null ? s.inchage_rating.point + " : " + s.inchage_rating.rating_name_en + "" : "",
                                        }).ToList();
                                    }
                                    string sTraineeFullname = _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "_" + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                                    //lstAns.ForEach(ed =>
                                    //{
                                    //    if (!string.IsNullOrEmpty(ed.comment))
                                    //    {

                                    //        List<string> newList = ed.comment.Split(' ').ToList();
                                    //        var sText = "";
                                    //        foreach (var item in newList)
                                    //        {
                                    //            if (item.Length > 40)
                                    //            {
                                    //                sText += (String.Join(" ", (item.SplitBy(40)).ToArray())) + " ";
                                    //            }
                                    //            else
                                    //            {
                                    //                sText += item + " ";
                                    //            }
                                    //        }


                                    //        ed.comment = sText;
                                    //    }
                                    //});
                                    DataTable dtR = SystemFunction.LinqToDataTable(lstAns);
                                    dsReportTraineeEvaForm ds = new dsReportTraineeEvaForm();
                                    ds.dsMain.Merge(dtR);
                                    ReportTraineeEvaForm report = new ReportTraineeEvaForm();
                                    //XRTableCell xrTraineeName = (XRTableCell)report.FindControl("xrTraineeName", true);
                                    //XRTableCell xrTraineeNo = (XRTableCell)report.FindControl("xrTraineeNo", true);
                                    //XRTableCell xrTraineeStart = (XRTableCell)report.FindControl("xrTraineeStart", true);
                                    //XRTableCell xrTraineeEnd = (XRTableCell)report.FindControl("xrTraineeEnd", true);
                                    //XRTableCell xrTraineeGroup = (XRTableCell)report.FindControl("xrTraineeGroup", true);
                                    //XRTableCell xrManager = (XRTableCell)report.FindControl("xrManager", true);
                                    //XRTableCell xrInCharge = (XRTableCell)report.FindControl("xrInCharge", true);
                                    //XRTableCell xrKeyCE = (XRTableCell)report.FindControl("xrKeyCE", true);
                                    XRLabel xrTraineeName = (XRLabel)report.FindControl("xrTraineeName", true);
                                    XRLabel xrTraineeNo = (XRLabel)report.FindControl("xrTraineeNo", true);
                                    XRLabel xrNickname = (XRLabel)report.FindControl("xrNickname", true);
                                    XRLabel xrTraineeStart = (XRLabel)report.FindControl("xrTraineeStart", true);
                                    XRLabel xrTraineeEnd = (XRLabel)report.FindControl("xrTraineeEnd", true);
                                    XRLabel xrTraineeGroup = (XRLabel)report.FindControl("xrTraineeGroup", true);
                                    XRLabel xrManager = (XRLabel)report.FindControl("xrManager", true);
                                    XRLabel xrInCharge = (XRLabel)report.FindControl("xrInCharge", true);
                                    XRLabel xrKeyCE = (XRLabel)report.FindControl("xrKeyCE", true);

                                    XRTableCell xrCellLeaned = (XRTableCell)report.FindControl("xrCellLeaned", true);
                                    XRTableCell xrCellWell = (XRTableCell)report.FindControl("xrCellWell", true);
                                    XRTableCell xrCellDevelop = (XRTableCell)report.FindControl("xrCellDevelop", true);
                                    XRTableCell xrCellHiring = (XRTableCell)report.FindControl("xrCellHiring", true);
                                    XRTableCell xrCellComment = (XRTableCell)report.FindControl("xrCellComment", true);
                                    XRTableCell xrTraineeProposed = (XRTableCell)report.FindControl("xrTraineeProposed", true);
                                    XRTableCell xrTraineeProposedDate = (XRTableCell)report.FindControl("xrTraineeProposedDate", true);
                                    XRTableCell xrEvaluatorby = (XRTableCell)report.FindControl("xrEvaluatorby", true);
                                    XRTableCell xrEvaluatorbyDate = (XRTableCell)report.FindControl("xrEvaluatorbyDate", true);
                                    XRPictureBox xrPicCheck = (XRPictureBox)report.FindControl("xrPicCheck", true);
                                    XRLabel xrlblPrintbt = (XRLabel)report.FindControl("xrlblPrintbt", true);

                                    xrTraineeName.Text = "Trainee Name: " + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                                    xrTraineeNo.Text = "Trainee No: " + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_TraineeNumber;
                                    xrNickname.Text = "Nickname: " + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.candidate_NickName + "";
                                    xrTraineeStart.Text = "Start Date: " + (_getDataEva.TM_PR_Candidate_Mapping.trainee_start.HasValue ? _getDataEva.TM_PR_Candidate_Mapping.trainee_start.Value.DateTimebyCulture() : "");
                                    xrTraineeEnd.Text = "End Date: " + (_getDataEva.TM_PR_Candidate_Mapping.trainee_end.HasValue ? _getDataEva.TM_PR_Candidate_Mapping.trainee_end.Value.DateTimebyCulture() : "");
                                    xrTraineeGroup.Text = "Group: " + _getDataEva.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en;
                                    xrManager.Text = "Performance Manager Name: " + _getUser.Where(w => w.EmpNo == _getDataEva.req_mgr_Approve_user).Select(s => s.EmpFullName).FirstOrDefault();
                                    xrInCharge.Text = "In-Charge / Engagement Manager Name: " + _getUser.Where(w => w.EmpNo == _getDataEva.req_incharge_Approve_user).Select(s => s.EmpFullName).FirstOrDefault();
                                    if (_getDataEva.approve_type + "" == "M")
                                    {
                                        xrKeyCE.Text = "Key Contact Evaluator Name: " + _getUser.Where(w => w.EmpNo == _getDataEva.req_mgr_Approve_user).Select(s => s.EmpFullName).FirstOrDefault();
                                    }
                                    else
                                    {
                                        xrKeyCE.Text = "Key Contact Evaluator Name: " + _getUser.Where(w => w.EmpNo == _getDataEva.req_incharge_Approve_user).Select(s => s.EmpFullName).FirstOrDefault();
                                    }

                                    xrCellLeaned.Text = _getDataEva.trainee_learned.TexttoReportnewline();
                                    xrCellWell.Text = _getDataEva.trainee_done_well.TexttoReportnewline();
                                    xrCellDevelop.Text = _getDataEva.trainee_developmental.TexttoReportnewline();
                                    //Check Hiring Form
                                    var _getActEva = _TM_Trainee_Eva_AnswerService.findByTraineeEvaId(nId); // for get info of eva form 

                                    var chkFormId = _getActEva.TM_Evaluation_Question.TM_Evaluation_Form.Id;
                                    //Add Code New Question 
                                    var _getEva = _TM_Evaluation_FormService.Find(chkFormId);
                                    if (_getEva.Id == 7)
                                    {

                                        xrCellHiring.Text = _getDataEva.TM_Trainee_HiringRating.Trainee_HiringRating_name_en;
                                    }
                                    else if (_getEva.Id != 7)
                                    {
                                        xrCellHiring.Text = _getDataEva.hiring_status + "" == "Y" ? "Yes" : "No";
                                    }
                                    xrCellComment.Text = _getDataEva.incharge_comments.TexttoReportnewline();
                                    xrTraineeProposed.Text = "Proposed by: " + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + _getDataEva.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "(Trainee)";
                                    xrTraineeProposedDate.Text = "Date: " + (_getDataEva.update_date.HasValue ? _getDataEva.update_date.Value.DateTimebyCulture() : "");
                                    //xrEvaluatorby.Text = "Evaluated by: ";
                                    if (_getDataEva.approve_type + "" == "M")
                                    {
                                        xrEvaluatorby.Text = "Evaluated by: " + _getUser.Where(w => w.EmpNo == _getDataEva.req_mgr_Approve_user).Select(s => s.EmpFullName).FirstOrDefault();
                                    }
                                    else
                                    {
                                        xrEvaluatorby.Text = "Evaluated by: " + _getUser.Where(w => w.EmpNo == _getDataEva.req_incharge_Approve_user).Select(s => s.EmpFullName).FirstOrDefault();
                                    }
                                    xrEvaluatorbyDate.Text = "Date: " + (_getDataEva.incharge_update_date.HasValue ? _getDataEva.incharge_update_date.Value.DateTimebyCulture() : "");
                                    if (_getDataEva.confidentiality_agreement + "" == "Y")
                                    {
                                        xrPicCheck.ImageUrl = "~\\Image\\check.png";
                                    }
                                    xrlblPrintbt.Text = "Printed by " + CGlobal.UserInfo.FullName + ",Date " + dNow.DateTimebyCulture();


                                    report.DataSource = ds;
                                    report.CreateDocument();
                                    PdfExportOptions options = new PdfExportOptions();
                                    options.ShowPrintDialogOnOpen = true;
                                    xrTemp.Pages.AddRange(report.Pages);
                                    xrTemp.PrintingSystem.ContinuousPageNumbering = false;
                                }
                            }
                            xrTemp.ExportToPdf(stream);
                            return File(stream.GetBuffer(), "application/pdf", "Trainee_Evaluation" + dNow.ToString("MMddyyyyHHmm") + ".pdf");
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

        public ActionResult ExptraineeEvaTracking(string qryStr, string sMode)
        {
            var sCheck = acCheckLoginAndPermissionExport("TraineeEvaReportList", "TraineeEva");
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            string sMode2 = "pdf";

            if (qryStr != "" && !string.IsNullOrEmpty(sMode))
            {
                rpvEva_Session objSession = Session[sMode] as rpvEva_Session; //as List<ListAdvanceTransaction>;
                                                                              //List<ListAdvanceTransaction> lstData = Session[qryStr] as List<ListAdvanceTransaction>;
                if (objSession != null && objSession.lstTraineeTracking != null && objSession.lstTraineeTracking.Any())
                {
                    try
                    {
                        string[] aID = qryStr.Split(',');
                        if (aID != null && aID.Length > 0)
                        {
                            int[] nID = aID.Select(s => SystemFunction.GetIntNullToZero(s + "")).ToArray();
                            nID = nID.Where(w => w != 0).ToArray();
                            XrTemp xrTemp = new XrTemp();
                            var stream = new MemoryStream();
                            xrTemp.CreateDocument();
                            var _getReport = objSession.lstTraineeTracking.Where(w => nID.Contains(w.Id)).ToList();
                            if (_getReport.Any())
                            {
                                try
                                {
                                    

                                    DataTable dtR = SystemFunction.LinqToDataTable(_getReport);
                                    dsTraineeTracking ds = new dsTraineeTracking();
                                    ds.dsMain.Merge(dtR);
                                    ReportTraineeListData report = new ReportTraineeListData();

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
                                    return File(breport, "application/excel", "TraineeListData" + dNow.ToString("MMddyyyyHHmm") + ".xlsx");

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