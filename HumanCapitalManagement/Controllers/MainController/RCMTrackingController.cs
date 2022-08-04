using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static HumanCapitalManagement.App_Start.HCMClass;

namespace HumanCapitalManagement.Controllers.MainController
{
    public class RCMTrackingController : BaseController
    {
        private DivisionService _DivisionService;
        private TM_UnitGroup_Approve_PermitService _TM_UnitGroup_Approve_PermitService;
        private PoolService _PoolService;
        private EmploymentTypeService _EmploymentTypeService;
        private TM_PositionService _TM_PositionService;
        private RequestTypeService _RequestTypeService;
        private TM_Step_ApproveService _TM_Step_ApproveService;
        private TM_Employment_RequestService _TM_Employment_RequestService;
        private TM_PR_StatusService _TM_PR_StatusService;
        private PersonnelRequestService _PersonnelRequestService;
        private MailContentService _MailContentService;
        private E_Mail_HistoryService _E_Mail_HistoryService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public RCMTrackingController(DivisionService DivisionService, PoolService PoolService, TM_UnitGroup_Approve_PermitService TM_UnitGroup_Approve_PermitService
     , EmploymentTypeService EmploymentTypeService
     , TM_PositionService TM_PositionService
     , RequestTypeService RequestTypeService
     , TM_Step_ApproveService TM_Step_ApproveService
     , TM_Employment_RequestService TM_Employment_RequestService
     , TM_PR_StatusService TM_PR_StatusService
     , PersonnelRequestService PersonnelRequestService
     , MailContentService MailContentService
     , E_Mail_HistoryService E_Mail_HistoryService)
        {
            _DivisionService = DivisionService;
            _PoolService = PoolService;
            _TM_UnitGroup_Approve_PermitService = TM_UnitGroup_Approve_PermitService;
            _EmploymentTypeService = EmploymentTypeService;
            _TM_PositionService = TM_PositionService;
            _RequestTypeService = RequestTypeService;
            _TM_Step_ApproveService = TM_Step_ApproveService;
            _TM_Employment_RequestService = TM_Employment_RequestService;
            _TM_PR_StatusService = TM_PR_StatusService;
            _PersonnelRequestService = PersonnelRequestService;
            _MailContentService = MailContentService;
            _E_Mail_HistoryService = E_Mail_HistoryService;
        }
        // GET: RCMTracking
        public ActionResult RCMTrackingList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            vRCMTracking result = new vRCMTracking();
            result.active_status = "Y";
            result.lstSubGroup = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPosition = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.sub_group_id = "";
            result.position_id = "";
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchRCMTracking SearchItem = (CSearchRCMTracking)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchRCMTracking)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                int nStatus = SystemFunction.GetIntNullToZero(SearchItem.pr_status + "");
                int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
                int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
                var lstData = _PersonnelRequestService.GetPersonnelRqListData(
                 SearchItem.group_code, aDivisionPermission,
                 "Y", nStatus, SearchItem.ref_no, nSubGroup, nPosition, isAdmin);
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

                result.group_code = SearchItem.group_code;
                result.pr_status = SearchItem.pr_status;
                result.ref_no = SearchItem.ref_no;
                result.sub_group_id = SearchItem.sub_group_id;
                result.position_id = SearchItem.position_id;

                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
                 
