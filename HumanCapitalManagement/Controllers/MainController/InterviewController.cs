using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TIFForm;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.MainController
{
    public class InterviewController : BaseController
    {
        private PersonnelRequestService _PersonnelRequestService;
        private TM_Candidate_Status_CycleService _TM_Candidate_Status_CycleService;
        private TM_TIF_FormService _TM_TIF_FormService;
        private TM_TIF_RatingService _TM_TIF_RatingService;
        private TM_TIF_StatusService _TM_TIF_StatusService;
        private TM_Candidate_TIFService _TM_Candidate_TIFService;
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private TM_Candidate_TIF_AnswerService _TM_Candidate_TIF_AnswerService;
        private TM_Candidate_TIF_ApprovService _TM_Candidate_TIF_ApprovService;
        private TM_Mass_TIF_FormService _TM_Mass_TIF_FormService;
        private TM_Mass_Auditing_QuestionService _TM_Mass_Auditing_QuestionService;
        private TM_Mass_ScoringService _TM_Mass_ScoringService;
        private TM_MassTIF_StatusService _TM_MassTIF_StatusService;
        private TM_Candidate_MassTIFService _TM_Candidate_MassTIFService;
        private TM_Candidate_MassTIF_CoreService _TM_Candidate_MassTIF_CoreService;
        private TM_Candidate_MassTIF_Audit_QstService _TM_Candidate_MassTIF_Audit_QstService;
        private TM_Mass_Question_TypeService _TM_Mass_Question_TypeService;
        private TM_Candidate_MassTIF_ApprovService _TM_Candidate_MassTIF_ApprovService;
        private TM_Pool_RankService _TM_Pool_RankService;
        private MailContentService _MailContentService;
        private TM_Additional_InformationService _TM_Additional_InformationService;
        private TM_Additional_QuestionsService _TM_Additional_QuestionsService;
        private TM_Additional_AnswersService _TM_Additional_AnswersService;
        private TM_Candidate_MassTIF_AdditionalService _TM_Candidate_MassTIF_AdditionalService;
        private TM_Candidate_MassTIF_Adnl_AnswerService _TM_Candidate_MassTIF_Adnl_AnswerService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public InterviewController(PersonnelRequestService PersonnelRequestService
            , TM_Candidate_Status_CycleService TM_Candidate_Status_CycleService
            , TM_TIF_FormService TM_TIF_FormService
            , TM_TIF_RatingService TM_TIF_RatingService
            , TM_TIF_StatusService TM_TIF_StatusService
            , TM_Candidate_TIFService TM_Candidate_TIFService
           , TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , TM_Candidate_TIF_AnswerService TM_Candidate_TIF_AnswerService
            , TM_Candidate_TIF_ApprovService TM_Candidate_TIF_ApprovService
            , TM_Mass_TIF_FormService TM_Mass_TIF_FormService
            , TM_Mass_Auditing_QuestionService TM_Mass_Auditing_QuestionService
            , TM_Mass_ScoringService TM_Mass_ScoringService
            , TM_MassTIF_StatusService TM_MassTIF_StatusService
            , TM_Candidate_MassTIFService TM_Candidate_MassTIFService
            , TM_Candidate_MassTIF_CoreService TM_Candidate_MassTIF_CoreService
            , TM_Candidate_MassTIF_Audit_QstService TM_Candidate_MassTIF_Audit_QstService
            , TM_Mass_Question_TypeService TM_Mass_Question_TypeService
            , TM_Candidate_MassTIF_ApprovService TM_Candidate_MassTIF_ApprovService
            , TM_Pool_RankService TM_Pool_RankService
            , MailContentService MailContentService
            , TM_Additional_InformationService TM_Additional_InformationService
            , TM_Additional_QuestionsService TM_Additional_QuestionsService
            , TM_Additional_AnswersService TM_Additional_AnswersService
            , TM_Candidate_MassTIF_AdditionalService TM_Candidate_MassTIF_AdditionalService
            , TM_Candidate_MassTIF_Adnl_AnswerService TM_Candidate_MassTIF_Adnl_AnswerService
           )
        {

            _PersonnelRequestService = PersonnelRequestService;
            _TM_Candidate_Status_CycleService = TM_Candidate_Status_CycleService;
            _TM_TIF_FormService = TM_TIF_FormService;
            _TM_TIF_RatingService = TM_TIF_RatingService;
            _TM_TIF_StatusService = TM_TIF_StatusService;
            _TM_Candidate_TIFService = TM_Candidate_TIFService;
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TM_Candidate_TIF_AnswerService = TM_Candidate_TIF_AnswerService;
            _TM_Candidate_TIF_ApprovService = TM_Candidate_TIF_ApprovService;
            _TM_Mass_TIF_FormService = TM_Mass_TIF_FormService;
            _TM_Mass_Auditing_QuestionService = TM_Mass_Auditing_QuestionService;
            _TM_Mass_ScoringService = TM_Mass_ScoringService;
            _TM_MassTIF_StatusService = TM_MassTIF_StatusService;
            _TM_Candidate_MassTIFService = TM_Candidate_MassTIFService;
            _TM_Candidate_MassTIF_CoreService = TM_Candidate_MassTIF_CoreService;
            _TM_Candidate_MassTIF_Audit_QstService = TM_Candidate_MassTIF_Audit_QstService;
            _TM_Mass_Question_TypeService = TM_Mass_Question_TypeService;
            _TM_Candidate_MassTIF_ApprovService = TM_Candidate_MassTIF_ApprovService;
            _TM_Pool_RankService = TM_Pool_RankService;
            _MailContentService = MailContentService;
            _TM_Additional_InformationService = TM_Additional_InformationService;
            _TM_Additional_QuestionsService = TM_Additional_QuestionsService;
            _TM_Additional_AnswersService = TM_Additional_AnswersService;
            _TM_Candidate_MassTIF_AdditionalService = TM_Candidate_MassTIF_AdditionalService;
            _TM_Candidate_MassTIF_Adnl_AnswerService = TM_Candidate_MassTIF_Adnl_AnswerService;
        }

        string oct2021 = "10-01-2021";
        public ActionResult InterviewList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            vInterview result = new vInterview();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchInterview SearchItem = (CSearchInterview)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchInterview)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                //var lstData = _TM_Candidate_Status_CycleService.GetCandidateForInterview(
                //                 SearchItem.group_code, aDivisionPermission,
                //                 "Y", isAdmin);

                var lstData = _TM_PR_Candidate_MappingService.GetDataForInterview(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code,
                                 SearchItem.group_code, aDivisionPermission,
                                 "Y", SearchItem.name, isAdmin);

                result.group_code = SearchItem.group_code;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();

                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    int[] nMapID = lstData.Select(s => s.Id).ToArray();
                    var _GetTifform = _TM_Candidate_TIFService.FindByMappingArrayID(nMapID);
                    var _GetMassTifform = _TM_Candidate_MassTIFService.FindByMappingArrayID(nMapID);
                    if (_GetTifform.Any(a => a.submit_status == "Y"))
                    {
                        lstData = lstData.Where(w => !_GetTifform.Where(w2 => w2.submit_status == "Y").Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray().Contains(w.Id)).ToList();
                    }
                    if (_GetMassTifform.Any(a => a.submit_status == "Y"))
                    {
                        lstData = lstData.Where(w => !_GetMassTifform.Where(w2 => w2.submit_status == "Y").Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray().Contains(w.Id)).ToList();
                    }

                    result.lstData = (from lstAD in lstData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id != 10)
                                      from lstTIF in _GetTifform.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_TIF())
                                      from lstMassTIF in _GetMassTifform.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_MassTIF())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vInterview_obj
                                      {
                                          refno = lstAD.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                          name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                          status = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.submit_status != null ? (lstTIF.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting") : (lstMassTIF.submit_status != null ? (lstMassTIF.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting"),
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault().Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                      }).ToList();
                }
            }

            #endregion
            return View(result);
        }
        //public ActionResult InterviewList(string qryStr)
        //{
        //    var sCheck = acCheckLoginAndPermission();
        //    if (sCheck != null)
        //    {
        //        return sCheck;
        //    }
        //    bool isAdmin = CGlobal.UserIsAdmin();
        //    vInterview result = new vInterview();
        //    result.active_status = "Y";
        //    #region main code
        //    if (!string.IsNullOrEmpty(qryStr))
        //    {
        //        CSearchInterview SearchItem = (CSearchInterview)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchInterview)));
        //        string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
        //        //var lstData = _TM_Candidate_Status_CycleService.GetCandidateForInterview(
        //        //                 SearchItem.group_code, aDivisionPermission,
        //        //                 "Y", isAdmin);

        //        var lstData = _TM_PR_Candidate_MappingService.GetDataForInterview(
        //                         SearchItem.group_code, aDivisionPermission,
        //                         "Y", SearchItem.name, isAdmin);
        //        string BackUrl = Uri.EscapeDataString(qryStr);
        //        if (lstData.Any())
        //        {
        //            string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();

        //            var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser);;
        //            int[] nMapID = lstData.Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray();
        //            var _GetTifform = _TM_Candidate_TIFService.FindByMappingArrayID(nMapID);
        //            var _GetMassTifform = _TM_Candidate_MassTIFService.FindByMappingArrayID(nMapID);
        //            if (_GetTifform.Any(a => a.submit_status == "Y"))
        //            {
        //                lstData = lstData.Where(w => !_GetTifform.Where(w2 => w2.submit_status == "Y").Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray().Contains(w.TM_PR_Candidate_Mapping.Id)).ToList();
        //            }
        //            if (_GetMassTifform.Any(a => a.submit_status == "Y"))
        //            {
        //                lstData = lstData.Where(w => !_GetMassTifform.Where(w2 => w2.submit_status == "Y").Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray().Contains(w.TM_PR_Candidate_Mapping.Id)).ToList();
        //            }

        //            result.lstData = (from lstAD in lstData
        //                              from lstTIF in _GetTifform.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.TM_PR_Candidate_Mapping.Id).DefaultIfEmpty(new TM_Candidate_TIF())
        //                              from lstMassTIF in _GetMassTifform.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.TM_PR_Candidate_Mapping.Id).DefaultIfEmpty(new TM_Candidate_MassTIF())
        //                              from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user+"").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
        //                              select new vInterview_obj
        //                              {
        //                                  refno = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
        //                                  group_name = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
        //                                  position = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
        //                                  rank = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
        //                                  name_en = lstAD.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
        //                                  status = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.submit_status != null ? (lstTIF.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting") : (lstMassTIF.submit_status != null ? (lstMassTIF.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting"),
        //                                  create_user = lstAD.create_user,
        //                                  create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
        //                                  update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
        //                                  update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
        //                                  Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
        //                                  pr_type = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
        //                              }).ToList();
        //        }
        //    }

        //    #endregion
        //    return View(result);
        //}

        [HttpPost]
        public ActionResult evaluateScore(EvaluateMassTif answer_list)
        {
            var statusDict = new Dictionary<string, int>();
            statusDict.Add("i", 1);
            statusDict.Add("n", 2);
            statusDict.Add("l", 3);
            statusDict.Add("f", 4);
            List<int> core_answers = answer_list.Core_answers;
            List<int> audit_answers = answer_list.Audit_answers;
            string status = _TM_TIF_FormService.InternEva(core_answers);
            status = _TM_TIF_FormService.auditQEva(audit_answers, status);
            return Json(new { status = statusDict[status], ok = true });
        }

        public ActionResult InterviewEdit(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            var formId = default(int);
            var MassFormId = default(int);
            vInterview_obj_save result = new vInterview_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Candidate_Status_CycleService.Find(nId);
                    if (_getData != null)
                    {
                        var _getRequest = _getData.TM_PR_Candidate_Mapping.PersonnelRequest;
                        if (_getRequest != null)
                        {

                            result.IdEncrypt = qryStr;
                            result.group_id = _getRequest.TM_Divisions.division_name_en;
                            result.rank_id = _getRequest.TM_Pool_Rank != null ? _getRequest.TM_Pool_Rank.Pool_rank_name_en + "" : "-";
                            result.recommended_rank_id = _getRequest.TM_Pool_Rank != null ? _getRequest.TM_Pool_Rank.Id + "" : "";
                            result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                            if (_getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank != null && _getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").Any())
                            {
                                result.lstrank.AddRange(_getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Rank.piority).Select(s => new vSelect_PR { id = s.Id + "", name = s.Pool_rank_name_en }).ToList());
                            }
                            result.sub_group_id = _getRequest.TM_SubGroup != null ? _getRequest.TM_SubGroup.sub_group_name_en + "" : "-";
                            result.position_id = _getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "-";
                            result.employment_type_id = _getRequest.TM_Employment_Request.TM_Employment_Type != null ? _getRequest.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                            result.request_type_id = _getRequest.TM_Employment_Request.TM_Request_Type != null ? _getRequest.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";
                            result.candidate_name = _getData.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + _getData.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;

                            if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm) && (_getRequest.type_of_TIFForm == "N"))
                            {
                                var _getTIF = _TM_TIF_FormService.GetActiveTIFForm();
                                if (_getTIF != null && _getTIF.TM_TIF_Form_Question != null)
                                {
                                    var _CheckDatanew = _TM_Candidate_TIFService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                                    if (_CheckDatanew != null)
                                    {
                                        result.recommended_rank_id = _CheckDatanew.Recommended_Rank != null ? _CheckDatanew.Recommended_Rank.Id + "" : "";
                                        result.confidentiality_agreement = _CheckDatanew.confidentiality_agreement + "";
                                        result.comment = _CheckDatanew.comments;
                                        result.tif_status_id = _CheckDatanew.TM_TIF_Status != null ? _CheckDatanew.TM_TIF_Status.Id + "" : "";
                                        if (_CheckDatanew.TM_Candidate_TIF_Answer != null && _CheckDatanew.TM_Candidate_TIF_Answer.Any(a => a.active_status == "Y"))
                                        {
                                            _getTIF = _TM_TIF_FormService.GetActiveTIFForm(_CheckDatanew.TM_Candidate_TIF_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_TIF_Form_Question.TM_TIF_Form.Id);
                                            result.TIF_type = _getRequest.type_of_TIFForm;
                                            result.objtifform = new vObject_of_tif();
                                            result.objtifform.TIF_no = _getTIF.Id + "";
                                            result.objtifform.lstQuestion = (from lstQ in _getTIF.TM_TIF_Form_Question
                                                                             select new vtif_list_question
                                                                             {
                                                                                 id = lstQ.Id + "",
                                                                                 question = lstQ.question,
                                                                                 header = lstQ.header,
                                                                                 nSeq = lstQ.seq,
                                                                                 remark = "",
                                                                                 rating = "",

                                                                             }).ToList();

                                            result.objtifform.lstQuestion.ForEach(ed =>
                                            {
                                                var GetAns = _CheckDatanew.TM_Candidate_TIF_Answer.Where(w => w.TM_TIF_Form_Question.Id + "" == ed.id).FirstOrDefault();
                                                if (GetAns != null)
                                                {
                                                    ed.remark = GetAns.answer + "";
                                                    ed.rating = GetAns.TM_TIF_Rating != null ? GetAns.TM_TIF_Rating.Id + "" : "";
                                                    ed.point = GetAns.TM_TIF_Rating != null ? GetAns.TM_TIF_Rating.point + "" : "";
                                                }
                                            });
                                        }
                                        else
                                        {

                                            result.TIF_type = _getRequest.type_of_TIFForm;
                                            result.objtifform = new vObject_of_tif();
                                            result.objtifform.TIF_no = _getTIF.Id + "";

                                            result.objtifform.lstQuestion = (from lstQ in _getTIF.TM_TIF_Form_Question
                                                                             select new vtif_list_question
                                                                             {
                                                                                 id = lstQ.Id + "",
                                                                                 question = lstQ.question,
                                                                                 header = lstQ.header,
                                                                                 nSeq = lstQ.seq,
                                                                                 remark = "",
                                                                                 rating = "",

                                                                             }).ToList();
                                        }
                                    }
                                    else
                                    {

                                        result.TIF_type = _getRequest.type_of_TIFForm;
                                        result.objtifform = new vObject_of_tif();
                                        result.objtifform.TIF_no = _getTIF.Id + "";

                                        result.objtifform.lstQuestion = (from lstQ in _getTIF.TM_TIF_Form_Question
                                                                         select new vtif_list_question
                                                                         {
                                                                             id = lstQ.Id + "",
                                                                             question = lstQ.question,
                                                                             header = lstQ.header,
                                                                             nSeq = lstQ.seq,
                                                                             remark = "",
                                                                             rating = "",
                                                                         }).ToList();
                                    }
                                    result.objtifform.lstRating = _TM_TIF_RatingService.GetDataForSelect().Select(s => new lstDataSelect
                                    {
                                        value = s.Id + "",
                                        text = s.rating_name_en + "",
                                        detail = s.rating_description + "",

                                    }).ToList();
                                    result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                }
                                else
                                {
                                    result.TIF_type = "";
                                    result.pr_no = _getRequest.RefNo + "";
                                    string sUserNo = _getData.TM_PR_Candidate_Mapping.TM_Recruitment_Team.user_no + "";
                                    if (!string.IsNullOrEmpty(sUserNo))
                                    {
                                        var getOwner = dbHr.AllInfo_WS.Where(w => w.EmpNo == sUserNo).FirstOrDefault();
                                        result.null_tif_contact = getOwner != null ? getOwner.EmpFullName + "(" + getOwner.OfficePhone + ")" : "";
                                    }
                                }

                            }
                            else if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm) && _getRequest.type_of_TIFForm == "M")
                            {
                                //check Form Mass New or Old
                                // 2/10/2020

                                var _getMassTIF = _TM_Mass_TIF_FormService.GetActiveTIFForm();
                                var _CheckDatanew = _TM_Candidate_MassTIFService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);



                                //Check Id form Question
                                if (_getMassTIF != null)
                                {
                                    //Question 
                                    result.TIF_type = _getRequest.type_of_TIFForm;
                                    result.objMasstifform = new vObject_of_Masstif();
                                    result.objMasstifform.TIF_no = _getMassTIF.Id + "";
                                    result.objMasstifform.lstQuestion = (from lstQ in _getMassTIF.TM_MassTIF_Form_Question
                                                                         select new vMasstif_list_question
                                                                         {
                                                                             id = lstQ.Id + "",
                                                                             header = lstQ.header + "",
                                                                             question = lstQ.question + "",
                                                                             rating = ""
                                                                         }).ToList();
                                    //rating Core
                                    var abc = _TM_TIF_RatingService.GetDataForSelect();
                                    result.objMasstifform.lstRating = abc.Select(s => new lstDataSelectMassRating
                                    {
                                        value = s.Id + "",
                                        text = s.rating_name_en + "",
                                    }).ToList();
                                    //Rating list Audit Question New
                                    result.objMasstifform.lstScoring = _TM_Mass_ScoringService.GetDataForSelect().Select(s => new lstDataSelectMass
                                    {
                                        value = s.Id + "",
                                        text = s.scoring_name_en + "",
                                        point = s.point,
                                        code = s.scoring_code
                                    }).ToList();
                                    result.objMasstifform.lstScoring.Insert(0, new lstDataSelectMass { value = "", text = " - Select - ", point = 0, code = "" });

                                    result.objMasstifform.lstRating.Insert(0, new lstDataSelectMassRating { value = "", text = " - Select - " });

                                }

                                var _chk = _TM_Mass_TIF_FormService.GetActiveTIFForm();
                                var _CheckData = _TM_Candidate_MassTIFService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                                List<vMasstif_list_Auditing> lstAuditing = new List<vMasstif_list_Auditing>();
                                //New or Old Audit question
                                if (_chk != null)
                                {
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Easy Question",
                                        nSeq = 1,
                                        id = 1 + "",
                                        answer = "",
                                        //lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.TM_Mass_Question_Type.Id == _CheckData.TM_Mass_Question_Type.Id && w.group_question == "1").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),
                                        lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.group_question == "1").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),

                                    });
                                    //var quiz1 = lstAuditing.Where(w => w.nSeq == 1).FirstOrDefault();
                                    //quiz1.lstQuiz.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Easy Question",
                                        nSeq = 2,
                                        id = 2 + "",
                                        answer = "",
                                        lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.group_question == "1").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),
                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Moderate Questions",
                                        nSeq = 3,
                                        id = 3 + "",
                                        answer = "",
                                        lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.group_question == "2").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),
                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Moderate Questions",
                                        nSeq = 4,
                                        id = 4 + "",
                                        answer = "",
                                        lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.group_question == "2").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),
                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Hard Questions",
                                        nSeq = 5,
                                        id = 5 + "",
                                        answer = "",
                                        lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.group_question == "3").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),
                                    });

                                    foreach (var item in lstAuditing)
                                    {
                                        item.lstQuiz.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                    }

                                }
                                else
                                {
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Fixed Question",
                                        nSeq = 1,
                                        id = 1 + "",
                                        answer = "",
                                        group = "1",

                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Fixed Question",
                                        nSeq = 2,
                                        id = 2 + "",
                                        answer = "",
                                        group = "1",
                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Auditing Knowledge",
                                        nSeq = 3,
                                        id = 3 + "",
                                        answer = "",
                                        group = "2",
                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Audit Procedures",
                                        nSeq = 4,
                                        id = 4 + "",
                                        answer = "",
                                        group = "2",
                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Skepticism / Analytical",
                                        nSeq = 5,
                                        id = 5 + "",
                                        answer = "",
                                        group = "3",
                                    });
                                }
                                result.objMasstifform.lstAuditing = lstAuditing.ToList();
                                result.objMasstifform.lstAuditing_Qst = new List<lstDataSelect>();
                                result.objMasstifform.lstAuditing_Qst.Insert(0, new lstDataSelect { value = "", text = " - Select - " });

                                var GetAdditional_Information = _TM_Additional_InformationService.GetActiveTIFForm();
                                var GetAdditional_InformationNew = _TM_Additional_InformationService.GetActiveTIFForm();
                                if (GetAdditional_Information != null)
                                {
                                    result.objMasstifform.lstAdInfo = new List<vMassAdInfo_Question>();
                                    result.objMasstifform.lstAdInfo = GetAdditional_InformationNew.TM_Additional_Questions.Select(s => new vMassAdInfo_Question
                                    {
                                        question = s.header + "" + "<br/>" + s.question + "",
                                        nID = s.Id + "",
                                        multi_answer = s.multi_answer + "",
                                        seq = s.seq,
                                        other_answer = "",
                                        lstAnswers = s.TM_Additional_Answers != null ? s.TM_Additional_Answers.Where(w => w.active_status == "Y").OrderBy(o => o.seq).Select(s2 => new vMassAdInfo_Answers
                                        {
                                            nID = s2.Id + "",
                                            answer = s2.answers,
                                            seq = s2.seq,

                                        }).ToList() : new List<vMassAdInfo_Answers>(),
                                        is_validate = s.is_validate,
                                        lstAnswersselect = new string[] { },
                                    }).ToList();
                                }


                                if (_CheckData != null)
                                {
                                    var _ChkMass = _TM_Candidate_MassTIFService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                                    var _formMass = _ChkMass.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question != null).FirstOrDefault();
                                    var _chkMass = _formMass.TM_MassTIF_Form_Question.TM_Mass_TIF_Form.Id;
                                    result.recommended_rank_id = _CheckData.Recommended_Rank != null ? _CheckData.Recommended_Rank.Id + "" : "";
                                    result.confidentiality_agreement = _CheckData.confidentiality_agreement + "";
                                    result.set_type = _CheckData.TM_Mass_Question_Type != null ? _CheckData.TM_Mass_Question_Type.Id + "" : "";
                                    result.comment = _CheckData.comments;
                                    result.tif_status_id = _CheckData.TM_MassTIF_Status != null ? _CheckData.TM_MassTIF_Status.Id + "" : "";
                                    result.target_start = _CheckData.can_start_date.HasValue ? _CheckData.can_start_date.Value.DateTimebyCulture() : "";

                                    if (_CheckData.TM_Candidate_MassTIF_Core != null && _CheckData.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                    {
                                        result.objMasstifform.lstQuestion.ForEach(ed =>
                                        {
                                            var GetAns = _CheckData.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question.Id + "" == ed.id).FirstOrDefault();
                                            //Get Ans Rating Comment 
                                            ed.rating = GetAns.TM_TIF_Rating != null ? GetAns.TM_TIF_Rating.Id + "" : "";
                                            ed.evidence = GetAns.evidence + "";
                                            ed.scoring = GetAns.TM_Mass_Scoring != null ? GetAns.TM_Mass_Scoring.scoring_code + "" : "";

                                        });
                                        //result.objMasstifform.lstQuestion.Insert(0, new vMasstif_list_question { value = "", rating = " - Select - " });

                                    }


                                    if (_CheckData.TM_Candidate_MassTIF_Audit_Qst != null && _CheckData.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                    {
                                        result.objMasstifform.lstAuditing.ForEach(ed =>
                                        {
                                            var GetAns = _CheckData.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.seq + "" == ed.id).FirstOrDefault();
                                            if (GetAns != null)
                                            {
                                                ed.answer = GetAns.answer + "";
                                                ed.scoring = GetAns.TM_Mass_Scoring != null ? GetAns.TM_Mass_Scoring.Id + "" : "";
                                                ed.Qst = GetAns.TM_Mass_Auditing_Question != null ? GetAns.TM_Mass_Auditing_Question.Id + "" : "";
                                            }
                                        });
                                    }
                                    if (_getMassTIF.TM_Mass_Auditing_Question != null && _getMassTIF.TM_Mass_Auditing_Question.Any() && _CheckData.TM_Mass_Question_Type != null)
                                    {

                                        result.objMasstifform.lstAuditing_Qst.AddRange(_getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.TM_Mass_Question_Type.Id == _CheckData.TM_Mass_Question_Type.Id && w.group_question == null).OrderBy(o => o.seq).Select(s => new lstDataSelect
                                        {
                                            value = s.Id + "",
                                            text = s.seq + " : " + s.header
                                        }).ToList());

                                    }
                                    if (_CheckData.TM_Candidate_MassTIF_Additional != null && _CheckData.TM_Candidate_MassTIF_Additional.Any(a => a.active_status == "Y"))
                                    {
                                        result.objMasstifform.lstAdInfo.ForEach(ed =>
                                        {
                                            var GetAns = _CheckData.TM_Candidate_MassTIF_Additional.Where(w => w.TM_Additional_Questions.Id + "" == ed.nID).FirstOrDefault();
                                            if (GetAns != null)
                                            {

                                                ed.other_answer = GetAns.other_answer + "";
                                                ed.lstAnswersselect = GetAns.TM_Candidate_MassTIF_Adnl_Answer != null ? GetAns.TM_Candidate_MassTIF_Adnl_Answer.Where(w => w.active_status == "Y").Select(s => s.TM_Additional_Answers.Id + "").ToArray() : new string[] { };


                                            }
                                        });

                                    }
                                    result.objMasstifform.lstScoring = _TM_Mass_ScoringService.GetDataForSelect().Select(s => new lstDataSelectMass
                                    {
                                        value = s.Id + "",
                                        text = s.scoring_name_en + "",
                                        point = s.point,
                                        code = s.scoring_code
                                    }).ToList();
                                    result.objMasstifform.lstScoring.Insert(0, new lstDataSelectMass { value = "", text = " - Select - ", point = 0, code = "" });
                                }
                                else
                                {
                                    //result.objMasstifform.lstScoring = _TM_Mass_ScoringService.GetDataForSelect().Select(s => new lstDataSelectMass
                                    //{
                                    //    value = s.Id + "",
                                    //    text = s.scoring_name_en + "",
                                    //    point = s.point,
                                    //    code = s.scoring_code
                                    //}).ToList();
                                    //result.objMasstifform.lstScoring.Insert(0, new lstDataSelectMass { value = "", text = " - Select - ", point = 0, code = "" });

                                }
                            }
                            else
                            {
                                result.TIF_type = "";
                                result.pr_no = _getRequest.RefNo + "";
                                string sUserNo = _getData.TM_PR_Candidate_Mapping.TM_Recruitment_Team.user_no + "";
                                if (!string.IsNullOrEmpty(sUserNo))
                                {
                                    var getOwner = dbHr.AllInfo_WS.Where(w => w.EmpNo == sUserNo).FirstOrDefault();
                                    result.null_tif_contact = getOwner != null ? getOwner.EmpFullName + "(" + getOwner.OfficePhone + ")" : "";
                                }
                            }
                            //get date of active form
                            var getactiveform = _TM_TIF_FormService.GetActiveTIFForm();

                            //Check Non mass empty or No empty
                            var _CheckPage = _TM_Candidate_TIFService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                            var _CheckPageMass = _TM_Candidate_MassTIFService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                            //If SaveDraft
                            if (_CheckPage != null && _CheckPage.TM_Candidate_TIF_Answer != null && _CheckPage.TM_Candidate_TIF_Answer.Any())
                            {
                                var _ChekAns = _CheckPage.TM_Candidate_TIF_Answer.Where(w => w.TM_TIF_Form_Question != null).FirstOrDefault();
                                formId = _ChekAns.TM_TIF_Form_Question.TM_TIF_Form.Id;

                                if (formId >= 7)
                                {
                                    if (formId == 7)
                                    {
                                        return View("InterviewEditNew", result);
                                    }
                                    else if (DateTime.Now >= getactiveform.action_date.Value)
                                    {
                                        return View("InterviewEditOCT2021", result);
                                    }
                                }
                                else if (formId == 1)
                                {
                                    return View(result);
                                }

                            }
                            // If not Answer NonMass and Mass
                            else if (_CheckPage == null && _CheckPageMass == null)
                            {
                                if (result.TIF_type == "N")
                                {

                                    if (getactiveform.Id >= 7)
                                    {
                                        if (getactiveform.Id == 7 && getactiveform.action_date.Value < DateTime.Now)
                                        {
                                            return View("InterviewEditNew", result);
                                        }
                                        else if (DateTime.Now >= getactiveform.action_date.Value)
                                        {
                                            return View("InterviewEditOCT2021", result);
                                        }
                                        else
                                        {

                                            return View("InterviewEditNew", result);

                                        }

                                    }
                                }
                                else
                                {
                                    return View("InterviewEditNew", result);
                                }
                            }
                            // Mass 
                            else
                            {

                                return View("InterviewEditNew", result);
                            }
                        }
                    }
                }
            }
            return RedirectToAction("InterviewList", "Interview");
            //return View(result);
            #endregion
        }
        public ActionResult APTIFFormList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            vInterview result = new vInterview();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                List<vInterview_obj> lstData_resutl = new List<vInterview_obj>();
                List<vInterview_obj> lstData_resutlMass = new List<vInterview_obj>();
                CSearchInterview SearchItem = (CSearchInterview)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchInterview)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                List<TM_Candidate_TIF_Approv> lstData = new List<TM_Candidate_TIF_Approv>();
                List<TM_Candidate_MassTIF_Approv> lstDataMass = new List<TM_Candidate_MassTIF_Approv>();

                if (SearchItem.tif_type + "" == "")
                {
                    lstData = _TM_Candidate_TIF_ApprovService.GetTIFFormForApprove(
                                                      SearchItem.group_code, aDivisionPermission,
                                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
                    lstDataMass = _TM_Candidate_MassTIF_ApprovService.GetTIFFormForApprove(
                                                      SearchItem.group_code, aDivisionPermission,
                                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
                }
                else if (SearchItem.tif_type + "" == "N")
                {
                    lstData = _TM_Candidate_TIF_ApprovService.GetTIFFormForApprove(
                                                      SearchItem.group_code, aDivisionPermission,
                                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
                }
                else if (SearchItem.tif_type + "" == "M")
                {
                    lstDataMass = _TM_Candidate_MassTIF_ApprovService.GetTIFFormForApprove(
                                          SearchItem.group_code, aDivisionPermission,
                                         CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
                }

                result.tif_type = SearchItem.tif_type;
                result.group_code = SearchItem.group_code;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    string[] aApproveUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                    var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                    lstData_resutl = (from lstAD in lstData.Where(w => w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id != 10)
                                      from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vInterview_obj
                                      {
                                          refno = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                          name_en = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                          status = !string.IsNullOrEmpty(lstAD.Approve_status) ? (lstAD.Approve_status == "Y" ? "Confirmed" : "Reject") : "Waiting",

                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          approval = AEmp.EmpFullName,
                                          pr_type = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                      }).ToList();
                }

                if (lstDataMass.Any())
                {

                    string[] aApproveUser = lstDataMass.Select(s => s.Req_Approve_user).ToArray();
                    string[] aUpdateUser = lstDataMass.Select(s => s.update_user).ToArray();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                    var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                    lstData_resutlMass = (from lstAD in lstDataMass.Where(w => w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id != 10)
                                          from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                          from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                          select new vInterview_obj
                                          {
                                              refno = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
                                              group_name = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                              position = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
                                              rank = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                              name_en = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                              status = !string.IsNullOrEmpty(lstAD.Approve_status) ? (lstAD.Approve_status == "Y" ? "Confirmed" : "Reject") : "Waiting",

                                              update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMass('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                              approval = AEmp.EmpFullName,
                                              pr_type = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                          }).ToList();
                }
                result.lstData = lstData_resutl.Concat(lstData_resutlMass).ToList();
            }

            #endregion
            return View(result);
        }
        public ActionResult APTIFFormEdit(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code
            var formId = default(int);
            var MassformId = default(int);
            vInterview_obj_save result = new vInterview_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Candidate_TIF_ApprovService.Find(nId);
                    if (_getData != null)
                    {

                        bool isHRAdmin = CGlobal.UserIsHRAdmin();


                        var _getRequest = _getData.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest;
                        if (_getRequest != null)
                        {
                            // if ((int)HCMClass.UnitGroup.HR == _getRequest.TM_Divisions.Id && )
                            if (!isHRAdmin && !CGlobal.UserIsAdmin() && _getData.Req_Approve_user != CGlobal.UserInfo.EmployeeNo)
                            {

                                return RedirectToAction("ErrorNopermission", "MasterPage");

                            }



                            result.lstApprove = new List<vPersonnelAp_obj>();
                            var _getTIF = _TM_TIF_FormService.GetActiveTIFForm();
                            result.IdEncrypt = qryStr;
                            result.group_id = _getRequest.TM_Divisions.division_name_en;
                            result.rank_id = _getRequest.TM_Pool_Rank != null ? _getRequest.TM_Pool_Rank.Pool_rank_name_en + "" : "-";
                            result.recommended_rank_id = _getRequest.TM_Pool_Rank != null ? _getRequest.TM_Pool_Rank.Id + "" : "";
                            result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                            if (_getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank != null && _getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").Any())
                            {
                                result.lstrank.AddRange(_getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Rank.piority).Select(s => new vSelect_PR { id = s.Id + "", name = s.Pool_rank_name_en }).ToList());
                            }
                            result.sub_group_id = _getRequest.TM_SubGroup != null ? _getRequest.TM_SubGroup.sub_group_name_en + "" : "-";
                            result.position_id = _getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "-";
                            result.employment_type_id = _getRequest.TM_Employment_Request.TM_Employment_Type != null ? _getRequest.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                            result.request_type_id = _getRequest.TM_Employment_Request.TM_Request_Type != null ? _getRequest.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";

                            result.candidate_name = _getData.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + _getData.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                            result.approve_status = _getData.Approve_status + "";
                            if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm)
                                && (_getRequest.type_of_TIFForm == "M" || _getRequest.type_of_TIFForm == "N")
                                && _getTIF != null && _getTIF.TM_TIF_Form_Question != null)
                            {
                                var _CheckData = _TM_Candidate_TIFService.FindByMappingID(_getData.TM_Candidate_TIF.TM_PR_Candidate_Mapping.Id);

                                result.TIF_type = _getRequest.type_of_TIFForm;
                                result.objtifform = new vObject_of_tif();
                                result.objtifform.TIF_no = _getTIF.Id + "";
                                result.objtifform.lstQuestion = (from lstQ in _getTIF.TM_TIF_Form_Question
                                                                 select new vtif_list_question
                                                                 {
                                                                     id = lstQ.Id + "",
                                                                     question = lstQ.question,
                                                                     header = lstQ.header,
                                                                     nSeq = lstQ.seq,
                                                                     remark = "",
                                                                     rating = "",
                                                                 }).ToList();

                                if (_CheckData != null)
                                {
                                    result.recommended_rank_id = _CheckData.Recommended_Rank != null ? _CheckData.Recommended_Rank.Id + "" : "";
                                    result.confidentiality_agreement = _CheckData.confidentiality_agreement + "";
                                    result.comment = _CheckData.comments;
                                    result.tif_status_id = _CheckData.TM_TIF_Status != null ? _CheckData.TM_TIF_Status.Id + "" : "";
                                    if (_CheckData.TM_Candidate_TIF_Answer != null && _CheckData.TM_Candidate_TIF_Answer.Any(a => a.active_status == "Y"))
                                    {
                                        _getTIF = _TM_TIF_FormService.GetActiveTIFForm(_CheckData.TM_Candidate_TIF_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_TIF_Form_Question.TM_TIF_Form.Id);
                                        if (_getTIF != null)
                                        {

                                        }
                                        result.TIF_type = _getRequest.type_of_TIFForm;
                                        result.objtifform = new vObject_of_tif();
                                        result.objtifform.TIF_no = _getTIF.Id + "";
                                        result.objtifform.lstQuestion = (from lstQ in _getTIF.TM_TIF_Form_Question
                                                                         select new vtif_list_question
                                                                         {
                                                                             id = lstQ.Id + "",
                                                                             question = lstQ.question,
                                                                             header = lstQ.header,
                                                                             nSeq = lstQ.seq,
                                                                             remark = "",
                                                                             rating = "",
                                                                         }).ToList();
                                        result.objtifform.lstQuestion.ForEach(ed =>
                                        {
                                            var GetAns = _CheckData.TM_Candidate_TIF_Answer.Where(w => w.TM_TIF_Form_Question.Id + "" == ed.id).FirstOrDefault();
                                            if (GetAns != null)
                                            {
                                                ed.remark = GetAns.answer + "";
                                                ed.rating = GetAns.TM_TIF_Rating != null ? GetAns.TM_TIF_Rating.Id + "" : "";
                                            }

                                        });
                                    }

                                    if (_CheckData.TM_Candidate_TIF_Approv != null && _CheckData.TM_Candidate_TIF_Approv.Any())
                                    {
                                        string[] aUser = _CheckData.TM_Candidate_TIF_Approv.Select(s => s.Req_Approve_user).ToArray();
                                        string[] aApproveUser = _CheckData.TM_Candidate_TIF_Approv.Select(s => s.Approve_user).ToArray();
                                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                        var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                        int nPriority = 1;
                                        result.lstApprove = (from Tif in _CheckData.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
                                                             from Emp in _getEmp.Where(w => w.EmpNo == Tif.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                             from AEmp in _getApproveEmp.Where(w => w.EmpNo == Tif.Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                             select new vPersonnelAp_obj
                                                             {
                                                                 nStep = nPriority++,
                                                                 app_name = Emp.EmpFullName + (Tif.Approve_status + "" == "Y" ? (Tif.Req_Approve_user + "" != Tif.Approve_user + "" ? "<br/>(by" + AEmp.EmpFullName + ")" : "") : ""),
                                                                 approve_date = Tif.Approve_date.HasValue ? Tif.Approve_date.Value.DateTimebyCulture() : "",
                                                                 app_code = Tif.Approve_status + "" == "" ? "Waiting" : (Tif.Approve_status + "" == "Y" ? "Confirmed" : "Reject"),
                                                             }).ToList();
                                    }
                                }
                                if (_getTIF.active_status == "Y")
                                {
                                    result.objtifform.lstRating = _TM_TIF_RatingService.GetDataForSelect().Select(s => new lstDataSelect
                                    {
                                        value = s.Id + "",
                                        text = s.rating_name_en + "",
                                        detail = s.rating_description + "",
                                    }).ToList();
                                }
                                else if (_getTIF.active_status == "N")
                                {
                                    result.objtifform.lstRating = _TM_TIF_RatingService.GetDataForSelectOld().Select(s => new lstDataSelect
                                    {
                                        value = s.Id + "",
                                        text = s.rating_name_en + "",
                                        detail = s.rating_description + "",
                                    }).ToList();
                                }

                                result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                            }
                            else
                            {
                                result.TIF_type = "";
                                result.pr_no = _getRequest.RefNo + "";
                                string sUserNo = _getData.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Recruitment_Team.user_no + "";
                                if (!string.IsNullOrEmpty(sUserNo))
                                {
                                    var getOwner = dbHr.AllInfo_WS.Where(w => w.EmpNo == sUserNo).FirstOrDefault();
                                    result.null_tif_contact = getOwner != null ? getOwner.EmpFullName + "(" + getOwner.OfficePhone + ")" : "";
                                }
                            }
                            //Send Form New or Old
                            //Check ID
                            var _getDataForm = _TM_Candidate_Status_CycleService.Find(nId);
                            //Check Form By ID Candidate
                            var _CheckForm = _TM_Candidate_TIF_ApprovService.Find(_getData.TM_Candidate_TIF.Id);


                            var _CheckPageApporve = _TM_Candidate_TIF_ApprovService.Find(_getDataForm.TM_PR_Candidate_Mapping.Id);


                            if (_CheckPageApporve != null)
                            {
                                //var _ChekAns = _CheckPage.TM_Candidate_TIF_Answer.Where(w => w.TM_TIF_Form_Question != null).FirstOrDefault();
                                var _CheckF = _CheckPageApporve.TM_Candidate_TIF.TM_Candidate_TIF_Answer.Where(w => w.TM_TIF_Form_Question != null).FirstOrDefault();
                                formId = _CheckF.TM_TIF_Form_Question.TM_TIF_Form.Id;
                                //Check ID Form for send New Form or Old Form
                                if (formId == 1)
                                {
                                    return View(result);
                                }
                                else
                                {
                                    return View("APTIFFormEditNew", result);
                                }
                            }
                            else
                            {
                                return View("APTIFFormEditNew", result);
                            }
                        }
                    }

                }

            }
            //return View(result);
            return RedirectToAction("APTIFFormList", "Interview");

            #endregion

        }
        public ActionResult APMassTIFFormEdit(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vInterview_obj_save result = new vInterview_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _TM_Candidate_MassTIF_ApprovService.Find(nId);
                    if (_getData != null)
                    {
                        var _getRequest = _getData.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest;
                        if (_getRequest != null)
                        {
                            if (!CGlobal.UserIsHRAdmin() && !CGlobal.UserIsAdmin() && _getData.Req_Approve_user != CGlobal.UserInfo.EmployeeNo)
                            {

                                return RedirectToAction("ErrorNopermission", "MasterPage");

                            }

                            result.lstApprove = new List<vPersonnelAp_obj>();
                            var _getMassTIF = _TM_Mass_TIF_FormService.GetActiveTIFForm();
                            result.IdEncrypt = qryStr;
                            result.group_id = _getRequest.TM_Divisions.division_name_en;
                            result.rank_id = _getRequest.TM_Pool_Rank != null ? _getRequest.TM_Pool_Rank.Pool_rank_name_en + "" : "-";
                            result.recommended_rank_id = _getRequest.TM_Pool_Rank != null ? _getRequest.TM_Pool_Rank.Id + "" : "";
                            result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                            if (_getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank != null && _getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").Any())
                            {
                                result.lstrank.AddRange(_getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Rank.piority).Select(s => new vSelect_PR { id = s.Id + "", name = s.Pool_rank_name_en }).ToList());
                            }

                            result.sub_group_id = _getRequest.TM_SubGroup != null ? _getRequest.TM_SubGroup.sub_group_name_en + "" : "-";
                            result.position_id = _getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "-";
                            result.employment_type_id = _getRequest.TM_Employment_Request.TM_Employment_Type != null ? _getRequest.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                            result.request_type_id = _getRequest.TM_Employment_Request.TM_Request_Type != null ? _getRequest.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";

                            result.candidate_name = _getData.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + _getData.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                            result.approve_status = _getData.Approve_status + "";
                            if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm)
                                && (_getRequest.type_of_TIFForm == "M" || _getRequest.type_of_TIFForm == "N")
                                && _getMassTIF != null && _getMassTIF.TM_MassTIF_Form_Question != null)
                            {

                                ////check Active Form
                                var _CheckDatanew = _TM_Candidate_MassTIFService.FindByMappingID(_getData.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.Id);

                                //check Form Mass
                                var _ChkMass = _TM_Candidate_MassTIFService.FindByMappingID(_getData.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.Id);
                                var _formMass = _ChkMass.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question != null).FirstOrDefault();
                                var _chkMass = _formMass.TM_MassTIF_Form_Question.TM_Mass_TIF_Form.Id;
                                if (_chkMass != 0)
                                    _getMassTIF = _TM_Mass_TIF_FormService.Find(_chkMass); 

                                if (_getMassTIF != null)
                                {
                                    //Question Old
                                    if (_chkMass == 1)
                                    {
                                        var _getMassForm = _TM_Mass_TIF_FormService.GetActiveMassTIFForm(_CheckDatanew.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").FirstOrDefault().TM_MassTIF_Form_Question.TM_Mass_TIF_Form.Id);

                                        result.TIF_type = _getRequest.type_of_TIFForm;
                                        result.objMasstifform = new vObject_of_Masstif();
                                        result.objMasstifform.TIF_no = _getMassForm.Id + "";
                                        result.objMasstifform.lstQuestion = (from lstQ in _getMassForm.TM_MassTIF_Form_Question
                                                                             select new vMasstif_list_question
                                                                             {
                                                                                 id = lstQ.Id + "",
                                                                                 header = lstQ.seq + ". " + lstQ.header,
                                                                                 nSeq = lstQ.seq,
                                                                                 a_answer = lstQ.a_answer + "",
                                                                                 b_answer = lstQ.b_answer + "",
                                                                                 c_answer = lstQ.c_answer + "",
                                                                                 evidence = "",
                                                                             }).ToList();
                                    }
                                    else
                                    {
                                        //Question Form New
                                        result.TIF_type = _getRequest.type_of_TIFForm;
                                        result.objMasstifform = new vObject_of_Masstif();
                                        result.objMasstifform.TIF_no = _getMassTIF.Id + "";
                                        result.objMasstifform.lstQuestion = (from lstQ in _getMassTIF.TM_MassTIF_Form_Question
                                                                            
                                                                             select new vMasstif_list_question
                                                                             {
                                                                                 id = lstQ.Id + "",
                                                                                 header = lstQ.header + "",
                                                                                 question = lstQ.question + "",
                                                                                 rating = ""
                                                                             }).ToList();

                                        //Rating
                                        var abc = _TM_TIF_RatingService.GetDataForSelect();
                                        result.objMasstifform.lstRating = abc.Select(s => new lstDataSelectMassRating
                                        {
                                            value = s.Id + "",
                                            text = s.rating_name_en + "",
                                        }).ToList();
                                        result.objMasstifform.lstRating.Insert(0, new lstDataSelectMassRating { value = "", text = " - Select - " });


                                    }

                                }
                                List<vMasstif_list_Auditing> lstAuditing = new List<vMasstif_list_Auditing>();
                                if (_chkMass == 1)
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


                                    result.objMasstifform.lstScoring = _TM_Mass_ScoringService.GetDataForSelectOld().Select(s => new lstDataSelectMass
                                    {
                                        value = s.Id + "",
                                        text = s.scoring_name_en + "",
                                        point = s.point,
                                        code = s.scoring_code
                                    }).ToList();
                                    result.objMasstifform.lstScoring.Insert(0, new lstDataSelectMass { value = "", text = " - Select - ", point = 0, code = "" });
                                }
                                else
                                {
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Easy Question",
                                        nSeq = 1,
                                        id = 1 + "",
                                        answer = "",
                                        //lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.TM_Mass_Question_Type.Id == _CheckData.TM_Mass_Question_Type.Id && w.group_question == "1").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),
                                        lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.group_question == "1").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),

                                    });
                                    //var quiz1 = lstAuditing.Where(w => w.nSeq == 1).FirstOrDefault();
                                    //quiz1.lstQuiz.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Easy Question",
                                        nSeq = 2,
                                        id = 2 + "",
                                        answer = "",
                                        lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.group_question == "1").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),
                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Moderate Questions",
                                        nSeq = 3,
                                        id = 3 + "",
                                        answer = "",
                                        lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.group_question == "2").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),
                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Moderate Questions",
                                        nSeq = 4,
                                        id = 4 + "",
                                        answer = "",
                                        lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.group_question == "2").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),
                                    });
                                    lstAuditing.Add(new vMasstif_list_Auditing
                                    {
                                        header = "Hard Questions",
                                        nSeq = 5,
                                        id = 5 + "",
                                        answer = "",
                                        lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.group_question == "3").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),
                                    });

                                    foreach (var item in lstAuditing)
                                    {
                                        item.lstQuiz.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                    }

                                    //Rating (Scoring)

                                    result.objMasstifform.lstScoring = _TM_Mass_ScoringService.GetDataForSelect().Select(s => new lstDataSelectMass
                                    {
                                        value = s.Id + "",
                                        text = s.scoring_name_en + "",
                                        point = s.point,
                                        code = s.scoring_code
                                    }).ToList();
                                    result.objMasstifform.lstScoring.Insert(0, new lstDataSelectMass { value = "", text = " - Select - ", point = 0, code = "" });
                                }
                                result.objMasstifform.lstAuditing = lstAuditing.ToList();
                                result.objMasstifform.lstAuditing_Qst = new List<lstDataSelect>();
                                result.objMasstifform.lstAuditing_Qst.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                // Additional Information
                                var GetAdditional_InformationOld = _TM_Additional_InformationService.GetActiveTIFFormOld();
                                var GetAdditional_InformationNew = _TM_Additional_InformationService.GetActiveTIFForm();
                                if (GetAdditional_InformationOld != null || GetAdditional_InformationNew != null)
                                {
                                    if (_chkMass == 1)
                                    {
                                        result.objMasstifform.lstAdInfo = new List<vMassAdInfo_Question>();
                                        result.objMasstifform.lstAdInfo = GetAdditional_InformationOld.TM_Additional_Questions.Select(s => new vMassAdInfo_Question
                                        {

                                            question = s.header + "" + "<br/>" + s.question + "",
                                            nID = s.Id + "",
                                            multi_answer = s.multi_answer + "",
                                            seq = s.seq,
                                            other_answer = "",
                                            lstAnswers = s.TM_Additional_Answers != null ? s.TM_Additional_Answers.Where(w => w.active_status == "Y").OrderBy(o => o.seq).Select(s2 => new vMassAdInfo_Answers
                                            {
                                                nID = s2.Id + "",
                                                answer = s2.answers,
                                                seq = s2.seq,

                                            }).ToList() : new List<vMassAdInfo_Answers>(),
                                            is_validate = s.is_validate,
                                            lstAnswersselect = new string[] { },
                                        }).ToList();
                                    }
                                    else
                                    {
                                        result.objMasstifform.lstAdInfo = new List<vMassAdInfo_Question>();
                                        result.objMasstifform.lstAdInfo = GetAdditional_InformationNew.TM_Additional_Questions.Select(s => new vMassAdInfo_Question
                                        {

                                            question = s.header + "" + "<br/>" + s.question + "",
                                            nID = s.Id + "",
                                            multi_answer = s.multi_answer + "",
                                            seq = s.seq,
                                            other_answer = "",
                                            lstAnswers = s.TM_Additional_Answers != null ? s.TM_Additional_Answers.Where(w => w.active_status == "Y").OrderBy(o => o.seq).Select(s2 => new vMassAdInfo_Answers
                                            {
                                                nID = s2.Id + "",
                                                answer = s2.answers,
                                                seq = s2.seq,

                                            }).ToList() : new List<vMassAdInfo_Answers>(),
                                            is_validate = s.is_validate,
                                            lstAnswersselect = new string[] { },
                                        }).ToList();
                                    }
                                }

                                //var bbb = _getData.TM_Candidate_MassTIF;
                                var _CheckData = _getData.TM_Candidate_MassTIF;

                                if (_CheckData != null)
                                {
                                    result.recommended_rank_id = _CheckData.Recommended_Rank != null ? _CheckData.Recommended_Rank.Id + "" : "";
                                    result.confidentiality_agreement = _CheckData.confidentiality_agreement + "";
                                    result.set_type = _CheckData.TM_Mass_Question_Type != null ? _CheckData.TM_Mass_Question_Type.Id + "" : "";
                                    result.comment = _CheckData.comments;
                                    result.tif_status_id = _CheckData.TM_MassTIF_Status != null ? _CheckData.TM_MassTIF_Status.Id + "" : "";
                                    result.target_start = _CheckData.can_start_date.HasValue ? _CheckData.can_start_date.Value.DateTimebyCulture() : "";
                                    //GetAns lstQuestion
                                    if (_CheckData.TM_Candidate_MassTIF_Core != null && _CheckData.TM_Candidate_MassTIF_Core.Any(a => a.active_status == "Y"))
                                    {
                                        if (_chkMass == 1)
                                        {
                                            result.objMasstifform.lstQuestion.ForEach(ed =>
                                            {
                                                var GetAns = _CheckData.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question.active_status == "N").FirstOrDefault();
                                                ed.scoring = GetAns.TM_Mass_Scoring != null ? GetAns.TM_Mass_Scoring.scoring_code + "" : "";
                                                ed.evidence = GetAns.evidence + "";
                                                //ed.header = GetAns.TM_MassTIF_Form_Question != null ? GetAns.TM_MassTIF_Form_Question.Id + "" : "";
                                            });
                                        }
                                        else
                                        {
                                            result.objMasstifform.lstQuestion.ForEach(ed =>
                                            {
                                                var GetAns = _CheckData.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question.Id + "" == ed.id).FirstOrDefault();
                                                //Get Ans Rating Comment 
                                                ed.rating = GetAns.TM_TIF_Rating != null ? GetAns.TM_TIF_Rating.Id + "" : "";
                                                ed.evidence = GetAns.evidence + "";
                                                ed.scoring = GetAns.TM_Mass_Scoring != null ? GetAns.TM_Mass_Scoring.scoring_code + "" : "";

                                            });
                                        }
                                    }
                                    //Get Ans lst_Audit 
                                    if (_CheckData.TM_Candidate_MassTIF_Audit_Qst != null && _CheckData.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                    {
                                        result.objMasstifform.lstAuditing.ForEach(ed =>
                                        {
                                            var GetAns = _CheckData.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.seq + "" == ed.id).FirstOrDefault();
                                            if (GetAns != null)
                                            {
                                                ed.answer = GetAns.answer + "";
                                                ed.scoring = GetAns.TM_Mass_Scoring != null ? GetAns.TM_Mass_Scoring.Id + "" : "";
                                                ed.Qst = GetAns.TM_Mass_Auditing_Question != null ? GetAns.TM_Mass_Auditing_Question.Id + "" : "";
                                            }
                                        });
                                    }


                                    if (_getMassTIF.TM_Mass_Auditing_Question != null && _getMassTIF.TM_Mass_Auditing_Question.Any() && _CheckData.TM_Mass_Question_Type != null)
                                    {
                                        result.objMasstifform.lstAuditing_Qst.AddRange(_getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.TM_Mass_Question_Type.Id == _CheckData.TM_Mass_Question_Type.Id).OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header }).ToList());
                                    }
                                    if (_CheckData.TM_Candidate_MassTIF_Approv != null && _CheckData.TM_Candidate_MassTIF_Approv.Any())
                                    {
                                        string[] aUser = _CheckData.TM_Candidate_MassTIF_Approv.Select(s => s.Req_Approve_user).ToArray();
                                        string[] aApproveUser = _CheckData.TM_Candidate_MassTIF_Approv.Select(s => s.Approve_user).ToArray();
                                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                        var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                        int nPriority = 1;
                                        result.lstApprove = (from Tif in _CheckData.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
                                                             from Emp in _getEmp.Where(w => w.EmpNo == Tif.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                             from AEmp in _getApproveEmp.Where(w => w.EmpNo == Tif.Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                             select new vPersonnelAp_obj
                                                             {
                                                                 nStep = nPriority++,
                                                                 app_name = Emp.EmpFullName + (Tif.Approve_status + "" == "Y" ? (Tif.Req_Approve_user + "" != Tif.Approve_user + "" ? "<br/>(by" + AEmp.EmpFullName + ")" : "") : ""),
                                                                 approve_date = Tif.Approve_date.HasValue ? Tif.Approve_date.Value.DateTimebyCulture() : "",
                                                                 app_code = Tif.Approve_status + "" == "" ? "Waiting" : (Tif.Approve_status + "" == "Y" ? "Confirmed" : "Reject"),
                                                             }).ToList();
                                    }

                                    if (_CheckData.TM_Candidate_MassTIF_Additional != null && _CheckData.TM_Candidate_MassTIF_Additional.Any(a => a.active_status == "Y"))
                                    {
                                        result.objMasstifform.lstAdInfo.ForEach(ed =>
                                        {
                                            var GetAns = _CheckData.TM_Candidate_MassTIF_Additional.Where(w => w.TM_Additional_Questions.Id + "" == ed.nID).FirstOrDefault();
                                            if (GetAns != null)
                                            {
                                                ed.other_answer = GetAns.other_answer + "";
                                                ed.lstAnswersselect = GetAns.TM_Candidate_MassTIF_Adnl_Answer != null ? GetAns.TM_Candidate_MassTIF_Adnl_Answer.Where(w => w.active_status == "Y").Select(s => s.TM_Additional_Answers.Id + "").ToArray() : new string[] { };
                                            }
                                        });

                                    }
                                }


                            }
                            else
                            {
                                result.TIF_type = "";
                                result.pr_no = _getRequest.RefNo + "";
                                string sUserNo = _getData.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Recruitment_Team.user_no + "";
                                if (!string.IsNullOrEmpty(sUserNo))
                                {
                                    var getOwner = dbHr.AllInfo_WS.Where(w => w.EmpNo == sUserNo).FirstOrDefault();
                                    result.null_tif_contact = getOwner != null ? getOwner.EmpFullName + "(" + getOwner.OfficePhone + ")" : "";
                                }
                            }

                        }
                    }
                }
                //
                var _getDataApp = _TM_Candidate_MassTIF_ApprovService.Find(nId);
                var _checkIdMass = _TM_Candidate_MassTIFService.FindByMappingID(_getDataApp.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.Id);
                var _idValue = _checkIdMass.TM_PR_Candidate_Mapping.Id;
                var _getMaptoMassTifFormId = _TM_Candidate_MassTIFService.FindByMappingID(_idValue);
                var trowToMassTifFormService = _getMaptoMassTifFormId.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question != null).FirstOrDefault();

                //Get Form Mass Id 
                var _getFormId = _TM_Mass_TIF_FormService.GetActiveTIFFormNew(trowToMassTifFormService.TM_MassTIF_Form_Question.TM_Mass_TIF_Form.Id);
                //Checl Form Mass By Id
                if (_getFormId.Id == 1)
                {
                    return View(result);
                }
                else
                {
                    return View("APMassTIFFormEditNew", result);
                }
            }


            return RedirectToAction("APTIFFormList", "Interview");
            //return View(result);

            #endregion

        }
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadInterviewList(CSearchInterview SearchItem)
        {
            vInterview_Return result = new vInterview_Return();
            bool isAdmin = CGlobal.UserIsAdmin();
            List<vInterview_obj> lstData_resutl = new List<vInterview_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            var lstData = _TM_PR_Candidate_MappingService.GetDataForInterview(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code,
                            SearchItem.group_code, aDivisionPermission,
                            "Y", SearchItem.name, isAdmin);
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

                string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();

                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                int[] nMapID = lstData.Select(s => s.Id).ToArray();
                var _GetTifform = _TM_Candidate_TIFService.FindByMappingArrayID(nMapID);
                var _GetMassTifform = _TM_Candidate_MassTIFService.FindByMappingArrayID(nMapID);
                if (_GetTifform.Any(a => a.submit_status == "Y"))
                {
                    lstData = lstData.Where(w => !_GetTifform.Where(w2 => w2.submit_status == "Y").Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray().Contains(w.Id)).ToList();
                }
                if (_GetMassTifform.Any(a => a.submit_status == "Y"))
                {
                    lstData = lstData.Where(w => !_GetMassTifform.Where(w2 => w2.submit_status == "Y").Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray().Contains(w.Id)).ToList();
                }
                lstData_resutl = (from lstAD in lstData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id != 10)
                                  from lstTIF in _GetTifform.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_TIF())
                                  from lstMassTIF in _GetMassTifform.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_MassTIF())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  select new vInterview_obj
                                  {
                                      refno = lstAD.PersonnelRequest.RefNo + "",
                                      group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                      position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                      rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                      name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                      status = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.submit_status != null ? (lstTIF.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting") : (lstMassTIF.submit_status != null ? (lstMassTIF.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting"),
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault().Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;
            return Json(new { result });
        }

        //[HttpPost]
        //public ActionResult LoadInterviewList(CSearchInterview SearchItem)
        //{
        //    vInterview_Return result = new vInterview_Return();
        //    bool isAdmin = CGlobal.UserIsAdmin();
        //    List<vInterview_obj> lstData_resutl = new List<vInterview_obj>();
        //    string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
        //    var lstData = _TM_Candidate_Status_CycleService.GetCandidateForInterview(
        //                            SearchItem.group_code, aDivisionPermission,
        //                            "Y", isAdmin);
        //    string qryStr = JsonConvert.SerializeObject(SearchItem,
        //    Formatting.Indented,
        //    new JsonSerializerSettings
        //    {
        //        NullValueHandling = NullValueHandling.Ignore,
        //        MissingMemberHandling = MissingMemberHandling.Ignore,
        //        DefaultValueHandling = DefaultValueHandling.Ignore,
        //    });
        //    string BackUrl = Uri.EscapeDataString(qryStr);
        //    if (lstData.Any())
        //    {

        //        string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();

        //        var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser);;
        //        int[] nMapID = lstData.Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray();
        //        var _GetTifform = _TM_Candidate_TIFService.FindByMappingArrayID(nMapID);
        //        var _GetMassTifform = _TM_Candidate_MassTIFService.FindByMappingArrayID(nMapID);
        //        if (_GetTifform.Any(a => a.submit_status == "Y"))
        //        {
        //            lstData = lstData.Where(w => !_GetTifform.Where(w2 => w2.submit_status == "Y").Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray().Contains(w.TM_PR_Candidate_Mapping.Id)).ToList();
        //        }
        //        if (_GetMassTifform.Any(a => a.submit_status == "Y"))
        //        {
        //            lstData = lstData.Where(w => !_GetMassTifform.Where(w2 => w2.submit_status == "Y").Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray().Contains(w.TM_PR_Candidate_Mapping.Id)).ToList();
        //        }
        //        lstData_resutl = (from lstAD in lstData
        //                          from lstTIF in _GetTifform.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.TM_PR_Candidate_Mapping.Id).DefaultIfEmpty(new TM_Candidate_TIF())
        //                          from lstMassTIF in _GetMassTifform.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.TM_PR_Candidate_Mapping.Id).DefaultIfEmpty(new TM_Candidate_MassTIF())
        //                          from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user+"").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
        //                          select new vInterview_obj
        //                          {
        //                              refno = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
        //                              group_name = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
        //                              position = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
        //                              rank = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
        //                              name_en = lstAD.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
        //                              status = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.submit_status != null ? (lstTIF.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting") : (lstMassTIF.submit_status != null ? (lstMassTIF.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting"),
        //                              create_user = lstAD.create_user,
        //                              create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
        //                              update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
        //                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
        //                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
        //                              pr_type = lstAD.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
        //                          }).ToList();
        //        result.lstData = lstData_resutl.ToList();
        //    }
        //    result.Status = SystemFunction.process_Success;
        //    return Json(new { result });
        //}
        [HttpPost]
        public ActionResult LoadApproveTIFFormList(CSearchInterview SearchItem)
        {
            vInterview_Return result = new vInterview_Return();
            bool isAdmin = CGlobal.UserIsAdmin();
            List<vInterview_obj> lstData_resutl = new List<vInterview_obj>();
            List<vInterview_obj> lstData_resutlMass = new List<vInterview_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            List<TM_Candidate_TIF_Approv> lstData = new List<TM_Candidate_TIF_Approv>();
            List<TM_Candidate_MassTIF_Approv> lstDataMass = new List<TM_Candidate_MassTIF_Approv>();

            if (SearchItem.tif_type + "" == "")
            {
                lstData = _TM_Candidate_TIF_ApprovService.GetTIFFormForApprove(
                                                  SearchItem.group_code, aDivisionPermission,
                                                 CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
                lstDataMass = _TM_Candidate_MassTIF_ApprovService.GetTIFFormForApprove(
                                      SearchItem.group_code, aDivisionPermission,
                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
            }
            else if (SearchItem.tif_type + "" == "N")
            {
                lstData = _TM_Candidate_TIF_ApprovService.GetTIFFormForApprove(
                                                  SearchItem.group_code, aDivisionPermission,
                                                 CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
            }
            else if (SearchItem.tif_type + "" == "M")
            {
                lstDataMass = _TM_Candidate_MassTIF_ApprovService.GetTIFFormForApprove(
                                      SearchItem.group_code, aDivisionPermission,
                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
            }


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

                string[] aApproveUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                lstData_resutl = (from lstAD in lstData.Where(w => w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id != 10)
                                  from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  select new vInterview_obj
                                  {
                                      refno = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
                                      group_name = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                      position = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
                                      rank = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                      name_en = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                      status = !string.IsNullOrEmpty(lstAD.Approve_status) ? (lstAD.Approve_status == "Y" ? "Confirmed" : "Reject") : "Waiting",
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      approval = AEmp.EmpFullName,
                                      pr_type = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                  }).ToList();


            }
            if (lstDataMass.Any())
            {

                string[] aApproveUser = lstDataMass.Select(s => s.Req_Approve_user).ToArray();
                string[] aUpdateUser = lstDataMass.Select(s => s.update_user).ToArray();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                lstData_resutlMass = (from lstAD in lstDataMass.Where(w => w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id != 10)
                                      from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vInterview_obj
                                      {
                                          refno = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                          name_en = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                          status = !string.IsNullOrEmpty(lstAD.Approve_status) ? (lstAD.Approve_status == "Y" ? "Confirmed" : "Reject") : "Waiting",

                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMass('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          approval = AEmp.EmpFullName,
                                          pr_type = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                      }).ToList();


            }
            result.lstData = lstData_resutl.Concat(lstData_resutlMass).ToList();
            result.Status = SystemFunction.process_Success;
            return Json(new { result });
        }

        [HttpPost]
        public ActionResult LoadApproveTIFFormListManage(CSearchInterview SearchItem)
        {
            vInterview_Return result = new vInterview_Return();
            bool isAdmin = true;
            List<vInterview_obj> lstData_resutl = new List<vInterview_obj>();
            List<vInterview_obj> lstData_resutlMass = new List<vInterview_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            List<TM_Candidate_TIF_Approv> lstData = new List<TM_Candidate_TIF_Approv>();
            List<TM_Candidate_MassTIF_Approv> lstDataMass = new List<TM_Candidate_MassTIF_Approv>();

            if (SearchItem.tif_type + "" == "")
            {
                lstData = _TM_Candidate_TIF_ApprovService.GetTIFFormForApprove(
                                                  SearchItem.group_code, aDivisionPermission,
                                                 CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
                lstDataMass = _TM_Candidate_MassTIF_ApprovService.GetTIFFormForApprove(
                                      SearchItem.group_code, aDivisionPermission,
                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
            }
            else if (SearchItem.tif_type + "" == "N")
            {
                lstData = _TM_Candidate_TIF_ApprovService.GetTIFFormForApprove(
                                                  SearchItem.group_code, aDivisionPermission,
                                                 CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
            }
            else if (SearchItem.tif_type + "" == "M")
            {
                lstDataMass = _TM_Candidate_MassTIF_ApprovService.GetTIFFormForApprove(
                                      SearchItem.group_code, aDivisionPermission,
                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
            }


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

                string[] aApproveUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                lstData_resutl = (from lstAD in lstData.Where(w => w.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id != 10)
                                  from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  select new vInterview_obj
                                  {
                                      refno = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
                                      group_name = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                      position = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
                                      rank = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                      name_en = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                      status = !string.IsNullOrEmpty(lstAD.Approve_status) ? (lstAD.Approve_status == "Y" ? "Confirmed" : "Reject") : "Waiting",
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      approval = AEmp.EmpFullName,
                                      pr_type = lstAD.TM_Candidate_TIF.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                  }).ToList();


            }
            if (lstDataMass.Any())
            {

                string[] aApproveUser = lstDataMass.Select(s => s.Req_Approve_user).ToArray();
                string[] aUpdateUser = lstDataMass.Select(s => s.update_user).ToArray();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                lstData_resutlMass = (from lstAD in lstDataMass.Where(w => w.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id != 10)
                                      from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vInterview_obj
                                      {
                                          refno = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                          name_en = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                          status = !string.IsNullOrEmpty(lstAD.Approve_status) ? (lstAD.Approve_status == "Y" ? "Confirmed" : "Reject") : "Waiting",

                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMass('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          approval = AEmp.EmpFullName,
                                          pr_type = lstAD.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                      }).ToList();


            }
            result.lstData = lstData_resutl.Concat(lstData_resutlMass).ToList();
            result.Status = SystemFunction.process_Success;
            return Json(new { result });
        }

        [HttpPost]
        public ActionResult CreateTIFForm(vInterview_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }

            if (ItemData != null && !string.IsNullOrEmpty(ItemData.user_no))
            {
                if (ItemData.confidentiality_agreement == "Y")
                {
                    DateTime dNow = DateTime.Now;
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                    int nRank = SystemFunction.GetIntNullToZero(ItemData.recommended_rank_id);
                    if (nId != 0)
                    {
                        var _getRank = _TM_Pool_RankService.Find(nRank);
                        if (_getRank == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Rank Not Found.";
                            return Json(new
                            {
                                result
                            });
                        }
                        var _getData = _TM_Candidate_Status_CycleService.Find(nId);
                        if (_getData != null)
                        {
                            //id null = first time Save data
                            var _CheckData = _TM_Candidate_TIFService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                            int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                            int nStatusId = SystemFunction.GetIntNullToZero(ItemData.tif_status_id);
                            var _getTifForm = _TM_TIF_FormService.Find(nTFId);
                            var _getStatus = _TM_TIF_StatusService.Find(nStatusId);
                            if (_getStatus != null)
                            {
                                if (_getTifForm != null && _getTifForm.TM_TIF_Form_Question != null && _getTifForm.TM_TIF_Form_Question.Any())
                                {
                                    var _getEmp = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && w.Employeeno == ItemData.user_no).FirstOrDefault();
                                    if (_getEmp != null && CGlobal.UserInfo.EmployeeNo != ItemData.user_no)
                                    {
                                        string[] aTIFQID = _getTifForm.TM_TIF_Form_Question.Select(s => s.Id + "").ToArray();
                                        List<TM_Candidate_TIF_Answer> lstAns = new List<TM_Candidate_TIF_Answer>();
                                        List<TM_Candidate_TIF_Approv> lstApprove = new List<TM_Candidate_TIF_Approv>();
                                        lstApprove.Add(new TM_Candidate_TIF_Approv()
                                        {
                                            Req_Approve_user = CGlobal.UserInfo.EmployeeNo,
                                            Approve_date = dNow,
                                            Approve_user = CGlobal.UserInfo.EmployeeNo,
                                            Approve_status = "Y",
                                            active_status = "Y",
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            seq = 1,

                                        });
                                        lstApprove.Add(new TM_Candidate_TIF_Approv()
                                        {
                                            Req_Approve_user = ItemData.user_no,
                                            active_status = "Y",
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            seq = 2,
                                        });
                                        // TM_Candidate_TIF_Approv objFirstApprove = new TM_Candidate_TIF_Approv()
                                        //{
                                        //    Req_Approve_user = CGlobal.UserInfo.EmployeeNo,
                                        //    Approve_date = dNow,
                                        //    Approve_user = CGlobal.UserInfo.EmployeeNo,
                                        //    Approve_status = "Y",
                                        //    active_status = "Y",
                                        //    update_user = CGlobal.UserInfo.UserId,
                                        //    update_date = dNow,
                                        //    create_user = CGlobal.UserInfo.UserId,
                                        //    create_date = dNow,
                                        //    seq = 1,

                                        //};
                                        //TM_Candidate_TIF_Approv objApprove = new TM_Candidate_TIF_Approv()
                                        //{
                                        //    Req_Approve_user = ItemData.user_no,
                                        //    active_status = "Y",
                                        //    update_user = CGlobal.UserInfo.UserId,
                                        //    update_date = dNow,
                                        //    create_user = CGlobal.UserInfo.UserId,
                                        //    create_date = dNow,
                                        //};
                                        if (ItemData.objtifform.lstQuestion != null)
                                        {
                                            lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                                      from lstQ in _getTifForm.TM_TIF_Form_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.TIFForm.TM_TIF_Form_Question())
                                                      select new TM_Candidate_TIF_Answer
                                                      {
                                                          update_user = CGlobal.UserInfo.UserId,
                                                          update_date = dNow,
                                                          create_user = CGlobal.UserInfo.UserId,
                                                          create_date = dNow,
                                                          answer = lstA.remark,
                                                          active_status = "Y",
                                                          TM_TIF_Form_Question = lstQ != null ? lstQ : null,
                                                          TM_TIF_Rating = _TM_TIF_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                                          TM_Candidate_TIF = _CheckData != null ? _CheckData : null,
                                                      }).ToList();
                                        }

                                        TM_Candidate_TIF objSave = new TM_Candidate_TIF()
                                        {
                                            TM_PR_Candidate_Mapping = _getData.TM_PR_Candidate_Mapping,
                                            comments = ItemData.comment,
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            active_status = "Y",
                                            TM_Candidate_TIF_Answer = lstAns,
                                            submit_status = "Y",
                                            TM_TIF_Status = _getStatus,
                                            TM_Candidate_TIF_Approv = lstApprove,
                                            confidentiality_agreement = ItemData.confidentiality_agreement,
                                            Recommended_Rank = _getRank != null ? _getRank : null,
                                        };
                                        //objSave.TM_Candidate_TIF_Approv.Add(objFirstApprove);
                                        //objSave.TM_Candidate_TIF_Approv.Add(objApprove);
                                        var sComplect = _TM_Candidate_TIFService.CreateNewOrUpdate(ref objSave);
                                        if (sComplect > 0)
                                        {
                                            if (_CheckData != null)
                                            {
                                                var sComplectAns = _TM_Candidate_TIF_AnswerService.UpdateAnswer(lstAns, _CheckData.Id, CGlobal.UserInfo.UserId, dNow);
                                                lstApprove.ForEach(ed => { ed.TM_Candidate_TIF = _CheckData; });
                                                var sComplectApprove = _TM_Candidate_TIF_ApprovService.CreateNewByList(lstApprove);
                                                if (sComplectAns < 0 || sComplectApprove < 0)
                                                {
                                                    if (sComplectAns < 0)
                                                    {
                                                        result.Status = SystemFunction.process_Failed;
                                                        result.Msg = "Error, Cannot Save Answer.";
                                                    }
                                                    else
                                                    {
                                                        result.Status = SystemFunction.process_Failed;
                                                        result.Msg = "Error, Cannot Save Approval.";
                                                    }
                                                    objSave.submit_status = "N";
                                                    _TM_Candidate_TIFService.CreateNewOrUpdate(ref objSave);
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Success;
                                                    string sError = "";
                                                    string mail_to_log = "";
                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Submit_TIFForm);
                                                    if (objSave != null)
                                                    {
                                                        var bSuss = SendFirstTIFFormSubmit(objSave, Mail1, ref sError, ref mail_to_log);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                result.Status = SystemFunction.process_Success;
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Submit_TIFForm);
                                                if (objSave != null)
                                                {
                                                    var bSuss = SendFirstTIFFormSubmit(objSave, Mail1, ref sError, ref mail_to_log);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Error, please try again.";
                                        }
                                    }
                                    else if (CGlobal.UserInfo.EmployeeNo == ItemData.user_no)
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, Approval Same first employee.";
                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, Approval Not Found.";
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, TIF Form Not Found.";
                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Status Not Found.";
                            }

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Candidate Not Found.";
                        }

                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Interviewer (Evaluator) must “tick” the box for acknowledgement confidential information from candidate's previous/current employer will not be disclosed to us during the interview session.";
                }


            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select Approval.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult SaveDraftTIFForm(vInterview_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
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
                int nRank = SystemFunction.GetIntNullToZero(ItemData.recommended_rank_id);
                if (nId != 0)
                {
                    var _getRank = _TM_Pool_RankService.Find(nRank);
                    var _getData = _TM_Candidate_Status_CycleService.Find(nId);
                    if (_getData != null)
                    {
                        //id null = first time Save data
                        var _CheckData = _TM_Candidate_TIFService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                        int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                        int nStatusId = SystemFunction.GetIntNullToZero(ItemData.tif_status_id);
                        var _getTifForm = _TM_TIF_FormService.Find(nTFId);
                        var _getStatus = _TM_TIF_StatusService.Find(nStatusId);
                        if (_getTifForm != null && _getTifForm.TM_TIF_Form_Question != null && _getTifForm.TM_TIF_Form_Question.Any())
                        {
                            string[] aTIFQID = _getTifForm.TM_TIF_Form_Question.Select(s => s.Id + "").ToArray();
                            List<TM_Candidate_TIF_Answer> lstAns = new List<TM_Candidate_TIF_Answer>();

                            if (ItemData.objtifform.lstQuestion != null)
                            {
                                lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                          from lstQ in _getTifForm.TM_TIF_Form_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.TIFForm.TM_TIF_Form_Question())
                                          select new TM_Candidate_TIF_Answer
                                          {
                                              update_user = CGlobal.UserInfo.UserId,
                                              update_date = dNow,
                                              create_user = CGlobal.UserInfo.UserId,
                                              create_date = dNow,
                                              answer = lstA.remark,
                                              active_status = "Y",
                                              TM_TIF_Form_Question = lstQ != null ? lstQ : null,
                                              TM_TIF_Rating = _TM_TIF_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                              TM_Candidate_TIF = _CheckData != null ? _CheckData : null,

                                          }).ToList();
                            }
                            lstAns = lstAns.Where(w => w.TM_TIF_Form_Question != null).ToList();
                            TM_Candidate_TIF objSave = new TM_Candidate_TIF()
                            {
                                TM_PR_Candidate_Mapping = _getData.TM_PR_Candidate_Mapping,
                                comments = ItemData.comment,
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                active_status = "Y",
                                TM_Candidate_TIF_Answer = lstAns,
                                submit_status = "N",
                                TM_TIF_Status = _getStatus,
                                confidentiality_agreement = ItemData.confidentiality_agreement,
                                Recommended_Rank = _getRank != null ? _getRank : null,

                            };
                            var sComplect = _TM_Candidate_TIFService.CreateNewOrUpdate(ref objSave);
                            if (sComplect > 0)
                            {
                                if (_CheckData != null)
                                {
                                    if (_TM_Candidate_TIF_AnswerService.UpdateAnswer(lstAns, _CheckData.Id, CGlobal.UserInfo.UserId, dNow) > 0)
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
                            result.Msg = "Error, TIF Form Not Found.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Candidate Not Found.";
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
        public ActionResult ApproveTIFForm(vInterview_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (ItemData != null && !string.IsNullOrEmpty(ItemData.approve_status))
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
                int nRank = SystemFunction.GetIntNullToZero(ItemData.recommended_rank_id);
                if (nId != 0)
                {
                    if (ItemData.confidentiality_agreement == "Y")
                    {
                        bool isAdmin = CGlobal.UserIsAdmin();
                        var _getRank = _TM_Pool_RankService.Find(nRank);
                        if (_getRank == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Rank Not Found.";
                            return Json(new
                            {
                                result
                            });
                        }
                        var _getData = _TM_Candidate_TIF_ApprovService.Find(nId);
                        if (_getData != null)
                        {
                            if (_getData.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                            {
                                int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                                int nStatusId = SystemFunction.GetIntNullToZero(ItemData.tif_status_id);
                                var _getTifForm = _TM_TIF_FormService.Find(nTFId);
                                var _getStatus = _TM_TIF_StatusService.Find(nStatusId);
                                if (_getStatus != null)
                                {

                                    if (_getTifForm != null && _getTifForm.TM_TIF_Form_Question != null && _getTifForm.TM_TIF_Form_Question.Any())
                                    {


                                        var _GetCandidateTIF = _TM_Candidate_TIFService.Find(_getData.TM_Candidate_TIF.Id);
                                        _getData.Approve_status = ItemData.approve_status;
                                        _getData.Approve_date = dNow;
                                        _getData.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                        _getData.update_user = CGlobal.UserInfo.UserId;
                                        _getData.update_date = dNow;
                                        List<TM_Candidate_TIF_Answer> lstAns = new List<TM_Candidate_TIF_Answer>();
                                        string[] aTIFQID = _getTifForm.TM_TIF_Form_Question.Select(s => s.Id + "").ToArray();
                                        if (ItemData.objtifform.lstQuestion != null)
                                        {
                                            lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                                      from lstQ in _getTifForm.TM_TIF_Form_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.TIFForm.TM_TIF_Form_Question())
                                                      select new TM_Candidate_TIF_Answer
                                                      {
                                                          update_user = CGlobal.UserInfo.UserId,
                                                          update_date = dNow,
                                                          create_user = CGlobal.UserInfo.UserId,
                                                          create_date = dNow,
                                                          answer = lstA.remark,
                                                          active_status = "Y",
                                                          TM_TIF_Form_Question = lstQ != null ? lstQ : null,
                                                          TM_TIF_Rating = _TM_TIF_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                                          TM_Candidate_TIF = _GetCandidateTIF,
                                                      }).ToList();

                                        }
                                        if (lstAns.Any(a => a.answer + "" == "" || a.TM_TIF_Rating == null))
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Warning, Please complete all ratings and supporting reasons before clicking \"CONFIRM\".";
                                            return Json(new
                                            {
                                                result
                                            });
                                        }
                                        else
                                        {
                                            var CheckAllApprove = _GetCandidateTIF.TM_Candidate_TIF_Approv.Where(w => w.seq < _getData.seq && (w.Approve_status + "" != "Y") && w.active_status == "Y").ToList();
                                            if (!CheckAllApprove.Any())
                                            {
                                                var ApproveComplect = _TM_Candidate_TIF_ApprovService.Update(_getData);
                                                if (ApproveComplect > 0)
                                                {
                                                    result.Status = SystemFunction.process_Success;

                                                    var _GetMap = _TM_PR_Candidate_MappingService.Find(_getData.TM_Candidate_TIF.TM_PR_Candidate_Mapping.Id);
                                                    if (_GetCandidateTIF != null && _GetMap != null)
                                                    {
                                                        var sComplectAns = 0;
                                                        _GetCandidateTIF.confidentiality_agreement = ItemData.confidentiality_agreement;
                                                        _GetCandidateTIF.Recommended_Rank = _getRank != null ? _getRank : null;
                                                        _GetCandidateTIF.comments = ItemData.comment;
                                                        _GetCandidateTIF.update_user = CGlobal.UserInfo.UserId;
                                                        _GetCandidateTIF.update_date = dNow;
                                                        _GetCandidateTIF.TM_TIF_Status = _getStatus;
                                                        _GetCandidateTIF.TM_PR_Candidate_Mapping = _GetMap;
                                                        var _SaveCandidateTIF = _TM_Candidate_TIFService.Update(_GetCandidateTIF);


                                                        sComplectAns = _TM_Candidate_TIF_AnswerService.UpdateAnswer(lstAns, _GetCandidateTIF.Id, CGlobal.UserInfo.UserId, dNow);
                                                        #region Send mail 
                                                        //check approved
                                                        var _getCheckTIF = _TM_Candidate_TIFService.Find(_getData.TM_Candidate_TIF.Id);
                                                        _getCheckTIF.TM_PR_Candidate_Mapping = _GetMap;
                                                        //var _GetMap = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                                                        if (_getCheckTIF != null && _GetMap != null)
                                                        {
                                                            var CheckCount = _getCheckTIF.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                                                            var CheckNotApprove = _getCheckTIF.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                                                            if (CheckNotApprove <= 0)
                                                            {
                                                                //to hr and complected
                                                                string sError = "";
                                                                string mail_to_log = "";
                                                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Approve_HR_Change);
                                                                if (Mail1 != null)
                                                                {
                                                                    var bSuss = SendTIFFormComplected(_getCheckTIF, Mail1, ref sError, ref mail_to_log);
                                                                }


                                                                string sError2 = "";
                                                                string mail_to_log2 = "";
                                                                var Mail2 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Send_to_HR);
                                                                if (Mail2 != null)
                                                                {
                                                                    var bSuss2 = SendTIFFormToHR(_getCheckTIF, Mail2, ref sError2, ref mail_to_log2);
                                                                }

                                                            }
                                                            else if (CheckNotApprove > 0)
                                                            {
                                                                //send submit
                                                                string sError = "";
                                                                string mail_to_log = "";
                                                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Submit_TIFForm);
                                                                if (Mail1 != null)
                                                                {
                                                                    var bSuss = SendFirstTIFFormSubmit(_getCheckTIF, Mail1, ref sError, ref mail_to_log);
                                                                }

                                                            }

                                                        }

                                                        #endregion
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
                                                result.Msg = "Error, Need befor step Confirmed.";
                                            }
                                        }


                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, TIF Form Not Found.";
                                    }

                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Status Not Found.";
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
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Interviewer (Evaluator) must “tick” the box for acknowledgement confidential information from candidate's previous/current employer will not be disclosed to us during the interview session.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select Approval.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult RollBackTIFForm(vInterview_obj_save ItemData)
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
                    bool isAdmin = CGlobal.UserIsAdmin();
                    var _getData = _TM_Candidate_TIF_ApprovService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                        {
                            var ApproveComplect = _TM_Candidate_TIF_ApprovService.RollBackStatus(_getData.TM_Candidate_TIF.Id, CGlobal.UserInfo.UserId, dNow);
                            if (ApproveComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Evaluator_Rollback);
                                var _getCheckTIF = _TM_Candidate_TIFService.Find(_getData.TM_Candidate_TIF.Id);
                                var _GetMap = _TM_PR_Candidate_MappingService.Find(_getCheckTIF.TM_PR_Candidate_Mapping.Id);
                                if (_getCheckTIF != null && _GetMap != null)
                                {
                                    int nSeq = SystemFunction.GetIntNullToZero(_getData.seq + "");
                                    _getCheckTIF.TM_PR_Candidate_Mapping = _GetMap;
                                    var bSuss = SendBuRollback(_getCheckTIF, Mail1, ItemData.rejeck, nSeq, ref sError, ref mail_to_log);
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
        [HttpPost]
        public ActionResult ApproveData(vInterview_obj_save ItemData)
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
                    bool isAdmin = CGlobal.UserIsAdmin();
                    var _getData = _TM_Candidate_TIF_ApprovService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                        {
                            var _getTIFForm = _TM_Candidate_TIFService.Find(_getData.TM_Candidate_TIF.Id);
                            if (_getTIFForm != null && _getTIFForm.TM_Candidate_TIF_Approv != null && _getTIFForm.TM_Candidate_TIF_Approv.Any())
                            {
                                if (!_getTIFForm.TM_Candidate_TIF_Answer.Any(a => a.active_status == "Y") || _getTIFForm.TM_Candidate_TIF_Answer.Any(a => a.answer + "" == "" && a.TM_TIF_Rating == null && a.active_status == "Y"))
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Warning, Please complete all ratings and supporting reasons before clicking \"CONFIRM\".";
                                    return Json(new
                                    {
                                        result
                                    });
                                }
                                else
                                {
                                    var CheckAllApprove = _getTIFForm.TM_Candidate_TIF_Approv.Where(w => w.seq < _getData.seq && (w.Approve_status + "" != "Y") && w.active_status == "Y").ToList();
                                    if (!CheckAllApprove.Any())
                                    {
                                        _getData.Approve_status = "Y";
                                        _getData.Approve_date = dNow;
                                        _getData.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                        _getData.update_user = CGlobal.UserInfo.UserId;
                                        var ApproveComplect = _TM_Candidate_TIF_ApprovService.Update(_getData);
                                        if (ApproveComplect > 0)
                                        {
                                            result.Status = SystemFunction.process_Success;
                                            #region Send mail 
                                            //check approved
                                            var _getCheckTIF = _TM_Candidate_TIFService.Find(_getData.TM_Candidate_TIF.Id);
                                            var _GetMap = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                                            if (_getCheckTIF != null && _GetMap != null)
                                            {
                                                _getCheckTIF.TM_PR_Candidate_Mapping = _GetMap;
                                                _getCheckTIF.update_user = CGlobal.UserInfo.UserId;
                                                _getCheckTIF.update_date = dNow;
                                                _TM_Candidate_TIFService.ApproveTIFForm(_getCheckTIF);
                                                var CheckCount = _getCheckTIF.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                                                var CheckNotApprove = _getCheckTIF.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                                                if (CheckNotApprove <= 0)
                                                {
                                                    //to hr and complected
                                                    string sError = "";
                                                    string mail_to_log = "";
                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Approve);
                                                    if (Mail1 != null)
                                                    {
                                                        var bSuss = SendTIFFormComplected(_getCheckTIF, Mail1, ref sError, ref mail_to_log);
                                                    }


                                                    string sError2 = "";
                                                    string mail_to_log2 = "";
                                                    var Mail2 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Send_to_HR);
                                                    if (Mail2 != null)
                                                    {
                                                        var bSuss2 = SendTIFFormToHR(_getCheckTIF, Mail2, ref sError2, ref mail_to_log2);
                                                    }

                                                }
                                                else if (CheckNotApprove > 0)
                                                {
                                                    //send submit
                                                    string sError = "";
                                                    string mail_to_log = "";
                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Submit_TIFForm);
                                                    if (Mail1 != null)
                                                    {
                                                        var bSuss = SendFirstTIFFormSubmit(_getCheckTIF, Mail1, ref sError, ref mail_to_log);
                                                    }

                                                }

                                            }

                                            #endregion
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
                                        result.Msg = "Error, Need befor step Confirmed.";
                                    }
                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, TIF Form not found.";
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
        //for mass TIF
        [HttpPost]
        public ActionResult CreateMassTIFForm(vInterview_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();

            if (ItemData != null && !string.IsNullOrEmpty(ItemData.user_no))
            {
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                if (ItemData.confidentiality_agreement == "Y")
                {
                    DateTime dNow = DateTime.Now;
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                    int nRank = SystemFunction.GetIntNullToZero(ItemData.recommended_rank_id);
                    if (nId != 0)
                    {
                        var _getRank = _TM_Pool_RankService.Find(nRank);
                        if (_getRank == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Rank Not Found.";
                            return Json(new
                            {
                                result
                            });
                        }
                        var _getData = _TM_Candidate_Status_CycleService.Find(nId);
                        if (_getData != null)
                        {
                            DateTime? dStart = null;
                            if (!string.IsNullOrEmpty(ItemData.target_start))
                            {
                                dStart = SystemFunction.ConvertStringToDateTime(ItemData.target_start, "");
                            }
                            //id null = first time Save data
                            var _CheckData = _TM_Candidate_MassTIFService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                            int nTFId = SystemFunction.GetIntNullToZero(ItemData.objMasstifform.TIF_no);
                            int nStatusId = SystemFunction.GetIntNullToZero(ItemData.tif_status_id);
                            var _getTifForm = _TM_Mass_TIF_FormService.Find(nTFId);
                            var _getStatus = _TM_MassTIF_StatusService.Find(nStatusId);
                            if (_getStatus != null)
                            {
                                if (_getTifForm != null && _getTifForm.TM_MassTIF_Form_Question != null && _getTifForm.TM_MassTIF_Form_Question.Any() && _getTifForm.TM_Mass_Auditing_Question != null && _getTifForm.TM_Mass_Auditing_Question.Any())
                                {
                                    var _getEmp = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && w.Employeeno == ItemData.user_no).FirstOrDefault();
                                    if (_getEmp != null && CGlobal.UserInfo.EmployeeNo != ItemData.user_no)
                                    {

                                        List<TM_Candidate_MassTIF_Approv> lstAns = new List<TM_Candidate_MassTIF_Approv>();
                                        List<TM_Candidate_MassTIF_Approv> lstApprove = new List<TM_Candidate_MassTIF_Approv>();
                                        lstApprove.Add(new TM_Candidate_MassTIF_Approv()
                                        {
                                            Req_Approve_user = CGlobal.UserInfo.EmployeeNo,
                                            Approve_date = dNow,
                                            Approve_user = CGlobal.UserInfo.EmployeeNo,
                                            Approve_status = "Y",
                                            active_status = "Y",
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            seq = 1,

                                        });
                                        lstApprove.Add(new TM_Candidate_MassTIF_Approv()
                                        {
                                            Req_Approve_user = ItemData.user_no,
                                            active_status = "Y",
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            seq = 2,
                                        });
                                        string[] aTIFQID = _getTifForm.TM_MassTIF_Form_Question.Select(s => s.Id + "").ToArray();
                                        List<TM_Candidate_MassTIF_Core> lstCore = new List<TM_Candidate_MassTIF_Core>();
                                        if (ItemData.objMasstifform.lstQuestion != null)
                                        {
                                            lstCore = (from lstA in ItemData.objMasstifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                                       from lstQ in _getTifForm.TM_MassTIF_Form_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.TIFForm.TM_MassTIF_Form_Question())

                                                       select new TM_Candidate_MassTIF_Core
                                                       {
                                                           update_user = CGlobal.UserInfo.UserId,
                                                           update_date = dNow,
                                                           create_user = CGlobal.UserInfo.UserId,
                                                           create_date = dNow,
                                                           evidence = lstA.evidence,
                                                           active_status = "Y",
                                                           TM_MassTIF_Form_Question = lstQ != null ? lstQ : null,
                                                           TM_Mass_Scoring = _TM_Mass_ScoringService.FindByCode(lstA.scoring),
                                                           TM_Candidate_MassTIF = _CheckData != null ? _CheckData : null,
                                                           TM_TIF_Rating_Id = lstA.rating != null ? Convert.ToInt32(lstA.rating) : default(int?),
                                                       }).ToList();
                                        }
                                        lstCore = lstCore.Where(w => w.TM_MassTIF_Form_Question != null).ToList();

                                        string[] aTIFAudID = _getTifForm.TM_Mass_Auditing_Question.Select(s => s.Id + "").ToArray();
                                        List<TM_Candidate_MassTIF_Audit_Qst> lstAud_Qst = new List<TM_Candidate_MassTIF_Audit_Qst>();
                                        if (ItemData.objMasstifform.lstAuditing != null)
                                        {
                                            lstAud_Qst = (from lstA in ItemData.objMasstifform.lstAuditing.Where(w => aTIFAudID.Contains(w.Qst))
                                                          from lstQ in _getTifForm.TM_Mass_Auditing_Question.Where(w => w.Id + "" == lstA.Qst).DefaultIfEmpty(new Models.TIFForm.TM_Mass_Auditing_Question())
                                                          select new TM_Candidate_MassTIF_Audit_Qst
                                                          {
                                                              update_user = CGlobal.UserInfo.UserId,
                                                              update_date = dNow,
                                                              create_user = CGlobal.UserInfo.UserId,
                                                              create_date = dNow,
                                                              answer = lstA.answer,
                                                              active_status = "Y",
                                                              TM_Mass_Auditing_Question = lstQ != null ? lstQ : null,
                                                              TM_Mass_Scoring = _TM_Mass_ScoringService.Find(SystemFunction.GetIntNullToZero(lstA.scoring)),
                                                              TM_Candidate_MassTIF = _CheckData != null ? _CheckData : null,
                                                              seq = SystemFunction.GetIntNullToZero(lstA.id),
                                                          }).ToList();
                                        }
                                        lstAud_Qst = lstAud_Qst.Where(w => w.TM_Mass_Auditing_Question != null).ToList();


                                        List<TM_Candidate_MassTIF_Additional> lstAdInfo = new List<TM_Candidate_MassTIF_Additional>();
                                        var GetAdditional = _TM_Additional_InformationService.GetActiveTIFForm();
                                        if (ItemData.objMasstifform.lstAdInfo != null && GetAdditional != null && GetAdditional.TM_Additional_Questions != null)
                                        {

                                            string[] aAdinfo = GetAdditional.TM_Additional_Questions.Select(s => s.Id + "").ToArray();

                                            lstAdInfo = (from lstA in ItemData.objMasstifform.lstAdInfo.Where(w => aAdinfo.Contains(w.nID))
                                                         from lstQ in GetAdditional.TM_Additional_Questions.Where(w => w.Id + "" == lstA.nID).DefaultIfEmpty(new Models.TIFForm.TM_Additional_Questions())
                                                         select new TM_Candidate_MassTIF_Additional
                                                         {
                                                             update_user = CGlobal.UserInfo.UserId,
                                                             update_date = dNow,
                                                             create_user = CGlobal.UserInfo.UserId,
                                                             create_date = dNow,
                                                             active_status = "Y",
                                                             TM_Additional_Questions = lstQ != null ? lstQ : null,
                                                             TM_Candidate_MassTIF = _CheckData != null ? _CheckData : null,
                                                             other_answer = lstA.other_answer,
                                                             TM_Candidate_MassTIF_Adnl_Answer = new List<TM_Candidate_MassTIF_Adnl_Answer>(),
                                                         }).ToList();
                                            lstAdInfo = lstAdInfo.Where(w => w.TM_Additional_Questions != null).ToList();

                                            lstAdInfo.ForEach(ed =>
                                            {
                                                var CheckAnswer = ItemData.objMasstifform.lstAdInfo.Where(w => w.nID == ed.TM_Additional_Questions.Id + "").FirstOrDefault();
                                                if (CheckAnswer != null && CheckAnswer.lstAnswersselect != null && CheckAnswer.lstAnswersselect.Length > 0)
                                                {
                                                    foreach (var ans in CheckAnswer.lstAnswersselect)
                                                    {
                                                        int nAnsId = SystemFunction.GetIntNullToZero(ans);
                                                        var _getAns = _TM_Additional_AnswersService.Find(nAnsId);
                                                        if (_getAns != null)
                                                        {
                                                            ed.TM_Candidate_MassTIF_Adnl_Answer.Add(new TM_Candidate_MassTIF_Adnl_Answer()
                                                            {
                                                                update_user = CGlobal.UserInfo.UserId,
                                                                update_date = dNow,
                                                                create_user = CGlobal.UserInfo.UserId,
                                                                create_date = dNow,
                                                                active_status = "Y",
                                                                TM_Additional_Answers = _getAns,
                                                                TM_Candidate_MassTIF_Additional = _CheckData != null ? _TM_Candidate_MassTIF_AdditionalService.FindForSave(_CheckData.Id, ed.TM_Additional_Questions.Id) : null,
                                                            });
                                                        }
                                                    }
                                                }

                                            });
                                        }

                                        TM_Candidate_MassTIF objSave = new TM_Candidate_MassTIF()
                                        {
                                            TM_PR_Candidate_Mapping = _getData.TM_PR_Candidate_Mapping,
                                            comments = ItemData.comment,
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            active_status = "Y",
                                            TM_Candidate_MassTIF_Core = lstCore,
                                            TM_Candidate_MassTIF_Audit_Qst = lstAud_Qst,
                                            submit_status = "Y",
                                            TM_MassTIF_Status = _getStatus,
                                            TM_Mass_Question_Type = _TM_Mass_Question_TypeService.Find(SystemFunction.GetIntNullToZero(ItemData.set_type + "")),
                                            TM_Candidate_MassTIF_Approv = lstApprove,
                                            confidentiality_agreement = ItemData.confidentiality_agreement,
                                            Recommended_Rank = _getRank != null ? _getRank : null,
                                            TM_Candidate_MassTIF_Additional = lstAdInfo,
                                            can_start_date = dStart,
                                        };


                                        var sComplect = _TM_Candidate_MassTIFService.CreateNewOrUpdate(objSave);
                                        if (sComplect > 0)
                                        {
                                            if (_CheckData != null)
                                            {
                                                var SaveCore = _TM_Candidate_MassTIF_CoreService.UpdateAnswer(lstCore, _CheckData.Id, CGlobal.UserInfo.UserId, dNow);
                                                var SaveAud = _TM_Candidate_MassTIF_Audit_QstService.UpdateAnswer(lstAud_Qst, _CheckData.Id, CGlobal.UserInfo.UserId, dNow);
                                                var SaveAdIn = -1;
                                                if (_TM_Candidate_MassTIF_AdditionalService.UpdateAnswer(lstAdInfo, _CheckData.Id, CGlobal.UserInfo.UserId, dNow) >= 0)
                                                {
                                                    result.Status = SystemFunction.process_Success;
                                                    int[] aQusID = lstAdInfo.Select(s => s.TM_Additional_Questions.Id).ToArray();
                                                    var _GetAdInfolst = _TM_Candidate_MassTIF_AdditionalService.GetListForUpdateAns(_CheckData.Id, aQusID);
                                                    if (_GetAdInfolst != null)
                                                    {
                                                        foreach (var Adin in _GetAdInfolst)
                                                        {
                                                            var _getAnss = lstAdInfo.Where(w => w.TM_Additional_Questions.Id == Adin.TM_Additional_Questions.Id).FirstOrDefault();
                                                            if (_getAnss != null)
                                                            {
                                                                _TM_Candidate_MassTIF_Adnl_AnswerService.UpdateLstAns(_getAnss.TM_Candidate_MassTIF_Adnl_Answer.ToList(), Adin.Id, CGlobal.UserInfo.UserId, dNow);

                                                            }
                                                        }
                                                        SaveAdIn = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Failed;
                                                    result.Msg = "Error, please try again.";
                                                }
                                                lstApprove.ForEach(ed => { ed.TM_Candidate_MassTIF = _CheckData; });
                                                var sComplectApprove = _TM_Candidate_MassTIF_ApprovService.CreateNewByList(lstApprove);
                                                if (SaveCore < 0 || sComplectApprove < 0 || SaveAud < 0 || SaveAdIn < 0)
                                                {
                                                    if (SaveCore < 0)
                                                    {
                                                        result.Status = SystemFunction.process_Failed;
                                                        result.Msg = "Error, Cannot Save Core Competencies.";
                                                    }
                                                    else if (sComplectApprove < 0)
                                                    {
                                                        result.Status = SystemFunction.process_Failed;
                                                        result.Msg = "Error, Cannot Save Approval.";
                                                    }
                                                    else if (SaveAud < 0)
                                                    {
                                                        result.Status = SystemFunction.process_Failed;
                                                        result.Msg = "Error, Cannot Save Auditing Questions.";
                                                    }
                                                    else if (SaveAdIn < 0)
                                                    {
                                                        result.Status = SystemFunction.process_Failed;
                                                        result.Msg = "Error, Cannot Save Additional Information.";
                                                    }
                                                    objSave.submit_status = "N";
                                                    _TM_Candidate_MassTIFService.CreateNewOrUpdate(objSave);
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Success;
                                                    string sError = "";
                                                    string mail_to_log = "";
                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Submit_TIFForm);
                                                    if (objSave != null)
                                                    {
                                                        var bSuss = SendFirstMassTIFFormSubmit(objSave, Mail1, ref sError, ref mail_to_log);
                                                    }
                                                }


                                                if (_TM_Candidate_MassTIF_CoreService.UpdateAnswer(lstCore, _CheckData.Id, CGlobal.UserInfo.UserId, dNow) >= 0)
                                                {
                                                    result.Status = SystemFunction.process_Success;
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Failed;
                                                    result.Msg = "Error, please try again.";
                                                }
                                                if (_TM_Candidate_MassTIF_Audit_QstService.UpdateAnswer(lstAud_Qst, _CheckData.Id, CGlobal.UserInfo.UserId, dNow) >= 0)
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
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Submit_TIFForm);
                                                if (objSave != null)
                                                {
                                                    var bSuss = SendFirstMassTIFFormSubmit(objSave, Mail1, ref sError, ref mail_to_log);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Error, please try again.";
                                        }


                                    }
                                    else if (CGlobal.UserInfo.EmployeeNo == ItemData.user_no)
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, Approval Same first employee.";
                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, Approval Not Found.";
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, TIF Form Not Found.";
                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Status Not Found.";
                            }

                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Candidate Not Found.";
                        }

                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Interviewer (Evaluator) must “tick” the box for acknowledgement confidential information from candidate's previous/current employer will not be disclosed to us during the interview session.";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select Approval.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult SaveDraftMassTIFForm(vInterview_obj_save ItemData)
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
                int nRank = SystemFunction.GetIntNullToZero(ItemData.recommended_rank_id);
                if (nId != 0)
                {

                    var _getData = _TM_Candidate_Status_CycleService.Find(nId);
                    var _getRank = _TM_Pool_RankService.Find(nRank);
                    if (_getData != null)
                    {
                        DateTime? dStart = null;
                        if (!string.IsNullOrEmpty(ItemData.target_start))
                        {
                            dStart = SystemFunction.ConvertStringToDateTime(ItemData.target_start, "");
                        }
                        //id null = first time Save data
                        var _CheckData = _TM_Candidate_MassTIFService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                        int nTFId = SystemFunction.GetIntNullToZero(ItemData.objMasstifform.TIF_no);
                        int nStatusId = SystemFunction.GetIntNullToZero(ItemData.tif_status_id);
                        var _getTifForm = _TM_Mass_TIF_FormService.Find(nTFId);
                        var _getStatus = _TM_MassTIF_StatusService.Find(nStatusId);
                        if (_getTifForm != null && _getTifForm.TM_MassTIF_Form_Question != null && _getTifForm.TM_MassTIF_Form_Question.Any() && _getTifForm.TM_Mass_Auditing_Question != null && _getTifForm.TM_Mass_Auditing_Question.Any())
                        {
                            string[] aTIFQID = _getTifForm.TM_MassTIF_Form_Question.Select(s => s.Id + "").ToArray();
                            List<TM_Candidate_MassTIF_Core> lstCore = new List<TM_Candidate_MassTIF_Core>();
                            if (ItemData.objMasstifform.lstQuestion != null)
                            {
                                lstCore = (from lstA in ItemData.objMasstifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                           from lstQ in _getTifForm.TM_MassTIF_Form_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.TIFForm.TM_MassTIF_Form_Question())
                                           select new TM_Candidate_MassTIF_Core
                                           {
                                               update_user = CGlobal.UserInfo.UserId,
                                               update_date = dNow,
                                               create_user = CGlobal.UserInfo.UserId,
                                               create_date = dNow,
                                               evidence = lstA.evidence,
                                               active_status = "Y",
                                               TM_MassTIF_Form_Question = lstQ != null ? lstQ : null,

                                               TM_Mass_Scoring = _TM_Mass_ScoringService.FindByCode(lstA.scoring),
                                               TM_Candidate_MassTIF = _CheckData != null ? _CheckData : null,
                                               TM_TIF_Rating_Id = lstA.rating != null ? Convert.ToInt32(lstA.rating) : default(int?),
                                           }).ToList();
                            }
                            lstCore = lstCore.Where(w => w.TM_MassTIF_Form_Question != null).ToList();

                            string[] aTIFAudID = _getTifForm.TM_Mass_Auditing_Question.Select(s => s.Id + "").ToArray();
                            List<TM_Candidate_MassTIF_Audit_Qst> lstAud_Qst = new List<TM_Candidate_MassTIF_Audit_Qst>();
                            if (ItemData.objMasstifform.lstAuditing != null)
                            {
                                lstAud_Qst = (from lstA in ItemData.objMasstifform.lstAuditing.Where(w => aTIFAudID.Contains(w.Qst))
                                              from lstQ in _getTifForm.TM_Mass_Auditing_Question.Where(w => w.Id + "" == lstA.Qst).DefaultIfEmpty(new Models.TIFForm.TM_Mass_Auditing_Question())
                                              select new TM_Candidate_MassTIF_Audit_Qst
                                              {
                                                  update_user = CGlobal.UserInfo.UserId,
                                                  update_date = dNow,
                                                  create_user = CGlobal.UserInfo.UserId,
                                                  create_date = dNow,
                                                  answer = lstA.answer,
                                                  active_status = "Y",
                                                  TM_Mass_Auditing_Question = lstQ != null ? lstQ : null,
                                                  TM_Mass_Scoring = _TM_Mass_ScoringService.Find(SystemFunction.GetIntNullToZero(lstA.scoring)),
                                                  TM_Candidate_MassTIF = _CheckData != null ? _CheckData : null,
                                                  seq = SystemFunction.GetIntNullToZero(lstA.id),
                                              }).ToList();
                            }
                            lstAud_Qst = lstAud_Qst.Where(w => w.TM_Mass_Auditing_Question != null).ToList();
                            List<TM_Candidate_MassTIF_Additional> lstAdInfo = new List<TM_Candidate_MassTIF_Additional>();
                            var GetAdditional = _TM_Additional_InformationService.GetActiveTIFForm();
                            if (ItemData.objMasstifform.lstAdInfo != null && GetAdditional != null && GetAdditional.TM_Additional_Questions != null)
                            {

                                string[] aAdinfo = GetAdditional.TM_Additional_Questions.Select(s => s.Id + "").ToArray();

                                lstAdInfo = (from lstA in ItemData.objMasstifform.lstAdInfo.Where(w => aAdinfo.Contains(w.nID))
                                             from lstQ in GetAdditional.TM_Additional_Questions.Where(w => w.Id + "" == lstA.nID).DefaultIfEmpty(new Models.TIFForm.TM_Additional_Questions())
                                             select new TM_Candidate_MassTIF_Additional
                                             {
                                                 update_user = CGlobal.UserInfo.UserId,
                                                 update_date = dNow,
                                                 create_user = CGlobal.UserInfo.UserId,
                                                 create_date = dNow,
                                                 active_status = "Y",
                                                 TM_Additional_Questions = lstQ != null ? lstQ : null,
                                                 TM_Candidate_MassTIF = _CheckData != null ? _CheckData : null,
                                                 other_answer = lstA.other_answer,
                                                 TM_Candidate_MassTIF_Adnl_Answer = new List<TM_Candidate_MassTIF_Adnl_Answer>(),
                                             }).ToList();
                                lstAdInfo = lstAdInfo.Where(w => w.TM_Additional_Questions != null).ToList();

                                lstAdInfo.ForEach(ed =>
                                {
                                    var CheckAnswer = ItemData.objMasstifform.lstAdInfo.Where(w => w.nID == ed.TM_Additional_Questions.Id + "").FirstOrDefault();
                                    if (CheckAnswer != null && CheckAnswer.lstAnswersselect != null && CheckAnswer.lstAnswersselect.Length > 0)
                                    {
                                        foreach (var ans in CheckAnswer.lstAnswersselect)
                                        {
                                            int nAnsId = SystemFunction.GetIntNullToZero(ans);
                                            var _getAns = _TM_Additional_AnswersService.Find(nAnsId);
                                            if (_getAns != null)
                                            {
                                                ed.TM_Candidate_MassTIF_Adnl_Answer.Add(new TM_Candidate_MassTIF_Adnl_Answer()
                                                {
                                                    update_user = CGlobal.UserInfo.UserId,
                                                    update_date = dNow,
                                                    create_user = CGlobal.UserInfo.UserId,
                                                    create_date = dNow,
                                                    active_status = "Y",
                                                    TM_Additional_Answers = _getAns,
                                                    TM_Candidate_MassTIF_Additional = _CheckData != null ? _TM_Candidate_MassTIF_AdditionalService.FindForSave(_CheckData.Id, ed.TM_Additional_Questions.Id) : null,
                                                });
                                            }
                                        }
                                    }

                                });


                            }


                            TM_Candidate_MassTIF objSave = new TM_Candidate_MassTIF()
                            {
                                TM_PR_Candidate_Mapping = _getData.TM_PR_Candidate_Mapping,
                                comments = ItemData.comment,
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                active_status = "Y",
                                TM_Candidate_MassTIF_Core = lstCore,
                                TM_Candidate_MassTIF_Audit_Qst = lstAud_Qst,
                                submit_status = "N",
                                TM_MassTIF_Status = _getStatus,
                                TM_Mass_Question_Type = _TM_Mass_Question_TypeService.Find(SystemFunction.GetIntNullToZero(ItemData.set_type + "")),
                                confidentiality_agreement = ItemData.confidentiality_agreement,
                                Recommended_Rank = _getRank != null ? _getRank : null,
                                TM_Candidate_MassTIF_Additional = lstAdInfo,

                                can_start_date = dStart,
                            };
                            var sComplect = _TM_Candidate_MassTIFService.CreateNewOrUpdate(objSave);
                            if (sComplect > 0)
                            {
                                if (_CheckData != null)
                                {
                                    if (_TM_Candidate_MassTIF_CoreService.UpdateAnswer(lstCore, _CheckData.Id, CGlobal.UserInfo.UserId, dNow) >= 0)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, please try again.";
                                    }


                                    if (_TM_Candidate_MassTIF_Audit_QstService.UpdateAnswer(lstAud_Qst, _CheckData.Id, CGlobal.UserInfo.UserId, dNow) >= 0)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, please try again.";
                                    }

                                    if (_TM_Candidate_MassTIF_AdditionalService.UpdateAnswer(lstAdInfo, _CheckData.Id, CGlobal.UserInfo.UserId, dNow) >= 0)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                        int[] aQusID = lstAdInfo.Select(s => s.TM_Additional_Questions.Id).ToArray();
                                        var _GetAdInfolst = _TM_Candidate_MassTIF_AdditionalService.GetListForUpdateAns(_CheckData.Id, aQusID);
                                        if (_GetAdInfolst != null)
                                        {
                                            foreach (var Adin in _GetAdInfolst)
                                            {
                                                var _getAnss = lstAdInfo.Where(w => w.TM_Additional_Questions.Id == Adin.TM_Additional_Questions.Id).FirstOrDefault();
                                                if (_getAnss != null)
                                                {
                                                    _TM_Candidate_MassTIF_Adnl_AnswerService.UpdateLstAns(_getAnss.TM_Candidate_MassTIF_Adnl_Answer.ToList(), Adin.Id, CGlobal.UserInfo.UserId, dNow);

                                                }
                                                //if (_TM_Candidate_MassTIF_Adnl_AnswerService.UpdateAnswer(Adin.TM_Candidate_MassTIF_Adnl_Answer.ToList(), _CheckData.Id, Adin.TM_Additional_Questions.Id, CGlobal.UserInfo.UserId, dNow) >= 0)
                                                //{
                                                //    result.Status = SystemFunction.process_Success;
                                                //}
                                                //else
                                                //{
                                                //    result.Status = SystemFunction.process_Failed;
                                                //    result.Msg = "Error, please try again.";
                                                //}
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
                            result.Msg = "Error, TIF Form Not Found.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Candidate Not Found.";
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
        public ActionResult ApproveMassTIFForm(vInterview_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (ItemData != null && !string.IsNullOrEmpty(ItemData.approve_status))
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
                int nRank = SystemFunction.GetIntNullToZero(ItemData.recommended_rank_id);
                if (nId != 0)
                {
                    if (ItemData.confidentiality_agreement == "Y")
                    {
                        bool isAdmin = CGlobal.UserIsAdmin();
                        var _getRank = _TM_Pool_RankService.Find(nRank);
                        var _getData = _TM_Candidate_MassTIF_ApprovService.Find(nId);

                        if (_getRank == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Rank Not Found.";
                            return Json(new
                            {
                                result
                            });
                        }
                        if (_getData != null)
                        {
                            if (_getData.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                            {

                                DateTime? dStart = null;
                                if (!string.IsNullOrEmpty(ItemData.target_start))
                                {
                                    dStart = SystemFunction.ConvertStringToDateTime(ItemData.target_start, "");
                                }

                                int nTFId = SystemFunction.GetIntNullToZero(ItemData.objMasstifform.TIF_no);
                                int nStatusId = SystemFunction.GetIntNullToZero(ItemData.tif_status_id);
                                var _getTifForm = _TM_Mass_TIF_FormService.Find(nTFId);
                                var _getStatus = _TM_MassTIF_StatusService.Find(nStatusId);
                                if (_getStatus != null)
                                {
                                    if (_getTifForm != null && _getTifForm.TM_MassTIF_Form_Question != null && _getTifForm.TM_MassTIF_Form_Question.Any() && _getTifForm.TM_Mass_Auditing_Question != null && _getTifForm.TM_Mass_Auditing_Question.Any())
                                    {
                                        var _GetCandidateTIF = _TM_Candidate_MassTIFService.Find(_getData.TM_Candidate_MassTIF.Id);
                                        _getData.Approve_status = ItemData.approve_status;
                                        _getData.Approve_date = dNow;
                                        _getData.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                        _getData.update_user = CGlobal.UserInfo.UserId;
                                        _getData.update_date = dNow;

                                        var CheckAllApprove = _GetCandidateTIF.TM_Candidate_MassTIF_Approv.Where(w => w.seq < _getData.seq && (w.Approve_status + "" != "Y") && w.active_status == "Y").ToList();
                                        if (!CheckAllApprove.Any())
                                        {

                                            var ApproveComplect = _TM_Candidate_MassTIF_ApprovService.Update(_getData);
                                            if (ApproveComplect > 0)
                                            {
                                                result.Status = SystemFunction.process_Success;

                                                var _GetMap = _TM_PR_Candidate_MappingService.Find(_getData.TM_Candidate_MassTIF.TM_PR_Candidate_Mapping.Id);

                                                if (_GetCandidateTIF != null && _GetMap != null)
                                                {
                                                    _GetCandidateTIF.confidentiality_agreement = ItemData.confidentiality_agreement;
                                                    _GetCandidateTIF.Recommended_Rank = _getRank != null ? _getRank : null;
                                                    _GetCandidateTIF.comments = ItemData.comment;
                                                    _GetCandidateTIF.update_user = CGlobal.UserInfo.UserId;
                                                    _GetCandidateTIF.update_date = dNow;
                                                    _GetCandidateTIF.TM_MassTIF_Status = _getStatus;
                                                    _GetCandidateTIF.TM_PR_Candidate_Mapping = _GetMap;
                                                    _GetCandidateTIF.can_start_date = dStart;
                                                    var _SaveCandidateTIF = _TM_Candidate_MassTIFService.Update(_GetCandidateTIF);

                                                    string[] aTIFQID = _getTifForm.TM_MassTIF_Form_Question.Select(s => s.Id + "").ToArray();
                                                    List<TM_Candidate_MassTIF_Core> lstCore = new List<TM_Candidate_MassTIF_Core>();
                                                    if (ItemData.objMasstifform.lstQuestion != null)
                                                    {
                                                        lstCore = (from lstA in ItemData.objMasstifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                                                   from lstQ in _getTifForm.TM_MassTIF_Form_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.TIFForm.TM_MassTIF_Form_Question())
                                                                   select new TM_Candidate_MassTIF_Core
                                                                   {
                                                                       update_user = CGlobal.UserInfo.UserId,
                                                                       update_date = dNow,
                                                                       create_user = CGlobal.UserInfo.UserId,
                                                                       create_date = dNow,
                                                                       evidence = lstA.evidence,
                                                                       active_status = "Y",
                                                                       TM_MassTIF_Form_Question = lstQ != null ? lstQ : null,
                                                                       TM_Mass_Scoring = _TM_Mass_ScoringService.FindByCode(lstA.scoring),
                                                                       TM_Candidate_MassTIF = _GetCandidateTIF,
                                                                       TM_TIF_Rating_Id = lstA.rating != null ? Convert.ToInt32(lstA.rating) : default(int?),
                                                                   }).ToList();
                                                    }
                                                    lstCore = lstCore.Where(w => w.TM_MassTIF_Form_Question != null).ToList();

                                                    string[] aTIFAudID = _getTifForm.TM_Mass_Auditing_Question.Select(s => s.Id + "").ToArray();
                                                    List<TM_Candidate_MassTIF_Audit_Qst> lstAud_Qst = new List<TM_Candidate_MassTIF_Audit_Qst>();
                                                    if (ItemData.objMasstifform.lstAuditing != null)
                                                    {
                                                        lstAud_Qst = (from lstA in ItemData.objMasstifform.lstAuditing.Where(w => aTIFAudID.Contains(w.Qst))
                                                                      from lstQ in _getTifForm.TM_Mass_Auditing_Question.Where(w => w.Id + "" == lstA.Qst).DefaultIfEmpty(new Models.TIFForm.TM_Mass_Auditing_Question())
                                                                      select new TM_Candidate_MassTIF_Audit_Qst
                                                                      {
                                                                          update_user = CGlobal.UserInfo.UserId,
                                                                          update_date = dNow,
                                                                          create_user = CGlobal.UserInfo.UserId,
                                                                          create_date = dNow,
                                                                          answer = lstA.answer,
                                                                          active_status = "Y",
                                                                          TM_Mass_Auditing_Question = lstQ != null ? lstQ : null,
                                                                          TM_Mass_Scoring = _TM_Mass_ScoringService.Find(SystemFunction.GetIntNullToZero(lstA.scoring)),
                                                                          TM_Candidate_MassTIF = _GetCandidateTIF,
                                                                          seq = lstA.nSeq,
                                                                      }).ToList();
                                                    }
                                                    lstAud_Qst = lstAud_Qst.Where(w => w.TM_Mass_Auditing_Question != null).ToList();

                                                    List<TM_Candidate_MassTIF_Additional> lstAdInfo = new List<TM_Candidate_MassTIF_Additional>();
                                                    var GetAdditional = _TM_Additional_InformationService.GetActiveTIFForm();
                                                    if (ItemData.objMasstifform.lstAdInfo != null && GetAdditional != null && GetAdditional.TM_Additional_Questions != null)
                                                    {

                                                        string[] aAdinfo = GetAdditional.TM_Additional_Questions.Select(s => s.Id + "").ToArray();

                                                        lstAdInfo = (from lstA in ItemData.objMasstifform.lstAdInfo.Where(w => aAdinfo.Contains(w.nID))
                                                                     from lstQ in GetAdditional.TM_Additional_Questions.Where(w => w.Id + "" == lstA.nID).DefaultIfEmpty(new Models.TIFForm.TM_Additional_Questions())
                                                                     select new TM_Candidate_MassTIF_Additional
                                                                     {
                                                                         update_user = CGlobal.UserInfo.UserId,
                                                                         update_date = dNow,
                                                                         create_user = CGlobal.UserInfo.UserId,
                                                                         create_date = dNow,
                                                                         active_status = "Y",
                                                                         TM_Additional_Questions = lstQ != null ? lstQ : null,
                                                                         TM_Candidate_MassTIF = _GetCandidateTIF != null ? _GetCandidateTIF : null,
                                                                         other_answer = lstA.other_answer,
                                                                         TM_Candidate_MassTIF_Adnl_Answer = new List<TM_Candidate_MassTIF_Adnl_Answer>(),
                                                                     }).ToList();
                                                        lstAdInfo = lstAdInfo.Where(w => w.TM_Additional_Questions != null).ToList();

                                                        lstAdInfo.ForEach(ed =>
                                                        {
                                                            var CheckAnswer = ItemData.objMasstifform.lstAdInfo.Where(w => w.nID == ed.TM_Additional_Questions.Id + "").FirstOrDefault();
                                                            if (CheckAnswer != null && CheckAnswer.lstAnswersselect != null && CheckAnswer.lstAnswersselect.Length > 0)
                                                            {
                                                                foreach (var ans in CheckAnswer.lstAnswersselect)
                                                                {
                                                                    int nAnsId = SystemFunction.GetIntNullToZero(ans);
                                                                    var _getAns = _TM_Additional_AnswersService.Find(nAnsId);
                                                                    if (_getAns != null)
                                                                    {
                                                                        ed.TM_Candidate_MassTIF_Adnl_Answer.Add(new TM_Candidate_MassTIF_Adnl_Answer()
                                                                        {
                                                                            update_user = CGlobal.UserInfo.UserId,
                                                                            update_date = dNow,
                                                                            create_user = CGlobal.UserInfo.UserId,
                                                                            create_date = dNow,
                                                                            active_status = "Y",
                                                                            TM_Additional_Answers = _getAns,
                                                                            TM_Candidate_MassTIF_Additional = _GetCandidateTIF != null ? _TM_Candidate_MassTIF_AdditionalService.FindForSave(_GetCandidateTIF.Id, ed.TM_Additional_Questions.Id) : null,
                                                                        });
                                                                    }
                                                                }
                                                            }

                                                        });


                                                    }


                                                    var SaveCore = _TM_Candidate_MassTIF_CoreService.UpdateAnswer(lstCore, _GetCandidateTIF.Id, CGlobal.UserInfo.UserId, dNow);
                                                    var SaveAud = _TM_Candidate_MassTIF_Audit_QstService.UpdateAnswer(lstAud_Qst, _GetCandidateTIF.Id, CGlobal.UserInfo.UserId, dNow);
                                                    if (_TM_Candidate_MassTIF_AdditionalService.UpdateAnswer(lstAdInfo, _GetCandidateTIF.Id, CGlobal.UserInfo.UserId, dNow) >= 0)
                                                    {
                                                        result.Status = SystemFunction.process_Success;
                                                        int[] aQusID = lstAdInfo.Select(s => s.TM_Additional_Questions.Id).ToArray();
                                                        var _GetAdInfolst = _TM_Candidate_MassTIF_AdditionalService.GetListForUpdateAns(_GetCandidateTIF.Id, aQusID);
                                                        if (_GetAdInfolst != null)
                                                        {
                                                            foreach (var Adin in _GetAdInfolst)
                                                            {
                                                                var _getAnss = lstAdInfo.Where(w => w.TM_Additional_Questions.Id == Adin.TM_Additional_Questions.Id).FirstOrDefault();
                                                                if (_getAnss != null)
                                                                {
                                                                    _TM_Candidate_MassTIF_Adnl_AnswerService.UpdateLstAns(_getAnss.TM_Candidate_MassTIF_Adnl_Answer.ToList(), Adin.Id, CGlobal.UserInfo.UserId, dNow);

                                                                }
                                                            }
                                                        }
                                                    }

                                                    #region Send mail 
                                                    //check approved
                                                    var _getCheckTIF = _TM_Candidate_MassTIFService.Find(_getData.TM_Candidate_MassTIF.Id);
                                                    _getCheckTIF.TM_PR_Candidate_Mapping = _GetMap;
                                                    //var _GetMap = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                                                    if (_getCheckTIF != null && _GetMap != null)
                                                    {
                                                        var CheckCount = _getCheckTIF.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                                                        var CheckNotApprove = _getCheckTIF.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();

                                                        if (CheckNotApprove <= 0)
                                                        {
                                                            //to hr and complected
                                                            string sError = "";
                                                            string mail_to_log = "";
                                                            var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Approve_HR_Change);
                                                            if (Mail1 != null)
                                                            {
                                                                var bSuss = SendMassTIFFormComplected(_getCheckTIF, Mail1, ref sError, ref mail_to_log);
                                                            }


                                                            string sError2 = "";
                                                            string mail_to_log2 = "";
                                                            var Mail2 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Send_to_HR);
                                                            if (Mail2 != null)
                                                            {
                                                                var bSuss2 = SendMassTIFFormToHR(_getCheckTIF, Mail2, ref sError2, ref mail_to_log2);
                                                            }

                                                        }
                                                        else if (CheckNotApprove > 0)
                                                        {
                                                            //send submit
                                                            string sError = "";
                                                            string mail_to_log = "";
                                                            var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Submit_TIFForm);
                                                            if (Mail1 != null)
                                                            {
                                                                var bSuss = SendFirstMassTIFFormSubmit(_getCheckTIF, Mail1, ref sError, ref mail_to_log);
                                                            }

                                                        }

                                                    }

                                                    #endregion
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
                                            result.Msg = "Error, Need befor step Confirmed.";
                                        }

                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, TIF Form Not Found.";
                                    }

                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Status Not Found.";
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
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Interviewer (Evaluator) must “tick” the box for acknowledgement confidential information from candidate's previous/current employer will not be disclosed to us during the interview session.";
                    }
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select Approval.";
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult RollBackMassTIFForm(vInterview_obj_save ItemData)
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
                    bool isAdmin = CGlobal.UserIsAdmin();
                    var _getData = _TM_Candidate_MassTIF_ApprovService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                        {
                            var ApproveComplect = _TM_Candidate_MassTIF_ApprovService.RollBackStatus(_getData.TM_Candidate_MassTIF.Id, CGlobal.UserInfo.UserId, dNow);
                            if (ApproveComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Evaluator_Rollback);
                                var _getCheckTIF = _TM_Candidate_MassTIFService.Find(_getData.TM_Candidate_MassTIF.Id);
                                var _GetMap = _TM_PR_Candidate_MappingService.Find(_getCheckTIF.TM_PR_Candidate_Mapping.Id);
                                if (_getCheckTIF != null && _GetMap != null)
                                {
                                    int nSeq = SystemFunction.GetIntNullToZero(_getData.seq + "");
                                    _getCheckTIF.TM_PR_Candidate_Mapping = _GetMap;
                                    var bSuss = SendMassBuRollback(_getCheckTIF, Mail1, ItemData.rejeck, nSeq, ref sError, ref mail_to_log);
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
        [HttpPost]
        public ActionResult ApproveMassData(vInterview_obj_save ItemData)
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
                    bool isAdmin = CGlobal.UserIsAdmin();
                    var _getData = _TM_Candidate_MassTIF_ApprovService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                        {
                            var _getTIFForm = _TM_Candidate_MassTIFService.Find(_getData.TM_Candidate_MassTIF.Id);
                            if (_getTIFForm != null && _getTIFForm.TM_Candidate_MassTIF_Approv != null && _getTIFForm.TM_Candidate_MassTIF_Approv.Any())
                            {
                                var CheckAllApprove = _getTIFForm.TM_Candidate_MassTIF_Approv.Where(w => w.seq < _getData.seq && (w.Approve_status + "" != "Y") && w.active_status == "Y").ToList();
                                if (!CheckAllApprove.Any())
                                {
                                    _getData.Approve_status = "Y";
                                    _getData.Approve_date = dNow;
                                    _getData.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                    _getData.update_user = CGlobal.UserInfo.UserId;
                                    var ApproveComplect = _TM_Candidate_MassTIF_ApprovService.Update(_getData);
                                    if (ApproveComplect > 0)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                        #region Send mail 
                                        //check approved
                                        var _getCheckTIF = _TM_Candidate_MassTIFService.Find(_getData.TM_Candidate_MassTIF.Id);
                                        var _GetMap = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                                        if (_getCheckTIF != null && _GetMap != null)
                                        {
                                            _getCheckTIF.TM_PR_Candidate_Mapping = _GetMap;
                                            _getCheckTIF.update_user = CGlobal.UserInfo.UserId;
                                            _getCheckTIF.update_date = dNow;
                                            _TM_Candidate_MassTIFService.ApproveTIFForm(_getCheckTIF);
                                            var CheckCount = _getCheckTIF.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                                            var CheckNotApprove = _getCheckTIF.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                                            if (CheckNotApprove <= 0)
                                            {
                                                //to hr and complected
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Approve_HR_Change);
                                                if (Mail1 != null)
                                                {
                                                    var bSuss = SendMassTIFFormComplected(_getCheckTIF, Mail1, ref sError, ref mail_to_log);
                                                }


                                                string sError2 = "";
                                                string mail_to_log2 = "";
                                                var Mail2 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Send_to_HR);
                                                if (Mail2 != null)
                                                {
                                                    var bSuss2 = SendMassTIFFormToHR(_getCheckTIF, Mail2, ref sError2, ref mail_to_log2);
                                                }

                                            }
                                            else if (CheckNotApprove > 0)
                                            {
                                                //send submit
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.Submit_TIFForm);
                                                if (Mail1 != null)
                                                {
                                                    var bSuss = SendFirstMassTIFFormSubmit(_getCheckTIF, Mail1, ref sError, ref mail_to_log);
                                                }

                                            }

                                        }

                                        #endregion
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
                                    result.Msg = "Error, Need befor step Confirmed.";
                                }


                            }

                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, TIF Form not found.";
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
        [HttpPost]
        public JsonResult GetMassAuditing(string SearchItem, string tifno)
        {
            vMassType_onchange result = new vMassType_onchange();
            result.lstAuditing_Qst = new List<lstDataSelect>() { new lstDataSelect { value = "", text = " - Select - ", } };

            if (SearchItem != "" && tifno != "")
            {
                int nTypeId = SystemFunction.GetIntNullToZero(SearchItem + "");
                int nTIFId = SystemFunction.GetIntNullToZero(tifno + "");
                var _GetData = _TM_Mass_Auditing_QuestionService.FindForTIFForm(nTypeId, nTIFId);
                if (_GetData.Any())
                {
                    result.lstAuditing_Qst.AddRange(_GetData.Where(w => w.active_status == "Y").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question + "" }).ToList());
                }
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}