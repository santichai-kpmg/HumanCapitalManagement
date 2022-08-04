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

namespace HumanCapitalManagement.Controllers.MainReport
{
    public class CandidateStatusReportController : BaseController
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
        private TM_Additional_InformationService _TM_Additional_InformationService;
        private TM_Additional_QuestionsService _TM_Additional_QuestionsService;
        private TM_Additional_AnswersService _TM_Additional_AnswersService;

        private DivisionService _DivisionService;

        private New_HRISEntities dbHr = new New_HRISEntities();
        public CandidateStatusReportController(PersonnelRequestService PersonnelRequestService
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
            _TM_Additional_InformationService = TM_Additional_InformationService;
            _TM_Additional_QuestionsService = TM_Additional_QuestionsService;
            _TM_Additional_AnswersService = TM_Additional_AnswersService;
            _DivisionService = DivisionService;
        }
        // GET: CandidateStatusReport
        public ActionResult rCandidateStatusList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            vrCandidateStatus result = new vrCandidateStatus();
            result.tif_type = "X";
            result.lstSubGroup = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPosition = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.sub_group_id = "";
            result.position_id = "";
            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "rpTIFlst" + unixTimestamps;
            result.session = sSession;
            vrCandidateStatus_Session objSession = new vrCandidateStatus_Session();
            Session[sSession] = new vrCandidateStatus_Session();//new List<ListAdvanceTransaction>();
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
                List<vrCandidateStatus_obj> lstData_resutl = new List<vrCandidateStatus_obj>();
                List<TM_Candidate_TIF> lstData = new List<TM_Candidate_TIF>();
                List<TM_Candidate_MassTIF> lstDataMass = new List<TM_Candidate_MassTIF>();
                List<TM_PR_Candidate_Mapping> lstAllData = new List<TM_PR_Candidate_Mapping>();
                lstAllData = _TM_PR_Candidate_MappingService.GetDataCandidateReport(SearchItem.group_code, aDivisionPermission,
                         "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin).ToList();
                result.active_status = SearchItem.active_status;
                result.group_code = SearchItem.group_code;
                result.tif_type = SearchItem.tif_type;
                result.sub_group_id = SearchItem.sub_group_id;
                result.position_id = SearchItem.position_id;
                result.ref_no = SearchItem.ref_no;
                result.name = SearchItem.name;

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstAllData.Any())
                {
                    lstDataMass = _TM_Candidate_MassTIFService.FindByMappingArrayID(lstAllData.Where(w => w.PersonnelRequest.type_of_TIFForm + "" == "M").Select(s => s.Id).ToArray()).ToList();
                    lstData = _TM_Candidate_TIFService.FindByMappingArrayID(lstAllData.Where(w => w.PersonnelRequest.type_of_TIFForm + "" == "N").Select(s => s.Id).ToArray()).ToList();
                    string[] aRecruit = lstAllData.Select(s => s.TM_Recruitment_Team.user_no).ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aRecruit.Contains(w.EmpNo)).ToList();
                    lstData_resutl = (from lstAD in lstAllData
                                      from lstTIF in lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_TIF())
                                      from lstMassTIF in lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_MassTIF())
                                      from Emp in _getEmp.Where(w => w.EmpNo == lstAD.TM_Recruitment_Team.user_no).DefaultIfEmpty(new AllInfo_WS())
                                      select new vrCandidateStatus_obj
                                      {
                                          Id = lstAD.Id,
                                          pr_status = lstAD.PersonnelRequest.TM_PR_Status != null ? lstAD.PersonnelRequest.TM_PR_Status.status_name_en + "" : "-",
                                          pr_status_id = lstAD.PersonnelRequest.TM_PR_Status.Id + "",
                                          IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                          refno = lstAD.PersonnelRequest.RefNo + "",
                                          group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                          sub_group = lstAD.PersonnelRequest.TM_SubGroup != null ? lstAD.PersonnelRequest.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                          position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                          pr_rank = lstAD.PersonnelRequest.TM_Pool_Rank != null ? lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en + "" : "",
                                          candidate_status = lstAD.Current_Status(),
                                          candidate_status_date = lstAD.Current_Status_date().HasValue ? lstAD.Current_Status_date().Value.DateTimebyCulture() : "",
                                          rank = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.Recommended_Rank != null ? lstTIF.Recommended_Rank.Pool_rank_short_name_en : "") : (lstMassTIF.Recommended_Rank != null ? lstMassTIF.Recommended_Rank.Pool_rank_short_name_en : ""),
                                          name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                          //status = lstAD.submit_status != null ? (lstAD.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting",
                                          tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.TM_TIF_Status != null ? lstTIF.TM_TIF_Status.tif_short_name_en + "" : "") : (lstMassTIF.TM_MassTIF_Status != null ? lstMassTIF.TM_MassTIF_Status.masstif_short_name_en + "" : ""),
                                          pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : (lstAD.PersonnelRequest.type_of_TIFForm + "" == "M" ? "Mass" : ""),
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
        //Load Dataforreport
        [HttpPost]
        public ActionResult LoadCandidateStatusList(CSearchTIFReport SearchItem)
        {
            vrCandidateStatus_Return result = new vrCandidateStatus_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new
                {
                    result
                });
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            DateTime? dStart = null, dTo = null;
            if (!string.IsNullOrEmpty(SearchItem.target_start))
            {
                dStart = SystemFunction.ConvertStringToDateTime(SearchItem.target_start, "");
            }
            if (!string.IsNullOrEmpty(SearchItem.target_end))
            {
                dTo = SystemFunction.ConvertStringToDateTime(SearchItem.target_end, "");
            }
            List<vrCandidateStatus_obj> lstData_resutl = new List<vrCandidateStatus_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
            int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
            List<TM_Candidate_TIF> lstData = new List<TM_Candidate_TIF>();
            List<TM_Candidate_MassTIF> lstDataMass = new List<TM_Candidate_MassTIF>();
            List<TM_PR_Candidate_Mapping> lstAllData = new List<TM_PR_Candidate_Mapping>();
            lstAllData = _TM_PR_Candidate_MappingService.GetDataCandidateReport(SearchItem.group_code, aDivisionPermission,
                                     "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin).ToList();
            //if (SearchItem.tif_type + "" == "X" || SearchItem.tif_type + "" == "")
            //{
            //    lstData = _TM_Candidate_TIFService.GetReportList(SearchItem.group_code, aDivisionPermission,
            //                         "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin).ToList();
            //    lstDataMass = _TM_Candidate_MassTIFService.GetReportList(
            //                          SearchItem.group_code, aDivisionPermission,
            //                         "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin).ToList();
            //}
            //else if (SearchItem.tif_type + "" == "N")
            //{
            //    lstData = _TM_Candidate_TIFService.GetReportList(SearchItem.group_code, aDivisionPermission,
            //                         "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin).ToList();
            //}
            //else if (SearchItem.tif_type + "" == "M")
            //{
            //    lstDataMass = _TM_Candidate_MassTIFService.GetReportList(
            //                               SearchItem.group_code, aDivisionPermission,
            //                              "Y", SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, dStart, dTo, isAdmin).ToList();
            //}


            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            string BackUrl = Uri.EscapeDataString(qryStr);
            vrCandidateStatus_Session objSession = new vrCandidateStatus_Session();
            if (lstAllData.Any())
            {
                lstDataMass = _TM_Candidate_MassTIFService.FindByMappingArrayID(lstAllData.Where(w => w.PersonnelRequest.type_of_TIFForm + "" == "M").Select(s => s.Id).ToArray()).ToList();
                lstData = _TM_Candidate_TIFService.FindByMappingArrayID(lstAllData.Where(w => w.PersonnelRequest.type_of_TIFForm + "" == "N").Select(s => s.Id).ToArray()).ToList();
                string[] aRecruit = lstAllData.Select(s => s.TM_Recruitment_Team.user_no).ToArray();
                var _getEmp = dbHr.AllInfo_WS.Where(w => aRecruit.Contains(w.EmpNo)).ToList();
                lstData_resutl = (from lstAD in lstAllData
                                  from lstTIF in lstData.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_TIF())
                                  from lstMassTIF in lstDataMass.Where(w => w.TM_PR_Candidate_Mapping.Id == lstAD.Id).DefaultIfEmpty(new TM_Candidate_MassTIF())
                                  from Emp in _getEmp.Where(w => w.EmpNo == lstAD.TM_Recruitment_Team.user_no).DefaultIfEmpty(new AllInfo_WS())
                                  select new vrCandidateStatus_obj
                                  {
                                      Id = lstAD.Id,
                                      pr_status = lstAD.PersonnelRequest.TM_PR_Status != null ? lstAD.PersonnelRequest.TM_PR_Status.status_name_en + "" : "-",
                                      pr_status_id = lstAD.PersonnelRequest.TM_PR_Status.Id + "",
                                      IdEncrypt = HCMFunc.Encrypt(lstAD.Id + ""),
                                      refno = lstAD.PersonnelRequest.RefNo + "",
                                      group_name = lstAD.PersonnelRequest.TM_Divisions.division_name_en + "",
                                      sub_group = lstAD.PersonnelRequest.TM_SubGroup != null ? lstAD.PersonnelRequest.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                      position = lstAD.PersonnelRequest.TM_Position.position_name_en + "",
                                      pr_rank = lstAD.PersonnelRequest.TM_Pool_Rank != null ? lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en + "" : "",
                                      candidate_status = lstAD.Current_Status(),
                                      candidate_status_date = lstAD.Current_Status_date().HasValue ? lstAD.Current_Status_date().Value.DateTimebyCulture() : "",
                                      rank = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.Recommended_Rank != null ? lstTIF.Recommended_Rank.Pool_rank_short_name_en : "") : (lstMassTIF.Recommended_Rank != null ? lstMassTIF.Recommended_Rank.Pool_rank_short_name_en : ""),
                                      name_en = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                      //status = lstAD.submit_status != null ? (lstAD.submit_status == "Y" ? "Submit" : "Save Draft") : "Waiting",
                                      tif_result = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? (lstTIF.TM_TIF_Status != null ? lstTIF.TM_TIF_Status.tif_short_name_en + "" : "") : (lstMassTIF.TM_MassTIF_Status != null ? lstMassTIF.TM_MassTIF_Status.masstif_short_name_en + "" : ""),
                                      pr_type = lstAD.PersonnelRequest.type_of_TIFForm + "" == "N" ? "Non-Mass" : (lstAD.PersonnelRequest.type_of_TIFForm + "" == "M" ? "Mass" : ""),
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
                objSession.lstData = lstData_resutl.ToList();
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