using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.PreInternAssessment;
using HumanCapitalManagement.Service.TIFForm;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using HumanCapitalManagement.ViewModel.ReportVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.PreIntern
{
    public class PreInternController : BaseController
    {
        #region Service
        //private wsHRIS.HRISSoapClient wsHRis = new wsHRIS.HRISSoapClient();
        #region TIF
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
        private TM_Candidate_StatusService _TM_Candidate_StatusService;
        private DivisionService _DivisionService;
        #endregion
        //Pre Intern
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
        private TM_PIntern_Mass_Form_QuestionService _TM_PIntern_Mass_Form_QuestionService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public PreInternController(PersonnelRequestService PersonnelRequestService
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
             , TM_Candidate_StatusService TM_Candidate_StatusService
             , DivisionService DivisionService
          , TM_PIntern_FormService TM_PIntern_FormService,
          TM_PIntern_RatingService TM_PIntern_RatingService,
          TM_PIntern_StatusService TM_PIntern_StatusService,
          TM_PInternAssessment_ActivitiesService TM_PInternAssessment_ActivitiesService,
          TM_Candidate_PIntern_AnswerService TM_Candidate_PIntern_AnswerService,
          TM_Candidate_PIntern_ApprovService TM_Candidate_PIntern_ApprovService,
          TM_Candidate_PInternService TM_Candidate_PInternService,
          TM_PIntern_RatingFormService TM_PIntern_RatingFormService,
          TM_PIntern_Mass_Form_QuestionService TM_PIntern_Mass_Form_QuestionService,
          TM_Candidate_PIntern_Mass_AnswerService TM_Candidate_PIntern_Mass_AnswerService,
          TM_Candidate_PIntern_Mass_ApprovService TM_Candidate_PIntern_Mass_ApprovService,
          TM_Candidate_PIntern_MassService TM_Candidate_PIntern_MassService
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
            _TM_Candidate_StatusService = TM_Candidate_StatusService;
            _DivisionService = DivisionService;
            //Pre Intern
            _TM_PIntern_FormService = TM_PIntern_FormService;
            _TM_PIntern_RatingService = TM_PIntern_RatingService;
            _TM_PIntern_StatusService = TM_PIntern_StatusService;
            _TM_PInternAssessment_ActivitiesService = TM_PInternAssessment_ActivitiesService;
            _TM_Candidate_PIntern_AnswerService = TM_Candidate_PIntern_AnswerService;
            _TM_Candidate_PIntern_ApprovService = TM_Candidate_PIntern_ApprovService;
            _TM_Candidate_PInternService = TM_Candidate_PInternService;
            _TM_PIntern_RatingFormService = TM_PIntern_RatingFormService;
            _TM_PIntern_Mass_Form_QuestionService = TM_PIntern_Mass_Form_QuestionService;
            _TM_Candidate_PIntern_Mass_AnswerService = TM_Candidate_PIntern_Mass_AnswerService;
            _TM_Candidate_PIntern_Mass_ApprovService = TM_Candidate_PIntern_Mass_ApprovService;
            _TM_Candidate_PIntern_MassService = TM_Candidate_PIntern_MassService;
        }
        #endregion
        //NonMass
        #region 1st-2nd Eva Pre-Intern
        // 1-2 Evalutation Pre-Intern
        public ActionResult PreInternFirstList(string qryStr)
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
                result.PR_No = SearchItem.PR_No;
                result.ActivitiesTrainee_code = SearchItem.ActivitiesTrainee_code;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    int[] nMapID = lstData.Select(s => s.Id).ToArray();
                    var _GetPreInternform = _TM_Candidate_PInternService.FindByMappingArrayID(nMapID);
                    var _GetMassTifform = _TM_Candidate_MassTIFService.FindByMappingArrayID(nMapID);
                    if (_GetPreInternform.Any(a => a.submit_status == "Y"))
                    {
                        lstData = lstData.Where(w => !_GetPreInternform.Where(w2 => w2.submit_status == "Y").Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray().Contains(w.Id)).ToList();
                    }
                    var _GetPreInternformMass = _TM_Candidate_PIntern_MassService.FindByMappingArrayID(nMapID);
                    if (_GetPreInternformMass.Any(a => a.submit_status == "Y"))
                    {
                        lstData = lstData.Where(w => !_GetPreInternformMass.Where(w2 => w2.submit_status == "Y").Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray().Contains(w.Id)).ToList();
                    }
                    result.lstData = (from lstAD in lstData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id == 10)
                                          //where lstAD.TM_PInternAssessment_Activities != null ? lstData.Where(w=>w.TM_PInternAssessment_Activities.Id = result.ActivitiesTrainee_code:null)
                                      from lstPIntern in _GetPreInternform.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_PIntern())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vInterview_obj
                                      {
                                          refno = lstAD.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                          name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                          status = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstPIntern.submit_status != null ? (lstPIntern.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting") : (lstPIntern.submit_status != null ? (lstPIntern.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting"),
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault().Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                          activities = lstAD.TM_PInternAssessment_Activities != null ? lstAD.TM_PInternAssessment_Activities.Activities_name_en + "" : "",
                                      }).ToList();
                }
            }

            #endregion
            return View("PreInternFirstList", result);

        }
        #region Ajax Function calculate score
        [HttpPost]
        public ActionResult PiaAssessmentScore(List<int> answer_list)
        {
            string status = _TM_PIntern_FormService.CalScore(answer_list);
            return Json(new { status = status, ok = true });
        }

        public ActionResult PreInternFirstForm(string qryStr)
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
                            result.activities_Id = _getData.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities != null ? _getData.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities.Activities_name_en + "" : "-";

                            if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm) && (_getRequest.type_of_TIFForm == "N"))
                            {
                                //Service PIA
                                var _getPintern = _TM_PIntern_FormService.GetActivePInternForm();
                                var _getRatingForm = _TM_PIntern_RatingFormService.GetActivePInternForm();
                                //if Question not null
                                if (_getPintern != null && _getPintern.TM_PIntern_Form_Question != null)
                                {
                                    var _CheckPIn = _TM_Candidate_PInternService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                                    //If have Candidate mappinng 
                                    if (_CheckPIn != null)
                                    {
                                        result.recommended_rank_id = _CheckPIn.Recommended_Rank != null ? _CheckPIn.Recommended_Rank.Id + "" : "";
                                        result.confidentiality_agreement = _CheckPIn.confidentiality_agreement + "";
                                        result.comment = _CheckPIn.comments;
                                        result.PIA_status_id = _CheckPIn.TM_PIntern_Status != null ? _CheckPIn.TM_PIntern_Status.Id + "" : "";
                                        //if have Ans
                                        if (_CheckPIn.TM_Candidate_PIntern_Answer != null && _CheckPIn.TM_Candidate_PIntern_Answer.Any(a => a.active_status == "Y"))
                                        {
                                            ////Question By Ans
                                            _getPintern = _TM_PIntern_FormService.GetActivePInternForm(_CheckPIn.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Form_Question.TM_PIntern_Form.Id);
                                            result.TIF_type = _getRequest.type_of_TIFForm;
                                            result.objtifform = new vObject_of_tif();
                                            result.objtifform.TIF_no = _getPintern.Id + "";
                                            result.objtifform.lstQuestion = (from lstQ in _getPintern.TM_PIntern_Form_Question
                                                                             select new vtif_list_question
                                                                             {
                                                                                 id = lstQ.Id + "",
                                                                                 question = lstQ.question,
                                                                                 header = lstQ.header,
                                                                                 nSeq = lstQ.seq,
                                                                                 remark = "",
                                                                                 rating = "",
                                                                                 topic = lstQ.topic,
                                                                             }).ToList();

                                            //rating Ans
                                            result.objtifform.lstQuestion.ForEach(ed =>
                                                {
                                                    var GetAnsP = _CheckPIn.TM_Candidate_PIntern_Answer.Where(w => w.TM_PIntern_Form_Question.Id + "" == ed.id).FirstOrDefault();
                                                    if (GetAnsP != null)
                                                    {
                                                        ed.remark = GetAnsP.answer + "";
                                                        ed.rating = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.Id + "" : "";
                                                        ed.point = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.point + "" : "";
                                                    }
                                                });


                                            var check_form = _CheckPIn.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating;

                                            var getratingform = check_form != null ? check_form.TM_PIntern_RatingForm.Id: 0;


                                            //select rating Citeria show by form
                                            var _getPinternR = _TM_PIntern_RatingFormService.GetActivePInternForm(getratingform);

                                            result.objtifform.lstRating = (from lstQ in _getPinternR.TM_PIntern_Rating
                                                                           orderby lstQ.seq
                                                                           select new lstDataSelect
                                                                           {
                                                                               value = lstQ.Id + "",
                                                                               text = lstQ.rating_name_en + "",
                                                                               detail = lstQ.rating_description + "",
                                                                               nSeq = lstQ.seq + "",
                                                                           }).ToList();
                                            result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                        }
                                        //if Never Ans
                                        else
                                        {

                                            result.TIF_type = _getRequest.type_of_TIFForm;
                                            result.objtifform = new vObject_of_tif();
                                            result.objtifform.TIF_no = _getPintern.Id + "";

                                            result.objtifform.lstQuestion = (from lstQ in _getPintern.TM_PIntern_Form_Question
                                                                             select new vtif_list_question
                                                                             {
                                                                                 id = lstQ.Id + "",
                                                                                 question = lstQ.question,
                                                                                 header = lstQ.header,
                                                                                 nSeq = lstQ.seq,
                                                                                 remark = "",
                                                                                 rating = "",
                                                                                 topic = lstQ.topic,
                                                                             }).ToList();

                                            var _getPinternR = _TM_PIntern_RatingService.GetDataForSelect();
                                            result.objtifform.lstRating = (from lstQ in _getPinternR.Where(w => w.TM_PIntern_RatingForm.active_status == "Y")
                                                                           orderby lstQ.seq
                                                                           select new lstDataSelect

                                                                           {
                                                                               value = lstQ.Id + "",
                                                                               text = lstQ.rating_name_en + "",
                                                                               detail = lstQ.rating_description + "",
                                                                               nSeq = lstQ.seq + "",
                                                                           }).ToList();
                                            result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                        }
                                    }
                                    //if not have  Candidate mapping
                                    else
                                    {
                                        //select question by : form active
                                        result.TIF_type = _getRequest.type_of_TIFForm;
                                        result.objtifform = new vObject_of_tif();
                                        result.objtifform.TIF_no = _getPintern.Id + "";
                                        result.objtifform.lstQuestion = (from lstQ in _getPintern.TM_PIntern_Form_Question
                                                                         select new vtif_list_question
                                                                         {
                                                                             id = lstQ.Id + "",
                                                                             question = lstQ.question,
                                                                             header = lstQ.header,
                                                                             nSeq = lstQ.seq,
                                                                             remark = "",
                                                                             rating = "",
                                                                             topic = lstQ.topic,
                                                                         }).ToList();
                                        //select rating by form : active
                                        //result.objtifform.lstRating = _TM_PIntern_RatingFormService.GetDataForSelect().Select(s => new lstDataSelect
                                        //{
                                        //    value = s.TM_PIntern_Rating.FirstOrDefault().Id + "",
                                        //    nSeq = s.TM_PIntern_Rating.FirstOrDefault().seq + "",
                                        //    text = s.TM_PIntern_Rating.FirstOrDefault().rating_name_en + "",
                                        //    detail = s.TM_PIntern_Rating.FirstOrDefault().rating_description + "",

                                        //}).ToList();
                                        //result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                        var _getPinternR = _TM_PIntern_RatingService.GetDataForSelect();
                                        result.objtifform.lstRating = (from lstQ in _getPinternR.Where(w => w.TM_PIntern_RatingForm.active_status == "Y")
                                                                       orderby lstQ.seq
                                                                       select new lstDataSelect

                                                                       {
                                                                           value = lstQ.Id + "",
                                                                           text = lstQ.rating_name_en + "",
                                                                           detail = lstQ.rating_description + "",
                                                                           nSeq = lstQ.seq + "",
                                                                       }).ToList();
                                        result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });


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
                            }
                            else if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm) && (_getRequest.type_of_TIFForm == "M"))
                            {
                                //Service PIA
                                var _getPinternMass = _TM_PIntern_Mass_Form_QuestionService.GetActivePInternForm();
                                var _getRatingForm = _TM_PIntern_RatingFormService.GetActivePInternForm();
                                //if Question not null
                                if (_getPinternMass != null && _getPinternMass.TM_PIntern_Mass_Question != null)
                                {
                                    var _CheckPInAns = _TM_Candidate_PIntern_MassService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                                    //If have Candidate mappinng 
                                    if (_CheckPInAns != null)
                                    {
                                        result.recommended_rank_id = _CheckPInAns.Recommended_Rank != null ? _CheckPInAns.Recommended_Rank.Id + "" : "";
                                        result.confidentiality_agreement = _CheckPInAns.confidentiality_agreement + "";
                                        result.comment = _CheckPInAns.comments;
                                        result.PIA_status_id = _CheckPInAns.TM_PIntern_Status != null ? _CheckPInAns.TM_PIntern_Status.Id + "" : "";
                                        //if have Ans
                                        if (_CheckPInAns.TM_Candidate_PIntern_Mass_Answer != null && _CheckPInAns.TM_Candidate_PIntern_Mass_Answer.Any(a => a.active_status == "Y"))
                                        {
                                            ////Question By Ans
                                            _getPinternMass = _TM_PIntern_Mass_Form_QuestionService.GetActivePInternForm(_CheckPInAns.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Mass_Question.TM_PIntern_Mass_Form_Question.Id);
                                            result.TIF_type = _getRequest.type_of_TIFForm;
                                            result.objtifform = new vObject_of_tif();
                                            result.objtifform.TIF_no = _getPinternMass.Id + "";
                                            result.objtifform.lstQuestion = (from lstQ in _getPinternMass.TM_PIntern_Mass_Question
                                                                             select new vtif_list_question
                                                                             {
                                                                                 id = lstQ.Id + "",
                                                                                 question = lstQ.question + "",
                                                                                 header = lstQ.header + "",
                                                                                 nSeq = lstQ.seq,
                                                                                 remark = "",
                                                                                 rating = "",
                                                                                 topic = lstQ.topic + "",
                                                                             }).ToList();

                                            //rating Ans
                                            result.objtifform.lstQuestion.ForEach(ed =>
                                            {
                                                var GetAnsP = _CheckPInAns.TM_Candidate_PIntern_Mass_Answer.Where(w => w.TM_PIntern_Mass_Question.Id + "" == ed.id).FirstOrDefault();
                                                if (GetAnsP != null)
                                                {
                                                    ed.remark = GetAnsP.answer + "";
                                                    ed.rating = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.Id + "" : "";
                                                    ed.point = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.point + "" : "";
                                                }
                                            });
                                            var check_form = _CheckPInAns.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating;

                                            var getratingform = check_form != null ? check_form.TM_PIntern_RatingForm.Id : 0;


                                            //select rating Citeria show by form
                                            var _getPinternR = _TM_PIntern_RatingFormService.GetActivePInternForm(getratingform);

                                            //select rating Citeria show by form
                                            //var _getPinternR = _TM_PIntern_RatingFormService.GetActivePInternForm(_CheckPInAns.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);

                                            result.objtifform.lstRating = (from lstQ in _getPinternR.TM_PIntern_Rating
                                                                           orderby lstQ.seq
                                                                           select new lstDataSelect
                                                                           {
                                                                               value = lstQ.Id + "",
                                                                               text = lstQ.rating_name_en + "",
                                                                               detail = lstQ.rating_description + "",
                                                                               nSeq = lstQ.seq + "",
                                                                           }).ToList();
                                            result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                        }
                                        //if Never Ans
                                        else
                                        {

                                            result.TIF_type = _getRequest.type_of_TIFForm;
                                            result.objtifform = new vObject_of_tif();
                                            result.objtifform.TIF_no = _getPinternMass.Id + "";

                                            result.objtifform.lstQuestion = (from lstQ in _getPinternMass.TM_PIntern_Mass_Question
                                                                             select new vtif_list_question
                                                                             {
                                                                                 id = lstQ.Id + "",
                                                                                 question = lstQ.question,
                                                                                 header = lstQ.header,
                                                                                 nSeq = lstQ.seq,
                                                                                 remark = "",
                                                                                 rating = "",
                                                                                 topic = lstQ.topic,
                                                                             }).ToList();

                                            var _getPinternR = _TM_PIntern_RatingService.GetDataForSelect();
                                            result.objtifform.lstRating = (from lstQ in _getPinternR.Where(w => w.TM_PIntern_RatingForm.active_status == "Y")
                                                                           orderby lstQ.seq
                                                                           select new lstDataSelect

                                                                           {
                                                                               value = lstQ.Id + "",
                                                                               text = lstQ.rating_name_en + "",
                                                                               detail = lstQ.rating_description + "",
                                                                               nSeq = lstQ.seq + "",
                                                                           }).ToList();
                                            result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                        }
                                    }
                                    //if not have  Candidate mapping
                                    else
                                    {
                                        //select question by : form active
                                        result.TIF_type = _getRequest.type_of_TIFForm;
                                        result.objtifform = new vObject_of_tif();
                                        result.objtifform.TIF_no = _getPinternMass.Id + "";
                                        result.objtifform.lstQuestion = (from lstQ in _getPinternMass.TM_PIntern_Mass_Question
                                                                         select new vtif_list_question
                                                                         {
                                                                             id = lstQ.Id + "",
                                                                             question = lstQ.question,
                                                                             header = lstQ.header,
                                                                             nSeq = lstQ.seq,
                                                                             remark = "",
                                                                             rating = "",
                                                                             topic = lstQ.topic,
                                                                         }).ToList();
                                        //select rating by form : active
                                        //result.objtifform.lstRating = _TM_PIntern_RatingFormService.GetDataForSelect().Select(s => new lstDataSelect
                                        //{
                                        //    value = s.TM_PIntern_Rating.FirstOrDefault().Id + "",
                                        //    nSeq = s.TM_PIntern_Rating.FirstOrDefault().seq + "",
                                        //    text = s.TM_PIntern_Rating.FirstOrDefault().rating_name_en + "",
                                        //    detail = s.TM_PIntern_Rating.FirstOrDefault().rating_description + "",

                                        //}).ToList();
                                        //result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                        var _getPinternR = _TM_PIntern_RatingService.GetDataForSelect();
                                        result.objtifform.lstRating = (from lstQ in _getPinternR.Where(w => w.TM_PIntern_RatingForm.active_status == "Y")
                                                                       orderby lstQ.seq
                                                                       select new lstDataSelect

                                                                       {
                                                                           value = lstQ.Id + "",
                                                                           text = lstQ.rating_name_en + "",
                                                                           detail = lstQ.rating_description + "",
                                                                           nSeq = lstQ.seq + "",
                                                                       }).ToList();
                                        result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });


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
                    }
                }
            }
            #endregion
            return View(result);

        }
        public ActionResult PreInternSecondList(string qryStr)
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

                CSearchInterview SearchItem = (CSearchInterview)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchInterview)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                List<TM_Candidate_PIntern_Mass_Approv> lstDataMass = new List<TM_Candidate_PIntern_Mass_Approv>();
                List<TM_Candidate_PIntern_Approv> lstData = new List<TM_Candidate_PIntern_Approv>();
                if (SearchItem.tif_type == "N")
                {
                    lstData = _TM_Candidate_PIntern_ApprovService.GetPInternFormForApprove(SearchItem.PR_No,
                                                          SearchItem.ActivitiesTrainee_code,
                                                          SearchItem.group_code, aDivisionPermission,
                                                         CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
                }
                else
                {
                    lstDataMass = _TM_Candidate_PIntern_Mass_ApprovService.GetPInternFormForApprove(SearchItem.PR_No,
                                                          SearchItem.ActivitiesTrainee_code,
                                                          SearchItem.group_code, aDivisionPermission,
                                                         CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
                }



                result.tif_type = SearchItem.tif_type;
                result.group_code = SearchItem.group_code;
                result.name = SearchItem.name;
                result.ActivitiesTrainee_code = SearchItem.ActivitiesTrainee_code;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any() || lstDataMass.Any())
                {
                    if (lstData.Any())
                    {
                        string[] aApproveUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                        string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                        var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                        var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                        lstData_resutl = (from lstAD in lstData.Where(w => w.TM_Candidate_PIntern.Recommended_Rank.TM_Rank.Id == 10)
                                              //where lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities != null
                                          from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                          from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                          select new vInterview_obj
                                          {
                                              refno = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
                                              group_name = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                              position = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
                                              rank = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                              name_en = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                              status = !string.IsNullOrEmpty(lstAD.Approve_status) ? (lstAD.Approve_status == "Y" ? "Confirmed" : "Reject") : "Waiting",
                                              update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                              approval = AEmp.EmpFullName,
                                              pr_type = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                              activities = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities != null ? lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities.Activities_name_en + "" : "",
                                          }).ToList();
                        result.lstData = lstData_resutl.ToList();
                    }
                    if (lstDataMass.Any())
                    {
                        string[] aApproveUser = lstDataMass.Select(s => s.Req_Approve_user).ToArray();
                        string[] aUpdateUser = lstDataMass.Select(s => s.update_user).ToArray();
                        var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                        var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                        lstData_resutl = (from lstAD in lstDataMass.Where(w => w.TM_Candidate_PIntern_Mass.Recommended_Rank.TM_Rank.Id == 10)
                                              //where lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities != null
                                          from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                          from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                          select new vInterview_obj
                                          {
                                              refno = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
                                              group_name = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                              position = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
                                              rank = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                              name_en = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                              status = !string.IsNullOrEmpty(lstAD.Approve_status) ? (lstAD.Approve_status == "Y" ? "Confirmed" : "Reject") : "Waiting",
                                              update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                              approval = AEmp.EmpFullName,
                                              pr_type = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                              activities = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities != null ? lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities.Activities_name_en + "" : "",
                                          }).ToList();
                        result.lstData = lstData_resutl.ToList();
                    }

                }

            }

            #endregion
            return View(result);

        }
        public ActionResult PreInternSecondForm(string qryStr)
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
                    var _getDatamapping = _TM_Candidate_Status_CycleService.Find(nId);

                    var _getData = _TM_Candidate_PIntern_ApprovService.Find(nId);
                    var _getDataMass = _TM_Candidate_PIntern_Mass_ApprovService.Find(nId);
                    if (_getData != null)
                    {
                        bool isHRAdmin = CGlobal.UserIsHRAdmin();
                        var _getRequest = _getData.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest;
                        if (_getRequest != null)
                        {
                            // if ((int)HCMClass.UnitGroup.HR == _getRequest.TM_Divisions.Id && )
                            if (!isHRAdmin && !CGlobal.UserIsAdmin() && _getData.Req_Approve_user != CGlobal.UserInfo.EmployeeNo)
                            {
                                return RedirectToAction("ErrorNopermission", "MasterPage");
                            }
                            result.lstApprove = new List<vPersonnelAp_obj>();
                            var _getPintern = _TM_PIntern_FormService.GetActivePInternForm();


                            // Check Non-Audit or Audit
                            //Non-Audit
                            if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm) && (_getRequest.type_of_TIFForm == "N")
                                && _getPintern != null && _getPintern.TM_PIntern_Form_Question != null)
                            {
                                //Non-Audit
                                result.IdEncrypt = qryStr;
                                result.group_id = _getRequest.TM_Divisions.division_name_en;
                                result.rank_id = _getRequest.TM_Pool_Rank != null ? _getRequest.TM_Pool_Rank.Pool_rank_name_en + "" : "-";
                                result.recommended_rank_id = _getRequest.TM_Pool_Rank != null ? _getRequest.TM_Pool_Rank.Id + "" : "";
                                result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                                result.activities_Id = _getData.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities != null ? _getData.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities.Activities_name_en + "" : "-";
                                if (_getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank != null && _getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").Any())
                                {
                                    result.lstrank.AddRange(_getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Rank.piority).Select(s => new vSelect_PR { id = s.Id + "", name = s.Pool_rank_name_en }).ToList());
                                }
                                result.sub_group_id = _getRequest.TM_SubGroup != null ? _getRequest.TM_SubGroup.sub_group_name_en + "" : "-";
                                result.position_id = _getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "-";
                                result.employment_type_id = _getRequest.TM_Employment_Request.TM_Employment_Type != null ? _getRequest.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                                result.request_type_id = _getRequest.TM_Employment_Request.TM_Request_Type != null ? _getRequest.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";

                                result.candidate_name = _getData.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + _getData.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                                result.approve_status = _getData.Approve_status + "";

                                var _CheckData = _TM_Candidate_PInternService.FindByMappingID(_getData.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.Id);

                                result.TIF_type = _getRequest.type_of_TIFForm;
                                result.objtifform = new vObject_of_tif();
                                result.objtifform.TIF_no = _getPintern.Id + "";
                                result.objtifform.lstQuestion = (from lstQ in _getPintern.TM_PIntern_Form_Question
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
                                    result.PIA_status_id = _CheckData.TM_PIntern_Status != null ? _CheckData.TM_PIntern_Status.Id + "" : "";
                                    if (_CheckData.TM_Candidate_PIntern_Answer != null && _CheckData.TM_Candidate_PIntern_Answer.Any(a => a.active_status == "Y"))
                                    {
                                        _getPintern = _TM_PIntern_FormService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Form_Question.TM_PIntern_Form.Id);
                                        if (_getPintern != null)
                                        {

                                            ////Question By Ans
                                            _getPintern = _TM_PIntern_FormService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Form_Question.TM_PIntern_Form.Id);
                                            result.TIF_type = _getRequest.type_of_TIFForm;
                                            result.objtifform = new vObject_of_tif();
                                            result.objtifform.TIF_no = _getPintern.Id + "";
                                            result.objtifform.lstQuestion = (from lstQ in _getPintern.TM_PIntern_Form_Question
                                                                             select new vtif_list_question
                                                                             {
                                                                                 id = lstQ.Id + "",
                                                                                 question = lstQ.question,
                                                                                 header = lstQ.header,
                                                                                 nSeq = lstQ.seq,
                                                                                 remark = "",
                                                                                 rating = "",
                                                                                 topic = lstQ.topic,
                                                                             }).ToList();

                                            //rating Ans
                                            result.objtifform.lstQuestion.ForEach(ed =>
                                            {
                                                var GetAnsP = _CheckData.TM_Candidate_PIntern_Answer.Where(w => w.TM_PIntern_Form_Question.Id + "" == ed.id).FirstOrDefault();
                                                if (GetAnsP != null)
                                                {
                                                    ed.remark = GetAnsP.answer + "";
                                                    ed.rating = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.Id + "" : "";
                                                    ed.point = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.point + "" : "";
                                                }
                                            });
                                            //select rating Citeria show by form
                                            var _getPinternR = _TM_PIntern_RatingFormService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);

                                            result.objtifform.lstRating = (from lstQ in _getPinternR.TM_PIntern_Rating
                                                                           orderby lstQ.seq
                                                                           select new lstDataSelect
                                                                           {
                                                                               value = lstQ.Id + "",
                                                                               text = lstQ.rating_name_en + "",
                                                                               detail = lstQ.rating_description + "",
                                                                               nSeq = lstQ.seq + "",
                                                                           }).ToList();
                                            result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });

                                        }

                                    }

                                    if (_CheckData.TM_Candidate_PIntern_Approv != null && _CheckData.TM_Candidate_PIntern_Approv.Any())
                                    {
                                        string[] aUser = _CheckData.TM_Candidate_PIntern_Approv.Select(s => s.Req_Approve_user).ToArray();
                                        string[] aApproveUser = _CheckData.TM_Candidate_PIntern_Approv.Select(s => s.Approve_user).ToArray();
                                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                        var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                        int nPriority = 1;
                                        result.lstApprove = (from Tif in _CheckData.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
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
                                //Get Rating Citeria show 
                                var _getPIARating = _TM_PIntern_RatingFormService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);

                                result.objtifform.lstRating = (from lstQ in _getPIARating.TM_PIntern_Rating
                                                               orderby lstQ.seq
                                                               select new lstDataSelect
                                                               {
                                                                   value = lstQ.Id + "",
                                                                   text = lstQ.rating_name_en + "",
                                                                   detail = lstQ.rating_description + "",
                                                                   nSeq = lstQ.seq + "",
                                                               }).ToList();
                                result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });

                            }
                        }
                        else
                        {
                            result.TIF_type = "";
                            result.pr_no = _getRequest.RefNo + "";
                            string sUserNo = _getData.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Recruitment_Team.user_no + "";
                            if (!string.IsNullOrEmpty(sUserNo))
                            {
                                var getOwner = dbHr.AllInfo_WS.Where(w => w.EmpNo == sUserNo).FirstOrDefault();
                                result.null_tif_contact = getOwner != null ? getOwner.EmpFullName + "(" + getOwner.OfficePhone + ")" : "";
                            }
                        }


                    }
                    else if (_getDataMass != null)
                    {
                        bool isHRAdmin = CGlobal.UserIsHRAdmin();
                        var _getRequestMass = _getDataMass.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest;
                        if (_getRequestMass != null)
                        {
                            // if ((int)HCMClass.UnitGroup.HR == _getRequest.TM_Divisions.Id && )
                            if (!isHRAdmin && !CGlobal.UserIsAdmin() && _getDataMass.Req_Approve_user != CGlobal.UserInfo.EmployeeNo)
                            {
                                return RedirectToAction("ErrorNopermission", "MasterPage");
                            }
                            result.lstApprove = new List<vPersonnelAp_obj>();
                            var _getPinternMass = _TM_PIntern_Mass_Form_QuestionService.GetActivePInternForm();


                            //Audit
                            if (!string.IsNullOrEmpty(_getRequestMass.type_of_TIFForm) && (_getRequestMass.type_of_TIFForm == "M")
                               && _getPinternMass != null && _getPinternMass.TM_PIntern_Mass_Question != null)
                            {
                                result.IdEncrypt = qryStr;
                                result.group_id = _getRequestMass.TM_Divisions.division_name_en;
                                result.rank_id = _getRequestMass.TM_Pool_Rank != null ? _getRequestMass.TM_Pool_Rank.Pool_rank_name_en + "" : "-";
                                result.recommended_rank_id = _getRequestMass.TM_Pool_Rank != null ? _getRequestMass.TM_Pool_Rank.Id + "" : "";
                                result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                                result.activities_Id = _getDataMass.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities != null ? _getDataMass.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities.Activities_name_en + "" : "-";
                                if (_getRequestMass.TM_Divisions.TM_Pool.TM_Pool_Rank != null && _getRequestMass.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").Any())
                                {
                                    result.lstrank.AddRange(_getRequestMass.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Rank.piority).Select(s => new vSelect_PR { id = s.Id + "", name = s.Pool_rank_name_en }).ToList());
                                }
                                result.sub_group_id = _getRequestMass.TM_SubGroup != null ? _getRequestMass.TM_SubGroup.sub_group_name_en + "" : "-";
                                result.position_id = _getRequestMass.TM_Position != null ? _getRequestMass.TM_Position.position_name_en + "" : "-";
                                result.employment_type_id = _getRequestMass.TM_Employment_Request.TM_Employment_Type != null ? _getRequestMass.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                                result.request_type_id = _getRequestMass.TM_Employment_Request.TM_Request_Type != null ? _getRequestMass.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";

                                result.candidate_name = _getDataMass.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + _getDataMass.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en;
                                result.approve_status = _getDataMass.Approve_status + "";

                                var _CheckDataMass = _TM_Candidate_PIntern_MassService.FindByMappingID(_getDataMass.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.Id);
                                result.TIF_type = _getRequestMass.type_of_TIFForm;
                                result.objtifform = new vObject_of_tif();
                                result.objtifform.TIF_no = _getPinternMass.Id + "";
                                result.objtifform.lstQuestion = (from lstQ in _getPinternMass.TM_PIntern_Mass_Question
                                                                 select new vtif_list_question
                                                                 {
                                                                     id = lstQ.Id + "",
                                                                     question = lstQ.question,
                                                                     header = lstQ.header,
                                                                     nSeq = lstQ.seq,
                                                                     remark = "",
                                                                     rating = "",
                                                                 }).ToList();
                                if (_CheckDataMass != null)
                                {
                                    result.recommended_rank_id = _CheckDataMass.Recommended_Rank != null ? _CheckDataMass.Recommended_Rank.Id + "" : "";
                                    result.confidentiality_agreement = _CheckDataMass.confidentiality_agreement + "";
                                    result.comment = _CheckDataMass.comments;
                                    result.PIA_status_id = _CheckDataMass.TM_PIntern_Status != null ? _CheckDataMass.TM_PIntern_Status.Id + "" : "";
                                    if (_CheckDataMass.TM_Candidate_PIntern_Mass_Answer != null && _CheckDataMass.TM_Candidate_PIntern_Mass_Answer.Any(a => a.active_status == "Y"))
                                    {
                                        _getPinternMass = _TM_PIntern_Mass_Form_QuestionService.GetActivePInternForm(_CheckDataMass.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Mass_Question.TM_PIntern_Mass_Form_Question.Id);
                                        if (_getPinternMass != null)
                                        {

                                            ////Question By Ans
                                            _getPinternMass = _TM_PIntern_Mass_Form_QuestionService.GetActivePInternForm(_CheckDataMass.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Mass_Question.TM_PIntern_Mass_Form_Question.Id);
                                            result.TIF_type = _getRequestMass.type_of_TIFForm;
                                            result.objtifform = new vObject_of_tif();
                                            result.objtifform.TIF_no = _getPinternMass.Id + "";
                                            result.objtifform.lstQuestion = (from lstQ in _getPinternMass.TM_PIntern_Mass_Question
                                                                             select new vtif_list_question
                                                                             {
                                                                                 id = lstQ.Id + "",
                                                                                 question = lstQ.question,
                                                                                 header = lstQ.header,
                                                                                 nSeq = lstQ.seq,
                                                                                 remark = "",
                                                                                 rating = "",
                                                                                 topic = lstQ.topic,
                                                                             }).ToList();

                                            //rating Ans
                                            result.objtifform.lstQuestion.ForEach(ed =>
                                            {
                                                var GetAnsP = _CheckDataMass.TM_Candidate_PIntern_Mass_Answer.Where(w => w.TM_PIntern_Mass_Question.Id + "" == ed.id).FirstOrDefault();
                                                if (GetAnsP != null)
                                                {
                                                    ed.remark = GetAnsP.answer + "";
                                                    ed.rating = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.Id + "" : "";
                                                    ed.point = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.point + "" : "";
                                                }
                                            });
                                            //select rating Citeria show by form
                                            var _getPinternR = _TM_PIntern_RatingFormService.GetActivePInternForm(_CheckDataMass.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);

                                            result.objtifform.lstRating = (from lstQ in _getPinternR.TM_PIntern_Rating
                                                                           orderby lstQ.seq
                                                                           select new lstDataSelect
                                                                           {
                                                                               value = lstQ.Id + "",
                                                                               text = lstQ.rating_name_en + "",
                                                                               detail = lstQ.rating_description + "",
                                                                               nSeq = lstQ.seq + "",
                                                                           }).ToList();
                                            result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });

                                        }

                                    }

                                    if (_CheckDataMass.TM_Candidate_PIntern_Mass_Approv != null && _CheckDataMass.TM_Candidate_PIntern_Mass_Approv.Any())
                                    {
                                        string[] aUser = _CheckDataMass.TM_Candidate_PIntern_Mass_Approv.Select(s => s.Req_Approve_user).ToArray();
                                        string[] aApproveUser = _CheckDataMass.TM_Candidate_PIntern_Mass_Approv.Select(s => s.Approve_user).ToArray();
                                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                        var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                        int nPriority = 1;
                                        result.lstApprove = (from Tif in _CheckDataMass.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
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
                                //Get Rating Citeria show 
                                var _getPIARating = _TM_PIntern_RatingFormService.GetActivePInternForm(_CheckDataMass.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);

                                result.objtifform.lstRating = (from lstQ in _getPIARating.TM_PIntern_Rating
                                                               orderby lstQ.seq
                                                               select new lstDataSelect
                                                               {
                                                                   value = lstQ.Id + "",
                                                                   text = lstQ.rating_name_en + "",
                                                                   detail = lstQ.rating_description + "",
                                                                   nSeq = lstQ.seq + "",
                                                               }).ToList();
                                result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                            }
                            else
                            {
                                result.TIF_type = "";
                                result.pr_no = _getRequestMass.RefNo + "";
                                string sUserNo = _getData.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Recruitment_Team.user_no + "";
                                if (!string.IsNullOrEmpty(sUserNo))
                                {
                                    var getOwner = dbHr.AllInfo_WS.Where(w => w.EmpNo == sUserNo).FirstOrDefault();
                                    result.null_tif_contact = getOwner != null ? getOwner.EmpFullName + "(" + getOwner.OfficePhone + ")" : "";
                                }
                            }


                        }

                    }

                }

            }
            #endregion
            return View(result);
        }

        #endregion

        #region Acknowledge HR Pre-Intern
        //Acknowledge  Pre-Inntern
        public ActionResult AcKnowledgePreInternList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            bool isHRAdmin = CGlobal.UserIsHRAdmin();

            vAcKnowledge result = new vAcKnowledge();
            result.active_status = "Y";
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchInterview SearchItem = (CSearchInterview)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchInterview)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();

                List<vAcKnowledge_obj> lstData_resutl = new List<vAcKnowledge_obj>();
                List<TM_Candidate_PIntern> lstData = new List<TM_Candidate_PIntern>();
                List<TM_Candidate_PIntern_Mass> lstDataMass = new List<TM_Candidate_PIntern_Mass>();
                List<TM_PR_Candidate_Mapping> lstAllData = new List<TM_PR_Candidate_Mapping>();
                if (SearchItem.tif_type + "" == "")
                {
                    lstData = _TM_Candidate_PInternService.GetForAcknowledge(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code,
                                                      SearchItem.group_code, aDivisionPermission,
                                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
                }
                else if (SearchItem.tif_type + "" == "N")
                {
                    lstData = _TM_Candidate_PInternService.GetForAcknowledge(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code,
                                                    SearchItem.group_code, aDivisionPermission,
                                                   CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
                }
                else if (SearchItem.tif_type + "" == "M")
                {
                    lstDataMass = _TM_Candidate_PIntern_MassService.GetForAcknowledge(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code,
                                                    SearchItem.group_code, aDivisionPermission,
                                                   CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
                }


                result.active_status = SearchItem.active_status;
                result.group_code = SearchItem.group_code;
                result.name = SearchItem.name;
                result.ActivitiesTrainee_code = SearchItem.ActivitiesTrainee_code;
                result.PR_No = SearchItem.PR_No;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    if (lstData.Any())
                    {
                        lstAllData.AddRange(lstData.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                    }

                    string[] aUpdateUser = lstAllData.Select(s => s.update_user).ToArray();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    lstData_resutl = (from lstAD in lstAllData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id == 10)
                                      from lstPIntern in lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_PIntern())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vAcKnowledge_obj
                                      {
                                          Id = lstAD.Id,
                                          IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                          refno = lstAD.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                          name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                          tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstPIntern.TM_PIntern_Status != null ? lstPIntern.TM_PIntern_Status.PIntern_short_name_en + "" : "") : (lstPIntern.TM_PIntern_Status != null ? lstPIntern.TM_PIntern_Status.PIntern_short_name_en + "" : ""),
                                          pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                          update_user = lstEmpUp.EmpFullName + "",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          activities = lstAD.TM_PInternAssessment_Activities != null ? lstAD.TM_PInternAssessment_Activities.Activities_name_en + "" : "",
                                      }).ToList();
                    result.lstData = lstData_resutl.ToList();
                }
                else if (lstDataMass.Any())
                {
                    if (lstDataMass.Any())
                    {
                        lstAllData.AddRange(lstDataMass.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                    }
                    string[] aUpdateUser = lstAllData.Select(s => s.update_user).ToArray();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    lstData_resutl = (from lstAD in lstAllData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id == 10)
                                      from lstPIntern in lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_PIntern_Mass())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vAcKnowledge_obj
                                      {
                                          Id = lstAD.Id,
                                          IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                          refno = lstAD.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                          name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                          tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstPIntern.TM_PIntern_Status != null ? lstPIntern.TM_PIntern_Status.PIntern_short_name_en + "" : "") : (lstPIntern.TM_PIntern_Status != null ? lstPIntern.TM_PIntern_Status.PIntern_short_name_en + "" : ""),
                                          pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                          update_user = lstEmpUp.EmpFullName + "",
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          activities = lstAD.TM_PInternAssessment_Activities != null ? lstAD.TM_PInternAssessment_Activities.Activities_name_en + "" : "",
                                      }).ToList();
                    result.lstData = lstData_resutl.ToList();
                }
            }

            #endregion
            return View(result);
        }
        public ActionResult AcKnowledgePreInternForm(string qryStr)
        {
            List<vRatingforHistory> lstRate = new List<vRatingforHistory>();
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code
            vAcKnowledge_obj_save result = new vAcKnowledge_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));

                if (nId != 0)
                {
                    bool isAdmin = CGlobal.UserIsAdmin();
                    bool isHRAdmin = CGlobal.UserIsHRAdmin();
                    var _getData = _TM_PR_Candidate_MappingService.FindForHR(nId, isAdmin, isHRAdmin);

                    if (_getData != null)
                    {
                        var _getRequest = _getData.PersonnelRequest;

                        if (_getRequest != null)
                        {
                            result.lstApprove = new List<vPersonnelAp_obj>();

                            result.IdEncrypt = qryStr;
                            result.group_id = _getRequest.TM_Divisions.division_name_en;
                            result.rank_id = _getRequest.TM_Pool_Rank != null ? _getRequest.TM_Pool_Rank.Pool_rank_name_en + "" : "-";
                            result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                            result.activities_Id = _getRequest.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities != null ? _getRequest.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities.Activities_name_en + "" : "-";

                            if (_getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank != null && _getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").Any())
                            {
                                result.lstrank.AddRange(_getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Rank.piority).Select(s => new vSelect_PR { id = s.Id + "", name = s.Pool_rank_name_en }).ToList());
                            }
                            result.sub_group_id = _getRequest.TM_SubGroup != null ? _getRequest.TM_SubGroup.sub_group_name_en + "" : "-";
                            result.position_id = _getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "-";
                            result.employment_type_id = _getRequest.TM_Employment_Request.TM_Employment_Type != null ? _getRequest.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                            result.request_type_id = _getRequest.TM_Employment_Request.TM_Request_Type != null ? _getRequest.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";

                            result.candidate_name = _getData.TM_Candidates.first_name_en + " " + _getData.TM_Candidates.last_name_en;

                            if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm) && _getRequest.type_of_TIFForm == "N")
                            {
                                var _getPintern = _TM_PIntern_FormService.GetActivePInternForm();
                                result.TIF_type = _getRequest.type_of_TIFForm;
                                var _CheckData = _TM_Candidate_PInternService.FindByMappingID(_getData.Id);
                                result.objtifform = new vObject_of_tif();

                                if (_CheckData != null)
                                {
                                    result.recommended_rank_id = _CheckData.Recommended_Rank != null ? _CheckData.Recommended_Rank.Id + "" : "";
                                    result.confidentiality_agreement = _CheckData.confidentiality_agreement + "";
                                    result.comment = _CheckData.comments;
                                    result.PIA_status_id = _CheckData.TM_PIntern_Status != null ? _CheckData.TM_PIntern_Status.Id + "" : "";

                                    if (_CheckData.TM_Candidate_PIntern_Answer != null && _CheckData.TM_Candidate_PIntern_Answer.Any(a => a.active_status == "Y"))
                                    {

                                        ////Question By Ans
                                        _getPintern = _TM_PIntern_FormService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Form_Question.TM_PIntern_Form.Id);
                                        result.TIF_type = _getRequest.type_of_TIFForm;
                                        result.objtifform = new vObject_of_tif();
                                        result.objtifform.TIF_no = _getPintern.Id + "";
                                        result.objtifform.lstQuestion = (from lstQ in _getPintern.TM_PIntern_Form_Question
                                                                         select new vtif_list_question
                                                                         {
                                                                             id = lstQ.Id + "",
                                                                             question = lstQ.question,
                                                                             header = lstQ.header,
                                                                             nSeq = lstQ.seq,
                                                                             remark = "",
                                                                             rating = "",
                                                                             topic = lstQ.topic,
                                                                         }).ToList();

                                        //rating Ans
                                        result.objtifform.lstQuestion.ForEach(ed =>
                                        {
                                            var GetAnsP = _CheckData.TM_Candidate_PIntern_Answer.Where(w => w.TM_PIntern_Form_Question.Id + "" == ed.id).FirstOrDefault();
                                            if (GetAnsP != null)
                                            {
                                                ed.remark = GetAnsP.answer + "";
                                                ed.rating = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.Id + "" : "";
                                                ed.point = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.point + "" : "";
                                            }
                                        });

                                    }

                                    if (_CheckData.TM_Candidate_PIntern_Approv != null && _CheckData.TM_Candidate_PIntern_Approv.Any())
                                    {
                                        string[] aUser = _CheckData.TM_Candidate_PIntern_Approv.Select(s => s.Req_Approve_user).ToArray();
                                        string[] aApproveUser = _CheckData.TM_Candidate_PIntern_Approv.Select(s => s.Approve_user).ToArray();
                                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                        var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                        int nPriority = 1;
                                        result.lstApprove = (from Tif in _CheckData.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
                                                             from Emp in _getEmp.Where(w => w.EmpNo == Tif.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                             from AEmp in _getApproveEmp.Where(w => w.EmpNo == Tif.Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                             select new vPersonnelAp_obj
                                                             {
                                                                 Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(Tif.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                 nStep = nPriority++,
                                                                 app_name = Emp.EmpFullName + (Tif.Approve_status + "" == "Y" ? (Tif.Req_Approve_user + "" != Tif.Approve_user + "" ? "<br/>(by" + AEmp.EmpFullName + ")" : "") : ""),
                                                                 approve_date = Tif.Approve_date.HasValue ? Tif.Approve_date.Value.DateTimebyCulture() : "",
                                                                 app_code = Tif.Approve_status + "" == "" ? "Waiting" : (Tif.Approve_status + "" == "Y" ? "Confirmed" : "Reject"),
                                                             }).ToList();
                                    }

                                }
                                //select rating Citeria show by form
                                var _getPIARating = _TM_PIntern_RatingFormService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);

                                result.objtifform.lstRating = (from lstQ in _getPIARating.TM_PIntern_Rating
                                                               orderby lstQ.seq
                                                               select new lstDataSelect
                                                               {
                                                                   value = lstQ.Id + "",
                                                                   text = lstQ.rating_name_en + "",
                                                                   detail = lstQ.rating_description + "",
                                                                   nSeq = lstQ.seq + "",
                                                               }).ToList();
                                result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });

                            }

                            else if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm) && _getRequest.type_of_TIFForm == "M")
                            {
                                var _getPintern = _TM_PIntern_Mass_Form_QuestionService.GetActivePInternForm();
                                result.TIF_type = _getRequest.type_of_TIFForm;
                                var _CheckData = _TM_Candidate_PIntern_MassService.FindByMappingID(_getData.Id);
                                result.objtifform = new vObject_of_tif();

                                if (_CheckData != null)
                                {
                                    result.recommended_rank_id = _CheckData.Recommended_Rank != null ? _CheckData.Recommended_Rank.Id + "" : "";
                                    result.confidentiality_agreement = _CheckData.confidentiality_agreement + "";
                                    result.comment = _CheckData.comments;
                                    result.PIA_status_id = _CheckData.TM_PIntern_Status != null ? _CheckData.TM_PIntern_Status.Id + "" : "";

                                    if (_CheckData.TM_Candidate_PIntern_Mass_Answer != null && _CheckData.TM_Candidate_PIntern_Mass_Answer.Any(a => a.active_status == "Y"))
                                    {

                                        ////Question By Ans
                                        _getPintern = _TM_PIntern_Mass_Form_QuestionService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Mass_Question.TM_PIntern_Mass_Form_Question.Id);
                                        result.TIF_type = _getRequest.type_of_TIFForm;
                                        result.objtifform = new vObject_of_tif();
                                        result.objtifform.TIF_no = _getPintern.Id + "";
                                        result.objtifform.lstQuestion = (from lstQ in _getPintern.TM_PIntern_Mass_Question
                                                                         select new vtif_list_question
                                                                         {
                                                                             id = lstQ.Id + "",
                                                                             question = lstQ.question,
                                                                             header = lstQ.header,
                                                                             nSeq = lstQ.seq,
                                                                             remark = "",
                                                                             rating = "",
                                                                             topic = lstQ.topic,
                                                                         }).ToList();

                                        //rating Ans
                                        result.objtifform.lstQuestion.ForEach(ed =>
                                        {
                                            var GetAnsP = _CheckData.TM_Candidate_PIntern_Mass_Answer.Where(w => w.TM_PIntern_Mass_Question.Id + "" == ed.id).FirstOrDefault();
                                            if (GetAnsP != null)
                                            {
                                                ed.remark = GetAnsP.answer + "";
                                                ed.rating = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.Id + "" : "";
                                                ed.point = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.point + "" : "";
                                            }
                                        });

                                    }

                                    if (_CheckData.TM_Candidate_PIntern_Mass_Approv != null && _CheckData.TM_Candidate_PIntern_Mass_Approv.Any())
                                    {
                                        string[] aUser = _CheckData.TM_Candidate_PIntern_Mass_Approv.Select(s => s.Req_Approve_user).ToArray();
                                        string[] aApproveUser = _CheckData.TM_Candidate_PIntern_Mass_Approv.Select(s => s.Approve_user).ToArray();
                                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                        var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                        int nPriority = 1;
                                        result.lstApprove = (from Tif in _CheckData.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
                                                             from Emp in _getEmp.Where(w => w.EmpNo == Tif.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                             from AEmp in _getApproveEmp.Where(w => w.EmpNo == Tif.Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                             select new vPersonnelAp_obj
                                                             {
                                                                 Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(Tif.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                 nStep = nPriority++,
                                                                 app_name = Emp.EmpFullName + (Tif.Approve_status + "" == "Y" ? (Tif.Req_Approve_user + "" != Tif.Approve_user + "" ? "<br/>(by" + AEmp.EmpFullName + ")" : "") : ""),
                                                                 approve_date = Tif.Approve_date.HasValue ? Tif.Approve_date.Value.DateTimebyCulture() : "",
                                                                 app_code = Tif.Approve_status + "" == "" ? "Waiting" : (Tif.Approve_status + "" == "Y" ? "Confirmed" : "Reject"),
                                                             }).ToList();
                                    }

                                }
                                //select rating Citeria show by form
                                var _getPIARating = _TM_PIntern_RatingFormService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);

                                result.objtifform.lstRating = (from lstQ in _getPIARating.TM_PIntern_Rating
                                                               orderby lstQ.seq
                                                               select new lstDataSelect
                                                               {
                                                                   value = lstQ.Id + "",
                                                                   text = lstQ.rating_name_en + "",
                                                                   detail = lstQ.rating_description + "",
                                                                   nSeq = lstQ.seq + "",
                                                               }).ToList();
                                result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });

                            }
                        }
                        else
                        {

                            return RedirectToAction("ErrorNopermission", "MasterPage");

                        }

                    }
                }
            }
            return View(result);

            #endregion
        }

        #endregion

        #region Pre-Intern Report
        public ActionResult PreInternReportList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            bool isHRAdmin = CGlobal.UserIsHRAdmin();
            vTIFReport result = new vTIFReport();
            result.tif_type = "X";
            result.lstSubGroup = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPosition = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstActivity = new List<vSelect_Activity>() { new vSelect_Activity { id = "", name = " - Select - ", } };
            result.sub_group_id = "";
            result.position_id = "";
            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "rpTIFlst" + unixTimestamps;
            result.session = sSession;
            rpvTIFReport_Session objSession = new rpvTIFReport_Session();
            Session[sSession] = new rpvTIFReport_Session();

            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchTIFReport SearchItem = (CSearchTIFReport)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchTIFReport)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                var lst = _DivisionService.FindByCode(SearchItem.group_code);

                if (lst != null)
                {
                    if (lst.TM_SubGroup != null && lst.TM_SubGroup.Any())
                    {
                        result.lstSubGroup.AddRange(lst.TM_SubGroup.OrderBy(o => o.sub_group_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.sub_group_name_en }).ToList());
                    }
                    if (lst.TM_Position != null && lst.TM_Position.Any())
                    {
                        result.lstPosition.AddRange(lst.TM_Position.OrderBy(o => o.position_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.position_name_en }).ToList());
                    }

                }
                DateTime? dStart = null, dTo = null;

                if (!string.IsNullOrEmpty(SearchItem.target_start))
                {
                    dStart = SystemFunction.ConvertStringToDateTime(SearchItem.target_start, "");
                }

                if (!string.IsNullOrEmpty(SearchItem.target_end))
                {
                    dTo = SystemFunction.ConvertStringToDateTime(SearchItem.target_end, "");
                }
                int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
                int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
                List<vTIFReport_obj> lstData_resutl = new List<vTIFReport_obj>();
                List<TM_Candidate_PIntern> lstData = new List<TM_Candidate_PIntern>();
                List<TM_Candidate_PIntern_Mass> lstDataMass = new List<TM_Candidate_PIntern_Mass>();
                List<TM_PR_Candidate_Mapping> lstAllData = new List<TM_PR_Candidate_Mapping>();


                lstData = _TM_Candidate_PInternService.GetReportList(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code, SearchItem.group_code, aDivisionPermission,
                                 "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin, isHRAdmin).ToList();

                lstDataMass = _TM_Candidate_PIntern_MassService.GetReportList(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code, SearchItem.group_code, aDivisionPermission,
                                "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin, isHRAdmin).ToList();

                result.active_status = SearchItem.active_status;
                result.group_code = SearchItem.group_code;
                result.tif_type = SearchItem.tif_type;
                result.sub_group_id = SearchItem.sub_group_id;
                result.position_id = SearchItem.position_id;
                result.ref_no = SearchItem.ref_no;
                result.name = SearchItem.name;
                result.ActivitiesTrainee_code = SearchItem.ActivitiesTrainee_code;
                string BackUrl = Uri.EscapeDataString(qryStr);

                if (lstData.Any())
                {

                    lstAllData.AddRange(lstData.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                    objSession.lstDataPIntern = lstData.ToList();
                    Session[sSession] = objSession;
                    string[] aRecruit = lstAllData.Select(s => s.TM_Recruitment_Team.user_no).ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aRecruit.Contains(w.EmpNo)).ToList();
                    lstData_resutl = (from lstAD in lstAllData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id == 10)
                                      from lstTIF in lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_PIntern())
                                      from Emp in _getEmp.Where(w => w.EmpNo == lstAD.TM_Recruitment_Team.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      select new vTIFReport_obj
                                      {
                                          Id = lstAD.Id,
                                          IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                          refno = lstAD.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.Recommended_Rank != null ? lstTIF.Recommended_Rank.Pool_rank_short_name_en : "") : (lstTIF.Recommended_Rank != null ? lstTIF.Recommended_Rank.Pool_rank_short_name_en : ""),
                                          name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                          tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.TM_PIntern_Status != null ? lstTIF.TM_PIntern_Status.PIntern_short_name_en + "" : "") : (lstTIF.TM_PIntern_Status != null ? lstTIF.TM_PIntern_Status.PIntern_short_name_en + "" : ""),
                                          pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                          pr_type_id = lstAD.PersonnelRequest.type_of_TIFForm + "",
                                          hr_owner = lstAD.TM_Recruitment_Team != null ? Emp.EmpFullName : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          activities = lstAD.TM_PInternAssessment_Activities != null ? lstAD.TM_PInternAssessment_Activities.Activities_name_en + "" : "",


                                      }).ToList();
                    lstData_resutl.ForEach(ed =>
                    {

                        if (ed.pr_type_id == "N" || ed.pr_type_id == "M")
                        {
                            var _getTif = lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == ed.Id).FirstOrDefault();

                            if (_getTif != null)
                            {
                                if (_getTif.TM_Candidate_PIntern_Approv != null && _getTif.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y"))
                                {
                                    var _getFirst = _getTif.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                    var _getSecond = _getTif.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();

                                    if (_getFirst != null)
                                    {
                                        var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();

                                        if (_getEmpFirst != null)
                                        {
                                            ed.first_eva = _getEmpFirst.EmpFullName + "";
                                            ed.first_eva_date = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                        }

                                    }
                                    if (_getSecond != null)
                                    {
                                        var _getEmpSecond = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getSecond.Req_Approve_user).FirstOrDefault();

                                        if (_getEmpSecond != null)
                                        {
                                            ed.second_eva = _getEmpSecond.EmpFullName + "";
                                            ed.second_eva_date = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                        }

                                    }
                                }
                                var _getEmpHrAc = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getTif.acknowledge_user && _getTif.hr_acknowledge == "Y").FirstOrDefault();

                                if (_getEmpHrAc != null)
                                {
                                    ed.hr_ac = _getEmpHrAc.EmpFullName + "";
                                    ed.hr_acdate = _getTif.acknowledge_date.HasValue ? _getTif.acknowledge_date.Value.DateTimebyCulture() : "";
                                }

                            }
                        }


                    });

                    result.lstData = lstData_resutl.ToList();
                }
                if (lstDataMass.Any())
                {

                    lstAllData.AddRange(lstDataMass.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                    objSession.lstDataPInternMass = lstDataMass.ToList();
                    Session[sSession] = objSession;
                    string[] aRecruit = lstAllData.Select(s => s.TM_Recruitment_Team.user_no).ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aRecruit.Contains(w.EmpNo)).ToList();
                    lstData_resutl = (from lstAD in lstAllData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id == 10)
                                      from lstTIF in lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_PIntern_Mass())
                                      from Emp in _getEmp.Where(w => w.EmpNo == lstAD.TM_Recruitment_Team.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      select new vTIFReport_obj
                                      {
                                          Id = lstAD.Id,
                                          IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                          refno = lstAD.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.Recommended_Rank != null ? lstTIF.Recommended_Rank.Pool_rank_short_name_en : "") : (lstTIF.Recommended_Rank != null ? lstTIF.Recommended_Rank.Pool_rank_short_name_en : ""),
                                          name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                          tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.TM_PIntern_Status != null ? lstTIF.TM_PIntern_Status.PIntern_short_name_en + "" : "") : (lstTIF.TM_PIntern_Status != null ? lstTIF.TM_PIntern_Status.PIntern_short_name_en + "" : ""),
                                          pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                          pr_type_id = lstAD.PersonnelRequest.type_of_TIFForm + "",
                                          hr_owner = lstAD.TM_Recruitment_Team != null ? Emp.EmpFullName : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          activities = lstAD.TM_PInternAssessment_Activities != null ? lstAD.TM_PInternAssessment_Activities.Activities_name_en + "" : "",


                                      }).ToList();
                    lstData_resutl.ForEach(ed =>
                    {

                        if (ed.pr_type_id == "M")
                        {
                            var _getTif = lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == ed.Id).FirstOrDefault();

                            if (_getTif != null)
                            {
                                if (_getTif.TM_Candidate_PIntern_Mass_Approv != null && _getTif.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y"))
                                {
                                    var _getFirst = _getTif.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                    var _getSecond = _getTif.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();

                                    if (_getFirst != null)
                                    {
                                        var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();

                                        if (_getEmpFirst != null)
                                        {
                                            ed.first_eva = _getEmpFirst.EmpFullName + "";
                                            ed.first_eva_date = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                        }

                                    }
                                    if (_getSecond != null)
                                    {
                                        var _getEmpSecond = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getSecond.Req_Approve_user).FirstOrDefault();

                                        if (_getEmpSecond != null)
                                        {
                                            ed.second_eva = _getEmpSecond.EmpFullName + "";
                                            ed.second_eva_date = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                        }

                                    }
                                }
                                var _getEmpHrAc = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getTif.acknowledge_user && _getTif.hr_acknowledge == "Y").FirstOrDefault();

                                if (_getEmpHrAc != null)
                                {
                                    ed.hr_ac = _getEmpHrAc.EmpFullName + "";
                                    ed.hr_acdate = _getTif.acknowledge_date.HasValue ? _getTif.acknowledge_date.Value.DateTimebyCulture() : "";
                                }

                            }
                        }


                    });

                    result.lstData = lstData_resutl.ToList();
                }
            }

            #endregion
            return View(result);
        }
        public ActionResult PreInternReportForm(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code
            vAcKnowledge_obj_save result = new vAcKnowledge_obj_save();
            List<vRatingforHistory> lstRate = new List<vRatingforHistory>();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));

                if (nId != 0)
                {
                    bool isAdmin = CGlobal.UserIsAdmin();
                    bool isHRAdmin = CGlobal.UserIsHRAdmin();
                    var _getData = _TM_PR_Candidate_MappingService.FindForHR(nId, isAdmin, isHRAdmin);

                    if (_getData != null)
                    {
                        var _getRequest = _getData.PersonnelRequest;

                        if (_getRequest != null)
                        {
                            result.lstApprove = new List<vPersonnelAp_obj>();

                            result.IdEncrypt = qryStr;
                            result.group_id = _getRequest.TM_Divisions.division_name_en;
                            result.rank_id = _getRequest.TM_Pool_Rank != null ? _getRequest.TM_Pool_Rank.Pool_rank_name_en + "" : "-";
                            result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

                            if (_getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank != null && _getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").Any())
                            {
                                result.lstrank.AddRange(_getRequest.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Rank.piority).Select(s => new vSelect_PR { id = s.Id + "", name = s.Pool_rank_name_en }).ToList());
                            }
                            result.sub_group_id = _getRequest.TM_SubGroup != null ? _getRequest.TM_SubGroup.sub_group_name_en + "" : "-";
                            result.position_id = _getRequest.TM_Position != null ? _getRequest.TM_Position.position_name_en + "" : "-";
                            result.employment_type_id = _getRequest.TM_Employment_Request.TM_Employment_Type != null ? _getRequest.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                            result.request_type_id = _getRequest.TM_Employment_Request.TM_Request_Type != null ? _getRequest.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";
                            result.activities_Id = _getRequest.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities != null ? _getRequest.TM_PR_Candidate_Mapping.FirstOrDefault().TM_PInternAssessment_Activities.Activities_name_en + "" : "-";
                            result.candidate_name = _getData.TM_Candidates.first_name_en + " " + _getData.TM_Candidates.last_name_en;

                            if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm) && _getRequest.type_of_TIFForm + "" == "N")
                            {
                                var _CheckData = _TM_Candidate_PInternService.FindByMappingID(_getData.Id);
                                var _getTIF = _TM_PIntern_FormService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Form_Question.TM_PIntern_Form.Id);
                                var tifID = _CheckData.TM_Candidate_PIntern_Answer.Select(w => w.TM_PIntern_Form_Question.TM_PIntern_Form.active_status).FirstOrDefault();
                                result.TIF_type = _getRequest.type_of_TIFForm;
                                result.objtifform = new vObject_of_tif();

                                if (_CheckData != null)
                                {
                                    result.recommended_rank_id = _CheckData.Recommended_Rank != null ? _CheckData.Recommended_Rank.Id + "" : "";
                                    result.confidentiality_agreement = _CheckData.confidentiality_agreement + "";
                                    result.comment = _CheckData.comments;
                                    result.PIA_status_id = _CheckData.TM_PIntern_Status != null ? _CheckData.TM_PIntern_Status.Id + "" : "";

                                    if (_CheckData.TM_Candidate_PIntern_Answer != null && _CheckData.TM_Candidate_PIntern_Answer.Any(a => a.active_status == "Y"))
                                    {
                                        ////Question By Ans
                                        var _getPintern = _TM_PIntern_FormService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Form_Question.TM_PIntern_Form.Id);
                                        result.TIF_type = _getRequest.type_of_TIFForm;
                                        result.objtifform = new vObject_of_tif();
                                        result.objtifform.TIF_no = _getPintern.Id + "";
                                        result.objtifform.lstQuestion = (from lstQ in _getPintern.TM_PIntern_Form_Question
                                                                         select new vtif_list_question
                                                                         {
                                                                             id = lstQ.Id + "",
                                                                             question = lstQ.question,
                                                                             header = lstQ.header,
                                                                             nSeq = lstQ.seq,
                                                                             remark = "",
                                                                             rating = "",
                                                                             topic = lstQ.topic,
                                                                         }).ToList();

                                        //rating Ans
                                        result.objtifform.lstQuestion.ForEach(ed =>
                                        {
                                            var GetAnsP = _CheckData.TM_Candidate_PIntern_Answer.Where(w => w.TM_PIntern_Form_Question.Id + "" == ed.id).FirstOrDefault();

                                            if (GetAnsP != null)
                                            {
                                                ed.remark = GetAnsP.answer + "";
                                                ed.rating = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.rating_name_en + "" : "";
                                                ed.point = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.point + "" : "";
                                            }
                                        });


                                        var _getPIARating = _TM_PIntern_RatingFormService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);
                                        result.objtifform.lstRating = (from lstQ in _getPIARating.TM_PIntern_Rating
                                                                       orderby lstQ.seq
                                                                       select new lstDataSelect
                                                                       {
                                                                           value = lstQ.Id + "",
                                                                           text = lstQ.rating_name_en + "",
                                                                           detail = lstQ.rating_description + "",
                                                                           nSeq = lstQ.seq + "",
                                                                       }).ToList();

                                        result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                    }

                                    if (_CheckData.TM_Candidate_PIntern_Approv != null && _CheckData.TM_Candidate_PIntern_Approv.Any())
                                    {
                                        string[] aUser = _CheckData.TM_Candidate_PIntern_Approv.Select(s => s.Req_Approve_user).ToArray();
                                        string[] aApproveUser = _CheckData.TM_Candidate_PIntern_Approv.Select(s => s.Approve_user).ToArray();
                                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                        var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                        int nPriority = 1;
                                        result.lstApprove = (from Tif in _CheckData.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
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
                            }
                            else if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm) && _getRequest.type_of_TIFForm + "" == "M")
                            {
                                var _CheckData = _TM_Candidate_PIntern_MassService.FindByMappingID(_getData.Id);
                                var _getTIF = _TM_PIntern_Mass_Form_QuestionService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Mass_Question.TM_PIntern_Mass_Form_Question.Id);
                                var tifID = _CheckData.TM_Candidate_PIntern_Mass_Answer.Select(w => w.TM_PIntern_Mass_Question.TM_PIntern_Mass_Form_Question.active_status).FirstOrDefault();
                                result.TIF_type = _getRequest.type_of_TIFForm;
                                result.objtifform = new vObject_of_tif();

                                if (_CheckData != null)
                                {
                                    result.recommended_rank_id = _CheckData.Recommended_Rank != null ? _CheckData.Recommended_Rank.Id + "" : "";
                                    result.confidentiality_agreement = _CheckData.confidentiality_agreement + "";
                                    result.comment = _CheckData.comments;
                                    result.PIA_status_id = _CheckData.TM_PIntern_Status != null ? _CheckData.TM_PIntern_Status.Id + "" : "";

                                    if (_CheckData.TM_Candidate_PIntern_Mass_Answer != null && _CheckData.TM_Candidate_PIntern_Mass_Answer.Any(a => a.active_status == "Y"))
                                    {
                                        ////Question By Ans
                                        var _getPintern = _TM_PIntern_Mass_Form_QuestionService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Mass_Question.TM_PIntern_Mass_Form_Question.Id);
                                        result.TIF_type = _getRequest.type_of_TIFForm;
                                        result.objtifform = new vObject_of_tif();
                                        result.objtifform.TIF_no = _getPintern.Id + "";
                                        result.objtifform.lstQuestion = (from lstQ in _getPintern.TM_PIntern_Mass_Question
                                                                         select new vtif_list_question
                                                                         {
                                                                             id = lstQ.Id + "",
                                                                             question = lstQ.question,
                                                                             header = lstQ.header,
                                                                             nSeq = lstQ.seq,
                                                                             remark = "",
                                                                             rating = "",
                                                                             topic = lstQ.topic,
                                                                         }).ToList();

                                        //rating Ans
                                        result.objtifform.lstQuestion.ForEach(ed =>
                                        {
                                            var GetAnsP = _CheckData.TM_Candidate_PIntern_Mass_Answer.Where(w => w.TM_PIntern_Mass_Question.Id + "" == ed.id).FirstOrDefault();

                                            if (GetAnsP != null)
                                            {
                                                ed.remark = GetAnsP.answer + "";
                                                ed.rating = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.rating_name_en + "" : "";
                                                ed.point = GetAnsP.TM_PIntern_Rating != null ? GetAnsP.TM_PIntern_Rating.point + "" : "";
                                            }
                                        });


                                        var _getPIARating = _TM_PIntern_RatingFormService.GetActivePInternForm(_CheckData.TM_Candidate_PIntern_Mass_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_PIntern_Rating.TM_PIntern_RatingForm.Id);
                                        result.objtifform.lstRating = (from lstQ in _getPIARating.TM_PIntern_Rating
                                                                       orderby lstQ.seq
                                                                       select new lstDataSelect
                                                                       {
                                                                           value = lstQ.Id + "",
                                                                           text = lstQ.rating_name_en + "",
                                                                           detail = lstQ.rating_description + "",
                                                                           nSeq = lstQ.seq + "",
                                                                       }).ToList();

                                        result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                    }

                                    if (_CheckData.TM_Candidate_PIntern_Mass_Approv != null && _CheckData.TM_Candidate_PIntern_Mass_Approv.Any())
                                    {
                                        string[] aUser = _CheckData.TM_Candidate_PIntern_Mass_Approv.Select(s => s.Req_Approve_user).ToArray();
                                        string[] aApproveUser = _CheckData.TM_Candidate_PIntern_Mass_Approv.Select(s => s.Approve_user).ToArray();
                                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                        var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                        int nPriority = 1;
                                        result.lstApprove = (from Tif in _CheckData.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
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
                            }
                            else
                            {
                                result.TIF_type = "";
                                result.pr_no = _getRequest.RefNo + "";
                                string sUserNo = _getData.TM_Recruitment_Team.user_no + "";

                                if (!string.IsNullOrEmpty(sUserNo))
                                {
                                    var getOwner = dbHr.AllInfo_WS.Where(w => w.EmpNo == sUserNo).FirstOrDefault();
                                    result.null_tif_contact = getOwner != null ? getOwner.EmpFullName + "(" + getOwner.OfficePhone + ")" : "";
                                }
                            }
                        }
                    }

                    else
                    {

                        return RedirectToAction("ErrorNopermission", "MasterPage");

                    }
                }
            }
            return View(result);
            #endregion
        }
        #endregion

        #region for unacknowledge
        [HttpPost]
        public ActionResult unAcKnowledgePreIntern(string qryStr)
        {
            vInterview_Approver_Return result = new vInterview_Approver_Return();

            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            if (qryStr != "")
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));

                DateTime dNow = DateTime.Now;
                if (nId != 0)
                {
                    var _getmapping = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getmapping.PersonnelRequest.type_of_TIFForm + "" == "N")
                    {
                        var _getTIFForm = _TM_Candidate_PInternService.FindByMappingID(nId);
                        if (_getTIFForm != null && _getTIFForm.TM_Candidate_PIntern_Approv != null)
                        {
                            var _GetMap = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                            var CheckCount = _getTIFForm.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                            var CheckNotApprove = _getTIFForm.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();

                            if (CheckCount >= 2 && CheckNotApprove <= 0 && _GetMap != null)
                            {
                                _getTIFForm.hr_acknowledge = null;
                                _getTIFForm.acknowledge_date = null;
                                _getTIFForm.acknowledge_user = null;
                                _getTIFForm.update_user = CGlobal.UserInfo.UserId;
                                _getTIFForm.update_date = dNow;
                                _getTIFForm.TM_PR_Candidate_Mapping = _GetMap;
                                var sComplect = _TM_Candidate_PInternService.Update(_getTIFForm);

                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    string sError = "";
                                    string mail_to_log = "";
                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.HR_Ack);

                                    if (Mail1 != null)
                                    {
                                        var bSuss = SendHRPreInternAck(_getTIFForm, Mail1, ref sError, ref mail_to_log);
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Pleace try again.";
                                }
                            }
                            else if (CheckCount < 2)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Approval more than 2.";
                            }
                            else if (CheckNotApprove > 0)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, somone not approve.";
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
                            result.Msg = "Error, Pre Intern Form not found.";
                        }
                    }
                    else if (_getmapping.PersonnelRequest.type_of_TIFForm + "" == "M")
                    {
                        var _getTIFForm = _TM_Candidate_PIntern_MassService.FindByMappingID(nId);
                        if (_getTIFForm != null && _getTIFForm.TM_Candidate_PIntern_Mass_Approv != null)
                        {
                            var _GetMap = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                            var CheckCount = _getTIFForm.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                            var CheckNotApprove = _getTIFForm.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();

                            if (CheckCount >= 2 && CheckNotApprove <= 0 && _GetMap != null)
                            {
                                _getTIFForm.hr_acknowledge = null;
                                _getTIFForm.acknowledge_date = null;
                                _getTIFForm.acknowledge_user = null;
                                _getTIFForm.update_user = CGlobal.UserInfo.UserId;
                                _getTIFForm.update_date = dNow;
                                _getTIFForm.TM_PR_Candidate_Mapping = _GetMap;
                                var sComplect = _TM_Candidate_PIntern_MassService.Update(_getTIFForm);

                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    string sError = "";
                                    string mail_to_log = "";
                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.HR_Ack);

                                    if (Mail1 != null)
                                    {
                                        var bSuss = SendHRPreIntern_Mass_Ack(_getTIFForm, Mail1, ref sError, ref mail_to_log);
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Pleace try again.";
                                }
                            }
                            else if (CheckCount < 2)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Approval more than 2.";
                            }
                            else if (CheckNotApprove > 0)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, somone not approve.";
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
                            result.Msg = "Error, Pre Intern Form not found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Pre Intern Type not found.";
                    }

                }
            }
            return Json(new
            {
                result
            });
        }
        #endregion

        #region Ajax Function 1st-2nd Eva Pre-Intern

        [HttpPost]
        public ActionResult LoadPreInternAssessmentList(CSearchInterview SearchItem)
        {
            vInterview_Return result = new vInterview_Return();
            bool isAdmin = CGlobal.UserIsAdmin();
            List<vInterview_obj> lstData_resutl = new List<vInterview_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            var lstData = _TM_PR_Candidate_MappingService.GetDataForInterview(SearchItem.PR_No,
                            SearchItem.ActivitiesTrainee_code,
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
                var _GetPreInternform = _TM_Candidate_PInternService.FindByMappingArrayID(nMapID);
                var _GetPreInternformMass = _TM_Candidate_PIntern_MassService.FindByMappingArrayID(nMapID);

                if (_GetPreInternform.Any(a => a.submit_status == "Y"))
                {
                    lstData = lstData.Where(w => !_GetPreInternform.Where(w2 => w2.submit_status == "Y").Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray().Contains(w.Id)).ToList();
                }
                if (_GetPreInternformMass.Any(a => a.submit_status == "Y"))
                {
                    lstData = lstData.Where(w => !_GetPreInternformMass.Where(w2 => w2.submit_status == "Y").Select(s => s.TM_PR_Candidate_Mapping.Id).ToArray().Contains(w.Id)).ToList();
                }
                lstData_resutl = (from lstAD in lstData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id == 10)
                                  from lstTIF in _GetPreInternform.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_PIntern())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  select new vInterview_obj
                                  {
                                      refno = lstAD.PersonnelRequest.RefNo + "",
                                      group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                      position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                      rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                      name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                      status = lstAD.PersonnelRequest.type_of_TIFForm + "" != null ? lstTIF.submit_status != null ? (lstTIF.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting" : (lstTIF.submit_status != null ? (lstTIF.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting"),
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                      update_user = lstEmpUp.EmpFullName + "",
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault().Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                      activities = lstAD.TM_PInternAssessment_Activities != null ? lstAD.TM_PInternAssessment_Activities.Activities_name_en + "" : "",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;
            return Json(new { result });
        }
        [HttpPost]
        public ActionResult LoadApprovePreInternFormList(CSearchInterview SearchItem)
        {
            vInterview_Return result = new vInterview_Return();
            bool isAdmin = CGlobal.UserIsAdmin();
            List<vInterview_obj> lstData_resutl = new List<vInterview_obj>();
            List<vInterview_obj> lstData_resutlMass = new List<vInterview_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            List<TM_Candidate_PIntern_Approv> lstData = new List<TM_Candidate_PIntern_Approv>();
            List<TM_Candidate_PIntern_Mass_Approv> lstDataMass = new List<TM_Candidate_PIntern_Mass_Approv>();
            if (SearchItem.tif_type + "" == "")
            {
                lstData = _TM_Candidate_PIntern_ApprovService.GetPInternFormForApprove(SearchItem.PR_No,
                            SearchItem.ActivitiesTrainee_code,
                            SearchItem.group_code, aDivisionPermission,
                                                 CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();

                lstDataMass = _TM_Candidate_PIntern_Mass_ApprovService.GetPInternFormForApprove(SearchItem.PR_No,
                                               SearchItem.ActivitiesTrainee_code,
                                               SearchItem.group_code, aDivisionPermission,
                                              CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
            }
            else if (SearchItem.tif_type + "" == "M")
            {
                lstDataMass = _TM_Candidate_PIntern_Mass_ApprovService.GetPInternFormForApprove(SearchItem.PR_No,
                                                  SearchItem.ActivitiesTrainee_code,
                                                  SearchItem.group_code, aDivisionPermission,
                                                 CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin).ToList();
            }
            else if (SearchItem.tif_type + "" == "N")
            {
                lstData = _TM_Candidate_PIntern_ApprovService.GetPInternFormForApprove(SearchItem.PR_No,
                             SearchItem.ActivitiesTrainee_code,
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

            if (lstData.Any() || lstDataMass.Any())
            {
                if (lstData.Any())
                {
                    string[] aApproveUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                    var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                    lstData_resutl = (from lstAD in lstData.Where(w => w.TM_Candidate_PIntern.Recommended_Rank.TM_Rank.Id == 10)
                                      from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vInterview_obj
                                      {
                                          refno = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                          name_en = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                          status = !string.IsNullOrEmpty(lstAD.Approve_status) ? (lstAD.Approve_status == "Y" ? "Confirmed" : "Reject") : "Waiting",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          approval = AEmp.EmpFullName,
                                          pr_type = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                          activities = lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities != null ? lstAD.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities.Activities_name_en + "" : "",
                                      }).ToList();
                }
                if (lstDataMass.Any())
                {
                    string[] aApproveUser = lstDataMass.Select(s => s.Req_Approve_user).ToArray();
                    string[] aUpdateUser = lstDataMass.Select(s => s.update_user).ToArray();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                    var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                    lstData_resutlMass = (from lstAD in lstDataMass.Where(w => w.TM_Candidate_PIntern_Mass.Recommended_Rank.TM_Rank.Id == 10)
                                          from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                          from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                          select new vInterview_obj
                                          {
                                              refno = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest.RefNo + "",
                                              group_name = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Divisions.division_name_en + "",
                                              position = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Position.position_name_en + "",
                                              rank = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                              name_en = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.first_name_en + " " + lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_Candidates.last_name_en,
                                              status = !string.IsNullOrEmpty(lstAD.Approve_status) ? (lstAD.Approve_status == "Y" ? "Confirmed" : "Reject") : "Waiting",
                                              update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                              approval = AEmp.EmpFullName,
                                              pr_type = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                              activities = lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities != null ? lstAD.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.TM_PInternAssessment_Activities.Activities_name_en + "" : "",
                                          }).ToList();
                }


            }
            result.lstData = lstData_resutl.Concat(lstData_resutlMass).ToList();
            result.Status = SystemFunction.process_Success;
            return Json(new { result });
        }
        [HttpPost]
        public ActionResult CreatePreInternForm(vInterview_obj_save ItemData)
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
            var checkTif = ItemData.TIF_type;
            if (checkTif == "N")
            {
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
                                var _CheckDataPIntern = _TM_Candidate_PInternService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                                int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                                int nStatusId = SystemFunction.GetIntNullToZero(ItemData.PIA_status_id);
                                var _getTifForm = _TM_TIF_FormService.Find(nTFId);
                                var _getPInternForm = _TM_PIntern_FormService.Find(nTFId);
                                var _getStatus = _TM_PIntern_StatusService.Find(nStatusId);

                                if (_getStatus != null)
                                {
                                    if (_getPInternForm != null && _getPInternForm.TM_PIntern_Form_Question != null && _getPInternForm.TM_PIntern_Form_Question.Any())
                                    {
                                        var _getEmp = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && w.Employeeno == ItemData.user_no).FirstOrDefault();

                                        if (_getEmp != null && CGlobal.UserInfo.EmployeeNo != ItemData.user_no)
                                        {
                                            string[] aTIFQID = _getPInternForm.TM_PIntern_Form_Question.Select(s => s.Id + "").ToArray();
                                            List<TM_Candidate_PIntern_Answer> lstAns = new List<TM_Candidate_PIntern_Answer>();
                                            List<TM_Candidate_PIntern_Approv> lstApprove = new List<TM_Candidate_PIntern_Approv>();
                                            lstApprove.Add(new TM_Candidate_PIntern_Approv()
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
                                            lstApprove.Add(new TM_Candidate_PIntern_Approv()
                                            {
                                                Req_Approve_user = ItemData.user_no,
                                                active_status = "Y",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                seq = 2,
                                            });
                                            if (ItemData.objtifform.lstQuestion != null)
                                            {
                                                lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                                          from lstQ in _getPInternForm.TM_PIntern_Form_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PreInternAssessment.TM_PIntern_Form_Question())
                                                          select new TM_Candidate_PIntern_Answer
                                                          {
                                                              update_user = CGlobal.UserInfo.UserId,
                                                              update_date = dNow,
                                                              create_user = CGlobal.UserInfo.UserId,
                                                              create_date = dNow,
                                                              answer = lstA.remark,
                                                              active_status = "Y",
                                                              TM_PIntern_Form_Question = lstQ != null ? lstQ : null,
                                                              TM_PIntern_Rating = _TM_PIntern_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                                              TM_Candidate_PIntern = _CheckDataPIntern != null ? _CheckDataPIntern : null,
                                                          }).ToList();
                                            }

                                            TM_Candidate_PIntern objSave = new TM_Candidate_PIntern()
                                            {
                                                TM_PR_Candidate_Mapping = _getData.TM_PR_Candidate_Mapping,
                                                comments = ItemData.comment,
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                active_status = "Y",
                                                TM_Candidate_PIntern_Answer = lstAns,
                                                submit_status = "Y",
                                                TM_PIntern_Status = _getStatus,
                                                TM_Candidate_PIntern_Approv = lstApprove,
                                                confidentiality_agreement = ItemData.confidentiality_agreement,
                                                Recommended_Rank = _getRank != null ? _getRank : null,
                                            };
                                            var sComplect = _TM_Candidate_PInternService.CreateNewOrUpdate(ref objSave);

                                            if (sComplect > 0)
                                            {
                                                if (_CheckDataPIntern != null)
                                                {
                                                    var sComplectAns = _TM_Candidate_PIntern_AnswerService.UpdateAnswer(lstAns, _CheckDataPIntern.Id, CGlobal.UserInfo.UserId, dNow);
                                                    lstApprove.ForEach(ed => { ed.TM_Candidate_PIntern = _CheckDataPIntern; });
                                                    var sComplectApprove = _TM_Candidate_PIntern_ApprovService.CreateNewByList(lstApprove);

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
                                                        _TM_Candidate_PInternService.CreateNewOrUpdate(ref objSave);
                                                    }
                                                    else
                                                    {
                                                        result.Status = SystemFunction.process_Success;
                                                        string sError = "";
                                                        string mail_to_log = "";
                                                        var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Submit_PreInternForm);
                                                        if (objSave != null)
                                                        {
                                                            var bSuss = SendFirstPreInternFormSubmit(objSave, Mail1, ref sError, ref mail_to_log);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Success;
                                                    string sError = "";
                                                    string mail_to_log = "";
                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Submit_PreInternForm);

                                                    if (objSave != null)
                                                    {
                                                        var bSuss = SendFirstPreInternFormSubmit(objSave, Mail1, ref sError, ref mail_to_log);
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
                                        result.Msg = "Error, Pre-Intern Form Not Found.";
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
            }
            else if (checkTif == "M")
            {
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
                                var _CheckDataPIntern = _TM_Candidate_PIntern_MassService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                                int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                                int nStatusId = SystemFunction.GetIntNullToZero(ItemData.PIA_status_id);
                                var _getTifForm = _TM_TIF_FormService.Find(nTFId);
                                var _getPInternForm = _TM_PIntern_Mass_Form_QuestionService.Find(nTFId);
                                var _getStatus = _TM_PIntern_StatusService.Find(nStatusId);

                                if (_getStatus != null)
                                {
                                    if (_getPInternForm != null && _getPInternForm.TM_PIntern_Mass_Question != null && _getPInternForm.TM_PIntern_Mass_Question.Any())
                                    {
                                        var _getEmp = dbHr.Employee.Where(w => (w.C_Status == "1" || w.C_Status == "3") && w.Employeeno == ItemData.user_no).FirstOrDefault();

                                        if (_getEmp != null && CGlobal.UserInfo.EmployeeNo != ItemData.user_no)
                                        {
                                            string[] aTIFQID = _getPInternForm.TM_PIntern_Mass_Question.Select(s => s.Id + "").ToArray();
                                            List<TM_Candidate_PIntern_Mass_Answer> lstAns = new List<TM_Candidate_PIntern_Mass_Answer>();
                                            List<TM_Candidate_PIntern_Mass_Approv> lstApprove = new List<TM_Candidate_PIntern_Mass_Approv>();
                                            lstApprove.Add(new TM_Candidate_PIntern_Mass_Approv()
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
                                            lstApprove.Add(new TM_Candidate_PIntern_Mass_Approv()
                                            {
                                                Req_Approve_user = ItemData.user_no,
                                                active_status = "Y",
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                seq = 2,
                                            });
                                            if (ItemData.objtifform.lstQuestion != null)
                                            {
                                                lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                                          from lstQ in _getPInternForm.TM_PIntern_Mass_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PreInternAssessment.TM_PIntern_Mass_Question())
                                                          select new TM_Candidate_PIntern_Mass_Answer
                                                          {
                                                              update_user = CGlobal.UserInfo.UserId,
                                                              update_date = dNow,
                                                              create_user = CGlobal.UserInfo.UserId,
                                                              create_date = dNow,
                                                              answer = lstA.remark,
                                                              active_status = "Y",
                                                              TM_PIntern_Mass_Question = lstQ != null ? lstQ : null,
                                                              TM_PIntern_Rating = _TM_PIntern_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                                              TM_Candidate_PIntern_Mass = _CheckDataPIntern != null ? _CheckDataPIntern : null,
                                                          }).ToList();
                                            }

                                            TM_Candidate_PIntern_Mass objSave = new TM_Candidate_PIntern_Mass()
                                            {
                                                TM_PR_Candidate_Mapping = _getData.TM_PR_Candidate_Mapping,
                                                comments = ItemData.comment,
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                active_status = "Y",
                                                TM_Candidate_PIntern_Mass_Answer = lstAns,
                                                submit_status = "Y",
                                                TM_PIntern_Status = _getStatus,
                                                TM_Candidate_PIntern_Mass_Approv = lstApprove,
                                                confidentiality_agreement = ItemData.confidentiality_agreement,
                                                Recommended_Rank = _getRank != null ? _getRank : null,
                                            };
                                            var sComplect = _TM_Candidate_PIntern_MassService.CreateNewOrUpdate(ref objSave);

                                            if (sComplect > 0)
                                            {
                                                if (_CheckDataPIntern != null)
                                                {
                                                    var sComplectAns = _TM_Candidate_PIntern_Mass_AnswerService.UpdateAnswer(lstAns, _CheckDataPIntern.Id, CGlobal.UserInfo.UserId, dNow);
                                                    lstApprove.ForEach(ed => { ed.TM_Candidate_PIntern_Mass = _CheckDataPIntern; });
                                                    var sComplectApprove = _TM_Candidate_PIntern_Mass_ApprovService.CreateNewByList(lstApprove);

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
                                                        _TM_Candidate_PIntern_MassService.CreateNewOrUpdate(ref objSave);
                                                    }
                                                    else
                                                    {
                                                        result.Status = SystemFunction.process_Success;
                                                        string sError = "";
                                                        string mail_to_log = "";
                                                        var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Submit_PreInternForm);
                                                        if (objSave != null)
                                                        {
                                                            var bSuss = SendFirstPreIntern_Mass_FormSubmit(objSave, Mail1, ref sError, ref mail_to_log);
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    result.Status = SystemFunction.process_Success;
                                                    string sError = "";
                                                    string mail_to_log = "";
                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Submit_PreInternForm);

                                                    if (objSave != null)
                                                    {
                                                        var bSuss = SendFirstPreIntern_Mass_FormSubmit(objSave, Mail1, ref sError, ref mail_to_log);
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
                                        result.Msg = "Error, Pre-Intern Form Not Found.";
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
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Please Check Type Pre Intern";
            }


            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult SaveDraftPreInternForm(vInterview_obj_save ItemData)
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
                var checkTif = ItemData.TIF_type;

                if (checkTif == "N")
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
                            var _CheckData = _TM_Candidate_PInternService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                            int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                            int nStatusId = SystemFunction.GetIntNullToZero(ItemData.PIA_status_id);
                            var _getPInternForm = _TM_PIntern_FormService.Find(nTFId);
                            var _getTifForm = _TM_TIF_FormService.Find(nTFId);
                            var _getStatus = _TM_PIntern_StatusService.Find(nStatusId);

                            if (_getPInternForm != null && _getPInternForm.TM_PIntern_Form_Question != null && _getPInternForm.TM_PIntern_Form_Question.Any())
                            {
                                string[] aTIFQID = _getPInternForm.TM_PIntern_Form_Question.Select(s => s.Id + "").ToArray();
                                List<TM_Candidate_PIntern_Answer> lstAns = new List<TM_Candidate_PIntern_Answer>();

                                if (ItemData.objtifform.lstQuestion != null)
                                {
                                    lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                              from lstQ in _getPInternForm.TM_PIntern_Form_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PreInternAssessment.TM_PIntern_Form_Question())
                                              select new TM_Candidate_PIntern_Answer
                                              {
                                                  update_user = CGlobal.UserInfo.UserId,
                                                  update_date = dNow,
                                                  create_user = CGlobal.UserInfo.UserId,
                                                  create_date = dNow,
                                                  answer = lstA.remark,
                                                  active_status = "Y",
                                                  TM_PIntern_Form_Question = lstQ != null ? lstQ : null,
                                                  TM_PIntern_Rating = _TM_PIntern_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                                  TM_Candidate_PIntern = _CheckData != null ? _CheckData : null,

                                              }).ToList();
                                }
                                lstAns = lstAns.Where(w => w.TM_PIntern_Form_Question != null).ToList();
                                TM_Candidate_PIntern objSave = new TM_Candidate_PIntern()
                                {
                                    TM_PR_Candidate_Mapping = _getData.TM_PR_Candidate_Mapping,
                                    comments = ItemData.comment,
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    active_status = "Y",
                                    TM_Candidate_PIntern_Answer = lstAns,
                                    submit_status = "N",
                                    TM_PIntern_Status = _getStatus,
                                    confidentiality_agreement = ItemData.confidentiality_agreement,
                                    Recommended_Rank = _getRank != null ? _getRank : null,

                                };
                                var sComplect = _TM_Candidate_PInternService.CreateNewOrUpdate(ref objSave);

                                if (sComplect > 0)
                                {
                                    if (_CheckData != null)
                                    {
                                        if (_TM_Candidate_PIntern_AnswerService.UpdateAnswer(lstAns, _CheckData.Id, CGlobal.UserInfo.UserId, dNow) > 0)
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
                                result.Msg = "Error, Pre-Intern Form Not Found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Candidate Not Found.";
                        }
                    }
                }
                else if (checkTif == "M")
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
                            var _CheckData = _TM_Candidate_PIntern_MassService.FindByMappingID(_getData.TM_PR_Candidate_Mapping.Id);
                            int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                            int nStatusId = SystemFunction.GetIntNullToZero(ItemData.PIA_status_id);
                            var _getPInternForm = _TM_PIntern_Mass_Form_QuestionService.Find(nTFId);
                            var _getTifForm = _TM_TIF_FormService.Find(nTFId);
                            var _getStatus = _TM_PIntern_StatusService.Find(nStatusId);

                            if (_getPInternForm != null && _getPInternForm.TM_PIntern_Mass_Question != null && _getPInternForm.TM_PIntern_Mass_Question.Any())
                            {
                                string[] aTIFQID = _getPInternForm.TM_PIntern_Mass_Question.Select(s => s.Id + "").ToArray();
                                List<TM_Candidate_PIntern_Mass_Answer> lstAns = new List<TM_Candidate_PIntern_Mass_Answer>();

                                if (ItemData.objtifform.lstQuestion != null)
                                {
                                    lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                              from lstQ in _getPInternForm.TM_PIntern_Mass_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PreInternAssessment.TM_PIntern_Mass_Question())
                                              select new TM_Candidate_PIntern_Mass_Answer
                                              {
                                                  update_user = CGlobal.UserInfo.UserId,
                                                  update_date = dNow,
                                                  create_user = CGlobal.UserInfo.UserId,
                                                  create_date = dNow,
                                                  answer = lstA.remark,
                                                  active_status = "Y",
                                                  TM_PIntern_Mass_Question = lstQ != null ? lstQ : null,
                                                  TM_PIntern_Rating = _TM_PIntern_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                                  TM_Candidate_PIntern_Mass = _CheckData != null ? _CheckData : null,

                                              }).ToList();
                                }
                                lstAns = lstAns.Where(w => w.TM_PIntern_Mass_Question != null).ToList();
                                TM_Candidate_PIntern_Mass objSave = new TM_Candidate_PIntern_Mass()
                                {
                                    TM_PR_Candidate_Mapping = _getData.TM_PR_Candidate_Mapping,
                                    comments = ItemData.comment,
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    active_status = "Y",
                                    TM_Candidate_PIntern_Mass_Answer = lstAns,
                                    submit_status = "N",
                                    TM_PIntern_Status = _getStatus,
                                    confidentiality_agreement = ItemData.confidentiality_agreement,
                                    Recommended_Rank = _getRank != null ? _getRank : null,

                                };
                                var sComplect = _TM_Candidate_PIntern_MassService.CreateNewOrUpdate(ref objSave);

                                if (sComplect > 0)
                                {
                                    if (_CheckData != null)
                                    {
                                        if (_TM_Candidate_PIntern_Mass_AnswerService.UpdateAnswer(lstAns, _CheckData.Id, CGlobal.UserInfo.UserId, dNow) > 0)
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
                                result.Msg = "Error, Pre-Intern Form Not Found.";
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
                    result.Msg = "Error, Please Check Type Pre Intern";
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
        public ActionResult ApprovePreInternForm(vInterview_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (ItemData != null && !string.IsNullOrEmpty(ItemData.approve_status))
            {
                var checkTif = ItemData.TIF_type;
                if (checkTif == "N")
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
                            var _getData = _TM_Candidate_PIntern_ApprovService.Find(nId);

                            if (_getData != null)
                            {
                                if (_getData.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                                {
                                    int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                                    int nStatusId = SystemFunction.GetIntNullToZero(ItemData.PIA_status_id);
                                    var _getStatus = _TM_PIntern_StatusService.Find(nStatusId);
                                    var _getPInternForm = _TM_PIntern_FormService.Find(nTFId);

                                    if (_getStatus != null)
                                    {
                                        if (_getPInternForm != null && _getPInternForm.TM_PIntern_Form_Question != null && _getPInternForm.TM_PIntern_Form_Question.Any())
                                        {
                                            var _GetCandidatePIntern = _TM_Candidate_PInternService.Find(_getData.TM_Candidate_PIntern.Id);
                                            _getData.Approve_status = ItemData.approve_status;
                                            _getData.Approve_date = dNow;
                                            _getData.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                            _getData.update_user = CGlobal.UserInfo.UserId;
                                            _getData.update_date = dNow;
                                            List<TM_Candidate_PIntern_Answer> lstAns = new List<TM_Candidate_PIntern_Answer>();
                                            string[] aTIFQID = _getPInternForm.TM_PIntern_Form_Question.Select(s => s.Id + "").ToArray();

                                            if (ItemData.objtifform.lstQuestion != null)
                                            {
                                                lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                                          from lstQ in _getPInternForm.TM_PIntern_Form_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PreInternAssessment.TM_PIntern_Form_Question())
                                                          select new TM_Candidate_PIntern_Answer
                                                          {
                                                              update_user = CGlobal.UserInfo.UserId,
                                                              update_date = dNow,
                                                              create_user = CGlobal.UserInfo.UserId,
                                                              create_date = dNow,
                                                              answer = lstA.remark,
                                                              active_status = "Y",
                                                              TM_PIntern_Form_Question = lstQ != null ? lstQ : null,
                                                              TM_PIntern_Rating = _TM_PIntern_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                                              TM_Candidate_PIntern = _GetCandidatePIntern,
                                                          }).ToList();

                                            }

                                            if (lstAns.Any(a => a.answer + "" == "" || a.TM_PIntern_Rating == null))
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
                                                var CheckAllApprove = _GetCandidatePIntern.TM_Candidate_PIntern_Approv.Where(w => w.seq < _getData.seq && (w.Approve_status + "" != "Y") && w.active_status == "Y").ToList();
                                                if (!CheckAllApprove.Any())
                                                {
                                                    var ApproveComplect = _TM_Candidate_PIntern_ApprovService.Update(_getData);
                                                    if (ApproveComplect > 0)
                                                    {
                                                        result.Status = SystemFunction.process_Success;
                                                        var _GetMap = _TM_PR_Candidate_MappingService.Find(_getData.TM_Candidate_PIntern.TM_PR_Candidate_Mapping.Id);

                                                        if (_GetCandidatePIntern != null && _GetMap != null)
                                                        {
                                                            var sComplectAns = 0;
                                                            _GetCandidatePIntern.confidentiality_agreement = ItemData.confidentiality_agreement;
                                                            _GetCandidatePIntern.Recommended_Rank = _getRank != null ? _getRank : null;
                                                            _GetCandidatePIntern.comments = ItemData.comment;
                                                            _GetCandidatePIntern.update_user = CGlobal.UserInfo.UserId;
                                                            _GetCandidatePIntern.update_date = dNow;
                                                            _GetCandidatePIntern.TM_PIntern_Status = _getStatus;
                                                            _GetCandidatePIntern.TM_PR_Candidate_Mapping = _GetMap;
                                                            var _SaveCandidateTIF = _TM_Candidate_PInternService.Update(_GetCandidatePIntern);


                                                            sComplectAns = _TM_Candidate_PIntern_AnswerService.UpdateAnswer(lstAns, _GetCandidatePIntern.Id, CGlobal.UserInfo.UserId, dNow);
                                                            #region Send mail 
                                                            //check approved
                                                            var _getCheckPreIntern = _TM_Candidate_PInternService.Find(_getData.TM_Candidate_PIntern.Id);
                                                            _getCheckPreIntern.TM_PR_Candidate_Mapping = _GetMap;
                                                            if (_getCheckPreIntern != null && _GetMap != null)
                                                            {
                                                                var CheckCount = _getCheckPreIntern.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                                                                var CheckNotApprove = _getCheckPreIntern.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                                                                if (CheckNotApprove <= 0)
                                                                {
                                                                    //to hr and complected
                                                                    string sError = "";
                                                                    string mail_to_log = "";
                                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Approve_HR_Change);
                                                                    if (Mail1 != null)
                                                                    {
                                                                        var bSuss = SendPreInternFormComplected(_getCheckPreIntern, Mail1, ref sError, ref mail_to_log);
                                                                    }
                                                                    string sError2 = "";
                                                                    string mail_to_log2 = "";
                                                                    var Mail2 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Send_to_HR);
                                                                    if (Mail2 != null)
                                                                    {
                                                                        var bSuss2 = SendPreInternFormToHR(_getCheckPreIntern, Mail2, ref sError2, ref mail_to_log2);
                                                                    }

                                                                }
                                                                else if (CheckNotApprove > 0)
                                                                {
                                                                    //send submit
                                                                    string sError = "";
                                                                    string mail_to_log = "";
                                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Submit_PreInternForm);
                                                                    if (Mail1 != null)
                                                                    {
                                                                        var bSuss = SendFirstPreInternFormSubmit(_getCheckPreIntern, Mail1, ref sError, ref mail_to_log);
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
                else if (checkTif == "M")
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
                            var _getData = _TM_Candidate_PIntern_Mass_ApprovService.Find(nId);

                            if (_getData != null)
                            {
                                if (_getData.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                                {
                                    int nTFId = SystemFunction.GetIntNullToZero(ItemData.objtifform.TIF_no);
                                    int nStatusId = SystemFunction.GetIntNullToZero(ItemData.PIA_status_id);
                                    var _getStatus = _TM_PIntern_StatusService.Find(nStatusId);
                                    var _getPInternForm = _TM_PIntern_Mass_Form_QuestionService.Find(nTFId);

                                    if (_getStatus != null)
                                    {
                                        if (_getPInternForm != null && _getPInternForm.TM_PIntern_Mass_Question != null && _getPInternForm.TM_PIntern_Mass_Question.Any())
                                        {
                                            var _GetCandidatePIntern = _TM_Candidate_PIntern_MassService.Find(_getData.TM_Candidate_PIntern_Mass.Id);
                                            _getData.Approve_status = ItemData.approve_status;
                                            _getData.Approve_date = dNow;
                                            _getData.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                            _getData.update_user = CGlobal.UserInfo.UserId;
                                            _getData.update_date = dNow;
                                            List<TM_Candidate_PIntern_Mass_Answer> lstAns = new List<TM_Candidate_PIntern_Mass_Answer>();
                                            string[] aTIFQID = _getPInternForm.TM_PIntern_Mass_Question.Select(s => s.Id + "").ToArray();

                                            if (ItemData.objtifform.lstQuestion != null)
                                            {
                                                lstAns = (from lstA in ItemData.objtifform.lstQuestion.Where(w => aTIFQID.Contains(w.id))
                                                          from lstQ in _getPInternForm.TM_PIntern_Mass_Question.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new Models.PreInternAssessment.TM_PIntern_Mass_Question())
                                                          select new TM_Candidate_PIntern_Mass_Answer
                                                          {
                                                              update_user = CGlobal.UserInfo.UserId,
                                                              update_date = dNow,
                                                              create_user = CGlobal.UserInfo.UserId,
                                                              create_date = dNow,
                                                              answer = lstA.remark,
                                                              active_status = "Y",
                                                              TM_PIntern_Mass_Question = lstQ != null ? lstQ : null,
                                                              TM_PIntern_Rating = _TM_PIntern_RatingService.Find(SystemFunction.GetIntNullToZero(lstA.rating)),
                                                              TM_Candidate_PIntern_Mass = _GetCandidatePIntern,
                                                          }).ToList();

                                            }

                                            if (lstAns.Any(a => a.answer + "" == "" || a.TM_PIntern_Rating == null))
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
                                                var CheckAllApprove = _GetCandidatePIntern.TM_Candidate_PIntern_Mass_Approv.Where(w => w.seq < _getData.seq && (w.Approve_status + "" != "Y") && w.active_status == "Y").ToList();
                                                if (!CheckAllApprove.Any())
                                                {
                                                    var ApproveComplect = _TM_Candidate_PIntern_Mass_ApprovService.Update(_getData);
                                                    if (ApproveComplect > 0)
                                                    {
                                                        result.Status = SystemFunction.process_Success;
                                                        var _GetMap = _TM_PR_Candidate_MappingService.Find(_getData.TM_Candidate_PIntern_Mass.TM_PR_Candidate_Mapping.Id);

                                                        if (_GetCandidatePIntern != null && _GetMap != null)
                                                        {
                                                            var sComplectAns = 0;
                                                            _GetCandidatePIntern.confidentiality_agreement = ItemData.confidentiality_agreement;
                                                            _GetCandidatePIntern.Recommended_Rank = _getRank != null ? _getRank : null;
                                                            _GetCandidatePIntern.comments = ItemData.comment;
                                                            _GetCandidatePIntern.update_user = CGlobal.UserInfo.UserId;
                                                            _GetCandidatePIntern.update_date = dNow;
                                                            _GetCandidatePIntern.TM_PIntern_Status = _getStatus;
                                                            _GetCandidatePIntern.TM_PR_Candidate_Mapping = _GetMap;
                                                            var _SaveCandidateTIF = _TM_Candidate_PIntern_MassService.Update(_GetCandidatePIntern);


                                                            sComplectAns = _TM_Candidate_PIntern_Mass_AnswerService.UpdateAnswer(lstAns, _GetCandidatePIntern.Id, CGlobal.UserInfo.UserId, dNow);
                                                            #region Send mail 
                                                            //check approved
                                                            var _getCheckPreIntern = _TM_Candidate_PIntern_MassService.Find(_getData.TM_Candidate_PIntern_Mass.Id);
                                                            _getCheckPreIntern.TM_PR_Candidate_Mapping = _GetMap;
                                                            if (_getCheckPreIntern != null && _GetMap != null)
                                                            {
                                                                var CheckCount = _getCheckPreIntern.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                                                                var CheckNotApprove = _getCheckPreIntern.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                                                                if (CheckNotApprove <= 0)
                                                                {
                                                                    //to hr and complected
                                                                    string sError = "";
                                                                    string mail_to_log = "";
                                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Approve_HR_Change);
                                                                    if (Mail1 != null)
                                                                    {
                                                                        var bSuss = SendPreIntern_Mass_FormComplected(_getCheckPreIntern, Mail1, ref sError, ref mail_to_log);
                                                                    }
                                                                    string sError2 = "";
                                                                    string mail_to_log2 = "";
                                                                    var Mail2 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Send_to_HR);
                                                                    if (Mail2 != null)
                                                                    {
                                                                        var bSuss2 = SendPreIntern_Mass_FormToHR(_getCheckPreIntern, Mail2, ref sError2, ref mail_to_log2);
                                                                    }

                                                                }
                                                                else if (CheckNotApprove > 0)
                                                                {
                                                                    //send submit
                                                                    string sError = "";
                                                                    string mail_to_log = "";
                                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Submit_PreInternForm);
                                                                    if (Mail1 != null)
                                                                    {
                                                                        var bSuss = SendFirstPreIntern_Mass_FormSubmit(_getCheckPreIntern, Mail1, ref sError, ref mail_to_log);
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
                    result.Msg = "Error, Pleas check Type Pre Intern";
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
        public ActionResult RollBackPreInternForm(vInterview_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();

            if (ItemData != null)
            {
                var checkTif = ItemData.TIF_type;
                if (checkTif == "N")
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
                        var _getData = _TM_Candidate_PIntern_ApprovService.Find(nId);

                        if (_getData != null)
                        {
                            if (_getData.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                            {
                                var ApproveComplect = _TM_Candidate_PIntern_ApprovService.RollBackStatus(_getData.TM_Candidate_PIntern.Id, CGlobal.UserInfo.UserId, dNow);
                                if (ApproveComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    string sError = "";
                                    string mail_to_log = "";
                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Evaluator_Rollback);
                                    var _getCheckPintern = _TM_Candidate_PInternService.Find(_getData.TM_Candidate_PIntern.Id);
                                    var _GetMap = _TM_PR_Candidate_MappingService.Find(_getCheckPintern.TM_PR_Candidate_Mapping.Id);

                                    if (_getCheckPintern != null && _GetMap != null)
                                    {
                                        int nSeq = SystemFunction.GetIntNullToZero(_getData.seq + "");
                                        _getCheckPintern.TM_PR_Candidate_Mapping = _GetMap;
                                        var bSuss = SendPreInternRollback(_getCheckPintern, Mail1, ItemData.rejeck, nSeq, ref sError, ref mail_to_log);
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
                else if (checkTif == "M")
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
                        var _getData = _TM_Candidate_PIntern_Mass_ApprovService.Find(nId);

                        if (_getData != null)
                        {
                            if (_getData.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                            {
                                var ApproveComplect = _TM_Candidate_PIntern_Mass_ApprovService.RollBackStatus(_getData.TM_Candidate_PIntern_Mass.Id, CGlobal.UserInfo.UserId, dNow);
                                if (ApproveComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    string sError = "";
                                    string mail_to_log = "";
                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Evaluator_Rollback);
                                    var _getCheckPintern = _TM_Candidate_PIntern_MassService.Find(_getData.TM_Candidate_PIntern_Mass.Id);
                                    var _GetMap = _TM_PR_Candidate_MappingService.Find(_getCheckPintern.TM_PR_Candidate_Mapping.Id);

                                    if (_getCheckPintern != null && _GetMap != null)
                                    {
                                        int nSeq = SystemFunction.GetIntNullToZero(_getData.seq + "");
                                        _getCheckPintern.TM_PR_Candidate_Mapping = _GetMap;
                                        var bSuss = SendPreIntern_Mass_Rollback(_getCheckPintern, Mail1, ItemData.rejeck, nSeq, ref sError, ref mail_to_log);
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
                var checkTif = ItemData.TIF_type;
                if (checkTif == "N")
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
                        var _getData = _TM_Candidate_PIntern_ApprovService.Find(nId);

                        if (_getData != null)
                        {
                            if (_getData.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                            {
                                var _getPInternForm = _TM_Candidate_PInternService.Find(_getData.TM_Candidate_PIntern.Id);
                                if (_getPInternForm != null && _getPInternForm.TM_Candidate_PIntern_Approv != null && _getPInternForm.TM_Candidate_PIntern_Approv.Any())
                                {
                                    if (!_getPInternForm.TM_Candidate_PIntern_Answer.Any(a => a.active_status == "Y") || _getPInternForm.TM_Candidate_PIntern_Answer.Any(a => a.answer + "" == "" && a.TM_PIntern_Rating == null && a.active_status == "Y"))
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
                                        var CheckAllApprove = _getPInternForm.TM_Candidate_PIntern_Approv.Where(w => w.seq < _getData.seq && (w.Approve_status + "" != "Y") && w.active_status == "Y").ToList();
                                        if (!CheckAllApprove.Any())
                                        {
                                            _getData.Approve_status = "Y";
                                            _getData.Approve_date = dNow;
                                            _getData.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                            _getData.update_user = CGlobal.UserInfo.UserId;
                                            var ApproveComplect = _TM_Candidate_PIntern_ApprovService.Update(_getData);

                                            if (ApproveComplect > 0)
                                            {
                                                result.Status = SystemFunction.process_Success;
                                                #region Send mail 
                                                //check approved
                                                var _getCheckPreIntern = _TM_Candidate_PInternService.Find(_getData.TM_Candidate_PIntern.Id);
                                                var _GetMap = _TM_PR_Candidate_MappingService.Find(_getPInternForm.TM_PR_Candidate_Mapping.Id);

                                                if (_getCheckPreIntern != null && _GetMap != null)
                                                {
                                                    _getCheckPreIntern.TM_PR_Candidate_Mapping = _GetMap;
                                                    _getCheckPreIntern.update_user = CGlobal.UserInfo.UserId;
                                                    _getCheckPreIntern.update_date = dNow;
                                                    _TM_Candidate_PInternService.ApprovePInternForm(_getCheckPreIntern);
                                                    var CheckCount = _getCheckPreIntern.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                                                    var CheckNotApprove = _getCheckPreIntern.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                                                    if (CheckNotApprove <= 0)
                                                    {
                                                        //to hr and complected
                                                        string sError = "";
                                                        string mail_to_log = "";
                                                        var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Approve);
                                                        if (Mail1 != null)
                                                        {
                                                            var bSuss = SendPreInternFormComplected(_getCheckPreIntern, Mail1, ref sError, ref mail_to_log);
                                                        }

                                                        string sError2 = "";
                                                        string mail_to_log2 = "";
                                                        var Mail2 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Send_to_HR);
                                                        if (Mail2 != null)
                                                        {
                                                            var bSuss2 = SendPreInternFormToHR(_getCheckPreIntern, Mail2, ref sError2, ref mail_to_log2);
                                                        }
                                                    }
                                                    else if (CheckNotApprove > 0)
                                                    {
                                                        //send submit
                                                        string sError = "";
                                                        string mail_to_log = "";
                                                        var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Submit_PreInternForm);
                                                        if (Mail1 != null)
                                                        {
                                                            var bSuss = SendFirstPreInternFormSubmit(_getCheckPreIntern, Mail1, ref sError, ref mail_to_log);
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
                                    result.Msg = "Error, Pre Intern Form not found.";
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
                            result.Msg = "Error, Pre Intern Form Not Found.";
                        }
                    }
                }
                else if (checkTif == "M")
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
                        var _getData = _TM_Candidate_PIntern_Mass_ApprovService.Find(nId);

                        if (_getData != null)
                        {
                            if (_getData.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || isAdmin)
                            {
                                var _getPInternForm = _TM_Candidate_PIntern_MassService.Find(_getData.TM_Candidate_PIntern_Mass.Id);
                                if (_getPInternForm != null && _getPInternForm.TM_Candidate_PIntern_Mass_Approv != null && _getPInternForm.TM_Candidate_PIntern_Mass_Approv.Any())
                                {
                                    if (!_getPInternForm.TM_Candidate_PIntern_Mass_Answer.Any(a => a.active_status == "Y") || _getPInternForm.TM_Candidate_PIntern_Mass_Answer.Any(a => a.answer + "" == "" && a.TM_PIntern_Rating == null && a.active_status == "Y"))
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
                                        var CheckAllApprove = _getPInternForm.TM_Candidate_PIntern_Mass_Approv.Where(w => w.seq < _getData.seq && (w.Approve_status + "" != "Y") && w.active_status == "Y").ToList();
                                        if (!CheckAllApprove.Any())
                                        {
                                            _getData.Approve_status = "Y";
                                            _getData.Approve_date = dNow;
                                            _getData.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                            _getData.update_user = CGlobal.UserInfo.UserId;
                                            var ApproveComplect = _TM_Candidate_PIntern_Mass_ApprovService.Update(_getData);

                                            if (ApproveComplect > 0)
                                            {
                                                result.Status = SystemFunction.process_Success;
                                                #region Send mail 
                                                //check approved
                                                var _getCheckPreIntern = _TM_Candidate_PIntern_MassService.Find(_getData.TM_Candidate_PIntern_Mass.Id);
                                                var _GetMap = _TM_PR_Candidate_MappingService.Find(_getPInternForm.TM_PR_Candidate_Mapping.Id);

                                                if (_getCheckPreIntern != null && _GetMap != null)
                                                {
                                                    _getCheckPreIntern.TM_PR_Candidate_Mapping = _GetMap;
                                                    _getCheckPreIntern.update_user = CGlobal.UserInfo.UserId;
                                                    _getCheckPreIntern.update_date = dNow;
                                                    _TM_Candidate_PIntern_MassService.ApprovePInternForm(_getCheckPreIntern);
                                                    var CheckCount = _getCheckPreIntern.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                                                    var CheckNotApprove = _getCheckPreIntern.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                                                    if (CheckNotApprove <= 0)
                                                    {
                                                        //to hr and complected
                                                        string sError = "";
                                                        string mail_to_log = "";
                                                        var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Approve);
                                                        if (Mail1 != null)
                                                        {
                                                            var bSuss = SendPreIntern_Mass_FormComplected(_getCheckPreIntern, Mail1, ref sError, ref mail_to_log);
                                                        }

                                                        string sError2 = "";
                                                        string mail_to_log2 = "";
                                                        var Mail2 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Send_to_HR);
                                                        if (Mail2 != null)
                                                        {
                                                            var bSuss2 = SendPreIntern_Mass_FormToHR(_getCheckPreIntern, Mail2, ref sError2, ref mail_to_log2);
                                                        }
                                                    }
                                                    else if (CheckNotApprove > 0)
                                                    {
                                                        //send submit
                                                        string sError = "";
                                                        string mail_to_log = "";
                                                        var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.Submit_PreInternForm);
                                                        if (Mail1 != null)
                                                        {
                                                            var bSuss = SendFirstPreIntern_Mass_FormSubmit(_getCheckPreIntern, Mail1, ref sError, ref mail_to_log);
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
                                    result.Msg = "Error, Pre Intern Form not found.";
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
                            result.Msg = "Error, Pre Intern Form Not Found.";
                        }
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Please check Type Pre Intern";
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

        #region Ajax Function Acknowledge HR Pre-Intern
        [HttpPost]
        public ActionResult LoadAcKnowledgeList(CSearchInterview SearchItem)
        {
            vAcKnowledge_Return result = new vAcKnowledge_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            bool isHRAdmin = CGlobal.UserIsHRAdmin();
            List<vAcKnowledge_obj> lstData_resutl = new List<vAcKnowledge_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            List<TM_Candidate_PIntern> lstData = new List<TM_Candidate_PIntern>();
            List<TM_Candidate_PIntern_Mass> lstDataMass = new List<TM_Candidate_PIntern_Mass>();
            List<TM_PR_Candidate_Mapping> lstAllData = new List<TM_PR_Candidate_Mapping>();

            if (SearchItem.tif_type + "" == "")
            {
                lstData = _TM_Candidate_PInternService.GetForAcknowledge(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code,
                                                  SearchItem.group_code, aDivisionPermission,
                                                 CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();

                lstDataMass = _TM_Candidate_PIntern_MassService.GetForAcknowledge(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code,
                                              SearchItem.group_code, aDivisionPermission,
                                             CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
            }
            else if (SearchItem.tif_type + "" == "N")
            {
                lstData = _TM_Candidate_PInternService.GetForAcknowledge(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code,
                                                  SearchItem.group_code, aDivisionPermission,
                                                 CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
            }
            else if (SearchItem.tif_type + "" == "M")
            {
                lstDataMass = _TM_Candidate_PIntern_MassService.GetForAcknowledge(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code,
                                                  SearchItem.group_code, aDivisionPermission,
                                                 CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
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
                if (lstData.Any())
                {
                    lstAllData.AddRange(lstData.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                }

                string[] aUpdateUser = lstAllData.Select(s => s.update_user).ToArray();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                lstData_resutl = (from lstAD in lstAllData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id == 10)
                                  from lstPIntern in lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_PIntern())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  select new vAcKnowledge_obj
                                  {
                                      Id = lstAD.Id,
                                      IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                      refno = lstAD.PersonnelRequest.RefNo + "",
                                      group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                      position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                      rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                      name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                      tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstPIntern.TM_PIntern_Status != null ? lstPIntern.TM_PIntern_Status.PIntern_short_name_en + "" : "") : (lstPIntern.TM_PIntern_Status != null ? lstPIntern.TM_PIntern_Status.PIntern_short_name_en + "" : ""),
                                      pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                      update_user = lstEmpUp.EmpFullName + "",
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      activities = lstAD.TM_PInternAssessment_Activities != null ? lstAD.TM_PInternAssessment_Activities.Activities_name_en + "" : "",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            if (lstDataMass.Any())
            {
                if (lstDataMass.Any())
                {
                    lstAllData.AddRange(lstDataMass.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                }
                string[] aUpdateUser = lstAllData.Select(s => s.update_user).ToArray();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                lstData_resutl = (from lstAD in lstAllData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id == 10)
                                  from lstPIntern in lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_PIntern_Mass())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  select new vAcKnowledge_obj
                                  {
                                      Id = lstAD.Id,
                                      IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                      refno = lstAD.PersonnelRequest.RefNo + "",
                                      group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                      position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                      rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en != null ? lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en : "",
                                      name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                      tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstPIntern.TM_PIntern_Status != null ? lstPIntern.TM_PIntern_Status.PIntern_short_name_en + "" : "") : (lstPIntern.TM_PIntern_Status != null ? lstPIntern.TM_PIntern_Status.PIntern_short_name_en + "" : ""),
                                      pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                      update_user = lstEmpUp.EmpFullName + "",
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      activities = lstAD.TM_PInternAssessment_Activities != null ? lstAD.TM_PInternAssessment_Activities.Activities_name_en + "" : "",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult AcKnowledgePreInternlst(vAcKnowledge_Return ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vAcKnowledge_Return result = new vAcKnowledge_Return();
            List<vAcKnowledge_lst> lstError = new List<vAcKnowledge_lst>();
            if (ItemData != null && ItemData.lstData != null && ItemData.lstData.Any())
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
                        var _GetMapping = _TM_PR_Candidate_MappingService.Find(nId);
                        if (_GetMapping != null)
                        {
                            if (_GetMapping.PersonnelRequest.type_of_TIFForm + "" == "N")
                            {
                                var _getPInternForm = _TM_Candidate_PInternService.FindByMappingID(_GetMapping.Id);
                                if (_getPInternForm != null && _getPInternForm.TM_Candidate_PIntern_Approv != null)
                                {
                                    var CheckCount = _getPInternForm.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                                    var CheckNotApprove = _getPInternForm.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();

                                    if (CheckCount >= 2 && CheckNotApprove <= 0)
                                    {
                                        var _getData = _TM_PR_Candidate_MappingService.Find(_getPInternForm.TM_PR_Candidate_Mapping.Id);
                                        var _getLastStatus = _getData.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault();
                                        var _getStatus = _TM_Candidate_StatusService.Find(_getPInternForm.TM_PIntern_Status.status_id.Value);

                                        if (_getPInternForm.TM_PIntern_Status.status_id.HasValue && _getData != null && _GetMapping != null)
                                        {
                                            TM_Candidate_Status_Cycle objCycle = new TM_Candidate_Status_Cycle()
                                            {
                                                seq = _getLastStatus.seq + 1,
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                active_status = "Y",
                                                TM_Candidate_Status = _getStatus,
                                                action_date = dNow,
                                                TM_PR_Candidate_Mapping = _getPInternForm.TM_PR_Candidate_Mapping,
                                            };
                                            _getPInternForm.hr_acknowledge = "Y";
                                            _getPInternForm.acknowledge_date = dNow;
                                            _getPInternForm.acknowledge_user = CGlobal.UserInfo.EmployeeNo;
                                            _getPInternForm.update_user = CGlobal.UserInfo.UserId;
                                            _getPInternForm.update_date = dNow;
                                            _getPInternForm.TM_PR_Candidate_Mapping = _GetMapping;
                                            var sComplect = _TM_Candidate_PInternService.Update(_getPInternForm);

                                            if (sComplect > 0)
                                            {
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.HR_Ack);
                                                if (Mail1 != null)
                                                {
                                                    var bSuss = SendHRPreInternAck(_getPInternForm, Mail1, ref sError, ref mail_to_log);
                                                }
                                            }
                                            else
                                            {
                                                lstError.Add(new vAcKnowledge_lst
                                                {
                                                    name_en = item.name_en,
                                                    status = "Y",
                                                    msg = "Error, Approval more than 2.",
                                                });
                                            }
                                        }
                                        else
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Error, Pleace try again.";
                                        }
                                    }
                                    else if (CheckNotApprove > 0)
                                    {
                                        lstError.Add(new vAcKnowledge_lst
                                        {
                                            name_en = item.name_en,
                                            status = "Y",
                                            msg = "Error, somone not approve.",
                                        });
                                    }
                                    else
                                    {
                                        lstError.Add(new vAcKnowledge_lst
                                        {
                                            name_en = item.name_en,
                                            status = "Y",
                                            msg = "Error, Error, Pre Intern Form not found.",
                                        });
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Pre Intern Form not found.";
                                    lstError.Add(new vAcKnowledge_lst
                                    {
                                        name_en = item.name_en,
                                        status = "Y",
                                        msg = "Error, Pre Intern Form not found.",
                                    });
                                }
                            }
                            else if (_GetMapping.PersonnelRequest.type_of_TIFForm + "" == "M")
                            {
                                var _getPInternForm = _TM_Candidate_PIntern_MassService.FindByMappingID(_GetMapping.Id);
                                if (_getPInternForm != null && _getPInternForm.TM_Candidate_PIntern_Mass_Approv != null)
                                {
                                    var CheckCount = _getPInternForm.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                                    var CheckNotApprove = _getPInternForm.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();

                                    if (CheckCount >= 2 && CheckNotApprove <= 0)
                                    {
                                        var _getData = _TM_PR_Candidate_MappingService.Find(_getPInternForm.TM_PR_Candidate_Mapping.Id);
                                        var _getLastStatus = _getData.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault();
                                        var _getStatus = _TM_Candidate_StatusService.Find(_getPInternForm.TM_PIntern_Status.status_id.Value);

                                        if (_getPInternForm.TM_PIntern_Status.status_id.HasValue && _getData != null && _GetMapping != null)
                                        {
                                            TM_Candidate_Status_Cycle objCycle = new TM_Candidate_Status_Cycle()
                                            {
                                                seq = _getLastStatus.seq + 1,
                                                update_user = CGlobal.UserInfo.UserId,
                                                update_date = dNow,
                                                create_user = CGlobal.UserInfo.UserId,
                                                create_date = dNow,
                                                active_status = "Y",
                                                TM_Candidate_Status = _getStatus,
                                                action_date = dNow,
                                                TM_PR_Candidate_Mapping = _getPInternForm.TM_PR_Candidate_Mapping,
                                            };
                                            _getPInternForm.hr_acknowledge = "Y";
                                            _getPInternForm.acknowledge_date = dNow;
                                            _getPInternForm.acknowledge_user = CGlobal.UserInfo.EmployeeNo;
                                            _getPInternForm.update_user = CGlobal.UserInfo.UserId;
                                            _getPInternForm.update_date = dNow;
                                            _getPInternForm.TM_PR_Candidate_Mapping = _GetMapping;
                                            var sComplect = _TM_Candidate_PIntern_MassService.Update(_getPInternForm);

                                            if (sComplect > 0)
                                            {
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.HR_Ack);
                                                if (Mail1 != null)
                                                {
                                                    var bSuss = SendHRPreIntern_Mass_Ack(_getPInternForm, Mail1, ref sError, ref mail_to_log);
                                                }
                                            }
                                            else
                                            {
                                                lstError.Add(new vAcKnowledge_lst
                                                {
                                                    name_en = item.name_en,
                                                    status = "Y",
                                                    msg = "Error, Approval more than 2.",
                                                });
                                            }
                                        }
                                        else
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Error, Pleace try again.";
                                        }
                                    }
                                    else if (CheckNotApprove > 0)
                                    {
                                        lstError.Add(new vAcKnowledge_lst
                                        {
                                            name_en = item.name_en,
                                            status = "Y",
                                            msg = "Error, somone not approve.",
                                        });
                                    }
                                    else
                                    {
                                        lstError.Add(new vAcKnowledge_lst
                                        {
                                            name_en = item.name_en,
                                            status = "Y",
                                            msg = "Error, Error, Pre Intern Form not found.",
                                        });
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Pre Intern Form not found.";
                                    lstError.Add(new vAcKnowledge_lst
                                    {
                                        name_en = item.name_en,
                                        status = "Y",
                                        msg = "Error, Pre Intern Form not found.",
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
        public ActionResult AcKnowledgePreIntern(vAcKnowledge_obj_save ItemData)
        {
            vInterview_Approver_Return result = new vInterview_Approver_Return();
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
                    var _getmapping = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getmapping.PersonnelRequest.type_of_TIFForm + "" == "N")
                    {
                        var _getPInternForm = _TM_Candidate_PInternService.FindByMappingID(nId);
                        if (_getPInternForm != null && _getPInternForm.TM_Candidate_PIntern_Approv != null)
                        {
                            var _GetMap = _TM_PR_Candidate_MappingService.Find(_getPInternForm.TM_PR_Candidate_Mapping.Id);
                            var CheckCount = _getPInternForm.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                            var CheckNotApprove = _getPInternForm.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();

                            if (CheckCount >= 2 && CheckNotApprove <= 0 && _GetMap != null)
                            {
                                _getPInternForm.hr_acknowledge = "Y";
                                _getPInternForm.acknowledge_date = dNow;
                                _getPInternForm.acknowledge_user = CGlobal.UserInfo.EmployeeNo;
                                _getPInternForm.update_user = CGlobal.UserInfo.UserId;
                                _getPInternForm.update_date = dNow;
                                _getPInternForm.TM_PR_Candidate_Mapping = _GetMap;
                                var sComplect = _TM_Candidate_PInternService.Update(_getPInternForm);

                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    string sError = "";
                                    string mail_to_log = "";
                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.HR_Ack);
                                    if (Mail1 != null)
                                    {
                                        var bSuss = SendHRPreInternAck(_getPInternForm, Mail1, ref sError, ref mail_to_log);
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Pleace try again.";
                                }
                            }
                            else if (CheckCount < 2)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Approval more than 2.";
                            }
                            else if (CheckNotApprove > 0)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, somone not approve.";
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Pre Intern Form not found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Pre Intern Form not found.";
                        }
                    }
                    else if (_getmapping.PersonnelRequest.type_of_TIFForm + "" == "M")
                    {
                        var _getPInternForm = _TM_Candidate_PIntern_MassService.FindByMappingID(nId);
                        if (_getPInternForm != null && _getPInternForm.TM_Candidate_PIntern_Mass_Approv != null)
                        {
                            var _GetMap = _TM_PR_Candidate_MappingService.Find(_getPInternForm.TM_PR_Candidate_Mapping.Id);
                            var CheckCount = _getPInternForm.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                            var CheckNotApprove = _getPInternForm.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();

                            if (CheckCount >= 2 && CheckNotApprove <= 0 && _GetMap != null)
                            {
                                _getPInternForm.hr_acknowledge = "Y";
                                _getPInternForm.acknowledge_date = dNow;
                                _getPInternForm.acknowledge_user = CGlobal.UserInfo.EmployeeNo;
                                _getPInternForm.update_user = CGlobal.UserInfo.UserId;
                                _getPInternForm.update_date = dNow;
                                _getPInternForm.TM_PR_Candidate_Mapping = _GetMap;
                                var sComplect = _TM_Candidate_PIntern_MassService.Update(_getPInternForm);

                                if (sComplect > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    string sError = "";
                                    string mail_to_log = "";
                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.HR_Ack);
                                    if (Mail1 != null)
                                    {
                                        var bSuss = SendHRPreIntern_Mass_Ack(_getPInternForm, Mail1, ref sError, ref mail_to_log);
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, Pleace try again.";
                                }
                            }
                            else if (CheckCount < 2)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Approval more than 2.";
                            }
                            else if (CheckNotApprove > 0)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, somone not approve.";
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Pre Intern Form not found.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Pre Intern Form not found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Pre Intern Type not found.";
                    }

                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult DelPreInternApprover(string ItemData)
        {
            vInterview_Approver_Return result = new vInterview_Approver_Return();
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
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData + ""));
                if (nId != 0)
                {
                    var _getmapping = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getmapping.PersonnelRequest.type_of_TIFForm + "" == "N")
                    {
                        var _getData = _TM_Candidate_PIntern_ApprovService.Find(nId);
                        if (_getData != null)
                        {

                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.active_status = "N";
                            var sComplect = _TM_Candidate_PIntern_ApprovService.Update(_getData);

                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getEditList = _TM_Candidate_PInternService.Find(_getData.TM_Candidate_PIntern.Id);

                                if (_getEditList != null && _getEditList.TM_Candidate_PIntern_Approv != null && _getEditList.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y"))
                                {
                                    string[] aUser = _getEditList.TM_Candidate_PIntern_Approv.Select(s => s.Req_Approve_user).ToArray();
                                    string[] aApproveUser = _getEditList.TM_Candidate_PIntern_Approv.Select(s => s.Approve_user).ToArray();
                                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                    var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                    int nPriority = 1;
                                    result.lstData = (from PIntern in _getEditList.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
                                                      from Emp in _getEmp.Where(w => w.EmpNo == PIntern.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                      from AEmp in _getApproveEmp.Where(w => w.EmpNo == PIntern.Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                      select new vPersonnelAp_obj
                                                      {
                                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(PIntern.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                          nStep = nPriority++,
                                                          app_name = Emp.EmpFullName + (PIntern.Approve_status + "" == "Y" ? (PIntern.Req_Approve_user + "" != PIntern.Approve_user + "" ? "<br/>(by" + AEmp.EmpFullName + ")" : "") : ""),
                                                          approve_date = PIntern.Approve_date.HasValue ? PIntern.Approve_date.Value.DateTimebyCulture() : "",
                                                          app_code = PIntern.Approve_status + "" == "" ? "Waiting" : (PIntern.Approve_status + "" == "Y" ? "Confirmed" : "Reject"),
                                                      }).ToList();
                                }
                                else
                                {
                                    result.lstData = new List<vPersonnelAp_obj>();
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
                            result.Msg = "Error, User Not Found.";
                        }
                    }
                    else if (_getmapping.PersonnelRequest.type_of_TIFForm + "" == "M")
                    {
                        var _getData = _TM_Candidate_PIntern_Mass_ApprovService.Find(nId);
                        if (_getData != null)
                        {

                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.active_status = "N";
                            var sComplect = _TM_Candidate_PIntern_Mass_ApprovService.Update(_getData);

                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                var _getEditList = _TM_Candidate_PIntern_MassService.Find(_getData.TM_Candidate_PIntern_Mass.Id);

                                if (_getEditList != null && _getEditList.TM_Candidate_PIntern_Mass_Approv != null && _getEditList.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y"))
                                {
                                    string[] aUser = _getEditList.TM_Candidate_PIntern_Mass_Approv.Select(s => s.Req_Approve_user).ToArray();
                                    string[] aApproveUser = _getEditList.TM_Candidate_PIntern_Mass_Approv.Select(s => s.Approve_user).ToArray();
                                    var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                    var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                    int nPriority = 1;
                                    result.lstData = (from PIntern in _getEditList.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
                                                      from Emp in _getEmp.Where(w => w.EmpNo == PIntern.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                      from AEmp in _getApproveEmp.Where(w => w.EmpNo == PIntern.Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                      select new vPersonnelAp_obj
                                                      {
                                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(PIntern.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                          nStep = nPriority++,
                                                          app_name = Emp.EmpFullName + (PIntern.Approve_status + "" == "Y" ? (PIntern.Req_Approve_user + "" != PIntern.Approve_user + "" ? "<br/>(by" + AEmp.EmpFullName + ")" : "") : ""),
                                                          approve_date = PIntern.Approve_date.HasValue ? PIntern.Approve_date.Value.DateTimebyCulture() : "",
                                                          app_code = PIntern.Approve_status + "" == "" ? "Waiting" : (PIntern.Approve_status + "" == "Y" ? "Confirmed" : "Reject"),
                                                      }).ToList();
                                }
                                else
                                {
                                    result.lstData = new List<vPersonnelAp_obj>();
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
                            result.Msg = "Error, User Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Type Pre intern Not Found.";
                    }

                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Group Not Found.";
                }
            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult HRRollBackPreInternForm(vAcKnowledge_obj_save ItemData)
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
                    var _getmapping = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getmapping.PersonnelRequest.type_of_TIFForm + "" == "N")
                    {
                        var _getData = _TM_Candidate_PInternService.FindByMappingID(nId);
                        if (_getData != null)
                        {
                            var ApproveComplect = _TM_Candidate_PIntern_ApprovService.RollBackStatus(_getData.Id, CGlobal.UserInfo.UserId, dNow);
                            if (ApproveComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.HR_Rollback);
                                var _getCheckPIntern = _TM_Candidate_PInternService.Find(_getData.Id);
                                var _GetMap = _TM_PR_Candidate_MappingService.Find(_getCheckPIntern.TM_PR_Candidate_Mapping.Id);

                                if (_getCheckPIntern != null && _GetMap != null)
                                {
                                    _getCheckPIntern.TM_PR_Candidate_Mapping = _GetMap;
                                    var bSuss = SendHRPReInternRollback(_getCheckPIntern, Mail1, ItemData.rejeck, ref sError, ref mail_to_log);
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
                            result.Msg = "Error, Pre Intern Form Not Found.";
                        }
                    }
                    else if (_getmapping.PersonnelRequest.type_of_TIFForm + "" == "M")
                    {
                        var _getData = _TM_Candidate_PIntern_MassService.FindByMappingID(nId);
                        if (_getData != null)
                        {
                            var ApproveComplect = _TM_Candidate_PIntern_Mass_ApprovService.RollBackStatus(_getData.Id, CGlobal.UserInfo.UserId, dNow);
                            if (ApproveComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.HR_Rollback);
                                var _getCheckPIntern = _TM_Candidate_PIntern_MassService.Find(_getData.Id);
                                var _GetMap = _TM_PR_Candidate_MappingService.Find(_getCheckPIntern.TM_PR_Candidate_Mapping.Id);

                                if (_getCheckPIntern != null && _GetMap != null)
                                {
                                    _getCheckPIntern.TM_PR_Candidate_Mapping = _GetMap;
                                    var bSuss = SendHRPReIntern_Mass_Rollback(_getCheckPIntern, Mail1, ItemData.rejeck, ref sError, ref mail_to_log);
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
                            result.Msg = "Error, Pre Intern Form Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Type pre intern not found.";
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
        public ActionResult HRResetPreInternForm(vAcKnowledge_obj_save ItemData)
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
                    var _getmapping = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getmapping.PersonnelRequest.type_of_TIFForm + "" == "N")
                    {
                        var _getData = _TM_Candidate_PInternService.FindByMappingID(nId);
                        if (_getData != null)
                        {
                            var ApproveComplect = _TM_Candidate_PIntern_ApprovService.RollBackStatus(_getData.Id, CGlobal.UserInfo.UserId, dNow);
                            if (ApproveComplect > 0)
                            {
                                _TM_Candidate_PIntern_AnswerService.InactiveAnswer(_getData.Id, CGlobal.UserInfo.UserId, dNow);

                                result.Status = SystemFunction.process_Success;
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.HR_Rollback);
                                var _getCheckPIntern = _TM_Candidate_PInternService.Find(_getData.Id);
                                var _GetMap = _TM_PR_Candidate_MappingService.Find(_getCheckPIntern.TM_PR_Candidate_Mapping.Id);

                                if (_getCheckPIntern != null && _GetMap != null)
                                {
                                    _getCheckPIntern.TM_PR_Candidate_Mapping = _GetMap;
                                    var bSuss = SendHRPReInternRollback(_getCheckPIntern, Mail1, ItemData.rejeck, ref sError, ref mail_to_log);
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
                            result.Msg = "Error, Pre Intern Form Not Found.";
                        }
                    }
                    else if (_getmapping.PersonnelRequest.type_of_TIFForm + "" == "M")
                    {
                        var _getData = _TM_Candidate_PInternService.FindByMappingID(nId);
                        if (_getData != null)
                        {
                            var ApproveComplect = _TM_Candidate_PIntern_Mass_ApprovService.RollBackStatus(_getData.Id, CGlobal.UserInfo.UserId, dNow);
                            if (ApproveComplect > 0)
                            {
                                _TM_Candidate_PIntern_Mass_AnswerService.InactiveAnswer(_getData.Id, CGlobal.UserInfo.UserId, dNow);

                                result.Status = SystemFunction.process_Success;
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPreInternForm.HR_Rollback);
                                var _getCheckPIntern = _TM_Candidate_PIntern_MassService.Find(_getData.Id);
                                var _GetMap = _TM_PR_Candidate_MappingService.Find(_getCheckPIntern.TM_PR_Candidate_Mapping.Id);

                                if (_getCheckPIntern != null && _GetMap != null)
                                {
                                    _getCheckPIntern.TM_PR_Candidate_Mapping = _GetMap;
                                    var bSuss = SendHRPReIntern_Mass_Rollback(_getCheckPIntern, Mail1, ItemData.rejeck, ref sError, ref mail_to_log);
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
                            result.Msg = "Error, Pre Intern Form Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Type Pre intern not found.";
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
        public ActionResult AddApprover(vAcKnowledge_obj_save ItemData)
        {
            vInterview_Approver_Return result = new vInterview_Approver_Return();
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
                    var _getmapping = _TM_PR_Candidate_MappingService.Find(nId);
                    if (_getmapping.PersonnelRequest.type_of_TIFForm + "" == "N")
                    {
                        var _CheckUser = dbHr.Employee.Where(w => w.Employeeno == ItemData.user_no && (w.C_Status == "1" || w.C_Status == "3")).FirstOrDefault();
                        if (_CheckUser != null)
                        {
                            var _getPInternForm = _TM_Candidate_PInternService.FindByMappingID(nId);
                            if (_getPInternForm != null && _getPInternForm.TM_Candidate_PIntern_Approv != null)
                            {
                                var CheckDup = _getPInternForm.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y" && w.Req_Approve_user == ItemData.user_no).FirstOrDefault();
                                if (CheckDup == null)
                                {
                                    List<TM_Candidate_PIntern_Approv> lstApprove = new List<TM_Candidate_PIntern_Approv>();
                                    lstApprove.Add(new TM_Candidate_PIntern_Approv()
                                    {

                                        Req_Approve_user = ItemData.user_no,
                                        active_status = "Y",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_Candidate_PIntern = _getPInternForm,
                                    });
                                    var sComplectApprove = _TM_Candidate_PIntern_ApprovService.CreateNewByList(lstApprove);
                                    if (sComplectApprove > 0)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                        var _getEditList = _TM_Candidate_PInternService.Find(_getPInternForm.Id);

                                        if (_getEditList != null && _getEditList.TM_Candidate_PIntern_Approv != null && _getEditList.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y"))
                                        {
                                            string[] aUser = _getEditList.TM_Candidate_PIntern_Approv.Select(s => s.Req_Approve_user).ToArray();
                                            string[] aApproveUser = _getEditList.TM_Candidate_PIntern_Approv.Select(s => s.Approve_user).ToArray();
                                            var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                            var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                            int nPriority = 1;
                                            result.lstData = (from Tif in _getEditList.TM_Candidate_PIntern_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
                                                              from Emp in _getEmp.Where(w => w.EmpNo == Tif.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                              from AEmp in _getApproveEmp.Where(w => w.EmpNo == Tif.Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                              select new vPersonnelAp_obj
                                                              {
                                                                  Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(Tif.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                  nStep = nPriority++,
                                                                  app_name = Emp.EmpFullName + (Tif.Approve_status + "" == "Y" ? (Tif.Req_Approve_user + "" != Tif.Approve_user + "" ? "<br/>(by" + AEmp.EmpFullName + ")" : "") : ""),
                                                                  approve_date = Tif.Approve_date.HasValue ? Tif.Approve_date.Value.DateTimebyCulture() : "",
                                                                  app_code = Tif.Approve_status + "" == "" ? "Waiting" : (Tif.Approve_status + "" == "Y" ? "Confirmed" : "Reject"),
                                                              }).ToList();
                                        }
                                        else
                                        {
                                            result.lstData = new List<vPersonnelAp_obj>();
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
                                    result.Msg = "Error, Duplicate Approval.";
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
                            result.Msg = "Error, User Not Found.";
                        }
                    }
                    else if (_getmapping.PersonnelRequest.type_of_TIFForm + "" == "M")
                    {
                        var _CheckUser = dbHr.Employee.Where(w => w.Employeeno == ItemData.user_no && (w.C_Status == "1" || w.C_Status == "3")).FirstOrDefault();
                        if (_CheckUser != null)
                        {
                            var _getPInternForm = _TM_Candidate_PIntern_MassService.FindByMappingID(nId);
                            if (_getPInternForm != null && _getPInternForm.TM_Candidate_PIntern_Mass_Approv != null)
                            {
                                var CheckDup = _getPInternForm.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y" && w.Req_Approve_user == ItemData.user_no).FirstOrDefault();
                                if (CheckDup == null)
                                {
                                    List<TM_Candidate_PIntern_Mass_Approv> lstApprove = new List<TM_Candidate_PIntern_Mass_Approv>();
                                    lstApprove.Add(new TM_Candidate_PIntern_Mass_Approv()
                                    {
                                        Req_Approve_user = ItemData.user_no,
                                        active_status = "Y",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_Candidate_PIntern_Mass = _getPInternForm,
                                    });
                                    var sComplectApprove = _TM_Candidate_PIntern_Mass_ApprovService.CreateNewByList(lstApprove);
                                    if (sComplectApprove > 0)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                        var _getEditList = _TM_Candidate_PIntern_MassService.Find(_getPInternForm.Id);

                                        if (_getEditList != null && _getEditList.TM_Candidate_PIntern_Mass_Approv != null && _getEditList.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y"))
                                        {
                                            string[] aUser = _getEditList.TM_Candidate_PIntern_Mass_Approv.Select(s => s.Req_Approve_user).ToArray();
                                            string[] aApproveUser = _getEditList.TM_Candidate_PIntern_Mass_Approv.Select(s => s.Approve_user).ToArray();
                                            var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                            var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                            int nPriority = 1;
                                            result.lstData = (from Tif in _getEditList.TM_Candidate_PIntern_Mass_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
                                                              from Emp in _getEmp.Where(w => w.EmpNo == Tif.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                              from AEmp in _getApproveEmp.Where(w => w.EmpNo == Tif.Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                                              select new vPersonnelAp_obj
                                                              {
                                                                  Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(Tif.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                  nStep = nPriority++,
                                                                  app_name = Emp.EmpFullName + (Tif.Approve_status + "" == "Y" ? (Tif.Req_Approve_user + "" != Tif.Approve_user + "" ? "<br/>(by" + AEmp.EmpFullName + ")" : "") : ""),
                                                                  approve_date = Tif.Approve_date.HasValue ? Tif.Approve_date.Value.DateTimebyCulture() : "",
                                                                  app_code = Tif.Approve_status + "" == "" ? "Waiting" : (Tif.Approve_status + "" == "Y" ? "Confirmed" : "Reject"),
                                                              }).ToList();
                                        }
                                        else
                                        {
                                            result.lstData = new List<vPersonnelAp_obj>();
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
                                    result.Msg = "Error, Duplicate Approval.";
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
                            result.Msg = "Error, User Not Found.";
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Type Pre intern Not Found.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, User Not Found.";
                }

            }
            return Json(new
            {
                result
            });
        }
        #endregion

        [HttpPost]
        public ActionResult LoadPreInternFormReportList(CSearchTIFReport SearchItem)
        {
            vTIFReport_Return result = new vTIFReport_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            bool isHRAdmin = CGlobal.UserIsHRAdmin();
            DateTime? dStart = null, dTo = null;

            if (!string.IsNullOrEmpty(SearchItem.target_start))
            {
                dStart = SystemFunction.ConvertStringToDateTime(SearchItem.target_start, "");
            }

            if (!string.IsNullOrEmpty(SearchItem.target_end))
            {
                dTo = SystemFunction.ConvertStringToDateTime(SearchItem.target_end, "");
            }

            List<vTIFReport_obj> lstData_resutl = new List<vTIFReport_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
            int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
            int nActivity = SystemFunction.GetIntNullToZero(SearchItem.ActivitiesTrainee_code + "");
            int nPR = SystemFunction.GetIntNullToZero(SearchItem.PR_No + "");
            List<TM_Candidate_PIntern> lstData = new List<TM_Candidate_PIntern>();
            List<TM_Candidate_PIntern_Mass> lstDataMass = new List<TM_Candidate_PIntern_Mass>();
            List<TM_PR_Candidate_Mapping> lstAllData = new List<TM_PR_Candidate_Mapping>();

            lstData = _TM_Candidate_PInternService.GetReportList(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code, SearchItem.group_code, aDivisionPermission,
                      "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin, isHRAdmin).ToList();

            lstDataMass = _TM_Candidate_PIntern_MassService.GetReportList(SearchItem.PR_No, SearchItem.ActivitiesTrainee_code, SearchItem.group_code, aDivisionPermission,
                   "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin, isHRAdmin).ToList();

            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });

            string BackUrl = Uri.EscapeDataString(qryStr);
            rpvTIFReport_Session objSession = new rpvTIFReport_Session();

            if (lstData.Any())
            {
                lstAllData.AddRange(lstData.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                objSession.lstDataPIntern = lstData.ToList();
                string[] aRecruit = lstAllData.Select(s => s.TM_Recruitment_Team.user_no).ToArray();
                var _getEmp = dbHr.AllInfo_WS.Where(w => aRecruit.Contains(w.EmpNo)).ToList();
                lstData_resutl = (from lstAD in lstAllData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id == 10)
                                  from lstTIF in lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_PIntern())
                                  from Emp in _getEmp.Where(w => w.EmpNo == lstAD.TM_Recruitment_Team.user_no).DefaultIfEmpty(new AllInfo_WS())
                                  select new vTIFReport_obj
                                  {
                                      Id = lstAD.Id,
                                      IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                      refno = lstAD.PersonnelRequest.RefNo + "",
                                      group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                      position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                      rank = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.Recommended_Rank != null ? lstTIF.Recommended_Rank.Pool_rank_short_name_en : "") : (lstTIF.Recommended_Rank != null ? lstTIF.Recommended_Rank.Pool_rank_short_name_en : ""),
                                      name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                      tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.TM_PIntern_Status != null ? lstTIF.TM_PIntern_Status.PIntern_short_name_en + "" : "") : (lstTIF.TM_PIntern_Status != null ? lstTIF.TM_PIntern_Status.PIntern_short_name_en + "" : ""),
                                      pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                      pr_type_id = lstAD.PersonnelRequest.type_of_TIFForm + "",
                                      hr_owner = lstAD.TM_Recruitment_Team != null ? Emp.EmpFullName : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      activities = lstAD.TM_PInternAssessment_Activities != null ? lstAD.TM_PInternAssessment_Activities.Activities_name_en + "" : "",
                                  }).ToList();

                lstData_resutl.ForEach(ed =>
                {
                    if (ed.pr_type_id == "N")
                    {
                        var _getTif = lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == ed.Id).FirstOrDefault();

                        if (_getTif != null)
                        {
                            if (_getTif.TM_Candidate_PIntern_Approv != null && _getTif.TM_Candidate_PIntern_Approv.Any(a => a.active_status == "Y"))
                            {
                                var _getFirst = _getTif.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                var _getSecond = _getTif.TM_Candidate_PIntern_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();

                                if (_getFirst != null)
                                {
                                    var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();
                                    if (_getEmpFirst != null)
                                    {
                                        ed.first_eva = _getEmpFirst.EmpFullName + "";
                                        ed.first_eva_date = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                    }

                                }

                                if (_getSecond != null)
                                {
                                    var _getEmpSecond = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getSecond.Req_Approve_user).FirstOrDefault();
                                    if (_getEmpSecond != null)
                                    {
                                        ed.second_eva = _getEmpSecond.EmpFullName + "";
                                        ed.second_eva_date = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                    }

                                }
                            }
                            var _getEmpHrAc = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getTif.acknowledge_user && _getTif.hr_acknowledge == "Y").FirstOrDefault();
                            if (_getEmpHrAc != null)
                            {
                                ed.hr_ac = _getEmpHrAc.EmpFullName + "";
                                ed.hr_acdate = _getTif.acknowledge_date.HasValue ? _getTif.acknowledge_date.Value.DateTimebyCulture() : "";
                            }

                        }
                    }
                });
                result.lstData = lstData_resutl.ToList();
            }
            if (lstDataMass.Any())
            {
                lstAllData.AddRange(lstDataMass.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                objSession.lstDataPInternMass = lstDataMass.ToList();
                string[] aRecruit = lstAllData.Select(s => s.TM_Recruitment_Team.user_no).ToArray();
                var _getEmp = dbHr.AllInfo_WS.Where(w => aRecruit.Contains(w.EmpNo)).ToList();
                lstData_resutl = (from lstAD in lstAllData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id == 10)
                                  from lstTIF in lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_PIntern_Mass())
                                  from Emp in _getEmp.Where(w => w.EmpNo == lstAD.TM_Recruitment_Team.user_no).DefaultIfEmpty(new AllInfo_WS())
                                  select new vTIFReport_obj
                                  {
                                      Id = lstAD.Id,
                                      IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                      refno = lstAD.PersonnelRequest.RefNo + "",
                                      group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                      position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                      rank = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.Recommended_Rank != null ? lstTIF.Recommended_Rank.Pool_rank_short_name_en : "") : (lstTIF.Recommended_Rank != null ? lstTIF.Recommended_Rank.Pool_rank_short_name_en : ""),
                                      name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                      tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.TM_PIntern_Status != null ? lstTIF.TM_PIntern_Status.PIntern_short_name_en + "" : "") : (lstTIF.TM_PIntern_Status != null ? lstTIF.TM_PIntern_Status.PIntern_short_name_en + "" : ""),
                                      pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                      pr_type_id = lstAD.PersonnelRequest.type_of_TIFForm + "",
                                      hr_owner = lstAD.TM_Recruitment_Team != null ? Emp.EmpFullName : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      activities = lstAD.TM_PInternAssessment_Activities != null ? lstAD.TM_PInternAssessment_Activities.Activities_name_en + "" : "",
                                  }).ToList();

                lstData_resutl.ForEach(ed =>
                {
                    if (ed.pr_type_id == "M")
                    {
                        var _getTif = lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == ed.Id).FirstOrDefault();

                        if (_getTif != null)
                        {
                            if (_getTif.TM_Candidate_PIntern_Mass_Approv != null && _getTif.TM_Candidate_PIntern_Mass_Approv.Any(a => a.active_status == "Y"))
                            {
                                var _getFirst = _getTif.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                var _getSecond = _getTif.TM_Candidate_PIntern_Mass_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();

                                if (_getFirst != null)
                                {
                                    var _getEmpFirst = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getFirst.Req_Approve_user).FirstOrDefault();
                                    if (_getEmpFirst != null)
                                    {
                                        ed.first_eva = _getEmpFirst.EmpFullName + "";
                                        ed.first_eva_date = _getFirst.Approve_date.HasValue ? _getFirst.Approve_date.Value.DateTimebyCulture() : "";
                                    }

                                }

                                if (_getSecond != null)
                                {
                                    var _getEmpSecond = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getSecond.Req_Approve_user).FirstOrDefault();
                                    if (_getEmpSecond != null)
                                    {
                                        ed.second_eva = _getEmpSecond.EmpFullName + "";
                                        ed.second_eva_date = _getSecond.Approve_date.HasValue ? _getSecond.Approve_date.Value.DateTimebyCulture() : "";
                                    }

                                }
                            }
                            var _getEmpHrAc = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getTif.acknowledge_user && _getTif.hr_acknowledge == "Y").FirstOrDefault();
                            if (_getEmpHrAc != null)
                            {
                                ed.hr_ac = _getEmpHrAc.EmpFullName + "";
                                ed.hr_acdate = _getTif.acknowledge_date.HasValue ? _getTif.acknowledge_date.Value.DateTimebyCulture() : "";
                            }

                        }
                    }
                });
                result.lstData = lstData_resutl.ToList();
            }
            Session[SearchItem.session] = objSession;
            result.Status = SystemFunction.process_Success;
            return Json(new
            {
                result
            });
        }
    }
}
#endregion