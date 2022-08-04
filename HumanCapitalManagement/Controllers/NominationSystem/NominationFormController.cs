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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.NominationSystem
{
    public class NominationFormController : BaseController
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
        New_HRISEntities dbHr = new New_HRISEntities();
        public NominationFormController(PES_Nomination_YearService PES_Nomination_YearService
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
        }

        // GET: NominationForm
        public ActionResult NominationFormList(string qryStr)
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
                var lstData = _PES_NominationService.GetNominationList(nYear, CGlobal.UserInfo.EmployeeNo, CGlobal.UserIsAdminPES());

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                    result.fy_year = SearchItem.fy_year;

                    string[] empNO = lstData.Where(a => a.active_status == "Y").Select(s => s.user_no).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    result.lstData = (from lstAD in lstData
                                      from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vNominationForm_obj
                                      {
                                          srank = lstEmpReq.RankCode,
                                          sgroup = lstEmpReq.UnitGroupName,
                                          name_en = lstAD.user_no + " : " + lstEmpReq.EmpFullName,
                                          active_status = lstAD.TM_PES_NMN_Status != null ? lstAD.TM_PES_NMN_Status.status_name_en : "", //lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                          fy_year = lstAD.PES_Nomination_Year.evaluation_year.HasValue ? lstAD.PES_Nomination_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          name = lstEmpReq.EmpFullName,
                                          nmn_type = lstAD.TM_PES_NMN_Type != null ? lstAD.TM_PES_NMN_Type.type_name_en : "",
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
        public ActionResult NominationFormEdit(string qryStr)
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
                    //canEdit.Add((int)PESClass.Nomination_Status.Form_Completed);
                    var _getData = _PES_NominationService.Find(nId);
                    ViewBag.formyear = _getData.PES_Nomination_Year.evaluation_year.Value.Year;
                    if (_getData != null)
                    {
                        if ((_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES()
                            || (
                            _getData.PES_Nomination_Signatures.Any(a => a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo
                            && a.active_status == "Y"
                            && a.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Sponsoring_Partner)
                            && !canEdit.Contains(_getData.TM_PES_NMN_Status.Id)
                            )

                            ))
                        {
                            var _checkActiv = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_no).FirstOrDefault();
                            if (_checkActiv != null)
                            {


                                result.admin_mode = CGlobal.UserIsAdminPES();

                                result.view_type = "1";
                                //edit for editdata
                                string isCurrent = "Y"; //"N";
                                result.disble_mode = ""; // "disabled";
                                string sMode = "N";//"Y";
                                result.sMode = "N";//"Y";

                                if (canEdit.Contains(_getData.TM_PES_NMN_Status.Id))
                                {

                                    sMode = "N";//
                                    isCurrent = "Y";
                                    result.disble_mode = "";
                                    result.sMode = "N";
                                }
                                else
                                {
                                    if (_getData.PES_Nomination_Signatures.Any(a => (a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                                    && a.active_status == "Y"
                                    && a.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Sponsoring_Partner
                                    && a.Approve_status != "Y"
                                   ))
                                    {
                                        sMode = "N";
                                        isCurrent = "Y";
                                        result.view_type = "2";
                                        result.sMode = "N";
                                        result.disble_mode = "";
                                        result.approve_remark = HCMFunc.DataDecryptPES(_getData.PES_Nomination_Signatures.Where(w => w.active_status == "Y" && w.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Sponsoring_Partner).Select(s => s.responses).FirstOrDefault() + "");
                                    }
                                    else if (_getData.TM_PES_NMN_Status_Id == (int)PESClass.Nomination_Status.Form_Completed)
                                    {
                                        sMode = "N";
                                        isCurrent = "Y";
                                        result.view_type = "2";
                                        result.sMode = "N";
                                        result.disble_mode = "";
                                        result.approve_remark = HCMFunc.DataDecryptPES(_getData.PES_Nomination_Signatures.Where(w => w.active_status == "Y" && w.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Sponsoring_Partner).Select(s => s.responses).FirstOrDefault() + "");

                                    }
                                }


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
                                if (_getData.PES_Nomination_Files != null && _getData.PES_Nomination_Files.Any(a => a.active_status == "Y"))
                                {
                                    int nFile = 1;
                                    result.lstFile = _getData.PES_Nomination_Files.Where(a => a.active_status == "Y").Select(s => new vNominationForm_lst_File
                                    {
                                        file_name = s.sfile_oldname,
                                        description = s.description,
                                        //Edit = (_getData.TM_PES_NMN_Status.Id != (int)PESClass.Nomination_Status.Draft_Form && _getData.TM_PES_NMN_Status.Id != (int)PESClass.Nomination_Status.Revised_Form) ? "" + nFile++ : @"<button id=""btnEdit""  type=""button"" onclick=""DeleteFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",

                                        Edit = @"<button id=""btnEdit""  type=""button"" onclick=""DeleteFile('" + HCMFunc.EncryptPES(s.Id + "") + @"')"" class=""btn btn-xs btn-danger""><i class=""glyphicon glyphicon-trash""></i></button>",
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
                                    string[] empNO = _getData.PES_Nomination_Signatures.Where(a => a.active_status == "Y").Select(s => s.Req_Approve_user).ToArray();
                                    var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();

                                    var testsub = PESFunc.GetNominationApproveList(_getData.PES_Nomination_Signatures.Where(w => w.active_status == "Y").ToList(), _getData);

                                    List<vNominationForm_lst_approve> setnew = new List<vNominationForm_lst_approve>();
                                    foreach (var ex in testsub)
                                    {
                                        vNominationForm_lst_approve lst = new vNominationForm_lst_approve();
                                        lst = ex;
                                        if (_getData.PES_Nomination_Signatures.Any(a =>
                               a.active_status == "Y"
                              && a.TM_PES_NMN_SignatureStep_Id != (int)PESClass.SignaturesStep.Self) && _getData.user_no == CGlobal.UserInfo.EmployeeNo)
                                        {
                                            lst.description = "";
                                        }

                                        //CGlobal.UserInfo.EmployeeNo = "00001654";
                                        //if (_getData.user_no == CGlobal.UserInfo.EmployeeNo)
                                        //{

                                        //    lst.description = "";
                                        //}

                                        //var abc = _getData.PES_Nomination_Signatures.ToList();


                                        setnew.Add(lst);
                                    }

                                    result.lstApprove = setnew;
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
                                                        isCurrent = isCurrent,//canEdit.Contains(_getData.TM_PES_NMN_Status.Id) ? "Y" : "N",
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

                                    if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2020 || _getData.PES_Nomination_Year.evaluation_year.Value.Year == 2021|| _getData.PES_Nomination_Year.evaluation_year.Value.Year == 2022)
                                    {
                                        result.lstKPIs = (from lst in _getKPIs
                                                          from lstKPICur in _getDataYCur.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                          from lstKPIOne in _getDataYOne.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                          from lstKPITwo in _getDataYTwo.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                          from lstKPIThree in _getDataYThree.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                          select new vNominationForm_KPIs
                                                          {
                                                              sname = lst.kpi_name_en,
                                                              target_data = (
                                                              lst.Id != (int)PESClass.KPIsBase.Contribution_margin
                                                              && lst.Id != (int)PESClass.KPIsBase.Recovery_rate
                                                              && lst.Id != (int)PESClass.KPIsBase.Chargeability
                                                              && lst.Id != (int)PESClass.KPIsBase.Lock_up
                                                              ? HCMFunc.DataDecryptPES(lstKPICur.target)
                                                              :
                                                              (lst.Id == (int)PESClass.KPIsBase.Contribution_margin
                                                              ? lstKPICur.TM_KPIs_Base.base_min.ToString()+"-"+ lstKPICur.TM_KPIs_Base.base_max.ToString() + "%" : (lst.Id == (int)PESClass.KPIsBase.Recovery_rate
                                                              ? lstKPICur.TM_KPIs_Base.base_min.ToString() + "-" + lstKPICur.TM_KPIs_Base.base_max.ToString() + "%" : (lst.Id == (int)PESClass.KPIsBase.Chargeability
                                                              ? HCMFunc.DataDecryptPES(lstKPICur.target) : (lst.Id == (int)PESClass.KPIsBase.Lock_up
                                                              ? lstKPICur.TM_KPIs_Base.base_min.ToString()  : ""))))
                                                              ),
                                                              yearoneTar = HCMFunc.DataDecryptPES(lstKPIOne.target),
                                                              // (lst.Id != (int)PESClass.KPIsBase.Contribution_margin && lst.Id != (int)PESClass.KPIsBase.Recovery_rate && lst.Id != (int)PESClass.KPIsBase.Chargeability ? HCMFunc.DataDecryptPES(lstKPIOne.target) : (lst.Id == (int)PESClass.KPIsBase.Contribution_margin ? "55-70%" : (lst.Id == (int)PESClass.KPIsBase.Recovery_rate ? "42-60%" : (lst.Id == (int)PESClass.KPIsBase.Chargeability ? "50-70%" : "")))),
                                                              yeartwoTar = HCMFunc.DataDecryptPES(lstKPITwo.target),
                                                              // (lst.Id != (int)PESClass.KPIsBase.Contribution_margin && lst.Id != (int)PESClass.KPIsBase.Recovery_rate && lst.Id != (int)PESClass.KPIsBase.Chargeability ? HCMFunc.DataDecryptPES(lstKPITwo.target) : (lst.Id == (int)PESClass.KPIsBase.Contribution_margin ? "55-70%" : (lst.Id == (int)PESClass.KPIsBase.Recovery_rate ? "42-60%" : (lst.Id == (int)PESClass.KPIsBase.Chargeability ? "50-70%" : "")))),
                                                              yearthreeTar = HCMFunc.DataDecryptPES(lstKPIThree.target),
                                                              //  (lst.Id != (int)PESClass.KPIsBase.Contribution_margin && lst.Id != (int)PESClass.KPIsBase.Recovery_rate && lst.Id != (int)PESClass.KPIsBase.Chargeability ? HCMFunc.DataDecryptPES(lstKPIThree.target) : (lst.Id == (int)PESClass.KPIsBase.Contribution_margin ? "55-70%" : (lst.Id == (int)PESClass.KPIsBase.Recovery_rate ? "42-60%" : (lst.Id == (int)PESClass.KPIsBase.Chargeability ? "50-70%" : "")))),

                                                              //remark = HCMFunc.DataDecryptPES(lst.final_remark + ""),
                                                              actual = HCMFunc.DataDecryptPES(lstKPICur.actual),
                                                              yearone = HCMFunc.DataDecryptPES(lstKPIOne.actual),//HCMFunc.DataDecryptPES(lstKPIOne.actual),
                                                              yeartwo = HCMFunc.DataDecryptPES(lstKPITwo.actual),//HCMFunc.DataDecryptPES(lstKPITwo.actual),
                                                              yearthree = HCMFunc.DataDecryptPES(lstKPIThree.actual),//HCMFunc.DataDecryptPES(lstKPIThree.actual),
                                                              sdisable = sMode,//,(_getData.TM_PES_NMN_Status.Id == (int)PESClass.Nomination_Status.Draft_Form || _getData.TM_PES_NMN_Status.Id == (int)PESClass.Nomination_Status.Revised_Form) ? "N" : "Y",
                                                              IdEncrypt = lst.Id + "",
                                                              stype = lst.type_of_kpi,
                                                              //target_group = lst.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lst.group_target) + "-" + HCMFunc.DataDecryptPES(lst.group_target_max) + "%" : (lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max))) : HCMFunc.DataDecryptPES(lst.group_target_max)),
                                                              //group_actual = lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_actual))) : HCMFunc.DataDecryptPES(lst.group_actual),


                                                          }).ToList();
                                    }
                                    else
                                    {
                                        result.lstKPIs = (from lst in _getKPIs
                                                          from lstKPICur in _getDataYCur.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                          from lstKPIOne in _getDataYOne.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                          from lstKPITwo in _getDataYTwo.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                          from lstKPIThree in _getDataYThree.Where(w => w.TM_KPIs_Base.Id == lst.Id).DefaultIfEmpty(new PES_Nomination_KPIs())
                                                          select new vNominationForm_KPIs
                                                          {
                                                              sname = lst.kpi_name_en,
                                                              target_data = (lst.Id != (int)PESClass.KPIsBase.Contribution_margin && lst.Id != (int)PESClass.KPIsBase.Recovery_rate && lst.Id != (int)PESClass.KPIsBase.Chargeability ? HCMFunc.DataDecryptPES(lstKPICur.target) : (lst.Id == (int)PESClass.KPIsBase.Contribution_margin ? "55-70%" : (lst.Id == (int)PESClass.KPIsBase.Recovery_rate ? "42-60%" : (lst.Id == (int)PESClass.KPIsBase.Chargeability ? "50-70%" : "")))),
                                                              yearoneTar = (lst.Id != (int)PESClass.KPIsBase.Contribution_margin && lst.Id != (int)PESClass.KPIsBase.Recovery_rate && lst.Id != (int)PESClass.KPIsBase.Chargeability ? HCMFunc.DataDecryptPES(lstKPIOne.target) : (lst.Id == (int)PESClass.KPIsBase.Contribution_margin ? "55-70%" : (lst.Id == (int)PESClass.KPIsBase.Recovery_rate ? "42-60%" : (lst.Id == (int)PESClass.KPIsBase.Chargeability ? "50-70%" : "")))),
                                                              // HCMFunc.DataDecryptPES(lstKPIOne.target), 
                                                              yeartwoTar = (lst.Id != (int)PESClass.KPIsBase.Contribution_margin && lst.Id != (int)PESClass.KPIsBase.Recovery_rate && lst.Id != (int)PESClass.KPIsBase.Chargeability ? HCMFunc.DataDecryptPES(lstKPITwo.target) : (lst.Id == (int)PESClass.KPIsBase.Contribution_margin ? "55-70%" : (lst.Id == (int)PESClass.KPIsBase.Recovery_rate ? "42-60%" : (lst.Id == (int)PESClass.KPIsBase.Chargeability ? "50-70%" : "")))),
                                                              // HCMFunc.DataDecryptPES(lstKPITwo.target), 
                                                              yearthreeTar = (lst.Id != (int)PESClass.KPIsBase.Contribution_margin && lst.Id != (int)PESClass.KPIsBase.Recovery_rate && lst.Id != (int)PESClass.KPIsBase.Chargeability ? HCMFunc.DataDecryptPES(lstKPIThree.target) : (lst.Id == (int)PESClass.KPIsBase.Contribution_margin ? "55-70%" : (lst.Id == (int)PESClass.KPIsBase.Recovery_rate ? "42-60%" : (lst.Id == (int)PESClass.KPIsBase.Chargeability ? "50-70%" : "")))),
                                                              //  HCMFunc.DataDecryptPES(lstKPIThree.target),

                                                              //remark = HCMFunc.DataDecryptPES(lst.final_remark + ""),
                                                              actual = HCMFunc.DataDecryptPES(lstKPICur.actual),
                                                              yearone = HCMFunc.DataDecryptPES(lstKPIOne.actual),//HCMFunc.DataDecryptPES(lstKPIOne.actual),
                                                              yeartwo = HCMFunc.DataDecryptPES(lstKPITwo.actual),//HCMFunc.DataDecryptPES(lstKPITwo.actual),
                                                              yearthree = HCMFunc.DataDecryptPES(lstKPIThree.actual),//HCMFunc.DataDecryptPES(lstKPIThree.actual),
                                                              sdisable = sMode,//,(_getData.TM_PES_NMN_Status.Id == (int)PESClass.Nomination_Status.Draft_Form || _getData.TM_PES_NMN_Status.Id == (int)PESClass.Nomination_Status.Revised_Form) ? "N" : "Y",
                                                              IdEncrypt = lst.Id + "",
                                                              stype = lst.type_of_kpi,
                                                              //target_group = lst.type_of_kpi == "P" ? HCMFunc.DataDecryptPES(lst.group_target) + "-" + HCMFunc.DataDecryptPES(lst.group_target_max) + "%" : (lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_target_max))) : HCMFunc.DataDecryptPES(lst.group_target_max)),
                                                              //group_actual = lst.type_of_kpi == "N" ? FormatNumber((long)SystemFunction.GetNumberNullToZero(HCMFunc.DataDecryptPES(lst.group_actual))) : HCMFunc.DataDecryptPES(lst.group_actual),


                                                          }).ToList();
                                    }
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
                                    return View("NominationFormEdit", result);
                                }
                                else if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2020)
                                {
                                    return View("NominationFormEdit20", result);
                                }
                                else if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2021)
                                {
                                    return View("NominationFormEdit21", result);
                                }
                                else if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2022)
                                {
                                    return View("NominationFormEdit22", result);
                                }
                                else if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2023)
                                {
                                    return View("NominationFormEdit23", result);
                                }
                                else if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2024)
                                {
                                    return View("NominationFormEdit24", result);
                                }
                                else if (_getData.PES_Nomination_Year.evaluation_year.Value.Year == 2025)
                                {
                                    return View("NominationFormEdit25", result);
                                }
                                
                                else
                                {
                                    return View(result);
                                }

                                //add new set year

                              
                            }
                            else
                            {
                                return RedirectToAction("NominationFormList", "NominationForm");
                            }
                            
                            
                        }
                        else
                        {
                            return RedirectToAction("ErrorNopermission", "MasterPage");
                        }

                    }
                    else
                    {
                        return RedirectToAction("NominationFormList", "NominationForm");
                    }
                }
                else
                {
                    return RedirectToAction("NominationFormList", "NominationForm");
                }
            }
            else
            {
                return RedirectToAction("NominationFormList", "NominationForm");
            }
            #endregion
        }
        public ActionResult CreatNominationFormFile(string id)
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
                    var _getData = _PES_Nomination_FilesService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.PES_Nomination != null && _getData.PES_Nomination.PES_Nomination_Signatures != null)
                        {
                            //if (_getData.PTR_Evaluation.PTR_Evaluation_Approve.Any(a => a.Req_Approve_user.Contains(CGlobal.UserInfo.EmployeeNo)) || CGlobal.UserIsAdminPES())
                            //{
                            if (_getData.sfileType == ".docx" || _getData.sfileType == ".doc")
                            {
                                return File(_getData.sfile64, "application/octet-stream", _getData.sfile_oldname + _getData.sfileType);
                            }
                            else if (_getData.sfileType == ".pdf")
                            {
                                return File(_getData.sfile64, "application/pdf", _getData.sfile_oldname + _getData.sfileType);
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

        #region Ajax Function
        [HttpPost]
        public ActionResult LoadNominationFormList(CSearchNMNForm SearchItem)
        {
            vNominationForm_Return result = new vNominationForm_Return();
            List<vNominationForm_obj> lstData_resutl = new List<vNominationForm_obj>();
            int nYear = SystemFunction.GetIntNullToZero(SearchItem.fy_year);
            var lstData = _PES_NominationService.GetNominationList(nYear, CGlobal.UserInfo.EmployeeNo, CGlobal.UserIsAdminPES());
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
                var _getPartnerUser = dbHr.AllInfo_WS.Where(w => empNO.Contains(w.EmpNo)).ToList();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                lstData_resutl = (from lstAD in lstData
                                  from lstEmpReq in _getPartnerUser.Where(w => w.EmpNo == lstAD.user_no).DefaultIfEmpty(new AllInfo_WS())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  select new vNominationForm_obj
                                  {
                                      srank = lstEmpReq.RankCode,
                                      sgroup = lstEmpReq.UnitGroupName,
                                      name_en = lstAD.user_no + " : " + lstEmpReq.EmpFullName,
                                      fy_year = lstAD.PES_Nomination_Year.evaluation_year.HasValue ? lstAD.PES_Nomination_Year.evaluation_year.Value.DateTimebyCulture("yyyy") : "",
                                      active_status = lstAD.TM_PES_NMN_Status != null ? lstAD.TM_PES_NMN_Status.status_name_en : "", //lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimeWithTimebyCulture() : "",
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimeWithTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.EncryptPES(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      name = lstEmpReq.EmpFullName,
                                      nmn_type = lstAD.TM_PES_NMN_Type != null ? lstAD.TM_PES_NMN_Type.type_name_en : "",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult SaveDraftNMNForm(vNominationForm_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vNominationForm_Return result = new vNominationForm_Return();
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
                    var _getData = _PES_NominationService.Find(nId);
                    if (_getData != null)
                    {
                        if (_getData.user_no == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdminPES())
                        {
                            DateTime? Year_joined_KPMG = null, being_AD_Director = null;
                            if (!string.IsNullOrEmpty(ItemData.Year_joined_KPMG))
                            {
                                Year_joined_KPMG = SystemFunction.ConvertStringToDateTime(ItemData.Year_joined_KPMG, "");
                            }
                            if (!string.IsNullOrEmpty(ItemData.being_AD_Director))
                            {
                                being_AD_Director = SystemFunction.ConvertStringToDateTime(ItemData.being_AD_Director, "");
                            }
                            int working_with_KPMG = (int)SystemFunction.GetNumberNullToZero(ItemData.working_with_KPMG);
                            int working_outside_KPMG = (int)SystemFunction.GetNumberNullToZero(ItemData.working_outside_KPMG);
                            int being_ADDirector = (int)SystemFunction.GetNumberNullToZero(ItemData.being_ADDirector);
                            _getData.Year_joined_KPMG = Year_joined_KPMG;
                            _getData.being_AD_Director = being_AD_Director;
                            _getData.working_outside_KPMG = working_outside_KPMG;
                            _getData.being_ADDirector = being_ADDirector;
                            _getData.working_with_KPMG = working_with_KPMG;
                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.Professional_Qualifications = ItemData.Professional_Qualifications + "";
                            _getData.DEVELOPMENT_AREAS = HCMFunc.DataEncryptPES(ItemData.DEVELOPMENT_AREAS + "");


                            List<PES_Nomination_Answer> lstAnswer = new List<PES_Nomination_Answer>();

                            #region answer

                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Client_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Client_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Client_Q2 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Client_Q2,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Client_Q3 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Client_Q3,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Client_Q4 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Client_Q4,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Business_Skills_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Business_Skills_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Business_Skills_Q2 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Business_Skills_Q2,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Management_Leadership_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Management_Leadership_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Management_Leadership_Q2 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Management_Leadership_Q2,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Management_Leadership_Q3 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Management_Leadership_Q3,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Social_Skills_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Social_Skills_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Social_Skills_Q2 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Social_Skills_Q2,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Thinking_Skills_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Thinking_Skills_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Thinking_Skills_Q2 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Thinking_Skills_Q2,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Thinking_Skills_Q3 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Thinking_Skills_Q3,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Technical_Competence_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Technical_Competence_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.RISK_MANAGEMENT_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.RISK_MANAGEMENT_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.RISK_MANAGEMENT_Q2 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.RISK_MANAGEMENT_Q2,
                                PES_Nomination = _getData,

                            });

                            if (_getData.TM_PES_NMN_Type_Id == (int)PESClass.Nomination_Type.Shareholder)
                            {

                                lstAnswer.Add(new PES_Nomination_Answer
                                {
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    answer = HCMFunc.DataEncryptPES(ItemData.Technical_Competence_Q2 + ""),
                                    active_status = "Y",
                                    TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Technical_Competence_Q2,
                                    PES_Nomination = _getData,

                                });

                                lstAnswer.Add(new PES_Nomination_Answer
                                {
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    answer = HCMFunc.DataEncryptPES(ItemData.Business_Skills_Q3 + ""),
                                    active_status = "Y",
                                    TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Business_Skills_Q3,
                                    PES_Nomination = _getData,

                                });
                                lstAnswer.Add(new PES_Nomination_Answer
                                {
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    answer = HCMFunc.DataEncryptPES(ItemData.Technical_Competence_Q3 + ""),
                                    active_status = "Y",
                                    TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Technical_Competence_Q3,
                                    PES_Nomination = _getData,

                                });

                            }
                            #endregion


                            List<PES_Nomination_Competencies> lstCompetencies = new List<PES_Nomination_Competencies>();
                            var _getCompetencies = _TM_PES_NMN_CompetenciesService.GetDataForSelect();
                            if (ItemData.lstRating_Summary != null)
                            {
                                lstCompetencies = (from lstA in ItemData.lstRating_Summary.Where(w => w.isCurrent == "Y" && w.nrate.HasValue && w.nrate != 0)
                                                   from lstQ in _getCompetencies.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new TM_PES_NMN_Competencies())
                                                   select new PES_Nomination_Competencies
                                                   {
                                                       update_user = CGlobal.UserInfo.UserId,
                                                       update_date = dNow,
                                                       create_user = CGlobal.UserInfo.UserId,
                                                       create_date = dNow,
                                                       active_status = "Y",
                                                       TM_PES_NMN_Competencies = lstQ != null ? lstQ : null,
                                                       PES_Nomination = _getData,
                                                       TM_PES_NMN_Competencies_Rating_Id = lstA.nrate,
                                                   }).ToList();
                            }
                            List<PES_Nomination_KPIs> lstKPIsThree = new List<PES_Nomination_KPIs>();
                            List<PES_Nomination_KPIs> lstKPIstwo = new List<PES_Nomination_KPIs>();
                            List<PES_Nomination_KPIs> lstKPIsone = new List<PES_Nomination_KPIs>();
                            List<PES_Nomination_KPIs> lstKPIsCur = new List<PES_Nomination_KPIs>();
                            var CurrentYear = _getData.PES_Nomination_Year.evaluation_year;
                            var _getTheeYear = _PES_Final_Rating_YearService.FindByYear(CurrentYear.Value.AddYears(-3).Year);
                            var _getTwoYear = _PES_Final_Rating_YearService.FindByYear(CurrentYear.Value.AddYears(-2).Year);
                            var _getOneYear = _PES_Final_Rating_YearService.FindByYear(CurrentYear.Value.AddYears(-1).Year);
                            var _getCurYear = _PES_Final_Rating_YearService.FindByYear(CurrentYear.Value.Year);


                            var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                            if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any())
                            {




                                if (_getTheeYear != null)
                                {
                                    lstKPIsThree.AddRange(ItemData.lstKPIs.Select(s => new PES_Nomination_KPIs
                                    {
                                        active_status = "Y",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_KPIs_Base_Id = SystemFunction.GetIntNullToZero(s.IdEncrypt + ""),
                                        PES_Final_Rating_Year = _getTheeYear,
                                        user_id = _getData.user_no,
                                        actual = HCMFunc.DataEncryptPES(s.yearthree + ""),
                                        target = HCMFunc.DataEncryptPES(s.yearthreeTar + ""),
                                    }).ToList());
                                }
                                if (_getTwoYear != null)
                                {
                                    lstKPIstwo.AddRange(ItemData.lstKPIs.Select(s => new PES_Nomination_KPIs
                                    {
                                        active_status = "Y",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_KPIs_Base_Id = SystemFunction.GetIntNullToZero(s.IdEncrypt + ""),
                                        PES_Final_Rating_Year = _getTwoYear,
                                        user_id = _getData.user_no,
                                        actual = HCMFunc.DataEncryptPES(s.yeartwo + ""),
                                        target = HCMFunc.DataEncryptPES(s.yeartwoTar + ""),
                                    }).ToList());
                                }
                                if (_getOneYear != null)
                                {
                                    lstKPIsone.AddRange(ItemData.lstKPIs.Select(s => new PES_Nomination_KPIs
                                    {
                                        active_status = "Y",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_KPIs_Base_Id = SystemFunction.GetIntNullToZero(s.IdEncrypt + ""),
                                        PES_Final_Rating_Year = _getOneYear,
                                        user_id = _getData.user_no,
                                        actual = HCMFunc.DataEncryptPES(s.yearone + ""),
                                        target = HCMFunc.DataEncryptPES(s.yearoneTar + ""),
                                    }).ToList());
                                }
                                if (_getCurYear != null)
                                {
                                    lstKPIsCur.AddRange(ItemData.lstKPIs.Select(s => new PES_Nomination_KPIs
                                    {
                                        active_status = "Y",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_KPIs_Base_Id = SystemFunction.GetIntNullToZero(s.IdEncrypt + ""),
                                        PES_Final_Rating_Year = _getCurYear,
                                        user_id = _getData.user_no,
                                        actual = HCMFunc.DataEncryptPES(s.actual + ""),
                                        target = HCMFunc.DataEncryptPES(s.target_data + ""),
                                    }).ToList());
                                }
                            }

                            var sComplect = _PES_NominationService.Complect(_getData);
                            if (sComplect > 0)
                            {

                                var _getRateYCur = _PES_Final_RatingService.FindForYearRate(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.Year);
                                int nSelfRate = SystemFunction.GetIntNullToZero(ItemData.yearcurrent_selfrate + "");
                                if (_getRateYCur != null && nSelfRate > 0)
                                {
                                    _getRateYCur.Self_TM_Annual_Rating_Id = nSelfRate;
                                    _getRateYCur.update_user = CGlobal.UserInfo.UserId;
                                    _getRateYCur.update_date = dNow;
                                    _PES_Final_RatingService.Update(_getRateYCur);

                                }

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
                                if (lstAnswer.Any())
                                {
                                    _PES_Nomination_AnswerService.UpdateAnswer(lstAnswer, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                }

                                if (lstCompetencies.Any())
                                {
                                    _PES_Nomination_CompetenciesService.UpdateAnswer(lstCompetencies, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                }

                                if (lstKPIsThree.Any() && _getTheeYear != null)
                                {
                                    _PES_Nomination_KPIsService.UpdateKPIs(lstKPIsThree, _getTheeYear.Id, _getData.user_no, CGlobal.UserInfo.UserId, dNow);
                                }
                                if (lstKPIstwo.Any() && _getTwoYear != null)
                                {
                                    _PES_Nomination_KPIsService.UpdateKPIs(lstKPIstwo, _getTwoYear.Id, _getData.user_no, CGlobal.UserInfo.UserId, dNow);
                                }
                                if (lstKPIsone.Any() && _getOneYear != null)
                                {
                                    _PES_Nomination_KPIsService.UpdateKPIs(lstKPIsone, _getOneYear.Id, _getData.user_no, CGlobal.UserInfo.UserId, dNow);
                                }
                                if (lstKPIsCur.Any() && _getCurYear != null)
                                {
                                    _PES_Nomination_KPIsService.UpdateKPIs(lstKPIsCur, _getCurYear.Id, _getData.user_no, CGlobal.UserInfo.UserId, dNow);
                                }

                                //var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                                //if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any() && _getData.PTR_Evaluation_KPIs != null)
                                //{
                                //    foreach (var item in ItemData.lstKPIs)
                                //    {
                                //        var GetKPI = _getData.PTR_Evaluation_KPIs.Where(w => w.Id + "" == item.IdEncrypt).FirstOrDefault();
                                //        if (GetKPI != null)
                                //        {
                                //            //if (GetKPI.TM_KPIs_Base.type_of_kpi == "N")
                                //            //{
                                //            //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "") * 1000000) + "");
                                //            //}
                                //            //else if (GetKPI.TM_KPIs_Base.type_of_kpi == "D")
                                //            //{
                                //            //    GetKPI.target_max = HCMFunc.DataEncryptPES((SystemFunction.GetNumberNullToZero(item.target_data + "")) + "");
                                //            //}
                                //            GetKPI.final_remark = HCMFunc.DataEncryptPES(item.remark + "");
                                //            GetKPI.update_user = CGlobal.UserInfo.UserId;
                                //            GetKPI.update_date = dNow;
                                //            GetKPI.PTR_Evaluation = _getData;
                                //            _PTR_Evaluation_KPIsService.Update(GetKPI);
                                //        }
                                //    }
                                //}
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
                            result.Msg = "Error, Don't have permission to save.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Nomination Not Found.";
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
        public ActionResult SaveNMNForm(vNominationForm_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vNominationForm_Return result = new vNominationForm_Return();
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
                    var _getData = _PES_NominationService.Find(nId);
                    if (_getData != null)
                    {
                        dNow = _getData.update_date.Value;
                        if ((_getData.user_no == CGlobal.UserInfo.EmployeeNo
                            || CGlobal.UserIsAdminPES())
                            || _getData.PES_Nomination_Signatures.Any(a =>
                        a.Req_Approve_user == CGlobal.UserInfo.EmployeeNo
                        && a.active_status == "Y"
                        && a.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Sponsoring_Partner
                        && a.Approve_status != "Y"))
                        {
                            DateTime? Year_joined_KPMG = null, being_AD_Director = null;
                            if (!string.IsNullOrEmpty(ItemData.Year_joined_KPMG))
                            {
                                Year_joined_KPMG = SystemFunction.ConvertStringToDateTime(ItemData.Year_joined_KPMG, "");
                            }
                            if (!string.IsNullOrEmpty(ItemData.being_AD_Director))
                            {
                                being_AD_Director = SystemFunction.ConvertStringToDateTime(ItemData.being_AD_Director, "");
                            }
                            int working_with_KPMG = (int)SystemFunction.GetNumberNullToZero(ItemData.working_with_KPMG);
                            int working_outside_KPMG = (int)SystemFunction.GetNumberNullToZero(ItemData.working_outside_KPMG);
                            int being_ADDirector = (int)SystemFunction.GetNumberNullToZero(ItemData.being_ADDirector);

                            #region validation

                            if (string.IsNullOrEmpty(ItemData.Client_Q1))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Client responsiveness.";
                                return Json(new
                                {
                                    result
                                });
                            }


                            if (string.IsNullOrEmpty(ItemData.Client_Q2))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Feedback from clients (Identify critical evidence).";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Client_Q3))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input New engagements acquired (Name /Fee not including referrals).";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Client_Q4))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Lost Accounts.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Business_Skills_Q1))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Commerciality.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Business_Skills_Q2))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Business Development.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Business_Skills_Q3) && _getData.TM_PES_NMN_Type.Id == (int)PESClass.Nomination_Type.Shareholder)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Alignment of Firm Strategy.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Management_Leadership_Q1))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Leadership Skills.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Management_Leadership_Q2))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Team Skills.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Management_Leadership_Q3))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input People Development.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Social_Skills_Q1))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Communication skills.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Social_Skills_Q2))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Self-Confidence.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Thinking_Skills_Q1))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Analytical Judgement.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Thinking_Skills_Q2))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Innovation.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Thinking_Skills_Q3))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Vision.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Technical_Competence_Q1))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Technical Competence.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Technical_Competence_Q2) && _getData.TM_PES_NMN_Type.Id == (int)PESClass.Nomination_Type.Shareholder)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input KPMG Story and KPMG Way.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.Technical_Competence_Q3) && _getData.TM_PES_NMN_Type.Id == (int)PESClass.Nomination_Type.Shareholder)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Entrepreneurship.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.RISK_MANAGEMENT_Q1))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Risk Management.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.RISK_MANAGEMENT_Q2))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Results from QPR / SEC/ RCP reviews (Issues identified, actions taken).";
                                return Json(new
                                {
                                    result
                                });
                            }

                            if (string.IsNullOrEmpty(ItemData.DEVELOPMENT_AREAS))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please input Summary of Development Areas.";
                                return Json(new
                                {
                                    result
                                });
                            }
                            if (string.IsNullOrEmpty(ItemData.yearcurrent_selfrate))
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please select Self Rating.";
                                return Json(new
                                {
                                    result
                                });
                            }

                            #endregion
                            _getData.Year_joined_KPMG = Year_joined_KPMG;
                            _getData.being_AD_Director = being_AD_Director;
                            _getData.working_outside_KPMG = working_outside_KPMG;
                            _getData.being_ADDirector = being_ADDirector;
                            _getData.working_with_KPMG = working_with_KPMG;
                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.Professional_Qualifications = ItemData.Professional_Qualifications + "";
                            _getData.DEVELOPMENT_AREAS = HCMFunc.DataEncryptPES(ItemData.DEVELOPMENT_AREAS + "");
                            //   _getData.TM_PES_NMN_Status_Id =( ItemData.type_id == "1" ? (int)PESClass.Nomination_Status.Waiting_Approval : (int)PESClass.Nomination_Status.Revised_Form);
                            if (_getData.TM_PES_NMN_Status_Id == (int)PESClass.Nomination_Status.Draft_Form || _getData.TM_PES_NMN_Status_Id == (int)PESClass.Nomination_Status.Revised_Form)
                            {
                                _getData.TM_PES_NMN_Status_Id = (int)PESClass.Nomination_Status.Waiting_Approval;
                            }

                            List<PES_Nomination_Answer> lstAnswer = new List<PES_Nomination_Answer>();

                            #region answer

                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Client_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Client_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Client_Q2 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Client_Q2,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Client_Q3 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Client_Q3,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Client_Q4 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Client_Q4,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Business_Skills_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Business_Skills_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Business_Skills_Q2 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Business_Skills_Q2,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Management_Leadership_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Management_Leadership_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Management_Leadership_Q2 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Management_Leadership_Q2,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Management_Leadership_Q3 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Management_Leadership_Q3,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Social_Skills_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Social_Skills_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Social_Skills_Q2 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Social_Skills_Q2,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Thinking_Skills_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Thinking_Skills_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Thinking_Skills_Q2 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Thinking_Skills_Q2,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Thinking_Skills_Q3 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Thinking_Skills_Q3,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.Technical_Competence_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Technical_Competence_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.RISK_MANAGEMENT_Q1 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.RISK_MANAGEMENT_Q1,
                                PES_Nomination = _getData,

                            });
                            lstAnswer.Add(new PES_Nomination_Answer
                            {
                                update_user = CGlobal.UserInfo.UserId,
                                update_date = dNow,
                                create_user = CGlobal.UserInfo.UserId,
                                create_date = dNow,
                                answer = HCMFunc.DataEncryptPES(ItemData.RISK_MANAGEMENT_Q2 + ""),
                                active_status = "Y",
                                TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.RISK_MANAGEMENT_Q2,
                                PES_Nomination = _getData,

                            });

                            if (_getData.TM_PES_NMN_Type_Id == (int)PESClass.Nomination_Type.Shareholder)
                            {

                                lstAnswer.Add(new PES_Nomination_Answer
                                {
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    answer = HCMFunc.DataEncryptPES(ItemData.Technical_Competence_Q2 + ""),
                                    active_status = "Y",
                                    TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Technical_Competence_Q2,
                                    PES_Nomination = _getData,

                                });

                                lstAnswer.Add(new PES_Nomination_Answer
                                {
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    answer = HCMFunc.DataEncryptPES(ItemData.Business_Skills_Q3 + ""),
                                    active_status = "Y",
                                    TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Business_Skills_Q3,
                                    PES_Nomination = _getData,

                                });
                                lstAnswer.Add(new PES_Nomination_Answer
                                {
                                    update_user = CGlobal.UserInfo.UserId,
                                    update_date = dNow,
                                    create_user = CGlobal.UserInfo.UserId,
                                    create_date = dNow,
                                    answer = HCMFunc.DataEncryptPES(ItemData.Technical_Competence_Q3 + ""),
                                    active_status = "Y",
                                    TM_PES_NMN_Questions_Id = (int)PESClass.Nomination_Questions.Technical_Competence_Q3,
                                    PES_Nomination = _getData,

                                });

                            }
                            #endregion
                            List<PES_Nomination_Competencies> lstCompetencies = new List<PES_Nomination_Competencies>();
                            var _getCompetencies = _TM_PES_NMN_CompetenciesService.GetDataForSelect();
                            if (ItemData.lstRating_Summary != null)
                            {
                                lstCompetencies = (from lstA in ItemData.lstRating_Summary.Where(w => w.isCurrent == "Y" && w.nrate.HasValue && w.nrate != 0)
                                                   from lstQ in _getCompetencies.Where(w => w.Id + "" == lstA.id).DefaultIfEmpty(new TM_PES_NMN_Competencies())
                                                   select new PES_Nomination_Competencies
                                                   {
                                                       update_user = CGlobal.UserInfo.UserId,
                                                       update_date = dNow,
                                                       create_user = CGlobal.UserInfo.UserId,
                                                       create_date = dNow,
                                                       active_status = "Y",
                                                       TM_PES_NMN_Competencies = lstQ != null ? lstQ : null,
                                                       PES_Nomination = _getData,
                                                       TM_PES_NMN_Competencies_Rating_Id = lstA.nrate,
                                                   }).ToList();
                            }
                            List<PES_Nomination_KPIs> lstKPIsThree = new List<PES_Nomination_KPIs>();
                            List<PES_Nomination_KPIs> lstKPIstwo = new List<PES_Nomination_KPIs>();
                            List<PES_Nomination_KPIs> lstKPIsone = new List<PES_Nomination_KPIs>();
                            List<PES_Nomination_KPIs> lstKPIsCur = new List<PES_Nomination_KPIs>();
                            var CurrentYear = _getData.PES_Nomination_Year.evaluation_year;
                            var _getTheeYear = _PES_Final_Rating_YearService.FindByYear(CurrentYear.Value.AddYears(-3).Year);
                            var _getTwoYear = _PES_Final_Rating_YearService.FindByYear(CurrentYear.Value.AddYears(-2).Year);
                            var _getOneYear = _PES_Final_Rating_YearService.FindByYear(CurrentYear.Value.AddYears(-1).Year);
                            var _getCurYear = _PES_Final_Rating_YearService.FindByYear(CurrentYear.Value.Year);
                            var _GetKPIs = _TM_KPIs_BaseService.GetDataForSelect();
                            if (ItemData.lstKPIs != null && ItemData.lstKPIs.Any())
                            {
                                if (_getTheeYear != null)
                                {
                                    lstKPIsThree.AddRange(ItemData.lstKPIs.Select(s => new PES_Nomination_KPIs
                                    {
                                        active_status = "Y",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_KPIs_Base_Id = SystemFunction.GetIntNullToZero(s.IdEncrypt + ""),
                                        PES_Final_Rating_Year = _getTheeYear,
                                        user_id = _getData.user_no,
                                        actual = HCMFunc.DataEncryptPES(s.yearthree + ""),
                                        target = HCMFunc.DataEncryptPES(s.yearthreeTar + ""),
                                    }).ToList());
                                }
                                if (_getTwoYear != null)
                                {
                                    lstKPIstwo.AddRange(ItemData.lstKPIs.Select(s => new PES_Nomination_KPIs
                                    {
                                        active_status = "Y",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_KPIs_Base_Id = SystemFunction.GetIntNullToZero(s.IdEncrypt + ""),
                                        PES_Final_Rating_Year = _getTwoYear,
                                        user_id = _getData.user_no,
                                        actual = HCMFunc.DataEncryptPES(s.yeartwo + ""),
                                        target = HCMFunc.DataEncryptPES(s.yeartwoTar + ""),
                                    }).ToList());
                                }
                                if (_getOneYear != null)
                                {
                                    lstKPIsone.AddRange(ItemData.lstKPIs.Select(s => new PES_Nomination_KPIs
                                    {
                                        active_status = "Y",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_KPIs_Base_Id = SystemFunction.GetIntNullToZero(s.IdEncrypt + ""),
                                        PES_Final_Rating_Year = _getOneYear,
                                        user_id = _getData.user_no,
                                        actual = HCMFunc.DataEncryptPES(s.yearone + ""),
                                        target = HCMFunc.DataEncryptPES(s.yearoneTar + ""),
                                    }).ToList());
                                }
                                if (_getCurYear != null)
                                {
                                    lstKPIsCur.AddRange(ItemData.lstKPIs.Select(s => new PES_Nomination_KPIs
                                    {
                                        active_status = "Y",
                                        update_user = CGlobal.UserInfo.UserId,
                                        update_date = dNow,
                                        create_user = CGlobal.UserInfo.UserId,
                                        create_date = dNow,
                                        TM_KPIs_Base_Id = SystemFunction.GetIntNullToZero(s.IdEncrypt + ""),
                                        PES_Final_Rating_Year = _getCurYear,
                                        user_id = _getData.user_no,
                                        actual = HCMFunc.DataEncryptPES(s.actual + ""),
                                        target = HCMFunc.DataEncryptPES(s.target_data + ""),
                                    }).ToList());
                                }
                            }

                            if (_getData.TM_PES_NMN_Status_Id == (int)PESClass.Nomination_Status.Form_Completed)
                                _getData.TM_PES_NMN_Status_Id = _getData.TM_PES_NMN_Status_Id;

                            var abs = _getData.TM_PES_NMN_Status_Id;

                            var sComplect = _PES_NominationService.Complect(_getData);
                            if (sComplect > 0 || 1 == 1)
                            {

                                var _getRateYCur = _PES_Final_RatingService.FindForYearRate(_getData.user_no, _getData.PES_Nomination_Year.evaluation_year.Value.Year);
                                int nSelfRate = SystemFunction.GetIntNullToZero(ItemData.yearcurrent_selfrate + "");
                                if (_getRateYCur != null && nSelfRate > 0)
                                {
                                    _getRateYCur.Self_TM_Annual_Rating_Id = nSelfRate;
                                    _getRateYCur.update_user = CGlobal.UserInfo.UserId;
                                    _getRateYCur.update_date = dNow;
                                    _PES_Final_RatingService.Update(_getRateYCur);
                                }
                                if (lstAnswer.Any())
                                {
                                    _PES_Nomination_AnswerService.UpdateAnswer(lstAnswer, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                }
                                if (lstCompetencies.Any())
                                {
                                    _PES_Nomination_CompetenciesService.UpdateAnswer(lstCompetencies, _getData.Id, CGlobal.UserInfo.UserId, dNow);
                                }
                                if (lstKPIsThree.Any() && _getTheeYear != null)
                                {
                                    _PES_Nomination_KPIsService.UpdateKPIs(lstKPIsThree, _getTheeYear.Id, _getData.user_no, CGlobal.UserInfo.UserId, dNow);
                                }
                                if (lstKPIstwo.Any() && _getTwoYear != null)
                                {
                                    _PES_Nomination_KPIsService.UpdateKPIs(lstKPIstwo, _getTwoYear.Id, _getData.user_no, CGlobal.UserInfo.UserId, dNow);
                                }
                                if (lstKPIsone.Any() && _getOneYear != null)
                                {
                                    _PES_Nomination_KPIsService.UpdateKPIs(lstKPIsone, _getOneYear.Id, _getData.user_no, CGlobal.UserInfo.UserId, dNow);
                                }
                                if (lstKPIsCur.Any() && _getCurYear != null)
                                {
                                    _PES_Nomination_KPIsService.UpdateKPIs(lstKPIsCur, _getCurYear.Id, _getData.user_no, CGlobal.UserInfo.UserId, dNow);
                                }

                                if (ItemData.view_type == "1")
                                {
                                    var _getSelfApprove = _PES_Nomination_SignaturesService.FindForEditSponsoring(_getData.Id, (int)PESClass.SignaturesStep.Self); //_getData.PES_Nomination_Signatures.Where(w => w.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Self && w.active_status == "Y").FirstOrDefault();
                                    if (_getSelfApprove != null)
                                    {
                                        _getSelfApprove.PES_Nomination = _getData;
                                        _getSelfApprove.update_user = CGlobal.UserInfo.UserId;
                                        _getSelfApprove.update_date = dNow;
                                        _getSelfApprove.Agree_Status = "Y";
                                        _getSelfApprove.Approve_status = "Y";
                                        _getSelfApprove.Approve_date = dNow;
                                        _getSelfApprove.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                        _getSelfApprove.TM_Annual_Rating_Id = nSelfRate > 0 ? nSelfRate : (int?)null;
                                        sComplect = _PES_Nomination_SignaturesService.Update(_getSelfApprove);
                                        if (sComplect > 0)
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
                                                var bSuss = SendNominationSubmit(GetMail, Mail1, ref sError, ref mail_to_log);
                                            }

                                        }
                                    }

                                }
                                else
                                {
                                    var _getSelfApprove = _PES_Nomination_SignaturesService.FindForEditSponsoring(_getData.Id, (int)PESClass.SignaturesStep.Sponsoring_Partner); //_getData.PES_Nomination_Signatures.Where(w => w.TM_PES_NMN_SignatureStep_Id == (int)PESClass.SignaturesStep.Self && w.active_status == "Y").FirstOrDefault();
                                    if (_getSelfApprove != null)
                                    {
                                        _getSelfApprove.PES_Nomination = _getData;
                                        _getSelfApprove.update_user = CGlobal.UserInfo.UserId;
                                        _getSelfApprove.update_date = dNow;
                                        _getSelfApprove.Agree_Status = ItemData.agree_status;
                                        _getSelfApprove.Approve_status = "Y";
                                        _getSelfApprove.Approve_date = dNow;
                                        _getSelfApprove.Approve_user = CGlobal.UserInfo.EmployeeNo;
                                        _getSelfApprove.responses = HCMFunc.DataEncryptPES((ItemData.approve_remark + "").Trim());
                                        sComplect = _PES_Nomination_SignaturesService.Update(_getSelfApprove);
                                        if (sComplect > 0)
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
                                                var bSuss = SendNominationSubmit(GetMail, Mail1, ref sError, ref mail_to_log);
                                            }

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
                            result.Msg = "Error, Don't have permission to save.";
                        }

                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Nomination Not Found.";
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
        public ActionResult DeleteFile(vNominationForm_File ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vNominationForm_UploadFile result = new vNominationForm_UploadFile();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.IdEncrypt + ""));
                int nFileId = SystemFunction.GetIntNullToZero(HCMFunc.DecryptPES(ItemData.File_IdEncrypt + ""));
                if (nId != 0 && nFileId != 0)
                {
                    var _getDataFile = _PES_Nomination_FilesService.FindForDelete(nId, nFileId);
                    var _getData = _PES_NominationService.Find(nId);
                    if (_getData != null && _getDataFile != null)
                    {
                        _getDataFile.PES_Nomination = _getData;
                        _getDataFile.update_user = CGlobal.UserInfo.UserId;
                        _getDataFile.update_date = dNow;
                        _getDataFile.sfile64 = null;
                        _getDataFile.active_status = "N";
                        var sComplect = _PES_Nomination_FilesService.DeleteFile(_getDataFile);
                        if (sComplect > 0)
                        {
                            var _getUpdate = _PES_NominationService.Find(nId);
                            if (_getUpdate != null && _getUpdate.PES_Nomination_Files != null && _getUpdate.PES_Nomination_Files.Any(a => a.active_status == "Y"))
                            {
                                result.lstNewData = _getUpdate.PES_Nomination_Files.Where(a => a.active_status == "Y").Select(s => new vNominationForm_lst_File
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
        public ActionResult UploadFileMulti()
        {
            vNominationForm_UploadFile result = new vNominationForm_UploadFile();
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
                    var _getData = _PES_NominationService.Find(nId);
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

                                            PES_Nomination_Files objSave = new PES_Nomination_Files()
                                            {
                                                PES_Nomination = _getData,
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


                                            var sComplect = _PES_Nomination_FilesService.CreateNew(objSave);
                                            result.Status = SystemFunction.process_Success;
                                        }
                                    }
                                }
                            }

                            var _getUpdate = _PES_NominationService.Find(nId);
                            if (_getUpdate != null && _getUpdate.PES_Nomination_Files != null && _getUpdate.PES_Nomination_Files.Any(a => a.active_status == "Y"))
                            {
                                result.lstNewData = _getUpdate.PES_Nomination_Files.Where(a => a.active_status == "Y").Select(s => new vNominationForm_lst_File
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
                    result.Msg = "Nomination not found.";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Nomination not found.";
            }

            return Json(new { result });
        }
        #endregion

    }
}