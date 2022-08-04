using DevExpress.XtraPrinting;
using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Report.DevReport.Form;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TIFForm;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using HumanCapitalManagement.ViewModel.ReportVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HumanCapitalManagement.App_Start.HCMClass;

namespace HumanCapitalManagement.Controllers.MainController
{
    public class TraineeEvaController : BaseController
    {
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private TM_Evaluation_FormService _TM_Evaluation_FormService;
        private TM_Eva_RatingService _TM_Eva_RatingService;
        private TM_Trainee_EvaService _TM_Trainee_EvaService;
        private TM_Trainee_Eva_AnswerService _TM_Trainee_Eva_AnswerService;
        private TM_TraineeEva_StatusService _TM_TraineeEva_StatusService;
        private MailContentService _MailContentService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        private TM_Evaluation_QuestionService _TM_Evaluation_QuestionService;
        public TraineeEvaController(
         TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , TM_Evaluation_FormService TM_Evaluation_FormService
            , TM_Eva_RatingService TM_Eva_RatingService
            , TM_Trainee_EvaService TM_Trainee_EvaService
            , TM_Trainee_Eva_AnswerService TM_Trainee_Eva_AnswerService
            , TM_TraineeEva_StatusService TM_TraineeEva_StatusService
            , TM_Evaluation_QuestionService TM_Evaluation_QuestionService
            , MailContentService MailContentService)
        {
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TM_Evaluation_FormService = TM_Evaluation_FormService;
            _TM_Eva_RatingService = TM_Eva_RatingService;
            _TM_Trainee_EvaService = TM_Trainee_EvaService;
            _TM_Trainee_Eva_AnswerService = TM_Trainee_Eva_AnswerService;
            _TM_TraineeEva_StatusService = TM_TraineeEva_StatusService;
            _MailContentService = MailContentService;
            _TM_Evaluation_QuestionService = TM_Evaluation_QuestionService;
        }
        // GET: TraineeEva
        public ActionResult TraineeEvaList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.IsRecruitmentOrAdmin();
            vTraineeEva result = new vTraineeEva();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                List<vTraineeEva_obj> lstData_resutl = new List<vTraineeEva_obj>();
                CSearchTraineeEva SearchItem = (CSearchTraineeEva)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchTraineeEva)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                IQueryable<Employee> sQuery = dbHr.Employee.Where(w => 1 == 1);
                string[] UserNo = null;
                if (!string.IsNullOrEmpty(SearchItem.eva_name))
                {
                    UserNo = sQuery.Where(w =>
                           (
                           (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                           ).Contains((SearchItem.eva_name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                            ).Select(s => ((s.Employeeno + "").Trim()).ToLower()).ToArray();
                }
                string[] UserNoIC = null;
                if (!string.IsNullOrEmpty(SearchItem.eva_ic_name))
                {
                    UserNoIC = dbHr.Employee.Where(w =>
                           (
                           (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                           ).Contains((SearchItem.eva_ic_name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                            ).Select(s => ((s.Employeeno + "").Trim()).ToLower()).ToArray();
                }
                var lstData = _TM_Trainee_EvaService.GetEvaForApprove(
                                                   SearchItem.group_code,
                                                   aDivisionPermission,
                                                   UserNo,
                                                   UserNoIC,
                                                   CGlobal.UserInfo.EmployeeNo,
                                                   SearchItem.name,
                                                   isAdmin).ToList();
                result.group_code = SearchItem.group_code;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    string[] aApproveUser = lstData.Select(s => (s.approve_type + "" == "M" ? s.req_mgr_Approve_user : s.req_incharge_Approve_user)).ToArray();
                    List<string> lstUser = new List<string>();
                    lstUser.AddRange(lstData.Select(s => s.req_incharge_Approve_user).ToList());
                    lstUser.AddRange(lstData.Select(s => s.req_mgr_Approve_user).ToList());

                    var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();
                    var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                    lstData_resutl = (from lstAD in lstData.AsEnumerable()
                                      select new vTraineeEva_obj
                                      {
                                          pm_name = _getUser.Where(w => w.EmpNo == lstAD.req_mgr_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                          incharge_name = _getUser.Where(w => w.EmpNo == lstAD.req_incharge_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                          key_type = lstAD.approve_type + "" != "" ? (lstAD.approve_type + "" == "M" ? "PM’s Name" : "In-Charge") : "",
                                          trainee_name = lstAD.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                          eva_status = lstAD.TM_TraineeEva_Status != null ? lstAD.TM_TraineeEva_Status.eva_status_name_en : "",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          sgroup = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          bu_approve = lstAD.approve_type == "M" ? (lstAD.mgr_Approve_date.HasValue ? lstAD.mgr_Approve_date.Value.DateTimebyCulture() : "") : (lstAD.incharge_Approve_date.HasValue ? lstAD.incharge_Approve_date.Value.DateTimebyCulture() : ""),
                                          hr_ack = lstAD.acknowledge_date.HasValue ? lstAD.acknowledge_date.Value.DateTimebyCulture() : "",
                                          target_start = lstAD.TM_PR_Candidate_Mapping.trainee_start.HasValue ? lstAD.TM_PR_Candidate_Mapping.trainee_start.Value.DateTimebyCulture() : "",
                                          target_end = lstAD.TM_PR_Candidate_Mapping.trainee_end.HasValue ? lstAD.TM_PR_Candidate_Mapping.trainee_end.Value.DateTimebyCulture() : "",

                                      }).ToList();
                }


                result.lstData = lstData_resutl.ToList();
            }

            #endregion
            return View(result);
        }
        public ActionResult TraineeEvaEdit(string qryStr)
        {
           


            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vTraineeEva_obj_Save result = new vTraineeEva_obj_Save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                result.IdEncrypt = qryStr;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    result.scomplete = "";
                    var _getDataEva = _TM_Trainee_EvaService.FindForApprove(nId, CGlobal.UserInfo.EmployeeNo, CGlobal.IsRecruitmentOrAdmin());
                    if (_getDataEva != null && _getDataEva.TM_PR_Candidate_Mapping != null)
                    {
                        if (_getDataEva.approve_type + "" == "M")
                        {
                            if (_getDataEva.req_mgr_Approve_user != CGlobal.UserInfo.EmployeeNo && !CGlobal.IsRecruitmentOrAdmin())
                            {
                                result.scomplete = "Y";
                            }
                        }
                        else
                        {
                            if (_getDataEva.req_incharge_Approve_user != CGlobal.UserInfo.EmployeeNo && !CGlobal.IsRecruitmentOrAdmin())
                            {
                                result.scomplete = "Y";
                            }
                        }
                        var _getData = _getDataEva.TM_PR_Candidate_Mapping;
                        result.group_name = _getData.PersonnelRequest.TM_Divisions.division_name_en + "";
                        result.trainee_name = _getData.TM_Candidates.first_name_en + " " + _getData.TM_Candidates.last_name_en;
                        result.position = _getData.PersonnelRequest.TM_Position.position_name_en + "";
                        result.no_eva = _getData.PersonnelRequest.no_of_eva + "";
                        result.nick_name = _getData.TM_Candidates.candidate_NickName + "";
                        result.trainee_no = _getData.TM_Candidates.candidate_TraineeNumber + "";
                        result.target_start = _getData.trainee_start.HasValue ? _getData.trainee_start.Value.DateTimebyCulture() : "";
                        result.target_end = _getData.trainee_end.HasValue ? _getData.trainee_end.Value.DateTimebyCulture() : "";
                        result.status = _getDataEva.submit_status + "";

                        if (_getDataEva.approve_type + "" == "M" && _getDataEva.mgr_Approve_status == "Y")
                        {
                            result.scomplete = "Y";
                        }
                        if (_getDataEva.approve_type + "" == "I" && _getDataEva.incharge_Approve_status == "Y")
                        {
                            result.scomplete = "Y";
                        }
                        //Add Code New Question 
                        var _getActEva = _TM_Trainee_Eva_AnswerService.findByTraineeEvaId(nId); // for get info of eva form 

                        var chkFormId = _getActEva.TM_Evaluation_Question.TM_Evaluation_Form.Id;
                        //Add Code New Question 
                        var _getEva = _TM_Evaluation_FormService.Find(chkFormId);
                        if (_getEva.Id == 7)
                        {
                            //Question
                            var _getlstQuestion = _TM_Evaluation_QuestionService.FindQuestion(_getEva.Id); /*for get a question by eva form id*/
                            result.objtifform = new vObject_of_TrainEva();
                            result.objtifform.TIF_no = _getEva.Id + "";
                            result.objtifform.lstQuestion = (from lstQ in _getlstQuestion
                                                             select new vEva_list_question
                                                             {
                                                                 id = lstQ.Id + "",
                                                                 question = lstQ.question,
                                                                 sgroup = lstQ.header,
                                                                 nSeq = lstQ.seq,
                                                                 remark = "",
                                                                 rating = "",
                                                                 hiringratingname = "",
                                                                 sDisable = result.scomplete,
                                                             }).ToList();
                        }
                        else
                        {
                            var _getlstQuestion = _TM_Evaluation_QuestionService.FindQuestion(_getEva.Id); /*for get a question by eva form id*/
                            result.objtifform = new vObject_of_TrainEva();
                            result.objtifform.TIF_no = _getEva.Id + "";
                            result.objtifform.lstQuestion = (from lstQ in _getlstQuestion
                                                             select new vEva_list_question
                                                             {
                                                                 id = lstQ.Id + "",
                                                                 question = lstQ.question,
                                                                 sgroup = lstQ.header,
                                                                 nSeq = lstQ.seq,
                                                                 remark = "",
                                                                 rating = "",
                                                                 sDisable = result.scomplete,
                                                             }).ToList();

                        }


                        if (_getEva != null && _getEva.TM_Evaluation_Question != null && _getEva.TM_Evaluation_Question.Any(a => a.active_status == "Y"))
                        {
                            //ดึงข้อมูลแบบใหม่
                            if(_getEva.Id >= 7)
                            {


                                //GetAns Rating by form
                                result.objtifform.lstRating = _TM_Eva_RatingService.GetDataForSelect().Select(s => new lstDataSelect
                                {
                                    value = s.Id + "",
                                    text = s.point + " : " + s.rating_name_en + "",
                                    detail = s.rating_description + "",
                                    
                            }).ToList();
                                //Get Hiring Rating New
                                result.hiringratingname = _getDataEva.TM_Trainee_HiringRating_Id + "";
                            }
                            //ดึงข้อมูลแบบเก่า
                            else if (_getEva.Id == 4 || _getEva.Id <7)
                            {
                                result.objtifform = new vObject_of_TrainEva();
                                result.objtifform.TIF_no = _getEva.Id + "";
                                result.objtifform.lstQuestion = (from lstQ in _getEva.TM_Evaluation_Question
                                                                 select new vEva_list_question
                                                                 {
                                                                     id = lstQ.Id + "",
                                                                     question = lstQ.question,
                                                                     sgroup = lstQ.header,
                                                                     nSeq = lstQ.seq,
                                                                     remark = "",
                                                                     rating = "",
                                                                     sDisable = result.scomplete,
                                                                 }).ToList();
                                //GetAns Rating Form 
                                result.objtifform.lstRating = _TM_Eva_RatingService.GetDataForSelectOld().Select(s => new lstDataSelect
                                {
                                    value = s.Id + "",
                                    text = s.point + " : " + s.rating_name_en + "",
                                    detail = s.rating_description + "",

                                }).ToList();
                                //getAns Hiring Radio buttton
                                result.hiring_status = _getDataEva.hiring_status;

                            }
                            result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                        }
                        result.IdEncryptBack = HCMFunc.Encrypt(_getDataEva.TM_PR_Candidate_Mapping.Id + "");
                        result.comment = _getDataEva.trainee_learned;
                        result.comment2 = _getDataEva.trainee_done_well;
                        result.comment3 = _getDataEva.trainee_developmental;
                        result.type_approve = _getDataEva.approve_type + "" == "M" ? "Performance Manager" : "In-Charge / Engagement Manager";
                        result.confidentiality_agreement = _getDataEva.confidentiality_agreement;
                       
                        result.incharge_comments = _getDataEva.incharge_comments;



                        if (!string.IsNullOrEmpty(_getDataEva.req_mgr_Approve_user))
                        {
                            var Getmrg = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getDataEva.req_mgr_Approve_user).FirstOrDefault();
                            if (Getmrg != null)
                            {
                                result.mgr_user_no = _getDataEva.req_mgr_Approve_user;
                                result.mgr_user_name = Getmrg.EmpFullName;
                                result.mgr_unit_name = Getmrg.UnitGroup;
                                result.mgr_user_rank = Getmrg.Rank;
                            }
                        }
                        if (!string.IsNullOrEmpty(_getDataEva.req_incharge_Approve_user))
                        {
                            var Getmrg = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getDataEva.req_incharge_Approve_user).FirstOrDefault();
                            if (Getmrg != null)
                            {
                                result.ic_user_no = _getDataEva.req_incharge_Approve_user;
                                result.ic_user_name = Getmrg.EmpFullName;
                                result.ic_unit_name = Getmrg.UnitGroup;
                                result.ic_user_rank = Getmrg.Rank;
                            }
                        }

                        if (_getDataEva.TM_Trainee_Eva_Answer != null && _getDataEva.TM_Trainee_Eva_Answer.Any(a => a.active_status == "Y"))
                        {
                            result.objtifform.lstQuestion.ForEach(ed =>
                            {
                                var GetAns = _getDataEva.TM_Trainee_Eva_Answer.Where(w => w.TM_Evaluation_Question.Id + "" == ed.id).FirstOrDefault();
                                if (GetAns != null)
                                {

                                    ed.rating = GetAns.trainee_rating != null ? GetAns.trainee_rating.point + " : " + GetAns.trainee_rating.rating_name_en + "" : "";
                                    ed.approve_rating = GetAns.inchage_rating != null ? GetAns.inchage_rating.Id + "" : "";
                                    ed.remark = GetAns.inchage_comment;
                                }
                            });
                        }
                        //Add Code New Question 
                        var _getformsend = _TM_Trainee_Eva_AnswerService.findByTraineeEvaId(nId); // for get info of eva form 

                        var chkForm = _getformsend.TM_Evaluation_Question.TM_Evaluation_Form.Id;
                        //Add Code New Question 
                        var _getformeva = _TM_Evaluation_FormService.Find(chkForm);
                        //Hiring Rate 
                        var chkhiring = SystemFunction.GetIntNullToZero(_getDataEva.hiring_status);
                        if (_getformeva.Id == 4 && chkhiring == 0)
                        {
                            return View("TraineeEvaEdit", result);
                        }
                        else
                        {
                            return View("TraineeEvaEdit_NewMode", result);
                        }
                    }
                    else
                    {
                        return RedirectToAction("TraineeEvaList", "TraineeEva");
                    }
                }
                else
                {
                    return RedirectToAction("TraineeEvaList", "TraineeEva");
                }
            }
            else
            {
                return RedirectToAction("TraineeEvaList", "TraineeEva");
            }

            //return View(result);

            return RedirectToAction("TraineeEvaList", "TraineeEva");

        }
        
        [HttpPost]
        public ActionResult evaluateScore(List<int> answer_list)
        {
            var statusDict = new Dictionary<string, int>();
            statusDict.Add("i", 1);
            statusDict.Add("n", 2);
            statusDict.Add("l", 3);
            statusDict.Add("f", 4);

            string status = _TM_Trainee_EvaService.InternEva(answer_list);
            return Json(new { status = statusDict[status], ok = true });
        }
  
        public ActionResult AcKnowledgeTraineeEvaList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.IsRecruitmentOrAdmin();
            vTraineeEva result = new vTraineeEva();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                List<vTraineeEva_obj> lstData_resutl = new List<vTraineeEva_obj>();
                CSearchTraineeEva SearchItem = (CSearchTraineeEva)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchTraineeEva)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                DateTime? dStart = null, dStartTo = null, dEnd = null, dEndTo = null;
                if (!string.IsNullOrEmpty(SearchItem.start_date))
                {
                    dStart = SystemFunction.ConvertStringToDateTime(SearchItem.start_date, "");
                }
                if (!string.IsNullOrEmpty(SearchItem.start_to))
                {
                    dStartTo = SystemFunction.ConvertStringToDateTime(SearchItem.start_to, "");
                }
                if (!string.IsNullOrEmpty(SearchItem.end_date))
                {
                    dEnd = SystemFunction.ConvertStringToDateTime(SearchItem.end_date, "");
                }
                if (!string.IsNullOrEmpty(SearchItem.end_to))
                {
                    dEndTo = SystemFunction.ConvertStringToDateTime(SearchItem.end_to, "");
                }
                IQueryable<Employee> sQuery = dbHr.Employee.Where(w => 1 == 1);
                string[] UserNo = null;
                if (!string.IsNullOrEmpty(SearchItem.eva_name))
                {
                    UserNo = sQuery.Where(w =>
                           (
                           (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                           ).Contains((SearchItem.eva_name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                            ).Select(s => ((s.Employeeno + "").Trim()).ToLower()).ToArray();
                }
                string[] UserNoIC = null;
                if (!string.IsNullOrEmpty(SearchItem.eva_ic_name))
                {
                    UserNoIC = dbHr.Employee.Where(w =>
                           (
                           (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                           ).Contains((SearchItem.eva_ic_name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                            ).Select(s => ((s.Employeeno + "").Trim()).ToLower()).ToArray();
                }
                var lstData = _TM_Trainee_EvaService.GetEvaForAcknow(SearchItem.group_code,
                    aDivisionPermission,
                     UserNo,
                    UserNoIC,
                    CGlobal.UserInfo.EmployeeNo,
                    SearchItem.name,
                    dStart,
                    dStartTo,
                    dEnd,
                    dEndTo,
                    isAdmin).ToList();
                result.group_code = SearchItem.group_code;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    List<string> lstUser = new List<string>();
                    lstUser.AddRange(lstData.Select(s => s.req_incharge_Approve_user).ToList());
                    lstUser.AddRange(lstData.Select(s => s.req_mgr_Approve_user).ToList());
                    var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();
                    string sUrl = Url.Action("AcKnowledgeTraineeEvaEdit", "TraineeEva", null, Request.Url.Scheme);
                    lstData_resutl = (from lstAD in lstData.AsEnumerable()
                                      select new vTraineeEva_obj
                                      {
                                          IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                          Id = lstAD.Id,
                                          pm_name = _getUser.Where(w => w.EmpNo == lstAD.req_mgr_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                          incharge_name = _getUser.Where(w => w.EmpNo == lstAD.req_incharge_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                          key_type = lstAD.approve_type + "" != "" ? (lstAD.approve_type + "" == "M" ? "PM’s Name" : "In-Charge") : "",
                                          trainee_name = @"<a target=""_blank""  href=""" + sUrl + "?qryStr=" + HCMFunc.Encrypt(lstAD.Id + "") + @""">" + lstAD.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name() + @"</a>",
                                          eva_status = lstAD.TM_TraineeEva_Status != null ? lstAD.TM_TraineeEva_Status.eva_status_name_en : "",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          sgroup = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          bu_approve = lstAD.approve_type == "M" ? (lstAD.mgr_Approve_date.HasValue ? lstAD.mgr_Approve_date.Value.DateTimebyCulture() : "") : (lstAD.incharge_Approve_date.HasValue ? lstAD.incharge_Approve_date.Value.DateTimebyCulture() : ""),
                                          hr_ack = lstAD.acknowledge_date.HasValue ? lstAD.acknowledge_date.Value.DateTimebyCulture() : "",
                                          target_start = lstAD.TM_PR_Candidate_Mapping.trainee_start.HasValue ? lstAD.TM_PR_Candidate_Mapping.trainee_start.Value.DateTimebyCulture() : "",
                                          target_end = lstAD.TM_PR_Candidate_Mapping.trainee_end.HasValue ? lstAD.TM_PR_Candidate_Mapping.trainee_end.Value.DateTimebyCulture() : "",
                                          hiring_status = String.IsNullOrEmpty(lstAD.hiring_status + "") ?lstAD.TM_Trainee_HiringRating.Trainee_HiringRating_name_en : lstAD.hiring_status + "" == "Y"? "Yes":"No",
                                         
                                          
                                          
                                          //hiring_status = lstAD.hiring_status + "" == "Y" ? "Yes" : "No",
                                          evaluator_name = lstAD.approve_type + "" == "M" ? _getUser.Where(w => w.EmpNo == lstAD.req_mgr_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + ""
                                      : _getUser.Where(w => w.EmpNo == lstAD.req_incharge_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                      }).ToList();
                }
                result.lstData = lstData_resutl.ToList();
            }
            #endregion
            return View(result);
        }
        public ActionResult AcKnowledgeTraineeEvaEdit(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vTraineeEva_obj_Save result = new vTraineeEva_obj_Save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                result.IdEncrypt = qryStr;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    result.scomplete = "";
                    var _getDataEva = _TM_Trainee_EvaService.FindForApprove(nId, CGlobal.UserInfo.EmployeeNo, CGlobal.IsRecruitmentOrAdmin());
                    if (_getDataEva != null && _getDataEva.TM_PR_Candidate_Mapping != null)
                    {
                        if (_getDataEva.approve_type + "" == "M")
                        {
                            if (_getDataEva.req_mgr_Approve_user != CGlobal.UserInfo.EmployeeNo && !CGlobal.IsRecruitmentOrAdmin())
                            {
                                result.scomplete = "Y";
                            }
                        }
                        else
                        {
                            if (_getDataEva.req_incharge_Approve_user != CGlobal.UserInfo.EmployeeNo && !CGlobal.IsRecruitmentOrAdmin())
                            {
                                result.scomplete = "Y";
                            }
                        }
                        var _getData = _getDataEva.TM_PR_Candidate_Mapping;
                        result.group_name = _getData.PersonnelRequest.TM_Divisions.division_name_en + "";
                        result.trainee_name = _getData.TM_Candidates.first_name_en + " " + _getData.TM_Candidates.last_name_en;
                        result.position = _getData.PersonnelRequest.TM_Position.position_name_en + "";
                        result.no_eva = _getData.PersonnelRequest.no_of_eva + "";
                        result.nick_name = _getData.TM_Candidates.candidate_NickName + "";
                        result.trainee_no = _getData.TM_Candidates.candidate_TraineeNumber + "";
                        result.target_start = _getData.trainee_start.HasValue ? _getData.trainee_start.Value.DateTimebyCulture() : "";
                        result.target_end = _getData.trainee_end.HasValue ? _getData.trainee_end.Value.DateTimebyCulture() : "";
                        result.status = _getDataEva.submit_status + "";

                        if (_getDataEva.approve_type + "" == "M" && _getDataEva.mgr_Approve_status == "Y")
                        {
                            result.scomplete = "Y";
                        }
                        if (_getDataEva.approve_type + "" == "I" && _getDataEva.incharge_Approve_status == "Y")
                        {
                            result.scomplete = "Y";
                        }
                        //Add Code New Question 
                        var _getActEva = _TM_Trainee_Eva_AnswerService.findByTraineeEvaId(nId); // for get info of eva form 

                        var chkFormId = _getActEva.TM_Evaluation_Question.TM_Evaluation_Form.Id;
                        //Add Code New Question 
                        var _getEva = _TM_Evaluation_FormService.Find(chkFormId);
                        if (_getEva.Id >= 7)
                        {
                            //Question
                            var _getlstQuestion = _TM_Evaluation_QuestionService.FindQuestion(_getEva.Id); /*for get a question by eva form id*/
                            result.objtifform = new vObject_of_TrainEva();
                            result.objtifform.TIF_no = _getEva.Id + "";
                            result.objtifform.lstQuestion = (from lstQ in _getlstQuestion
                                                             select new vEva_list_question
                                                             {
                                                                 id = lstQ.Id + "",
                                                                 question = lstQ.question,
                                                                 sgroup = lstQ.header,
                                                                 nSeq = lstQ.seq,
                                                                 remark = "",
                                                                 rating = "",
                                                                 sDisable = result.scomplete,
                                                             }).ToList();
                        }
                        else
                        {
                            var _getlstQuestion = _TM_Evaluation_QuestionService.FindQuestion(_getEva.Id); /*for get a question by eva form id*/
                            result.objtifform = new vObject_of_TrainEva();
                            result.objtifform.TIF_no = _getEva.Id + "";
                            result.objtifform.lstQuestion = (from lstQ in _getlstQuestion
                                                             select new vEva_list_question
                                                             {
                                                                 id = lstQ.Id + "",
                                                                 question = lstQ.question,
                                                                 sgroup = lstQ.header,
                                                                 nSeq = lstQ.seq,
                                                                 remark = "",
                                                                 rating = "",
                                                                 sDisable = result.scomplete,
                                                             }).ToList();

                        }
                        //GetAns
                        if (_getEva != null && _getEva.TM_Evaluation_Question != null && _getEva.TM_Evaluation_Question.Any(a => a.active_status == "Y"))
                        {
                            result.objtifform = new vObject_of_TrainEva();
                            result.objtifform.TIF_no = _getEva.Id + "";
                            result.objtifform.lstQuestion = (from lstQ in _getEva.TM_Evaluation_Question
                                                             select new vEva_list_question
                                                             {
                                                                 id = lstQ.Id + "",
                                                                 question = lstQ.question,
                                                                 sgroup = lstQ.header,
                                                                 nSeq = lstQ.seq,
                                                                 remark = "",
                                                                 rating = "",
                                                                 sDisable = result.scomplete,
                                                             }).ToList();

                            //Add Code 15/9/2020 for Edit Rating Eva
                            if (_getEva.Id >= 7)
                            {
                                result.objtifform.lstRating = _TM_Eva_RatingService.GetDataForSelect().Where(w=> w.TM_Evaluation_Form.Id == _getEva.Id).Select(s => new lstDataSelect
                                {
                                    value = s.Id + "",
                                    text = s.point + " : " + s.rating_name_en + "",
                                    detail = s.rating_description + "",

                                }).ToList();

                                //Get Hiring Rating New
                                result.hiringratingname = _getDataEva.TM_Trainee_HiringRating_Id + "";
                            }
                            else if (_getEva.Id == 4)
                            {
                                result.objtifform.lstRating = _TM_Eva_RatingService.GetDataForSelectOld().Where(w=> w.TM_Evaluation_Form.Id == _getEva.Id).Select(s => new lstDataSelect
                                {
                                    value = s.Id + "",
                                    text = s.point + " : " + s.rating_name_en + "",
                                    detail = s.rating_description + "",

                                }).ToList();
                            }
                            result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });

                        }
                        result.IdEncryptBack = HCMFunc.Encrypt(_getDataEva.TM_PR_Candidate_Mapping.Id + "");
                        result.comment = _getDataEva.trainee_learned;
                        result.comment2 = _getDataEva.trainee_done_well;
                        result.comment3 = _getDataEva.trainee_developmental;
                        result.type_approve = _getDataEva.approve_type + "" == "M" ? "Performance Manager" : "In-Charge / Engagement Manager";
                        result.confidentiality_agreement = _getDataEva.confidentiality_agreement;
                        result.hiring_status = _getDataEva.hiring_status;
                        result.incharge_comments = _getDataEva.incharge_comments;


                        if (!string.IsNullOrEmpty(_getDataEva.req_mgr_Approve_user))
                        {
                            var Getmrg = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getDataEva.req_mgr_Approve_user).FirstOrDefault();
                            if (Getmrg != null)
                            {
                                result.mgr_user_no = _getDataEva.req_mgr_Approve_user;
                                result.mgr_user_name = Getmrg.EmpFullName;
                                result.mgr_unit_name = Getmrg.UnitGroup;
                                result.mgr_user_rank = Getmrg.Rank;
                            }
                        }
                        if (!string.IsNullOrEmpty(_getDataEva.req_incharge_Approve_user))
                        {
                            var Getmrg = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getDataEva.req_incharge_Approve_user).FirstOrDefault();
                            if (Getmrg != null)
                            {
                                result.ic_user_no = _getDataEva.req_incharge_Approve_user;
                                result.ic_user_name = Getmrg.EmpFullName;
                                result.ic_unit_name = Getmrg.UnitGroup;
                                result.ic_user_rank = Getmrg.Rank;
                            }
                        }

                        if (_getDataEva.TM_Trainee_Eva_Answer != null && _getDataEva.TM_Trainee_Eva_Answer.Any(a => a.active_status == "Y"))
                        {
                            result.objtifform.lstQuestion.ForEach(ed =>
                            {
                                var GetAns = _getDataEva.TM_Trainee_Eva_Answer.Where(w => w.TM_Evaluation_Question.Id + "" == ed.id).FirstOrDefault();
                                if (GetAns != null)
                                {

                                    ed.rating = GetAns.trainee_rating != null ? GetAns.trainee_rating.point + " : " + GetAns.trainee_rating.rating_name_en + "" : "";
                                    ed.approve_rating = GetAns.inchage_rating != null ? GetAns.inchage_rating.Id + "" : "";
                                    ed.remark = GetAns.inchage_comment;
                                }
                            });
                        }

                        if (_getEva.Id >= 7)
                        {
                            return View("AcKnowledgeTraineeEvaEditNew", result);
                        }
                        else if (_getEva.Id < 7)
                        {
                            return View("AcKnowledgeTraineeEvaEdit", result);
                        }
                    }

                    else
                    {
                        return RedirectToAction("TraineeEvaList", "TraineeEva");
                    }

                }
                else
                {
                    return RedirectToAction("TraineeEvaList", "TraineeEva");
                }
            }
            else
            {
                return RedirectToAction("TraineeEvaList", "TraineeEva");
            }
            return RedirectToAction("TraineeEvaList", "TraineeEva");
            //return View(result);

        }

        public ActionResult TraineeEvaReportList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.IsRecruitmentOrAdmin();
            vTraineeEva result = new vTraineeEva();
            result.active_status = "Y";
            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "rpEva" + unixTimestamps;
            result.session = sSession;
            rpvEva_Session objSession = new rpvEva_Session();
            Session[sSession] = new rpvEva_Session();//new List<ListAdvanceTransaction>();
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                List<vTraineeEva_obj> lstData_resutl = new List<vTraineeEva_obj>();
                CSearchTraineeEva SearchItem = (CSearchTraineeEva)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchTraineeEva)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                int nEvaStatus = SystemFunction.GetIntNullToZero(SearchItem.eva_status);
                DateTime? dStart = null, dStartTo = null, dEnd = null, dEndTo = null;
                if (!string.IsNullOrEmpty(SearchItem.start_date))
                {
                    dStart = SystemFunction.ConvertStringToDateTime(SearchItem.start_date, "");
                }
                if (!string.IsNullOrEmpty(SearchItem.start_to))
                {
                    dStartTo = SystemFunction.ConvertStringToDateTime(SearchItem.start_to, "");
                }
                if (!string.IsNullOrEmpty(SearchItem.end_date))
                {
                    dEnd = SystemFunction.ConvertStringToDateTime(SearchItem.end_date, "");
                }
                if (!string.IsNullOrEmpty(SearchItem.end_to))
                {
                    dEndTo = SystemFunction.ConvertStringToDateTime(SearchItem.end_to, "");
                }
                IQueryable<Employee> sQuery = dbHr.Employee.Where(w => 1 == 1);
                string[] UserNo = null;
                if (!string.IsNullOrEmpty(SearchItem.eva_name))
                {
                    UserNo = sQuery.Where(w =>
                           (
                           (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                           ).Contains((SearchItem.eva_name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                            ).Select(s => ((s.Employeeno + "").Trim()).ToLower()).ToArray();
                }
                string[] UserNoIC = null;
                if (!string.IsNullOrEmpty(SearchItem.eva_ic_name))
                {
                    UserNoIC = dbHr.Employee.Where(w =>
                           (
                           (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                           ).Contains((SearchItem.eva_ic_name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                           (w.Employeename + "").Trim().ToLower() +
                           (w.Employeesurname + "").Trim().ToLower() +
                           (w.EmpThaiName + "").Trim().ToLower() +
                           (w.EmpThaiSurName + "").Trim().ToLower() +
                            ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                            ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                            ).Select(s => ((s.Employeeno + "").Trim()).ToLower()).ToArray();
                }
                var lstData = _TM_Trainee_EvaService.GetEvaForReport(SearchItem.group_code,
                    aDivisionPermission,
                    UserNo,
                    UserNoIC,
                    CGlobal.UserInfo.EmployeeNo,
                    SearchItem.name,
                    nEvaStatus,
                    dStart,
                    dStartTo,
                    dEnd,
                    dEndTo,
                    isAdmin).ToList();
                result.group_code = SearchItem.group_code;
                result.name = SearchItem.name;
                result.eva_status = SearchItem.eva_status;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    string[] aApproveUser = lstData.Select(s => (s.approve_type + "" == "M" ? s.req_mgr_Approve_user : s.req_incharge_Approve_user)).ToArray();
                    List<string> lstUser = new List<string>();
                    lstUser.AddRange(lstData.Select(s => s.req_incharge_Approve_user).ToList());
                    lstUser.AddRange(lstData.Select(s => s.req_mgr_Approve_user).ToList());
                    lstUser.AddRange(lstData.Select(s => s.acknowledge_user).ToList());
                    var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();
                    objSession.lstData = lstData.ToList();

                    lstData_resutl = (from lstAD in lstData.AsEnumerable()
                                      from lstMGR in _getUser.Where(w => w.EmpNo == lstAD.req_mgr_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstIC in _getUser.Where(w => w.EmpNo == lstAD.req_incharge_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstHR in _getUser.Where(w => w.EmpNo == lstAD.acknowledge_user).DefaultIfEmpty(new AllInfo_WS())
                                      select new vTraineeEva_obj
                                      {
                                          IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                          Id = lstAD.Id,
                                          hr_name = lstHR.EmpFullName + "",
                                          pm_name = lstMGR.EmpFullName + "",
                                          incharge_name = lstIC.EmpFullName + "",
                                          key_type = lstAD.approve_type + "" != "" ? (lstAD.approve_type + "" == "M" ? "PM’s Name" : "In-Charge") : "",
                                          trainee_name = lstAD.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                          eva_status = lstAD.TM_TraineeEva_Status != null ? lstAD.TM_TraineeEva_Status.eva_status_name_en : "",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          sgroup = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          bu_approve = lstAD.approve_type == "M" ? (lstAD.mgr_Approve_date.HasValue ? lstAD.mgr_Approve_date.Value.DateTimebyCulture() : "") : (lstAD.incharge_Approve_date.HasValue ? lstAD.incharge_Approve_date.Value.DateTimebyCulture() : ""),
                                          hr_ack = lstAD.acknowledge_date.HasValue ? lstAD.acknowledge_date.Value.DateTimebyCulture() : "",
                                          target_start = lstAD.TM_PR_Candidate_Mapping.trainee_start.HasValue ? lstAD.TM_PR_Candidate_Mapping.trainee_start.Value.DateTimebyCulture() : "",
                                          target_end = lstAD.TM_PR_Candidate_Mapping.trainee_end.HasValue ? lstAD.TM_PR_Candidate_Mapping.trainee_end.Value.DateTimebyCulture() : "",
                                          hiring_status = lstAD.hiring_status + "" == "" ? lstAD.TM_Trainee_HiringRating.Trainee_HiringRating_name_en : (lstAD.hiring_status + "" == "Y" ? "Yes" : "No"),
                                          evaluator_name = lstAD.approve_type + "" == "M" ? lstMGR.EmpFullName + "" : lstIC.EmpFullName + "",
                                          evaluator_mail = lstAD.approve_type + "" == "M" ? lstMGR.Email + "" : lstIC.Email + "",
                                          pm_no = lstAD.req_mgr_Approve_user + "",
                                          ic_no = lstAD.req_incharge_Approve_user + "",
                                          surname = lstAD.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "",
                                          name = lstAD.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "",
                                          nickname = lstAD.TM_PR_Candidate_Mapping.TM_Candidates.candidate_NickName + "",
                                      }).ToList();
                    objSession.lstTraineeTracking = lstData_resutl.ToList();
                    Session[sSession] = objSession;
                }


                result.lstData = lstData_resutl.ToList();
            }

            #endregion
            return View(result);
        }
        public ActionResult TraineeEvaReportView(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vTraineeEva_obj_Save result = new vTraineeEva_obj_Save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                result.IdEncrypt = qryStr;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    result.scomplete = "";
                    var _getDataEva = _TM_Trainee_EvaService.Find(nId);
                    if (_getDataEva != null && _getDataEva.TM_PR_Candidate_Mapping != null)
                    {
                        if (_getDataEva.approve_type + "" == "M")
                        {
                            if (_getDataEva.req_mgr_Approve_user != CGlobal.UserInfo.EmployeeNo && !CGlobal.IsRecruitmentOrAdmin())
                            {
                                result.scomplete = "Y";
                            }
                        }
                        else
                        {
                            if (_getDataEva.req_incharge_Approve_user != CGlobal.UserInfo.EmployeeNo && !CGlobal.IsRecruitmentOrAdmin())
                            {
                                result.scomplete = "Y";
                            }
                        }
                        var _getData = _getDataEva.TM_PR_Candidate_Mapping;
                        result.group_name = _getData.PersonnelRequest.TM_Divisions.division_name_en + "";
                        result.trainee_name = _getData.TM_Candidates.first_name_en + " " + _getData.TM_Candidates.last_name_en;
                        result.position = _getData.PersonnelRequest.TM_Position.position_name_en + "";
                        result.no_eva = _getData.PersonnelRequest.no_of_eva + "";
                        result.nick_name = _getData.TM_Candidates.candidate_NickName + "";
                        result.trainee_no = _getData.TM_Candidates.candidate_TraineeNumber + "";
                        result.target_start = _getData.trainee_start.HasValue ? _getData.trainee_start.Value.DateTimebyCulture() : "";
                        result.target_end = _getData.trainee_end.HasValue ? _getData.trainee_end.Value.DateTimebyCulture() : "";
                        result.status = _getDataEva.submit_status + "";

                        if (_getDataEva.approve_type + "" == "M" && _getDataEva.mgr_Approve_status == "Y")
                        {
                            result.scomplete = "Y";
                        }
                        if (_getDataEva.approve_type + "" == "I" && _getDataEva.incharge_Approve_status == "Y")
                        {
                            result.scomplete = "Y";
                        }
                        //Add Code New Question 
                        var _getActEva = _TM_Trainee_Eva_AnswerService.findByTraineeEvaId(nId); // for get info of eva form 

                        var chkFormId = _getActEva.TM_Evaluation_Question.TM_Evaluation_Form.Id;
                        //Add Code New Question 
                        var _getEva = _TM_Evaluation_FormService.Find(chkFormId);
                        if (_getEva.Id == 7)
                        {
                            //Question
                            var _getlstQuestion = _TM_Evaluation_QuestionService.FindQuestion(_getEva.Id); /*for get a question by eva form id*/
                            result.objtifform = new vObject_of_TrainEva();
                            result.objtifform.TIF_no = _getEva.Id + "";
                            result.objtifform.lstQuestion = (from lstQ in _getlstQuestion
                                                             select new vEva_list_question
                                                             {
                                                                 id = lstQ.Id + "",
                                                                 question = lstQ.question,
                                                                 sgroup = lstQ.header,
                                                                 nSeq = lstQ.seq,
                                                                 remark = "",
                                                                 rating = "",
                                                                 sDisable = result.scomplete,
                                                             }).ToList();
                        }
                        else
                        {
                            var _getlstQuestion = _TM_Evaluation_QuestionService.FindQuestion(_getEva.Id); /*for get a question by eva form id*/
                            result.objtifform = new vObject_of_TrainEva();
                            result.objtifform.TIF_no = _getEva.Id + "";
                            result.objtifform.lstQuestion = (from lstQ in _getlstQuestion
                                                             select new vEva_list_question
                                                             {
                                                                 id = lstQ.Id + "",
                                                                 question = lstQ.question,
                                                                 sgroup = lstQ.header,
                                                                 nSeq = lstQ.seq,
                                                                 remark = "",
                                                                 rating = "",
                                                                 sDisable = result.scomplete,
                                                             }).ToList();
                        }

                        if (_getEva != null && _getEva.TM_Evaluation_Question != null && _getEva.TM_Evaluation_Question.Any(a => a.active_status == "Y"))
                        {
                            result.objtifform = new vObject_of_TrainEva();
                            result.objtifform.TIF_no = _getEva.Id + "";
                            result.objtifform.lstQuestion = (from lstQ in _getEva.TM_Evaluation_Question
                                                             select new vEva_list_question
                                                             {
                                                                 id = lstQ.Id + "",
                                                                 question = lstQ.question,
                                                                 sgroup = lstQ.header,
                                                                 nSeq = lstQ.seq,
                                                                 remark = "",
                                                                 rating = "",
                                                                 sDisable = result.scomplete,
                                                             }).ToList();

                            //Add Code 15/9/2020 for Edit Rating Eva
                            if (_getEva.Id >= 7)
                            {
                                result.objtifform.lstRating = _TM_Eva_RatingService.GetDataForSelectAll().Where(w=> w.TM_Evaluation_Form.Id == _getEva.Id).Select(s => new lstDataSelect
                                {
                                    value = s.Id + "",
                                    text = s.point + " : " + s.rating_name_en + "",
                                    detail = s.rating_description + "",

                                }).ToList();
                                //Get Hiring Rating New
                                result.hiringratingname = _getDataEva.TM_Trainee_HiringRating_Id + "";
                            }
                            else if (_getEva.Id < 7)
                            {
                                result.objtifform.lstRating = _TM_Eva_RatingService.GetDataForSelectAll().Where(w=> w.TM_Evaluation_Form.Id == _getEva.Id).Select(s => new lstDataSelect
                                {
                                    value = s.Id + "",
                                    text = s.point + " : " + s.rating_name_en + "",
                                    detail = s.rating_description + "",

                                }).ToList();
                            }
                            result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                        }
                        result.IdEncryptBack = HCMFunc.Encrypt(_getDataEva.TM_PR_Candidate_Mapping.Id + "");
                        result.comment = _getDataEva.trainee_learned;
                        result.comment2 = _getDataEva.trainee_done_well;
                        result.comment3 = _getDataEva.trainee_developmental;
                        result.type_approve = _getDataEva.approve_type + "" == "M" ? "Performance Manager" : "In-Charge / Engagement Manager";
                        result.confidentiality_agreement = _getDataEva.confidentiality_agreement;
                        result.hiring_status = _getDataEva.hiring_status + "" == "Y" ? "Yes" : "No";
                        result.incharge_comments = _getDataEva.incharge_comments;


                        if (!string.IsNullOrEmpty(_getDataEva.req_mgr_Approve_user))
                        {
                            var Getmrg = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getDataEva.req_mgr_Approve_user).FirstOrDefault();
                            if (Getmrg != null)
                            {
                                result.mgr_user_no = _getDataEva.req_mgr_Approve_user;
                                result.mgr_user_name = Getmrg.EmpFullName;
                                result.mgr_unit_name = Getmrg.UnitGroup;
                                result.mgr_user_rank = Getmrg.Rank;
                            }
                        }
                        if (!string.IsNullOrEmpty(_getDataEva.req_incharge_Approve_user))
                        {
                            var Getmrg = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getDataEva.req_incharge_Approve_user).FirstOrDefault();
                            if (Getmrg != null)
                            {
                                result.ic_user_no = _getDataEva.req_incharge_Approve_user;
                                result.ic_user_name = Getmrg.EmpFullName;
                                result.ic_unit_name = Getmrg.UnitGroup;
                                result.ic_user_rank = Getmrg.Rank;
                            }
                        }

                        if (_getDataEva.TM_Trainee_Eva_Answer != null && _getDataEva.TM_Trainee_Eva_Answer.Any(a => a.active_status == "Y"))
                        {
                            result.objtifform.lstQuestion.ForEach(ed =>
                            {
                                var GetAns = _getDataEva.TM_Trainee_Eva_Answer.Where(w => w.TM_Evaluation_Question.Id + "" == ed.id).FirstOrDefault();
                                if (GetAns != null)
                                {
                                    ed.rating = GetAns.trainee_rating != null ? GetAns.trainee_rating.point + " : " + GetAns.trainee_rating.rating_name_en + "" : "";
                                    ed.approve_rating = GetAns.inchage_rating != null ? GetAns.inchage_rating.point + " : " + GetAns.inchage_rating.rating_name_en + "" : "";
                                    ed.remark = GetAns.inchage_comment;
                                }
                            });
                        }
                        if (_getEva.Id >= 7)
                        {
                            return View("TraineeEvaReportViewNew", result);
                        }
                        else if (_getEva.Id < 7)
                        {
                            return View("TraineeEvaReportView", result);
                        }
                    }
                    else
                    {
                        return RedirectToAction("TraineeEvaList", "TraineeEva");
                    }
                }
                else
                {
                    return RedirectToAction("TraineeEvaList", "TraineeEva");
                }
            }
            else
            {
                return RedirectToAction("TraineeEvaList", "TraineeEva");
            }
            return RedirectToAction("TraineeEvaList", "TraineeEva");
            //return View(result);

        }

        #region Ajax Function
        [HttpPost]
        public ActionResult LoadTraineeEvaList(CSearchTraineeEva SearchItem)
        {
            vTraineeEva_Return result = new vTraineeEva_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            bool isAdmin = CGlobal.IsRecruitmentOrAdmin();
            List<vTraineeEva_obj> lstData_resutl = new List<vTraineeEva_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => 1 == 1);
            string[] UserNo = null;
            if (!string.IsNullOrEmpty(SearchItem.eva_name))
            {
                UserNo = sQuery.Where(w =>
                       (
                       (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                       ).Contains((SearchItem.eva_name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                        ).Select(s => ((s.Employeeno + "").Trim()).ToLower()).ToArray();
            }
            string[] UserNoIC = null;
            if (!string.IsNullOrEmpty(SearchItem.eva_ic_name))
            {
                UserNoIC = dbHr.Employee.Where(w =>
                       (
                       (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                       ).Contains((SearchItem.eva_ic_name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                        ).Select(s => ((s.Employeeno + "").Trim()).ToLower()).ToArray();
            }
            var lstData = _TM_Trainee_EvaService.GetEvaForApprove(
                                                          SearchItem.group_code,
                                                          aDivisionPermission,
                                                          UserNo,
                                                          UserNoIC,
                                                          CGlobal.UserInfo.EmployeeNo,
                                                          SearchItem.name,
                                                          isAdmin).ToList();
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
                string[] aApproveUser = lstData.Select(s => (s.approve_type + "" == "M" ? s.req_mgr_Approve_user : s.req_incharge_Approve_user)).ToArray();
                List<string> lstUser = new List<string>();
                lstUser.AddRange(lstData.Select(s => s.req_incharge_Approve_user).ToList());
                lstUser.AddRange(lstData.Select(s => s.req_mgr_Approve_user).ToList());

                var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();
                var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                lstData_resutl = (from lstAD in lstData.AsEnumerable()
                                  select new vTraineeEva_obj
                                  {
                                      pm_name = _getUser.Where(w => w.EmpNo == lstAD.req_mgr_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                      incharge_name = _getUser.Where(w => w.EmpNo == lstAD.req_incharge_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                      key_type = lstAD.approve_type + "" != "" ? (lstAD.approve_type + "" == "M" ? "PM’s Name" : "In-Charge") : "",
                                      trainee_name = lstAD.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                      eva_status = lstAD.TM_TraineeEva_Status != null ? lstAD.TM_TraineeEva_Status.eva_status_name_en : "",
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      sgroup = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                      bu_approve = lstAD.approve_type == "M" ? (lstAD.mgr_Approve_date.HasValue ? lstAD.mgr_Approve_date.Value.DateTimebyCulture() : "") : (lstAD.incharge_Approve_date.HasValue ? lstAD.incharge_Approve_date.Value.DateTimebyCulture() : ""),
                                      hr_ack = lstAD.acknowledge_date.HasValue ? lstAD.acknowledge_date.Value.DateTimebyCulture() : "",

                                      target_start = lstAD.TM_PR_Candidate_Mapping.trainee_start.HasValue ? lstAD.TM_PR_Candidate_Mapping.trainee_start.Value.DateTimebyCulture() : "",
                                      target_end = lstAD.TM_PR_Candidate_Mapping.trainee_end.HasValue ? lstAD.TM_PR_Candidate_Mapping.trainee_end.Value.DateTimebyCulture() : "",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;
            return Json(new { result });
        }
        [HttpPost]
        public ActionResult SaveEditTraineeEva(vTraineeEva_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTraineeEva_Return result = new vTraineeEva_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {


                    var _getData = _TM_Trainee_EvaService.Find(nId);
                    if (_getData != null)
                    {
                        //id null = first time Save data
                        if ((_getData.approve_type == "M" && _getData.req_mgr_Approve_user == CGlobal.UserInfo.EmployeeNo)
                            || (_getData.approve_type == "I" && _getData.req_incharge_Approve_user == CGlobal.UserInfo.EmployeeNo)
                            || CGlobal.UserIsAdmin())
                        {
                            var _getMap = _TM_PR_Candidate_MappingService.Find(_getData.TM_PR_Candidate_Mapping.Id);
                            int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                            var _getTifForm = _TM_Evaluation_FormService.Find(nTFId);
                            if (_getTifForm != null && _getTifForm.TM_Evaluation_Question != null && _getTifForm.TM_Evaluation_Question.Any())
                            {
                                string[] aTIFQID = _getTifForm.TM_Evaluation_Question.Select(s => s.Id + "").ToArray();
                                List<TM_Trainee_Eva_Answer> lstAns = new List<TM_Trainee_Eva_Answer>();
                                if (ItemData.objtifform.lstQuestion != null)
                                {
                                    lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                              from lstQ in _getTifForm.TM_Evaluation_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new HumanCapitalManagement.Models.TIFForm.TM_Evaluation_Question())
                                              select new TM_Trainee_Eva_Answer
                                              {
                                                  update_user = CGlobal.UserInfo.UserId,
                                                  update_date = dNow,
                                                  create_user = CGlobal.UserInfo.UserId,
                                                  create_date = dNow,
                                                  inchage_comment = lstA.remark + "",
                                                  active_status = "Y",
                                                  TM_Evaluation_Question = lstQ != null ? lstQ : null,
                                                  //  trainee_rating = _TM_Eva_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                                  inchage_rating = _TM_Eva_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.approve_rating)),
                                                  TM_Trainee_Eva = _getData,
                                              }).ToList();
                                }
                                lstAns = lstAns.Where(w => w.TM_Evaluation_Question != null).ToList();
                                _getData.TM_PR_Candidate_Mapping = _getMap;
                                _getData.incharge_comments = ItemData.incharge_comments;
                                _getData.confidentiality_agreement = ItemData.confidentiality_agreement;
                                _getData.hiring_status = ItemData.hiring_status;
                                if(!String.IsNullOrEmpty(ItemData.hiringratingname) && string.IsNullOrEmpty(ItemData.hiring_status))
                                _getData.TM_Trainee_HiringRating_Id =SystemFunction.GetIntNullToZero(ItemData.hiringratingname);

                                _getData.incharge_update_date = dNow;
                                _getData.incharge_update_user = CGlobal.UserInfo.EmployeeNo;
                                _getData.TM_TraineeEva_Status = _TM_TraineeEva_StatusService.FindForSave((int)StatusTraineeEvaluation.HR_Approve);
                                if (_getData.approve_type == "M")
                                {
                                    _getData.mgr_Approve_user = CGlobal.UserInfo.EmployeeNo;
                                    _getData.mgr_Approve_date = dNow;

                                    _getData.mgr_Approve_status = "Y";
                                }
                                else
                                {
                                    _getData.incharge_Approve_user = CGlobal.UserInfo.EmployeeNo;
                                    _getData.incharge_Approve_date = dNow;
                                    _getData.incharge_Approve_status = "Y";
                                }


                                var sComplect = _TM_Trainee_EvaService.UpdateEvaApprove(ref _getData);
                                if (sComplect > 0)
                                {
                                    if (_getData != null)
                                    {
                                        if (_TM_Trainee_Eva_AnswerService.UpdateAnswerApprove(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow) > 0)
                                        {
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
                                        result.Status = SystemFunction.process_Success;
                                    }
                                    string sError = "";
                                    string mail_to_log = "";
                                    var Mail1 = _MailContentService.Find(10);
                                    if (_getData != null)
                                    {
                                        var bSuss = SendEvaApprove(_getData, Mail1, ref sError, ref mail_to_log);
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
                                result.Msg = "Error, Evaluation Form Not Found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Don't have Permission.";
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
        public ActionResult SaveDraftTraineeEva(vTraineeEva_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTraineeEva_Return result = new vTraineeEva_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {


                    var _getData = _TM_Trainee_EvaService.Find(nId);
                    if (_getData != null)
                    {
                        //id null = first time Save data
                        if ((_getData.approve_type == "M" && _getData.req_mgr_Approve_user == CGlobal.UserInfo.EmployeeNo)
                            || (_getData.approve_type == "I" && _getData.req_incharge_Approve_user == CGlobal.UserInfo.EmployeeNo)
                            || CGlobal.UserIsAdmin())
                        {
                            var _getMap = _TM_PR_Candidate_MappingService.Find(_getData.TM_PR_Candidate_Mapping.Id);
                            int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                            var _getTifForm = _TM_Evaluation_FormService.Find(nTFId);
                            if (_getTifForm != null && _getTifForm.TM_Evaluation_Question != null && _getTifForm.TM_Evaluation_Question.Any())
                            {
                                string[] aTIFQID = _getTifForm.TM_Evaluation_Question.Select(s => s.Id + "").ToArray();
                                List<TM_Trainee_Eva_Answer> lstAns = new List<TM_Trainee_Eva_Answer>();
                                if (ItemData.objtifform.lstQuestion != null)
                                {
                                    lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                              from lstQ in _getTifForm.TM_Evaluation_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new HumanCapitalManagement.Models.TIFForm.TM_Evaluation_Question())
                                              select new TM_Trainee_Eva_Answer
                                              {
                                                  update_user = CGlobal.UserInfo.UserId,
                                                  update_date = dNow,
                                                  create_user = CGlobal.UserInfo.UserId,
                                                  create_date = dNow,
                                                  inchage_comment = lstA.remark + "",
                                                  active_status = "Y",
                                                  TM_Evaluation_Question = lstQ != null ? lstQ : null,
                                                  //  trainee_rating = _TM_Eva_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                                  inchage_rating = _TM_Eva_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.approve_rating)),
                                                  TM_Trainee_Eva = _getData,
                                              }).ToList();
                                }
                                lstAns = lstAns.Where(w => w.TM_Evaluation_Question != null).ToList();
                                _getData.TM_PR_Candidate_Mapping = _getMap;
                                _getData.incharge_comments = ItemData.incharge_comments;
                                _getData.confidentiality_agreement = ItemData.confidentiality_agreement;
                                _getData.hiring_status = ItemData.hiring_status != null ? ItemData.hiring_status : null;
                                _getData.TM_Trainee_HiringRating_Id = ItemData.hiringratingname != null && ItemData.hiringratingname != "" ?  Convert.ToInt32(ItemData.hiringratingname) :  default(int?);
                                _getData.incharge_update_date = dNow;
                                _getData.incharge_update_user = CGlobal.UserInfo.EmployeeNo;

                                var sComplect = _TM_Trainee_EvaService.UpdateEvaApprove(ref _getData);
                                if (sComplect > 0)
                                {
                                    if (_getData != null)
                                    {
                                        if (_TM_Trainee_Eva_AnswerService.UpdateAnswerApprove(lstAns, _getData.Id, CGlobal.UserInfo.UserId, dNow) > 0)
                                        {
                                            result.Status = SystemFunction.process_Success;
                                        }
                                        else
                                        {
                                            result.Status = SystemFunction.process_Success;
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
                                    result.Msg = "Error, please try again.";
                                }
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Evaluation Form Not Found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Don't have Permission.";
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
        public ActionResult RollBackTIFForm(vTraineeEva_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (ItemData != null)
            {
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    bool isAdmin = CGlobal.IsRecruitmentOrAdmin();
                    var _getData = _TM_Trainee_EvaService.Find(nId);
                    if (_getData != null)
                    {
                        if ((_getData.approve_type == "M" && _getData.req_mgr_Approve_user == CGlobal.UserInfo.EmployeeNo)
                            || (_getData.approve_type == "I" && _getData.req_incharge_Approve_user == CGlobal.UserInfo.EmployeeNo)
                            || CGlobal.IsRecruitmentOrAdmin())
                        {
                            _getData.TM_TraineeEva_Status = _TM_TraineeEva_StatusService.FindForSave((int)StatusTraineeEvaluation.Revise);
                            _TM_Trainee_EvaService.UpdateEva(ref _getData);
                            var ApproveComplect = _TM_Trainee_EvaService.RollBackStatus(_getData.Id, CGlobal.UserInfo.UserId, dNow);
                            if (ApproveComplect > 0)
                            {
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
                            result.Msg = "Error, Not permission.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, TIF Form Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select Approve Status.";
            }

            return Json(new
            {
                result
            });
        }
        #endregion

        #region Ajax Function For HR
        [HttpPost]
        public ActionResult LoadAcknowTraineeEvaList(CSearchTraineeEva SearchItem)
        {
            vTraineeEva_Return result = new vTraineeEva_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            bool isAdmin = CGlobal.IsRecruitmentOrAdmin();
            List<vTraineeEva_obj> lstData_resutl = new List<vTraineeEva_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            DateTime? dStart = null, dStartTo = null, dEnd = null, dEndTo = null;
            if (!string.IsNullOrEmpty(SearchItem.start_date))
            {
                dStart = SystemFunction.ConvertStringToDateTime(SearchItem.start_date, "");
            }
            if (!string.IsNullOrEmpty(SearchItem.start_to))
            {
                dStartTo = SystemFunction.ConvertStringToDateTime(SearchItem.start_to, "");
            }
            if (!string.IsNullOrEmpty(SearchItem.end_date))
            {
                dEnd = SystemFunction.ConvertStringToDateTime(SearchItem.end_date, "");
            }
            if (!string.IsNullOrEmpty(SearchItem.end_to))
            {
                dEndTo = SystemFunction.ConvertStringToDateTime(SearchItem.end_to, "");
            }
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => 1 == 1);
            string[] UserNo = null;
            if (!string.IsNullOrEmpty(SearchItem.eva_name))
            {
                UserNo = sQuery.Where(w =>
                       (
                       (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                       ).Contains((SearchItem.eva_name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                        ).Select(s => ((s.Employeeno + "").Trim()).ToLower()).ToArray();
            }
            string[] UserNoIC = null;
            if (!string.IsNullOrEmpty(SearchItem.eva_ic_name))
            {
                UserNoIC = dbHr.Employee.Where(w =>
                       (
                       (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                       ).Contains((SearchItem.eva_ic_name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                        ).Select(s => ((s.Employeeno + "").Trim()).ToLower()).ToArray();
            }
            var lstData = _TM_Trainee_EvaService.GetEvaForAcknow(SearchItem.group_code,
                  aDivisionPermission,
                   UserNo,
                  UserNoIC,
                  CGlobal.UserInfo.EmployeeNo,
                  SearchItem.name,
                  dStart,
                  dStartTo,
                  dEnd,
                  dEndTo,
                  isAdmin).ToList();
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
                string[] aApproveUser = lstData.Select(s => (s.approve_type + "" == "M" ? s.req_mgr_Approve_user : s.req_incharge_Approve_user)).ToArray();
                List<string> lstUser = new List<string>();
                lstUser.AddRange(lstData.Select(s => s.req_incharge_Approve_user).ToList());
                lstUser.AddRange(lstData.Select(s => s.req_mgr_Approve_user).ToList());

                var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();
                var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                string sUrl = Url.Action("AcKnowledgeTraineeEvaEdit", "TraineeEva", null, Request.Url.Scheme);
                lstData_resutl = (from lstAD in lstData.AsEnumerable()
                                  select new vTraineeEva_obj
                                  {
                                      IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                      Id = lstAD.Id,
                                      pm_name = _getUser.Where(w => w.EmpNo == lstAD.req_mgr_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                      incharge_name = _getUser.Where(w => w.EmpNo == lstAD.req_incharge_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                      key_type = lstAD.approve_type + "" != "" ? (lstAD.approve_type + "" == "M" ? "PM’s Name" : "In-Charge") : "",
                                      trainee_name = @"<a target=""_blank""  href=""" + sUrl + "?qryStr=" + HCMFunc.Encrypt(lstAD.Id + "") + @""">" + lstAD.TM_PR_Candidate_Mapping.TM_Candidates.candidate_full_name() + @"</a>",
                                      // refno = @"<a target=""_blank""  href=""" + sUrl + "?qryStr=" + HCMFunc.Encrypt(lstAD.Id + "") + @""">" + lstAD.RefNo + @"</a>",
                                      eva_status = lstAD.TM_TraineeEva_Status != null ? lstAD.TM_TraineeEva_Status.eva_status_name_en : "",
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      sgroup = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                      bu_approve = lstAD.approve_type == "M" ? (lstAD.mgr_Approve_date.HasValue ? lstAD.mgr_Approve_date.Value.DateTimebyCulture() : "") : (lstAD.incharge_Approve_date.HasValue ? lstAD.incharge_Approve_date.Value.DateTimebyCulture() : ""),
                                      hr_ack = lstAD.acknowledge_date.HasValue ? lstAD.acknowledge_date.Value.DateTimebyCulture() : "",
                                      target_start = lstAD.TM_PR_Candidate_Mapping.trainee_start.HasValue ? lstAD.TM_PR_Candidate_Mapping.trainee_start.Value.DateTimebyCulture() : "",
                                      target_end = lstAD.TM_PR_Candidate_Mapping.trainee_end.HasValue ? lstAD.TM_PR_Candidate_Mapping.trainee_end.Value.DateTimebyCulture() : "",

                                      hiring_status = String.IsNullOrEmpty(lstAD.hiring_status + "") ? lstAD.TM_Trainee_HiringRating == null ?  "" : lstAD.TM_Trainee_HiringRating.Trainee_HiringRating_name_en : string.IsNullOrEmpty(lstAD.hiring_status + "")? "" : lstAD.hiring_status + "" == "Y" ? "Yes" : "No",

                                      evaluator_name = lstAD.approve_type + "" == "M" ? _getUser.Where(w => w.EmpNo == lstAD.req_mgr_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + ""
                                      : _getUser.Where(w => w.EmpNo == lstAD.req_incharge_Approve_user).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;
            return Json(new { result });
        }

        [HttpPost]
        public ActionResult AcKnowledgeTraineeEvalst(vTraineeEva_Return ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vTraineeEva_Return result = new vTraineeEva_Return();
            List<vTraineeEva_lst> lstError = new List<vTraineeEva_lst>();
            bool isAdmin = CGlobal.IsRecruitmentOrAdmin();
            if (ItemData != null && ItemData.lstData != null && ItemData.lstData.Any() && isAdmin)
            {
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                DateTime dNow = DateTime.Now;

                foreach (var item in ItemData.lstData)
                {
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(item.IdEncrypt + ""));
                    if (nId != 0)
                    {
                        var _GetData = _TM_Trainee_EvaService.Find(nId);
                        if (_GetData != null)
                        {
                            var _GetMapping = _TM_PR_Candidate_MappingService.Find(_GetData.TM_PR_Candidate_Mapping.Id);
                            if (_GetMapping != null)
                            {
                                _GetData.hr_acknowledge = "Y";
                                _GetData.acknowledge_date = dNow;
                                _GetData.acknowledge_user = CGlobal.UserInfo.EmployeeNo;
                                _GetData.TM_PR_Candidate_Mapping = _GetMapping;
                                _GetData.TM_TraineeEva_Status = _TM_TraineeEva_StatusService.FindForSave((int)StatusTraineeEvaluation.Complect);
                                var sComplect = _TM_Trainee_EvaService.AcKnowledgeEva(ref _GetData);
                                if (sComplect > 0)
                                {



                                }
                                else
                                {
                                    //result.Status = SystemFunction.process_Failed;
                                    //result.Msg = "Error, Approval more than 2.";
                                    lstError.Add(new vTraineeEva_lst
                                    {
                                        name_en = item.Id + "",
                                        status = "Y",
                                        msg = "Error, Approval more than 2.",
                                    });
                                }
                            }
                        }
                    }
                }
                result.Status = SystemFunction.process_Success;
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select Approve Status.";
            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult RollEvaToBu(vTraineeEva_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (ItemData != null)
            {
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {

                    var _getData = _TM_Trainee_EvaService.Find(nId);
                    if (_getData != null)
                    {
                        if (CGlobal.IsRecruitmentOrAdmin())
                        {
                            var _GetMapping = _TM_PR_Candidate_MappingService.Find(_getData.TM_PR_Candidate_Mapping.Id);
                            if (_GetMapping != null)
                            {
                                _getData.TM_TraineeEva_Status = _TM_TraineeEva_StatusService.FindForSave((int)StatusTraineeEvaluation.BuRevise);
                                _getData.TM_PR_Candidate_Mapping = _GetMapping;
                                var ApproveComplect = _TM_Trainee_EvaService.RollBackToBu(_getData);
                                if (ApproveComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    string sError = "";
                                    string mail_to_log = "";
                                    var Mail1 = _MailContentService.Find(11);
                                    if (_getData != null)
                                    {
                                        var bSuss = SendEvaRollback(_getData, Mail1, ref sError, ref mail_to_log, ItemData.sreject);
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
                                result.Msg = "Error, Data not found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Not permission.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Form Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select Approve Status.";
            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult AcKnowledgeEva(vTraineeEva_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (ItemData != null)
            {
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {

                    var _getData = _TM_Trainee_EvaService.Find(nId);
                    if (_getData != null)
                    {
                        if (CGlobal.IsRecruitmentOrAdmin())
                        {
                            var _GetMapping = _TM_PR_Candidate_MappingService.Find(_getData.TM_PR_Candidate_Mapping.Id);
                            if (_GetMapping != null)
                            {
                                _getData.TM_TraineeEva_Status = _TM_TraineeEva_StatusService.FindForSave((int)StatusTraineeEvaluation.Complect);
                                _getData.TM_PR_Candidate_Mapping = _GetMapping;
                                _getData.hr_acknowledge = "Y";
                                _getData.acknowledge_date = dNow;
                                _getData.acknowledge_user = CGlobal.UserInfo.EmployeeNo;
                                var ApproveComplect = _TM_Trainee_EvaService.AcKnowledgeEva(ref _getData);
                                if (ApproveComplect > 0)
                                {
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
                                result.Msg = "Error, Data not found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Not permission.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Evaluation Form Not Found.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select Approve Status.";
            }

            return Json(new
            {
                result
            });
        }
        #endregion

        #region Ajax For Report

        [HttpPost]
        public ActionResult LoadReportTraineeEvaList(CSearchTraineeEva SearchItem)
        {
            vTraineeEva_Return result = new vTraineeEva_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            bool isAdmin = CGlobal.IsRecruitmentOrAdmin();
            List<vTraineeEva_obj> lstData_resutl = new List<vTraineeEva_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            int nEvaStatus = SystemFunction.GetIntNullToZero(SearchItem.eva_status);
            DateTime? dStart = null, dStartTo = null, dEnd = null, dEndTo = null;
            if (!string.IsNullOrEmpty(SearchItem.start_date))
            {
                dStart = SystemFunction.ConvertStringToDateTime(SearchItem.start_date, "");
            }
            if (!string.IsNullOrEmpty(SearchItem.start_to))
            {
                dStartTo = SystemFunction.ConvertStringToDateTime(SearchItem.start_to, "");
            }
            if (!string.IsNullOrEmpty(SearchItem.end_date))
            {
                dEnd = SystemFunction.ConvertStringToDateTime(SearchItem.end_date, "");
            }
            if (!string.IsNullOrEmpty(SearchItem.end_to))
            {
                dEndTo = SystemFunction.ConvertStringToDateTime(SearchItem.end_to, "");
            }
            IQueryable<Employee> sQuery = dbHr.Employee.Where(w => 1 == 1);
            string[] UserNo = null;
            if (!string.IsNullOrEmpty(SearchItem.eva_name))
            {
                UserNo = sQuery.Where(w =>
                       (
                       (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                       ).Contains((SearchItem.eva_name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                        ).Select(s => ((s.Employeeno + "").Trim()).ToLower()).ToArray();
            }
            string[] UserNoIC = null;
            if (!string.IsNullOrEmpty(SearchItem.eva_ic_name))
            {
                UserNoIC = dbHr.Employee.Where(w =>
                       (
                       (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower())
                       ).Contains((SearchItem.eva_ic_name + "").Trim().ToLower() ?? (w.Employeeno + "").Trim().ToLower() +
                       (w.Employeename + "").Trim().ToLower() +
                       (w.Employeesurname + "").Trim().ToLower() +
                       (w.EmpThaiName + "").Trim().ToLower() +
                       (w.EmpThaiSurName + "").Trim().ToLower() +
                        ((w.Employeename + "").Trim().ToLower() + " " + (w.Employeesurname + "").Trim().ToLower()) +
                        ((w.EmpThaiName + "").Trim().ToLower() + " " + (w.EmpThaiSurName + "").Trim().ToLower()))
                        ).Select(s => ((s.Employeeno + "").Trim()).ToLower()).ToArray();
            }
            var lstData = _TM_Trainee_EvaService.GetEvaForReport(SearchItem.group_code,
                            aDivisionPermission,
                            UserNo,
                            UserNoIC,
                            CGlobal.UserInfo.EmployeeNo,
                            SearchItem.name,
                            nEvaStatus,
                            dStart,
                            dStartTo,
                            dEnd,
                            dEndTo,
                            isAdmin).ToList();
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            rpvEva_Session objSession = new rpvEva_Session();
            string BackUrl = Uri.EscapeDataString(qryStr);
            if (lstData.Any())
            {
                string[] aApproveUser = lstData.Select(s => (s.approve_type + "" == "M" ? s.req_mgr_Approve_user : s.req_incharge_Approve_user)).ToArray();
                List<string> lstUser = new List<string>();
                lstUser.AddRange(lstData.Select(s => s.req_incharge_Approve_user).ToList());
                lstUser.AddRange(lstData.Select(s => s.req_mgr_Approve_user).ToList());
                lstUser.AddRange(lstData.Select(s => s.acknowledge_user).ToList());
                var _getUser = dbHr.AllInfo_WS.Where(w => lstUser.Contains(w.EmpNo)).ToList();
                try
                {
                    lstData_resutl = (from lstAD in lstData.AsEnumerable()
                                      from lstMGR in _getUser.Where(w => w.EmpNo == lstAD.req_mgr_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstIC in _getUser.Where(w => w.EmpNo == lstAD.req_incharge_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstHR in _getUser.Where(w => w.EmpNo == lstAD.acknowledge_user).DefaultIfEmpty(new AllInfo_WS())
                                      select new vTraineeEva_obj
                                      {
                                          IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                          Id = lstAD.Id,
                                          hr_name = lstHR.EmpFullName + "",
                                          pm_name = lstMGR.EmpFullName + "",
                                          incharge_name = lstIC.EmpFullName + "",
                                          key_type = lstAD.approve_type + "" != "" ? (lstAD.approve_type + "" == "M" ? "PM’s Name" : "In-Charge") : "",
                                          trainee_name = lstAD.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                          eva_status = lstAD.TM_TraineeEva_Status != null ? lstAD.TM_TraineeEva_Status.eva_status_name_en : "",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          sgroup = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          bu_approve = lstAD.approve_type == "M" ? (lstAD.mgr_Approve_date.HasValue ? lstAD.mgr_Approve_date.Value.DateTimebyCulture() : "") : (lstAD.incharge_Approve_date.HasValue ? lstAD.incharge_Approve_date.Value.DateTimebyCulture() : ""),
                                          hr_ack = lstAD.acknowledge_date.HasValue ? lstAD.acknowledge_date.Value.DateTimebyCulture() : "",
                                          target_start = lstAD.TM_PR_Candidate_Mapping.trainee_start.HasValue ? lstAD.TM_PR_Candidate_Mapping.trainee_start.Value.DateTimebyCulture() : "",
                                          target_end = lstAD.TM_PR_Candidate_Mapping.trainee_end.HasValue ? lstAD.TM_PR_Candidate_Mapping.trainee_end.Value.DateTimebyCulture() : "",
                                          //hiring_status = lstAD.hiring_status + "" == "" ? lstAD.TM_Trainee_HiringRating.Trainee_HiringRating_name_en : (lstAD.hiring_status + "" == "Y" ? "Yes" : "No"),
                                          hiring_status = String.IsNullOrEmpty(lstAD.hiring_status + "") ? lstAD.TM_Trainee_HiringRating == null ? "" : lstAD.TM_Trainee_HiringRating.Trainee_HiringRating_name_en : string.IsNullOrEmpty(lstAD.hiring_status + "") ? "" : lstAD.hiring_status + "" == "Y" ? "Yes" : "No",
                                          evaluator_name = lstAD.approve_type + "" == "M" ? lstMGR.EmpFullName + "" : lstIC.EmpFullName + "",
                                          evaluator_mail = lstAD.approve_type + "" == "M" ? lstMGR.Email + "" : lstIC.Email + "",
                                          pm_no = lstAD.req_mgr_Approve_user + "",
                                          ic_no = lstAD.req_incharge_Approve_user + "",
                                          surname = lstAD.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en + "",
                                          name = lstAD.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + "",
                                          nickname = lstAD.TM_PR_Candidate_Mapping.TM_Candidates.candidate_NickName + "",

                                      }).ToList();
                }
                catch (Exception ex)
                {
                    var error = ex.Message;
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = ex.Message;
                    return Json(new { result });
                }
                result.lstData = lstData_resutl.ToList();
                objSession.lstData = lstData.ToList();
                objSession.lstTraineeTracking = lstData_resutl.ToList();
            }
            Session[SearchItem.session] = objSession;
            result.Status = SystemFunction.process_Success;
            return Json(new { result });
        }
        #endregion
    }
}