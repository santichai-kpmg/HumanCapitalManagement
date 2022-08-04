using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.PartnerEvaluation.PESMain;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.PartnerEvaluation;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.PESVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using static HumanCapitalManagement.App_Start.PESClass;

namespace HumanCapitalManagement.Controllers.PartnerEvaluation
{
    public class PTREvaluationController : BaseController
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
        private PTR_Evaluation_IncidentsService _PTR_Evaluation_IncidentsService;
        private TM_Feedback_RatingService _TM_Feedback_RatingService;
        private DivisionService _DivisionService;
        private PTR_Feedback_EmpService _PTR_Feedback_EmpService;
        private PTR_Feedback_UnitGroupService _PTR_Feedback_UnitGroupService;
        private PTR_Evaluation_Incidents_ScoreService _PTR_Evaluation_Incidents_ScoreService;
        private MailContentService _MailContentService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public PTREvaluationController(PTR_Evaluation_YearService PTR_Evaluation_YearService
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
            , MailContentService MailContentService
            , PTR_Evaluation_IncidentsService PTR_Evaluation_IncidentsService
            , PTR_Evaluation_Incidents_ScoreService PTR_Evaluation_Incidents_ScoreService)
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
            _MailContentService = MailContentService;
            _PTR_Evaluation_IncidentsService = PTR_Evaluation_IncidentsService;
            _PTR_Evaluation_Incidents_ScoreService = PTR_Evaluation_Incidents_ScoreService;
        }
        // GET: PTREvaluation

        public ActionResult PTREvaluationList(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPTREvaluation result = new vPTREvaluation();
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPTREvaluation SearchItem = (CSearchPTREvaluation)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPTREvaluation)));
                int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
                var lstData = _PTR_EvaluationService.GetPTREvaluationList(nYear, CGlobal.UserInfo.EmployeeNo, CGlobal.UserIsAdminPES());

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.fy_year = SearchItem.fy_year;

                    string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();

                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo) && w.Status == 3 || w.Status == 1).ToList();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser);
                    if (_getPartnerUser.Any())
                    {
                        string[] empNOActive = _getPartnerUser.Select(s => s.EmpNo).ToArray();
                        result.lstData = (from lstAD in lstData.Where(w => empNOActive.Contains(w.user_no))
                                          from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                          from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                          select new vPTREvaluation_obj
                                          {
                                              srank = lstEmpReq.RankCode,
                                              sgroup = lstEmpReq.UnitGroupName,
                                              name_en = lstAD.user_no + " : " + lstEmpReq.EmpFullName,
                                              fy_year = lstAD.PTR_Evaluation_Year.evaluation_year.HasValue ? lstAD.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                              active_status = lstAD.TM_PTR_Eva_Status != null ? lstAD.TM_PTR_Eva_Status.status_name_en : "", //lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                              create_user = lstAD.create_user,
                                              create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                              update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                              name = lstEmpReq.EmpFullName,
                                          }).ToList();
                    }
                }
            }
            else
            {
                //var _getYear = _PTR_Evaluation_YearService.FindNowYear();
                //if (_getYear != null)
                //{
                //    result.fy_year = _getYear.Id + "";
                //}
            }
            #endregion
            return View(result);
        }
        public ActionResult PTREvaluationEdit(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code
            bool bCEONeedEdit = true;
            vPTREvaluation_obj_save result = new vPTREvaluation_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                if (nId != 0)
                {
                    List<int> lstStatusPlan = new List<int>();
                    lstStatusPlan.Add((int)Eva_Status.Draft_Plan);
                    lstStatusPlan.Add((int)Eva_Status.Waiting_for_Planning_Approval);
                    lstStatusPlan.Add((int)Eva_Status.Waiting_for_Revised_Plan);
                    lstStatusPlan.Add((int)Eva_Status.Planning_Completed);

                    List<int> lstStatusEva = new List<int>();
                    lstStatusEva.Add((int)Eva_Status.Draft_Evaluate);
                    lstStatusEva.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
                    lstStatusEva.Add((int)Eva_Status.Waiting_for_Revised_Evaluate);
                    lstStatusEva.Add((int)Eva_Status.Evaluation_Completed);


                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                        {
                            var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                            if (_checkActiv != null)
                            {
                                var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                                result.IdEncrypt = qryStr;
                                result.code = _getData.user_no;
                                result.sname = _checkActiv.EmpFullName;
                                result.sgroup = _checkActiv.UnitGroupName;
                                result.srank = _checkActiv.Rank;
                                result.status_name = _getData.TM_PTR_Eva_Status != null ? _getData.TM_PTR_Eva_Status.status_name_en + "" : "";
                                result.status_id = _getData.TM_PTR_Eva_Status != null ? _getData.TM_PTR_Eva_Status.Id + "" : "";
                                result.yearcurrent = _getData.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy");
                                result.other_role = _getData.other_roles;
                                if (_getData.PTR_Evaluation_File != null && _getData.PTR_Evaluation_File.Any(a => a.active_status == "Y"))
                                {
                                    int nFile = 1;

                                    if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019)
                                    {
                                        result.lstFile = _getData.PTR_Evaluation_File.Where(a => a.active_status == "Y").Select(s => new vPTREvaluation_lst_File
                                        {
                                            file_name = s.sfile_oldname,
                                            description = s.description,
                                            Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                            View = @"<button id=""btnEdit""  type=""button"" onclick=""LoadFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-success""><i class=""glyphicon glyphicon-download-alt""></i></button>",
                                        }).ToList();
                                    }
                                    else
                                    {
                                        result.lstFile = _getData.PTR_Evaluation_File.Where(a => a.active_status == "Y").Select(s => new vPTREvaluation_lst_File
                                        {
                                            file_name = s.sfile_oldname,
                                            description = s.description,
                                            Edit = (_getData.TM_PTR_Eva_Status.Id != (int)Eva_Status.Draft_Evaluate && _getData.TM_PTR_Eva_Status.Id != (int)Eva_Status.Waiting_for_Revised_Evaluate) ? "" + nFile++ : @"<button id=""btnEdit""  type=""button"" onclick=""DeleteFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                            View = @"<button id=""btnEdit""  type=""button"" onclick=""LoadFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-success""><i class=""glyphicon glyphicon-download-alt""></i></button>",
                                        }).ToList();
                                    }


                                }
                                #region if else Data
                                if (lstStatusPlan.Contains(_getData.TM_PTR_Eva_Status.Id))
                                {
                                    #region Plaining Data
                                    result.eva_mode = "Plan";
                                    if (_getData.PTR_Evaluation_KPIs != null && _getData.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                                    {
                                        result.lstKPIs = _getData.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").Select(s => new vPTREvaluation_KPIs
                                        {
                                            sname = s.TM_KPIs_Base.kpi_name_en,
                                            target_data = s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : (s.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(s.target_max))) : HCMFunc.DataDecryptPES(s.target_max)),
                                            // target = lst.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lst.target) + "-" + HCMFunc.DataDecryptPES(lst.target_max) + "%" : (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max))) : HCMFunc.DataDecryptPES(lst.target_max)),
                                            howto = HCMFunc.DataDecryptPES(s.how_to),
                                            sdisable = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Plan || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Plan) ? "N" : "Y",
                                            IdEncrypt = s.Id + "",
                                            stype = s.TM_KPIs_Base.type_of_kpi,
                                        }).ToList();

                                    }
                                    if (_getData.PTR_Evaluation_Approve != null && _getData.PTR_Evaluation_Approve.Any(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "P"))
                                    {
                                        string[] empNO = _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "P").Select(s => s.Req_Approve_user).ToArray();
                                        var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                        result.lstApprove = (from lst in _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "P")
                                                             from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                             select new vPTREvaluation_lst_approve
                                                             {
                                                                 step_name = lst.TM_PTR_Eva_ApproveStep.Step_name_en,
                                                                 app_name = lstEmpReq.EmpFullName,
                                                                 nStep = lst.TM_PTR_Eva_ApproveStep.seq.HasValue ? lst.TM_PTR_Eva_ApproveStep.seq.Value : 0,
                                                                 approve_date = lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture() : "",
                                                             }).ToList();
                                    }

                                    if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null)
                                    {
                                        result.lstAnswer = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.active_status == "Y" && w.questions_type == "P")
                                                            select new vPTREvaluation_Answer
                                                            {
                                                                nCID = lstQ.nCId,
                                                                id = lstQ.Id + "",
                                                                question = lstQ.question,
                                                                header = lstQ.header,
                                                                nSeq = lstQ.seq,
                                                                remark = "",
                                                                sgroup = lstQ.qgroup + "",
                                                                sdisable = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Plan || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Plan) ? "N" : "Y",
                                                            }).ToList();
                                        if (_getData.PTR_Evaluation_Answer != null && _getData.PTR_Evaluation_Answer.Any(a => a.active_status == "Y"))
                                        {
                                            result.lstAnswer.ForEach(ed =>
                                            {
                                                var GetAns = _getData.PTR_Evaluation_Answer.Where(w => w.TM_PTR_Eva_Questions.Id + "" == ed.id).FirstOrDefault();
                                                if (GetAns != null)
                                                {
                                                    ed.remark = HCMFunc.DataDecryptPES(GetAns.answer + "");
                                                }
                                            });
                                        }
                                    }
                                    if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null)
                                    {
                                        result.lstAnswer = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.active_status == "Y" && w.questions_type == "P")
                                                            select new vPTREvaluation_Answer
                                                            {
                                                                id = lstQ.Id + "",
                                                                question = lstQ.question,
                                                                header = lstQ.header,
                                                                nSeq = lstQ.seq,
                                                                remark = "",
                                                                sgroup = lstQ.qgroup + "",
                                                                sdisable = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Plan || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Plan) ? "N" : "Y",
                                                            }).ToList();
                                        if (_getData.PTR_Evaluation_Answer != null && _getData.PTR_Evaluation_Answer.Any(a => a.active_status == "Y"))
                                        {
                                            result.lstAnswer.ForEach(ed =>
                                            {
                                                var GetAns = _getData.PTR_Evaluation_Answer.Where(w => w.TM_PTR_Eva_Questions.Id + "" == ed.id).FirstOrDefault();
                                                if (GetAns != null)
                                                {
                                                    ed.remark = HCMFunc.DataDecryptPES(GetAns.answer + "");
                                                }
                                            });
                                        }
                                    }
                                    #endregion

                                }
                                else
                                {

                                    if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019)
                                    {
                                        bCEONeedEdit = true;
                                    }


                                    result.lstFeedback = new List<vPTREvaluation_Feedback>();
                                    #region Eva Data 
                                    result.eva_mode = "Eva";
                                    if (_getData.PTR_Evaluation_Approve != null && _getData.PTR_Evaluation_Approve.Any(a => a.active_status == "Y"))
                                    {
                                        var _getRate = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self).Select(s => s.Annual_Rating != null ? s.Annual_Rating : null).FirstOrDefault();
                                        if (_getRate != null)
                                        {
                                            result.self_rating_id = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? _getRate.Id + "" : _getRate.rating_name_en;
                                        }

                                    }

                                    if (_getData.PTR_Evaluation_Year.evaluation_year.HasValue)
                                    {

                                        int nCurrent = _getData.PTR_Evaluation_Year.evaluation_year.Value.Year;
                                        result.yearone = (_getData.PTR_Evaluation_Year.evaluation_year.Value.AddYears(-1)).DateTimebyCulture("yyyy");
                                        result.yeartwo = (_getData.PTR_Evaluation_Year.evaluation_year.Value.AddYears(-2)).DateTimebyCulture("yyyy");
                                        result.yearthree = (_getData.PTR_Evaluation_Year.evaluation_year.Value.AddYears(-3)).DateTimebyCulture("yyyy");

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

                                        result.lstKPIs = (from lst in _getData.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y")
                                                          from lstKPIOne in lstKPI1.Where(w => w.TM_KPIs_Base.Id == lst.TM_KPIs_Base.Id).DefaultIfEmpty(new PTR_Evaluation_KPIs())
                                                          from lstKPITwo in lstKPI2.Where(w => w.TM_KPIs_Base.Id == lst.TM_KPIs_Base.Id).DefaultIfEmpty(new PTR_Evaluation_KPIs())
                                                          from lstKPIThree in lstKPI3.Where(w => w.TM_KPIs_Base.Id == lst.TM_KPIs_Base.Id).DefaultIfEmpty(new PTR_Evaluation_KPIs())
                                                          select new vPTREvaluation_KPIs
                                                          {
                                                              sname = lst.TM_KPIs_Base.kpi_name_en,
                                                              target_data =
                                                              lst.TM_KPIs_Base.type_of_kpi == "P"
                                                              ? SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target)).NodecimalFormatHasvalue() + "-" + SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)).NodecimalFormatHasvalue() + "%"
                                                              : (lst.TM_KPIs_Base.type_of_kpi == "N"
                                                              ? SystemFunction.GetNumberNullToZero(FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)))).NodecimalFormatHasvalue()
                                                              : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)).NodecimalFormatHasvalue()),

                                                              remark = HCMFunc.DataDecryptPES(lst.final_remark + ""),
                                                              actual = lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.actual)).NodecimalFormatHasvalue() + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),
                                                              yearone = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.actual))) : (SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.actual)).NodecimalFormatHasvalue())) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIOne.actual),
                                                              yeartwo = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.actual))) : (SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.actual)).NodecimalFormatHasvalue())) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPITwo.actual),
                                                              yearthree = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIThree.actual),

                                                              group_yearone = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.group_actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIOne.actual),
                                                              group_yeartwo = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.group_actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPITwo.actual),
                                                              group_yearthree = (lst.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.group_actual))) : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.group_actual)).NodecimalFormatHasvalue()) + (lst.TM_KPIs_Base.type_of_kpi == "P" ? "%" : ""),//HCMFunc.DataDecryptPES(lstKPIThree.actual),

                                                              sdisable = (bCEONeedEdit || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? "N" : "Y",
                                                              IdEncrypt = lst.Id + "",
                                                              stype = lst.TM_KPIs_Base.type_of_kpi,
                                                              target_group =
                                                              lst.TM_KPIs_Base.type_of_kpi == "P"
                                                              ? SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target)).NodecimalFormatHasvalue() + "-" + SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max)).NodecimalFormatHasvalue() + "%"
                                                              : (lst.TM_KPIs_Base.type_of_kpi == "N"
                                                              ? SystemFunction.GetNumberNullToZero(FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max)))).NodecimalFormatHasvalue()
                                                              : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max)).NodecimalFormatHasvalue()),

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

                                    if (_getEvaQuestion != null)
                                    {
                                        if (_getEvaQuestion.TM_PTR_Eva_Questions != null)
                                        {
                                            if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2020)
                                            {

                                                result.lstAnswer = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions
                                                                    select new vPTREvaluation_Answer
                                                                    {
                                                                        nCID = lstQ.nCId,
                                                                        id = lstQ.Id + "",
                                                                        question = lstQ.question,
                                                                        header = lstQ.header,
                                                                        nSeq = lstQ.seq,
                                                                        remark = "",
                                                                        sgroup = lstQ.qgroup + "",
                                                                        sdisable = ((bCEONeedEdit || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) && lstQ.questions_type == "G") ? "N" : "Y",
                                                                    }).ToList();
                                                if (_getData.PTR_Evaluation_Answer != null && _getData.PTR_Evaluation_Answer.Any(a => a.active_status == "Y"))
                                                {
                                                    result.lstAnswer.ForEach(ed =>
                                                    {
                                                        var GetAns = _getData.PTR_Evaluation_Answer.Where(w => w.TM_PTR_Eva_Questions.Id + "" == ed.id).FirstOrDefault();
                                                        if (GetAns != null)
                                                        {
                                                            ed.remark = HCMFunc.DataDecryptPES(GetAns.answer + "");
                                                        }
                                                    });
                                                }
                                            }
                                            else if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019)
                                            {

                                                result.lstAnswer = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions
                                                                    select new vPTREvaluation_Answer
                                                                    {
                                                                        nCID = lstQ.nCId,
                                                                        id = lstQ.Id + "",
                                                                        question = lstQ.question,
                                                                        header = lstQ.header,
                                                                        nSeq = lstQ.seq,
                                                                        remark = "",
                                                                        sgroup = lstQ.qgroup + "",
                                                                        sdisable = ((bCEONeedEdit || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) && lstQ.questions_type == "G") ? "N" : "Y",
                                                                    }).ToList();
                                                if (_getData.PTR_Evaluation_Answer != null && _getData.PTR_Evaluation_Answer.Any(a => a.active_status == "Y"))
                                                {
                                                    result.lstAnswer.ForEach(ed =>
                                                    {
                                                        var GetAns = _getData.PTR_Evaluation_Answer.Where(w => w.TM_PTR_Eva_Questions.Id + "" == ed.id).FirstOrDefault();
                                                        if (GetAns != null)
                                                        {
                                                            ed.remark = HCMFunc.DataDecryptPES(GetAns.answer + "");
                                                        }
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                result.lstAnswer = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.questions_type == "G")
                                                                    select new vPTREvaluation_Answer
                                                                    {
                                                                        nCID = lstQ.nCId,
                                                                        id = lstQ.Id + "",
                                                                        question = lstQ.question,
                                                                        header = lstQ.header,
                                                                        nSeq = lstQ.seq,
                                                                        remark = "",
                                                                        sgroup = lstQ.qgroup + "",
                                                                        sdisable = ((bCEONeedEdit || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) && lstQ.questions_type == "G") ? "N" : "Y",
                                                                    }).ToList();
                                                if (_getData.PTR_Evaluation_Answer != null && _getData.PTR_Evaluation_Answer.Any(a => a.active_status == "Y"))
                                                {
                                                    result.lstAnswer.ForEach(ed =>
                                                    {
                                                        var GetAns = _getData.PTR_Evaluation_Answer.Where(w => w.TM_PTR_Eva_Questions.Id + "" == ed.id).FirstOrDefault();
                                                        if (GetAns != null)
                                                        {
                                                            ed.remark = HCMFunc.DataDecryptPES(GetAns.answer + "");
                                                        }
                                                    });
                                                }
                                            }
                                        }


                                        if (_getEvaQuestion.TM_PTR_Eva_Incidents != null)
                                        {
                                            if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2020)
                                            {
                                                result.lstIncidents = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents
                                                                       select new vPTREvaluation_Answer
                                                                       {
                                                                           id = lstQ.Id + "",
                                                                           question = lstQ.question,
                                                                           header = lstQ.header,
                                                                           nSeq = lstQ.seq,
                                                                           remark = "",
                                                                           sgroup = lstQ.qgroup + "",
                                                                           sdisable = ((bCEONeedEdit || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate)) ? "N" : "Y",
                                                                       }).ToList();

                                                //result.lstIncidents_score = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents
                                                //                             select new vPTREvaluation_Incidents_Score
                                                //                             {
                                                //                                 id = lstQ.Id + "",
                                                //                                 question = lstQ.question,
                                                //                                 header = lstQ.header,
                                                //                                 nSeq = lstQ.seq,
                                                //                                 sgroup = lstQ.qgroup + "",
                                                //                                 Excellent = "N",
                                                //                                 High = "N",
                                                //                                 NI = "N",
                                                //                                 Low = "N",
                                                //                                 nscore = lstQ.nscore + "",
                                                //                                 sdisable = ((_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate)) ? "N" : "Y",
                                                //                             }).ToList();
                                                if (_getData.PTR_Evaluation_Incidents != null && _getData.PTR_Evaluation_Incidents.Any(a => a.active_status == "Y"))
                                                {
                                                    result.lstIncidents.ForEach(ed =>
                                                    {
                                                        var GetAns = _getData.PTR_Evaluation_Incidents.Where(w => w.TM_PTR_Eva_Incidents.Id + "" == ed.id).FirstOrDefault();
                                                        if (GetAns != null)
                                                        {
                                                            ed.remark = HCMFunc.DataDecryptPES(GetAns.answer + "");
                                                        }
                                                    });
                                                    //result.lstIncidents_score.ForEach(ed =>
                                                    //{
                                                    //    var GetAns = _getData.PTR_Evaluation_Incidents.Where(w => w.TM_PTR_Eva_Incidents.Id + "" == ed.id).FirstOrDefault();
                                                    //    if (GetAns != null && GetAns.TM_PTR_Eva_Incidents_Score_Id.HasValue)
                                                    //    {
                                                    //        if (GetAns.TM_PTR_Eva_Incidents_Score_Id == (int)PESClass.Incidents_Score.Excellent)
                                                    //        {
                                                    //            ed.Excellent = "Y";
                                                    //        }
                                                    //        else if (GetAns.TM_PTR_Eva_Incidents_Score_Id == (int)PESClass.Incidents_Score.High)
                                                    //        {
                                                    //            ed.High = "Y";
                                                    //        }
                                                    //        else if (GetAns.TM_PTR_Eva_Incidents_Score_Id == (int)PESClass.Incidents_Score.Low)
                                                    //        {
                                                    //            ed.Low = "Y";
                                                    //        }
                                                    //        else if (GetAns.TM_PTR_Eva_Incidents_Score_Id == (int)PESClass.Incidents_Score.NI)
                                                    //        {
                                                    //            ed.NI = "Y";
                                                    //        }
                                                    //    }
                                                    //});
                                                }
                                            }
                                            else if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019)
                                            {
                                                result.lstIncidents = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents
                                                                       select new vPTREvaluation_Answer
                                                                       {
                                                                           id = lstQ.Id + "",
                                                                           question = lstQ.question,
                                                                           header = lstQ.header,
                                                                           nSeq = lstQ.seq,
                                                                           remark = "",
                                                                           sgroup = lstQ.qgroup + "",
                                                                           sdisable = ((bCEONeedEdit || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate)) ? "N" : "Y",
                                                                       }).ToList();

                                                //result.lstIncidents_score = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents
                                                //                             select new vPTREvaluation_Incidents_Score
                                                //                             {
                                                //                                 id = lstQ.Id + "",
                                                //                                 question = lstQ.question,
                                                //                                 header = lstQ.header,
                                                //                                 nSeq = lstQ.seq,
                                                //                                 sgroup = lstQ.qgroup + "",
                                                //                                 Excellent = "N",
                                                //                                 High = "N",
                                                //                                 NI = "N",
                                                //                                 Low = "N",
                                                //                                 nscore = lstQ.nscore + "",
                                                //                                 sdisable = ((_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate)) ? "N" : "Y",
                                                //                             }).ToList();
                                                if (_getData.PTR_Evaluation_Incidents != null && _getData.PTR_Evaluation_Incidents.Any(a => a.active_status == "Y"))
                                                {
                                                    result.lstIncidents.ForEach(ed =>
                                                    {
                                                        var GetAns = _getData.PTR_Evaluation_Incidents.Where(w => w.TM_PTR_Eva_Incidents.Id + "" == ed.id).FirstOrDefault();
                                                        if (GetAns != null)
                                                        {
                                                            ed.remark = HCMFunc.DataDecryptPES(GetAns.answer + "");
                                                        }
                                                    });
                                                    //result.lstIncidents_score.ForEach(ed =>
                                                    //{
                                                    //    var GetAns = _getData.PTR_Evaluation_Incidents.Where(w => w.TM_PTR_Eva_Incidents.Id + "" == ed.id).FirstOrDefault();
                                                    //    if (GetAns != null && GetAns.TM_PTR_Eva_Incidents_Score_Id.HasValue)
                                                    //    {
                                                    //        if (GetAns.TM_PTR_Eva_Incidents_Score_Id == (int)PESClass.Incidents_Score.Excellent)
                                                    //        {
                                                    //            ed.Excellent = "Y";
                                                    //        }
                                                    //        else if (GetAns.TM_PTR_Eva_Incidents_Score_Id == (int)PESClass.Incidents_Score.High)
                                                    //        {
                                                    //            ed.High = "Y";
                                                    //        }
                                                    //        else if (GetAns.TM_PTR_Eva_Incidents_Score_Id == (int)PESClass.Incidents_Score.Low)
                                                    //        {
                                                    //            ed.Low = "Y";
                                                    //        }
                                                    //        else if (GetAns.TM_PTR_Eva_Incidents_Score_Id == (int)PESClass.Incidents_Score.NI)
                                                    //        {
                                                    //            ed.NI = "Y";
                                                    //        }
                                                    //    }
                                                    //});
                                                }
                                            }
                                        }


                                    }

                                    if (_getData.PTR_Evaluation_Approve != null && _getData.PTR_Evaluation_Approve.Any(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G"))
                                    {
                                        string[] empNO = _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G").Select(s => s.Req_Approve_user).ToArray();
                                        var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                        result.lstApprove = (from lst in _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G")
                                                             from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                             select new vPTREvaluation_lst_approve
                                                             {
                                                                 step_name = lst.TM_PTR_Eva_ApproveStep.Step_name_en,
                                                                 app_name = lstEmpReq.EmpFullName,
                                                                 nStep = lst.TM_PTR_Eva_ApproveStep.seq.HasValue ? lst.TM_PTR_Eva_ApproveStep.seq.Value : 0,
                                                                 approve_date = lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture() : "",

                                                             }).ToList();

                                        //result.lstFinal = (from lst in _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G")

                                        //                   select new vPTREvaluation_Final
                                        //                   {
                                        //                       position = lst.TM_PTR_Eva_ApproveStep.Step_name_en,
                                        //                       status = lst.Annual_Rating != null ? lst.Annual_Rating.rating_name_en : "",
                                        //                       nSeq = lst.TM_PTR_Eva_ApproveStep.seq,
                                        //                       remark = HCMFunc.DataDecryptPES(lst.responses),

                                        //                   }).ToList();

                                        if (_getData.user_no == CGlobal.UserInfo.EmployeeNo && _getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2020)
                                        {
                                            if (_getData.TM_PTR_Eva_Status.Id == (int)PESClass.Eva_Status.Evaluation_Completed)
                                            {
                                                result.lstFinal = (from lst in _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G")
                                                                   select new vPTREvaluation_Final
                                                                   {
                                                                       position = lst.TM_PTR_Eva_ApproveStep.Step_name_en,
                                                                       //status = lst.Annual_Rating != null ? ((lst.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo || lst.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Self) ? lst.Annual_Rating.rating_name_en : "") : "",
                                                                       status = lst.Annual_Rating != null ? lst.Annual_Rating.rating_name_en : "",
                                                                       nSeq = lst.TM_PTR_Eva_ApproveStep.seq,
                                                                       remark = HCMFunc.DataDecryptPES(lst.responses),
                                                                   }).ToList();
                                            }

                                        }
                                        else if (_getData.user_no == CGlobal.UserInfo.EmployeeNo && _getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019)
                                        {
                                            if (_getData.TM_PTR_Eva_Status.Id == (int)PESClass.Eva_Status.Evaluation_Completed)
                                            {
                                                result.lstFinal = (from lst in _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G")
                                                                   select new vPTREvaluation_Final
                                                                   {
                                                                       position = lst.TM_PTR_Eva_ApproveStep.Step_name_en,
                                                                       //status = lst.Annual_Rating != null ? ((lst.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo || lst.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Self) ? lst.Annual_Rating.rating_name_en : "") : "",
                                                                       status = lst.Annual_Rating != null ? lst.Annual_Rating.rating_name_en : "",
                                                                       nSeq = lst.TM_PTR_Eva_ApproveStep.seq,
                                                                       remark = HCMFunc.DataDecryptPES(lst.responses),
                                                                   }).ToList();
                                            }

                                        }
                                        else
                                        {
                                            if (_getData.TM_PTR_Eva_Status.Id == (int)PESClass.Eva_Status.Evaluation_Completed)
                                            {
                                                result.lstFinal = (from lst in _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G")
                                                                   select new vPTREvaluation_Final
                                                                   {
                                                                       position = lst.TM_PTR_Eva_ApproveStep.Step_name_en,
                                                                       status = lst.Annual_Rating != null ? lst.Annual_Rating.rating_name_en : "",
                                                                       nSeq = lst.TM_PTR_Eva_ApproveStep.seq,
                                                                       remark = HCMFunc.DataDecryptPES(lst.responses),
                                                                   }).ToList();
                                            }
                                        }


                                        var _getApprovalforIncident = _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G" && a.TM_PTR_Eva_ApproveStep.Id != (int)StepApproveEvaluate.Self).ToList();
                                        if (_getEvaQuestion.TM_PTR_Eva_Incidents != null && _getApprovalforIncident.Any())
                                        {
                                            result.lstIncidents_score = new List<vPTREvaluation_Incidents_Score>();
                                            List<vPTREvaluation_Incidents_Score> lstInScore = new List<vPTREvaluation_Incidents_Score>();
                                            var GetRatescore = _PTR_Evaluation_Incidents_ScoreService.GetScored(_getApprovalforIncident.Select(s => s.Id).ToArray(), true).ToList();
                                            foreach (var item in _getApprovalforIncident)
                                            {
                                                lstInScore.AddRange(
                                                    (from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents
                                                     select new vPTREvaluation_Incidents_Score
                                                     {

                                                         nstep = item.TM_PTR_Eva_ApproveStep != null ? item.TM_PTR_Eva_ApproveStep.seq : 0,
                                                         id = lstQ.Id + "",
                                                         question = lstQ.question,
                                                         header = lstQ.header,
                                                         nSeq = lstQ.seq,
                                                         sgroup = item.TM_PTR_Eva_ApproveStep != null ? item.TM_PTR_Eva_ApproveStep.Step_name_en + "" : "",
                                                         sExcellent = "N",
                                                         sHigh = "N",
                                                         sNI = "N",
                                                         sMeet = "N",
                                                         sLow = "N",
                                                         nscore = lstQ.nscore + "",
                                                         isCurrent = "N",
                                                         nrate = GetRatescore.Where(w => w.active_status == "Y" && w.TM_PTR_Eva_Incidents_Id == lstQ.Id && w.PTR_Evaluation_Approve_Id == item.Id).Select(s => s.TM_PTR_Eva_Incidents_Score_Id).FirstOrDefault(),
                                                         sdisable = ((bCEONeedEdit || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate)) ? "N" : "Y",
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

                                            result.lstIncidents_score = lstInScore.ToList();
                                        }


                                    }


                                    if (_getData.PTR_Feedback_Emp != null && _getData.PTR_Feedback_Emp.Any(w => w.active_status == "Y"))
                                    {
                                        string[] aNo = _getData.PTR_Feedback_Emp.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                                        var _getPartnerUser = dbHr.AllInfo_WS.Where(w => aNo.Contains(w.EmpNo)).ToList();

                                        result.lstFeedback.AddRange((from lstAD in _getData.PTR_Feedback_Emp.Where(w => w.active_status == "Y")
                                                                     from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                                     select new vPTREvaluation_Feedback
                                                                     {
                                                                         sname = lstEmpReq.EmpFullName,
                                                                         rating = lstAD.TM_Feedback_Rating.feedback_rating_name_en,
                                                                         appreciations = HCMFunc.DataDecryptPES(lstAD.appreciations + ""),
                                                                         recommendations = HCMFunc.DataDecryptPES(lstAD.recommendations + ""),
                                                                         Edit = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? @"<button id=""btnEdit""  type=""button"" onclick=""DelectFeedEmp('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>" : "",
                                                                         sgroup = "Partner/Directors/ADs",
                                                                     }).ToList());
                                    }
                                    if (_getData.PTR_Feedback_UnitGroup != null && _getData.PTR_Feedback_UnitGroup.Any(w => w.active_status == "Y"))
                                    {
                                        var _getUnit = _DivisionService.GetForFeedback();
                                        result.lstFeedback.AddRange((from lstAD in _getData.PTR_Feedback_UnitGroup.Where(w => w.active_status == "Y")
                                                                     from lstEmpReq in _getUnit.Where(w => w.division_code == lstAD.unitcode).DefaultIfEmpty(new Models.Common.TM_Divisions())
                                                                     select new vPTREvaluation_Feedback
                                                                     {
                                                                         sname = lstEmpReq.division_name_en,
                                                                         rating = lstAD.TM_Feedback_Rating.feedback_rating_name_en,
                                                                         appreciations = HCMFunc.DataDecryptPES(lstAD.appreciations + ""),
                                                                         recommendations = HCMFunc.DataDecryptPES(lstAD.recommendations + ""),
                                                                         Edit = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? @"<button id=""btnEdit""  type=""button"" onclick=""DelectFeedUnit('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>" : "",
                                                                         sgroup = "Unit Name",
                                                                     }).ToList());
                                    }
                                    #endregion

                                }
                                #endregion




                                if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2018)
                                {
                                    return View(result);
                                }
                                else if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019)
                                {
                                    return View("PTREvaluationEdit19", result);
                                }
                                else if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2020)
                                {
                                    return View("PTREvaluationEdit20", result);
                                }
                                else
                                {
                                    return View(result);
                                }

                            }
                            else
                            {
                                return RedirectToAction("PTREvaluationList", "PTREvaluation");
                            }
                        }
                        else
                        {
                            return RedirectToAction("ErrorNopermission", "MasterPage");
                        }

                    }
                    else
                    {
                        return RedirectToAction("PTREvaluationList", "PTREvaluation");
                    }
                }
                else
                {
                    return RedirectToAction("PTREvaluationList", "PTREvaluation");
                }
            }
            else
            {
                return RedirectToAction("PTREvaluationList", "PTREvaluation");
            }
            #endregion
        }
        public ActionResult PTREvaluationApprove(string id)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            return View();
        }
        public ActionResult CreatPTREvaluationFile(string id)
        {
            var sCheck = pesCheckLoginAndPermission(true);
            if (sCheck != null)
            {
                return sCheck;
            }
            Stream stream = new MemoryStream();
            if (!string.IsNullOrEmpty(id))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(id + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_Evaluation_FileService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.PTR_Evaluation != null && _getData.PTR_Evaluation.PTR_Evaluation_Approve != null)
                        {
                            //if (_getData.PTR_Evaluation.PTR_Evaluation_Approve.Any(a => a.Req_Approve_user.Contains(CGlobal.UserInfo.EmployeeNo)) || CGlobal.UserIsAdminPES())
                            //{
                            if (_getData.sfileType == ".docx" || _getData.sfileType == ".doc")
                            {
                                return File(_getData.sfile64, "application/octet-stream", _getData.sfile_oldname /*+ _getData.sfileType*/);
                            }
                            else if (_getData.sfileType == ".pdf")
                            {
                                return File(_getData.sfile64, "application/pdf", _getData.sfile_oldname /*+ _getData.sfileType*/);
                            }
                            //}
                            //else
                            //{
                            //    return JavaScript("CloseTab();");
                            //}
                        }
                        else
                        {
                            return JavaScript("CloseTab();");
                        }

                        //stream = new MemoryStream(_getData.sfile64);
                        //// Get content of your Excel file
                        ////var stream = wBook.WriteXLSX(); // Return a MemoryStream (Using DTG.Spreadsheet)

                        //stream.Seek(0, SeekOrigin.Begin);
                    }
                }
            }
            return JavaScript("CloseTab();");
        }
        #region Performance Plan
        public ActionResult PTRPersonalPlanList(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPTREvaluation result = new vPTREvaluation();
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPTREvaluation SearchItem = (CSearchPTREvaluation)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPTREvaluation)));
                int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
                var lstData = _PTR_EvaluationService.GetPTRPerformancePlanList(nYear, CGlobal.UserInfo.EmployeeNo, CGlobal.UserIsAdminPES());

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.fy_year = SearchItem.fy_year;
                    string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo) && w.Status == 3 || w.Status == 1).ToList();

                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser);
                    if (_getPartnerUser.Any())
                    {
                        string[] empNOActive = _getPartnerUser.Select(s => s.EmpNo).ToArray();
                        result.lstData = (from lstAD in lstData.Where(w => empNOActive.Contains(w.user_no))
                                          from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                          from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                          select new vPTREvaluation_obj
                                          {
                                              srank = lstEmpReq.RankCode,
                                              sgroup = lstEmpReq.UnitGroupName,
                                              name_en = lstAD.user_no + " : " + lstEmpReq.EmpFullName,
                                              fy_year = lstAD.PTR_Evaluation_Year.evaluation_year.HasValue ? lstAD.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                              active_status = lstAD.TM_PTR_Eva_Status != null ? lstAD.TM_PTR_Eva_Status.status_name_en : "", //lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                              create_user = lstAD.create_user,
                                              create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                              update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                              name = lstEmpReq.EmpFullName,
                                          }).ToList();
                    }

                }
            }
            else
            {
                //var _getYear = _PTR_Evaluation_YearService.FindNowYear();
                //if (_getYear != null)
                //{
                //    result.fy_year = _getYear.Id + "";
                //}
            }
            #endregion
            return View(result);
        }
        public ActionResult PTRPersonalPlanEdit(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vPTREvaluation_obj_save result = new vPTREvaluation_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                if (nId != 0)
                {
                    List<int> lstStatusPlan = new List<int>();
                    lstStatusPlan.Add((int)Eva_Status.Draft_Plan);
                    lstStatusPlan.Add((int)Eva_Status.Waiting_for_Planning_Approval);
                    lstStatusPlan.Add((int)Eva_Status.Waiting_for_Revised_Plan);
                    lstStatusPlan.Add((int)Eva_Status.Planning_Completed);

                    List<int> lstQuesionGold = new List<int>();
                    lstQuesionGold.Add((int)Evaluation_Questions.Development_Goal_stretch);
                    lstQuesionGold.Add((int)Evaluation_Questions.Development_Goal_Achievements);
                    lstQuesionGold.Add((int)Evaluation_Questions.Development_Goal_Plan);


                    List<int> lstStatusEva = new List<int>();
                    lstStatusEva.Add((int)Eva_Status.Draft_Evaluate);
                    lstStatusEva.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
                    lstStatusEva.Add((int)Eva_Status.Waiting_for_Revised_Evaluate);
                    lstStatusEva.Add((int)Eva_Status.Evaluation_Completed);


                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                        {
                            var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                            if (_checkActiv != null)
                            {
                                var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                                result.IdEncrypt = qryStr;
                                result.code = _getData.user_no;
                                result.sname = _checkActiv.EmpFullName;
                                result.sgroup = _checkActiv.UnitGroupName;
                                result.srank = _checkActiv.Rank;
                                result.status_name = _getData.TM_PTR_Eva_Status != null ? _getData.TM_PTR_Eva_Status.status_name_en + "" : "";
                                result.status_id = _getData.TM_PTR_Eva_Status != null ? _getData.TM_PTR_Eva_Status.Id + "" : "";
                                result.yearcurrent = _getData.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy");
                                result.other_role = _getData.other_roles;
                                if (_getData.PTR_Evaluation_File != null && _getData.PTR_Evaluation_File.Any(a => a.active_status == "Y"))
                                {
                                    int nFile = 1;
                                    result.lstFile = _getData.PTR_Evaluation_File.Where(a => a.active_status == "Y").Select(s => new vPTREvaluation_lst_File
                                    {
                                        file_name = s.sfile_oldname,
                                        description = s.description,
                                        Edit = (_getData.TM_PTR_Eva_Status.Id != (int)Eva_Status.Draft_Plan && _getData.TM_PTR_Eva_Status.Id != (int)Eva_Status.Waiting_for_Revised_Plan) ? "" + nFile++ : @"<button id=""btnEdit""  type=""button"" onclick=""DeleteFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                        View = @"<button id=""btnEdit""  type=""button"" onclick=""LoadFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-success""><i class=""glyphicon glyphicon-download-alt""></i></button>",
                                    }).ToList();
                                }
                                #region if else Data

                                #region Plaining Data
                                result.eva_mode = "Plan";
                                if (_getData.PTR_Evaluation_KPIs != null && _getData.PTR_Evaluation_KPIs.Any(a => a.active_status == "Y"))
                                {
                                    result.lstKPIs = _getData.PTR_Evaluation_KPIs.Where(a => a.active_status == "Y").Select(s => new vPTREvaluation_KPIs
                                    {
                                        sname = s.TM_KPIs_Base.kpi_name_en,
                                        target_data = s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : (s.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(s.target_max))) : HCMFunc.DataDecryptPES(s.target_max)),
                                        target_group = s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.group_target) + "-" + HCMFunc.DataDecryptPES(s.group_target_max) + "%" : (s.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(s.group_target_max))) : HCMFunc.DataDecryptPES(s.group_target_max)),

                                        //  target = s.TM_KPIs_Base.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(s.target) + "-" + HCMFunc.DataDecryptPES(s.target_max) + "%" : (s.TM_KPIs_Base.type_of_kpi == "N" ? FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(s.target_max))) : HCMFunc.DataDecryptPES(s.target_max)),
                                        howto = HCMFunc.DataDecryptPES(s.how_to),
                                        sdisable = (true || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Plan || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Plan) ? "N" : "Y",
                                        IdEncrypt = s.Id + "",
                                        stype = s.TM_KPIs_Base.type_of_kpi,
                                    }).ToList();

                                }
                                if (_getData.PTR_Evaluation_Approve != null && _getData.PTR_Evaluation_Approve.Any(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "P"))
                                {
                                    string[] empNO = _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "P").Select(s => s.Req_Approve_user).ToArray();
                                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                                    result.lstApprove = (from lst in _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "P")
                                                         from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lst.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                         select new vPTREvaluation_lst_approve
                                                         {
                                                             step_name = lst.TM_PTR_Eva_ApproveStep.Step_name_en,
                                                             app_name = lstEmpReq.EmpFullName,
                                                             nStep = lst.TM_PTR_Eva_ApproveStep.seq.HasValue ? lst.TM_PTR_Eva_ApproveStep.seq.Value : 0,
                                                             approve_date = lst.Approve_date.HasValue ? lst.Approve_date.Value.DateTimebyCulture() : "",
                                                         }).ToList();
                                }

                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null)
                                {
                                    result.lstAnswer = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.active_status == "Y" && w.questions_type == "P")
                                                        select new vPTREvaluation_Answer
                                                        {
                                                            nCID = lstQ.nCId,
                                                            id = lstQ.Id + "",
                                                            question = lstQ.question,
                                                            header = lstQ.header,
                                                            nSeq = lstQ.seq,
                                                            remark = "",
                                                            sgroup = lstQ.qgroup + "",
                                                            sdisable = (true || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Plan || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Plan) ? "N" : "Y",
                                                        }).ToList();
                                    if (_getData.PTR_Evaluation_Answer != null && _getData.PTR_Evaluation_Answer.Any(a => a.active_status == "Y"))
                                    {
                                        result.lstAnswer.ForEach(ed =>
                                        {
                                            var GetAns = _getData.PTR_Evaluation_Answer.Where(w => w.TM_PTR_Eva_Questions.Id + "" == ed.id).FirstOrDefault();
                                            if (GetAns != null)
                                            {
                                                ed.remark = HCMFunc.DataDecryptPES(GetAns.answer + "");
                                            }
                                        });
                                    }
                                }
                                #endregion



                                #endregion




                                if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2018)
                                {
                                    return View(result);
                                }
                                else if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019)
                                {
                                    return View("PTRPersonalPlanEdit19", result);
                                }
                                else if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2020)
                                {
                                    return View("PTRPersonalPlanEdit20", result);
                                }
                                else
                                {
                                    return View(result);
                                }

                            }
                            else
                            {
                                return RedirectToAction("PTRPersonalPlanList", "PTREvaluation");
                            }
                        }
                        else
                        {
                            return RedirectToAction("ErrorNopermission", "MasterPage");
                        }

                    }
                    else
                    {
                        return RedirectToAction("PTRPersonalPlanList", "PTREvaluation");
                    }
                }
                else
                {
                    return RedirectToAction("PTRPersonalPlanList", "PTREvaluation");
                }
            }
            else
            {
                return RedirectToAction("PTRPersonalPlanList", "PTREvaluation");
            }
            #endregion
        }
        #endregion

        #region Ajax Function
        [HttpPost]
        public ActionResult LoadPTREvaluationList(CSearchPTREvaluation SearchItem)
        {
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            List<vPTREvaluation_obj> lstData_resutl = new List<vPTREvaluation_obj>();
            int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
            var lstData = _PTR_EvaluationService.GetPTREvaluationList(nYear, CGlobal.UserInfo.EmployeeNo, CGlobal.UserIsAdminPES());
            var testgetlists = lstData.Where(w => w.user_no == "00010849").FirstOrDefault();
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);
            if (lstData.Any())
            {
                string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                var getnoace = empNO.Where(w => w == "00010849").FirstOrDefault();
                string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo) && w.Status == 3 || w.Status == 1).ToList();
                var testgetpart = _getPartnerUser.Where(w => w.EmpNo == "00010849").FirstOrDefault();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser);

                if (_getPartnerUser.Any())
                {
                    string[] empNOActive = _getPartnerUser.Select(s => s.EmpNo).ToArray();
                    var getnoac = empNO.Where(w => w == "00010849").FirstOrDefault();
                    lstData_resutl = (from lstAD in lstData.Where(w => empNOActive.Contains(w.user_no))
                                      from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vPTREvaluation_obj
                                      {
                                          srank = lstEmpReq.RankCode,
                                          sgroup = lstEmpReq.UnitGroupName,
                                          name_en = lstAD.user_no + " : " + lstEmpReq.EmpFullName,
                                          fy_year = lstAD.PTR_Evaluation_Year.evaluation_year.HasValue ? lstAD.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                          active_status = lstAD.TM_PTR_Eva_Status != null ? lstAD.TM_PTR_Eva_Status.status_name_en : "", //lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          name = lstEmpReq.EmpFullName,
                                      }).ToList();
                }


                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult UploadFileMulti()
        {
            vPTREvaluation_UploadFile result = new vPTREvaluation_UploadFile();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            string IdEncrypt = Request.Form["IdEncrypt"];
            if (!string.IsNullOrEmpty(IdEncrypt))
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        if (Request != null)
                        {
                            string sSess = Request.Form["sSess"];
                            string desc = Request.Form["description"];
                            if (Request.Files.Count > 0)
                            {
                                string[] aType = new string[] { ".pdf", ".docx", ".doc" };
                                foreach (string file in Request.Files)
                                {
                                    var filedata = new byte[] { };
                                    var fileContent = Request.Files[file];
                                    if (fileContent != null && fileContent.ContentLength > 0)
                                    {
                                        // get a stream
                                        var stream = fileContent.InputStream;
                                        // and optionally write the file to disk
                                        var fileName = Path.GetFileName(file);
                                        var fileType = Path.GetExtension(fileName).ToLower() + "";
                                        if (aType.Contains(fileType))
                                        {
                                            using (var binaryReader = new BinaryReader(stream))
                                            {
                                                filedata = binaryReader.ReadBytes(fileContent.ContentLength);
                                            }

                                            PTR_Evaluation_File objSave = new PTR_Evaluation_File()
                                            {
                                                PTR_Evaluation = _getData,
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                active_status = "Y",
                                                sfile64 = filedata,
                                                sfile_oldname = fileName,
                                                sfileType = fileType,
                                                description = desc,
                                            };


                                            var sComplect = _PTR_Evaluation_FileService.CreateNew(objSave);
                                            result.Status = SystemFunction.process_Success;
                                        }
                                    }
                                }
                            }

                            var _getUpdate = _PTR_EvaluationService.Find(nId);
                            if (_getUpdate != null && _getUpdate.PTR_Evaluation_File != null && _getUpdate.PTR_Evaluation_File.Any(a => a.active_status == "Y"))
                            {
                                result.lstNewData = _getUpdate.PTR_Evaluation_File.Where(a => a.active_status == "Y").Select(s => new vPTREvaluation_lst_File
                                {
                                    file_name = s.sfile_oldname,
                                    description = s.description,
                                    Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                    View = @"<button id=""btnEdit""  type=""button"" onclick=""LoadFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-success""><i class=""glyphicon glyphicon-download-alt""></i></button>",
                                }).ToList();
                            }
                        }
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Evaluation not found.";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Evaluation not found.";
            }

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult EditPTREvaluation(vPTRMailBox_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTRMailBox_Return result = new vPTRMailBox_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Planning_Completed);

                    if (_GetStatus == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Status not found.";
                        return Json(new
                        {
                            result
                        });
                    }
                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                        if (_getDataYear != null)
                        {
                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.TM_PTR_Eva_Status = _GetStatus;
                            _getData.PTR_Evaluation_Year = _getDataYear;

                            var sComplect = _PTR_EvaluationService.Complect(_getData);
                            if (sComplect > 0)
                            {

                                result.Status = SystemFunction.process_Success;
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, please try again.";
                            }

                        }


                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Request Type Not Found.";
                    }
                }
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult ComplectToSaveDarft(vPTRMailBox_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTRMailBox_Return result = new vPTRMailBox_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Draft_Evaluate);

                    if (_GetStatus == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Status not found.";
                        return Json(new
                        {
                            result
                        });
                    }
                    var _getData = _PTR_EvaluationService.Find(nId);
                    var _GetApproveStep = _TM_PTR_Eva_ApproveStepService.GetDataForSelect();
                    if (_getData != null)
                    {
                        List<int> lstPlan = new List<int>();
                        lstPlan.Add((int)StepApproveEvaluate.pHead);
                        lstPlan.Add((int)StepApproveEvaluate.gHead);
                        lstPlan.Add((int)StepApproveEvaluate.Ceo);
                        if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year != 2020)
                        {
                            lstPlan.Add((int)StepApproveEvaluate.DCEO);
                        }
                        var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                        if (_checkActiv != null && _GetApproveStep.Any())
                        {
                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            if (_getDataYear != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.TM_PTR_Eva_Status = _GetStatus;
                                _getData.PTR_Evaluation_Year = _getDataYear;
                                var sComplect = _PTR_EvaluationService.Complect(_getData);
                                if (sComplect > 0)
                                {
                                    List<PTR_Evaluation_Approve> lstApprove = new List<PTR_Evaluation_Approve>();
                                    lstApprove.Add(new PTR_Evaluation_Approve
                                    {
                                        active_status = "Y",
                                        Req_Approve_user = _getData.user_no + "",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_PTR_Eva_ApproveStep = _GetApproveStep.Where(w => w.Id == (int)StepApproveEvaluate.Self).FirstOrDefault(),
                                        PTR_Evaluation = _getData,

                                    });
                                    #region Add Approve Step 
                                    var CheckGroupH = dbHr.tbMaster_UnitGroupHead.Where(w => w.EmployeeNo == _getData.user_no && w.RankID == 1).FirstOrDefault();
                                    var CheckPool = dbHr.tbMaster_PoolHead.Where(w => w.EmployeeNo == _getData.user_no).FirstOrDefault();
                                    var CheckCEO = dbHr.tbMaster_CompanyHead.Where(w => w.EmployeeNo == _getData.user_no).FirstOrDefault();
                                    foreach (var app in _GetApproveStep.Where(w => lstPlan.Contains(w.Id)).OrderBy(o => o.seq))
                                    {
                                        if (app.Id == (int)StepApproveEvaluate.gHead)
                                        {

                                            if (CheckGroupH == null && CheckPool == null && CheckCEO == null)
                                            {
                                                var _getGroupHead = dbHr.tbMaster_UnitGroupHead.Where(w => w.UnitGroupID == _checkActiv.UnitGroupID && w.RankID == 1 && w.EndDate > DateTime.Now).FirstOrDefault();
                                                if (_getGroupHead != null)
                                                {
                                                    lstApprove.Add(new PTR_Evaluation_Approve
                                                    {
                                                        active_status = "Y",
                                                        Req_Approve_user = _getGroupHead.EmployeeNo + "",
                                                        update_user = CGlobal.UserInfo.UserId,
                                                        update_date = dNow,
                                                        create_user = CGlobal.UserInfo.UserId,
                                                        create_date = dNow,
                                                        TM_PTR_Eva_ApproveStep = _GetApproveStep.Where(w => w.Id == (int)StepApproveEvaluate.gHead).FirstOrDefault(),
                                                        PTR_Evaluation = _getData,
                                                    });
                                                }

                                            }
                                        }
                                        else if (app.Id == (int)StepApproveEvaluate.pHead)
                                        {

                                            if (CheckPool == null && CheckCEO == null)
                                            {
                                                var _getPoolHead = dbHr.tbMaster_PoolHead.Where(w => w.PoolID == _checkActiv.PoolID && w.EndDate > DateTime.Now).FirstOrDefault();
                                                if (_getPoolHead != null)
                                                {
                                                    lstApprove.Add(new PTR_Evaluation_Approve
                                                    {
                                                        active_status = "Y",
                                                        Req_Approve_user = _getPoolHead.EmployeeNo + "",
                                                        update_user = CGlobal.UserInfo.UserId,
                                                        update_date = dNow,
                                                        create_user = CGlobal.UserInfo.UserId,
                                                        create_date = dNow,
                                                        TM_PTR_Eva_ApproveStep = _GetApproveStep.Where(w => w.Id == (int)StepApproveEvaluate.pHead).FirstOrDefault(),
                                                        PTR_Evaluation = _getData,
                                                    });
                                                }

                                            }
                                        }
                                        else if (app.Id == (int)StepApproveEvaluate.Ceo)
                                        {

                                            if (CheckCEO == null)
                                            {
                                                var _getCEOHead = dbHr.tbMaster_CompanyHead.Where(w => w.tbMaster_Company.LocalCompCode == _checkActiv.CompanyCode && w.EndDate > DateTime.Now).FirstOrDefault();
                                                if (_getCEOHead != null)
                                                {
                                                    lstApprove.Add(new PTR_Evaluation_Approve
                                                    {
                                                        active_status = "Y",
                                                        Req_Approve_user = _getCEOHead.EmployeeNo + "",
                                                        update_user = CGlobal.UserInfo.UserId,
                                                        update_date = dNow,
                                                        create_user = CGlobal.UserInfo.UserId,
                                                        create_date = dNow,
                                                        TM_PTR_Eva_ApproveStep = _GetApproveStep.Where(w => w.Id == (int)StepApproveEvaluate.Ceo).FirstOrDefault(),
                                                        PTR_Evaluation = _getData,
                                                    });
                                                }
                                            }
                                        }
                                        else if (app.Id == (int)StepApproveEvaluate.DCEO)
                                        {

                                            lstApprove.Add(new PTR_Evaluation_Approve
                                            {
                                                active_status = "Y",
                                                Req_Approve_user = PESClass.Deputy_Ceo + "",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                TM_PTR_Eva_ApproveStep = _GetApproveStep.Where(w => w.Id == (int)StepApproveEvaluate.DCEO).FirstOrDefault(),
                                                PTR_Evaluation = _getData,
                                            });
                                        }
                                    }
                                    #endregion
                                    var sComplectApprove = _PTR_Evaluation_ApproveService.CreateNewByList(lstApprove);
                                    result.Msg = ItemData.IdEncrypt + "";
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Request Type Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Request Type Not Found.";
                    }
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult DeleteFile(vPTREvaluation_File ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTREvaluation_UploadFile result = new vPTREvaluation_UploadFile();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                int nFileId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.File_IdEncrypt + ""));
                if (nId != 0 && nFileId != 0)
                {
                    var _getDataFile = _PTR_Evaluation_FileService.FindForDelete(nId, nFileId);
                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null && _getDataFile != null)
                    {
                        _getDataFile.PTR_Evaluation = _getData;
                        _getDataFile.update_user = CGlobal.UserInfo.UserId;
                        _getDataFile.update_date = dNow;
                        _getDataFile.sfile64 = null;
                        _getDataFile.active_status = "N";
                        var sComplect = _PTR_Evaluation_FileService.DeleteFile(_getDataFile);
                        if (sComplect > 0)
                        {
                            var _getUpdate = _PTR_EvaluationService.Find(nId);
                            if (_getUpdate != null && _getUpdate.PTR_Evaluation_File != null && _getUpdate.PTR_Evaluation_File.Any(a => a.active_status == "Y"))
                            {
                                result.lstNewData = _getUpdate.PTR_Evaluation_File.Where(a => a.active_status == "Y").Select(s => new vPTREvaluation_lst_File
                                {
                                    file_name = s.sfile_oldname,
                                    description = s.description,
                                    Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                    View = @"<button id=""btnEdit""  type=""button"" onclick=""LoadFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-success""><i class=""glyphicon glyphicon-download-alt""></i></button>",
                                }).ToList();
                            }
                            result.Status = SystemFunction.process_Success;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please try again.";
                        }
                    }

                }
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public JsonResult UserAutoComplete(string SearchItem)
        {
            List<C_USERS_RETURN> result = new List<C_USERS_RETURN>();
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            string[] aRank = new string[] { "12", "20" };
            IQueryable<AllInfo_WS> sQuery = dbHr.AllInfo_WS.Where(w => (w.Status == 1 || w.Status == 3) && aRank.Contains(w.RankID));

            var _getData = sQuery.Where(w =>
            (
            (w.EmpNo + "").Trim().ToLower() +
            (w.EmpFirstName + "").Trim().ToLower() +
            (w.EmpSurname + "").Trim().ToLower() +
            (w.EmpThaiName + "").Trim().ToLower() +
            (w.EmpThaiSurName + "").Trim().ToLower() +
             ((w.EmpFirstName + "").Trim().ToLower() + " " + (w.EmpSurname + "").Trim().ToLower()) +
             ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
            ).Contains((SearchItem + "").Trim().ToLower() ?? (w.EmpNo + "").Trim().ToLower() +
            (w.EmpFirstName + "").Trim().ToLower() +
            (w.EmpSurname + "").Trim().ToLower() +
            (w.EmpThaiName + "").Trim().ToLower() +
            (w.EmpThaiSurName + "").Trim().ToLower() +
             ((w.EmpFirstName + "").Trim().ToLower() + " " + (w.EmpSurname + "").Trim().ToLower()) +
             ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
            ).Take(15).ToList();
            if (_getData.Any())
            {
                result = (from lstGt in _getData
                          select new C_USERS_RETURN
                          {
                              id = lstGt.EmpNo,
                              user_id = lstGt.UserID,
                              user_name = lstGt.EmpFullName,
                              unit_name = lstGt.UnitGroupName
                          }
                    ).ToList();
            }
            #endregion
            //return result;
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        #region Save Evaluation
        [HttpPost]
        public ActionResult CreateFeedbackEmp(vPTREvaluation_Feedback_obj_save ItemData)
        {
            objPTREvaluation_Feedback_Return result = new objPTREvaluation_Feedback_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            result.lstData = new List<vPTREvaluation_Feedback>();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.rating_id))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                    int nRateID = SystemFunction.GetIntNullToZero(ItemData.rating_id);
                    if (nId != 0)
                    {
                        var _getData = _PTR_EvaluationService.Find(nId);
                        var _getRating = _TM_Feedback_RatingService.Find(nRateID);
                        if (_getData != null && _getRating != null)
                        {
                            string[] aRank = new string[] { "12", "20", "32" };
                            var checkActive = dbHr.AllInfo_WS.Where(w => (w.Status == 1 || w.Status == 3) && aRank.Contains(w.RankID) && w.EmpNo == ItemData.emp_no).FirstOrDefault();
                            if (checkActive != null)
                            {
                                PTR_Feedback_Emp objSave = new PTR_Feedback_Emp()
                                {
                                    //Id = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.id + "")),
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    PTR_Evaluation = _getData,
                                    recommendations = HCMFunc.DataEncryptPES(ItemData.recommendations + ""),
                                    appreciations = HCMFunc.DataEncryptPES(ItemData.appreciations + ""),
                                    TM_Feedback_Rating = _getRating,
                                    active_status = "Y",
                                    user_no = ItemData.emp_no,
                                };
                                if (_PTR_Feedback_EmpService.CanSave(objSave))
                                {
                                    var sComplect = _PTR_Feedback_EmpService.CreateNew(objSave);
                                    if (sComplect > 0)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                        var _getEditList = _PTR_EvaluationService.Find(nId);
                                        if (_getEditList != null)
                                        {
                                            if (_getEditList.PTR_Feedback_Emp != null && _getEditList.PTR_Feedback_Emp.Any(w => w.active_status == "Y"))
                                            {
                                                string[] aNo = _getEditList.PTR_Feedback_Emp.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                                                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => aNo.Contains(w.EmpNo)).ToList();

                                                result.lstData.AddRange((from lstAD in _getEditList.PTR_Feedback_Emp.Where(w => w.active_status == "Y")
                                                                         from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                                         select new vPTREvaluation_Feedback
                                                                         {
                                                                             sname = lstEmpReq.EmpFullName,
                                                                             rating = lstAD.TM_Feedback_Rating.feedback_rating_name_en,
                                                                             appreciations = HCMFunc.DataDecryptPES(lstAD.appreciations + ""),
                                                                             recommendations = HCMFunc.DataDecryptPES(lstAD.recommendations + ""),
                                                                             Edit = (_getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? @"<button id=""btnEdit""  type=""button"" onclick=""DelectFeedEmp('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>" : "",
                                                                             sgroup = "Partner/Directors/ADs",
                                                                         }).ToList());
                                            }
                                            if (_getEditList.PTR_Feedback_UnitGroup != null && _getEditList.PTR_Feedback_UnitGroup.Any(w => w.active_status == "Y"))
                                            {
                                                var _getUnit = _DivisionService.GetForFeedback();
                                                result.lstData.AddRange((from lstAD in _getEditList.PTR_Feedback_UnitGroup.Where(w => w.active_status == "Y")
                                                                         from lstEmpReq in _getUnit.Where(w => w.division_code == lstAD.unitcode).DefaultIfEmpty(new Models.Common.TM_Divisions())
                                                                         select new vPTREvaluation_Feedback
                                                                         {
                                                                             sname = lstEmpReq.division_name_en,
                                                                             rating = lstAD.TM_Feedback_Rating.feedback_rating_name_en,
                                                                             appreciations = HCMFunc.DataDecryptPES(lstAD.appreciations + ""),
                                                                             recommendations = HCMFunc.DataDecryptPES(lstAD.recommendations + ""),
                                                                             Edit = (_getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? @"<button id=""btnEdit""  type=""button"" onclick=""DelectFeedUnit('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>" : "",
                                                                             sgroup = "Unit Name",
                                                                         }).ToList());
                                            }
                                        }
                                        else
                                        {
                                            result.lstData = new List<vPTREvaluation_Feedback>();
                                        }

                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, please try again.";
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Duplicate;
                                    result.Msg = "Duplicate Employee name.";
                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Employee Not Found.";
                            }

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Rating Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation not found";
                    }

                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please select rating.";
                }
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult DeleteFeedbackEmp(vPTREvaluation_Feedback_obj_save ItemData)
        {
            objPTREvaluation_Feedback_Return result = new objPTREvaluation_Feedback_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            result.lstData = new List<vPTREvaluation_Feedback>();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                    int nEvaID = 0;
                    if (nId != 0)
                    {
                        var _getData = _PTR_Feedback_EmpService.Find(nId);
                        if (_getData != null)
                        {
                            nEvaID = _getData.PTR_Evaluation.Id;
                            var _getEva = _PTR_EvaluationService.Find(nEvaID);
                            _getData.active_status = "N";
                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.PTR_Evaluation = _getEva;
                            var sComplect = _PTR_Feedback_EmpService.Update(_getData);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getEditList = _PTR_EvaluationService.Find(nEvaID);
                                if (_getEditList != null)
                                {
                                    if (_getEditList.PTR_Feedback_Emp != null && _getEditList.PTR_Feedback_Emp.Any(w => w.active_status == "Y"))
                                    {
                                        string[] aNo = _getEditList.PTR_Feedback_Emp.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                                        var _getPartnerUser = dbHr.AllInfo_WS.Where(w => aNo.Contains(w.EmpNo)).ToList();

                                        result.lstData.AddRange((from lstAD in _getEditList.PTR_Feedback_Emp.Where(w => w.active_status == "Y")
                                                                 from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                                 select new vPTREvaluation_Feedback
                                                                 {
                                                                     sname = lstEmpReq.EmpFullName,
                                                                     rating = lstAD.TM_Feedback_Rating.feedback_rating_name_en,
                                                                     appreciations = HCMFunc.DataDecryptPES(lstAD.appreciations + ""),
                                                                     recommendations = HCMFunc.DataDecryptPES(lstAD.recommendations + ""),
                                                                     Edit = (_getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? @"<button id=""btnEdit""  type=""button"" onclick=""DelectFeedEmp('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>" : "",
                                                                     sgroup = "Partner/Directors/ADs",
                                                                 }).ToList());
                                    }
                                    if (_getEditList.PTR_Feedback_UnitGroup != null && _getEditList.PTR_Feedback_UnitGroup.Any(w => w.active_status == "Y"))
                                    {
                                        var _getUnit = _DivisionService.GetForFeedback();
                                        result.lstData.AddRange((from lstAD in _getEditList.PTR_Feedback_UnitGroup.Where(w => w.active_status == "Y")
                                                                 from lstEmpReq in _getUnit.Where(w => w.division_code == lstAD.unitcode).DefaultIfEmpty(new Models.Common.TM_Divisions())
                                                                 select new vPTREvaluation_Feedback
                                                                 {
                                                                     sname = lstEmpReq.division_name_en,
                                                                     rating = lstAD.TM_Feedback_Rating.feedback_rating_name_en,
                                                                     appreciations = HCMFunc.DataDecryptPES(lstAD.appreciations + ""),
                                                                     recommendations = HCMFunc.DataDecryptPES(lstAD.recommendations + ""),
                                                                     Edit = (_getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? @"<button id=""btnEdit""  type=""button"" onclick=""DelectFeedUnit('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>" : "",
                                                                     sgroup = "Unit Name",
                                                                 }).ToList());
                                    }
                                }
                                else
                                {
                                    result.lstData = new List<vPTREvaluation_Feedback>();
                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, please try again.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Employee not found";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Employee not found";
                    }

                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Employee not found";
                }
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult CreateFeedbackGroup(vPTREvaluation_Feedback_obj_save ItemData)
        {
            objPTREvaluation_Feedback_Return result = new objPTREvaluation_Feedback_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            result.lstData = new List<vPTREvaluation_Feedback>();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.rating_id))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                    int nRateID = SystemFunction.GetIntNullToZero(ItemData.rating_id);
                    if (nId != 0)
                    {
                        var _getData = _PTR_EvaluationService.Find(nId);
                        var _getRating = _TM_Feedback_RatingService.Find(nRateID);
                        if (_getData != null && _getRating != null)
                        {
                            var _checkUnit = _DivisionService.FindByCode(ItemData.unit_id);
                            if (_checkUnit != null)
                            {
                                PTR_Feedback_UnitGroup objSave = new PTR_Feedback_UnitGroup()
                                {
                                    //Id = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.id + "")),
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    PTR_Evaluation = _getData,
                                    recommendations = HCMFunc.DataEncryptPES(ItemData.recommendations + ""),
                                    appreciations = HCMFunc.DataEncryptPES(ItemData.appreciations + ""),
                                    TM_Feedback_Rating = _getRating,
                                    active_status = "Y",
                                    unitcode = ItemData.unit_id,
                                };
                                if (_PTR_Feedback_UnitGroupService.CanSave(objSave))
                                {
                                    var sComplect = _PTR_Feedback_UnitGroupService.CreateNew(objSave);
                                    if (sComplect > 0)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                        var _getEditList = _PTR_EvaluationService.Find(nId);
                                        if (_getEditList != null)
                                        {
                                            if (_getEditList.PTR_Feedback_Emp != null && _getEditList.PTR_Feedback_Emp.Any(w => w.active_status == "Y"))
                                            {
                                                string[] aNo = _getEditList.PTR_Feedback_Emp.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                                                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => aNo.Contains(w.EmpNo)).ToList();

                                                result.lstData.AddRange((from lstAD in _getEditList.PTR_Feedback_Emp.Where(w => w.active_status == "Y")
                                                                         from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                                         select new vPTREvaluation_Feedback
                                                                         {
                                                                             sname = lstEmpReq.EmpFullName,
                                                                             rating = lstAD.TM_Feedback_Rating.feedback_rating_name_en,
                                                                             appreciations = HCMFunc.DataDecryptPES(lstAD.appreciations + ""),
                                                                             recommendations = HCMFunc.DataDecryptPES(lstAD.recommendations + ""),
                                                                             Edit = (_getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? @"<button id=""btnEdit""  type=""button"" onclick=""DelectFeedEmp('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>" : "",
                                                                             sgroup = "Partner/Directors/ADs",
                                                                         }).ToList());
                                            }
                                            if (_getEditList.PTR_Feedback_UnitGroup != null && _getEditList.PTR_Feedback_UnitGroup.Any(w => w.active_status == "Y"))
                                            {
                                                var _getUnit = _DivisionService.GetForFeedback();
                                                result.lstData.AddRange((from lstAD in _getEditList.PTR_Feedback_UnitGroup.Where(w => w.active_status == "Y")
                                                                         from lstEmpReq in _getUnit.Where(w => w.division_code == lstAD.unitcode).DefaultIfEmpty(new Models.Common.TM_Divisions())
                                                                         select new vPTREvaluation_Feedback
                                                                         {
                                                                             sname = lstEmpReq.division_name_en,
                                                                             rating = lstAD.TM_Feedback_Rating.feedback_rating_name_en,
                                                                             appreciations = HCMFunc.DataDecryptPES(lstAD.appreciations + ""),
                                                                             recommendations = HCMFunc.DataDecryptPES(lstAD.recommendations + ""),
                                                                             Edit = (_getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? @"<button id=""btnEdit""  type=""button"" onclick=""DelectFeedUnit('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>" : "",
                                                                             sgroup = "Unit Name",
                                                                         }).ToList());
                                            }
                                        }
                                        else
                                        {
                                            result.lstData = new List<vPTREvaluation_Feedback>();
                                        }

                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, please try again.";
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Duplicate;
                                    result.Msg = "Duplicate Group name.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Unit Group Not Found.";
                            }

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Rating Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation not found";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, please select rating.";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult DeleteFeedbackGroup(vPTREvaluation_Feedback_obj_save ItemData)
        {
            objPTREvaluation_Feedback_Return result = new objPTREvaluation_Feedback_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            result.lstData = new List<vPTREvaluation_Feedback>();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                    int nEvaID = 0;
                    if (nId != 0)
                    {
                        var _getData = _PTR_Feedback_UnitGroupService.Find(nId);
                        if (_getData != null)
                        {
                            nEvaID = _getData.PTR_Evaluation.Id;
                            var _getEva = _PTR_EvaluationService.Find(nEvaID);
                            _getData.active_status = "N";
                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.PTR_Evaluation = _getEva;
                            var sComplect = _PTR_Feedback_UnitGroupService.Update(_getData);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getEditList = _PTR_EvaluationService.Find(nEvaID);
                                if (_getEditList != null)
                                {
                                    if (_getEditList.PTR_Feedback_Emp != null && _getEditList.PTR_Feedback_Emp.Any(w => w.active_status == "Y"))
                                    {
                                        string[] aNo = _getEditList.PTR_Feedback_Emp.Where(w => w.active_status == "Y").Select(s => s.user_no).ToArray();
                                        var _getPartnerUser = dbHr.AllInfo_WS.Where(w => aNo.Contains(w.EmpNo)).ToList();

                                        result.lstData.AddRange((from lstAD in _getEditList.PTR_Feedback_Emp.Where(w => w.active_status == "Y")
                                                                 from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                                                 select new vPTREvaluation_Feedback
                                                                 {
                                                                     sname = lstEmpReq.EmpFullName,
                                                                     rating = lstAD.TM_Feedback_Rating.feedback_rating_name_en,
                                                                     appreciations = HCMFunc.DataDecryptPES(lstAD.appreciations + ""),
                                                                     recommendations = HCMFunc.DataDecryptPES(lstAD.recommendations + ""),
                                                                     Edit = (_getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? @"<button id=""btnEdit""  type=""button"" onclick=""DelectFeedEmp('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>" : "",
                                                                     sgroup = "Partner/Directors/ADs",
                                                                 }).ToList());
                                    }
                                    if (_getEditList.PTR_Feedback_UnitGroup != null && _getEditList.PTR_Feedback_UnitGroup.Any(w => w.active_status == "Y"))
                                    {
                                        var _getUnit = _DivisionService.GetForFeedback();
                                        result.lstData.AddRange((from lstAD in _getEditList.PTR_Feedback_UnitGroup.Where(w => w.active_status == "Y")
                                                                 from lstEmpReq in _getUnit.Where(w => w.division_code == lstAD.unitcode).DefaultIfEmpty(new Models.Common.TM_Divisions())
                                                                 select new vPTREvaluation_Feedback
                                                                 {
                                                                     sname = lstEmpReq.division_name_en,
                                                                     rating = lstAD.TM_Feedback_Rating.feedback_rating_name_en,
                                                                     appreciations = HCMFunc.DataDecryptPES(lstAD.appreciations + ""),
                                                                     recommendations = HCMFunc.DataDecryptPES(lstAD.recommendations + ""),
                                                                     Edit = (_getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getEditList.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) ? @"<button id=""btnEdit""  type=""button"" onclick=""DelectFeedUnit('" + HCMFunc.EncryptPES(lstAD.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>" : "",
                                                                     sgroup = "Unit Name",
                                                                 }).ToList());
                                    }
                                }
                                else
                                {
                                    result.lstData = new List<vPTREvaluation_Feedback>();
                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, please try again.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Unit Group Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Unit Group Not Found.";

                    }

                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Unit Group Not Found.";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult SaveDraftEvaForm(vPTREvaluation_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Planning_Completed);

                    if (_GetStatus == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Status not found.";
                        return Json(new
                        {
                            result
                        });
                    }

                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                        {
                            List<PTR_Evaluation_Answer> lstAns = new List<PTR_Evaluation_Answer>();
                            List<PTR_Evaluation_KPIs> lstKPIs = new List<PTR_Evaluation_KPIs>();
                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.PTR_Evaluation_Year = _getDataYear;
                                _getData.other_roles = ItemData.other_role;
                                string[] aTIFQID = _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.questions_type == "G").Select(s => s.Id + "").ToArray();
                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null && _getEvaQuestion.TM_PTR_Eva_Questions.Any())
                                {
                                    if (ItemData.lstAnswer != null)
                                    {
                                        lstAns = (from lstA in ItemData.lstAnswer.Where(w => aTIFQID.Contains(w.id))
                                                  from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Questions())
                                                  select new PTR_Evaluation_Answer
                                                  {
                                                      update_user = CGlobal.UserInfo.UserId,
                                                      update_date = dNow,
                                                      create_user = CGlobal.UserInfo.UserId,
                                                      create_date = dNow,
                                                      answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                      active_status = "Y",
                                                      TM_PTR_Eva_Questions = lstQ != null ? lstQ : null,
                                                      PTR_Evaluation = _getData,
                                                  }).ToList();
                                    }
                                }


                                var sComplect = _PTR_EvaluationService.Complect(_getData);
                                if (sComplect > 0)
                                {
                                    int nRating = SystemFunction.GetIntNullToZero(ItemData.self_rating_id);
                                    var _getEvaRating = _TM_Annual_RatingService.Find(nRating);
                                    if (_getEvaRating != null)
                                    {
                                        var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self && w.active_status == "Y").FirstOrDefault();
                                        if (_getSelfApprove != null)
                                        {
                                            _getSelfApprove.PTR_Evaluation = _getData;
                                            _getSelfApprove.update_user = CGlobal.UserInfo.UserId;
                                            _getSelfApprove.update_date = dNow;
                                            _getSelfApprove.Annual_Rating = _getEvaRating;
                                            _PTR_Evaluation_ApproveService.Update(_getSelfApprove);
                                        }

                                    }
                                    if (lstAns.Any())
                                    {
                                        _PTR_Evaluation_AnswerService.UpdateAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }

                                    var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                                    if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any() && _getData.PTR_Evaluation_KPIs != null)
                                    {
                                        foreach (var item in ItemData.lstKPIs)
                                        {
                                            var GetKPI = _getData.PTR_Evaluation_KPIs.Where(w => w.Id + "" == item.IdEncrypt).FirstOrDefault();
                                            if (GetKPI != null)
                                            {
                                                if (GetKPI.TM_KPIs_Base.type_of_kpi == "N")
                                                {
                                                    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "") * 1000000) + "");
                                                }
                                                else if (GetKPI.TM_KPIs_Base.type_of_kpi == "D")
                                                {
                                                    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "")) + "");
                                                }
                                                GetKPI.final_remark = HCMFunc.DataEncryptPES(item.remark + "");
                                                GetKPI.update_user = CGlobal.UserInfo.UserId;
                                                GetKPI.update_date = dNow;
                                                GetKPI.PTR_Evaluation = _getData;
                                                _PTR_Evaluation_KPIsService.Update(GetKPI);
                                            }
                                        }
                                    }
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Don't have permission to save.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult SaveEvaForm(vPTREvaluation_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Evaluation_Approval);

                    if (_GetStatus == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Status not found.";
                        return Json(new
                        {
                            result
                        });
                    }

                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        int nRating = SystemFunction.GetIntNullToZero(ItemData.self_rating_id);
                        var _getEvaRating = _TM_Annual_RatingService.Find(nRating);
                        if (_getEvaRating != null)
                        {
                            List<PTR_Evaluation_Answer> lstAns = new List<PTR_Evaluation_Answer>();
                            List<PTR_Evaluation_KPIs> lstKPIs = new List<PTR_Evaluation_KPIs>();
                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.PTR_Evaluation_Year = _getDataYear;
                                _getData.other_roles = ItemData.other_role;
                                _getData.TM_PTR_Eva_Status = _GetStatus;
                                string[] aTIFQID = _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.questions_type == "G").Select(s => s.Id + "").ToArray();
                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null && _getEvaQuestion.TM_PTR_Eva_Questions.Any())
                                {
                                    if (ItemData.lstAnswer != null)
                                    {
                                        lstAns = (from lstA in ItemData.lstAnswer.Where(w => aTIFQID.Contains(w.id))
                                                  from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Questions())
                                                  select new PTR_Evaluation_Answer
                                                  {
                                                      update_user = CGlobal.UserInfo.UserId,
                                                      update_date = dNow,
                                                      create_user = CGlobal.UserInfo.UserId,
                                                      create_date = dNow,
                                                      answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                      active_status = "Y",
                                                      TM_PTR_Eva_Questions = lstQ != null ? lstQ : null,
                                                      PTR_Evaluation = _getData,
                                                  }).ToList();
                                    }
                                }


                                var sComplect = _PTR_EvaluationService.Complect(_getData);
                                if (sComplect > 0)
                                {

                                    if (_getEvaRating != null)
                                    {
                                        var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self && w.active_status == "Y").FirstOrDefault();
                                        if (_getSelfApprove != null)
                                        {
                                            _getSelfApprove.PTR_Evaluation = _getData;
                                            _getSelfApprove.update_user = CGlobal.UserInfo.UserId;
                                            _getSelfApprove.update_date = dNow;
                                            _getSelfApprove.Annual_Rating = _getEvaRating;
                                            _getSelfApprove.Approve_status = "Y";
                                            _getSelfApprove.Approve_date = dNow;
                                            _getSelfApprove.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                            sComplect = _PTR_Evaluation_ApproveService.Update(_getSelfApprove);
                                            if (sComplect > 0)
                                            {
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find(6);
                                                var GetMail = _PTR_Evaluation_ApproveService.GetEvaForMail(_getData.Id).FirstOrDefault();
                                                if (GetMail != null)
                                                {
                                                    var bSuss = SendPTRSubmitNewVersion(GetMail, Mail1, ref sError, ref mail_to_log);
                                                }

                                            }
                                        }

                                    }
                                    if (lstAns.Any())
                                    {
                                        _PTR_Evaluation_AnswerService.UpdateAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }

                                    var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                                    if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any() && _getData.PTR_Evaluation_KPIs != null)
                                    {
                                        foreach (var item in ItemData.lstKPIs)
                                        {
                                            var GetKPI = _getData.PTR_Evaluation_KPIs.Where(w => w.Id + "" == item.IdEncrypt).FirstOrDefault();
                                            if (GetKPI != null)
                                            {
                                                if (GetKPI.TM_KPIs_Base.type_of_kpi == "N")
                                                {
                                                    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "") * 1000000) + "");
                                                }
                                                else if (GetKPI.TM_KPIs_Base.type_of_kpi == "D")
                                                {
                                                    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "")) + "");
                                                }
                                                GetKPI.final_remark = HCMFunc.DataEncryptPES(item.remark + "");
                                                GetKPI.update_user = CGlobal.UserInfo.UserId;
                                                GetKPI.update_date = dNow;
                                                GetKPI.PTR_Evaluation = _getData;
                                                _PTR_Evaluation_KPIsService.Update(GetKPI);
                                            }
                                        }
                                    }
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Rating not found.";
                            }
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }

        #endregion
        #region Save Plan

        [HttpPost]
        public ActionResult SaveDraftPlanForm(vPTREvaluation_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Planning_Completed);

                    if (_GetStatus == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Status not found.";
                        return Json(new
                        {
                            result
                        });
                    }

                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                        {
                            List<PTR_Evaluation_Answer> lstAns = new List<PTR_Evaluation_Answer>();
                            List<PTR_Evaluation_KPIs> lstKPIs = new List<PTR_Evaluation_KPIs>();
                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.PTR_Evaluation_Year = _getDataYear;
                                _getData.other_roles = ItemData.other_role;
                                string[] aTIFQID = _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.questions_type == "P").Select(s => s.Id + "").ToArray();
                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null && _getEvaQuestion.TM_PTR_Eva_Questions.Any())
                                {
                                    if (ItemData.lstAnswer != null)
                                    {
                                        lstAns = (from lstA in ItemData.lstAnswer.Where(w => aTIFQID.Contains(w.id))
                                                  from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Questions())
                                                  select new PTR_Evaluation_Answer
                                                  {
                                                      update_user = CGlobal.UserInfo.UserId,
                                                      update_date = dNow,
                                                      create_user = CGlobal.UserInfo.UserId,
                                                      create_date = dNow,
                                                      answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                      active_status = "Y",
                                                      TM_PTR_Eva_Questions = lstQ != null ? lstQ : null,
                                                      PTR_Evaluation = _getData,
                                                  }).ToList();
                                    }
                                }


                                var sComplect = _PTR_EvaluationService.Complect(_getData);
                                if (sComplect > 0)
                                {
                                    //int nRating = SystemFunction.GetIntNullToZero(ItemData.self_rating_id);
                                    //var _getEvaRating = _TM_Annual_RatingService.Find(nRating);
                                    //if (_getEvaRating != null)
                                    //{
                                    //    var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self && w.active_status == "Y").FirstOrDefault();
                                    //    if (_getSelfApprove != null)
                                    //    {
                                    //        _getSelfApprove.PTR_Evaluation = _getData;
                                    //        _getSelfApprove.update_user = CGlobal.UserInfo.UserId;
                                    //        _getSelfApprove.update_date = dNow;
                                    //        _getSelfApprove.Annual_Rating = _getEvaRating;
                                    //        _PTR_Evaluation_ApproveService.Update(_getSelfApprove);
                                    //    }

                                    //}
                                    if (lstAns.Any())
                                    {
                                        _PTR_Evaluation_AnswerService.UpdatePlanAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }

                                    var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                                    if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any() && _getData.PTR_Evaluation_KPIs != null)
                                    {
                                        foreach (var item in ItemData.lstKPIs)
                                        {
                                            var GetKPI = _getData.PTR_Evaluation_KPIs.Where(w => w.Id + "" == item.IdEncrypt).FirstOrDefault();
                                            if (GetKPI != null)
                                            {
                                                GetKPI.update_user = CGlobal.UserInfo.UserId;
                                                GetKPI.update_date = dNow;
                                                GetKPI.PTR_Evaluation = _getData;
                                                GetKPI.how_to = HCMFunc.DataEncryptPES(item.remark + "");
                                                _PTR_Evaluation_KPIsService.Update(GetKPI);
                                            }
                                        }
                                    }
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Don't have permission to save.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult SavePlanForm(vPTREvaluation_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Planning_Approval);

                    if (_GetStatus == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Status not found.";
                        return Json(new
                        {
                            result
                        });
                    }

                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                        {
                            int nRating = SystemFunction.GetIntNullToZero(ItemData.self_rating_id);

                            List<PTR_Evaluation_Answer> lstAns = new List<PTR_Evaluation_Answer>();
                            List<PTR_Evaluation_KPIs> lstKPIs = new List<PTR_Evaluation_KPIs>();
                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.PTR_Evaluation_Year = _getDataYear;
                                _getData.other_roles = ItemData.other_role;
                                _getData.TM_PTR_Eva_Status = _GetStatus;
                                string[] aTIFQID = _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.questions_type == "P").Select(s => s.Id + "").ToArray();
                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null && _getEvaQuestion.TM_PTR_Eva_Questions.Any())
                                {
                                    if (ItemData.lstAnswer != null)
                                    {
                                        lstAns = (from lstA in ItemData.lstAnswer.Where(w => aTIFQID.Contains(w.id))
                                                  from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Questions())
                                                  select new PTR_Evaluation_Answer
                                                  {
                                                      update_user = CGlobal.UserInfo.UserId,
                                                      update_date = dNow,
                                                      create_user = CGlobal.UserInfo.UserId,
                                                      create_date = dNow,
                                                      answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                      active_status = "Y",
                                                      TM_PTR_Eva_Questions = lstQ != null ? lstQ : null,
                                                      PTR_Evaluation = _getData,
                                                  }).ToList();
                                    }
                                }


                                var sComplect = _PTR_EvaluationService.Complect(_getData);
                                if (sComplect > 0)
                                {

                                    if (1 == 1)
                                    {
                                        var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApprovePlan.Self && w.active_status == "Y").FirstOrDefault();
                                        if (_getSelfApprove != null)
                                        {
                                            _getSelfApprove.PTR_Evaluation = _getData;
                                            _getSelfApprove.update_user = CGlobal.UserInfo.UserId;
                                            _getSelfApprove.update_date = dNow;
                                            //_getSelfApprove.Annual_Rating = _getEvaRating;
                                            _getSelfApprove.Approve_status = "Y";
                                            _getSelfApprove.Approve_date = dNow;
                                            _getSelfApprove.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                            sComplect = _PTR_Evaluation_ApproveService.Update(_getSelfApprove);
                                            if (sComplect > 0)
                                            {
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPTR.Plan_Submit);
                                                var GetMail = _PTR_Evaluation_ApproveService.GetPlanForMail(_getData.Id).FirstOrDefault();
                                                if (GetMail != null)
                                                {
                                                    var bSuss = SendPTRSubmitPlan(GetMail, Mail1, ref sError, ref mail_to_log);
                                                }

                                            }
                                        }

                                    }
                                    if (lstAns.Any())
                                    {
                                        _PTR_Evaluation_AnswerService.UpdatePlanAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }

                                    var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                                    if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any() && _getData.PTR_Evaluation_KPIs != null)
                                    {
                                        foreach (var item in ItemData.lstKPIs)
                                        {
                                            var GetKPI = _getData.PTR_Evaluation_KPIs.Where(w => w.Id + "" == item.IdEncrypt).FirstOrDefault();
                                            if (GetKPI != null)
                                            {
                                                GetKPI.update_user = CGlobal.UserInfo.UserId;
                                                GetKPI.update_date = dNow;
                                                GetKPI.PTR_Evaluation = _getData;
                                                GetKPI.how_to = HCMFunc.DataEncryptPES(item.remark + "");
                                                _PTR_Evaluation_KPIsService.Update(GetKPI);
                                            }
                                        }
                                    }
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Rating not found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Don't have permission to save.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult LoadPTRPlanList(CSearchPTREvaluation SearchItem)
        {
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            List<vPTREvaluation_obj> lstData_resutl = new List<vPTREvaluation_obj>();
            int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
            var lstData = _PTR_EvaluationService.GetPTRPerformancePlanList(nYear, CGlobal.UserInfo.EmployeeNo, CGlobal.UserIsAdminPES());
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);
            if (lstData.Any())
            {
                string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo) && w.Status == 3 || w.Status == 1).ToList();

                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser);
                if (_getPartnerUser.Any())
                {
                    string[] empNOActive = _getPartnerUser.Select(s => s.EmpNo).ToArray();
                    lstData_resutl = (from lstAD in lstData.Where(w => empNOActive.Contains(w.user_no))
                                      from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vPTREvaluation_obj
                                      {
                                          srank = lstEmpReq.RankCode,
                                          sgroup = lstEmpReq.UnitGroupName,
                                          name_en = lstAD.user_no + " : " + lstEmpReq.EmpFullName,
                                          fy_year = lstAD.PTR_Evaluation_Year.evaluation_year.HasValue ? lstAD.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                          active_status = lstAD.TM_PTR_Eva_Status != null ? lstAD.TM_PTR_Eva_Status.status_name_en : "", //lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          name = lstEmpReq.EmpFullName,
                                      }).ToList();
                }
                result.lstData = lstData_resutl.ToList();

            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        #endregion


        #region Save Plan 2019

        [HttpPost]
        public ActionResult Save2019DraftEvaForm(vPTREvaluation_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                        {
                            List<PTR_Evaluation_Answer> lstAns = new List<PTR_Evaluation_Answer>();
                            List<PTR_Evaluation_Incidents> lstAnsIncidents = new List<PTR_Evaluation_Incidents>();

                            List<PTR_Evaluation_KPIs> lstKPIs = new List<PTR_Evaluation_KPIs>();
                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.PTR_Evaluation_Year = _getDataYear;
                                _getData.other_roles = ItemData.other_role;
                                string[] aTIFQID = _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.questions_type == "G").Select(s => s.Id + "").ToArray();
                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null && _getEvaQuestion.TM_PTR_Eva_Questions.Any())
                                {
                                    if (ItemData.lstAnswer != null)
                                    {
                                        lstAns = (from lstA in ItemData.lstAnswer.Where(w => aTIFQID.Contains(w.id))
                                                  from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Questions())
                                                  select new PTR_Evaluation_Answer
                                                  {
                                                      update_user = CGlobal.UserInfo.UserId,
                                                      update_date = dNow,
                                                      create_user = CGlobal.UserInfo.UserId,
                                                      create_date = dNow,
                                                      answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                      active_status = "Y",
                                                      TM_PTR_Eva_Questions = lstQ != null ? lstQ : null,
                                                      PTR_Evaluation = _getData,
                                                  }).ToList();
                                    }
                                }

                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Incidents != null && _getEvaQuestion.TM_PTR_Eva_Incidents.Any())
                                {
                                    if (ItemData.lstIncidents != null)
                                    {
                                        lstAnsIncidents = (from lstA in ItemData.lstIncidents
                                                           from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Incidents())
                                                           select new PTR_Evaluation_Incidents
                                                           {
                                                               update_user = CGlobal.UserInfo.UserId,
                                                               update_date = dNow,
                                                               create_user = CGlobal.UserInfo.UserId,
                                                               create_date = dNow,
                                                               answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                               active_status = "Y",
                                                               TM_PTR_Eva_Incidents = lstQ != null ? lstQ : null,
                                                               PTR_Evaluation = _getData,
                                                           }).ToList();
                                    }
                                }



                                var sComplect = _PTR_EvaluationService.Complect(_getData);
                                if (sComplect > 0)
                                {
                                    int nRating = SystemFunction.GetIntNullToZero(ItemData.self_rating_id);
                                    var _getEvaRating = _TM_Annual_RatingService.Find(nRating);
                                    if (_getEvaRating != null)
                                    {
                                        var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self && w.active_status == "Y").FirstOrDefault();
                                        if (_getSelfApprove != null)
                                        {
                                            _getSelfApprove.PTR_Evaluation = _getData;
                                            _getSelfApprove.update_user = CGlobal.UserInfo.UserId;
                                            _getSelfApprove.update_date = dNow;
                                            _getSelfApprove.Annual_Rating = _getEvaRating;
                                            _PTR_Evaluation_ApproveService.Update(_getSelfApprove);
                                        }

                                    }
                                    if (lstAns.Any())
                                    {
                                        _PTR_Evaluation_AnswerService.UpdateAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }

                                    if (lstAnsIncidents.Any())
                                    {
                                        _PTR_Evaluation_IncidentsService.UpdateAnswer(lstAnsIncidents, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }
                                    var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                                    if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any() && _getData.PTR_Evaluation_KPIs != null)
                                    {
                                        foreach (var item in ItemData.lstKPIs)
                                        {
                                            var GetKPI = _getData.PTR_Evaluation_KPIs.Where(w => w.Id + "" == item.IdEncrypt).FirstOrDefault();
                                            if (GetKPI != null)
                                            {
                                                //if (GetKPI.TM_KPIs_Base.type_of_kpi == "N")
                                                //{
                                                //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "") * 1000000) + "");
                                                //}
                                                //else if (GetKPI.TM_KPIs_Base.type_of_kpi == "D")
                                                //{
                                                //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "")) + "");
                                                //}
                                                GetKPI.final_remark = HCMFunc.DataEncryptPES(item.remark + "");
                                                GetKPI.update_user = CGlobal.UserInfo.UserId;
                                                GetKPI.update_date = dNow;
                                                GetKPI.PTR_Evaluation = _getData;
                                                _PTR_Evaluation_KPIsService.Update(GetKPI);
                                            }
                                        }
                                    }
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Don't have permission to save.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult SaveEvaForm2019(vPTREvaluation_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Evaluation_Approval);

                    if (_GetStatus == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Status not found.";
                        return Json(new
                        {
                            result
                        });
                    }

                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        int nRating = SystemFunction.GetIntNullToZero(ItemData.self_rating_id);
                        var _getEvaRating = _TM_Annual_RatingService.Find(nRating);
                        if (_getEvaRating != null)
                        {
                            List<PTR_Evaluation_Incidents> lstAnsIncidents = new List<PTR_Evaluation_Incidents>();
                            List<PTR_Evaluation_Answer> lstAns = new List<PTR_Evaluation_Answer>();
                            List<PTR_Evaluation_KPIs> lstKPIs = new List<PTR_Evaluation_KPIs>();
                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.PTR_Evaluation_Year = _getDataYear;
                                _getData.other_roles = ItemData.other_role;
                                _getData.TM_PTR_Eva_Status = _GetStatus;
                                string[] aTIFQID = _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.questions_type == "G").Select(s => s.Id + "").ToArray();
                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null && _getEvaQuestion.TM_PTR_Eva_Questions.Any())
                                {
                                    if (ItemData.lstAnswer != null)
                                    {
                                        lstAns = (from lstA in ItemData.lstAnswer.Where(w => aTIFQID.Contains(w.id))
                                                  from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Questions())
                                                  select new PTR_Evaluation_Answer
                                                  {
                                                      update_user = CGlobal.UserInfo.UserId,
                                                      update_date = dNow,
                                                      create_user = CGlobal.UserInfo.UserId,
                                                      create_date = dNow,
                                                      answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                      active_status = "Y",
                                                      TM_PTR_Eva_Questions = lstQ != null ? lstQ : null,
                                                      PTR_Evaluation = _getData,
                                                  }).ToList();
                                    }
                                }

                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Incidents != null && _getEvaQuestion.TM_PTR_Eva_Incidents.Any())
                                {
                                    if (ItemData.lstIncidents != null)
                                    {
                                        lstAnsIncidents = (from lstA in ItemData.lstIncidents
                                                           from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Incidents())
                                                           select new PTR_Evaluation_Incidents
                                                           {
                                                               update_user = CGlobal.UserInfo.UserId,
                                                               update_date = dNow,
                                                               create_user = CGlobal.UserInfo.UserId,
                                                               create_date = dNow,
                                                               answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                               active_status = "Y",
                                                               TM_PTR_Eva_Incidents = lstQ != null ? lstQ : null,
                                                               PTR_Evaluation = _getData,
                                                           }).ToList();
                                    }
                                }

                                var sComplect = _PTR_EvaluationService.Complect(_getData);
                                if (sComplect > 0)
                                {

                                    if (_getEvaRating != null)
                                    {
                                        var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self && w.active_status == "Y").FirstOrDefault();
                                        if (_getSelfApprove != null)
                                        {
                                            _getSelfApprove.PTR_Evaluation = _getData;
                                            _getSelfApprove.update_user = CGlobal.UserInfo.UserId;
                                            _getSelfApprove.update_date = dNow;
                                            _getSelfApprove.Annual_Rating = _getEvaRating;
                                            _getSelfApprove.Approve_status = "Y";
                                            _getSelfApprove.Approve_date = dNow;
                                            _getSelfApprove.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                            sComplect = _PTR_Evaluation_ApproveService.Update(_getSelfApprove);
                                            if (sComplect > 0)
                                            {
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find(6);
                                                var GetMail = _PTR_Evaluation_ApproveService.GetEvaForMail(_getData.Id).FirstOrDefault();
                                                if (GetMail != null)
                                                {
                                                    // var bSuss = SendPTRSubmit(GetMail, Mail1, ref sError, ref mail_to_log);
                                                }

                                            }
                                        }

                                    }
                                    if (lstAns.Any())
                                    {
                                        _PTR_Evaluation_AnswerService.UpdateAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }
                                    if (lstAnsIncidents.Any())
                                    {
                                        _PTR_Evaluation_IncidentsService.UpdateAnswer(lstAnsIncidents, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }

                                    var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                                    if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any() && _getData.PTR_Evaluation_KPIs != null)
                                    {
                                        foreach (var item in ItemData.lstKPIs)
                                        {
                                            var GetKPI = _getData.PTR_Evaluation_KPIs.Where(w => w.Id + "" == item.IdEncrypt).FirstOrDefault();
                                            if (GetKPI != null)
                                            {
                                                //if (GetKPI.TM_KPIs_Base.type_of_kpi == "N")
                                                //{
                                                //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "") * 1000000) + "");
                                                //}
                                                //else if (GetKPI.TM_KPIs_Base.type_of_kpi == "D")
                                                //{
                                                //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "")) + "");
                                                //}
                                                GetKPI.final_remark = HCMFunc.DataEncryptPES(item.remark + "");
                                                GetKPI.update_user = CGlobal.UserInfo.UserId;
                                                GetKPI.update_date = dNow;
                                                GetKPI.PTR_Evaluation = _getData;
                                                _PTR_Evaluation_KPIsService.Update(GetKPI);
                                            }
                                        }
                                    }
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Rating not found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Pleace select rating.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult EditEvaForm2019(vPTREvaluation_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Evaluation_Approval);

                    if (_GetStatus == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Status not found.";
                        return Json(new
                        {
                            result
                        });
                    }

                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {

                        dNow = _getData.update_date.Value;
                        int nRating = SystemFunction.GetIntNullToZero(ItemData.self_rating_id);
                        var _getEvaRating = _TM_Annual_RatingService.Find(nRating);
                        if (_getEvaRating != null)
                        {
                            List<PTR_Evaluation_Incidents> lstAnsIncidents = new List<PTR_Evaluation_Incidents>();
                            List<PTR_Evaluation_Answer> lstAns = new List<PTR_Evaluation_Answer>();
                            List<PTR_Evaluation_KPIs> lstKPIs = new List<PTR_Evaluation_KPIs>();
                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {

                                _getData.PTR_Evaluation_Year = _getDataYear;
                                _getData.other_roles = ItemData.other_role;
                                _getData.TM_PTR_Eva_Status = _GetStatus;
                                string[] aTIFQID = _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.questions_type == "G").Select(s => s.Id + "").ToArray();
                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null && _getEvaQuestion.TM_PTR_Eva_Questions.Any())
                                {
                                    if (ItemData.lstAnswer != null)
                                    {
                                        lstAns = (from lstA in ItemData.lstAnswer.Where(w => aTIFQID.Contains(w.id))
                                                  from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Questions())
                                                  select new PTR_Evaluation_Answer
                                                  {
                                                      update_user = CGlobal.UserInfo.UserId,
                                                      update_date = dNow,
                                                      create_user = CGlobal.UserInfo.UserId,
                                                      create_date = dNow,
                                                      answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                      active_status = "Y",
                                                      TM_PTR_Eva_Questions = lstQ != null ? lstQ : null,
                                                      PTR_Evaluation = _getData,
                                                  }).ToList();
                                    }
                                }

                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Incidents != null && _getEvaQuestion.TM_PTR_Eva_Incidents.Any())
                                {
                                    if (ItemData.lstIncidents != null)
                                    {
                                        lstAnsIncidents = (from lstA in ItemData.lstIncidents
                                                           from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Incidents())
                                                           select new PTR_Evaluation_Incidents
                                                           {
                                                               update_user = CGlobal.UserInfo.UserId,
                                                               update_date = dNow,
                                                               create_user = CGlobal.UserInfo.UserId,
                                                               create_date = dNow,
                                                               answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                               active_status = "Y",
                                                               TM_PTR_Eva_Incidents = lstQ != null ? lstQ : null,
                                                               PTR_Evaluation = _getData,
                                                           }).ToList();
                                    }
                                }

                                var sComplect = _PTR_EvaluationService.Complect(_getData);
                                if (sComplect > 0 || 1 == 1)
                                {

                                    if (_getEvaRating != null)
                                    {
                                        var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self && w.active_status == "Y").FirstOrDefault();
                                        if (_getSelfApprove != null)
                                        {
                                            _getSelfApprove.PTR_Evaluation = _getData;
                                            _getSelfApprove.update_user = CGlobal.UserInfo.UserId;
                                            _getSelfApprove.update_date = dNow;
                                            _getSelfApprove.Annual_Rating = _getEvaRating;
                                            _getSelfApprove.Approve_status = "Y";
                                            _getSelfApprove.Approve_date = dNow;
                                            _getSelfApprove.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                            sComplect = _PTR_Evaluation_ApproveService.Update(_getSelfApprove);
                                            if (sComplect > 0)
                                            {

                                            }
                                        }

                                    }
                                    if (lstAns.Any())
                                    {
                                        _PTR_Evaluation_AnswerService.UpdateAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }
                                    if (lstAnsIncidents.Any())
                                    {
                                        _PTR_Evaluation_IncidentsService.UpdateAnswer(lstAnsIncidents, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }

                                    var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                                    if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any() && _getData.PTR_Evaluation_KPIs != null)
                                    {
                                        foreach (var item in ItemData.lstKPIs)
                                        {
                                            var GetKPI = _getData.PTR_Evaluation_KPIs.Where(w => w.Id + "" == item.IdEncrypt).FirstOrDefault();
                                            if (GetKPI != null)
                                            {
                                                //if (GetKPI.TM_KPIs_Base.type_of_kpi == "N")
                                                //{
                                                //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "") * 1000000) + "");
                                                //}
                                                //else if (GetKPI.TM_KPIs_Base.type_of_kpi == "D")
                                                //{
                                                //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "")) + "");
                                                //}
                                                GetKPI.final_remark = HCMFunc.DataEncryptPES(item.remark + "");
                                                GetKPI.update_user = CGlobal.UserInfo.UserId;
                                                GetKPI.update_date = dNow;
                                                GetKPI.PTR_Evaluation = _getData;
                                                _PTR_Evaluation_KPIsService.Update(GetKPI);
                                            }
                                        }
                                    }
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Rating not found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Pleace select rating.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }

        #endregion

        #region Save Eva 2020

        [HttpPost]
        public ActionResult Save2020DraftEvaForm(vPTREvaluation_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                        {
                            List<PTR_Evaluation_Answer> lstAns = new List<PTR_Evaluation_Answer>();
                            List<PTR_Evaluation_Incidents> lstAnsIncidents = new List<PTR_Evaluation_Incidents>();

                            List<PTR_Evaluation_KPIs> lstKPIs = new List<PTR_Evaluation_KPIs>();
                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.PTR_Evaluation_Year = _getDataYear;
                                _getData.other_roles = ItemData.other_role;
                                string[] aTIFQID = _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.questions_type == "G").Select(s => s.Id + "").ToArray();
                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null && _getEvaQuestion.TM_PTR_Eva_Questions.Any())
                                {
                                    if (ItemData.lstAnswer != null)
                                    {
                                        lstAns = (from lstA in ItemData.lstAnswer.Where(w => aTIFQID.Contains(w.id))
                                                  from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Questions())
                                                  select new PTR_Evaluation_Answer
                                                  {
                                                      update_user = CGlobal.UserInfo.UserId,
                                                      update_date = dNow,
                                                      create_user = CGlobal.UserInfo.UserId,
                                                      create_date = dNow,
                                                      answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                      active_status = "Y",
                                                      TM_PTR_Eva_Questions = lstQ != null ? lstQ : null,
                                                      PTR_Evaluation = _getData,
                                                  }).ToList();
                                    }
                                }

                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Incidents != null && _getEvaQuestion.TM_PTR_Eva_Incidents.Any())
                                {
                                    if (ItemData.lstIncidents != null)
                                    {
                                        lstAnsIncidents = (from lstA in ItemData.lstIncidents
                                                           from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Incidents())
                                                           select new PTR_Evaluation_Incidents
                                                           {
                                                               update_user = CGlobal.UserInfo.UserId,
                                                               update_date = dNow,
                                                               create_user = CGlobal.UserInfo.UserId,
                                                               create_date = dNow,
                                                               answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                               active_status = "Y",
                                                               TM_PTR_Eva_Incidents = lstQ != null ? lstQ : null,
                                                               PTR_Evaluation = _getData,
                                                           }).ToList();
                                    }
                                }



                                var sComplect = _PTR_EvaluationService.Complect(_getData);
                                if (sComplect > 0)
                                {
                                    int nRating = SystemFunction.GetIntNullToZero(ItemData.self_rating_id);
                                    var _getEvaRating = _TM_Annual_RatingService.Find(nRating);
                                    if (_getEvaRating != null)
                                    {
                                        var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self && w.active_status == "Y").FirstOrDefault();
                                        if (_getSelfApprove != null)
                                        {
                                            _getSelfApprove.PTR_Evaluation = _getData;
                                            _getSelfApprove.update_user = CGlobal.UserInfo.UserId;
                                            _getSelfApprove.update_date = dNow;
                                            _getSelfApprove.Annual_Rating = _getEvaRating;
                                            _PTR_Evaluation_ApproveService.Update(_getSelfApprove);
                                        }

                                    }
                                    if (lstAns.Any())
                                    {
                                        _PTR_Evaluation_AnswerService.UpdateAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }

                                    if (lstAnsIncidents.Any())
                                    {
                                        _PTR_Evaluation_IncidentsService.UpdateAnswer(lstAnsIncidents, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }
                                    var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                                    if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any() && _getData.PTR_Evaluation_KPIs != null)
                                    {
                                        foreach (var item in ItemData.lstKPIs)
                                        {
                                            var GetKPI = _getData.PTR_Evaluation_KPIs.Where(w => w.Id + "" == item.IdEncrypt).FirstOrDefault();
                                            if (GetKPI != null)
                                            {
                                                //if (GetKPI.TM_KPIs_Base.type_of_kpi == "N")
                                                //{
                                                //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "") * 1000000) + "");
                                                //}
                                                //else if (GetKPI.TM_KPIs_Base.type_of_kpi == "D")
                                                //{
                                                //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "")) + "");
                                                //}
                                                GetKPI.final_remark = HCMFunc.DataEncryptPES(item.remark + "");
                                                GetKPI.update_user = CGlobal.UserInfo.UserId;
                                                GetKPI.update_date = dNow;
                                                GetKPI.PTR_Evaluation = _getData;
                                                _PTR_Evaluation_KPIsService.Update(GetKPI);
                                            }
                                        }
                                    }
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Don't have permission to save.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult SaveEvaForm2020(vPTREvaluation_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Evaluation_Approval);

                    if (_GetStatus == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Status not found.";
                        return Json(new
                        {
                            result
                        });
                    }

                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {
                        int nRating = SystemFunction.GetIntNullToZero(ItemData.self_rating_id);
                        var _getEvaRating = _TM_Annual_RatingService.Find(nRating);
                        if (_getEvaRating != null)
                        {
                            List<PTR_Evaluation_Incidents> lstAnsIncidents = new List<PTR_Evaluation_Incidents>();
                            List<PTR_Evaluation_Answer> lstAns = new List<PTR_Evaluation_Answer>();
                            List<PTR_Evaluation_KPIs> lstKPIs = new List<PTR_Evaluation_KPIs>();
                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.PTR_Evaluation_Year = _getDataYear;
                                _getData.other_roles = ItemData.other_role;
                                if (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Evaluation_Completed)
                                    _getData.TM_PTR_Eva_Status = _getData.TM_PTR_Eva_Status;
                                else
                                    _getData.TM_PTR_Eva_Status = _GetStatus;
                                string[] aTIFQID = _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.questions_type == "G").Select(s => s.Id + "").ToArray();
                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null && _getEvaQuestion.TM_PTR_Eva_Questions.Any())
                                {
                                    if (ItemData.lstAnswer != null)
                                    {
                                        lstAns = (from lstA in ItemData.lstAnswer.Where(w => aTIFQID.Contains(w.id))
                                                  from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Questions())
                                                  select new PTR_Evaluation_Answer
                                                  {
                                                      update_user = CGlobal.UserInfo.UserId,
                                                      update_date = dNow,
                                                      create_user = CGlobal.UserInfo.UserId,
                                                      create_date = dNow,
                                                      answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                      active_status = "Y",
                                                      TM_PTR_Eva_Questions = lstQ != null ? lstQ : null,
                                                      PTR_Evaluation = _getData,
                                                  }).ToList();
                                    }
                                }

                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Incidents != null && _getEvaQuestion.TM_PTR_Eva_Incidents.Any())
                                {
                                    if (ItemData.lstIncidents != null)
                                    {
                                        lstAnsIncidents = (from lstA in ItemData.lstIncidents
                                                           from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Incidents())
                                                           select new PTR_Evaluation_Incidents
                                                           {
                                                               update_user = CGlobal.UserInfo.UserId,
                                                               update_date = dNow,
                                                               create_user = CGlobal.UserInfo.UserId,
                                                               create_date = dNow,
                                                               answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                               active_status = "Y",
                                                               TM_PTR_Eva_Incidents = lstQ != null ? lstQ : null,
                                                               PTR_Evaluation = _getData,
                                                           }).ToList();
                                    }
                                }

                                var sComplect = _PTR_EvaluationService.Complect(_getData);
                                if (sComplect > 0)
                                {

                                    if (_getEvaRating != null)
                                    {
                                        var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self && w.active_status == "Y").FirstOrDefault();
                                        if (_getSelfApprove != null)
                                        {
                                            _getSelfApprove.PTR_Evaluation = _getData;
                                            _getSelfApprove.update_user = CGlobal.UserInfo.UserId;
                                            _getSelfApprove.update_date = dNow;
                                            if (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Evaluation_Completed)
                                                _getSelfApprove.Annual_Rating = _getSelfApprove.Annual_Rating;
                                            else
                                            _getSelfApprove.Annual_Rating = _getEvaRating;

                                            _getSelfApprove.Approve_status = "Y";
                                            _getSelfApprove.Approve_date = dNow;
                                            _getSelfApprove.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                            sComplect = _PTR_Evaluation_ApproveService.Update(_getSelfApprove);
                                            if (sComplect > 0)
                                            {
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find(47);
                                                var GetMail = _PTR_Evaluation_ApproveService.GetEvaForMail(_getData.Id).FirstOrDefault();
                                                if (GetMail != null)
                                                {
                                                    var bSuss = SendPTRSubmitNewVersion(GetMail, Mail1, ref sError, ref mail_to_log);
                                                }

                                            }
                                        }

                                    }
                                    if (lstAns.Any())
                                    {
                                        _PTR_Evaluation_AnswerService.UpdateAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }
                                    if (lstAnsIncidents.Any())
                                    {
                                        _PTR_Evaluation_IncidentsService.UpdateAnswer(lstAnsIncidents, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }

                                    var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                                    if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any() && _getData.PTR_Evaluation_KPIs != null)
                                    {
                                        foreach (var item in ItemData.lstKPIs)
                                        {
                                            var GetKPI = _getData.PTR_Evaluation_KPIs.Where(w => w.Id + "" == item.IdEncrypt).FirstOrDefault();
                                            if (GetKPI != null)
                                            {
                                                //if (GetKPI.TM_KPIs_Base.type_of_kpi == "N")
                                                //{
                                                //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "") * 1000000) + "");
                                                //}
                                                //else if (GetKPI.TM_KPIs_Base.type_of_kpi == "D")
                                                //{
                                                //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "")) + "");
                                                //}
                                                GetKPI.final_remark = HCMFunc.DataEncryptPES(item.remark + "");
                                                GetKPI.update_user = CGlobal.UserInfo.UserId;
                                                GetKPI.update_date = dNow;
                                                GetKPI.PTR_Evaluation = _getData;
                                                _PTR_Evaluation_KPIsService.Update(GetKPI);
                                            }
                                        }
                                    }
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Rating not found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Pleace select rating.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult EditEvaForm2020(vPTREvaluation_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _GetStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Evaluation_Approval);

                    if (_GetStatus == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Status not found.";
                        return Json(new
                        {
                            result
                        });
                    }

                    var _getData = _PTR_EvaluationService.Find(nId);
                    if (_getData != null)
                    {

                        dNow = _getData.update_date.Value;
                        int nRating = SystemFunction.GetIntNullToZero(ItemData.self_rating_id);
                        var _getEvaRating = _TM_Annual_RatingService.Find(nRating);
                        if (_getEvaRating != null)
                        {
                            List<PTR_Evaluation_Incidents> lstAnsIncidents = new List<PTR_Evaluation_Incidents>();
                            List<PTR_Evaluation_Answer> lstAns = new List<PTR_Evaluation_Answer>();
                            List<PTR_Evaluation_KPIs> lstKPIs = new List<PTR_Evaluation_KPIs>();
                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {

                                _getData.PTR_Evaluation_Year = _getDataYear;
                                _getData.other_roles = ItemData.other_role;
                                _getData.TM_PTR_Eva_Status = _GetStatus;
                                string[] aTIFQID = _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.questions_type == "G").Select(s => s.Id + "").ToArray();
                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null && _getEvaQuestion.TM_PTR_Eva_Questions.Any())
                                {
                                    if (ItemData.lstAnswer != null)
                                    {
                                        lstAns = (from lstA in ItemData.lstAnswer.Where(w => aTIFQID.Contains(w.id))
                                                  from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Questions())
                                                  select new PTR_Evaluation_Answer
                                                  {
                                                      update_user = CGlobal.UserInfo.UserId,
                                                      update_date = dNow,
                                                      create_user = CGlobal.UserInfo.UserId,
                                                      create_date = dNow,
                                                      answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                      active_status = "Y",
                                                      TM_PTR_Eva_Questions = lstQ != null ? lstQ : null,
                                                      PTR_Evaluation = _getData,
                                                  }).ToList();
                                    }
                                }

                                if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Incidents != null && _getEvaQuestion.TM_PTR_Eva_Incidents.Any())
                                {
                                    if (ItemData.lstIncidents != null)
                                    {
                                        lstAnsIncidents = (from lstA in ItemData.lstIncidents
                                                           from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Incidents())
                                                           select new PTR_Evaluation_Incidents
                                                           {
                                                               update_user = CGlobal.UserInfo.UserId,
                                                               update_date = dNow,
                                                               create_user = CGlobal.UserInfo.UserId,
                                                               create_date = dNow,
                                                               answer = HCMFunc.DataEncryptPES(lstA.remark + ""),
                                                               active_status = "Y",
                                                               TM_PTR_Eva_Incidents = lstQ != null ? lstQ : null,
                                                               PTR_Evaluation = _getData,
                                                           }).ToList();
                                    }
                                }

                                var sComplect = _PTR_EvaluationService.Complect(_getData);
                                if (sComplect > 0 || 1 == 1)
                                {

                                    if (_getEvaRating != null)
                                    {
                                        var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self && w.active_status == "Y").FirstOrDefault();
                                        if (_getSelfApprove != null)
                                        {
                                            _getSelfApprove.PTR_Evaluation = _getData;
                                            _getSelfApprove.update_user = CGlobal.UserInfo.UserId;
                                            _getSelfApprove.update_date = dNow;
                                            _getSelfApprove.Annual_Rating = _getEvaRating;
                                            _getSelfApprove.Approve_status = "Y";
                                            _getSelfApprove.Approve_date = dNow;
                                            _getSelfApprove.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                            sComplect = _PTR_Evaluation_ApproveService.Update(_getSelfApprove);
                                            if (sComplect > 0)
                                            {

                                            }
                                        }

                                    }
                                    if (lstAns.Any())
                                    {
                                        _PTR_Evaluation_AnswerService.UpdateAnswer(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }
                                    if (lstAnsIncidents.Any())
                                    {
                                        _PTR_Evaluation_IncidentsService.UpdateAnswer(lstAnsIncidents, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                    }

                                    var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                                    if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any() && _getData.PTR_Evaluation_KPIs != null)
                                    {
                                        foreach (var item in ItemData.lstKPIs)
                                        {
                                            var GetKPI = _getData.PTR_Evaluation_KPIs.Where(w => w.Id + "" == item.IdEncrypt).FirstOrDefault();
                                            if (GetKPI != null)
                                            {
                                                //if (GetKPI.TM_KPIs_Base.type_of_kpi == "N")
                                                //{
                                                //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "") * 1000000) + "");
                                                //}
                                                //else if (GetKPI.TM_KPIs_Base.type_of_kpi == "D")
                                                //{
                                                //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "")) + "");
                                                //}
                                                GetKPI.final_remark = HCMFunc.DataEncryptPES(item.remark + "");
                                                GetKPI.update_user = CGlobal.UserInfo.UserId;
                                                GetKPI.update_date = dNow;
                                                GetKPI.PTR_Evaluation = _getData;
                                                _PTR_Evaluation_KPIsService.Update(GetKPI);
                                            }
                                        }
                                    }
                                    result.Status = SystemFunction.process_Success;
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please try again.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Rating not found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Pleace select rating.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Data not found.";
            }

            return Json(new
            {
                result
            });
        }

        #endregion


        //[HttpPost]
        //public ActionResult EditPTREvaluation(vPTREvaluation_obj_save ItemData)
        //{
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    vPTREvaluation_Return result = new vPTREvaluation_Return();
        //    if (ItemData != null)
        //    {
        //        DateTime dNow = DateTime.Now;
        //        int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
        //        if (nId != 0)
        //        {
        //            var _getData = _TM_Candidate_RankService.Find(nId);
        //            if (_getData != null)
        //            {
        //                if (!string.IsNullOrEmpty(ItemData.name_en))
        //                {

        //                    _getData.update_user = CGlobal.UserInfo.UserId;
        //                    _getData.update_date = dNow;
        //                    _getData.active_status = ItemData.active_status;
        //                    _getData.crank_name_en = ItemData.name_en;
        //                    _getData.crank_short_name_en = ItemData.short_name_en;
        //                    _getData.crank_description = ItemData.description;
        //                    _getData.piority = Convert.ToInt32(SystemFunction.GetNumberNullToZero(ItemData.piority));
        //                    if (_TM_Candidate_RankService.CanSave(_getData))
        //                    {
        //                        var sComplect = _TM_Candidate_RankService.Update(_getData);
        //                        if (sComplect > 0)
        //                        {
        //                            result.Status = SystemFunction.process_Success;
        //                        }
        //                        else
        //                        {
        //                            result.Status = SystemFunction.process_Failed;
        //                            result.Msg = "Error, please try again.";
        //                        }
        //                    }
        //                    else
        //                    {
        //                        result.Status = SystemFunction.process_Duplicate;
        //                        result.Msg = "Duplicate Type name.";
        //                    }
        //                }
        //                else
        //                {
        //                    result.Status = SystemFunction.process_Failed;
        //                    result.Msg = "Error, please enter name";
        //                }
        //            }
        //            else
        //            {
        //                result.Status = SystemFunction.process_Failed;
        //                result.Msg = "Error, Request Type Not Found.";
        //            }
        //        }
        //    }

        //    return Json(new
        //    {
        //        result
        //    });
        //}
        #endregion

        private static string FormatNumber(double num)
        {
            // Ensure number has max 3 significant digits (no rounding up can happen)
            double i = (double)Math.Pow(10, (int)Math.Max(0, Math.Log10(num) - 3));
            num = num / i * i;

            //if (num >= 1000000000)
            //    return (num / 1000000000D).ToString("0.##") + "";
            if (num >= 1000000)
                return (num / 1000000D).ToString("0.##") + "";
            if (num >= 1000)
                return (num / 1000D).ToString("0.##") + "";

            return num.ToString("#,0");
        }
    }
}