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
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HumanCapitalManagement.App_Start.PESClass;

namespace HumanCapitalManagement.Controllers.PartnerEvaluation
{
    public class PTRApproveController : BaseController
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
        private TM_PTR_E_Mail_HistoryService _TM_PTR_E_Mail_HistoryService;
        private PTR_Evaluation_Incidents_ScoreService _PTR_Evaluation_Incidents_ScoreService;
        private MailContentService _MailContentService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public PTRApproveController(PTR_Evaluation_YearService PTR_Evaluation_YearService
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
            , TM_PTR_E_Mail_HistoryService TM_PTR_E_Mail_HistoryService
           , MailContentService MailContentService
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
            _TM_PTR_E_Mail_HistoryService = TM_PTR_E_Mail_HistoryService;
            _MailContentService = MailContentService;
            _PTR_Evaluation_Incidents_ScoreService = PTR_Evaluation_Incidents_ScoreService;
        }
        // GET: PTRApprove
        public ActionResult PTRApproveList(string qryStr)
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
                var lstData = _PTR_Evaluation_ApproveService.GetEvaForApprove(CGlobal.UserInfo.EmployeeNo, nYear, CGlobal.UserIsAdminPES());

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.fy_year = SearchItem.fy_year;
                    string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.PTR_Evaluation.user_no).ToArray();
                    string[] aApproveUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.PTR_Evaluation.update_user).ToArray();
                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                    var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    result.lstData = (from lstAD in lstData
                                      from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.PTR_Evaluation.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == lstAD.PTR_Evaluation.update_user).DefaultIfEmpty(new AllInfo_WS())
                                      select new vPTREvaluation_obj
                                      {
                                          srank = lstEmpReq.RankCode,
                                          sgroup = lstEmpReq.UnitGroupName,
                                          name_en = lstAD.PTR_Evaluation.user_no + " : " + lstEmpReq.EmpFullName,
                                          fy_year = lstAD.PTR_Evaluation.PTR_Evaluation_Year.evaluation_year.HasValue ? lstAD.PTR_Evaluation.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                          active_status = lstAD.PTR_Evaluation.TM_PTR_Eva_Status != null ? lstAD.PTR_Evaluation.TM_PTR_Eva_Status.status_name_en : "", //lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.PTR_Evaluation.update_date.HasValue ? lstAD.PTR_Evaluation.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          approval = AEmp.EmpFullName + (lstAD.TM_PTR_Eva_ApproveStep != null ? "(" + lstAD.TM_PTR_Eva_ApproveStep.Step_name_en + ")" : ""),
                                          name = lstEmpReq.EmpFullName,
                                          approve_status = lstAD.Approve_status + "" == "Y" ? "Completed" : "",
                                      }).ToList();
                }
            }
            else
            {
                var _getYear = _PTR_Evaluation_YearService.FindNowYear();
                if (_getYear != null)
                {
                    //result.fy_year = _getYear.Id + "";
                }
            }
            #endregion
            return View(result);
        }
        public ActionResult PTRApproveCreate()
        {
            var sCheck = pesCheckLoginAndPermission(true);
            if (sCheck != null)
            {
                return sCheck;
            }

            return View();
        }
        public ActionResult PTRApproveEdit(string qryStr)
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

                    List<int> lstStatusEva = new List<int>();
                    lstStatusEva.Add((int)Eva_Status.Draft_Evaluate);
                    lstStatusEva.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
                    lstStatusEva.Add((int)Eva_Status.Waiting_for_Revised_Evaluate);
                    lstStatusEva.Add((int)Eva_Status.Evaluation_Completed);

                    // current step
                    //_GetApprove.Req_Approve_user
                    var _GetApprove = _PTR_Evaluation_ApproveService.Find(nId);
                    if (_GetApprove != null)
                    {
                        var _getData = _GetApprove.PTR_Evaluation;
                        if ((_GetApprove.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES()) && _getData != null)
                        {
                            if (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Evaluation_Approval)
                            {
                                var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                                if (_checkActiv != null)
                                {
                                    var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                                    result.is_ceo = _GetApprove.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Ceo ? "Y" : "N";
                                    //set default
                                    result.approve_remark = HCMFunc.DataDecryptPES(_GetApprove.responses + "");
                                    result.approve_rating_id = _GetApprove.Annual_Rating != null ? _GetApprove.Annual_Rating.Id + "" : "";
                                    if (_GetApprove.Approve_status == "Y")
                                    {
                                        result.is_update_by_approval = "Y";

                                    }

                                    result.IdEncrypt = qryStr;
                                    result.code = _getData.user_no;
                                    result.sname = _checkActiv.EmpFullName;
                                    result.sgroup = _checkActiv.UnitGroupName;
                                    result.srank = _checkActiv.Rank;

                                    result.revise_comment = _getData.comments;
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
                                            Edit = _getData.TM_PTR_Eva_Status.Id != (int)Eva_Status.Draft_Plan ? "" + nFile++ : @"<button id=""btnEdit""  type=""button"" onclick=""DeleteFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                            View = @"<button id=""btnEdit""  type=""button"" onclick=""LoadFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-success""><i class=""glyphicon glyphicon-download-alt""></i></button>",
                                        }).ToList();
                                    }
                                    #region if else Data
                                    if (lstStatusEva.Contains(_getData.TM_PTR_Eva_Status.Id))
                                    {
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

                                                                  target_data = lst.TM_KPIs_Base.type_of_kpi == "P"
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

                                        if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Questions != null)
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
                                                                        sdisable = ((_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) && lstQ.questions_type == "G") ? "N" : "Y",
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
                                                                        sdisable = ((_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) && lstQ.questions_type == "G") ? "N" : "Y",
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
                                                                        id = lstQ.Id + "",
                                                                        question = lstQ.question,
                                                                        header = lstQ.header,
                                                                        nSeq = lstQ.seq,
                                                                        remark = "",
                                                                        sgroup = lstQ.questions_type + "" == "P" ? "Personal Performance Plan" : "1. Top 5 Priorities and Achievement",
                                                                        sdisable = ((_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate) && lstQ.questions_type == "G") ? "N" : "Y",
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
                                        if (_getEvaQuestion != null && _getEvaQuestion.TM_PTR_Eva_Incidents != null)
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
                                                                           sdisable = ((_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate)) ? "N" : "Y",
                                                                       }).ToList();


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
                                                                           sdisable = ((_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate)) ? "N" : "Y",
                                                                       }).ToList();


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

                                            result.lstFinal = (from lst in _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G")

                                                               select new vPTREvaluation_Final
                                                               {
                                                                   position = lst.TM_PTR_Eva_ApproveStep.Step_name_en,
                                                                   status = lst.Annual_Rating != null ? lst.Annual_Rating.rating_name_en : "",
                                                                   nSeq = lst.TM_PTR_Eva_ApproveStep.seq,
                                                                   remark = HCMFunc.DataDecryptPES(lst.responses),

                                                               }).ToList();
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
                                                             isCurrent = (_GetApprove.TM_PTR_Eva_ApproveStep.Id == item.TM_PTR_Eva_ApproveStep.Id) ? "Y" : "N",

                                                             nstep = item.TM_PTR_Eva_ApproveStep != null ? item.TM_PTR_Eva_ApproveStep.seq : 0,
                                                             id = lstQ.Id + "",
                                                             question = lstQ.question,
                                                             header = lstQ.header,
                                                             nSeq = lstQ.seq,
                                                             sgroup = item.TM_PTR_Eva_ApproveStep != null ? item.TM_PTR_Eva_ApproveStep.Step_name_en + "" : "",
                                                             sExcellent = "N",
                                                             sHigh = "N",
                                                             sNI = "N",
                                                             sLow = "N",
                                                             sMeet = "N",
                                                             nscore = lstQ.nscore + "",
                                                             nrate = GetRatescore.Where(w => w.active_status == "Y" && w.TM_PTR_Eva_Incidents_Id == lstQ.Id && w.PTR_Evaluation_Approve_Id == item.Id).Select(s => s.TM_PTR_Eva_Incidents_Score_Id).FirstOrDefault(),
                                                             sdisable = ((_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Evaluate || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Evaluate)) ? "N" : "Y",
                                                             user = item.Req_Approve_user
                                                         }).ToList()
                                                        );


                                                }
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

                                        if (_GetApprove.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Ceo)
                                        {
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
                                        return View("PTRApproveEdit19", result);
                                    }
                                    else if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2020)
                                    {
                                        return View("PTRApproveEdit20", result);
                                    }
                                    else
                                    {
                                        return View(result);
                                    }
                                }
                                else
                                {
                                    return RedirectToAction("ErrorNopermission", "MasterPage");
                                }
                            }
                            else
                            {
                                return RedirectToAction("PTRApproveList", "PTRApprove");
                            }

                        }
                        else
                        {
                            return RedirectToAction("PTRApproveList", "PTRApprove");
                        }
                    }
                    else
                    {
                        return RedirectToAction("PTRApproveList", "PTRApprove");
                    }
                }
                else
                {
                    return RedirectToAction("PTRApproveList", "PTRApprove");
                }
            }
            else
            {
                return RedirectToAction("PTRApproveList", "PTRApprove");
            }
            // return View(result);

            #endregion
        }

        #region #region Performance Plan
        public ActionResult PTRPlanApproveList(string qryStr)
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
                var lstData = _PTR_Evaluation_ApproveService.GetPlanForApprove(CGlobal.UserInfo.EmployeeNo, nYear, CGlobal.UserIsAdminPES());

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.fy_year = SearchItem.fy_year;
                    string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.PTR_Evaluation.user_no).ToArray();
                    string[] aApproveUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.PTR_Evaluation.update_user).ToArray();
                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                    var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    result.lstData = (from lstAD in lstData
                                      from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.PTR_Evaluation.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == lstAD.PTR_Evaluation.update_user).DefaultIfEmpty(new AllInfo_WS())
                                      select new vPTREvaluation_obj
                                      {
                                          srank = lstEmpReq.RankCode,
                                          sgroup = lstEmpReq.UnitGroupName,
                                          name_en = lstAD.PTR_Evaluation.user_no + " : " + lstEmpReq.EmpFullName,
                                          fy_year = lstAD.PTR_Evaluation.PTR_Evaluation_Year.evaluation_year.HasValue ? lstAD.PTR_Evaluation.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                          active_status = lstAD.PTR_Evaluation.TM_PTR_Eva_Status != null ? lstAD.PTR_Evaluation.TM_PTR_Eva_Status.status_name_en : "", //lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.PTR_Evaluation.update_date.HasValue ? lstAD.PTR_Evaluation.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          approval = AEmp.EmpFullName,
                                          name = lstEmpReq.EmpFullName,
                                      }).ToList();
                }
            }
            else
            {
                var _getYear = _PTR_Evaluation_YearService.FindNowYear();
                if (_getYear != null)
                {
                    //result.fy_year = _getYear.Id + "";
                }
            }
            #endregion
            return View(result);
        }

        public ActionResult PTRPlanApproveEdit(string qryStr)
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

                    List<int> lstStatusEva = new List<int>();
                    lstStatusEva.Add((int)Eva_Status.Draft_Evaluate);
                    lstStatusEva.Add((int)Eva_Status.Waiting_for_Evaluation_Approval);
                    lstStatusEva.Add((int)Eva_Status.Waiting_for_Revised_Evaluate);
                    lstStatusEva.Add((int)Eva_Status.Evaluation_Completed);


                    var _GetApprove = _PTR_Evaluation_ApproveService.Find(nId);
                    if (_GetApprove != null)
                    {
                        var _getData = _GetApprove.PTR_Evaluation;
                        if (_GetApprove.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES() && _getData != null)
                        {
                            if (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Evaluation_Approval || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Planning_Approval)
                            {
                                var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                                if (_checkActiv != null)
                                {
                                    var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                                    result.is_ceo = _GetApprove.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Ceo ? "Y" : "N";
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
                                            Edit = _getData.TM_PTR_Eva_Status.Id != (int)Eva_Status.Draft_Plan ? "" + nFile++ : @"<button id=""btnEdit""  type=""button"" onclick=""DeleteFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
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
                                            howto = HCMFunc.DataDecryptPES(s.how_to),
                                            sdisable = (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Draft_Plan || _getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Waiting_for_Revised_Plan) ? "N" : "Y",
                                            IdEncrypt = s.Id + "",
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
                                    #endregion


                                    #endregion

                                    if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2018)
                                    {
                                        return View(result);
                                    }
                                    else if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019)
                                    {
                                        return View(result);
                                    }
                                    else if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2020)
                                    {
                                        return View("PTRPlanApproveEdit20", result);
                                    }
                                    else
                                    {
                                        return View(result);
                                    }
                                }
                                else
                                {
                                    return RedirectToAction("ErrorNopermission", "MasterPage");
                                }
                            }
                            else
                            {
                                return RedirectToAction("PTRApproveList", "PTRApprove");
                            }

                        }
                        else
                        {
                            return RedirectToAction("PTRApproveList", "PTRApprove");
                        }
                    }
                    else
                    {
                        return RedirectToAction("PTRApproveList", "PTRApprove");
                    }
                }
                else
                {
                    return RedirectToAction("PTRApproveList", "PTRApprove");
                }
            }
            else
            {
                return RedirectToAction("PTRApproveList", "PTRApprove");
            }
            // return View(result);

            #endregion
        }
        #endregion
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadPTRApproveList(CSearchPTREvaluation SearchItem)
        {
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            List<vPTREvaluation_obj> lstData_resutl = new List<vPTREvaluation_obj>();
            int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
            var lstData = _PTR_Evaluation_ApproveService.GetEvaForApprove(CGlobal.UserInfo.EmployeeNo, nYear, CGlobal.UserIsAdminPES());
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
                string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.PTR_Evaluation.user_no).ToArray();
                string[] aApproveUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                string[] aUpdateUser = lstData.Select(s => s.PTR_Evaluation.update_user).ToArray();

                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                lstData_resutl = (from lstAD in lstData
                                  from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.PTR_Evaluation.user_no).DefaultIfEmpty(new AllInfo_WS())
                                  from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == lstAD.PTR_Evaluation.update_user).DefaultIfEmpty(new AllInfo_WS())
                                  select new vPTREvaluation_obj
                                  {
                                      srank = lstEmpReq.RankCode,
                                      sgroup = lstEmpReq.UnitGroupName,
                                      name_en = lstAD.PTR_Evaluation.user_no + " : " + lstEmpReq.EmpFullName,
                                      fy_year = lstAD.PTR_Evaluation.PTR_Evaluation_Year.evaluation_year.HasValue ? lstAD.PTR_Evaluation.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                      active_status = lstAD.PTR_Evaluation.TM_PTR_Eva_Status != null ? lstAD.PTR_Evaluation.TM_PTR_Eva_Status.status_name_en : "", //lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.PTR_Evaluation.update_date.HasValue ? lstAD.PTR_Evaluation.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      approval = AEmp.EmpFullName + (lstAD.TM_PTR_Eva_ApproveStep != null ? "(" + lstAD.TM_PTR_Eva_ApproveStep.Step_name_en + ")" : ""),
                                      name = lstEmpReq.EmpFullName,
                                      approve_status = lstAD.Approve_status + "" == "Y" ? "Completed" : "",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult ApproveEvaluation(vPTREvaluation_obj_save ItemData, string sMode)
        {
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt) && !string.IsNullOrEmpty(sMode) && (!string.IsNullOrEmpty(ItemData.approve_rating_id) || sMode == "N"))
                {
                    if (sMode == "N" && (ItemData.revise_remark + "").Trim() == "")
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Please provide the remark why you are revise this Evaluation Form.";
                        return Json(new
                        {
                            result
                        });
                    }

                    if (sMode != "Y" && sMode != "N")
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Can't find Approve or Reject.";
                        return Json(new
                        {
                            result
                        });
                    }
                    var _GetStatusRevis = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Revised_Evaluate);
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                    var _getDataApprove = _PTR_Evaluation_ApproveService.Find(nId);
                    if (_getDataApprove != null)
                    {
                        var _getData = _getDataApprove.PTR_Evaluation;
                        if (_getData != null)
                        {
                            int nRating = SystemFunction.GetIntNullToZero(ItemData.approve_rating_id);
                            var _getEvaRating = _TM_Annual_RatingService.Find(nRating);

                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                if (_getDataApprove.Approve_status != "Y")
                                {
                                    _getData.update_date = dNow;
                                }

                                _getData.PTR_Evaluation_Year = _getDataYear;
                            }
                            //var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && w.active_status == "Y" && w.Approve_status != "Y").FirstOrDefault();
                            if (sMode == "Y")
                            {
                                if (_getEvaRating != null)
                                {

                                    if ((_getDataApprove.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES()) && _getDataApprove.active_status == "Y" /*&& _getDataApprove.Approve_status != "Y"*/)
                                    {
                                        if (/*_getDataApprove.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Ceo &&*/ _PTR_Evaluation_ApproveService.CanCompleted(_getData.Id, nId))
                                        {
                                            var _getStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Evaluation_Completed);
                                            _getData.TM_PTR_Eva_Status = _getStatus;
                                        }

                                        var sComplect = _PTR_EvaluationService.Complect(_getData);
                                        if (sComplect > 0 || _getDataApprove.Approve_status == "Y")
                                        {
                                            if (_getEvaRating != null)
                                            {
                                                if (_getDataApprove != null)
                                                {

                                                    List<PTR_Evaluation_Incidents_Score> lstAnsIncidents = new List<PTR_Evaluation_Incidents_Score>();

                                                    if (ItemData.lstIncidents != null)
                                                    {
                                                        lstAnsIncidents = (from lstA in ItemData.lstIncidents_score.Where(w => w.isCurrent == "Y" && w.nrate.HasValue && w.nrate != 0)
                                                                           from lstQ in _getEvaQuestion.TM_PTR_Eva_Incidents.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PartnerEvaluation.TM_PTR_Eva_Incidents())
                                                                           select new PTR_Evaluation_Incidents_Score
                                                                           {
                                                                               update_user = CGlobal.UserInfo.UserId,
                                                                               update_date = dNow,
                                                                               create_user = CGlobal.UserInfo.UserId,
                                                                               create_date = dNow,
                                                                               active_status = "Y",
                                                                               TM_PTR_Eva_Incidents = lstQ != null ? lstQ : null,
                                                                               PTR_Evaluation_Approve = _getDataApprove,
                                                                               TM_PTR_Eva_Incidents_Score_Id = lstA.nrate,

                                                                           }).ToList();
                                                    }



                                                    _getDataApprove.PTR_Evaluation = _getData;
                                                    if (_getDataApprove.Approve_status != "Y")
                                                    {
                                                        _getDataApprove.update_user = CGlobal.UserInfo.UserId;
                                                        _getDataApprove.update_date = dNow;
                                                        _getDataApprove.Approve_date = dNow;
                                                        _getDataApprove.Approve_status = "Y";
                                                    }

                                                    _getDataApprove.Annual_Rating = _getEvaRating;

                                                    _getDataApprove.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                                    _getDataApprove.responses = HCMFunc.DataEncryptPES((ItemData.approve_remark + "").Trim());
                                                    sComplect = _PTR_Evaluation_ApproveService.Update(_getDataApprove);
                                                    if (sComplect > 0 || _getDataApprove.Approve_status == "Y")
                                                    {
                                                        if (lstAnsIncidents.Any())
                                                        {
                                                            _PTR_Evaluation_Incidents_ScoreService.UpdateAnswer(lstAnsIncidents, _getDataApprove.Id, CGlobal.UserInfo.UserId, dNow);
                                                        }


                                                        result.Status = SystemFunction.process_Success;
                                                        result.Msg = "Approval Completed";
                                                        if (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Evaluation_Completed)
                                                        {
                                                            string sError = "";
                                                            string mail_to_log = "";
                                                            var Mail1 = _MailContentService.Find(12);
                                                            if (_getData != null)
                                                            {
                                                                //complete
                                                                //var bSuss = SendPTRSuccessfully(_getData, Mail1, ref sError, ref mail_to_log);
                                                            }
                                                        }
                                                        else
                                                        {
                                                            string sError = "";
                                                            string mail_to_log = "";
                                                            var Mail1 = _MailContentService.Find(47);
                                                            var GetMail = _PTR_Evaluation_ApproveService.GetEvaForMail(_getData.Id).FirstOrDefault();
                                                            if (GetMail != null)
                                                            {
                                                                //approve
                                                                var bSuss = SendPTRSubmit(GetMail, "", Mail1, ref sError, ref mail_to_log);
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        result.Status = SystemFunction.process_Failed;
                                                        result.Msg = "Error, please try again.";
                                                    }
                                                }
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
                                        result.Msg = "Error, Can't find Approve or Reject.";

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
                                var _getApprove = _PTR_Evaluation_ApproveService.GetApproveByEva(_getData.Id);
                                if (_getApprove != null && _getApprove.Any())
                                {
                                    var _getWaittingApprove = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Evaluation_Approval);
                                    _getData.TM_PTR_Eva_Status = _getWaittingApprove;// _GetStatusRevis;
                                    if (_getApprove.OrderByDescending(o => o.TM_PTR_Eva_ApproveStep.seq).FirstOrDefault().TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self)
                                    {
                                        _getWaittingApprove = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Revised_Evaluate);
                                        _getData.TM_PTR_Eva_Status = _getWaittingApprove;
                                    }

                                   

                                    _getData.comments = ItemData.revise_remark;
                                    var sComplect = _PTR_EvaluationService.Complect(_getData);
                                    if (sComplect > 0)
                                    {
                                        var item = _getApprove.OrderByDescending(o => o.TM_PTR_Eva_ApproveStep.seq).FirstOrDefault();
                                        //foreach (var item in _getApprove.Where(w => w.TM_PTR_Eva_ApproveStep.Id != (int)StepApproveEvaluate.Self).ToList())
                                        //foreach (var item in _getApprove.ToList())
                                        //{
                                        item.PTR_Evaluation = _getData;
                                        item.update_user = CGlobal.UserInfo.UserId;
                                        item.update_date = dNow;
                                        item.Annual_Rating = null;
                                        item.Approve_status = "";
                                        item.Approve_date = null;
                                        item.Approve_user = "";
                                        item.responses = "";
                                        sComplect = _PTR_Evaluation_ApproveService.Update(item);
                                        if (sComplect > 0)
                                        {
                                            string sError = "";
                                            string mail_to_log = "";
                                            var Mail1 = _MailContentService.Find(8);
                                            var GetMail = _PTR_EvaluationService.Find(_getData.Id);
                                            if (GetMail != null)
                                            {
                                                //revise
                                                var bSuss = SendPTRReviseNewVersion(item, ItemData.revise_remark, Mail1, ref sError, ref mail_to_log);
                                            }
                                            result.Status = SystemFunction.process_Success;
                                            result.Msg = "Sending Completed";

                                            //}

                                        }
                                        result.Status = SystemFunction.process_Success;
                                        result.Msg = "Sending Completed";
                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, please try again.";
                                    }
                                }
                                else
                                {
                                    _getApprove = _PTR_Evaluation_ApproveService.GetApproveByEvaReturn(_getData.Id);
                                    var _getWaittingApprove = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Revised_Evaluate);
                                    _getData.TM_PTR_Eva_Status = _getWaittingApprove;// _GetStatusRevis;

                                    _getData.comments = ItemData.revise_remark;
                                    var sComplect = _PTR_EvaluationService.Complect(_getData);
                                    if (sComplect > 0)
                                    {
                                        //var item = _getApprove.OrderByDescending(o => o.TM_PTR_Eva_ApproveStep.seq).FirstOrDefault();
                                        foreach (var item in _getApprove.Where(w => w.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Self).ToList())
                                        //foreach (var item in _getApprove.ToList())
                                        {
                                            item.PTR_Evaluation = _getData;
                                            item.update_user = CGlobal.UserInfo.UserId;
                                            item.update_date = dNow;
                                            item.Annual_Rating = null;
                                            item.Approve_status = "";
                                            item.Approve_date = null;
                                            item.Approve_user = "";
                                            item.responses = "";
                                            sComplect = _PTR_Evaluation_ApproveService.Update(item);
                                            if (sComplect > 0)
                                            {
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find(8);
                                                var GetMail = _PTR_EvaluationService.Find(_getData.Id);
                                                if (GetMail != null)
                                                {
                                                    //revise
                                                    var bSuss = SendPTRReviseNewVersion(item, ItemData.revise_remark, Mail1, ref sError, ref mail_to_log);
                                                }
                                                result.Status = SystemFunction.process_Success;
                                                result.Msg = "Sending Completed";

                                            }

                                        }
                                        result.Status = SystemFunction.process_Success;
                                        result.Msg = "Sending Completed";
                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, please try again.";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Evaluation not found.";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult ReviseToCandidate(vPTREvaluation_obj_save ItemData)
        {
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    if ((ItemData.revise_remark + "").Trim() == "")
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Please provide the remark why you are revise this Evaluation Form.";
                        return Json(new
                        {
                            result
                        });
                    }


                    var _GetStatusRevis = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Revised_Evaluate);
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                    var _getDataApprove = _PTR_Evaluation_ApproveService.Find(nId);
                    if (_getDataApprove != null)
                    {
                        var _getData = _getDataApprove.PTR_Evaluation;
                        if (_getData != null)
                        {
                            int nRating = SystemFunction.GetIntNullToZero(ItemData.approve_rating_id);
                            var _getEvaRating = _TM_Annual_RatingService.Find(nRating);

                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.PTR_Evaluation_Year = _getDataYear;
                                _getData.TM_PTR_Eva_Status = _GetStatusRevis;
                                _getData.comments = ItemData.revise_remark;
                            }

                            var sComplect = _PTR_EvaluationService.Complect(_getData);
                            if (sComplect > 0)
                            {
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPTR.Eva_Revise);
                                var GetMail = _PTR_EvaluationService.Find(_getData.Id);

                                result.Status = SystemFunction.process_Success;
                                result.Msg = "Sending Completed";

                                var _getApprove = _PTR_Evaluation_ApproveService.GetApproveByEvaReturn(_getData.Id);
                                if (_getApprove != null && _getApprove.Any())
                                {
                                    foreach (var item in _getApprove)
                                    {


                                        item.PTR_Evaluation = _getData;
                                        item.update_user = CGlobal.UserInfo.UserId;
                                        item.update_date = dNow;
                                        item.Annual_Rating = null;
                                        item.Approve_status = "";
                                        item.Approve_date = null;
                                        item.Approve_user = "";
                                        //item.responses = "";
                                        sComplect = _PTR_Evaluation_ApproveService.Update(item);
                                        if (sComplect > 0)
                                        {
                                            if (GetMail != null)
                                            {
                                                var bSuss = SendPTRReviseNewVersion(GetMail, item, Mail1, ref sError, ref mail_to_log);
                                            }
                                        }



                                    }
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
                            result.Msg = "Error, Evaluation Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Evaluation not found.";
                }
            }
            return Json(new
            {
                result
            });
        }
        #endregion
        #region Ajax Function Plan
        [HttpPost]
        public ActionResult LoadPTRPlanApproveList(CSearchPTREvaluation SearchItem)
        {
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            List<vPTREvaluation_obj> lstData_resutl = new List<vPTREvaluation_obj>();
            int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
            var lstData = _PTR_Evaluation_ApproveService.GetPlanForApprove(CGlobal.UserInfo.EmployeeNo, nYear, CGlobal.UserIsAdminPES());
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
                string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.PTR_Evaluation.user_no).ToArray();
                string[] aApproveUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                string[] aUpdateUser = lstData.Select(s => s.PTR_Evaluation.update_user).ToArray();

                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                lstData_resutl = (from lstAD in lstData
                                  from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.PTR_Evaluation.user_no).DefaultIfEmpty(new AllInfo_WS())
                                  from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == lstAD.PTR_Evaluation.update_user).DefaultIfEmpty(new AllInfo_WS())
                                  select new vPTREvaluation_obj
                                  {
                                      srank = lstEmpReq.RankCode,
                                      sgroup = lstEmpReq.UnitGroupName,
                                      name_en = lstAD.PTR_Evaluation.user_no + " : " + lstEmpReq.EmpFullName,
                                      fy_year = lstAD.PTR_Evaluation.PTR_Evaluation_Year.evaluation_year.HasValue ? lstAD.PTR_Evaluation.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                      active_status = lstAD.PTR_Evaluation.TM_PTR_Eva_Status != null ? lstAD.PTR_Evaluation.TM_PTR_Eva_Status.status_name_en : "", //lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.PTR_Evaluation.update_date.HasValue ? lstAD.PTR_Evaluation.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      approval = AEmp.EmpFullName,
                                      name = lstEmpReq.EmpFullName,
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult ApprovePersonalPlan(vPTREvaluation_obj_save ItemData, string sMode)
        {
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt) && !string.IsNullOrEmpty(sMode))
                {
                    if (sMode == "N" && (ItemData.revise_remark + "").Trim() == "")
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Please provide the remark why you are revise this Evaluation Form.";
                        return Json(new
                        {
                            result
                        });
                    }

                    if (sMode != "Y" && sMode != "N")
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Can't find Approve or Reject.";
                        return Json(new
                        {
                            result
                        });
                    }
                    var _GetStatusRevis = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Revised_Plan);
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                    var _getDataApprove = _PTR_Evaluation_ApproveService.Find(nId);
                    if (_getDataApprove != null)
                    {
                        var _getData = _getDataApprove.PTR_Evaluation;
                        if (_getData != null)
                        {
                            int nRating = SystemFunction.GetIntNullToZero(ItemData.approve_rating_id);
                            //var _getEvaRating = _TM_Annual_RatingService.Find(nRating);

                            var _getDataYear = _PTR_Evaluation_YearService.Find(_getData.PTR_Evaluation_Year.Id);
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getDataYear != null)
                            {
                                _getData.update_user = CGlobal.UserInfo.UserId;
                                _getData.update_date = dNow;
                                _getData.PTR_Evaluation_Year = _getDataYear;
                            }
                            //var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && w.active_status == "Y" && w.Approve_status != "Y").FirstOrDefault();
                            if (sMode == "Y")
                            {
                                if ((_getDataApprove.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES()) && _getDataApprove.active_status == "Y" && _getDataApprove.Approve_status != "Y")
                                {
                                    if (_getDataApprove.TM_PTR_Eva_ApproveStep.Id == (int)StepApprovePlan.Ceo)
                                    {
                                        var _getStatus = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Planning_Completed);
                                        _getData.TM_PTR_Eva_Status = _getStatus;
                                    }

                                    var sComplect = _PTR_EvaluationService.Complect(_getData);
                                    if (sComplect > 0)
                                    {

                                        if (_getDataApprove != null)
                                        {
                                            _getDataApprove.PTR_Evaluation = _getData;
                                            _getDataApprove.update_user = CGlobal.UserInfo.UserId;
                                            _getDataApprove.update_date = dNow;
                                            // _getDataApprove.Annual_Rating = _getEvaRating;
                                            _getDataApprove.Approve_status = "Y";
                                            _getDataApprove.Approve_date = dNow;
                                            _getDataApprove.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                            _getDataApprove.responses = HCMFunc.DataEncryptPES((ItemData.approve_remark + "").Trim());
                                            sComplect = _PTR_Evaluation_ApproveService.Update(_getDataApprove);
                                            if (sComplect > 0)
                                            {
                                                result.Status = SystemFunction.process_Success;
                                                result.Msg = "Approval Completed";
                                                if (_getData.TM_PTR_Eva_Status.Id == (int)Eva_Status.Planning_Completed)
                                                {
                                                    string sError = "";
                                                    string mail_to_log = "";
                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPTR.Plan_Suscess);
                                                    if (_getData != null)
                                                    {
                                                        //var bSuss = SendPTRSuccessfully(_getData, Mail1, ref sError, ref mail_to_log);
                                                    }
                                                }
                                                else
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
                                        result.Msg = "Error, please try again.";
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Can't find Approve or Reject.";

                                }

                            }
                            else
                            {
                                var _getApprove = _PTR_Evaluation_ApproveService.GetApproveByPlan(_getData.Id);
                                if (_getApprove != null && _getApprove.Any())
                                {
                                    _getData.TM_PTR_Eva_Status = _GetStatusRevis;
                                    _getData.comments = ItemData.revise_remark;
                                    var sComplect = _PTR_EvaluationService.Complect(_getData);
                                    if (sComplect > 0)
                                    {
                                        foreach (var item in _getApprove)
                                        {
                                            item.PTR_Evaluation = _getData;
                                            item.update_user = CGlobal.UserInfo.UserId;
                                            item.update_date = dNow;
                                            item.Annual_Rating = null;
                                            item.Approve_status = "";
                                            item.Approve_date = null;
                                            item.Approve_user = "";
                                            item.responses = "";
                                            sComplect = _PTR_Evaluation_ApproveService.Update(item);
                                            if (sComplect > 0)
                                            {


                                            }
                                        }
                                        string sError = "";
                                        string mail_to_log = "";
                                        var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPTR.Plan_Revise);
                                        var GetMail = _PTR_EvaluationService.Find(_getData.Id);
                                        if (GetMail != null)
                                        {
                                            var bSuss = SendPTRRevise(GetMail, Mail1, ref sError, ref mail_to_log);
                                        }
                                        result.Status = SystemFunction.process_Success;
                                        result.Msg = "Sending Completed";
                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, please try again.";
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Not Found.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Evaluation not found.";
                }
            }
            return Json(new
            {
                result
            });
        }
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
        private void AddMailLog(PTR_Evaluation_Approve PTR_Evaluation_Approve, Models.Common.MailContent MailContent, string descriptions, string mail_to_log, bool suss)
        {
            try
            {
                DateTime dNow = DateTime.Now;
                TM_PTR_E_Mail_History objSave = new TM_PTR_E_Mail_History();
                objSave.PTR_Evaluation_Approve = PTR_Evaluation_Approve;
                objSave.MailContent = MailContent;
                objSave.create_user = CGlobal.UserInfo.UserId;
                objSave.update_user = CGlobal.UserInfo.UserId;
                objSave.create_date = dNow;
                objSave.update_date = dNow;
                objSave.sent_status = suss ? "Y" : "N";
                objSave.descriptions = descriptions;
                objSave.mail_to = mail_to_log;
                _TM_PTR_E_Mail_HistoryService.CreateNew(objSave);
            }
            catch (Exception e)
            {


            }

        }
    }
}