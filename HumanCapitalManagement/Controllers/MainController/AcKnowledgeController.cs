using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.TIFForm;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using HumanCapitalManagement.ViewModel.ReportVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.MainController
{
    public class AcKnowledgeController : BaseController
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
        private TM_Candidate_StatusService _TM_Candidate_StatusService;
        private TM_Candidate_MassTIFService _TM_Candidate_MassTIFService;
        private TM_Candidate_MassTIF_ApprovService _TM_Candidate_MassTIF_ApprovService;
        private TM_Mass_TIF_FormService _TM_Mass_TIF_FormService;
        private TM_Mass_ScoringService _TM_Mass_ScoringService;
        private MailContentService _MailContentService;
        private TM_Additional_InformationService _TM_Additional_InformationService;
        private TM_Additional_QuestionsService _TM_Additional_QuestionsService;
        private TM_Additional_AnswersService _TM_Additional_AnswersService;

        private DivisionService _DivisionService;

        private New_HRISEntities dbHr = new New_HRISEntities();
        public AcKnowledgeController(PersonnelRequestService PersonnelRequestService
                         , TM_Candidate_Status_CycleService TM_Candidate_Status_CycleService
                         , TM_TIF_FormService TM_TIF_FormService
                         , TM_TIF_RatingService TM_TIF_RatingService
                         , TM_TIF_StatusService TM_TIF_StatusService
                         , TM_Candidate_TIFService TM_Candidate_TIFService
                         , TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
                         , TM_Candidate_TIF_AnswerService TM_Candidate_TIF_AnswerService
                         , TM_Candidate_TIF_ApprovService TM_Candidate_TIF_ApprovService
            , TM_Candidate_StatusService TM_Candidate_StatusService
            , TM_Candidate_MassTIFService TM_Candidate_MassTIFService
            , TM_Candidate_MassTIF_ApprovService TM_Candidate_MassTIF_ApprovService
            , TM_Mass_TIF_FormService TM_Mass_TIF_FormService
            , TM_Mass_ScoringService TM_Mass_ScoringService
            , MailContentService MailContentService
            , TM_Additional_InformationService TM_Additional_InformationService
            , TM_Additional_QuestionsService TM_Additional_QuestionsService
            , TM_Additional_AnswersService TM_Additional_AnswersService
            , DivisionService DivisionService
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
            _TM_Candidate_StatusService = TM_Candidate_StatusService;
            _TM_Candidate_MassTIFService = TM_Candidate_MassTIFService;
            _TM_Candidate_MassTIF_ApprovService = TM_Candidate_MassTIF_ApprovService;
            _TM_Mass_TIF_FormService = TM_Mass_TIF_FormService;
            _TM_Mass_ScoringService = TM_Mass_ScoringService;
            _MailContentService = MailContentService;
            _TM_Additional_InformationService = TM_Additional_InformationService;
            _TM_Additional_QuestionsService = TM_Additional_QuestionsService;
            _TM_Additional_AnswersService = TM_Additional_AnswersService;
            _DivisionService = DivisionService;
        }
        // GET: AcKnowledge
        public ActionResult AcKnowledgeList(string qryStr)
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
                List<TM_Candidate_TIF> lstData = new List<TM_Candidate_TIF>();
                List<TM_Candidate_MassTIF> lstDataMass = new List<TM_Candidate_MassTIF>();
                List<TM_PR_Candidate_Mapping> lstAllData = new List<TM_PR_Candidate_Mapping>();
                if (SearchItem.tif_type + "" == "")
                {
                    lstData = _TM_Candidate_TIFService.GetForAcknowledge(
                                                      SearchItem.group_code, aDivisionPermission,
                                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
                    lstDataMass = _TM_Candidate_MassTIFService.GetForAcknowledge(
                                          SearchItem.group_code, aDivisionPermission,
                                         CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
                }
                else if (SearchItem.tif_type + "" == "N")
                {
                    lstData = _TM_Candidate_TIFService.GetForAcknowledge(
                                                      SearchItem.group_code, aDivisionPermission,
                                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
                }
                else if (SearchItem.tif_type + "" == "M")
                {
                    lstDataMass = _TM_Candidate_MassTIFService.GetForAcknowledge(
                                SearchItem.group_code, aDivisionPermission,
                               CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
                }
                result.active_status = SearchItem.active_status;
                result.group_code = SearchItem.group_code;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any() || lstDataMass.Any())
                {
                    if (lstData.Any())
                    {
                        lstAllData.AddRange(lstData.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                    }
                    if (lstDataMass.Any())
                    {
                        lstAllData.AddRange(lstDataMass.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                    }
                    string[] aUpdateUser = lstAllData.Select(s => s.update_user).ToArray();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    lstData_resutl = (from lstAD in lstAllData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id != 10)
                                      from lstTIF in lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_TIF())
                                      from lstMassTIF in lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_MassTIF())
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
                                          //status = lstAD.submit_status != null ? (lstAD.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting",
                                          tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.TM_TIF_Status != null ? lstTIF.TM_TIF_Status.tif_short_name_en + "" : "") : (lstMassTIF.TM_MassTIF_Status != null ? lstMassTIF.TM_MassTIF_Status.masstif_short_name_en + "" : ""),
                                          pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",

                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();
                    result.lstData = lstData_resutl.ToList();
                }
            }

            #endregion
            return View(result);
        }
        public ActionResult AcKnowledgeEdit(string qryStr)
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
                                result.TIF_type = _getRequest.type_of_TIFForm;
                                //var _getTIF = _TM_TIF_FormService.GetActiveTIFForm();
                                //if (_getTIF != null && _getTIF.TM_TIF_Form_Question != null)
                                //{
                                var _CheckData = _TM_Candidate_TIFService.FindByMappingID(_getData.Id);


                                result.objtifform = new vObject_of_tif();
                                //result.objtifform.TIF_no = _getTIF.Id + "";
                                //result.objtifform.lstQuestion = (from lstQ in _getTIF.TM_TIF_Form_Question
                                //                                 select new vtif_list_question
                                //                                 {
                                //                                     id = lstQ.Id + "",
                                //                                     question = lstQ.question,
                                //                                     header = lstQ.header,
                                //                                     nSeq = lstQ.seq,

                                //                                 }).ToList();

                                if (_CheckData != null)
                                {
                                    result.recommended_rank_id = _CheckData.Recommended_Rank != null ? _CheckData.Recommended_Rank.Id + "" : "";
                                    result.confidentiality_agreement = _CheckData.confidentiality_agreement + "";
                                    result.comment = _CheckData.comments;
                                    result.tif_status_id = _CheckData.TM_TIF_Status != null ? _CheckData.TM_TIF_Status.Id + "" : "";
                                    if (_CheckData.TM_Candidate_TIF_Answer != null)
                                    {

                                        if (_CheckData.TM_Candidate_TIF_Answer != null)
                                        {
                                            result.objtifform.lstQuestion = (from lstQ in _CheckData.TM_Candidate_TIF_Answer
                                                                             select new vtif_list_question
                                                                             {
                                                                                 id = lstQ.Id + "",
                                                                                 question = lstQ.TM_TIF_Form_Question.question,
                                                                                 header = lstQ.TM_TIF_Form_Question.header,
                                                                                 nSeq = lstQ.seq,
                                                                                 remark = lstQ.answer + "",
                                                                                 rating = lstQ.TM_TIF_Rating != null ? lstQ.TM_TIF_Rating.Id + "" : "",
                                                                             }).ToList();


                                        }
                                        //result.objtifform.lstQuestion.ForEach(ed =>
                                        //{
                                        //    var GetAns = _CheckData.TM_Candidate_TIF_Answer.Where(w => w.TM_TIF_Form_Question.Id + "" == ed.id).FirstOrDefault();
                                        //    if (GetAns != null)
                                        //    {
                                        //        ed.remark = GetAns.answer + "";
                                        //        ed.rating = GetAns.TM_TIF_Rating != null ? GetAns.TM_TIF_Rating.Id + "" : "";
                                        //    }

                                        //});
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
                                                                 Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(Tif.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                                                 nStep = nPriority++,
                                                                 app_name = Emp.EmpFullName + (Tif.Approve_status + "" == "Y" ? (Tif.Req_Approve_user + "" != Tif.Approve_user + "" ? "<br/>(by" + AEmp.EmpFullName + ")" : "") : ""),
                                                                 approve_date = Tif.Approve_date.HasValue ? Tif.Approve_date.Value.DateTimebyCulture() : "",
                                                                 app_code = Tif.Approve_status + "" == "" ? "Waiting" : (Tif.Approve_status + "" == "Y" ? "Confirmed" : "Reject"),
                                                             }).ToList();
                                    }

                                }
                                var _getTIF = _TM_TIF_FormService.GetActiveTIFForm(_CheckData.TM_Candidate_TIF_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_TIF_Form_Question.TM_TIF_Form.Id);
                                if (_getTIF.active_status == "Y")
                                {
                                    var lstRating = _TM_TIF_RatingService.GetDataForSelect();

                                    result.objtifform.lstRating = _TM_TIF_RatingService.GetDataForSelect().Select(s => new lstDataSelect
                                    {
                                        value = s.Id + "",
                                        text = s.rating_name_en + "",
                                        detail = s.rating_description + "",

                                    }).ToList();

                                    //lstRate = (from lstData in lstRating
                                    //           select new vRatingforHistory
                                    //           {
                                    //               text = lstData.rating_name_en + "",
                                    //               detail = lstData.rating_description + "",
                                    //              value = lstData.Id + "",
                                    //           }).ToList();

                                    //foreach(var item in lstRating)
                                    //{
                                    //    switch(item.seq)
                                    //    {
                                    //        case 1: result.r1Header = item.rating_name_en;
                                    //            result.r1Deatail = item.rating_description;

                                    //            break;
                                    //    }
                                    //}

                                    //result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });

                                }
                                else if (_getTIF.active_status == "N")
                                {
                                    var lstRatingOld = _TM_TIF_RatingService.GetDataForSelectOld();


                                    result.objtifform.lstRating = _TM_TIF_RatingService.GetDataForSelectOld().Select(s => new lstDataSelect
                                    {
                                        value = s.Id + "",
                                        text = s.rating_name_en + "",
                                        detail = s.rating_description + "",

                                    }).ToList();
                                    //lstRate = (from lstData in lstRatingOld
                                    //           select new vRatingforHistory
                                    //           {
                                    //               text = lstData.rating_name_en + "",
                                    //               detail = lstData.rating_description + "",
                                    //               value = lstData.Id + "",
                                    //           }).ToList();
                                }
                                //result.lstRating = lstRate.ToList();
                                //result.objtifform.lstRating = _TM_TIF_RatingService.GetDataForSelect().Select(s => new lstDataSelect
                                //{
                                //    value = s.Id + "",
                                //    text = s.rating_name_en + ""
                                //}).ToList();
                                result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });


                                //}

                            }
                            else if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm) && _getRequest.type_of_TIFForm == "M")
                            {

                                var _getMassTIF = _TM_Mass_TIF_FormService.GetActiveTIFForm();
                                ////check Active Form
                                var _CheckDatanew = _TM_Candidate_MassTIFService.FindByMappingID(_getData.Id);

                                //check Form Mass
                                var _ChkMass = _TM_Candidate_MassTIFService.FindByMappingID(_getData.Id);
                                var _formMass = _ChkMass.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question != null).FirstOrDefault();
                                var _chkMass = _formMass.TM_MassTIF_Form_Question.TM_Mass_TIF_Form.Id;
                                if (_chkMass != 0)
                                    _getMassTIF = _TM_Mass_TIF_FormService.Find(_chkMass);

                                if (_getMassTIF != null && _getMassTIF.TM_MassTIF_Form_Question != null)
                                {
                                    var _getMassForm = _TM_Mass_TIF_FormService.GetActiveMassTIFForm(_CheckDatanew.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").FirstOrDefault().TM_MassTIF_Form_Question.TM_Mass_TIF_Form.Id);
                                    var _CheckData = _TM_Candidate_MassTIFService.FindByMappingID(_getData.Id);
                                    if (_chkMass == 1)
                                    {
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

                                        result.objMasstifform.lstScoring = _TM_Mass_ScoringService.GetDataForSelectOld().Select(s => new lstDataSelectMass
                                        {
                                            value = s.Id + "",
                                            text = s.scoring_name_en + "",
                                            point = s.point,
                                            code = s.scoring_code
                                        }).ToList();
                                        result.objMasstifform.lstScoring.Insert(0, new lstDataSelectMass { value = "", text = " - Select - ", point = 0, code = "" });
                                    }
                                    else if (_chkMass >= 2)
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


                                    List<vMasstif_list_Auditing> lstAuditing = new List<vMasstif_list_Auditing>();
                                    if (_chkMass >= 2)
                                    {
                                        lstAuditing.Add(new vMasstif_list_Auditing
                                        {
                                            header = "Easy Question",
                                            nSeq = 1,
                                            id = 1 + "",
                                            answer = "",
                                            lstQuiz = _getMassTIF.TM_Mass_Auditing_Question != null ? _getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.group_question == "1").OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header, group_question = s.group_question }).ToList() : new List<lstDataSelect>(),
                                        });
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
                                    //result.objMasstifform = new vObject_of_Masstif();
                                    result.objMasstifform.lstAuditing = new List<vMasstif_list_Auditing>();
                                    result.objMasstifform.lstAuditing = lstAuditing.ToList();
                                    result.objMasstifform.lstAuditing_Qst = new List<lstDataSelect>();
                                    result.objMasstifform.lstAuditing_Qst.Insert(0, new lstDataSelect { value = "", text = " - Select - " });

                                    // Additional Information
                                    var GetAdditional_Information = _TM_Additional_InformationService.GetActiveTIFForm();
                                    if (GetAdditional_Information != null)
                                    {
                                        //result.objMasstifform.lstAdInfo = new List<vMassAdInfo_Question>();
                                        //result.objMasstifform.lstAdInfo = GetAdditional_Information.TM_Additional_Questions.Select(s => new vMassAdInfo_Question
                                        //{

                                        //    question = s.question + "",
                                        //    nID = s.Id + "",
                                        //    multi_answer = s.multi_answer + "",
                                        //    seq = s.seq,
                                        //    other_answer = "",
                                        //    lstAnswers = s.TM_Additional_Answers != null ? s.TM_Additional_Answers.Where(w => w.active_status == "Y").OrderBy(o => o.seq).Select(s2 => new vMassAdInfo_Answers
                                        //    {
                                        //        nID = s2.Id + "",
                                        //        answer = s2.answers,
                                        //        seq = s2.seq,

                                        //    }).ToList() : new List<vMassAdInfo_Answers>(),
                                        //    is_validate = s.is_validate,
                                        //    lstAnswersselect = new string[] { },
                                        //}).ToList();
                                        var GetAdditional_InformationOld = _TM_Additional_InformationService.GetActiveTIFFormOld();
                                        var GetAdditional_InformationNew = _TM_Additional_InformationService.GetActiveTIFForm();
                                        if (_chkMass >= 2)
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
                                        else
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
                                    }

                                    if (_CheckData != null)
                                    {
                                        result.recommended_rank_id = _CheckData.Recommended_Rank != null ? _CheckData.Recommended_Rank.Id + "" : "";
                                        result.confidentiality_agreement = _CheckData.confidentiality_agreement + "";
                                        result.set_type = _CheckData.TM_Mass_Question_Type != null ? _CheckData.TM_Mass_Question_Type.Id + "" : "";
                                        result.comment = _CheckData.comments;
                                        result.tif_status_id = _CheckData.TM_MassTIF_Status != null ? _CheckData.TM_MassTIF_Status.Id + "" : "";
                                        result.target_start = _CheckData.can_start_date.HasValue ? _CheckData.can_start_date.Value.DateTimebyCulture() : "";


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

                                                result.objMasstifform.lstScoring = _TM_Mass_ScoringService.GetDataForSelectOld().Select(s => new lstDataSelectMass
                                                {
                                                    value = s.Id + "",
                                                    text = s.scoring_name_en + "",
                                                    point = s.point,
                                                    code = s.scoring_code
                                                }).ToList();
                                                result.objMasstifform.lstScoring.Insert(0, new lstDataSelectMass { value = "", text = " - Select - ", point = 0, code = "" });
                                            }
                                            else if (_chkMass >= 2)
                                            {
                                                result.objMasstifform.lstQuestion.ForEach(ed =>
                                                {
                                                    var GetAns = _CheckData.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question.Id + "" == ed.id).FirstOrDefault();
                                                    //Get Ans Rating Comment 
                                                    ed.rating = GetAns.TM_TIF_Rating != null ? GetAns.TM_TIF_Rating.Id + "" : "";
                                                    ed.evidence = GetAns.evidence + "";
                                                    ed.scoring = GetAns.TM_Mass_Scoring != null ? GetAns.TM_Mass_Scoring.scoring_code + "" : "";

                                                });

                                                result.objMasstifform.lstScoring = _TM_Mass_ScoringService.GetDataForSelect().Select(s => new lstDataSelectMass
                                                {
                                                    value = s.Id + "",
                                                    text = s.scoring_name_en + "",
                                                    point = s.point,
                                                    code = s.scoring_code
                                                }).ToList();
                                                result.objMasstifform.lstScoring.Insert(0, new lstDataSelectMass { value = "", text = " - Select - ", point = 0, code = "" });
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
                                                result.objMasstifform.lstAuditing_Qst.AddRange(_getMassTIF.TM_Mass_Auditing_Question.Where(w => w.active_status == "Y" && w.TM_Mass_Question_Type.Id == _CheckData.TM_Mass_Question_Type.Id && w.group_question == null).OrderBy(o => o.seq).Select(s => new lstDataSelect { value = s.Id + "", text = s.seq + " : " + s.header }).ToList());
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
                                                                         Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Del('" + HCMFunc.Encrypt(Tif.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
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
                                        string sUserNo = _getData.TM_Recruitment_Team.user_no + "";
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
                        //Check Form
                        var _CheckPage = _TM_Candidate_TIFService.FindByMappingID(_getData.Id);
                        var _CheckPageMass = _TM_Candidate_MassTIFService.FindByMappingID(_getData.Id);
                        //if Have Ans
                        if (_CheckPage != null && _CheckPage.TM_Candidate_TIF_Answer != null && _CheckPage.TM_Candidate_TIF_Answer.Any())
                        {
                            var _ChekAns = _CheckPage.TM_Candidate_TIF_Answer.Where(w => w.TM_TIF_Form_Question != null).FirstOrDefault();
                            var formId = _ChekAns.TM_TIF_Form_Question.TM_TIF_Form.Id;
                            if (formId == 1)
                            {
                                return View("AcKnowledgeEdit", result);
                            }
                            else
                            {
                                return View("AcKnowledgeEditNew", result);
                            }
                        }
                        //If not Ans
                        else if (_CheckPage == null && _CheckPageMass == null)
                        {
                            return View("AcKnowledgeEditNew", result);
                        }
                        else
                        {
                            //Check Form Mass (New or Old)
                            var _CheckDataF = _TM_Candidate_MassTIFService.FindByMappingID(_getData.Id);
                            var _formId = _CheckDataF.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question != null).FirstOrDefault();
                            var _chkM = _formId.TM_MassTIF_Form_Question.TM_Mass_TIF_Form.Id;
                            if (_chkM >= 2)
                            {
                                return View("AcKnowledgeEditNew", result);
                            }
                            else
                            {
                                return View(result);
                            }
                        }
                    }
                }
            }

            //return View(result);
            return RedirectToAction("AcKnowledgeList", "AcKnowledge");
            #endregion
        }

        public ActionResult TIFFormReportList(string qryStr)
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
            result.sub_group_id = "";
            result.position_id = "";
            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "rpTIFlst" + unixTimestamps;
            result.session = sSession;
            rpvTIFReport_Session objSession = new rpvTIFReport_Session();
            Session[sSession] = new rpvTIFReport_Session();//new List<ListAdvanceTransaction>();
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
                List<TM_Candidate_TIF> lstData = new List<TM_Candidate_TIF>();
                List<TM_Candidate_MassTIF> lstDataMass = new List<TM_Candidate_MassTIF>();
                List<TM_PR_Candidate_Mapping> lstAllData = new List<TM_PR_Candidate_Mapping>();
                if (SearchItem.tif_type + "" == "X" || SearchItem.tif_type + "" == "")
                {
                    lstData = _TM_Candidate_TIFService.GetReportList(SearchItem.group_code, aDivisionPermission,
                                     "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin, isHRAdmin).ToList();
                    lstDataMass = _TM_Candidate_MassTIFService.GetReportList(
                                      SearchItem.group_code, aDivisionPermission,
                                     "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin, isHRAdmin).ToList();
                }
                else if (SearchItem.tif_type + "" == "N")
                {
                    lstData = _TM_Candidate_TIFService.GetReportList(SearchItem.group_code, aDivisionPermission,
                                     "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin, isHRAdmin).ToList();
                }
                else if (SearchItem.tif_type + "" == "M")
                {
                    lstDataMass = _TM_Candidate_MassTIFService.GetReportList(
                                      SearchItem.group_code, aDivisionPermission,
                                     "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin, isHRAdmin).ToList();
                }
                result.active_status = SearchItem.active_status;
                result.group_code = SearchItem.group_code;
                result.tif_type = SearchItem.tif_type;
                result.sub_group_id = SearchItem.sub_group_id;
                result.position_id = SearchItem.position_id;
                result.ref_no = SearchItem.ref_no;
                result.name = SearchItem.name;

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any() || lstDataMass.Any())
                {
                    if (lstData.Any())
                    {
                        lstAllData.AddRange(lstData.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                        objSession.lstData = lstData.ToList();
                    }
                    if (lstDataMass.Any())
                    {
                        lstAllData.AddRange(lstDataMass.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                        objSession.lstDataMass = lstDataMass.ToList();

                    }
                    Session[sSession] = objSession;
                    string[] aRecruit = lstAllData.Select(s => s.TM_Recruitment_Team.user_no).ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aRecruit.Contains(w.EmpNo)).ToList();
                    lstData_resutl = (from lstAD in lstAllData.Where(w => w.PersonnelRequest.TM_Pool_Rank.TM_Rank.Id != 10)
                                      from lstTIF in lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_TIF())
                                      from lstMassTIF in lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_MassTIF())
                                      from Emp in _getEmp.Where(w => w.EmpNo == lstAD.TM_Recruitment_Team.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      select new vTIFReport_obj
                                      {
                                          Id = lstAD.Id,
                                          IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                          refno = lstAD.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.Recommended_Rank != null ? lstTIF.Recommended_Rank.Pool_rank_short_name_en : "") : (lstMassTIF.Recommended_Rank != null ? lstMassTIF.Recommended_Rank.Pool_rank_short_name_en : ""),
                                          name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                          //status = lstAD.submit_status != null ? (lstAD.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting",
                                          tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.TM_TIF_Status != null ? lstTIF.TM_TIF_Status.tif_short_name_en + "" : "") : (lstMassTIF.TM_MassTIF_Status != null ? lstMassTIF.TM_MassTIF_Status.masstif_short_name_en + "" : ""),
                                          pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                          pr_type_id = lstAD.PersonnelRequest.type_of_TIFForm + "",
                                          hr_owner = lstAD.TM_Recruitment_Team != null ? Emp.EmpFullName : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",

                                      }).ToList();
                    lstData_resutl.ForEach(ed =>
                    {
                        if (ed.pr_type_id == "N")
                        {
                            var _getTif = lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == ed.Id).FirstOrDefault();
                            if (_getTif != null)
                            {
                                if (_getTif.TM_Candidate_TIF_Approv != null && _getTif.TM_Candidate_TIF_Approv.Any(a => a.active_status == "Y"))
                                {
                                    var _getFirst = _getTif.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                    var _getSecond = _getTif.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
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
                        else
                        {
                            var _getTif = lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == ed.Id).FirstOrDefault();
                            if (_getTif != null)
                            {
                                if (_getTif.TM_Candidate_MassTIF_Approv != null && _getTif.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y"))
                                {
                                    var _getFirst = _getTif.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                                    var _getSecond = _getTif.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
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
        public ActionResult TIFFormReportView(string qryStr)
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

                            result.candidate_name = _getData.TM_Candidates.first_name_en + " " + _getData.TM_Candidates.last_name_en;
                            if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm)
                                && _getRequest.type_of_TIFForm == "N")
                            {
                                result.TIF_type = _getRequest.type_of_TIFForm;
                                var _CheckData = _TM_Candidate_TIFService.FindByMappingID(_getData.Id);
                                var _getTIF = _TM_TIF_FormService.GetActiveTIFForm(_CheckData.TM_Candidate_TIF_Answer.Where(w => w.active_status == "Y").FirstOrDefault().TM_TIF_Form_Question.TM_TIF_Form.Id);

                                var tifID = _CheckData.TM_Candidate_TIF_Answer.Select(w => w.TM_TIF_Form_Question.TM_TIF_Form.active_status).FirstOrDefault();
                                //เช็คเงื่อนไขว่าเก่าใหม่
                                if (_getTIF.active_status == "Y")
                                {
                                    var lstRating = _TM_TIF_RatingService.GetDataForSelect();

                                    lstRate = (from lstData in lstRating
                                               select new vRatingforHistory
                                               {
                                                   text = lstData.rating_name_en + "",
                                                   detail = lstData.rating_description + ""
                                               }).ToList();



                                }
                                else if (_getTIF.active_status == "N")
                                {
                                    var lstRatingOld = _TM_TIF_RatingService.GetDataForSelectOld();

                                    lstRate = (from lstData in lstRatingOld
                                               select new vRatingforHistory
                                               {
                                                   text = lstData.rating_name_en + "",
                                                   detail = lstData.rating_description + "",
                                               }).ToList();
                                }
                                result.lstRating = lstRate.ToList();

                                result.objtifform = new vObject_of_tif();
                                if (_CheckData != null)
                                {
                                    result.recommended_rank_id = _CheckData.Recommended_Rank != null ? _CheckData.Recommended_Rank.Id + "" : "";
                                    result.confidentiality_agreement = _CheckData.confidentiality_agreement + "";
                                    result.comment = _CheckData.comments;
                                    result.tif_status_id = _CheckData.TM_TIF_Status != null ? _CheckData.TM_TIF_Status.Id + "" : "";
                                    if (_CheckData.TM_Candidate_TIF_Answer != null && _CheckData.TM_Candidate_TIF_Answer.Any(a => a.active_status == "Y"))
                                    {
                                        result.objtifform.lstQuestion = (from lstQ in _CheckData.TM_Candidate_TIF_Answer.Where(a => a.active_status == "Y")
                                                                         select new vtif_list_question
                                                                         {
                                                                             id = lstQ.Id + "",
                                                                             question = lstQ.TM_TIF_Form_Question.question,
                                                                             header = lstQ.TM_TIF_Form_Question.header,
                                                                             nSeq = lstQ.seq,
                                                                             remark = lstQ.answer + "",
                                                                             rating = lstQ.TM_TIF_Rating != null ? lstQ.TM_TIF_Rating.rating_name_en + "" : "",
                                                                         }).ToList();


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

                                    result.objtifform.lstRating.Insert(0, new lstDataSelect { value = "", text = " - Select - " });
                                    //}
                                }
                            }

                            else if (!string.IsNullOrEmpty(_getRequest.type_of_TIFForm) && _getRequest.type_of_TIFForm == "M")
                            {
                                var _getMassTIF = _TM_Mass_TIF_FormService.GetActiveTIFForm();

                                if (_getMassTIF != null && _getMassTIF.TM_MassTIF_Form_Question != null)
                                {
                                    //check Active Form
                                    var _CheckData = _TM_Candidate_MassTIFService.FindByMappingID(_getData.Id);

                                    result.TIF_type = _getRequest.type_of_TIFForm;

                                    result.objMasstifform = new vObject_of_Masstif();
                                    result.objMasstifform.TIF_no = _getMassTIF.Id + "";



                                    //check Form Mass
                                    var _formMass = _CheckData.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question != null).FirstOrDefault();
                                    var _chkMass = _formMass.TM_MassTIF_Form_Question.TM_Mass_TIF_Form.Id;
                                    if (_chkMass != 0)
                                        _getMassTIF = _TM_Mass_TIF_FormService.Find(_chkMass);
                                    //Question Old
                                    if (_chkMass == 1)
                                    {
                                        var _getMassForm = _TM_Mass_TIF_FormService.GetActiveMassTIFForm(_CheckData.TM_Candidate_MassTIF_Core.Where(w => w.active_status == "Y").FirstOrDefault().TM_MassTIF_Form_Question.TM_Mass_TIF_Form.Id);

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
                                    else if (_chkMass >= 2)
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



                                    List<vMasstif_list_Auditing> lstAuditing = new List<vMasstif_list_Auditing>();
                                    if (_chkMass >= 2)
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


                                        result.objMasstifform.lstScoring = _TM_Mass_ScoringService.GetDataForSelectOld().Select(s => new lstDataSelectMass
                                        {
                                            value = s.Id + "",
                                            text = s.scoring_name_en + "",
                                            point = s.point,
                                            code = s.scoring_code
                                        }).ToList();
                                        result.objMasstifform.lstScoring.Insert(0, new lstDataSelectMass { value = "", text = " - Select - ", point = 0, code = "" });
                                    }
                                    result.objMasstifform.lstAuditing = lstAuditing.ToList();
                                    // Additional Information

                                    //result.objMasstifform.lstAdInfo = new List<vMassAdInfo_Question>();


                                    if (_CheckData != null)
                                    {
                                        result.recommended_rank_id = _CheckData.Recommended_Rank != null ? _CheckData.Recommended_Rank.Id + "" : "";
                                        result.confidentiality_agreement = _CheckData.confidentiality_agreement + "";
                                        result.set_type = _CheckData.TM_Mass_Question_Type != null ? _CheckData.TM_Mass_Question_Type.Id + "" : "";
                                        result.comment = _CheckData.comments;
                                        result.tif_status_id = _CheckData.TM_MassTIF_Status != null ? _CheckData.TM_MassTIF_Status.Id + "" : "";
                                        result.target_start = _CheckData.can_start_date.HasValue ? _CheckData.can_start_date.Value.DateTimebyCulture() : "";


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
                                            else if (_chkMass >= 2)
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
                                        if (_CheckData.TM_Candidate_MassTIF_Audit_Qst != null && _CheckData.TM_Candidate_MassTIF_Audit_Qst.Any(a => a.active_status == "Y"))
                                        {
                                            result.objMasstifform.lstAuditing.ForEach(ed =>
                                            {
                                                var GetAns = _CheckData.TM_Candidate_MassTIF_Audit_Qst.Where(w => w.seq + "" == ed.id).FirstOrDefault();
                                                if (GetAns != null)
                                                {

                                                    ed.answer = GetAns.answer + "";
                                                    ed.scoring = GetAns.TM_Mass_Scoring != null ? GetAns.TM_Mass_Scoring.scoring_name_en + "" : "";
                                                    ed.Qst = GetAns.TM_Mass_Auditing_Question != null ? GetAns.TM_Mass_Auditing_Question.header + "" : "";
                                                }
                                            });
                                        }
                                        if (_CheckData.TM_Candidate_MassTIF_Additional != null && _CheckData.TM_Candidate_MassTIF_Additional.Any(a => a.active_status == "Y"))
                                        {

                                            //result.objMasstifform.lstAdInfo = _CheckData.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y").OrderBy(o => o.TM_Additional_Questions.seq).Select(s => new vMassAdInfo_Question
                                            //{

                                            //    question = s.TM_Additional_Questions.question + "",
                                            //    seq = s.TM_Additional_Questions.seq,
                                            //    other_answer = s.other_answer + "",
                                            //    lstAnswers = s.TM_Candidate_MassTIF_Adnl_Answer != null ? s.TM_Candidate_MassTIF_Adnl_Answer.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Additional_Answers.seq).Select(s2 => new vMassAdInfo_Answers
                                            //    {
                                            //        nID = s2.Id + "",
                                            //        answer = s2.TM_Additional_Answers.answers,
                                            //        seq = s2.TM_Additional_Answers.seq,

                                            //    }).ToList() : new List<vMassAdInfo_Answers>(),
                                            //}).ToList();
                                            var GetAdditional_InformationOld = _TM_Additional_InformationService.GetActiveTIFFormOld();
                                            var GetAdditional_InformationNew = _TM_Additional_InformationService.GetActiveTIFForm();
                                            if (_chkMass >= 2)
                                            {
                                                result.objMasstifform.lstAdInfo = new List<vMassAdInfo_Question>();
                                                result.objMasstifform.lstAdInfo = _CheckData.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y").OrderBy(o => o.TM_Additional_Questions.seq).Select(s => new vMassAdInfo_Question
                                                {
                                                    question = s.TM_Additional_Questions.header + "<br/>" + s.TM_Additional_Questions.question + "",
                                                    seq = s.TM_Additional_Questions.seq,
                                                    other_answer = s.other_answer + "",
                                                    lstAnswers = s.TM_Candidate_MassTIF_Adnl_Answer != null ? s.TM_Candidate_MassTIF_Adnl_Answer.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Additional_Answers.seq).Select(s2 => new vMassAdInfo_Answers
                                                    {
                                                        nID = s2.Id + "",
                                                        answer = s2.TM_Additional_Answers.answers,
                                                        seq = s2.TM_Additional_Answers.seq,

                                                    }).ToList() : new List<vMassAdInfo_Answers>(),
                                                }).ToList();
                                            }
                                            else
                                            {
                                                result.objMasstifform.lstAdInfo = new List<vMassAdInfo_Question>();
                                                result.objMasstifform.lstAdInfo = _CheckData.TM_Candidate_MassTIF_Additional.Where(a => a.active_status == "Y").OrderBy(o => o.TM_Additional_Questions.seq).Select(s => new vMassAdInfo_Question
                                                {
                                                    question = s.TM_Additional_Questions.header + "<br/>" + s.TM_Additional_Questions.question + "",
                                                    seq = s.TM_Additional_Questions.seq,
                                                    other_answer = s.other_answer + "",
                                                    lstAnswers = s.TM_Candidate_MassTIF_Adnl_Answer != null ? s.TM_Candidate_MassTIF_Adnl_Answer.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Additional_Answers.seq).Select(s2 => new vMassAdInfo_Answers
                                                    {
                                                        nID = s2.Id + "",
                                                        answer = s2.TM_Additional_Answers.answers,
                                                        seq = s2.TM_Additional_Answers.seq,

                                                    }).ToList() : new List<vMassAdInfo_Answers>(),
                                                }).ToList();
                                            }


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
                    //check Tif empty or No empty
                    var _CheckPage = _TM_Candidate_TIFService.FindByMappingID(_getData.Id);
                    var _CheckPageMass = _TM_Candidate_MassTIFService.FindByMappingID(_getData.Id);

                    //check for send View Non-Mass or Mass
                    if (_CheckPage != null && _CheckPage.TM_Candidate_TIF_Answer != null && _CheckPage.TM_Candidate_TIF_Answer.Any())
                    {
                        //Check Form Non Mass
                        var _CheckDataF = _TM_Candidate_TIFService.FindByMappingID(_getData.Id);
                        var formId = _CheckDataF.TM_Candidate_TIF_Answer.Where(w => w.TM_TIF_Form_Question != null).FirstOrDefault();
                        var _CheckF = formId.TM_TIF_Form_Question.TM_TIF_Form.Id;
                        if (_CheckF == 1)
                        {
                            return View("TIFFormReportView", result);
                        }
                        else
                        {
                            return View("TIFFormReportViewNew", result);
                        }
                    }
                    else if (_CheckPage == null && _CheckPageMass == null)
                    {
                        return View("TIFFormReportViewNew", result);
                    }
                    else
                    {
                        //Check Form Mass (New or Old)
                        var _CheckDataF = _TM_Candidate_MassTIFService.FindByMappingID(_getData.Id);
                        var _formId = _CheckDataF.TM_Candidate_MassTIF_Core.Where(w => w.TM_MassTIF_Form_Question != null).FirstOrDefault();
                        var _chkM = _formId.TM_MassTIF_Form_Question.TM_Mass_TIF_Form.Id;
                        if (_chkM >= 2)
                        {
                            return View("TIFFormReportViewNew", result);
                        }
                        else
                        {
                            return View(result);
                        }
                    }

                }

            }
            //return View(result);
            return RedirectToAction("TIFFormReportList", "AcKnowledge");

            #endregion
        }
        #region Ajax Function
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

            List<TM_Candidate_TIF> lstData = new List<TM_Candidate_TIF>();
            List<TM_Candidate_MassTIF> lstDataMass = new List<TM_Candidate_MassTIF>();
            List<TM_PR_Candidate_Mapping> lstAllData = new List<TM_PR_Candidate_Mapping>();
            if (SearchItem.tif_type + "" == "")
            {
                lstData = _TM_Candidate_TIFService.GetForAcknowledge(
                                                  SearchItem.group_code, aDivisionPermission,
                                                 CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
                lstDataMass = _TM_Candidate_MassTIFService.GetForAcknowledge(
                                      SearchItem.group_code, aDivisionPermission,
                                     CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
            }
            else if (SearchItem.tif_type + "" == "N")
            {
                lstData = _TM_Candidate_TIFService.GetForAcknowledge(
                                                  SearchItem.group_code, aDivisionPermission,
                                                 CGlobal.UserInfo.EmployeeNo, SearchItem.name, isAdmin, isHRAdmin).ToList();
            }
            else if (SearchItem.tif_type + "" == "M")
            {
                lstDataMass = _TM_Candidate_MassTIFService.GetForAcknowledge(
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
            if (lstData.Any() || lstDataMass.Any())
            {
                if (lstData.Any())
                {
                    lstAllData.AddRange(lstData.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                }
                if (lstDataMass.Any())
                {
                    lstAllData.AddRange(lstDataMass.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                }
                string[] aUpdateUser = lstAllData.Select(s => s.update_user).ToArray();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser);
                lstData_resutl = (from lstAD in lstAllData
                                  from lstTIF in lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_TIF())
                                  from lstMassTIF in lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_MassTIF())
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
                                      //status = lstAD.submit_status != null ? (lstAD.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting",
                                      tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.TM_TIF_Status != null ? lstTIF.TM_TIF_Status.tif_short_name_en + "" : "") : (lstMassTIF.TM_MassTIF_Status != null ? lstMassTIF.TM_MassTIF_Status.masstif_short_name_en + "" : ""),
                                      pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
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
        public ActionResult AcKnowledgeTIFlst(vAcKnowledge_Return ItemData)
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
                                var _getTIFForm = _TM_Candidate_TIFService.FindByMappingID(_GetMapping.Id);
                                if (_getTIFForm != null && _getTIFForm.TM_Candidate_TIF_Approv != null)
                                {
                                    var CheckCount = _getTIFForm.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                                    var CheckNotApprove = _getTIFForm.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                                    if (CheckCount >= 2 && CheckNotApprove <= 0)
                                    {
                                        var _getData = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                                        var _getLastStatus = _getData.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault();
                                        var _getStatus = _TM_Candidate_StatusService.Find(_getTIFForm.TM_TIF_Status.status_id.Value);
                                        if (_getTIFForm.TM_TIF_Status.status_id.HasValue && _getData != null && _GetMapping != null)
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
                                                TM_PR_Candidate_Mapping = _getTIFForm.TM_PR_Candidate_Mapping,
                                            };
                                            _getTIFForm.hr_acknowledge = "Y";
                                            _getTIFForm.acknowledge_date = dNow;
                                            _getTIFForm.acknowledge_user = CGlobal.UserInfo.EmployeeNo;
                                            _getTIFForm.update_user = CGlobal.UserInfo.UserId;
                                            _getTIFForm.update_date = dNow;
                                            _getTIFForm.TM_PR_Candidate_Mapping = _GetMapping;
                                            var sComplect = _TM_Candidate_TIFService.Update(_getTIFForm);
                                            if (sComplect > 0)
                                            {
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.HR_Ack);
                                                if (Mail1 != null)
                                                {
                                                    var bSuss = SendHRAck(_getTIFForm, Mail1, ref sError, ref mail_to_log);
                                                }
                                                // sComplect = _TM_Candidate_Status_CycleService.CreateNew(objCycle);

                                            }
                                            else
                                            {
                                                //result.Status = SystemFunction.process_Failed;
                                                //result.Msg = "Error, Approval more than 2.";
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
                                        //result.Status = SystemFunction.process_Failed;
                                        //result.Msg = "Error, somone not approve.";
                                        lstError.Add(new vAcKnowledge_lst
                                        {
                                            name_en = item.name_en,
                                            status = "Y",
                                            msg = "Error, somone not approve.",
                                        });
                                    }
                                    else
                                    {
                                        //result.Status = SystemFunction.process_Failed;
                                        //result.Msg = "Error, TIF Form not found.";
                                        lstError.Add(new vAcKnowledge_lst
                                        {
                                            name_en = item.name_en,
                                            status = "Y",
                                            msg = "Error, Error, TIF Form not found.",
                                        });
                                    }

                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, TIF Form not found.";
                                    lstError.Add(new vAcKnowledge_lst
                                    {
                                        name_en = item.name_en,
                                        status = "Y",
                                        msg = "Error, TIF Form not found.",
                                    });
                                }
                            }
                            else
                            {
                                var _getTIFForm = _TM_Candidate_MassTIFService.FindByMappingID(_GetMapping.Id);
                                if (_getTIFForm != null && _getTIFForm.TM_Candidate_MassTIF_Approv != null)
                                {
                                    var CheckCount = _getTIFForm.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                                    var CheckNotApprove = _getTIFForm.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                                    if (CheckCount >= 2 && CheckNotApprove <= 0)
                                    {
                                        var _getData = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                                        var _getLastStatus = _getData.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault();
                                        var _getStatus = _TM_Candidate_StatusService.Find(_getTIFForm.TM_MassTIF_Status.status_id.Value);
                                        if (_getTIFForm.TM_MassTIF_Status.status_id.HasValue && _getData != null && _GetMapping != null)
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
                                                TM_PR_Candidate_Mapping = _getTIFForm.TM_PR_Candidate_Mapping,
                                            };
                                            _getTIFForm.hr_acknowledge = "Y";
                                            _getTIFForm.acknowledge_date = dNow;
                                            _getTIFForm.acknowledge_user = CGlobal.UserInfo.EmployeeNo;
                                            _getTIFForm.update_user = CGlobal.UserInfo.UserId;
                                            _getTIFForm.update_date = dNow;
                                            _getTIFForm.TM_PR_Candidate_Mapping = _GetMapping;
                                            var sComplect = _TM_Candidate_MassTIFService.Update(_getTIFForm);
                                            if (sComplect > 0)
                                            {

                                                //sComplect = _TM_Candidate_Status_CycleService.CreateNew(objCycle);
                                                string sError = "";
                                                string mail_to_log = "";
                                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.HR_Ack);
                                                if (Mail1 != null)
                                                {
                                                    var bSuss = SendMassHRAck(_getTIFForm, Mail1, ref sError, ref mail_to_log);
                                                }
                                            }
                                            else
                                            {
                                                //result.Status = SystemFunction.process_Failed;
                                                //result.Msg = "Error, Approval more than 2.";
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
                                        //result.Status = SystemFunction.process_Failed;
                                        //result.Msg = "Error, somone not approve.";
                                        lstError.Add(new vAcKnowledge_lst
                                        {
                                            name_en = item.name_en,
                                            status = "Y",
                                            msg = "Error, somone not approve.",
                                        });
                                    }
                                    else
                                    {
                                        //result.Status = SystemFunction.process_Failed;
                                        //result.Msg = "Error, TIF Form not found.";
                                        lstError.Add(new vAcKnowledge_lst
                                        {
                                            name_en = item.name_en,
                                            status = "Y",
                                            msg = "Error, Error, TIF Form not found.",
                                        });
                                    }

                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, TIF Form not found.";
                                    lstError.Add(new vAcKnowledge_lst
                                    {
                                        name_en = item.name_en,
                                        status = "Y",
                                        msg = "Error, TIF Form not found.",
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

        //Load Dataforreport
        [HttpPost]
        public ActionResult LoadTIFFormReportList(CSearchTIFReport SearchItem)
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
            List<TM_Candidate_TIF> lstData = new List<TM_Candidate_TIF>();
            List<TM_Candidate_MassTIF> lstDataMass = new List<TM_Candidate_MassTIF>();
            List<TM_PR_Candidate_Mapping> lstAllData = new List<TM_PR_Candidate_Mapping>();
            if (SearchItem.tif_type + "" == "X" || SearchItem.tif_type + "" == "")
            {
                lstData = _TM_Candidate_TIFService.GetReportList(SearchItem.group_code, aDivisionPermission,
                                     "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin, isHRAdmin).ToList();
                lstDataMass = _TM_Candidate_MassTIFService.GetReportList(
                                      SearchItem.group_code, aDivisionPermission,
                                     "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin, isHRAdmin).ToList();
            }
            else if (SearchItem.tif_type + "" == "N")
            {
                lstData = _TM_Candidate_TIFService.GetReportList(SearchItem.group_code, aDivisionPermission,
                                     "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin, isHRAdmin).ToList();
            }
            else if (SearchItem.tif_type + "" == "M")
            {
                lstDataMass = _TM_Candidate_MassTIFService.GetReportList(
                                           SearchItem.group_code, aDivisionPermission,
                                          "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin, isHRAdmin).ToList();
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
            rpvTIFReport_Session objSession = new rpvTIFReport_Session();
            if (lstData.Any() || lstDataMass.Any())
            {
                //if (lstData.Any())
                //{
                //    lstAllData.AddRange(lstData.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                //}
                //if (lstDataMass.Any())
                //{
                //    lstAllData.AddRange(lstDataMass.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                //    objSession.lstData = lstDataMass.ToList();
                //}
                if (lstData.Any())
                {
                    lstAllData.AddRange(lstData.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                    objSession.lstData = lstData.ToList();
                }
                if (lstDataMass.Any())
                {
                    lstAllData.AddRange(lstDataMass.Select(s => s.TM_PR_Candidate_Mapping).ToList());
                    objSession.lstDataMass = lstDataMass.ToList();

                }

                string[] aRecruit = lstAllData.Select(s => s.TM_Recruitment_Team.user_no).ToArray();
                var _getEmp = dbHr.AllInfo_WS.Where(w => aRecruit.Contains(w.EmpNo)).ToList();

                try
                {
                    lstData_resutl = (from lstAD in lstAllData
                                      from lstTIF in lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_TIF())
                                      from lstMassTIF in lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_MassTIF())
                                      from Emp in _getEmp.Where(w => w.EmpNo == lstAD.TM_Recruitment_Team.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      select new vTIFReport_obj
                                      {
                                          Id = lstAD.Id,
                                          IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                          refno = lstAD.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                          rank = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.Recommended_Rank != null ? lstTIF.Recommended_Rank.Pool_rank_short_name_en : "") : (lstMassTIF.Recommended_Rank != null ? lstMassTIF.Recommended_Rank.Pool_rank_short_name_en : ""),
                                          name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                          //status = lstAD.submit_status != null ? (lstAD.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting",
                                          tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.TM_TIF_Status != null ? lstTIF.TM_TIF_Status.tif_short_name_en + "" : "") : (lstMassTIF.TM_MassTIF_Status != null ? lstMassTIF.TM_MassTIF_Status.masstif_short_name_en + "" : ""),
                                          pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : "Mass",
                                          pr_type_id = lstAD.PersonnelRequest.type_of_TIFForm + "",
                                          hr_owner = lstAD.TM_Recruitment_Team != null ? Emp.EmpFullName : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      }).ToList();

                }
                catch (Exception ex)
                {
                    var es = ex.Message;
                }
                lstData_resutl.ForEach(ed =>
            {
                if (ed.pr_type_id == "N")
                {
                    var _getTif = lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == ed.Id).FirstOrDefault();
                    if (_getTif != null)
                    {
                        if (_getTif.TM_Candidate_TIF_Approv != null && _getTif.TM_Candidate_TIF_Approv.Any(a => a.active_status == "Y"))
                        {
                            var _getFirst = _getTif.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                            var _getSecond = _getTif.TM_Candidate_TIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
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
                else
                {
                    var _getTif = lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == ed.Id).FirstOrDefault();
                    if (_getTif != null)
                    {
                        if (_getTif.TM_Candidate_MassTIF_Approv != null && _getTif.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y"))
                        {
                            var _getFirst = _getTif.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).FirstOrDefault();
                            var _getSecond = _getTif.TM_Candidate_MassTIF_Approv.Where(a => a.active_status == "Y").OrderBy(o => o.seq).Skip(1).FirstOrDefault();
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

        #region for Non-Mass TIF Form
        [HttpPost]
        public ActionResult AcKnowledgeTIF(vAcKnowledge_obj_save ItemData)
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
                    var _getTIFForm = _TM_Candidate_TIFService.FindByMappingID(nId);
                    if (_getTIFForm != null && _getTIFForm.TM_Candidate_TIF_Approv != null)
                    {
                        var _GetMap = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                        var CheckCount = _getTIFForm.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                        var CheckNotApprove = _getTIFForm.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                        if (CheckCount >= 2 && CheckNotApprove <= 0 && _GetMap != null)
                        {
                            _getTIFForm.hr_acknowledge = "Y";
                            _getTIFForm.acknowledge_date = dNow;
                            _getTIFForm.acknowledge_user = CGlobal.UserInfo.EmployeeNo;
                            _getTIFForm.update_user = CGlobal.UserInfo.UserId;
                            _getTIFForm.update_date = dNow;
                            _getTIFForm.TM_PR_Candidate_Mapping = _GetMap;
                            var sComplect = _TM_Candidate_TIFService.Update(_getTIFForm);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.HR_Ack);
                                if (Mail1 != null)
                                {
                                    var bSuss = SendHRAck(_getTIFForm, Mail1, ref sError, ref mail_to_log);
                                }
                                //var _getData = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                                //if (_getTIFForm.TM_TIF_Status.status_id.HasValue && _getData != null)
                                //{
                                //    var _getLastStatus = _getData.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault();
                                //    var _getStatus = _TM_Candidate_StatusService.Find(_getTIFForm.TM_TIF_Status.status_id.Value);
                                //    TM_Candidate_Status_Cycle objCycle = new TM_Candidate_Status_Cycle()
                                //    {
                                //        seq = _getLastStatus.seq + 1,
                                //        update_user = CGlobal.UserInfo.UserId,
                                //        update_date = dNow,
                                //        create_user = CGlobal.UserInfo.UserId,
                                //        create_date = dNow,
                                //        active_status = "Y",
                                //        TM_Candidate_Status = _getStatus,
                                //        action_date = dNow,
                                //        TM_PR_Candidate_Mapping = _getTIFForm.TM_PR_Candidate_Mapping,
                                //    };
                                //    sComplect = _TM_Candidate_Status_CycleService.CreateNew(objCycle);
                                //}
                                //else
                                //{
                                //    result.Status = SystemFunction.process_Failed;
                                //    result.Msg = "Error, Pleace try again.";
                                //}
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
                        result.Msg = "Error, TIF Form not found.";
                    }
                }
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
                    var _CheckUser = dbHr.Employee.Where(w => w.Employeeno == ItemData.user_no && (w.C_Status == "1" || w.C_Status == "3")).FirstOrDefault();
                    if (_CheckUser != null)
                    {
                        var _getTIFForm = _TM_Candidate_TIFService.FindByMappingID(nId);
                        if (_getTIFForm != null && _getTIFForm.TM_Candidate_TIF_Approv != null)
                        {
                            var CheckDup = _getTIFForm.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y" && w.Req_Approve_user == ItemData.user_no).FirstOrDefault();
                            if (CheckDup == null)
                            {
                                List<TM_Candidate_TIF_Approv> lstApprove = new List<TM_Candidate_TIF_Approv>();
                                lstApprove.Add(new TM_Candidate_TIF_Approv()
                                {

                                    Req_Approve_user = ItemData.user_no,
                                    active_status = "Y",
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    TM_Candidate_TIF = _getTIFForm,
                                });

                                var sComplectApprove = _TM_Candidate_TIF_ApprovService.CreateNewByList(lstApprove);
                                if (sComplectApprove > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    var _getEditList = _TM_Candidate_TIFService.Find(_getTIFForm.Id);
                                    if (_getEditList != null && _getEditList.TM_Candidate_TIF_Approv != null && _getEditList.TM_Candidate_TIF_Approv.Any(a => a.active_status == "Y"))
                                    {
                                        string[] aUser = _getEditList.TM_Candidate_TIF_Approv.Select(s => s.Req_Approve_user).ToArray();
                                        string[] aApproveUser = _getEditList.TM_Candidate_TIF_Approv.Select(s => s.Approve_user).ToArray();
                                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                        var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                        int nPriority = 1;
                                        result.lstData = (from Tif in _getEditList.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
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

            }

            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult DelTIFApprover(string ItemData)
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

                    var _getData = _TM_Candidate_TIF_ApprovService.Find(nId);
                    if (_getData != null)
                    {

                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        _getData.active_status = "N";
                        var sComplect = _TM_Candidate_TIF_ApprovService.Update(_getData);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            var _getEditList = _TM_Candidate_TIFService.Find(_getData.TM_Candidate_TIF.Id);
                            if (_getEditList != null && _getEditList.TM_Candidate_TIF_Approv != null && _getEditList.TM_Candidate_TIF_Approv.Any(a => a.active_status == "Y"))
                            {
                                string[] aUser = _getEditList.TM_Candidate_TIF_Approv.Select(s => s.Req_Approve_user).ToArray();
                                string[] aApproveUser = _getEditList.TM_Candidate_TIF_Approv.Select(s => s.Approve_user).ToArray();
                                var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                int nPriority = 1;
                                result.lstData = (from Tif in _getEditList.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
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
                        result.Msg = "Error, User Not Found.";
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
        public ActionResult HRRollBackTIFForm(vAcKnowledge_obj_save ItemData)
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
                    var _getData = _TM_Candidate_TIFService.FindByMappingID(nId);
                    if (_getData != null)
                    {
                        var ApproveComplect = _TM_Candidate_TIF_ApprovService.RollBackStatus(_getData.Id, CGlobal.UserInfo.UserId, dNow);
                        if (ApproveComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            string sError = "";
                            string mail_to_log = "";
                            var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.HR_Rollback);
                            var _getCheckTIF = _TM_Candidate_TIFService.Find(_getData.Id);
                            var _GetMap = _TM_PR_Candidate_MappingService.Find(_getCheckTIF.TM_PR_Candidate_Mapping.Id);
                            if (_getCheckTIF != null && _GetMap != null)
                            {
                                _getCheckTIF.TM_PR_Candidate_Mapping = _GetMap;
                                var bSuss = SendHRRollback(_getCheckTIF, Mail1, ItemData.rejeck, ref sError, ref mail_to_log);
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
        public ActionResult HRResetTIFForm(vAcKnowledge_obj_save ItemData)
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
                    var _getData = _TM_Candidate_TIFService.FindByMappingID(nId);
                    if (_getData != null)
                    {
                        var ApproveComplect = _TM_Candidate_TIF_ApprovService.RollBackStatus(_getData.Id, CGlobal.UserInfo.UserId, dNow);
                        if (ApproveComplect > 0)
                        {
                            _TM_Candidate_TIF_AnswerService.InactiveAnswer(_getData.Id, CGlobal.UserInfo.UserId, dNow);

                            result.Status = SystemFunction.process_Success;
                            string sError = "";
                            string mail_to_log = "";
                            var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.HR_Rollback);
                            var _getCheckTIF = _TM_Candidate_TIFService.Find(_getData.Id);
                            var _GetMap = _TM_PR_Candidate_MappingService.Find(_getCheckTIF.TM_PR_Candidate_Mapping.Id);
                            if (_getCheckTIF != null && _GetMap != null)
                            {
                                _getCheckTIF.TM_PR_Candidate_Mapping = _GetMap;
                                var bSuss = SendHRRollback(_getCheckTIF, Mail1, ItemData.rejeck, ref sError, ref mail_to_log);
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
        #region for Mass TIF Form
        [HttpPost]
        public ActionResult AcKnowledgeMassTIF(vAcKnowledge_obj_save ItemData)
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
                    var _getTIFForm = _TM_Candidate_MassTIFService.FindByMappingID(nId);

                    if (_getTIFForm != null && _getTIFForm.TM_Candidate_MassTIF_Approv != null)
                    {
                        var _GetMap = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                        var CheckCount = _getTIFForm.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                        var CheckNotApprove = _getTIFForm.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                        if (CheckCount >= 2 && CheckNotApprove <= 0)
                        {
                            _getTIFForm.hr_acknowledge = "Y";
                            _getTIFForm.acknowledge_date = dNow;
                            _getTIFForm.acknowledge_user = CGlobal.UserInfo.EmployeeNo;
                            _getTIFForm.update_user = CGlobal.UserInfo.UserId;
                            _getTIFForm.update_date = dNow;
                            _getTIFForm.TM_PR_Candidate_Mapping = _GetMap;
                            var sComplect = _TM_Candidate_MassTIFService.Update(_getTIFForm);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.HR_Ack);
                                if (Mail1 != null)
                                {
                                    var bSuss = SendMassHRAck(_getTIFForm, Mail1, ref sError, ref mail_to_log);
                                }
                                //var _getData = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                                //if (_getTIFForm.TM_MassTIF_Status.status_id.HasValue && _getData != null)
                                //{
                                //    var _getLastStatus = _getData.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault();
                                //    var _getStatus = _TM_Candidate_StatusService.Find(_getTIFForm.TM_MassTIF_Status.status_id.Value);
                                //    TM_Candidate_Status_Cycle objCycle = new TM_Candidate_Status_Cycle()
                                //    {
                                //        seq = _getLastStatus.seq + 1,
                                //        update_user = CGlobal.UserInfo.UserId,
                                //        update_date = dNow,
                                //        create_user = CGlobal.UserInfo.UserId,
                                //        create_date = dNow,
                                //        active_status = "Y",
                                //        TM_Candidate_Status = _getStatus,
                                //        action_date = dNow,
                                //        TM_PR_Candidate_Mapping = _getTIFForm.TM_PR_Candidate_Mapping,
                                //    };
                                //    sComplect = _TM_Candidate_Status_CycleService.CreateNew(objCycle);
                                //}
                                //else
                                //{
                                //    result.Status = SystemFunction.process_Failed;
                                //    result.Msg = "Error, Pleace try again.";
                                //}
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Approval more than 2.";
                            }
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
                        result.Msg = "Error, TIF Form not found.";
                    }
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult AddMassApprover(vAcKnowledge_obj_save ItemData)
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
                    var _CheckUser = dbHr.Employee.Where(w => w.Employeeno == ItemData.user_no && (w.C_Status == "1" || w.C_Status == "3")).FirstOrDefault();
                    if (_CheckUser != null)
                    {
                        var _getTIFForm = _TM_Candidate_MassTIFService.FindByMappingID(nId);
                        if (_getTIFForm != null && _getTIFForm.TM_Candidate_MassTIF_Approv != null)
                        {
                            var CheckDup = _getTIFForm.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y" && w.Req_Approve_user == ItemData.user_no).FirstOrDefault();
                            if (CheckDup == null)
                            {
                                List<TM_Candidate_MassTIF_Approv> lstApprove = new List<TM_Candidate_MassTIF_Approv>();
                                lstApprove.Add(new TM_Candidate_MassTIF_Approv()
                                {

                                    Req_Approve_user = ItemData.user_no,
                                    active_status = "Y",
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    TM_Candidate_MassTIF = _getTIFForm,
                                });

                                var sComplectApprove = _TM_Candidate_MassTIF_ApprovService.CreateNewByList(lstApprove);
                                if (sComplectApprove > 0)
                                {
                                    result.Status = SystemFunction.process_Success;
                                    var _getEditList = _TM_Candidate_MassTIFService.Find(_getTIFForm.Id);
                                    if (_getEditList != null && _getEditList.TM_Candidate_MassTIF_Approv != null && _getEditList.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y"))
                                    {
                                        string[] aUser = _getEditList.TM_Candidate_MassTIF_Approv.Select(s => s.Req_Approve_user).ToArray();
                                        string[] aApproveUser = _getEditList.TM_Candidate_MassTIF_Approv.Select(s => s.Approve_user).ToArray();
                                        var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                        var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                        int nPriority = 1;
                                        result.lstData = (from Tif in _getEditList.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
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

            }

            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult DelMassTIFApprover(string ItemData)
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

                    var _getData = _TM_Candidate_MassTIF_ApprovService.Find(nId);
                    if (_getData != null)
                    {

                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        _getData.active_status = "N";
                        var sComplect = _TM_Candidate_MassTIF_ApprovService.Update(_getData);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            var _getEditList = _TM_Candidate_MassTIFService.Find(_getData.TM_Candidate_MassTIF.Id);
                            if (_getEditList != null && _getEditList.TM_Candidate_MassTIF_Approv != null && _getEditList.TM_Candidate_MassTIF_Approv.Any(a => a.active_status == "Y"))
                            {
                                string[] aUser = _getEditList.TM_Candidate_MassTIF_Approv.Select(s => s.Req_Approve_user).ToArray();
                                string[] aApproveUser = _getEditList.TM_Candidate_MassTIF_Approv.Select(s => s.Approve_user).ToArray();
                                var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                                var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                                int nPriority = 1;
                                result.lstData = (from Tif in _getEditList.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y").OrderBy(o => o.seq)
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
                        result.Msg = "Error, User Not Found.";
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
        public ActionResult HRRollBackMassTIFForm(vAcKnowledge_obj_save ItemData)
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
                    var _getData = _TM_Candidate_MassTIFService.FindByMappingID(nId);
                    if (_getData != null)
                    {
                        var ApproveComplect = _TM_Candidate_MassTIF_ApprovService.RollBackStatus(_getData.Id, CGlobal.UserInfo.UserId, dNow);
                        if (ApproveComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            string sError = "";
                            string mail_to_log = "";
                            var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.HR_Rollback);
                            var _getCheckTIF = _TM_Candidate_MassTIFService.Find(_getData.Id);
                            var _GetMap = _TM_PR_Candidate_MappingService.Find(_getCheckTIF.TM_PR_Candidate_Mapping.Id);
                            if (_getCheckTIF != null && _GetMap != null)
                            {
                                _getCheckTIF.TM_PR_Candidate_Mapping = _GetMap;
                                var bSuss = SendMassHRRollback(_getCheckTIF, Mail1, ItemData.rejeck, ref sError, ref mail_to_log);
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
        #endregion

        #region for unacknowledge
        [HttpPost]
        public ActionResult unAcKnowledgeTIF(string qryStr)
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
                    var _getTIFForm = _TM_Candidate_TIFService.FindByMappingID(nId);

                    if (_getTIFForm != null && _getTIFForm.TM_Candidate_TIF_Approv != null)
                    {
                        var _GetMap = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                        var CheckCount = _getTIFForm.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                        var CheckNotApprove = _getTIFForm.TM_Candidate_TIF_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                        if (CheckCount >= 2 && CheckNotApprove <= 0 && _GetMap != null)
                        {
                            _getTIFForm.hr_acknowledge = null;
                            _getTIFForm.acknowledge_date = null;
                            _getTIFForm.acknowledge_user = null;
                            _getTIFForm.update_user = CGlobal.UserInfo.UserId;
                            _getTIFForm.update_date = dNow;
                            _getTIFForm.TM_PR_Candidate_Mapping = _GetMap;
                            var sComplect = _TM_Candidate_TIFService.Update(_getTIFForm);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.HR_Ack);
                                if (Mail1 != null)
                                {
                                    var bSuss = SendHRAck(_getTIFForm, Mail1, ref sError, ref mail_to_log);
                                }
                                //var _getData = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                                //if (_getTIFForm.TM_TIF_Status.status_id.HasValue && _getData != null)
                                //{
                                //    var _getLastStatus = _getData.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault();
                                //    var _getStatus = _TM_Candidate_StatusService.Find(_getTIFForm.TM_TIF_Status.status_id.Value);
                                //    TM_Candidate_Status_Cycle objCycle = new TM_Candidate_Status_Cycle()
                                //    {
                                //        seq = _getLastStatus.seq + 1,
                                //        update_user = CGlobal.UserInfo.UserId,
                                //        update_date = dNow,
                                //        create_user = CGlobal.UserInfo.UserId,
                                //        create_date = dNow,
                                //        active_status = "Y",
                                //        TM_Candidate_Status = _getStatus,
                                //        action_date = dNow,
                                //        TM_PR_Candidate_Mapping = _getTIFForm.TM_PR_Candidate_Mapping,
                                //    };
                                //    sComplect = _TM_Candidate_Status_CycleService.CreateNew(objCycle);
                                //}
                                //else
                                //{
                                //    result.Status = SystemFunction.process_Failed;
                                //    result.Msg = "Error, Pleace try again.";
                                //}
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
                        result.Msg = "Error, TIF Form not found.";
                    }
                }
            }
            return Json(new
            {
                result
            });
        }

        [HttpPost]
        public ActionResult unAcKnowledgeMassTIF(string qryStr)
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
                    var _getTIFForm = _TM_Candidate_MassTIFService.FindByMappingID(nId);

                    if (_getTIFForm != null && _getTIFForm.TM_Candidate_MassTIF_Approv != null)
                    {
                        var _GetMap = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                        var CheckCount = _getTIFForm.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y" && w.Approve_status == "Y").Count();
                        var CheckNotApprove = _getTIFForm.TM_Candidate_MassTIF_Approv.Where(w => w.active_status == "Y" && string.IsNullOrEmpty(w.Approve_status)).Count();
                        if (CheckCount >= 2 && CheckNotApprove <= 0)
                        {
                            _getTIFForm.hr_acknowledge = null;
                            _getTIFForm.acknowledge_date = null;
                            _getTIFForm.acknowledge_user = null;
                            _getTIFForm.update_user = CGlobal.UserInfo.UserId;
                            _getTIFForm.update_date = dNow;
                            _getTIFForm.TM_PR_Candidate_Mapping = _GetMap;
                            var sComplect = _TM_Candidate_MassTIFService.Update(_getTIFForm);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentTIFForm.HR_Ack);
                                if (Mail1 != null)
                                {
                                    var bSuss = SendMassHRAck(_getTIFForm, Mail1, ref sError, ref mail_to_log);
                                }
                                //var _getData = _TM_PR_Candidate_MappingService.Find(_getTIFForm.TM_PR_Candidate_Mapping.Id);
                                //if (_getTIFForm.TM_TIF_Status.status_id.HasValue && _getData != null)
                                //{
                                //    var _getLastStatus = _getData.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault();
                                //    var _getStatus = _TM_Candidate_StatusService.Find(_getTIFForm.TM_TIF_Status.status_id.Value);
                                //    TM_Candidate_Status_Cycle objCycle = new TM_Candidate_Status_Cycle()
                                //    {
                                //        seq = _getLastStatus.seq + 1,
                                //        update_user = CGlobal.UserInfo.UserId,
                                //        update_date = dNow,
                                //        create_user = CGlobal.UserInfo.UserId,
                                //        create_date = dNow,
                                //        active_status = "Y",
                                //        TM_Candidate_Status = _getStatus,
                                //        action_date = dNow,
                                //        TM_PR_Candidate_Mapping = _getTIFForm.TM_PR_Candidate_Mapping,
                                //    };
                                //    sComplect = _TM_Candidate_Status_CycleService.CreateNew(objCycle);
                                //}
                                //else
                                //{
                                //    result.Status = SystemFunction.process_Failed;
                                //    result.Msg = "Error, Pleace try again.";
                                //}
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
                        result.Msg = "Error, TIF Form not found.";
                    }
                }
            }
            return Json(new
            {
                result
            });
        }

        #endregion
    }
}