                    string[] aRequestUser = lstData.Select(s => s.request_user).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    result.lstData = (from lstAD in lstData
                                      from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vRCMTracking_obj
                                      {
                                          refno = lstAD.RefNo + "",
                                          sgroup = lstAD.TM_Divisions.division_name_en + "",
                                          position = lstAD.TM_Position.position_name_en + "",
                                          request_type = lstAD.TM_Employment_Request.TM_Request_Type.request_type_name_en + "",
                                          rank = lstAD.TM_Pool_Rank.Pool_rank_short_name_en + "",
                                          approve_date = lstAD.target_period.HasValue ? lstAD.target_period.Value.DateTimebyCulture() : "",
                                         // approve_date = lstAD.TM_Pool_Rank.TM_Rank.ceo_approve + "" != "Y" ? (lstAD.HeadApprove_date.HasValue ? lstAD.HeadApprove_date.Value.DateTimebyCulture() : "") : (lstAD.CeoApprove_date.HasValue ? lstAD.CeoApprove_date.Value.DateTimebyCulture() : ""),
                                          pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                          pr_status_id = lstAD.TM_PR_Status.Id + "",
                                          pr_status_seq = lstAD.TM_PR_Status.seq + "",
                                          request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                          hc = lstAD.no_of_headcount + "",
                                          request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          IdEncrypt = ("RC" + lstAD.Id + "M"),
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + ("RC" + lstAD.Id + "M") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary"" tooltip=""View"" flow=""right""><i class=""glyphicon glyphicon-search""></i></button>",
                                          no_select = lstAD.TM_PR_Candidate_Mapping != null ? lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y").OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Interview).Count() + "" : "0",
                                          no_accept = lstAD.TM_PR_Candidate_Mapping != null ? lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y").Select(s => s.TM_Candidate_Status.Id).Contains((int)StatusCandidate.OnBoard)
                                          && !w.TM_Candidate_Status_Cycle.Any(a => HCMClass.lstNotSelect().Contains(a.TM_Candidate_Status.Id) && a.active_status == "Y")).Count() + "" : "0",
                                          sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                      }).ToList();
                }
            }

            return View(result);
        }
        public ActionResult RCMTrackingView(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vRCMTracking_obj_view result = new vRCMTracking_obj_view();
            result.lstApprove = new List<vRCMTracking_lst_approve>();
            if (!string.IsNullOrEmpty(qryStr))
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(((qryStr + "").Replace("RC", "")).Replace("M", ""));
                if (nId != 0)
                {
                    var _getData = _PersonnelRequestService.Find(nId);
                    if (_getData != null)
                    {
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
                        result.FY_plan_title = "HC Plan FY " + dNow.ToString("yy") + " :";
                        result.FY_plan = "0 person(s)";
                        int UnitID = SystemFunction.GetIntNullToZero(_getData.TM_Divisions.division_code);
                        result.cur_headcount = dbHr.AllInfo_WS.Where(w => w.UnitGroupID == UnitID && (w.Status == 1 || w.Status == 3)).Count() + " person(s)";
                        result.group_id = _getData.TM_Divisions.division_name_en;
                        var _getApproveStep = _TM_Step_ApproveService.GetDataForSelect();
                        if (_getApproveStep != null && _getApproveStep.Any())
                        {
                            string[] aUser = lstUserApp.ToArray();
                            var _getEmp = dbHr.AllInfo_WS.Where(w => aUser.Contains(w.EmpNo)).ToList();
                            result.lstApprove = _getApproveStep.Select(s => new vRCMTracking_lst_approve
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

                        result.position_id = _getData.TM_Position != null ? _getData.TM_Position.position_name_en + "" : "-";
                        result.employment_type_id = _getData.TM_Employment_Request.TM_Employment_Type != null ? _getData.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                        result.request_type_id = _getData.TM_Employment_Request.TM_Request_Type != null ? _getData.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";
                        result.target_start = _getData.target_period.HasValue ? _getData.target_period.Value.DateTimebyCulture() : "";
                        result.target_end = _getData.target_period_to.HasValue ? _getData.target_period_to.Value.DateTimebyCulture() : "";
                        result.no_of_head = _getData.no_of_headcount + "";
                        result.job_descriptions = _getData.job_descriptions;
                        result.qualification_experience = _getData.qualification_experience;
                        result.description = _getData.remark;
                        result.request_status = _getData.TM_PR_Status.Id + "";
                        result.status_name = _getData.TM_PR_Status.status_name_en + "";
                        result.cancel_remark = _getData.cancel_reason + "";
                        result.reject_remark = _getData.reject_reason + "";
                        result.code = _getData.RefNo + "";
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
                        if (_getData.TM_PR_Candidate_Mapping != null && _getData.TM_PR_Candidate_Mapping.Any(w => w.active_status == "Y"))
                        {
                            string[] User = _getData.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                            var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                            result.lstCandidates = (from lstAD in _getData.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y"
                                                    //w.TM_Candidate_Status_Cycle.OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Interview ||
                                                    //w.TM_Candidate_Status_Cycle.OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Accepted
                                                    )
                                                    from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                    select new vRCMTracking_lstData
                                                    {
                                                        candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                        candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                        owner_name = lstEmp.EmpFullName,
                                                        rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                        remark = lstAD.description + "",
                                                        action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                    }).ToList();
                        }
                    }
                }
            }
            else
            {
                return RedirectToAction("RCMTrackingList", "RCMTracking");
            }

            return View(result);
        }
        public ActionResult DetailRCM(vRCMTracking_popup_detail ItemData)
        {
            vRCMTracking_popup_detail result = new vRCMTracking_popup_detail();
            if (ItemData != null && !string.IsNullOrEmpty(ItemData.IdEncrypt))
            {
                int nID = SystemFunction.GetIntNullToZero(((ItemData.IdEncrypt + "").Replace("RC", "")).Replace("M", ""));
                if (nID != 0)
                {
                    List<vRCMTracking_lstData> lstData = new List<vRCMTracking_lstData>();
                    var _getData = _PersonnelRequestService.Find(nID);
                    if (_getData != null)
                    {
                        if (_getData.TM_PR_Candidate_Mapping != null && _getData.TM_PR_Candidate_Mapping.Any(w => w.active_status == "Y"))
                        {
                            string[] User = _getData.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                            var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                            if (ItemData.sMode == "S")
                            {
                                result.lstCandidates = (from lstAD in _getData.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y"
                                                        && w.TM_Candidate_Status_Cycle.OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Interview)
                                                        from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                        select new vRCMTracking_lstData
                                                        {
                                                            candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                            candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                            owner_name = lstEmp.EmpFullName,
                                                            rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                            remark = lstAD.description + "",
                                                            action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                        }).ToList();
                            }
                            else if (ItemData.sMode == "A")
                            {
                                List<int> lstNotSelect = new List<int>();
                                lstNotSelect.Add((int)StatusCandidate.Turndown);
                                lstNotSelect.Add((int)StatusCandidate.NoShow);
                                result.lstCandidates = (from lstAD in _getData.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y"
                                                        && w.TM_Candidate_Status_Cycle.Select(s => s.TM_Candidate_Status.Id).Contains((int)StatusCandidate.Accepted)
                                                        && !w.TM_Candidate_Status_Cycle.Any(a => lstNotSelect.Contains(a.TM_Candidate_Status.Id)))
                                                        from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                        select new vRCMTracking_lstData
                                                        {
                                                            candidate_name = lstAD.TM_Candidates.first_name_en + " " + lstAD.TM_Candidates.last_name_en,
                                                            candidate_status = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => s.TM_Candidate_Status.candidate_status_name_en).First(),
                                                            owner_name = lstEmp.EmpFullName,
                                                            rank = lstAD.PersonnelRequest.TM_Pool_Rank.Pool_rank_name_en,
                                                            remark = lstAD.description + "",
                                                            action_date = lstAD.TM_Candidate_Status_Cycle.Where(w => w.active_status == "Y").OrderByDescending(o => o.seq).Select(s => (s.action_date.HasValue ? s.action_date.Value.DateTimebyCulture() : "")).First(),
                                                        }).ToList();
                            }
                            else
                            {
                                result.lstCandidates = new List<vRCMTracking_lstData>();
                            }

                        }
                    }

                }
                else
                {
                    result.IdEncrypt = "";
                }

            }
            else
            {
                result.IdEncrypt = "";
            }
            return PartialView("_RCMTrackingDetail", result);
        }
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadRCMTrackingList(CSearchRCMTracking SearchItem)
        {
            bool isAdmin = CGlobal.UserIsAdmin();
            vRCMTracking_Return result = new vRCMTracking_Return();
            List<vRCMTracking_obj> lstData_resutl = new List<vRCMTracking_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            int nStatus = SystemFunction.GetIntNullToZero(SearchItem.pr_status + "");

            int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
            int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
            var lstData = _PersonnelRequestService.GetPersonnelRqListData(
            SearchItem.group_code, aDivisionPermission,
            "Y", nStatus, SearchItem.ref_no, nSubGroup, nPosition, isAdmin);
            string qryStr = JsonConvert.SerializeObject(SearchItem,
            Formatting.Indented,
            new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore,
                DefaultValueHandling = DefaultValueHandling.Ignore,
            });
            //int[] aNotSelect = Enum.GetValues(typeof(StatusCandidate)).Cast<StatusCandidate>().Select(v => (int)v).ToArray();
        
            string BackUrl = Uri.EscapeDataString(qryStr);
            if (lstData.Any())
            {
                string[] aRequestUser = lstData.Select(s => s.request_user).ToArray();
                string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser);


                lstData_resutl = (from lstAD in lstData
                                  from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  select new vRCMTracking_obj
                                  {
                                      refno = lstAD.RefNo + "",
                                      sgroup = lstAD.TM_Divisions.division_name_en + "",
                                      position = lstAD.TM_Position.position_name_en + "",
                                      request_type = lstAD.TM_Employment_Request.TM_Request_Type.request_type_name_en + "",
                                      rank = lstAD.TM_Pool_Rank.Pool_rank_short_name_en + "",
                                      approve_date = lstAD.target_period.HasValue ? lstAD.target_period.Value.DateTimebyCulture() : "",
                                      // approve_date = lstAD.TM_Pool_Rank.TM_Rank.ceo_approve + "" != "Y" ? (lstAD.HeadApprove_date.HasValue ? lstAD.HeadApprove_date.Value.DateTimebyCulture() : "") : (lstAD.CeoApprove_date.HasValue ? lstAD.CeoApprove_date.Value.DateTimebyCulture() : ""),
                                      pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                      pr_status_id = lstAD.TM_PR_Status.Id + "",
                                      pr_status_seq = lstAD.TM_PR_Status.seq + "",
                                      request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                      hc = lstAD.no_of_headcount + "",
                                      request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimebyCulture() : "",
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      IdEncrypt = ("RC" + lstAD.Id + "M"),
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + ("RC" + lstAD.Id + "M") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""  tooltip=""View"" flow=""right""><i class=""glyphicon glyphicon-search""></i></button>",
                                      no_select = lstAD.TM_PR_Candidate_Mapping != null ? lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y").OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Interview).Count() + "" : "0",
                                      no_accept = lstAD.TM_PR_Candidate_Mapping != null ? lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y").Select(s => s.TM_Candidate_Status.Id).Contains((int)StatusCandidate.OnBoard)
                                      && !w.TM_Candidate_Status_Cycle.Any(a => HCMClass.lstNotSelect().Contains(a.TM_Candidate_Status.Id) && a.active_status == "Y")).Count() + "" : "0",
                                      sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",

                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        #endregion
    }
}