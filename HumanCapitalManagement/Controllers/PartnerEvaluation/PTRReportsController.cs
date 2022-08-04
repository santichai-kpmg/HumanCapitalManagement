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
    public class PTRReportsController : BaseController
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
        public PTRReportsController(PTR_Evaluation_YearService PTR_Evaluation_YearService
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
        // GET: PTRReports
        public ActionResult PTRReportsList(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPTREvaluation result = new vPTREvaluation();
            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "rpEva" + unixTimestamps;
            result.session = sSession;
            rpvPTREvaluation_Session objSession = new rpvPTREvaluation_Session();
            Session[sSession] = new rpvPTREvaluation_Session();//new List<ListAdvanceTransaction>();
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPTREvaluation SearchItem = (CSearchPTREvaluation)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPTREvaluation)));
                int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
                int nStatus = SystemFunction.GetIntNullToZero(SearchItem.status_id);
                var lstData = _PTR_EvaluationService.GetPTRApproveReport(nYear, nStatus, CGlobal.UserInfo.EmployeeNo, CGlobal.UserIsAdminPES());

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo) && w.Status == 3 || w.Status == 1).ToList();
                    if (_getPartnerUser.Any())
                    {

                        //List<int> nApID = new List<int>();
                        //foreach (var nID in Enum.GetValues(typeof(PESClass.StepApproveEvaluate)))
                        //{
                        //    nApID.Add((int)nID);
                        //}
                        // List<string> ApproveNO = new List<string>();
                        //var _getApprove = lstData.Where(a => a.active_status == "Y").Select(s => s.PTR_Evaluation_Approve.Where(ws=> ws.active_status== "Y" && nApID.Contains(ws.TM_PTR_Eva_ApproveStep.Id)).Select(s2=> s2).ToList()).ToList();
                        //if(_getApprove.Any())
                        //{
                        //    ApproveNO = _getApprove.Where(w=> w.)
                        //}

                        //foreach (var iDA in lstData.Where(a => a.active_status == "Y"))
                        //{
                        //    ApproveNO.AddRange(iDA.PTR_Evaluation_Approve.Where(ws => ws.active_status == "Y" && nApID.Contains(ws.TM_PTR_Eva_ApproveStep.Id)).Select(s => s.Req_Approve_user).ToList());
                        //}
                        //set Approval Data
                        #region 
                        List<vPTRApproval_Sumary> lstBUHead = new List<vPTRApproval_Sumary>();
                        List<vPTRApproval_Sumary> lstPracticeHead = new List<vPTRApproval_Sumary>();
                        List<vPTRApproval_Sumary> lstCeoHead = new List<vPTRApproval_Sumary>();
                        List<vPTRApproval_Sumary> lstCeoElect = new List<vPTRApproval_Sumary>();

                        var lstBU = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead)).ToList();
                        if (lstBU.Any())
                        {
                            string[] _aApproveNO = lstBU.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                            var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                            lstBUHead = (from item in lstBU.Where(w => w.active_status == "Y")
                                         from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                         select new vPTRApproval_Sumary
                                         {
                                             eva_id = item.PTR_Evaluation.Id,
                                             approva_date = item.Approve_date,
                                             emp_name = lstEmpReq.EmpFullName,
                                             emp_no = item.Req_Approve_user,
                                             // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                             rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                             rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                         }).ToList();
                        }
                        var lstPH = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead)).ToList();
                        if (lstPH.Any())
                        {
                            string[] _aApproveNO = lstPH.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                            var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                            lstPracticeHead = (from item in lstPH.Where(w => w.active_status == "Y")
                                               from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                               select new vPTRApproval_Sumary
                                               {
                                                   eva_id = item.PTR_Evaluation.Id,
                                                   approva_date = item.Approve_date,
                                                   emp_name = lstEmpReq.EmpFullName,
                                                   emp_no = item.Req_Approve_user,
                                                   // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                                   rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                                   rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                               }).ToList();
                        }
                        var lstCeo = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo)).ToList();
                        if (lstCeo.Any())
                        {
                            string[] _aApproveNO = lstCeo.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                            var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                            lstCeoHead = (from item in lstCeo.Where(w => w.active_status == "Y")
                                          from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                          select new vPTRApproval_Sumary
                                          {
                                              eva_id = item.PTR_Evaluation.Id,
                                              approva_date = item.Approve_date,
                                              emp_name = lstEmpReq.EmpFullName,
                                              emp_no = item.Req_Approve_user,
                                              // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                              rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                              rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                          }).ToList();
                        }

                        var lstCeoE = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO)).ToList();
                        if (lstCeoE.Any())
                        {
                            string[] _aApproveNO = lstCeoE.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                            var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                            lstCeoElect = (from item in lstCeoE.Where(w => w.active_status == "Y")
                                           from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                           select new vPTRApproval_Sumary
                                           {
                                               eva_id = item.PTR_Evaluation.Id,
                                               approva_date = item.Approve_date,
                                               emp_name = lstEmpReq.EmpFullName,
                                               emp_no = item.Req_Approve_user,
                                               // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                               rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                               rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                           }).ToList();
                        }

                        #endregion


                        if (!string.IsNullOrEmpty(SearchItem.group_id))
                        {
                            _getPartnerUser = _getPartnerUser.Where(w => w.UnitGroupID + "" == SearchItem.group_id).ToList();
                        }
                        var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                        result.lstData = (from lstAD in lstData
                                              // from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                          join lstEmpReq in _getPartnerUser on lstAD.user_no equals lstEmpReq.EmpNo
                                          from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                          from glst in lstBUHead.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead) != null
                                          && w.eva_id == lstAD.Id
                                          && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                          from plst in lstPracticeHead.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead) != null
                                          && w.eva_id == lstAD.Id
                                          && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                          from clst in lstCeoHead.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo) != null
                                          && w.eva_id == lstAD.Id
                                          && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())

                                          from ceoelst in lstCeoElect.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO) != null
                                          && w.eva_id == lstAD.Id
                                          && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())


                                          select new vPTREvaluation_obj
                                          {
                                              status_id = lstAD.TM_PTR_Eva_Status.Id + "",
                                              name_en = lstAD.user_no + " : " + lstEmpReq.EmpFullName,
                                              name = lstEmpReq.EmpFullName,
                                              emp_no = lstAD.user_no,
                                              fy_year = lstAD.PTR_Evaluation_Year.evaluation_year.HasValue ? lstAD.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                              eva_status = lstAD.TM_PTR_Eva_Status != null ? lstAD.TM_PTR_Eva_Status.status_name_en : "",
                                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                              sgroup = lstEmpReq.UnitGroupName + "",
                                              srank = lstEmpReq.Rank + "",
                                              sceo = (clst.emp_no + "" != "") ? (clst.emp_name + "<br/>" + "(" + (clst.approva_date.HasValue ? clst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                              spractice = (plst.emp_no + "" != "") ? (plst.emp_name + "<br/>" + "(" + (plst.approva_date.HasValue ? plst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                              sbu = (glst.emp_no + "" != "") ? (glst.emp_name + "<br/>" + "(" + (glst.approva_date.HasValue ? glst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                              self_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Self) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Self).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Self).Annual_Rating.rating_name_en : "") : "",
                                              final_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo).Annual_Rating.rating_name_en : "") : "",
                                              bu_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead).Annual_Rating.rating_name_en : "") : "",
                                              practice_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead).Annual_Rating.rating_name_en : "") : "",
                                              ceoe_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO).Annual_Rating.rating_name_en : "") : "",
                                              Id = lstAD.Id,
                                              ceoe = (ceoelst.emp_no + "" != "") ? (ceoelst.emp_name + "<br/>" + "(" + (ceoelst.approva_date.HasValue ? ceoelst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",

                                          }).ToList();
                        if (result.lstData.Any())
                        {
                            result.lstData.ForEach(ed =>
                            {
                                if (ed.status_id != "8")
                                {
                                    ed.bu_eva = "-";
                                    ed.practice_eva = "-";
                                    ed.ceoe_eva = "-";
                                    ed.ceoe_eva = "-";
                                }
                            });
                            objSession.lstData = (from item in result.lstData
                                                  from lstEva in lstData.AsEnumerable().Where(w => w.Id == item.Id).DefaultIfEmpty(new PTR_Evaluation())
                                                  select new vPTREvaluation_report
                                                  {
                                                      name = item.name,
                                                      sbu = item.sbu.Replace("<br/>", " "),
                                                      sceo = item.sceo.Replace("<br/>", " "),
                                                      self_eva = item.self_eva,
                                                      sgroup = item.sgroup,
                                                      spractice = item.spractice.Replace("<br/>", " "),
                                                      emp_no = item.emp_no,
                                                      eva_status = item.eva_status,
                                                      bu_eva = item.bu_eva,
                                                      final_eva = item.final_eva,
                                                      fy_year = item.fy_year,
                                                      practice_eva = item.practice_eva,
                                                      srank = item.srank,
                                                      bu_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead).responses + "") : "",
                                                      practice_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead).responses + "") : "",
                                                      final_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo).responses + "") : "",

                                                      ceoe = item.ceoe.Replace("<br/>", " "),
                                                      ceoe_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO).responses + "") : "",
                                                      ceoe_eva = item.ceoe_eva,

                                                  }).ToList();

                            objSession.lstData.ForEach(ed =>
                            {
                                ed.bu_comment = HCMFunc.DataDecryptPES(ed.bu_comment + "");
                                ed.practice_comment = HCMFunc.DataDecryptPES(ed.practice_comment + "");
                                ed.final_comment = HCMFunc.DataDecryptPES(ed.final_comment + "");
                                ed.ceoe_comment = HCMFunc.DataDecryptPES(ed.ceoe_comment + "");
                            });

                            Session[sSession] = objSession.lstData;
                        }
                        else
                        {

                        }
                    }



                }
            }
            else
            {
                var _getYear = _PTR_Evaluation_YearService.FindNowYear();
                if (_getYear != null)
                {
                    result.fy_year = _getYear.Id + "";
                }
            }
            #endregion
            return View(result);
        }
        public ActionResult PTRReportsEdit(string qryStr)
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
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getData.PTR_Evaluation_Approve.Any(a => a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && a.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Ceo && a.active_status == "Y")
                                || CGlobal.UserIsAdminPES())
                            {
                                result.is_ceo = "Y";
                            }

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
                                                          target_data = lst.TM_KPIs_Base.type_of_kpi == "P" ? SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target)).NodecimalFormatHasvalue() + "-" + SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)).NodecimalFormatHasvalue() + "%" : (lst.TM_KPIs_Base.type_of_kpi == "N" ? SystemFunction.GetNumberNullToZero(FormatNumber((double)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)))).NodecimalFormatHasvalue() : SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.target_max)).NodecimalFormatHasvalue()),
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
                                    if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019 || _getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2020)
                                    {
                                        result.lstAnswer = (from lstQ in _getEvaQuestion.TM_PTR_Eva_Questions
                                                            select new vPTREvaluation_Answer
                                                            {
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
                                    if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019 || _getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2020)
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
                                    if (_getData.user_no == CGlobal.UserInfo.EmployeeNo && _getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019 || _getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2020)
                                    {
                                        if (_getData.TM_PTR_Eva_Status.Id == (int)PESClass.Eva_Status.Evaluation_Completed)
                                        {
                                            result.lstFinal = (from lst in _getData.PTR_Evaluation_Approve.Where(a => a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "G")

                                                               select new vPTREvaluation_Final
                                                               {
                                                                   position = lst.TM_PTR_Eva_ApproveStep.Step_name_en,
                                                                   // status = lst.Annual_Rating != null ? ((lst.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo || lst.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Self) ? lst.Annual_Rating.rating_name_en : "") : "",
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
                                                     isCurrent = "N",

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
                                if (_getData.PTR_Evaluation_Approve.Any(a => a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && a.TM_PTR_Eva_ApproveStep.Id == (int)StepApproveEvaluate.Ceo && a.active_status == "Y")
                              || CGlobal.UserIsAdminPES())
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
                            else
                            {
                                return RedirectToAction("PTRReportsList", "PTRReports");
                            }
                            #endregion

                            if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2018)
                            {
                                return View(result);
                            }
                            else if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2019)
                            {
                                return View("PTRReportsEdit19", result);
                            }
                            else if (_getData.PTR_Evaluation_Year.evaluation_year.Value.Year == 2020)
                            {
                                return View("PTRReportsEdit20", result);
                            }
                            else
                            {
                                return View(result);
                            }
                        }
                        else
                        {
                            return RedirectToAction("PTRReportsList", "PTRReports");
                        }

                    }
                    else
                    {
                        return RedirectToAction("PTRReportsList", "PTRReports");
                    }

                }
                else
                {
                    return RedirectToAction("PTRReportsList", "PTRReports");
                }
            }
            else
            {
                return RedirectToAction("PTRReportsList", "PTRReports");
            }
            //       return View(result);

            #endregion
        }

        #region Plan 
        public ActionResult PTRPlanReportsList(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPTREvaluation result = new vPTREvaluation();
            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "rpPlan" + unixTimestamps;
            result.session = sSession;
            rpvPTREvaluation_Session objSession = new rpvPTREvaluation_Session();
            Session[sSession] = new rpvPTREvaluation_Session();//new List<ListAdvanceTransaction>();
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPTREvaluation SearchItem = (CSearchPTREvaluation)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPTREvaluation)));
                int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
                int nStatus = SystemFunction.GetIntNullToZero(SearchItem.status_id);
                var lstData = _PTR_EvaluationService.GetPTRPlanApproveReport(nYear, nStatus, CGlobal.UserInfo.EmployeeNo, CGlobal.UserIsAdminPES());
                result.fy_year = SearchItem.fy_year;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo) && w.Status == 3 || w.Status == 1).ToList();
                    if (_getPartnerUser.Any())
                    {
                        //List<int> nApID = new List<int>();
                        //foreach (var nID in Enum.GetValues(typeof(PESClass.StepApprovePlan)))
                        //{
                        //    nApID.Add((int)nID);
                        //}
                        // List<string> ApproveNO = new List<string>();
                        //var _getApprove = lstData.Where(a => a.active_status == "Y").Select(s => s.PTR_Evaluation_Approve.Where(ws=> ws.active_status== "Y" && nApID.Contains(ws.TM_PTR_Eva_ApproveStep.Id)).Select(s2=> s2).ToList()).ToList();
                        //if(_getApprove.Any())
                        //{
                        //    ApproveNO = _getApprove.Where(w=> w.)
                        //}

                        //foreach (var iDA in lstData.Where(a => a.active_status == "Y"))
                        //{
                        //    ApproveNO.AddRange(iDA.PTR_Evaluation_Approve.Where(ws => ws.active_status == "Y" && nApID.Contains(ws.TM_PTR_Eva_ApproveStep.Id)).Select(s => s.Req_Approve_user).ToList());
                        //}
                        //set Approval Data
                        #region 
                        List<vPTRApproval_Sumary> lstSelf = new List<vPTRApproval_Sumary>();
                        List<vPTRApproval_Sumary> lstBUHead = new List<vPTRApproval_Sumary>();
                        List<vPTRApproval_Sumary> lstPracticeHead = new List<vPTRApproval_Sumary>();
                        List<vPTRApproval_Sumary> lstCeoHead = new List<vPTRApproval_Sumary>();
                        var lstSelfs = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self)).ToList();
                        if (lstSelfs.Any())
                        {
                            string[] _aApproveNO = lstSelfs.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                            var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                            lstSelf = (from item in lstSelfs.Where(w => w.active_status == "Y")
                                       from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                       select new vPTRApproval_Sumary
                                       {
                                           eva_id = item.PTR_Evaluation.Id,
                                           approva_date = item.Approve_date,
                                           emp_name = lstEmpReq.EmpFullName,
                                           emp_no = item.Req_Approve_user,
                                           // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                           rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                           rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                       }).ToList();
                        }
                        var lstBU = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead)).ToList();
                        if (lstBU.Any())
                        {
                            string[] _aApproveNO = lstBU.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                            var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                            lstBUHead = (from item in lstBU.Where(w => w.active_status == "Y")
                                         from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                         select new vPTRApproval_Sumary
                                         {
                                             eva_id = item.PTR_Evaluation.Id,
                                             approva_date = item.Approve_date,
                                             emp_name = lstEmpReq.EmpFullName,
                                             emp_no = item.Req_Approve_user,
                                             // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                             rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                             rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                         }).ToList();
                        }
                        var lstPH = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead)).ToList();
                        if (lstPH.Any())
                        {
                            string[] _aApproveNO = lstPH.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                            var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                            lstPracticeHead = (from item in lstPH.Where(w => w.active_status == "Y")
                                               from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                               select new vPTRApproval_Sumary
                                               {
                                                   eva_id = item.PTR_Evaluation.Id,
                                                   approva_date = item.Approve_date,
                                                   emp_name = lstEmpReq.EmpFullName,
                                                   emp_no = item.Req_Approve_user,
                                                   // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                                   rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                                   rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                               }).ToList();
                        }
                        var lstCeo = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo)).ToList();
                        if (lstCeo.Any())
                        {
                            string[] _aApproveNO = lstCeo.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                            var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                            lstCeoHead = (from item in lstCeo.Where(w => w.active_status == "Y")
                                          from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                          select new vPTRApproval_Sumary
                                          {
                                              eva_id = item.PTR_Evaluation.Id,
                                              approva_date = item.Approve_date,
                                              emp_name = lstEmpReq.EmpFullName,
                                              emp_no = item.Req_Approve_user,
                                              // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                              rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                              rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                          }).ToList();
                        }
                        #endregion





                        if (!string.IsNullOrEmpty(SearchItem.group_id))
                        {
                            _getPartnerUser = _getPartnerUser.Where(w => w.UnitGroupID + "" == SearchItem.group_id).ToList();
                        }
                        var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                        result.lstData = (from lstAD in lstData
                                              // from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                          join lstEmpReq in _getPartnerUser on lstAD.user_no equals lstEmpReq.EmpNo
                                          from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                          from slst in lstSelf.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self) != null
                                          && w.eva_id == lstAD.Id
                                          && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                          from glst in lstBUHead.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead) != null
                                          && w.eva_id == lstAD.Id
                                          && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                          from plst in lstPracticeHead.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead) != null
                                          && w.eva_id == lstAD.Id
                                          && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                          from clst in lstCeoHead.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo) != null
                                          && w.eva_id == lstAD.Id
                                          && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                          select new vPTREvaluation_obj
                                          {
                                              name_en = lstAD.user_no + " : " + lstEmpReq.EmpFullName,
                                              name = lstEmpReq.EmpFullName,
                                              emp_no = lstAD.user_no,
                                              fy_year = lstAD.PTR_Evaluation_Year.evaluation_year.HasValue ? lstAD.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                              eva_status = lstAD.TM_PTR_Eva_Status != null ? lstAD.TM_PTR_Eva_Status.status_name_en : "",
                                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                              sgroup = lstEmpReq.UnitGroupName + "",
                                              srank = lstEmpReq.Rank + "",
                                              sceo = (clst.emp_no + "" != "") ? (clst.emp_name + "<br/>" + "(" + (clst.approva_date.HasValue ? clst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                              spractice = (plst.emp_no + "" != "") ? (plst.emp_name + "<br/>" + "(" + (plst.approva_date.HasValue ? plst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                              sbu = (glst.emp_no + "" != "") ? (glst.emp_name + "<br/>" + "(" + (glst.approva_date.HasValue ? glst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                              self_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self).Annual_Rating.rating_name_en : "") : "",
                                              final_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo).Annual_Rating.rating_name_en : "") : "",
                                              bu_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead).Annual_Rating.rating_name_en : "") : "",
                                              practice_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead).Annual_Rating.rating_name_en : "") : "",
                                              Id = lstAD.Id,
                                              update_date = lstEmpUp.EmpFullName + "(" + (lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "") + ")",
                                              p_d = (slst.approva_date.HasValue ? slst.approva_date.Value.DateTimebyCulture() : "Waiting"),
                                          }).ToList();
                        if (result.lstData.Any())
                        {
                            objSession.lstData = (from item in result.lstData
                                                  from lstEva in lstData.AsEnumerable().Where(w => w.Id == item.Id).DefaultIfEmpty(new PTR_Evaluation())
                                                  select new vPTREvaluation_report
                                                  {
                                                      name = item.name,
                                                      sbu = item.sbu.Replace("<br/>", Environment.NewLine + ""),
                                                      sceo = item.sceo.Replace("<br/>", Environment.NewLine + ""),
                                                      self_eva = item.self_eva,
                                                      sgroup = item.sgroup,
                                                      spractice = item.spractice.Replace("<br/>", Environment.NewLine + ""),
                                                      emp_no = item.emp_no,
                                                      eva_status = item.eva_status,
                                                      bu_eva = item.bu_eva,
                                                      final_eva = item.final_eva,
                                                      fy_year = item.fy_year,
                                                      practice_eva = item.practice_eva,
                                                      srank = item.srank,
                                                      bu_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead).responses + "") : "",
                                                      practice_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead).responses + "") : "",
                                                      final_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo).responses + "") : "",
                                                      last_update = item.update_date.Replace("(", Environment.NewLine + "("),
                                                      p_d = item.p_d,
                                                  }).ToList();

                            objSession.lstData.ForEach(ed =>
                            {
                                ed.bu_comment = HCMFunc.DataDecryptPES(ed.bu_comment + "");
                                ed.practice_comment = HCMFunc.DataDecryptPES(ed.practice_comment + "");
                                ed.final_comment = HCMFunc.DataDecryptPES(ed.final_comment + "");
                            });

                            Session[sSession] = objSession.lstData;
                        }
                        else
                        {

                        }
                    }


                }
            }
            else
            {
                var _getYear = _PTR_Evaluation_YearService.FindNowYear();
                if (_getYear != null)
                {
                    // result.fy_year = _getYear.Id + "";
                }
            }
            #endregion
            return View(result);
        }
        public ActionResult PTRPlanReportsEdit(string qryStr)
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

                    var _getData = _PTR_EvaluationService.FindReport(nId);
                    bool checkAuthor = false;
                    if (_getData != null)
                    {
                        if (_getData.PTR_Evaluation_Authorized != null)
                        {
                            checkAuthor = _getData.PTR_Evaluation_Authorized.Any(a => a.active_status == "Y" && a.authorized_user == CGlobal.UserInfo.EmployeeNo);
                        }
                    }

                    if (_getData != null
                        && (_getData.PTR_Evaluation_Approve != null && _getData.PTR_Evaluation_Approve.Any(a => a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && a.active_status == "Y" && a.TM_PTR_Eva_ApproveStep.type_of_step == "P")
                        || CGlobal.UserIsAdminPES())
                        || checkAuthor
                        )
                    {

                        var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                        if (_checkActiv != null)
                        {
                            var _getEvaQuestion = _TM_Partner_EvaluationService.GetActiveEvaForm(_getData.PTR_Evaluation_Year.evaluation_year.Value.Year);
                            if (_getData.PTR_Evaluation_Approve.Any(a => a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && a.TM_PTR_Eva_ApproveStep.Id == (int)StepApprovePlan.Ceo && a.active_status == "Y")
                                || CGlobal.UserIsAdminPES())
                            {
                                result.is_ceo = "Y";
                            }

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
                                return View("PTRPlanReportsEdit20", result);
                            }
                            else
                            {
                                return View(result);
                            }
                        }
                        else
                        {
                            return RedirectToAction("PTRPlanReportsList", "PTRReports");
                        }

                    }
                    else
                    {
                        return RedirectToAction("PTRPlanReportsList", "PTRReports");
                    }

                }
                else
                {
                    return RedirectToAction("PTRPlanReportsList", "PTRReports");
                }
            }
            else
            {
                return RedirectToAction("PTRPlanReportsList", "PTRReports");
            }
            //   return View(result);

            #endregion
        }
        #endregion
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadPTRReportsList(CSearchPTREvaluation SearchItem)
        {
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            List<vPTREvaluation_obj> lstData_resutl = new List<vPTREvaluation_obj>();
            int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
            int nStatus = SystemFunction.GetIntNullToZero(SearchItem.status_id);
            var lstData = _PTR_EvaluationService.GetPTRApproveReport(nYear, nStatus, CGlobal.UserInfo.EmployeeNo, CGlobal.UserIsAdminPES());
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);
            rpvPTREvaluation_Session objSession = new rpvPTREvaluation_Session();
            if (lstData.Any())
            {
                string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo) && w.Status == 3 || w.Status == 1).ToList();
                if (_getPartnerUser.Any())
                {
                    #region 
                    List<vPTRApproval_Sumary> lstBUHead = new List<vPTRApproval_Sumary>();
                    List<vPTRApproval_Sumary> lstPracticeHead = new List<vPTRApproval_Sumary>();
                    List<vPTRApproval_Sumary> lstCeoHead = new List<vPTRApproval_Sumary>();
                    List<vPTRApproval_Sumary> lstCeoElect = new List<vPTRApproval_Sumary>();
                    var lstBU = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead)).ToList();
                    if (lstBU.Any())
                    {
                        string[] _aApproveNO = lstBU.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                        lstBUHead = (from item in lstBU.Where(w => w.active_status == "Y")
                                     from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                     select new vPTRApproval_Sumary
                                     {
                                         eva_id = item.PTR_Evaluation.Id,
                                         approva_date = item.Approve_date,
                                         emp_name = lstEmpReq.EmpFullName,
                                         emp_no = item.Req_Approve_user,
                                         // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                         rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                         rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                     }).ToList();
                    }
                    var lstPH = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead)).ToList();
                    if (lstPH.Any())
                    {
                        string[] _aApproveNO = lstPH.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                        lstPracticeHead = (from item in lstPH.Where(w => w.active_status == "Y")
                                           from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                           select new vPTRApproval_Sumary
                                           {
                                               eva_id = item.PTR_Evaluation.Id,
                                               approva_date = item.Approve_date,
                                               emp_name = lstEmpReq.EmpFullName,
                                               emp_no = item.Req_Approve_user,
                                               // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                               rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                               rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                           }).ToList();
                    }
                    var lstCeo = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo)).ToList();
                    if (lstCeo.Any())
                    {
                        string[] _aApproveNO = lstCeo.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                        lstCeoHead = (from item in lstCeo.Where(w => w.active_status == "Y")
                                      from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      select new vPTRApproval_Sumary
                                      {
                                          eva_id = item.PTR_Evaluation.Id,
                                          approva_date = item.Approve_date,
                                          emp_name = lstEmpReq.EmpFullName,
                                          emp_no = item.Req_Approve_user,
                                          // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                          rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                          rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                      }).ToList();
                    }
                    var lstCeoE = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO)).ToList();
                    if (lstCeoE.Any())
                    {
                        string[] _aApproveNO = lstCeoE.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                        lstCeoElect = (from item in lstCeoE.Where(w => w.active_status == "Y")
                                       from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                       select new vPTRApproval_Sumary
                                       {
                                           eva_id = item.PTR_Evaluation.Id,
                                           approva_date = item.Approve_date,
                                           emp_name = lstEmpReq.EmpFullName,
                                           emp_no = item.Req_Approve_user,
                                           // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                           rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                           rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                       }).ToList();
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(SearchItem.group_id))
                    {
                        _getPartnerUser = _getPartnerUser.Where(w => w.UnitGroupID + "" == SearchItem.group_id).ToList();
                    }
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser);

                    lstData_resutl = (from lstAD in lstData
                                          // from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      join lstEmpReq in _getPartnerUser on lstAD.user_no equals lstEmpReq.EmpNo
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      from glst in lstBUHead.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead) != null
                                      && w.eva_id == lstAD.Id
                                      && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                      from plst in lstPracticeHead.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead) != null
                                      && w.eva_id == lstAD.Id
                                      && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                      from clst in lstCeoHead.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo) != null
                                      && w.eva_id == lstAD.Id
                                      && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())

                                      from ceoelst in lstCeoElect.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO) != null
                                         && w.eva_id == lstAD.Id
                                         && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())

                                      select new vPTREvaluation_obj
                                      {
                                          status_id = lstAD.TM_PTR_Eva_Status.Id + "",
                                          name_en = lstAD.user_no + " : " + lstEmpReq.EmpFullName,
                                          name = lstEmpReq.EmpFullName,
                                          emp_no = lstAD.user_no,
                                          fy_year = lstAD.PTR_Evaluation_Year.evaluation_year.HasValue ? lstAD.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                          eva_status = lstAD.TM_PTR_Eva_Status != null ? lstAD.TM_PTR_Eva_Status.status_name_en : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          sgroup = lstEmpReq.UnitGroupName + "",
                                          srank = lstEmpReq.Rank + "",
                                          sceo = (clst.emp_no + "" != "") ? (clst.emp_name + "<br/>" + "(" + (clst.approva_date.HasValue ? clst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                          spractice = (plst.emp_no + "" != "") ? (plst.emp_name + "<br/>" + "(" + (plst.approva_date.HasValue ? plst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                          sbu = (glst.emp_no + "" != "") ? (glst.emp_name + "<br/>" + "(" + (glst.approva_date.HasValue ? glst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                          self_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Self) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Self).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Self).Annual_Rating.rating_name_en : "") : "",
                                          final_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo).Annual_Rating.rating_name_en : "") : "",
                                          bu_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead).Annual_Rating.rating_name_en : "") : "",
                                          practice_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead).Annual_Rating.rating_name_en : "") : "",
                                          ceoe_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO).Annual_Rating.rating_name_en : "") : "",
                                          Id = lstAD.Id,
                                          ceoe = (ceoelst.emp_no + "" != "") ? (ceoelst.emp_name + "<br/>" + "(" + (ceoelst.approva_date.HasValue ? ceoelst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                      }).ToList();



                    lstData_resutl.ForEach(ed =>
                    {
                        if (ed.status_id != "8")
                        {
                            ed.bu_eva = "-";
                            ed.practice_eva = "-";
                            ed.ceoe_eva = "-";
                            ed.ceoe_eva = "-";
                        }


                    });



                    result.lstData = lstData_resutl.ToList();
                    if (result.lstData.Any())
                    {
                        objSession.lstData = (from item in result.lstData
                                              from lstEva in lstData.AsEnumerable().Where(w => w.Id == item.Id).DefaultIfEmpty(new PTR_Evaluation())
                                              select new vPTREvaluation_report
                                              {
                                                  name = item.name,
                                                  emp_no = item.emp_no,
                                                  eva_status = item.eva_status,
                                                  bu_eva = item.bu_eva,
                                                  final_eva = item.final_eva,
                                                  fy_year = item.fy_year,
                                                  practice_eva = item.practice_eva,
                                                  sbu = item.sbu.Replace("<br/>", " "),
                                                  sceo = item.sceo.Replace("<br/>", " "),
                                                  self_eva = item.self_eva,
                                                  sgroup = item.sgroup,
                                                  spractice = item.spractice.Replace("<br/>", " "),
                                                  srank = item.srank,
                                                  bu_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.gHead).responses + "") : "",
                                                  practice_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.pHead).responses + "") : "",
                                                  final_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.Ceo).responses + "") : "",
                                                  ceoe = item.ceoe.Replace("<br/>", " "),
                                                  ceoe_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApproveEvaluate.DCEO).responses + "") : "",
                                                  ceoe_eva = item.ceoe_eva,
                                              }).ToList();
                        objSession.lstData.ForEach(ed =>
                        {
                            ed.bu_comment = HCMFunc.DataDecryptPES(ed.bu_comment + "");
                            ed.practice_comment = HCMFunc.DataDecryptPES(ed.practice_comment + "");
                            ed.final_comment = HCMFunc.DataDecryptPES(ed.final_comment + "");
                            ed.ceoe_comment = HCMFunc.DataDecryptPES(ed.ceoe_comment + "");
                        });
                    }
                    else
                    {

                    }
                }



            }
            result.Status = SystemFunction.process_Success;
            Session[SearchItem.session] = objSession;
            return Json(new { result });
        }
        #endregion
        #region Ajax Function Plan
        [HttpPost]
        public ActionResult LoadPTRPlanReportsList(CSearchPTREvaluation SearchItem)
        {
            vPTREvaluation_Return result = new vPTREvaluation_Return();
            List<vPTREvaluation_obj> lstData_resutl = new List<vPTREvaluation_obj>();
            int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
            int nStatus = SystemFunction.GetIntNullToZero(SearchItem.status_id);
            var lstData = _PTR_EvaluationService.GetPTRPlanApproveReport(nYear, nStatus, CGlobal.UserInfo.EmployeeNo, CGlobal.UserIsAdminPES());
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);
            rpvPTREvaluation_Session objSession = new rpvPTREvaluation_Session();
            if (lstData.Any())
            {
                string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo) && w.Status == 3 || w.Status == 1).ToList();
                if (_getPartnerUser.Any())
                {
                    #region 
                    List<vPTRApproval_Sumary> lstSelf = new List<vPTRApproval_Sumary>();
                    List<vPTRApproval_Sumary> lstBUHead = new List<vPTRApproval_Sumary>();
                    List<vPTRApproval_Sumary> lstPracticeHead = new List<vPTRApproval_Sumary>();
                    List<vPTRApproval_Sumary> lstCeoHead = new List<vPTRApproval_Sumary>();
                    var lstSelfs = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self)).ToList();
                    if (lstSelfs.Any())
                    {
                        string[] _aApproveNO = lstSelfs.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                        lstSelf = (from item in lstSelfs.Where(w => w.active_status == "Y")
                                   from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                   select new vPTRApproval_Sumary
                                   {
                                       eva_id = item.PTR_Evaluation.Id,
                                       approva_date = item.Approve_date,
                                       emp_name = lstEmpReq.EmpFullName,
                                       emp_no = item.Req_Approve_user,
                                       // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                       rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                       rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                   }).ToList();
                    }
                    var lstBU = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead)).ToList();
                    if (lstBU.Any())
                    {
                        string[] _aApproveNO = lstBU.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                        lstBUHead = (from item in lstBU.Where(w => w.active_status == "Y")
                                     from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                     select new vPTRApproval_Sumary
                                     {
                                         eva_id = item.PTR_Evaluation.Id,
                                         approva_date = item.Approve_date,
                                         emp_name = lstEmpReq.EmpFullName,
                                         emp_no = item.Req_Approve_user,
                                         // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                         rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                         rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                     }).ToList();
                    }
                    var lstPH = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead)).ToList();
                    if (lstPH.Any())
                    {
                        string[] _aApproveNO = lstPH.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                        lstPracticeHead = (from item in lstPH.Where(w => w.active_status == "Y")
                                           from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                           select new vPTRApproval_Sumary
                                           {
                                               eva_id = item.PTR_Evaluation.Id,
                                               approva_date = item.Approve_date,
                                               emp_name = lstEmpReq.EmpFullName,
                                               emp_no = item.Req_Approve_user,
                                               // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                               rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                               rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                           }).ToList();
                    }
                    var lstCeo = lstData.Where(a => a.active_status == "Y" && a.PTR_Evaluation_Approve.Any(a2 => a2.active_status == "Y" && a2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo)).SelectMany(s => s.PTR_Evaluation_Approve.Where(w2 => w2.active_status == "Y" && w2.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo)).ToList();
                    if (lstCeo.Any())
                    {
                        string[] _aApproveNO = lstCeo.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                        var _GetApproveNO = dbHr.AllInfo_WS.Where(w => _aApproveNO.Contains(w.EmpNo)).ToList();
                        lstCeoHead = (from item in lstCeo.Where(w => w.active_status == "Y")
                                      from lstEmpReq in _GetApproveNO.Where(w => w.EmpNo == item.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      select new vPTRApproval_Sumary
                                      {
                                          eva_id = item.PTR_Evaluation.Id,
                                          approva_date = item.Approve_date,
                                          emp_name = lstEmpReq.EmpFullName,
                                          emp_no = item.Req_Approve_user,
                                          // eva_status_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                          rating_id = item.Annual_Rating != null ? item.Annual_Rating.Id : 0,
                                          rating_name = item.Annual_Rating != null ? item.Annual_Rating.rating_name_en : "Waiting",
                                      }).ToList();
                    }
                    #endregion




                    if (!string.IsNullOrEmpty(SearchItem.group_id))
                    {
                        _getPartnerUser = _getPartnerUser.Where(w => w.UnitGroupID + "" == SearchItem.group_id).ToList();
                    }
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                    lstData_resutl = (from lstAD in lstData
                                          // from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      join lstEmpReq in _getPartnerUser on lstAD.user_no equals lstEmpReq.EmpNo
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      from slst in lstSelf.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self) != null
                                       && w.eva_id == lstAD.Id
                                       && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                      from glst in lstBUHead.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead) != null
                                      && w.eva_id == lstAD.Id
                                      && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                      from plst in lstPracticeHead.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead) != null
                                      && w.eva_id == lstAD.Id
                                      && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                      from clst in lstCeoHead.Where(w => lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo) != null
                                      && w.eva_id == lstAD.Id
                                      && w.emp_no == lstAD.PTR_Evaluation_Approve.FirstOrDefault(a => a.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo).Req_Approve_user).DefaultIfEmpty(new vPTRApproval_Sumary())
                                      select new vPTREvaluation_obj
                                      {
                                          name_en = lstAD.user_no + " : " + lstEmpReq.EmpFullName,
                                          name = lstEmpReq.EmpFullName,
                                          emp_no = lstAD.user_no,
                                          fy_year = lstAD.PTR_Evaluation_Year.evaluation_year.HasValue ? lstAD.PTR_Evaluation_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                          eva_status = lstAD.TM_PTR_Eva_Status != null ? lstAD.TM_PTR_Eva_Status.status_name_en : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          sgroup = lstEmpReq.UnitGroupName + "",
                                          srank = lstEmpReq.Rank + "",
                                          sceo = (clst.emp_no + "" != "") ? (clst.emp_name + "<br/>" + "(" + (clst.approva_date.HasValue ? clst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                          spractice = (plst.emp_no + "" != "") ? (plst.emp_name + "<br/>" + "(" + (plst.approva_date.HasValue ? plst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                          sbu = (glst.emp_no + "" != "") ? (glst.emp_name + "<br/>" + "(" + (glst.approva_date.HasValue ? glst.approva_date.Value.DateTimebyCulture() : "Waiting") + ")") : "-",
                                          self_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Self).Annual_Rating.rating_name_en : "") : "",
                                          final_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo).Annual_Rating.rating_name_en : "") : "",
                                          bu_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead).Annual_Rating.rating_name_en : "") : "",
                                          practice_eva = lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead) != null ? (lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead).Annual_Rating != null ? lstAD.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead).Annual_Rating.rating_name_en : "") : "",
                                          Id = lstAD.Id,
                                          update_date = lstEmpUp.EmpFullName + "(" + (lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "") + ")",
                                          p_d = (slst.approva_date.HasValue ? slst.approva_date.Value.DateTimebyCulture() : "Waiting"),
                                      }).ToList();
                    result.lstData = lstData_resutl.ToList();
                    if (result.lstData.Any())
                    {
                        objSession.lstData = (from item in result.lstData
                                              from lstEva in lstData.AsEnumerable().Where(w => w.Id == item.Id).DefaultIfEmpty(new PTR_Evaluation())
                                              select new vPTREvaluation_report
                                              {
                                                  name = item.name,
                                                  emp_no = item.emp_no,
                                                  eva_status = item.eva_status,
                                                  bu_eva = item.bu_eva,
                                                  final_eva = item.final_eva,
                                                  fy_year = item.fy_year,
                                                  practice_eva = item.practice_eva,
                                                  sbu = item.sbu.Replace("<br/>", Environment.NewLine + ""),
                                                  sceo = item.sceo.Replace("<br/>", Environment.NewLine + ""),
                                                  self_eva = item.self_eva,
                                                  sgroup = item.sgroup,
                                                  spractice = item.spractice.Replace("<br/>", Environment.NewLine + ""),
                                                  srank = item.srank,
                                                  bu_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.gHead).responses + "") : "",
                                                  practice_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.pHead).responses + "") : "",
                                                  final_comment = lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo) != null ? (lstEva.PTR_Evaluation_Approve.FirstOrDefault(f => f.TM_PTR_Eva_ApproveStep.Id == (int)PESClass.StepApprovePlan.Ceo).responses + "") : "",
                                                  last_update = item.update_date.Replace("(", Environment.NewLine + "("),
                                                  p_d = item.p_d,
                                              }).ToList();
                        objSession.lstData.ForEach(ed =>
                        {
                            ed.bu_comment = HCMFunc.DataDecryptPES(ed.bu_comment + "");
                            ed.practice_comment = HCMFunc.DataDecryptPES(ed.practice_comment + "");
                            ed.final_comment = HCMFunc.DataDecryptPES(ed.final_comment + "");

                        });
                    }
                    else
                    {

                    }
                }


            }
            result.Status = SystemFunction.process_Success;
            Session[SearchItem.session] = objSession;
            return Json(new { result });
        }
        #endregion
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