using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.Service.PreInternAssessment;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using OfficeOpenXml;
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
    public class PRCandidateController : BaseController
    {
        private DivisionService _DivisionService;
        private PoolService _PoolService;
        private EmploymentTypeService _EmploymentTypeService;
        private TM_PositionService _TM_PositionService;
        private RequestTypeService _RequestTypeService;
        private TM_PR_StatusService _TM_PR_StatusService;
        private PersonnelRequestService _PersonnelRequestService;
        private TM_Step_ApproveService _TM_Step_ApproveService;
        private TM_CandidatesService _TM_CandidatesService;
        private TM_Recruitment_TeamService _TM_Recruitment_TeamService;
        private TM_Candidate_StatusService _TM_Candidate_StatusService;
        private TM_PR_Candidate_MappingService _TM_PR_Candidate_MappingService;
        private TM_Candidate_RankService _TM_Candidate_RankService;
        private TM_Candidate_Status_CycleService _TM_Candidate_Status_CycleService;
        private TM_FY_PlanService _TM_FY_PlanService;
        private TM_PInternAssessment_ActivitiesService _TM_PInternAssessment_ActivitiesService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public PRCandidateController(DivisionService DivisionService, PoolService PoolService
     , EmploymentTypeService EmploymentTypeService
     , TM_PositionService TM_PositionService
     , RequestTypeService RequestTypeService
     , TM_Step_ApproveService TM_Step_ApproveService
     , TM_Employment_RequestService TM_Employment_RequestService
     , TM_PR_StatusService TM_PR_StatusService
     , PersonnelRequestService PersonnelRequestService
     , TM_CandidatesService TM_CandidatesService
     , TM_Recruitment_TeamService TM_Recruitment_TeamService
     , TM_Candidate_StatusService TM_Candidate_StatusService
            , TM_PR_Candidate_MappingService TM_PR_Candidate_MappingService
            , TM_Candidate_RankService TM_Candidate_RankService
            , TM_Candidate_Status_CycleService TM_Candidate_Status_CycleService
            , TM_FY_PlanService TM_FY_PlanService
            , TM_PInternAssessment_ActivitiesService TM_PInternAssessment_ActivitiesService)
        {
            _TM_Step_ApproveService = TM_Step_ApproveService;
            _DivisionService = DivisionService;
            _PoolService = PoolService;
            _EmploymentTypeService = EmploymentTypeService;
            _TM_PositionService = TM_PositionService;
            _RequestTypeService = RequestTypeService;
            _TM_PR_StatusService = TM_PR_StatusService;
            _PersonnelRequestService = PersonnelRequestService;
            _TM_CandidatesService = TM_CandidatesService;
            _TM_Recruitment_TeamService = TM_Recruitment_TeamService;
            _TM_Candidate_StatusService = TM_Candidate_StatusService;
            _TM_PR_Candidate_MappingService = TM_PR_Candidate_MappingService;
            _TM_Candidate_RankService = TM_Candidate_RankService;
            _TM_Candidate_Status_CycleService = TM_Candidate_Status_CycleService;
            _TM_FY_PlanService = TM_FY_PlanService;
            _TM_PInternAssessment_ActivitiesService = TM_PInternAssessment_ActivitiesService;
        }
        // GET: PRCandidate
        public ActionResult PRCandidateList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            vPRCandidates result = new vPRCandidates();
            result.active_status = "Y";
            result.lstSubGroup = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPosition = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.sub_group_id = "";
            result.position_id = "";
            result.rank_id = "";
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPRCandidates SearchItem = (CSearchPRCandidates)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPRCandidates)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                int nStatus = SystemFunction.GetIntNullToZero(SearchItem.pr_status + "");
                int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
                int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
                int nRank = SystemFunction.GetIntNullToZero(SearchItem.rank_id + "");
                var lstData = _PersonnelRequestService.GetPRListForAddCandidate(
            SearchItem.group_code, aDivisionPermission,
            "Y", nStatus, SearchItem.ref_no, nSubGroup, nPosition, nRank, SearchItem.name, isAdmin);
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
                    if (lst.TM_Pool.TM_Pool_Rank != null && lst.TM_Pool.TM_Pool_Rank.Any())
                    {
                        result.lstrank.AddRange(lst.TM_Pool.TM_Pool_Rank.OrderBy(o => o.TM_Rank.piority).Select(s => new vSelect_PR { id = s.Id + "", name = s.Pool_rank_name_en }).ToList());
                    }
                }
                result.group_code = SearchItem.group_code;
                result.pr_status = SearchItem.pr_status;
                result.ref_no = SearchItem.ref_no;
                result.sub_group_id = SearchItem.sub_group_id;
                result.position_id = SearchItem.position_id;
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {

                    string[] aRequestUser = lstData.Select(s => s.request_user).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    string sUrl = Url.Action("PRCandidateEdit", "PRCandidate", null, Request.Url.Scheme);
                    result.lstData = (from lstAD in lstData
                                      from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())

                                      select new vPRCandidates_obj
                                      {
                                          //refno = lstAD.RefNo + "",
                                          //sgroup = lstAD.TM_Divisions.division_name_en + "",
                                          //position = lstAD.TM_Position.position_name_en + "",
                                          //request_type = lstAD.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "",
                                          //rank = lstAD.TM_Pool_Rank.TM_Rank.rank_name_en + "",
                                          //pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                          //request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                          //hc = lstAD.no_of_headcount + "",
                                          //request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimebyCulture() : "",
                                          //create_user = lstAD.create_user,
                                          //create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                          //update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          //update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          //sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                          //pr_status_id = lstAD.TM_PR_Status.Id + "",
                                          refno = @"<a target=""_blank""  href=""" + sUrl + "?qryStr=" + HCMFunc.Encrypt(lstAD.Id + "") + @""">" + lstAD.RefNo + @"</a>",
                                          sgroup = lstAD.TM_Divisions.division_name_en + "",
                                          position = lstAD.TM_Position.position_name_en + "",
                                          request_type = lstAD.TM_Employment_Request.TM_Request_Type.request_type_name_en + "",
                                          rank = lstAD.TM_Pool_Rank.Pool_rank_short_name_en + "",
                                          approve_date = lstAD.target_period.HasValue ? lstAD.target_period.Value.DateTimebyCulture() : "",
                                          //approve_date = lstAD.TM_Pool_Rank.TM_Rank.ceo_approve + "" != "Y" ? (lstAD.HeadApprove_date.HasValue ? lstAD.HeadApprove_date.Value.DateTimebyCulture() : "") : (lstAD.CeoApprove_date.HasValue ? lstAD.CeoApprove_date.Value.DateTimebyCulture() : ""),
                                          pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                          pr_status_id = lstAD.TM_PR_Status.Id + "",
                                          pr_status_seq = lstAD.TM_PR_Status.seq + "",
                                          request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                          hc = lstAD.no_of_headcount + "",
                                          request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          IdEncrypt = ("RC" + lstAD.Id + "M"),

                                          no_select = lstAD.TM_PR_Candidate_Mapping != null ? lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y").OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Interview).Count() + "" : "0",
                                          no_accept = lstAD.TM_PR_Candidate_Mapping != null ? lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y").Select(s => s.TM_Candidate_Status.Id).Contains((int)StatusCandidate.OnBoard)
                                          && !w.TM_Candidate_Status_Cycle.Any(a => HCMClass.lstNotSelect().Contains(a.TM_Candidate_Status.Id) && a.active_status == "Y")).Count() + "" : "0",
                                          sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                      }).ToList();
                }
            }

            return View(result);
        }
        public ActionResult PRCandidateEdit(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPRCandidates_obj_Save result = new vPRCandidates_obj_Save();
            DateTime dNow = DateTime.Now;
            result.lstApprove = new List<vPRCandidates_lst_approve>();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _PersonnelRequestService.FindForAddCadidate(nId);
                    if (_getData != null)
                    {
                        string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
                        string sSession = "PrCandi" + unixTimestamps;
                        result.Session = sSession;
                        Session[sSession] = new File_Upload_PRCan();
                        result.IdEncrypt = qryStr;
                        List<string> lstUserApp = new List<string>();
                        lstUserApp.Add(_getData.request_user);
                        lstUserApp.Add(_getData.Req_BUApprove_user);
                        lstUserApp.Add(_getData.Req_HeadApprove_user);
                        lstUserApp.Add(_getData.Req_CeoApprove_user);
                        if (!string.IsNullOrEmpty(_getData.BUApprove_user))
                        {
                            lstUserApp.Add(_getData.BUApprove_user);
                        }
                        if (!string.IsNullOrEmpty(_getData.HeadApprove_user))
                        {
                            lstUserApp.Add(_getData.HeadApprove_user);
                        }
                        if (!string.IsNullOrEmpty(_getData.CeoApprove_user))
                        {
                            lstUserApp.Add(_getData.CeoApprove_user);
                        }



                        result.FY_plan = "0 person(s)";
                        if (_getData.request_date.Value.Month >= 10)
                        {
                            dNow = dNow.AddYears(1);
                        }
                        var _getPlan = _TM_FY_PlanService.FindbyYear(dNow.Year);
                        if (_getPlan != null && _getPlan.TM_FY_Detail != null)
                        {
                            var _getFYPlan = _getPlan.TM_FY_Detail.Where(w => w.TM_Divisions.division_code == _getData.TM_Divisions.division_code).FirstOrDefault();
                            if (_getFYPlan != null)
                            {
                                decimal? nSum = ((_getFYPlan.para.HasValue ? _getFYPlan.para.Value : 0) +
                        (_getFYPlan.aa.HasValue ? _getFYPlan.aa.Value : 0) +
                         (_getFYPlan.sr.HasValue ? _getFYPlan.sr.Value : 0) +
                          (_getFYPlan.am.HasValue ? _getFYPlan.am.Value : 0) +
                         (_getFYPlan.mgr.HasValue ? _getFYPlan.mgr.Value : 0) +
                         (_getFYPlan.ad.HasValue ? _getFYPlan.ad.Value : 0) +
                          (_getFYPlan.dir.HasValue ? _getFYPlan.dir.Value : 0) +
                         (_getFYPlan.ptr.HasValue ? _getFYPlan.ptr.Value : 0));
                                result.FY_plan = nSum.NodecimalFormat() + " person(s)";
                            }
                        }
                        result.FY_plan_title = "HC Plan FY " + dNow.ToString("yy") + " :";


                        int UnitID = SystemFunction.GetIntNullToZero(_getData.TM_Divisions.division_code);
                        result.cur_headcount = dbHr.AllInfo_WS.Where(w => w.UnitGroupID == UnitID && (w.Status == 1 || w.Status == 3)).Count() + " person(s)";
                        result.group_id = _getData.TM_Divisions.division_name_en;
                        var _getApproveStep = _TM_Step_ApproveService.GetDataForSelect();
                        if (_getApproveStep != null && _getApproveStep.Any())
                        {
                            string[] aUser = lstUserApp.ToArray();
                            var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                            result.lstApprove = _getApproveStep.Select(s => new vPRCandidates_lst_approve
                            {
                                nStep = s.step,
                                step_name = (s.step_name_en + "").Replace("(", "<br/>("),

                            }).ToList();
                            if (_getEmp.Any())
                            {
                                //Requested by
                                result.lstApprove.Where(w => w.nStep == 1).ToList().ForEach(ed =>
                                {
                                    ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.request_user).Select(s => s.EmpFullName).FirstOrDefault() + "";
                                    ed.approve_date = _getData.request_date.HasValue ? _getData.request_date.Value.DateTimebyCulture() : "";
                                });
                                //Requested by Group Head
                                result.lstApprove.Where(w => w.nStep == 2).ToList().ForEach(ed =>
                                {
                                    if (_getData.BUApprove_date.HasValue)
                                    {

                                        ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.Req_BUApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                        +
                                        ""
                                        +
                                        ((!string.IsNullOrEmpty(_getData.BUApprove_user) && _getData.BUApprove_user != _getData.Req_BUApprove_user) ?
                                        "<br/>(by "
                                        + _getEmp.Where(w => w.EmpNo == _getData.BUApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                        + ")"
                                        : "");
                                        ed.approve_date = _getData.BUApprove_date.Value.DateTimebyCulture();
                                        ed.description = _getData.BUApprove_remark + "";
                                    }
                                    else
                                    {
                                        ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.Req_BUApprove_user).Select(s => s.EmpFullName).FirstOrDefault() + "";
                                        ed.approve_date = "";
                                    }
                                });
                                //Approve by Functional HOP
                                result.lstApprove.Where(w => w.nStep == 3).ToList().ForEach(ed =>
                                {
                                    if (_getData.HeadApprove_date.HasValue)
                                    {

                                        ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.Req_HeadApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                        +
                                        ""
                                        +
                                        ((!string.IsNullOrEmpty(_getData.HeadApprove_user) && _getData.HeadApprove_user != _getData.Req_HeadApprove_user) ?
                                        "<br/>(by "
                                        + _getEmp.Where(w => w.EmpNo == _getData.HeadApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                        + ")"
                                        : "");
                                        ed.approve_date = _getData.HeadApprove_date.Value.DateTimebyCulture();
                                        ed.description = _getData.HeadApprove_remark + "";
                                    }
                                    else
                                    {
                                        ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.Req_HeadApprove_user).Select(s => s.EmpFullName).FirstOrDefault() + "";
                                        ed.approve_date = "";
                                    }
                                });
                                //ceo approve
                                if (_getData.need_ceo_approve + "" == "Y")
                                {
                                    result.lstApprove.Where(w => w.nStep == 4).ToList().ForEach(ed =>
                                    {
                                        if (_getData.CeoApprove_date.HasValue)
                                        {

                                            ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.Req_CeoApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                            +
                                            ""
                                            +
                                            ((!string.IsNullOrEmpty(_getData.CeoApprove_user) && _getData.CeoApprove_user != _getData.Req_CeoApprove_user) ?
                                            "<br/>(by "
                                            + _getEmp.Where(w => w.EmpNo == _getData.CeoApprove_user).Select(s => s.EmpFullName).FirstOrDefault()
                                            + ")"
                                            : "");
                                            ed.approve_date = _getData.CeoApprove_date.Value.DateTimebyCulture();
                                            ed.description = _getData.CeoApprove_remark + "";
                                        }
                                        else
                                        {
                                            ed.app_name = _getEmp.Where(w => w.EmpNo == _getData.Req_CeoApprove_user).Select(s => s.EmpFullName).FirstOrDefault() + "";
                                            ed.approve_date = "";
                                        }
                                    });
                                }
                                else
                                {
                                    result.lstApprove.RemoveAll(w => w.nStep == 4);
                                }
                            }
                        }
                        if (_getData.TM_SubGroup != null)
                        {
                            var GetHeadSG = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.TM_SubGroup.head_user_no).FirstOrDefault();
                            result.sub_group_id = _getData.TM_SubGroup.sub_group_name_en + "";
                            result.sub_group = "Y";
                            result.sub_group_head = GetHeadSG != null ? GetHeadSG.EmpFullName + "" : "-";
                        }
                        else
                        {
                            result.sub_group_id = "-";
                            result.sub_group = "N";
                            result.sub_group_head = "";
                        }
                        result.rank_id = _getData.TM_Pool_Rank != null ? _getData.TM_Pool_Rank.Pool_rank_name_en + "" : "-";
                        //result.sub_group_id = _getData.TM_SubGroup != null ? _getData.TM_SubGroup.sub_group_name_en + "" : "-";
                        result.position_id = _getData.TM_Position != null ? _getData.TM_Position.position_name_en + "" : "-";
                        result.employment_type_id = _getData.TM_Employment_Request.TM_Employment_Type != null ? _getData.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                        result.request_type_id = _getData.TM_Employment_Request.TM_Request_Type != null ? _getData.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";
                        result.target_start = _getData.target_period.HasValue ? _getData.target_period.Value.DateTimebyCulture() : "";
                        result.target_end = _getData.target_period_to.HasValue ? _getData.target_period_to.Value.DateTimebyCulture() : "";
                        result.no_of_head = _getData.no_of_headcount + "";
                        result.description = _getData.remark;
                        result.request_status = _getData.TM_PR_Status.Id + "";
                        result.status_name = _getData.TM_PR_Status.status_name_en + "";
                        result.hr_remark = _getData.hr_remark + "";
                        result.TIF_type = _getData.type_of_TIFForm + "";
                        result.no_of_eva = _getData.no_of_eva + "";
                        result.is_trainee = _getData.TM_Employment_Request.TM_Employment_Type.personnel_type + "";
                        result.code = _getData.RefNo + "";
                        result.pr_status_id = _getData.TM_PR_Status.Id + "";
                        result.job_descriptions = _getData.job_descriptions;
                        result.qualification_experience = _getData.qualification_experience;
                        if (_getData.replaced_status + "" == "Y" && !string.IsNullOrEmpty(_getData.user_replaced))
                        {
                            var _getReplaceuser = dbHr.AllInfo_WS.Where(w => w.EmpNo == _getData.user_replaced).FirstOrDefault();
                            if (_getReplaceuser != null)
                            {
                                result.user_no = _getData.user_replaced;
                                result.user_name = _getReplaceuser.EmpFullName;
                                result.unit_name = _getReplaceuser.UnitGroup;
                                result.user_position = _getReplaceuser.Rank;
                                result.replaced_user = _getData.replaced_status;
                            }
                        }
                        if (_getData.TM_PR_Candidate_Mapping != null && _getData.TM_PR_Candidate_Mapping.Any())
                        {
                            string[] User = _getData.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                            var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                            result.lstCandidates = (from lstAD in _getData.TM_PR_Candidate_Mapping
                                                    from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                    select new vPRCandidates_lstData
                                                    {
                                                        Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMapping('" + HCMFunc.Encrypt(lstAD.Id + "MPC") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                        candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                        candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                        owner_name = lstEmp.EmpFullName,
                                                        rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                        remark = lstAD.description + "",
                                                        action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                        active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                        active_status_id = lstAD.active_status + "",
                                                        Id = HCMFunc.Encrypt(lstAD.Id + "MPC"),
                                                        
                                                    }).ToList();
                        }
                    }
                    else
                    {
                        return RedirectToAction("PRCandidateList", "PRCandidate");
                    }
                }
                else
                {
                    return RedirectToAction("PRCandidateList", "PRCandidate");
                }
            }
            else
            {
                return RedirectToAction("PRCandidateList", "PRCandidate");
            }

            return View(result);
        }
        public ActionResult PRCandidateUpload(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission(true);
            if (sCheck != null)
            {
                return sCheck;
            }
            if (!CGlobal.UserIsAdmin())
            {
                return RedirectToAction("PRCandidateList", "PRCandidate");
            }
            vPRCandidates_obj_Save result = new vPRCandidates_obj_Save();
            DateTime dNow = DateTime.Now;
            string unixTimestamps = ((Int32)(DateTime.Now.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
            string sSession = "PrCandi" + unixTimestamps;
            result.Session = sSession;
            Session[sSession] = new File_Upload_PRCan();

            return View(result);
        }
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadPRCandidate(CSearchPRCandidates SearchItem)
        {
            bool isAdmin = CGlobal.UserIsAdmin();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            List<vPRCandidates_obj> lstData_resutl = new List<vPRCandidates_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            int nStatus = SystemFunction.GetIntNullToZero(SearchItem.pr_status + "");
            int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
            int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
            int nRank = SystemFunction.GetIntNullToZero(SearchItem.rank_id + "");

            var lstData = _PersonnelRequestService.GetPRListForAddCandidate(
        SearchItem.group_code, aDivisionPermission,
        "Y", nStatus, SearchItem.ref_no, nSubGroup, nPosition, nRank, SearchItem.name, isAdmin);
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

                string[] aRequestUser = lstData.Select(s => s.request_user).ToArray();
                string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser);
                string sUrl = Url.Action("PRCandidateEdit", "PRCandidate", null, Request.Url.Scheme);
                try
                {
                    result.lstData = (from lstAD in lstData
                                      from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())

                                      select new vPRCandidates_obj
                                      {
                                          //refno = lstAD.RefNo + "",
                                          //sgroup = lstAD.TM_Divisions.division_name_en + "",
                                          //position = lstAD.TM_Position.position_name_en + "",
                                          //request_type = lstAD.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "",
                                          //rank = lstAD.TM_Pool_Rank.TM_Rank.rank_name_en + "",
                                          //pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                          //request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                          //hc = lstAD.no_of_headcount + "",
                                          //request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimebyCulture() : "",
                                          //create_user = lstAD.create_user,
                                          //create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                          //update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          //update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          //sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                          //pr_status_id = lstAD.TM_PR_Status.Id + "",
                                          refno = @"<a target=""_blank""  href=""" + sUrl + "?qryStr=" + HCMFunc.Encrypt(lstAD.Id + "") + @""">" + lstAD.RefNo + @"</a>",
                                          sgroup = lstAD.TM_Divisions.division_name_en + "",
                                          position = lstAD.TM_Position.position_name_en + "",
                                          request_type = lstAD.TM_Employment_Request.TM_Request_Type.request_type_name_en + "",
                                          rank = lstAD.TM_Pool_Rank.Pool_rank_short_name_en + "",
                                          approve_date = lstAD.target_period.HasValue ? lstAD.target_period.Value.DateTimebyCulture() : "",
                                          //approve_date = lstAD.TM_Pool_Rank.TM_Rank.ceo_approve + "" != "Y" ? (lstAD.HeadApprove_date.HasValue ? lstAD.HeadApprove_date.Value.DateTimebyCulture() : "") : (lstAD.CeoApprove_date.HasValue ? lstAD.CeoApprove_date.Value.DateTimebyCulture() : ""),
                                          pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                          pr_status_id = lstAD.TM_PR_Status.Id + "",
                                          pr_status_seq = lstAD.TM_PR_Status.seq + "",
                                          request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                          hc = lstAD.no_of_headcount + "",
                                          request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          IdEncrypt = ("RC" + lstAD.Id + "M"),

                                          no_select = lstAD.TM_PR_Candidate_Mapping != null ? lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y").OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Interview).Count() + "" : "0",
                                          no_accept = lstAD.TM_PR_Candidate_Mapping != null ? lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y").Select(s => s.TM_Candidate_Status.Id).Contains((int)StatusCandidate.OnBoard)
                                          && !w.TM_Candidate_Status_Cycle.Any(a => HCMClass.lstNotSelect().Contains(a.TM_Candidate_Status.Id) && a.active_status == "Y")).Count() + "" : "0",

                                          sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                      }).ToList();
                    //result.lstData = lstData_resutl.ToList();
                }
                catch (Exception ex)
                {
                    string geterror = ex.Message;
                }
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult UpdatePRCandidate(vPRCandidates_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    int no_eva = 0;
                    var _getData = _PersonnelRequestService.FindForCancel(nId);
                    if (_getData != null)
                    {
                        //if (_getData.TM_Employment_Request.TM_Employment_Type.personnel_type + "" == "T")
                        //{
                        //    if (string.IsNullOrEmpty(ItemData.no_of_eva))
                        //    {
                        //        result.Status = SystemFunction.process_Failed;
                        //        result.Msg = "Error, Plaece input No. of evaluation";
                        //        return Json(new
                        //        {
                        //            result
                        //        });
                        //    }
                        //    else
                        //    {
                        //        no_eva = SystemFunction.GetIntNullToZero(ItemData.no_of_eva);
                        //        if (no_eva <= 0)
                        //        {
                        //            result.Status = SystemFunction.process_Failed;
                        //            result.Msg = "Error,  No. of evaluation more than 0.";
                        //            return Json(new
                        //            {
                        //                result
                        //            });
                        //        }
                        //        else
                        //        {
                        //       _getData.no_of_eva = no_eva;
                        //        }
                        //    }
                        //}

                        _getData.hr_remark = ItemData.hr_remark + "";
                        _getData.type_of_TIFForm = ItemData.TIF_type + "";
                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;

                        var sComplect = _PersonnelRequestService.UpdateApprove(_getData);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Plaece try again.";
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
        public ActionResult CompletePR(vPRCandidates_obj_Save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    int nComp = (int)StatusPR.Complete;
                    var _getData = _PersonnelRequestService.FindForCancel(nId);
                    if (_getData != null)
                    {

                        if (_getData.BUApprove_status == "Y" && _getData.HeadApprove_status == "Y" && (_getData.need_ceo_approve == "N" || _getData.CeoApprove_status == "Y"))
                        {


                            var GetCountCandidate = _getData.TM_PR_Candidate_Mapping.Where(w =>
                            w.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y").Select(s => s.TM_Candidate_Status.Id).Contains((int)StatusCandidate.OnBoard)
                            && !w.TM_Candidate_Status_Cycle.Any(a => HCMClass.lstNotSelect().Contains(a.TM_Candidate_Status.Id) && a.active_status == "Y")
                            && w.active_status == "Y"
                            ).Count();

                            if (GetCountCandidate != 0 && _getData.no_of_headcount >= GetCountCandidate)
                            {
                                var _getStatus = _TM_PR_StatusService.Find(nComp);
                                if (_getStatus != null)
                                {
                                    _getData.TM_PR_Status = _getStatus;
                                    _getData.update_user = CGlobal.UserInfo.UserId;
                                    _getData.update_date = dNow;

                                    var sComplect = _PersonnelRequestService.UpdateApprove(_getData);
                                    if (sComplect > 0)
                                    {
                                        result.Status = SystemFunction.process_Success;
                                    }
                                    else
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, Plaece try again.";
                                    }
                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, PR Status Not Found.";
                                }
                            }
                            else if (GetCountCandidate == 0)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Please ensure that there is a successful candidate for this PR (On-Board status) before clicking “Complete” this PR.";
                            }
                            else if (_getData.no_of_headcount < GetCountCandidate)
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, headcount < candidate.";
                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, Plaece try again.";
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = @"Error, You can change the PR status to ""Completed"" only when the PR approval process is completed.";
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
        public ActionResult RollbackStatusCandi(vCandidates_obj_update ItemData)
        {
            objCandidates_Return result = new objCandidates_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (ItemData != null && !string.IsNullOrEmpty(ItemData.IdEncrypt))
                {
                    int nID = SystemFunction.GetIntNullToZero((HCMFunc.Decrypt(ItemData.IdEncrypt + "")).Replace("MPC", ""));
                    int nStatusID = SystemFunction.GetIntNullToZero(ItemData.current_status_id);
                    var _getData = _TM_Candidate_Status_CycleService.FindForRollback(nID, nStatusID);
                    if (_getData != null && nStatusID != (int)StatusCandidate.AddNew)
                    {
                        int prID = _getData.TM_PR_Candidate_Mapping.PersonnelRequest.Id;

                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        _getData.active_status = "N";

                        var sComplect = _TM_Candidate_Status_CycleService.Update(_getData);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            var _getPR = _PersonnelRequestService.FindForAddCadidate(prID);
                            if (_getPR.TM_PR_Candidate_Mapping != null && _getPR.TM_PR_Candidate_Mapping.Any())
                            {
                                string[] User = _getPR.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                                var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                                result.lstCandidates = (from lstAD in _getPR.TM_PR_Candidate_Mapping
                                                        from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                        select new vPRCandidates_lstData
                                                        {
                                                            Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMapping('" + HCMFunc.Encrypt(lstAD.Id + "MPC") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                            candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                            candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                            owner_name = lstEmp.EmpFullName,
                                                            rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                            remark = lstAD.description + "",
                                                            action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                            active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                            active_status_id = lstAD.active_status + "",
                                                            Id = HCMFunc.Encrypt(lstAD.Id + "MPC"),
                                                        }).ToList();
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
                        result.Msg = "Error, please status type";
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, Data not found.";
                }
            }
            return Json(new
            {
                result
            });
        }
        [HttpPost]
        public ActionResult GetStatusLifeCycle(string SearchItem)
        {
            vPRCandidates_Detail_Return result = new vPRCandidates_Detail_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            List<vPRCandidates_Detail> lstData = new List<vPRCandidates_Detail>();
            int nID = SystemFunction.GetIntNullToZero((HCMFunc.Decrypt(SearchItem + "")).Replace("MPC", ""));
            var _getData = _TM_PR_Candidate_MappingService.Find(nID);
            if (_getData != null && _getData.TM_Candidate_Status_Cycle != null && _getData.TM_Candidate_Status_Cycle.Any(a => a.active_status + "" == "Y"))
            {
                result.Status = SystemFunction.process_Success;
                lstData = _getData.TM_Candidate_Status_Cycle.Where(w => w.active_status + "" == "Y").OrderBy(o => o.TM_Candidate_Status.seq).Select(s => new vPRCandidates_Detail
                {
                    action_name = s.TM_Candidate_Status.candidate_status_name_en,
                    action_date = s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "",
                    desc = s.description + "",
                }).ToList();
                result.lstData = lstData.ToList();
            }

            return Json(new { result });
        }
        #endregion

        #region Upload File
        public ActionResult AddCandidateByExcel()
        {
            vPRCan_file result = new vPRCan_file();
            if (CGlobal.UserInfo != null)
            {
                result.empNo = CGlobal.UserInfo.EmployeeNo + "";
            }
            return PartialView("_ExcelCandidate", result);
        }
        [HttpPost]
        public ActionResult UploadFile()
        {
            vPRCanReturn_UploadFile result = new vPRCanReturn_UploadFile();
            File_Upload_PRCan objFile = new File_Upload_PRCan();
            List<vPRCan_FileTemp> lstFile = new List<vPRCan_FileTemp>();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            string IdEncrypt = Request.Form["IdEncrypt"];
            if (!string.IsNullOrEmpty(IdEncrypt))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _PersonnelRequestService.FindForAddCadidate(nId);
                    if (_getData != null)
                    {
                        if (Request != null)
                        {
                            string sSess = Request.Form["sSess"];
                            objFile = Session[sSess] as File_Upload_PRCan;
                            if (objFile == null)
                            {
                                objFile = new File_Upload_PRCan();
                            }
                            if (Request.Files.Count > 0)
                            {

                                foreach (string file in Request.Files)
                                {
                                    var fileContent = Request.Files[file];
                                    if (fileContent != null && fileContent.ContentLength > 0)
                                    {
                                        // get a stream
                                        var stream = fileContent.InputStream;
                                        // and optionally write the file to disk
                                        var fileName = Path.GetFileName(file);
                                        try
                                        {
                                            using (var package = new ExcelPackage(stream))
                                            {
                                                var ws = package.Workbook.Worksheets.First();
                                                // objFile.sfile64 = package.GetAsByteArray();
                                                objFile.sfile_name = fileName;
                                                objFile.sfileType = Path.GetExtension(fileName).ToLower() + "";

                                                DataTable tbl = new DataTable();
                                                // List<string> lstHead = new List<string>();
                                                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                                                {
                                                    tbl.Columns.Add(true ? (firstRowCell.Text + "").Trim().ToLower() : string.Format("Column {0}", firstRowCell.Start.Column));
                                                    //lstHead.Add((firstRowCell.Text + "").Trim().ToLower());
                                                }
                                                var startRow = true ? 2 : 1;
                                                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                                {
                                                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                                                    DataRow row = tbl.Rows.Add();
                                                    foreach (var cell in wsRow)
                                                    {
                                                        row[cell.Start.Column - 1] = cell.Text;
                                                    }
                                                }
                                                if (tbl != null)
                                                {

                                                    if (tbl.Columns.Contains("NameEng") && tbl.Columns.Contains("SurnameEng"))
                                                    {
                                                        int i = 1;
                                                        //lstFile = tbl.AsEnumerable().Where(w => (w.Field<string>("NameEng") + "") != ""
                                                        //&& (w.Field<string>("SurnameEng") + "") != ""
                                                        //).Select(s =>
                                                        //new vPRCan_FileTemp
                                                        //{
                                                        //    sName = s.Field<string>("NameEng"),
                                                        //    sLName = s.Field<string>("SurnameEng"),
                                                        //    nCandidate_ID = _TM_CandidatesService.FindForUploadFile(s.Field<string>("NameEng"), s.Field<string>("SurnameEng")),

                                                        //}).ToList();

                                                        lstFile = (from row in tbl.AsEnumerable().Where(w => (w.Field<string>("NameEng") + "") != ""
                                                        && (w.Field<string>("SurnameEng") + "") != "")
                                                                   group row by new { name = row.Field<string>("NameEng"), lname = row.Field<string>("SurnameEng") } into grp
                                                                   select new vPRCan_FileTemp
                                                                   {
                                                                       sName = grp.Key.name,
                                                                       sLName = grp.Key.lname,
                                                                       nCandidate_ID = _TM_CandidatesService.FindForUploadFile(grp.Key.name, grp.Key.lname),

                                                                   }

                                                        ).ToList();

                                                        lstFile.ForEach(ed =>
                                                        {
                                                            var GetData = tbl.AsEnumerable().Where(w => (w.Field<string>("NameEng") + "") == ed.sName && (w.Field<string>("SurnameEng") + "") == ed.sLName).FirstOrDefault();
                                                            if (GetData != null)
                                                            {
                                                                if (tbl.Columns.Contains("Email"))
                                                                {
                                                                    ed.email = GetData.Field<string>("Email") + "";
                                                                }
                                                                if (tbl.Columns.Contains("Mobile"))
                                                                {
                                                                    ed.phone = GetData.Field<string>("Mobile") + "";
                                                                }
                                                                if (tbl.Columns.Contains("ID Card"))
                                                                {
                                                                    ed.id_card = GetData.Field<string>("ID Card") + "";
                                                                }
                                                                if (tbl.Columns.Contains("Nickname"))
                                                                {
                                                                    ed.snickname = GetData.Field<string>("Nickname") + "";
                                                                }
                                                                if (tbl.Columns.Contains("Activity"))
                                                                {
                                                                    ed.activity = GetData.Field<string>("Activity") + "";
                                                                }
                                                            }
                                                        });


                                                        if (_getData.TM_PR_Candidate_Mapping != null && _getData.TM_PR_Candidate_Mapping.Any())
                                                        {
                                                            int[] aID = _getData.TM_PR_Candidate_Mapping.Select(s => s.TM_Candidates.Id).ToArray();
                                                            lstFile = lstFile.Where(w => !aID.Contains(w.nCandidate_ID)).ToList();
                                                        }


                                                        if (lstFile.Any())
                                                        {
                                                            result.lstNewData = (from lFile in lstFile
                                                                                 select new vPRCan_obj
                                                                                 {
                                                                                     sName = lFile.sName,
                                                                                     sIsOld = lFile.nCandidate_ID == 0 ? "New" : "Ex Candidate",
                                                                                     sLName = lFile.sLName,
                                                                                     email = lFile.email + "",
                                                                                     phone = lFile.phone + "",
                                                                                     snickname = lFile.snickname + "",
                                                                                     activity  =lFile.activity + "",
                                                                                 }).ToList();
                                                            objFile.lstTempCandidate = lstFile;

                                                        }
                                                        result.Status = SystemFunction.process_Success;

                                                    }
                                                    else
                                                    {
                                                        result.Status = SystemFunction.process_Failed;
                                                        result.Msg = "Incorrect template, please try again.";
                                                    }

                                                }
                                            }
                                        }
                                        catch (Exception e)
                                        {

                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Incorrect template, please try again.";
                                        }
                                    }
                                }

                                Session[sSess] = objFile;


                            }
                            else
                            {
                                Session[sSess] = objFile;
                                result.Status = "Clear";
                            }
                        }
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Session!";
                }
            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Session!";
            }

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult AddExcelCandidate(vPRCan_file ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                string sSess = ItemData.Session;
                DateTime dNow = DateTime.Now;
                File_Upload_PRCan objFile = new File_Upload_PRCan();
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    int no_eva = 0;
                    var _getData = _PersonnelRequestService.FindForAddCadidate(nId);
                    if (_getData != null)
                    {
                        var _GetRecruit = _TM_Recruitment_TeamService.FindByEmpID(ItemData.empNo);
                        if (_GetRecruit == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Recruitment Team not found.";
                            return Json(new
                            {
                                result
                            });
                        }
                       
                        objFile = Session[sSess] as File_Upload_PRCan;
                        if (objFile != null)
                        {
                            if (objFile.lstTempCandidate != null && objFile.lstTempCandidate.Any())
                            {
                                foreach (var item in objFile.lstTempCandidate)
                                {
                                    var _GetActivity = _TM_PInternAssessment_ActivitiesService.FindAddUploadFile(item.activity);
                                    var _GetCandidate = _TM_CandidatesService.FindAddUploadFile(item.nCandidate_ID);
                                    if (_GetCandidate != null)
                                    {
                                        var _getStatus = _TM_Candidate_StatusService.Find((int)StatusCandidate.AddNew);
                                        // กรณี All new Candidate


                                        TM_Candidate_Status_Cycle objCycle = new TM_Candidate_Status_Cycle()
                                        {

                                            seq = 1,
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            active_status = "Y",
                                            TM_Candidate_Status = _getStatus,
                                            action_date = dNow,
                                        };

                                        TM_PR_Candidate_Mapping objMapping = new TM_PR_Candidate_Mapping()
                                        {

                                            ntime = 1,
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            active_status = "Y",
                                            PersonnelRequest = _getData,
                                            TM_Candidate_Status_Cycle = new List<TM_Candidate_Status_Cycle>(),
                                            TM_Recruitment_Team = _GetRecruit,
                                            TM_Candidates = _GetCandidate,
                                            TM_PInternAssessment_Activities = _GetActivity ,

                                        };
                                        objMapping.TM_Candidate_Status_Cycle.Add(objCycle);
                                        if (_TM_PR_Candidate_MappingService.CanCreateMapping(objMapping))
                                        {
                                            var sComplect = _TM_PR_Candidate_MappingService.CreateNew(objMapping);
                                        }
                                    }
                                    else
                                    {
                                        var _getStatus = _TM_Candidate_StatusService.Find((int)StatusCandidate.AddNew);
                                        // กรณี All new Candidate

                                        TM_Candidate_Status_Cycle objCycle = new TM_Candidate_Status_Cycle()
                                        {
                                            Id = 0,
                                            seq = 1,
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            active_status = "Y",
                                            TM_Candidate_Status = _getStatus,
                                            action_date = dNow,
                                        };

                                        TM_PR_Candidate_Mapping objMapping = new TM_PR_Candidate_Mapping()
                                        {
                                            Id = 0,
                                            ntime = 1,
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            active_status = "Y",
                                            PersonnelRequest = _getData,
                                            TM_Candidate_Status_Cycle = new List<TM_Candidate_Status_Cycle>(),
                                            TM_Recruitment_Team = _GetRecruit,
                                            TM_PInternAssessment_Activities = _GetActivity,
                                        };
                                        objMapping.TM_Candidate_Status_Cycle.Add(objCycle);

                                        TM_Candidates objSave = new TM_Candidates()
                                        {
                                            Id = 0,
                                            update_user = CGlobal.UserInfo.UserId,
                                            update_date = dNow,
                                            create_user = CGlobal.UserInfo.UserId,
                                            create_date = dNow,
                                            active_status = "Y",
                                            first_name_en = (item.sName + "").Trim(),
                                            last_name_en = (item.sLName + "").Trim(),
                                            save_success = "N",
                                            TM_PR_Candidate_Mapping = new List<TM_PR_Candidate_Mapping>(),
                                            candidate_Email = item.email + "",
                                            candidate_phone = item.phone + "",
                                            candidate_NickName = item.snickname + "",
                                            id_card = item.id_card + "",
                                        };
                                        objSave.TM_PR_Candidate_Mapping.Add(objMapping);
                                        //add new candidate

                                        //map pr candidate first status = 1


                                        if (_TM_CandidatesService.CanSave(objSave))
                                        {
                                            var sComplect = _TM_CandidatesService.CreateNew(objSave);
                                            if (sComplect > 0)
                                            {

                                            }
                                        }
                                    }
                                }
                                result.Status = SystemFunction.process_Success;
                                var _getDataPR = _PersonnelRequestService.FindForAddCadidate(nId);
                                if (_getDataPR.TM_PR_Candidate_Mapping != null && _getDataPR.TM_PR_Candidate_Mapping.Any())
                                {

                                    string[] User = _getDataPR.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                                    var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                                    result.lstCandidates = (from lstAD in _getDataPR.TM_PR_Candidate_Mapping
                                                            from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                            select new vPRCandidates_lstData
                                                            {
                                                                Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMapping('" + HCMFunc.Encrypt(lstAD.Id + "MPC") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                                candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                                candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                                owner_name = lstEmp.EmpFullName,
                                                                rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                                remark = lstAD.description + "",
                                                                action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                                active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                                active_status_id = lstAD.active_status + "",
                                                                Id = HCMFunc.Encrypt(lstAD.Id + "MPC"),
                                                                activity = lstAD.TM_PInternAssessment_Activities != null ? lstAD.TM_PInternAssessment_Activities.Activities_name_en + "":"",
                                                            }).ToList();
                                }

                            }
                            else
                            {
                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Error, File Not Found.";
                            }


                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, File Not Found.";
                        }



                        //var sComplect = _PersonnelRequestService.UpdateApprove(_getData);
                        //if (sComplect > 0)
                        //{
                        //    result.Status = SystemFunction.process_Success;
                        //}
                        //else
                        //{
                        //    result.Status = SystemFunction.process_Failed;
                        //    result.Msg = "Error, Plaece try again.";
                        //}
                        //result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, PR Not Found.";
                    }
                }

            }

            return Json(new
            {
                result
            });
        }
        #endregion
        #region update many status
        public ActionResult UpdateManyStatus()
        {
            vPRCan_many result = new vPRCan_many();
            if (CGlobal.UserInfo != null)
            {
                result.recruitment_id = CGlobal.UserInfo.EmployeeNo + "";
            }
            result.active_status = "Y";
            result.lstStatus = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

            return PartialView("_UpdateManyStatus", result);
        }
        [HttpPost]
        public ActionResult LoadPRCandidateMany(SearchManyPR SearchItem)
        {
            bool isAdmin = CGlobal.UserIsAdmin();
            vPRmany_Return result = new vPRmany_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }

            if (SearchItem != null)
            {
                DateTime dNow = DateTime.Now;
                File_Upload_PRCan objFile = new File_Upload_PRCan();
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(SearchItem.IdEncrypt + ""));
                int nId_Status = SystemFunction.GetIntNullToZero(SearchItem.status_id + "");
                if (nId != 0 && nId_Status != 0)
                {
                    result.lstStatus = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                    var _GetLastStatus = _TM_Candidate_StatusService.Find(nId_Status);

                    int[] aStatus = (_GetLastStatus.TM_Candidate_Status_Next != null && _GetLastStatus.TM_Candidate_Status_Next.Any()) ?
                                _GetLastStatus.TM_Candidate_Status_Next.Select(s => s.next_status_id).ToArray() : null;
                    var _GetNextStatus = _TM_Candidate_StatusService.GetStatusForSave(aStatus);
                    if (_GetNextStatus != null && _GetNextStatus.Any())
                    {
                        result.lstStatus.AddRange(_GetNextStatus.OrderBy(o => o.seq).Select(s => new vSelect_PR { id = s.Id + "", name = s.candidate_status_name_en }).ToList());
                    }



                    int no_eva = 0;
                    var _getData = _PersonnelRequestService.FindForAddCadidate(nId);
                    if (_getData != null)
                    {
                        if (_getData.TM_PR_Candidate_Mapping != null && _getData.TM_PR_Candidate_Mapping.Any(a => a.active_status == "Y"))
                        {
                            string[] User = _getData.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                            var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                            result.lstCandidates = (from lstAD in _getData.TM_PR_Candidate_Mapping.Where(a => a.active_status == "Y" && a.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.Id).First() == nId_Status)
                                                    from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                    select new vPRmany_lstData
                                                    {
                                                        Id = lstAD.Id + "",
                                                        candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                        candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                        owner_name = lstEmp.EmpFullName,
                                                        rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                        remark = lstAD.description + "",
                                                        action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                        active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                        active_status_id = lstAD.active_status + "",
                                                    }).ToList();
                        }
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, PR Not Found.";
                    }
                }

            }

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult SaveManyCandidate(vPRCan_many ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (ItemData != null && ItemData.lstCandidates != null && ItemData.lstCandidates.Any())
            {
                if (CGlobal.IsUserExpired())
                {
                    result.Status = SystemFunction.process_SessionExpired;
                    return Json(new
                    {
                        result
                    });
                }
                int nPRId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                var _getDataPR = _PersonnelRequestService.FindForAddCadidate(nPRId);
                if (_getDataPR != null)
                {
                    DateTime dNow = DateTime.Now;
                    var _GetRecruit = _TM_Recruitment_TeamService.FindByEmpID(ItemData.recruitment_id);
                    if (_GetRecruit == null)
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, Recruitment Team not found.";
                        return Json(new
                        {
                            result
                        });
                    }

                    DateTime? action_date = null;
                    if (!string.IsNullOrEmpty(ItemData.action_date))
                    {
                        action_date = SystemFunction.ConvertStringToDateTime(ItemData.action_date, "");
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, please enter action date";
                        return Json(new
                        {
                            result
                        });
                    }


                    int nStatusID = SystemFunction.GetIntNullToZero(ItemData.status_id);
                    var _getStatus = _TM_Candidate_StatusService.Find(nStatusID);

                    if (_getStatus != null)
                    {
                        if (_getStatus.remark_validate + "" == "Y" && string.IsNullOrEmpty((ItemData.action_remark + "").Trim()))
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter remark status.";
                            return Json(new
                            {
                                result
                            });
                        }
                        foreach (var item in ItemData.lstCandidates)
                        {
                            int nId = SystemFunction.GetIntNullToZero(item.Id + "");
                            if (nId != 0)
                            {
                                var _getData = _TM_PR_Candidate_MappingService.Find(nId);
                                if (_getData != null)
                                {
                                    if (!string.IsNullOrEmpty(ItemData.candidate_rank_id))
                                    {
                                        int nRID = SystemFunction.GetIntNullToZero(ItemData.candidate_rank_id);
                                        var _GetRank = _TM_Candidate_RankService.FindForSelect(nRID);
                                        if (_GetRank != null)
                                        {
                                            _getData.TM_Candidate_Rank = _GetRank;
                                        }

                                    }
                                    var _getLastStatus = _getData.TM_Candidate_Status_Cycle.OrderByDescending(o => o.seq).FirstOrDefault();
                                    _getData.update_user = CGlobal.UserInfo.UserId;
                                    _getData.update_date = dNow;
                                    _getData.PersonnelRequest = _getDataPR;
                                    if (_getLastStatus.TM_Candidate_Status.Id != _getStatus.Id)
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
                                            action_date = action_date,
                                            description = ItemData.action_remark,
                                            TM_PR_Candidate_Mapping = _getData,
                                        };

                                        var sComplect = _TM_PR_Candidate_MappingService.Update(_getData);
                                        if (sComplect > 0)
                                        {
                                            result.Status = SystemFunction.process_Success;
                                            sComplect = _TM_Candidate_Status_CycleService.CreateNew(objCycle);
                                            if (sComplect > 0)
                                            {

                                            }

                                        }
                                    }
                                }

                            }
                        }
                        result.Status = SystemFunction.process_Success;
                        var _getDataPRlast = _PersonnelRequestService.FindForAddCadidate(nPRId);
                        if (_getDataPRlast.TM_PR_Candidate_Mapping != null && _getDataPRlast.TM_PR_Candidate_Mapping.Any())
                        {

                            string[] User = _getDataPRlast.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                            var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                            result.lstCandidates = (from lstAD in _getDataPRlast.TM_PR_Candidate_Mapping
                                                    from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                    select new vPRCandidates_lstData
                                                    {
                                                        Edit = @"<button id=""btnEdit""  type=""button"" onclick=""EditMapping('" + HCMFunc.Encrypt(lstAD.Id + "MPC") + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                                        candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                        candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                        owner_name = lstEmp.EmpFullName,
                                                        rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                        remark = lstAD.description + "",
                                                        action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                        active_status = lstAD.active_status + "" == "Y" ? "Active" : "Inactive",
                                                        active_status_id = lstAD.active_status + "",
                                                        Id = HCMFunc.Encrypt(lstAD.Id + "MPC"),
                                                    }).ToList();
                        }
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, PR Not Found.";
                }

            }
            else
            {
                result.Status = SystemFunction.process_Failed;
                result.Msg = "Error, Pleace select candidate.";
            }

            return Json(new
            {
                result
            });
        }

        #endregion

        #region byPass UploadFile
        [HttpPost]
        public ActionResult UploadFileBypass()
        {
            vPRCanReturn_UploadFile result = new vPRCanReturn_UploadFile();
            File_Upload_PRCan objFile = new File_Upload_PRCan();
            List<vPRCan_FileTemp> lstFile = new List<vPRCan_FileTemp>();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            string IdEncrypt = Request.Form["IdEncrypt"];
            if (Request != null)
            {
                string sSess = Request.Form["sSess"];
                objFile = Session[sSess] as File_Upload_PRCan;
                if (objFile == null)
                {
                    objFile = new File_Upload_PRCan();
                }
                if (Request.Files.Count > 0)
                {

                    foreach (string file in Request.Files)
                    {
                        var fileContent = Request.Files[file];
                        if (fileContent != null && fileContent.ContentLength > 0)
                        {
                            // get a stream
                            var stream = fileContent.InputStream;
                            // and optionally write the file to disk
                            var fileName = Path.GetFileName(file);
                            try
                            {
                                using (var package = new ExcelPackage(stream))
                                {
                                    var ws = package.Workbook.Worksheets.First();
                                    // objFile.sfile64 = package.GetAsByteArray();
                                    objFile.sfile_name = fileName;
                                    objFile.sfileType = Path.GetExtension(fileName).ToLower() + "";



                                    DataTable tbl = new DataTable();
                                    // List<string> lstHead = new List<string>();
                                    foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                                    {
                                        tbl.Columns.Add(true ? (firstRowCell.Text + "").Trim().ToLower() : string.Format("Column {0}", firstRowCell.Start.Column));
                                        //lstHead.Add((firstRowCell.Text + "").Trim().ToLower());
                                    }
                                    var startRow = true ? 2 : 1;
                                    for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                                    {
                                        var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                                        DataRow row = tbl.Rows.Add();
                                        foreach (var cell in wsRow)
                                        {
                                            row[cell.Start.Column - 1] = cell.Text;
                                        }
                                    }
                                    if (tbl != null)
                                    {

                                        if (tbl.Columns.Contains("NameEng") && tbl.Columns.Contains("SurnameEng") && tbl.Columns.Contains("IDCard"))
                                        {
                                            int i = 1;
                                            //lstFile = tbl.AsEnumerable().Where(w => (w.Field<string>("NameEng") + "") != ""
                                            //&& (w.Field<string>("SurnameEng") + "") != ""
                                            //).Select(s =>
                                            //new vPRCan_FileTemp
                                            //{
                                            //    sName = s.Field<string>("NameEng"),
                                            //    sLName = s.Field<string>("SurnameEng"),
                                            //    nCandidate_ID = _TM_CandidatesService.FindForUploadFile(s.Field<string>("NameEng"), s.Field<string>("SurnameEng")),

                                            //}).ToList();

                                            lstFile = (from row in tbl.AsEnumerable().Where(w => (w.Field<string>("NameEng") + "") != ""
                                            && (w.Field<string>("SurnameEng") + "") != "")
                                                       group row by new { name = row.Field<string>("NameEng"), lname = row.Field<string>("SurnameEng"), idcard = row.Field<string>("IDCard") } into grp
                                                       select new vPRCan_FileTemp
                                                       {
                                                           sName = grp.Key.name,
                                                           sLName = grp.Key.lname,
                                                           nCandidate_ID = _TM_CandidatesService.FindForUploadFile(grp.Key.name, grp.Key.lname),
                                                           id_card = grp.Key.idcard,

                                                       }

                                            ).ToList();


                                            if (lstFile.Any())
                                            {
                                                result.lstNewData = (from lFile in lstFile
                                                                     select new vPRCan_obj
                                                                     {
                                                                         sName = lFile.sName,
                                                                         sIsOld = lFile.nCandidate_ID == 0 ? "New" : "Ex Candidate",
                                                                         sLName = lFile.sLName,
                                                                         username = ((lFile.sName).Replace(" ", "")).ToLower() + "." + ((lFile.sLName).Replace(" ", "")).ToLower()
                                                                     }).ToList();
                                                objFile.lstTempCandidate = lstFile;

                                            }
                                            result.Status = SystemFunction.process_Success;

                                        }
                                        else
                                        {
                                            result.Status = SystemFunction.process_Failed;
                                            result.Msg = "Incorrect template, please try again.";
                                        }

                                    }
                                }
                            }
                            catch (Exception e)
                            {

                                result.Status = SystemFunction.process_Failed;
                                result.Msg = "Incorrect template, please try again.";
                            }
                        }
                    }

                    Session[sSess] = objFile;


                }
                else
                {
                    Session[sSess] = objFile;
                    result.Status = "Clear";
                }
            }

            return Json(new { result });
        }

        [HttpPost]
        public ActionResult UpdateExcelCandidate(vPRCan_file ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vPRCandidates_Return result = new vPRCandidates_Return();
            if (CGlobal.IsUserExpired())
            {
                result.Status = SystemFunction.process_SessionExpired;
                return Json(new { result });
            }
            if (ItemData != null)
            {
                string sSess = ItemData.Session;
                DateTime dNow = DateTime.Now;
                File_Upload_PRCan objFile = new File_Upload_PRCan();
                objFile = Session[sSess] as File_Upload_PRCan;
                if (objFile != null)
                {
                    if (objFile.lstTempCandidate != null && objFile.lstTempCandidate.Any())
                    {
                        foreach (var item in objFile.lstTempCandidate.Where(w => w.nCandidate_ID != 0))
                        {
                            var _getCandidate = _TM_CandidatesService.FindAddUploadFile(item.nCandidate_ID);
                            if (_getCandidate != null)
                            {
                                _getCandidate.candidate_user_id = ((item.sName).Replace(" ", "")).ToLower() + "." + ((item.sLName).Replace(" ", "")).ToLower();
                                _getCandidate.id_card = item.id_card;
                                _getCandidate.update_date = dNow;
                                _getCandidate.update_user = CGlobal.UserInfo.UserId;
                                string fPass = (item.sName + "").Length >= 3 ? (item.sName.Substring(0, 3)).ToLower() : (item.sName).ToLower();
                                string sPass = (item.id_card + "").Length >= 3 ? (item.id_card + "").Substring(0, 3) : (item.id_card + "");
                                int maxleang = (item.id_card + "").Length;
                                string tPass = (item.id_card + "").Length >= 3 ? (item.id_card + "").Substring(maxleang - 3) : (item.id_card + "");

                                _getCandidate.candidate_password = SHA.GenerateSHA256String(fPass + (sPass.ToLower()) + (tPass.ToLower()));

                                var sComplect = _TM_CandidatesService.Update(_getCandidate);
                            }
                        }
                        result.Status = SystemFunction.process_Success;
                    }
                    else
                    {
                        result.Status = SystemFunction.process_Failed;
                        result.Msg = "Error, File Not Found.";
                    }


                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, File Not Found.";
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