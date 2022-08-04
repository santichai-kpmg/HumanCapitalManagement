using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Data;
using HumanCapitalManagement.Models.LogModels;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.LogService;
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
    public class PRAdminController : BaseController
    {
        private DivisionService _DivisionService;
        private TM_PositionService _TM_PositionService;
        private TM_Step_ApproveService _TM_Step_ApproveService;
        private TM_PR_StatusService _TM_PR_StatusService;
        private PersonnelRequestService _PersonnelRequestService;
        private E_Mail_HistoryService _E_Mail_HistoryService;
        private TM_Pool_RankService _TM_Pool_RankService;
        private TM_SubGroupService _TM_SubGroupService;
        private LogPersonnelRequestService _LogPersonnelRequestService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        public PRAdminController(DivisionService DivisionService
            , TM_PositionService TM_PositionService
            , TM_Step_ApproveService TM_Step_ApproveService
            , TM_PR_StatusService TM_PR_StatusService
            , PersonnelRequestService PersonnelRequestService
            , E_Mail_HistoryService E_Mail_HistoryService
            , TM_Pool_RankService TM_Pool_RankService
            , TM_SubGroupService TM_SubGroupService
            , LogPersonnelRequestService LogPersonnelRequestService
            )
        {
            _DivisionService = DivisionService;
            _TM_PositionService = TM_PositionService;
            _TM_Step_ApproveService = TM_Step_ApproveService;
            _TM_PR_StatusService = TM_PR_StatusService;
            _PersonnelRequestService = PersonnelRequestService;
            _E_Mail_HistoryService = E_Mail_HistoryService;
            _TM_Pool_RankService = TM_Pool_RankService;
            _TM_SubGroupService = TM_SubGroupService;
            _LogPersonnelRequestService = LogPersonnelRequestService;
        }
        // GET: PRAdmin
        public ActionResult PRAdminList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            bool isAdmin = CGlobal.UserIsAdmin();
            vPRAdmin result = new vPRAdmin();
            result.active_status = "Y";
            result.lstSubGroup = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPosition = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.sub_group_id = "";
            result.position_id = "";
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPRAdmin SearchItem = (CSearchPRAdmin)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPRAdmin)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                int nStatus = SystemFunction.GetIntNullToZero(SearchItem.pr_status + "");
                int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
                int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
                var lstData = _PersonnelRequestService.GetPRListForPRAdmin(
                 SearchItem.group_code, aDivisionPermission,
                 "Y", nStatus, SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, isAdmin);
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
                result.name = SearchItem.name;
                string BackUrl = Uri.EscapeDataString(qryStr);
                if (lstData.Any())
                {
               
                    string[] aRequestUser = lstData.Select(s => s.request_user).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    int[] aNID = lstData.Select(s => s.Id).ToArray();
                    var _getLog = _LogPersonnelRequestService.GetListData(aNID);
                    result.lstData = (from lstAD in lstData
                                      from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                      select new vPRAdmin_obj
                                      {
                                          any_changes = _getLog.Where(w => w.PersonnelRequest_Id == lstAD.Id).Count(),
                                          refno = lstAD.RefNo + "",
                                          sgroup = lstAD.TM_Divisions.division_name_en + "",
                                          position = lstAD.TM_Position.position_name_en + "",
                                          request_type = lstAD.TM_Employment_Request.TM_Request_Type.request_type_name_en + "",
                                          rank = lstAD.TM_Pool_Rank.Pool_rank_short_name_en + "",
                                          approve_date = lstAD.TM_Pool_Rank.TM_Rank.ceo_approve + "" != "Y" ? (lstAD.HeadApprove_date.HasValue ? lstAD.HeadApprove_date.Value.DateTimebyCulture() : "") : (lstAD.CeoApprove_date.HasValue ? lstAD.CeoApprove_date.Value.DateTimebyCulture() : ""),
                                          pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                          pr_status_id = lstAD.TM_PR_Status.Id + "",
                                          pr_status_seq = lstAD.TM_PR_Status.seq + "",
                                          request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                          hc = lstAD.no_of_headcount + "",
                                          request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          IdEncrypt = ("RC" + lstAD.Id + "M"),
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          no_select = lstAD.TM_PR_Candidate_Mapping != null ? lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y").OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Interview).Count() + "" : "0",
                                          no_accept = lstAD.TM_PR_Candidate_Mapping != null ? lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.Where(w2 => w2.active_status == "Y").Select(s => s.TM_Candidate_Status.Id).Contains((int)StatusCandidate.OnBoard)
                                          && !w.TM_Candidate_Status_Cycle.Any(a => HCMClass.lstNotSelect().Contains(a.TM_Candidate_Status.Id) && a.active_status == "Y")).Count() + "" : "0",
                                          sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                      }).ToList();
                }
            }

            return View(result);
        }
        public ActionResult PRAdminEdit(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            DateTime dNow = DateTime.Now;
            vPersonnelAP_obj_save result = new vPersonnelAP_obj_save();
            result.lstApprove = new List<vPersonnelAp_obj>();
            if (!string.IsNullOrEmpty(qryStr))
            {
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(qryStr + ""));
                if (nId != 0)
                {
                    var _getData = _PersonnelRequestService.FindPRAdmin(nId);
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
                            result.lstApprove = _getApproveStep.Select(s => new vPersonnelAp_obj
                            {
                                nStep = s.step,
                                step_name = (s.step_name_en + "").Replace("(", "<br/>("),

                            }).ToList();
                            if (_getEmp.Any())
                            {
                                var _getUserReq = _getApproveStep.Where(w => w.step == 1).FirstOrDefault();
                                if (_getUserReq != null)
                                {
                                    vPersonnelAp_obj objApp = new vPersonnelAp_obj();
                                }
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



                        //result.rank_id = _getData.TM_Pool_Rank != null ? _getData.TM_Pool_Rank.Pool_rank_name_en + "" : "-";

                        //result.position_id = _getData.TM_Position != null ? _getData.TM_Position.position_name_en + "" : "-";
                        result.employment_type_id = _getData.TM_Employment_Request.TM_Employment_Type != null ? _getData.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                        result.request_type_id = _getData.TM_Employment_Request.TM_Request_Type != null ? _getData.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";
                        //teve fix for  bug  
                        result.sub_group_id = _getData.TM_SubGroup != null ? _getData.TM_SubGroup.sub_group_name_en + "" : "-";
                        //result.lstSubgroup = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        //result.sub_group_id = _getData.TM_SubGroup != null ? _getData.TM_SubGroup.Id + "" : "";
                        //if (_getData.TM_Divisions.TM_SubGroup != null && _getData.TM_Divisions.TM_SubGroup.Where(w => w.active_status == "Y").Any())
                        //{
                        //    result.lstSubgroup.AddRange(_getData.TM_Divisions.TM_SubGroup.Where(w => w.active_status == "Y").OrderBy(o => o.sub_group_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.sub_group_name_en }).ToList());
                        //}
                        result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.rank_id = _getData.TM_Pool_Rank != null ? _getData.TM_Pool_Rank.Id + "" : "";//_getData.TM_Pool_Rank != null ? _getData.TM_Pool_Rank.Pool_rank_name_en + "" : "-";
                        if (_getData.TM_Divisions.TM_Pool.TM_Pool_Rank != null && _getData.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").Any())
                        {
                            result.lstrank.AddRange(_getData.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Rank.piority).Select(s => new vSelect_PR { id = s.Id + "", name = s.Pool_rank_name_en }).ToList());
                        }
                        result.lstPosition = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        result.position_id = _getData.TM_Position != null ? _getData.TM_Position.Id + "" : "-";
                        if (_getData.TM_Divisions.TM_Position != null && _getData.TM_Divisions.TM_Position.Any())
                        {
                            result.lstPosition.AddRange(_getData.TM_Divisions.TM_Position.OrderBy(o => o.position_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.position_name_en }).ToList());
                        }
                        result.active_status = _getData.active_status + "";

                        //result.lstTypeReq = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };

                        //result.employment_type_id = _getData.TM_Employment_Request.TM_Employment_Type != null ? _getData.TM_Employment_Request.TM_Employment_Type.Id + "" : "-"; //_getData.TM_Employment_Request.TM_Employment_Type != null ? _getData.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                        //result.request_type_id = _getData.TM_Employment_Request.TM_Request_Type != null ? _getData.TM_Employment_Request.TM_Request_Type.Id + "" : "-";//_getData.TM_Employment_Request.TM_Request_Type != null ? _getData.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";
                        //if (_getData.TM_Employment_Request.TM_Employment_Type.TM_Employment_Request != null && _getData.TM_Employment_Request.TM_Employment_Type.TM_Employment_Request.Where(w => w.active_status == "Y").Any())
                        //{
                        //    result.lstTypeReq.AddRange(_getData.TM_Employment_Request.TM_Employment_Type.TM_Employment_Request.Where(w => w.active_status == "Y").Select(s => new vSelect_PR { id = s.Id + "", name = s.TM_Request_Type.request_type_name_en }).ToList());
                        //}

                        //end


                        result.target_start = _getData.target_period.HasValue ? _getData.target_period.Value.DateTimebyCulture() : "";
                        result.target_end = _getData.target_period_to.HasValue ? _getData.target_period_to.Value.DateTimebyCulture() : "";
                        result.no_of_head = _getData.no_of_headcount + "";
                        result.job_descriptions = _getData.job_descriptions;
                        result.qualification_experience = _getData.qualification_experience;
                        result.description = _getData.remark;
                        result.request_status = _getData.TM_PR_Status.Id + "";
                        result.status_name = _getData.TM_PR_Status.status_name_en + "";
                        result.pr_status_id = _getData.TM_PR_Status.Id + "";
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
                        if (_getData.TM_PR_Candidate_Mapping != null && _getData.TM_PR_Candidate_Mapping.Any())
                        {
                            string[] User = _getData.TM_PR_Candidate_Mapping.Select(s => s.TM_Recruitment_Team != null ? s.TM_Recruitment_Team.user_no : "").ToArray();
                            var _gettUser = dbHr.AllInfo_WS.Where(w => User.Contains(w.EmpNo)).ToList();
                            result.lstCandidates = (from lstAD in _getData.TM_PR_Candidate_Mapping
                                                    from lstEmp in _gettUser.Where(w => w.EmpNo == (lstAD.TM_Recruitment_Team != null ? lstAD.TM_Recruitment_Team.user_no : "")).DefaultIfEmpty(new AllInfo_WS())
                                                    select new vPRCandidates_lstData
                                                    {
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
                }
            }
            else
            {
                return RedirectToAction("PRAdminList", "PRAdmin");
            }

            return View(result);
        }
        //public ActionResult LoadPRAdminListTest(CSearchRCMTracking SearchItem)
        //{
        //    //Server Side Parameter
        //    int start = Convert.ToInt32(Request["start"]);
        //    int length = Convert.ToInt32(Request["length"]);
        //    string searchValue = Request["search[value]"];
        //    string sortColumnName = Request["columns[" + Request["order[0][column]"] + "][name]"];
        //    string sortDirection = Request["order[0][dir]"];
        //    var lstData = _PersonnelRequestService.GetPRListForPRAdmin(
        // "", null,
        // "Y", 0, "", 0, 0, true);
        //    List<vRCMTracking_obj> lstData_resutl = new List<vRCMTracking_obj>();
        //    int totalrows = 0;
        //    int totalrowsafterfiltering = 0;
        //    string qryStr = JsonConvert.SerializeObject(SearchItem,
        //  Formatting.Indented,
        //  new JsonSerializerSettings
        //  {
        //      NullValueHandling = NullValueHandling.Ignore,
        //      MissingMemberHandling = MissingMemberHandling.Ignore,
        //      DefaultValueHandling = DefaultValueHandling.Ignore,
        //  });
        //    //int[] aNotSelect = Enum.GetValues(typeof(StatusCandidate)).Cast<StatusCandidate>().Select(v => (int)v).ToArray();
        //    List<int> lstNotSelect = new List<int>();
        //    lstNotSelect.Add((int)StatusCandidate.Turndown);
        //    lstNotSelect.Add((int)StatusCandidate.NoShow);
        //    string BackUrl = Uri.EscapeDataString(qryStr);
        //    if (lstData.Any())
        //    {
        //        string[] aRequestUser = lstData.Select(s => s.request_user).ToArray();
        //        string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
        //        var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
        //        var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser);
        //        totalrows = lstData.Count();

        //        lstData_resutl = (from lstAD in lstData
        //                          from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
        //                          from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user+"").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
        //                          select new vRCMTracking_obj
        //                          {
        //                              refno = lstAD.RefNo + "",
        //                              sgroup = lstAD.TM_Divisions.division_name_en + "",
        //                              position = lstAD.TM_Position.position_name_en + "",
        //                              request_type = lstAD.TM_Employment_Request.TM_Request_Type.request_type_name_en + "",
        //                              rank = lstAD.TM_Pool_Rank.Pool_rank_short_name_en + "",
        //                              approve_date = lstAD.TM_Pool_Rank.TM_Rank.ceo_approve + "" != "Y" ? (lstAD.HeadApprove_date.HasValue ? lstAD.HeadApprove_date.Value.DateTimebyCulture() : "") : (lstAD.CeoApprove_date.HasValue ? lstAD.CeoApprove_date.Value.DateTimebyCulture() : ""),
        //                              pr_status = lstAD.TM_PR_Status.status_name_en + "",
        //                              pr_status_id = lstAD.TM_PR_Status.Id + "",
        //                              pr_status_seq = lstAD.TM_PR_Status.seq + "",
        //                              request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
        //                              hc = lstAD.no_of_headcount + "",
        //                              request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimebyCulture() : "",
        //                              update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
        //                              update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
        //                              IdEncrypt = ("RC" + lstAD.Id + "M"),
        //                              Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
        //                              no_select = lstAD.TM_PR_Candidate_Mapping != null ? lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.OrderByDescending(m => m.seq).FirstOrDefault().TM_Candidate_Status.Id == (int)StatusCandidate.Interview).Count() + "" : "0",
        //                              no_accept = lstAD.TM_PR_Candidate_Mapping != null ? lstAD.TM_PR_Candidate_Mapping.Where(w => w.active_status == "Y" && w.TM_Candidate_Status_Cycle.Select(s => s.TM_Candidate_Status.Id).Contains((int)StatusCandidate.Accepted)
        //                              && !w.TM_Candidate_Status_Cycle.Any(a => lstNotSelect.Contains(a.TM_Candidate_Status.Id))).Count() + "" : "0",
        //                              sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",

        //                          }).ToList();
        //        //sorting


        //      //  lstData_resutl = lstData_resutl.OrderBy(sortColumnName + " " + sortDirection).ToList<vRCMTracking_obj>();
        //        //paging

        //        lstData_resutl = lstData_resutl.Skip(start).Take(length).ToList<vRCMTracking_obj>();
        //    }

        //    return Json(new { data = lstData_resutl, draw = Request["draw"], recordsTotal = totalrows, recordsFiltered = totalrowsafterfiltering }, JsonRequestBehavior.AllowGet);
        //}
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadPRAdminList(CSearchPRAdmin SearchItem)
        {
            bool isAdmin = CGlobal.UserIsAdmin();
            vPRAdmin_Return result = new vPRAdmin_Return();
            List<vPRAdmin_obj> lstData_resutl = new List<vPRAdmin_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            int nStatus = SystemFunction.GetIntNullToZero(SearchItem.pr_status + "");

            int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
            int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
            var lstData = _PersonnelRequestService.GetPRListForPRAdmin(
            SearchItem.group_code, aDivisionPermission,
            "Y", nStatus, SearchItem.ref_no, nSubGroup, nPosition, SearchItem.name, isAdmin);
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
                int[] aNID = lstData.Select(s => s.Id).ToArray();
                var _getLog = _LogPersonnelRequestService.GetListData(aNID);
                lstData_resutl = (from lstAD in lstData
                                  from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())
                                  select new vPRAdmin_obj
                                  {
                                      any_changes = _getLog.Where(w => w.PersonnelRequest_Id == lstAD.Id).Count(),
                                      refno = lstAD.RefNo + "",
                                      sgroup = lstAD.TM_Divisions.division_name_en + "",
                                      position = lstAD.TM_Position.position_name_en + "",
                                      request_type = lstAD.TM_Employment_Request.TM_Request_Type.request_type_name_en + "",
                                      rank = lstAD.TM_Pool_Rank.Pool_rank_short_name_en + "",
                                      approve_date = lstAD.TM_Pool_Rank.TM_Rank.ceo_approve + "" != "Y" ? (lstAD.HeadApprove_date.HasValue ? lstAD.HeadApprove_date.Value.DateTimebyCulture() : "") : (lstAD.CeoApprove_date.HasValue ? lstAD.CeoApprove_date.Value.DateTimebyCulture() : ""),
                                      pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                      pr_status_id = lstAD.TM_PR_Status.Id + "",
                                      pr_status_seq = lstAD.TM_PR_Status.seq + "",
                                      request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                      hc = lstAD.no_of_headcount + "",
                                      request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimebyCulture() : "",
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      IdEncrypt = ("RC" + lstAD.Id + "M"),
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
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

        [HttpPost]
        public ActionResult UpdatePRAdmin(vPersonnelAP_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            objPRAdmin_Return result = new objPRAdmin_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _PersonnelRequestService.FindPRAdmin(nId);
                    int nComp = SystemFunction.GetIntNullToZero(ItemData.pr_status_id);
                    var _getStatus = _TM_PR_StatusService.Find(nComp);
                    int nRank = SystemFunction.GetIntNullToZero(ItemData.rank_id);
                    int nPosition = SystemFunction.GetIntNullToZero(ItemData.position_id);
                    int nSub = SystemFunction.GetIntNullToZero(ItemData.sub_group_id);
                    int no_of_headcount = SystemFunction.GetIntNullToZero(ItemData.no_of_head);
                    if (_getData != null)
                    {

                        if (_getData.TM_Employment_Request.TM_Request_Type.replaced_user + "" == "Y" && no_of_headcount > 1)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, For replacements, you can request only ONE headcount per PR.";
                            return Json(new
                            {
                                result
                            });
                        }
                        if (no_of_headcount <= 0)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter No. of headcount or  No. of headcount more than 0";
                            return Json(new
                            {
                                result
                            });
                        }

                        var sLog = setLog(_getData);
                        _getData.TM_PR_Status = _getStatus;
                        _getData.update_user = CGlobal.UserInfo.UserId;
                        _getData.update_date = dNow;
                        _getData.active_status = ItemData.active_status;
                        _getData.job_descriptions = ItemData.job_descriptions;
                        _getData.qualification_experience = ItemData.qualification_experience;
                        _getData.remark = ItemData.description;
                        _getData.no_of_headcount = no_of_headcount;
                        //var _getSubgroup = _TM_SubGroupService.Find(nSub);
                        //if (_getSubgroup != null)
                        //{
                        //    //_getData.TM_SubGroup = _getSubgroup;
                        //    _getData.TM_SubGroup_Id = _getSubgroup != null ? _getSubgroup.Id : (int?)null;
                        //}
                        //else
                        //{
                        //    _getData.TM_SubGroup_Id = null;
                        //    // _getData.TM_SubGroup = null;

                        //}

                        var _getRank = _TM_Pool_RankService.Find(nRank);
                        if (_getRank != null)
                        {
                            _getData.TM_Pool_Rank = _getRank;
                        }
                        var _getPosition = _TM_PositionService.Find(nPosition);
                        if (_getPosition != null)
                        {
                            _getData.TM_Position = _getPosition;
                        }

                        var sComplect = _PersonnelRequestService.AdminUpdate(_getData);
                        if (sComplect > 0)
                        {
                            _LogPersonnelRequestService.CreateNew(sLog);
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

        #endregion
        private LogPersonnelRequest setLog(PersonnelRequest s)
        {
            LogPersonnelRequest objRerutn = new LogPersonnelRequest()
            {
                active_status = "Y",
                PersonnelRequest_Id = s.Id,
                BUApprove_date = s.BUApprove_date,
                BUApprove_remark = s.BUApprove_remark,
                job_descriptions = s.job_descriptions,
                target_period = s.target_period,
                target_period_to = s.target_period_to,
                BUApprove_status = s.BUApprove_status,
                BUApprove_user = s.BUApprove_user,
                cancel_reason = s.cancel_reason,
                cancel_user = s.cancel_user,
                CeoApprove_date = s.CeoApprove_date,
                CeoApprove_remark = s.CeoApprove_remark,
                CeoApprove_status = s.CeoApprove_status,
                CeoApprove_user = s.CeoApprove_user,
                create_date = s.create_date,
                create_user = s.create_user,
                HeadApprove_date = s.HeadApprove_date,
                HeadApprove_remark = s.HeadApprove_remark,
                HeadApprove_status = s.HeadApprove_status,
                HeadApprove_user = s.HeadApprove_user,
                hr_remark = s.hr_remark,
                need_ceo_approve = s.need_ceo_approve,
                no_of_eva = s.no_of_eva,
                no_of_headcount = s.no_of_headcount,
                qualification_experience = s.qualification_experience,
                RefNo = s.RefNo,
                reject_reason = s.reject_reason,
                reject_user = s.reject_user,
                remark = s.remark,
                replaced_status = s.replaced_status,
                request_date = s.request_date,
                request_user = s.request_user,
                Req_BUApprove_user = s.Req_BUApprove_user,
                Req_CeoApprove_user = s.Req_CeoApprove_user,
                Req_HeadApprove_user = s.Req_HeadApprove_user,
                TM_Divisions_Id = s.TM_Divisions != null ? s.TM_Divisions.Id : (int?)null,
                TM_Employment_Request_Id = s.TM_Employment_Request.Id,
                user_replaced = s.user_replaced,
                type_of_TIFForm = s.type_of_TIFForm,
                update_date = s.update_date,
                update_user = s.update_user,
                TM_Pool_Rank_Id = s.TM_Pool_Rank != null ? s.TM_Pool_Rank.Id : (int?)null,
                TM_Position_Id = s.TM_Position != null ? s.TM_Position.Id : (int?)null,
                TM_PR_Status_Id = s.TM_PR_Status != null ? s.TM_PR_Status.Id : (int?)null,
                TM_SubGroup_Id = s.TM_SubGroup != null ? s.TM_SubGroup.Id : (int?)null,
            };


            return objRerutn;
        }
    }
}