using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.PartnerEvaluation.NominationMain;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.PartnerEvaluation;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.NMNVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.NominationSystem
{
    public class NominationApproveController : BaseController
    {
        private PES_Nomination_YearService _PES_Nomination_YearService;
        private TM_PES_NMN_StatusService _TM_PES_NMN_StatusService;
        private TM_PES_NMN_SignatureStepService _TM_PES_NMN_SignatureStepService;
        private TM_KPIs_BaseService _TM_KPIs_BaseService;
        private PES_NominationService _PES_NominationService;
        private TM_PES_NMN_CompetenciesService _TM_PES_NMN_CompetenciesService;
        private PES_Nomination_CompetenciesService _PES_Nomination_CompetenciesService;
        private PES_Final_Rating_YearService _PES_Final_Rating_YearService;
        private PES_Nomination_KPIsService _PES_Nomination_KPIsService;
        private PES_Nomination_AnswerService _PES_Nomination_AnswerService;
        private PES_Final_RatingService _PES_Final_RatingService;
        private PES_Nomination_FilesService _PES_Nomination_FilesService;
        private PES_Nomination_SignaturesService _PES_Nomination_SignaturesService;
        private MailContentService _MailContentService;
        private TM_Annual_RatingService _TM_Annual_RatingService;
        New_HRISEntities dbHr = new New_HRISEntities();
        public NominationApproveController(PES_Nomination_YearService PES_Nomination_YearService
            , TM_PES_NMN_StatusService TM_PES_NMN_StatusService
            , TM_PES_NMN_SignatureStepService TM_PES_NMN_SignatureStepService
            , TM_KPIs_BaseService TM_KPIs_BaseService
            , PES_NominationService PES_NominationService, TM_PES_NMN_CompetenciesService TM_PES_NMN_CompetenciesService
            , PES_Nomination_CompetenciesService PES_Nomination_CompetenciesService
            , PES_Final_Rating_YearService PES_Final_Rating_YearService
            , PES_Nomination_KPIsService PES_Nomination_KPIsService
            , PES_Nomination_AnswerService PES_Nomination_AnswerService
             , PES_Final_RatingService PES_Final_RatingService
            , PES_Nomination_FilesService PES_Nomination_FilesService
            , PES_Nomination_SignaturesService PES_Nomination_SignaturesService
            , MailContentService MailContentService
            , TM_Annual_RatingService TM_Annual_RatingService
            )
        {
            _PES_Nomination_YearService = PES_Nomination_YearService;
            _TM_PES_NMN_StatusService = TM_PES_NMN_StatusService;
            _TM_PES_NMN_SignatureStepService = TM_PES_NMN_SignatureStepService;
            _TM_KPIs_BaseService = TM_KPIs_BaseService;
            _PES_NominationService = PES_NominationService;
            _TM_PES_NMN_CompetenciesService = TM_PES_NMN_CompetenciesService;
            _PES_Nomination_CompetenciesService = PES_Nomination_CompetenciesService;
            _PES_Final_Rating_YearService = PES_Final_Rating_YearService;
            _PES_Nomination_KPIsService = PES_Nomination_KPIsService;
            _PES_Nomination_AnswerService = PES_Nomination_AnswerService;
            _PES_Final_RatingService = PES_Final_RatingService;
            _PES_Nomination_FilesService = PES_Nomination_FilesService;
            _PES_Nomination_SignaturesService = PES_Nomination_SignaturesService;
            _MailContentService = MailContentService;
            _TM_Annual_RatingService = TM_Annual_RatingService;
        }
        // GET: NominationApprove
        public ActionResult NominationApproveList(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vNominationForm result = new vNominationForm();
            #region main code
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchNMNForm SearchItem = (CSearchNMNForm)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchNMNForm)));
                int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
                int nStep = SystemFunction.GetIntNullToZero(SearchItem.signature_id);
                List<int> nStatus = new List<int>();
                nStatus.Add((int)PESClass.Nomination_Status.Waiting_Approval);
                //nStatus.Add((int)PESClass.Nomination_Status.Form_Completed);
                List<int> nSelf = new List<int>();
                nSelf.Add((int)PESClass.SignaturesStep.Self);
                List<int> nHead = new List<int>();
                nHead.Add((int)PESClass.SignaturesStep.Ceo);
                nHead.Add((int)PESClass.SignaturesStep.Deputy_Ceo);
                nHead.Add((int)PESClass.SignaturesStep.COO);
                //nHead.Add((int)PESClass.SignaturesStep.Risk_Management);

                List<int> nCommittee = new List<int>();
                nCommittee.Add((int)PESClass.SignaturesStep.Nominating);
                var lstData = _PES_Nomination_SignaturesService.GetEvaForApprove(CGlobal.UserInfo.EmployeeNo, nYear, nStep, nStatus, nSelf, nHead, nCommittee, CGlobal.UserIsAdminPES());

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.fy_year = SearchItem.fy_year;

                    string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.PES_Nomination.user_no).ToArray();
                    string[] aApproveUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.PES_Nomination.update_user).ToArray();

                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                    var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                    result.lstData = (from lstAD in lstData
                                      from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.PES_Nomination.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == lstAD.PES_Nomination.update_user).DefaultIfEmpty(new AllInfo_WS())
                                      select new vNominationForm_obj
                                      {
                                          srank = lstEmpReq.RankCode,
                                          sgroup = lstEmpReq.UnitGroupName,
                                          name_en = lstAD.PES_Nomination.user_no + " : " + lstEmpReq.EmpFullName,
                                          fy_year = lstAD.PES_Nomination.PES_Nomination_Year.evaluation_year.HasValue ? lstAD.PES_Nomination.PES_Nomination_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                          active_status = lstAD.PES_Nomination.TM_PES_NMN_Status != null ? lstAD.PES_Nomination.TM_PES_NMN_Status.status_name_en : "", //lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.PES_Nomination.update_date.HasValue ? lstAD.PES_Nomination.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          approval = AEmp.EmpFullName + (lstAD.TM_PES_NMN_SignatureStep != null ? "(" + lstAD.TM_PES_NMN_SignatureStep.Step_name_en + ")" : ""),
                                          name = lstEmpReq.EmpFullName,
                                          approval_step = lstAD.TM_PES_NMN_SignatureStep != null ? lstAD.TM_PES_NMN_SignatureStep.Step_short_name_en + "" : ""
                                      }).ToList();

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
        public ActionResult NominationApproveEdit(string qryStr)
        {
            var sCheck = pesCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            #region main code

            vNominationForm_obj_save result = new vNominationForm_obj_save();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(qryStr + ""));
                if (nId != 0)
                {

                    List<int> canEdit = new List<int>();
                    canEdit.Add((int)PESClass.Nomination_Status.Draft_Form);
                    canEdit.Add((int)PESClass.Nomination_Status.Revised_Form);
                    var _GetApprove = _PES_Nomination_SignaturesService.Find(nId);
                    if (_GetApprove != null && _GetApprove.PES_Nomination != null)
                    {
                        var _getData = _GetApprove.PES_Nomination;
                        ViewBag.formyear = _getData.PES_Nomination_Year.evaluation_year.Value.Year;
                        if ((_GetApprove.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())/* && _GetApprove.Approve_status != "Y"*/)
                        {
                            var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                            if (_checkActiv != null)
                            {

                                if (_GetApprove.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Sponsoring_Partner)
                                {
                                    return RedirectToAction("NominationFormEdit", "NominationForm", new { qryStr = HCMFunc.EncryptPES(_getData.Id + "") });
                                }

                                if (_GetApprove.Approve_status == "Y")
                                {
                                    result.is_update_by_approval = "Y";

                                }

                                result.step_id = _GetApprove.TM_PES_NMN_SignatureStep_Id + "";
                                result.approve_rating_id = _GetApprove.TM_Annual_Rating_Id + "";
                                result.IdEncrypt = qryStr;
                                result.code = _getData.user_no;
                                result.sname = _checkActiv.EmpFullName;
                                result.sgroup = _checkActiv.UnitGroupName;
                                result.srank = _checkActiv.Rank;
                                result.status_name = _getData.TM_PES_NMN_Status != null ? _getData.TM_PES_NMN_Status.status_name_en + "" : "";
                                result.status_id = _getData.TM_PES_NMN_Status_Id + "";
                                result.yearcurrent = _getData.PES_Nomination_Year.evaluation_year.Value.DateTimebyCulture("yyyy");
                                result.other_role = _getData.other_roles;
                                result.type_id = _getData.TM_PES_NMN_Type_Id + "";
                                result.type_name = _getData.TM_PES_NMN_Type != null ? _getData.TM_PES_NMN_Type.type_name_en : "";
                                result.Year_joined_KPMG = _getData.Year_joined_KPMG.HasValue ? _getData.Year_joined_KPMG.Value.DateTimebyCulture() : "";
                                result.working_with_KPMG = _getData.working_with_KPMG + "";
                                result.working_outside_KPMG = _getData.working_outside_KPMG + "";
                                result.being_ADDirector = _getData.being_ADDirector + "";
                                result.being_AD_Director = _getData.being_AD_Director.HasValue ? _getData.being_AD_Director.Value.DateTimebyCulture() : "";
                                result.Professional_Qualifications = _getData.Professional_Qualifications + "";
                                result.DEVELOPMENT_AREAS = HCMFunc.DataDecryptPES(_getData.DEVELOPMENT_AREAS);
                                result.approve_remark = HCMFunc.DataDecryptPES(_GetApprove.responses + "");
                                if (_getData.PES_Nomination_Files != null && _getData.PES_Nomination_Files.Any(a => a.active_status == "Y"))
                                {
                                    int nFile = 1;
                                    result.lstFile = _getData.PES_Nomination_Files.Where(a => a.active_status == "Y").Select(s => new vNominationForm_lst_File
                                    {
                                        file_name = s.sfile_oldname,
                                        description = s.description,
                                        Edit = (_getData.TM_PES_NMN_Status.Id != (int)PESClass.Nomination_Status.Draft_Form && _getData.TM_PES_NMN_Status.Id != (int)PESClass.Nomination_Status.Revised_Form) ? "" + nFile++ : @"<button id=""btnEdit""  type=""button"" onclick=""DeleteFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
                                        View = @"<button id=""btnEdit""  type=""button"" onclick=""LoadFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-success""><i class=""glyphicon glyphicon-download-alt""></i></button>",
                                    }).ToList();
                                }
                                #region if else Data

                                #region Eva Data 
                                result.eva_mode = "Eva";


                                if (_getData.PES_Nomination_Year.evaluation_year.HasValue)
                                {

                                    int nCurrent = _getData.PES_Nomination_Year.evaluation_year.Value.Year;
                                    result.yearone = (_getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-1)).DateTimebyCulture("yyyy");
                                    result.yeartwo = (_getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-2)).DateTimebyCulture("yyyy");
                                    result.yearthree = (_getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-3)).DateTimebyCulture("yyyy");
                                }
                                if (_getData.PES_Nomination_Signatures != null && _getData.PES_Nomination_Signatures.Any(a => a.active_status == "Y"))
                                {

                                    result.lstApprove = PESFunc.GetNominationApproveList(_getData.PES_Nomination_Signatures.Where(w => w.active_status == "Y").ToList(), _getData);
                                }


                                var _getCom = _TM_PES_NMN_CompetenciesService.GetDataForSelect();
                                if (_getCom.Any())
                                {
                                    List<vNominationForm_RATING_SUMMARY> lstRating_Summary = new List<vNominationForm_RATING_SUMMARY>();
                                    var GetRatescore = _PES_Nomination_CompetenciesService.GetScored(_getData.Id, true).ToList();

                                    lstRating_Summary.AddRange(
                                                   (from lstQ in _getCom
                                                    select new vNominationForm_RATING_SUMMARY
                                                    {

                                                        nstep = lstQ.seq,
                                                        id = lstQ.Id + "",
                                                        question = lstQ.question,
                                                        header = lstQ.header,
                                                        nSeq = lstQ.seq,
                                                        sgroup = lstQ.header,
                                                        sExcellent = "N",
                                                        sHigh = "N",
                                                        sNI = "N",
                                                        sLow = "N",
                                                        nscore = lstQ.nscore + "",
                                                        isCurrent = canEdit.Contains(_getData.TM_PES_NMN_Status.Id) ? "Y" : "N",
                                                        nrate = GetRatescore.Where(w => w.active_status == "Y" && w.TM_PES_NMN_Competencies_Id == lstQ.Id).Select(s => s.TM_PES_NMN_Competencies_Rating_Id).FirstOrDefault(),
                                                    }).ToList()
                                                   );

                                    lstRating_Summary.ForEach(ed =>
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
                                    });

                                    result.lstRating_Summary = lstRating_Summary.ToList();
                                }

                                var _getKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                                if (_getKPIs.Any())
                                {
                                    var _getDataYCur = _PES_Nomination_KPIsService.FindFor3YearKPI(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.Year);
                                    var _getDataYOne = _PES_Nomination_KPIsService.FindFor3YearKPI(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-1).Year);
                                    var _getDataYTwo = _PES_Nomination_KPIsService.FindFor3YearKPI(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-2).Year);
                                    var _getDataYThree = _PES_Nomination_KPIsService.FindFor3YearKPI(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-3).Year);

                                    //canEdit.Add((int)PESClass.Nomination_Status.Draft_Form);
                                    //canEdit.Add((int)PESClass.Nomination_Status.Revised_Form);
                                    result.lstKPIs = (from lst in _getKPIs
                                                      from lstKPICur in _getDataYCur.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                      from lstKPIOne in _getDataYOne.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                      from lstKPITwo in _getDataYTwo.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                      from lstKPIThree in _getDataYThree.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                      select new vNominationForm_KPIs
                                                      {
                                                          sname = lst.kpi_name_en,
                                                          target_data = HCMFunc.DataDecryptPES(lstKPICur.target),// lst.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lstKPICur.target) + "-" + HCMFunc.DataDecryptPES(lstKPICur.target_max) + "%" : (lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPICur.target_max))) : HCMFunc.DataDecryptPES(lstKPICur.target_max)),
                                                          yearoneTar = HCMFunc.DataDecryptPES(lstKPIOne.target), //lst.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lstKPIOne.target)+ "-" + HCMFunc.DataDecryptPES(lstKPIOne.target_max) + "%" : (lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIOne.target_max))) : HCMFunc.DataDecryptPES(lstKPIOne.target_max)),
                                                          yeartwoTar = HCMFunc.DataDecryptPES(lstKPITwo.target), // lst.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lstKPITwo.target)+ "-" + HCMFunc.DataDecryptPES(lstKPITwo.target_max) + "%" : (lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPITwo.target_max))) : HCMFunc.DataDecryptPES(lstKPITwo.target_max)),
                                                          yearthreeTar = HCMFunc.DataDecryptPES(lstKPIThree.target), // lst.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lstKPIThree.target)+ "-" + HCMFunc.DataDecryptPES(lstKPIThree.target_max) + "%" : (lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lstKPIThree.target_max))) : HCMFunc.DataDecryptPES(lstKPIThree.target_max)),

                                                          //remark = HCMFunc.DataDecryptPES(lst.final_remark + ""),
                                                          actual = HCMFunc.DataDecryptPES(lstKPICur.actual),
                                                          yearone = HCMFunc.DataDecryptPES(lstKPIOne.actual),//HCMFunc.DataDecryptPES(lstKPIOne.actual),
                                                          yeartwo = HCMFunc.DataDecryptPES(lstKPITwo.actual),//HCMFunc.DataDecryptPES(lstKPITwo.actual),
                                                          yearthree = HCMFunc.DataDecryptPES(lstKPIThree.actual),//HCMFunc.DataDecryptPES(lstKPIThree.actual),
                                                          sdisable = (_getData.TM_PES_NMN_Status.Id == (int)PESClass.Nomination_Status.Draft_Form || _getData.TM_PES_NMN_Status.Id == (int)PESClass.Nomination_Status.Revised_Form) ? "N" : "Y",
                                                          IdEncrypt = lst.Id + "",
                                                          stype = lst.type_of_kpi,
                                                          //target_group = lst.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lst.group_target) + "-" + HCMFunc.DataDecryptPES(lst.group_target_max) + "%" : (lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max))) : HCMFunc.DataDecryptPES(lst.group_target_max)),
                                                          //group_actual = lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_actual))) : HCMFunc.DataDecryptPES(lst.group_actual),


                                                      }).ToList();
                                }
                                var _getRateYCur = _PES_Final_RatingService.FindForYearRate(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.Year);
                                var _getRateYOne = _PES_Final_RatingService.FindForYearRate(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-1).Year);
                                var _getRateYTwo = _PES_Final_RatingService.FindForYearRate(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-2).Year);
                                var _getRateYThree = _PES_Final_RatingService.FindForYearRate(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.AddYears(-3).Year);
                                result.yearcurrent_rate = "";
                                result.yearcurrent_selfrate = "";
                                result.yearone_rate = "";
                                result.yeartwo_rate = "";
                                result.yearthree_rate = "";

                                if (_getRateYCur != null)
                                {
                                    result.yearcurrent_rate = _getRateYCur.Final_TM_Annual_Rating_Id + "";
                                    result.yearcurrent_selfrate = _getRateYCur.Self_TM_Annual_Rating_Id + "";
                                }
                                if (_getRateYOne != null)
                                {
                                    result.yearone_rate = _getRateYOne.Final_TM_Annual_Rating_Id + "";
                                }
                                if (_getRateYTwo != null)
                                {
                                    result.yeartwo_rate = _getRateYTwo.Final_TM_Annual_Rating_Id + "";
                                }
                                if (_getRateYThree != null)
                                {
                                    result.yearthree_rate = _getRateYThree.Final_TM_Annual_Rating_Id + "";
                                }
                                #region answer

                                if (_getData.PES_Nomination_Answer != null && _getData.PES_Nomination_Answer.Any(a => a.active_status == "Y"))
                                {
                                    var _getAnswer = _getData.PES_Nomination_Answer.Where(a => a.active_status == "Y").ToList();
                                    result.Client_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Client_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Client_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Client_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Client_Q3 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Client_Q3).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Client_Q4 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Client_Q4).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Business_Skills_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Business_Skills_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Business_Skills_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Business_Skills_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Management_Leadership_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Management_Leadership_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Management_Leadership_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Management_Leadership_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Management_Leadership_Q3 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Management_Leadership_Q3).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Social_Skills_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Social_Skills_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Social_Skills_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Social_Skills_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Thinking_Skills_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Thinking_Skills_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Thinking_Skills_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Thinking_Skills_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Thinking_Skills_Q3 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Thinking_Skills_Q3).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Technical_Competence_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Technical_Competence_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.RISK_MANAGEMENT_Q1 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.RISK_MANAGEMENT_Q1).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.RISK_MANAGEMENT_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.RISK_MANAGEMENT_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Business_Skills_Q3 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Business_Skills_Q3).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Technical_Competence_Q2 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Technical_Competence_Q2).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                    result.Technical_Competence_Q3 = _getAnswer.Where(a => a.TM_PES_NMN_Questions_Id == (int)PESClass.Nomination_Questions.Technical_Competence_Q3).Select(s => HCMFunc.DataDecryptPES(s.answer + "")).FirstOrDefault() + "";
                                }
                                #endregion

                                #endregion
                                #endregion

                                if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2018)
                                {
                                    return View(result);
                                }
                                else if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2019)
                                {
                                    return View(result);
                                }
                                else if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2020)
                                {
                                    return View("NominationApproveEdit2020", result);
                                }
                                else if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2021)
                                {
                                    return View("NominationApproveEdit2021", result);
                                }
                                else if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == DateTime.Now.Year)
                                {
                                    return View("NominationApproveEdit"+DateTime.Now.ToString("yy"), result);
                                }
                                else
                                {
                                    return View(result);
                                }
                               
                            }
                            else
                            {
                                return RedirectToAction("NominationApproveList", "NominationApprove");
                            }
                        }
                        else
                        {
                            return RedirectToAction("ErrorNopermission", "MasterPage");
                        }

                    }
                    else
                    {
                        return RedirectToAction("NominationApproveList", "NominationApprove");
                    }
                }
                else
                {
                    return RedirectToAction("NominationApproveList", "NominationApprove");
                }
            }
            else
            {
                return RedirectToAction("NominationApproveList", "NominationApprove");
            }
            #endregion
        }

        #region Ajax Function
        [HttpPost]
        public ActionResult LoadNominationApproveList(CSearchNMNForm SearchItem)
        {
            vNominationForm_Return result = new vNominationForm_Return();
            List<vNominationForm_obj> lstData_resutl = new List<vNominationForm_obj>();
            int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
            int nStep = SystemFunction.GetIntNullToZero(SearchItem.signature_id);
            List<int> nStatus = new List<int>();
            nStatus.Add((int)PESClass.Nomination_Status.Waiting_Approval);
            //nStatus.Add((int)PESClass.Nomination_Status.Form_Completed);
            List<int> nSelf = new List<int>();
            nSelf.Add((int)PESClass.SignaturesStep.Self);
            List<int> nHead = new List<int>();
            nHead.Add((int)PESClass.SignaturesStep.Ceo);
            nHead.Add((int)PESClass.SignaturesStep.Deputy_Ceo);
            nHead.Add((int)PESClass.SignaturesStep.COO);
            //nHead.Add((int)PESClass.SignaturesStep.Risk_Management);
            List<int> nCommittee = new List<int>();
            nCommittee.Add((int)PESClass.SignaturesStep.Nominating);
            var lstData = _PES_Nomination_SignaturesService.GetEvaForApprove(CGlobal.UserInfo.EmployeeNo, nYear, nStep, nStatus, nSelf, nHead, nCommittee, CGlobal.UserIsAdminPES());
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
                string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.PES_Nomination.user_no).ToArray();
                string[] aApproveUser = lstData.Select(s => s.Req_Approve_user).ToArray();
                string[] aUpdateUser = lstData.Select(s => s.PES_Nomination.update_user).ToArray();

                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                var _getApproveEmp = dbHr.AllInfo_WS.Where(w => aApproveUser.Contains(w.EmpNo)).ToList();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;

                lstData_resutl = (from lstAD in lstData
                                  from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.PES_Nomination.user_no).DefaultIfEmpty(new AllInfo_WS())
                                  from AEmp in _getApproveEmp.Where(w => w.EmpNo == lstAD.Req_Approve_user).DefaultIfEmpty(new AllInfo_WS())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == lstAD.PES_Nomination.update_user).DefaultIfEmpty(new AllInfo_WS())
                                  select new vNominationForm_obj
                                  {
                                      srank = lstEmpReq.RankCode,
                                      sgroup = lstEmpReq.UnitGroupName,
                                      name_en = lstAD.PES_Nomination.user_no + " : " + lstEmpReq.EmpFullName,
                                      fy_year = lstAD.PES_Nomination.PES_Nomination_Year.evaluation_year.HasValue ? lstAD.PES_Nomination.PES_Nomination_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                      active_status = lstAD.PES_Nomination.TM_PES_NMN_Status != null ? lstAD.PES_Nomination.TM_PES_NMN_Status.status_name_en : "", //lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.PES_Nomination.update_date.HasValue ? lstAD.PES_Nomination.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      approval = AEmp.EmpFullName + (lstAD.TM_PES_NMN_SignatureStep != null ? "(" + lstAD.TM_PES_NMN_SignatureStep.Step_name_en + ")" : ""),
                                      name = lstEmpReq.EmpFullName,
                                      approval_step = lstAD.TM_PES_NMN_SignatureStep != null ? lstAD.TM_PES_NMN_SignatureStep.Step_short_name_en + "" : ""
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult ApproveNomination(vNominationForm_obj_save ItemData, string sMode, string sAgree)
        {
            vNominationForm_Return result = new vNominationForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt) && !string.IsNullOrEmpty(sMode) || sMode == "N")
                {
                    if (sMode == "N" && (ItemData.revise_remark + "").Trim() == "")
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Please provide the remark why you are revise this Nomination Form.";
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
                    // var _GetStatusRevis = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Revised_Evaluate);
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                    var _getDataApprove = _PES_Nomination_SignaturesService.Find(nId);
                    if (_getDataApprove != null)
                    {

                        int nRating = SystemFunction.GetIntNullToZero(ItemData.approve_rating_id);
                        var _getEvaRating = _TM_Annual_RatingService.Find(nRating);

                        List<int> lstSignatureNeedRating = new List<int>();
                        //lstPlan.Add((int)StepApprovePlan.Self);
                        //lstSignature.Add((int)PESClass.SignaturesStep.Sponsoring_Partner);

                        lstSignatureNeedRating.Add((int)PESClass.SignaturesStep.Group_Head);
                        lstSignatureNeedRating.Add((int)PESClass.SignaturesStep.HOP);
                        lstSignatureNeedRating.Add((int)PESClass.SignaturesStep.Deputy_Ceo);
                        lstSignatureNeedRating.Add((int)PESClass.SignaturesStep.Ceo);

                        if (lstSignatureNeedRating.Contains(_getDataApprove.TM_PES_NMN_SignatureStep.Id) && _getEvaRating == null && sMode == "Y")
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error,Please select Rating.";
                            return Json(new
                            {
                                result
                            });
                        }

                        var _getData = _getDataApprove.PES_Nomination;
                        if (_getData != null)
                        {
                            _getData.update_user = CGlobal.UserInfo.UserId;
                            if (_getDataApprove.Approve_status == "Y")
                            {
                                dNow = _getData.update_date.Value;
                            }


                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            //var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && w.active_status == "Y" && w.Approve_status != "Y").FirstOrDefault();
                            if (sMode == "Y")
                            {
                                if ((_getDataApprove.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES()) && _getDataApprove.active_status == "Y" /*&& _getDataApprove.Approve_status != "Y"*/)
                                {
                                    if (_PES_Nomination_SignaturesService.CanCompleted(_getData.Id, nId))
                                    {
                                        _getData.TM_PES_NMN_Status_Id = (int)PESClass.Nomination_Status.Form_Completed;
                                    }
                                    if (_getData != null)
                                    {
                                        var getyear = _getData.PES_Nomination_Year;
                                        var sComplect = _PES_NominationService.Complect(_getData);
                                        if (sComplect > 0 || 1 == 1)
                                        {
                                            if (_getDataApprove.TM_PES_NMN_SignatureStep.Id == (int)PESClass.SignaturesStep.Ceo)
                                            {
                                                var _getRateYCur = _PES_Final_RatingService.FindForYearRate(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.Year);
                                                int nSelfRate = SystemFunction.GetIntNullToZero(ItemData.yearcurrent_selfrate + "");
                                                if (_getRateYCur != null && nSelfRate > 0)
                                                {
                                                    _getRateYCur.Final_TM_Annual_Rating_Id = _getEvaRating != null ? _getEvaRating.Id : (int?)null;
                                                    _getRateYCur.update_user = CGlobal.UserInfo.UserId;
                                                    _getRateYCur.update_date = dNow;
                                                    _PES_Final_RatingService.Update(_getRateYCur);
                                                }
                                            }

                                            _getDataApprove.PES_Nomination = _getData;
                                            _getDataApprove.update_user = CGlobal.UserInfo.UserId;
                                            _getDataApprove.update_date = dNow;

                                            if (_getDataApprove.Approve_status != "Y")
                                            {
                                                _getDataApprove.Approve_date = dNow;
                                                _getDataApprove.Agree_Status = sAgree;
                                                _getDataApprove.Approve_status = "Y";
                                                _getDataApprove.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                            }



                                            _getDataApprove.responses = HCMFunc.DataEncryptPES((ItemData.approve_remark + "").Trim());
                                            _getDataApprove.TM_Annual_Rating_Id = _getEvaRating != null ? _getEvaRating.Id : (int?)null;
                                            sComplect = _PES_Nomination_SignaturesService.Update(_getDataApprove);
                                            if (sComplect > 0 || 1 == 1)
                                            {
                                                result.Status = SystemFunction.process_Success;
                                                result.Msg = "Approval Completed";
                                                if (_getData.TM_PES_NMN_Status_Id == (int)PESClass.Nomination_Status.Form_Completed)
                                                {
                                                    string sError = "";
                                                    string mail_to_log = "";
                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPTR.Nomination_Suscess);
                                                    if (_getData != null)
                                                    {
                                                        //  var bSuss = SendNominationSuccessfully(_getData, Mail1, ref sError, ref mail_to_log);
                                                    }
                                                }
                                                else
                                                {
                                                    List<int> nStatus = new List<int>();
                                                    nStatus.Add((int)PESClass.Nomination_Status.Waiting_Approval);
                                                    List<int> nSelf = new List<int>();
                                                    nSelf.Add((int)PESClass.SignaturesStep.Self);
                                                    string sError = "";
                                                    string mail_to_log = "";
                                                    var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPTR.Nomination_Submit);
                                                    var GetMail = _PES_Nomination_SignaturesService.GetEvaForMail(_getData.Id, nStatus, nSelf).FirstOrDefault();
                                                    if (GetMail != null)
                                                    {
                                                        if (GetMail.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Nominating)
                                                        {
                                                            if (_getDataApprove.TM_PES_NMN_SignatureStep_Id != (int)PESClass.SignaturesStep.Nominating)
                                                            {
                                                                List<int> nMail = new List<int>();
                                                                nMail.Add((int)PESClass.SignaturesStep.Nominating);
                                                                var GetApproveMail = _PES_Nomination_SignaturesService.GetApproveByEvaAndStep(_getData.Id, nMail);
                                                                if (GetApproveMail.Any())
                                                                {
                                                                    foreach (var item in GetApproveMail)
                                                                    {
                                                                        //var bSuss = SendNominationSubmit(item, Mail1, ref sError, ref mail_to_log);
                                                                    }
                                                                }
                                                            }


                                                        }
                                                        else if ((
                                                          GetMail.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.HOP
                                                            || GetMail.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Risk_Management
                                                            //|| GetMail.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.COO
                                                            ))
                                                        {

                                                            List<int> nMail = new List<int>();
                                                            nMail.Add((int)GetMail.TM_PES_NMN_SignatureStep_Id);

                                                            var GetApproveMail = _PES_Nomination_SignaturesService.GetApproveByEvaAndStep(_getData.Id, nMail);
                                                            if (GetApproveMail.Any())
                                                            {
                                                                foreach (var item in GetApproveMail)
                                                                {
                                                                    var bSuss = SendNominationSubmit(item, Mail1, ref sError, ref mail_to_log);
                                                                }
                                                            }


                                                        }
                                                        //else if ((GetMail.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Ceo
                                                        //    //|| GetMail.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Deputy_Ceo
                                                        //    || GetMail.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Risk_Management
                                                        //    || GetMail.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.COO
                                                        //    ))
                                                        //{
                                                        //    if ((_getDataApprove.TM_PES_NMN_SignatureStep_Id != (int)PESClass.SignaturesStep.Ceo
                                                        //    //&& _getDataApprove.TM_PES_NMN_SignatureStep_Id != (int)PESClass.SignaturesStep.Deputy_Ceo
                                                        //    && _getDataApprove.TM_PES_NMN_SignatureStep_Id != (int)PESClass.SignaturesStep.Risk_Management
                                                        //    && _getDataApprove.TM_PES_NMN_SignatureStep_Id != (int)PESClass.SignaturesStep.COO
                                                        //    ))
                                                        //    {
                                                        //        List<int> nMail = new List<int>();
                                                        //        nMail.Add((int)PESClass.SignaturesStep.Ceo);
                                                        //        nMail.Add((int)PESClass.SignaturesStep.Deputy_Ceo);
                                                        //        //nMail.Add((int)PESClass.SignaturesStep.Risk_Management);
                                                        //        nMail.Add((int)PESClass.SignaturesStep.COO);

                                                        //        var GetApproveMail = _PES_Nomination_SignaturesService.GetApproveByEvaAndStep(_getData.Id, nMail);
                                                        //        if (GetApproveMail.Any())
                                                        //        {
                                                        //            foreach (var item in GetApproveMail)
                                                        //            {
                                                        //                var bSuss = SendNominationSubmit(item, Mail1, ref sError, ref mail_to_log);
                                                        //            }
                                                        //        }
                                                        //    }

                                                        //}
                                                        else
                                                        {
                                                            //var bSuss = SendNominationSubmit(GetMail, Mail1, ref sError, ref mail_to_log);
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
                                            result.Msg = "Error, please try again.";
                                        }
                                    }
                                    else 
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, Can't save.";
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
                                List<int> nStatus = new List<int>();
                                nStatus.Add((int)PESClass.Nomination_Status.Waiting_Approval);
                                List<int> nSelf = new List<int>();
                                nSelf.Add((int)PESClass.SignaturesStep.Nominating);
                                var _getApprove = _PES_Nomination_SignaturesService.GetApproveByEva(_getData.Id, nStatus, nSelf);
                                if (_getApprove != null && _getApprove.Any())
                                {
                                    List<string> MailCC = new List<string>();
                                    _getData.TM_PES_NMN_Status_Id = (int)PESClass.Nomination_Status.Revised_Form;
                                    _getData.comments = ItemData.revise_remark;
                                    var sComplect = _PES_NominationService.Complect(_getData);
                                    if (sComplect > 0)
                                    {
                                        foreach (var item in _getApprove)
                                        {
                                            item.PES_Nomination = _getData;
                                            item.update_user = CGlobal.UserInfo.UserId;
                                            item.update_date = dNow;
                                            item.Approve_status = "";
                                            item.Approve_date = null;
                                            item.Approve_user = "";
                                            item.Agree_Status = "";
                                            //item.responses = "";
                                            sComplect = _PES_Nomination_SignaturesService.Update(item);
                                            if (sComplect > 0)
                                            {

                                                MailCC.Add(item.Req_Approve_user);
                                            }
                                        }
                                        string sError = "";
                                        string mail_to_log = "";
                                        var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPTR.Nomination_Revise);
                                        var GetMail = _PES_NominationService.Find(_getData.Id);
                                        if (GetMail != null)
                                        {
                                            var bSuss = SendNominationReviseNewVersion(GetMail, MailCC, Mail1, ref sError, ref mail_to_log);
                                            if (bSuss)
                                            {
                                                result.Status = SystemFunction.process_Success;
                                                result.Msg = "Sending Completed";
                                            }
                                            else
                                            {
                                                result.Status = SystemFunction.process_Success;
                                                result.Msg = "Sending Completed";
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
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Nomination Not Found.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Nomination not found.";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult ApproveNominationReject(vNominationForm_obj_save ItemData, string sMode, string sAgree)
        {
            vNominationForm_Return result = new vNominationForm_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.IdEncrypt) && !string.IsNullOrEmpty(sMode) || sMode == "N")
                {
                    if (sMode == "N" && (ItemData.revise_remark + "").Trim() == "")
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error,Please provide the remark why you are revise this Nomination Form.";
                        return Json(new
                        {
                            result
                        });
                    }

                    // var _GetStatusRevis = _TM_PTR_Eva_StatusService.Find((int)Eva_Status.Waiting_for_Revised_Evaluate);
                    int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));


                    var _getData = _PES_NominationService.Find(nId);
                    if (_getData != null)
                    {
                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        //var _getSelfApprove = _getData.PTR_Evaluation_Approve.Where(w => w.Req_Approve_user == CGlobal.UserInfo.EmployeeNo && w.active_status == "Y" && w.Approve_status != "Y").FirstOrDefault();
                        List<int> nStatus = new List<int>();
                        nStatus.Add((int)PESClass.Nomination_Status.Waiting_Approval);
                        List<int> nSelf = new List<int>();
                        nSelf.Add((int)PESClass.SignaturesStep.Nominating);
                        var _getApprove = _PES_Nomination_SignaturesService.GetApproveByEva(_getData.Id, nStatus, nSelf);
                        if (_getApprove != null && _getApprove.Any())
                        {
                            List<string> MailCC = new List<string>();
                            _getData.TM_PES_NMN_Status_Id = (int)PESClass.Nomination_Status.Revised_Form;
                            _getData.comments = ItemData.revise_remark;
                            var sComplect = _PES_NominationService.Complect(_getData);
                            if (sComplect > 0)
                            {
                                foreach (var item in _getApprove)
                                {
                                    item.PES_Nomination = _getData;
                                    item.update_user = CGlobal.UserInfo.UserId;
                                    item.update_date = dNow;
                                    item.Approve_status = "";
                                    item.Approve_date = null;
                                    item.Approve_user = "";
                                    item.Agree_Status = "";
                                    //item.responses = "";
                                    sComplect = _PES_Nomination_SignaturesService.Update(item);
                                    if (sComplect > 0)
                                    {
                                        MailCC.Add(item.Req_Approve_user);
                                    }
                                }
                                string sError = "";
                                string mail_to_log = "";
                                var Mail1 = _MailContentService.Find((int)HCMClass.MailContentPTR.Nomination_Revise);
                                var GetMail = _PES_NominationService.Find(_getData.Id);
                                if (GetMail != null)
                                {
                                    var bSuss = SendNominationReviseNewVersion(GetMail, MailCC, Mail1, ref sError, ref mail_to_log);
                                    if (bSuss)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                        result.Msg = "Sending Completed";
                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Success;
                                        result.Msg = "Sending Completed";
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
                        result.Msg = "Error, Nomination not found.";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Nomination not found.";
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