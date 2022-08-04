using HumanCapitalManagement.App_Start;
using HumanCapitalManagement.Controllers.Common;
using HumanCapitalManagement.Models.Common;
using HumanCapitalManagement.Models.MainModels;
using HumanCapitalManagement.Service.Common;
using HumanCapitalManagement.Service.MainService;
using HumanCapitalManagement.ViewModel;
using HumanCapitalManagement.ViewModel.CommonVM;
using HumanCapitalManagement.ViewModel.MainVM;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HumanCapitalManagement.Controllers.MainController
{
    public class PersonnelRequestController : BaseController
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
        private TM_Pool_RankService _TM_Pool_RankService;
        private TM_FY_PlanService _TM_FY_PlanService;

        private E_Mail_HistoryService _E_Mail_HistoryService;
        private New_HRISEntities dbHr = new New_HRISEntities();
        //private wsEmail2.EMail_NewSoapClient wsMail = new wsEmail2.EMail_NewSoapClient();
        public PersonnelRequestController(DivisionService DivisionService, PoolService PoolService, TM_UnitGroup_Approve_PermitService TM_UnitGroup_Approve_PermitService
            , EmploymentTypeService EmploymentTypeService
            , TM_PositionService TM_PositionService
            , RequestTypeService RequestTypeService
            , TM_Step_ApproveService TM_Step_ApproveService
            , TM_Employment_RequestService TM_Employment_RequestService
            , TM_PR_StatusService TM_PR_StatusService
            , PersonnelRequestService PersonnelRequestService
            , MailContentService MailContentService
            , E_Mail_HistoryService E_Mail_HistoryService
            , TM_Pool_RankService TM_Pool_RankService
            , TM_FY_PlanService TM_FY_PlanService)
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
            _TM_Pool_RankService = TM_Pool_RankService;
            _TM_FY_PlanService = TM_FY_PlanService;
        }
        // GET: PersonnelRequest-
        public ActionResult PersonnelRequestList(string qryStr)
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }

            bool isAdmin = CGlobal.UserIsAdmin();
            vPersonnelRequest result = new vPersonnelRequest();
            result.active_status = "Y";
            if (!string.IsNullOrEmpty(qryStr))
            {
                CSearchPersonnelRequest SearchItem = (CSearchPersonnelRequest)JsonConvert.DeserializeObject(qryStr, (typeof(CSearchPersonnelRequest)));
                string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
                int nStatus = SystemFunction.GetIntNullToZero(SearchItem.pr_status + "");
                int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
                int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
                var lstData = _PersonnelRequestService.GetPersonnelPRListData(
        SearchItem.group_code, aDivisionPermission,
        "Y", nStatus, SearchItem.ref_no, nSubGroup, nPosition, CGlobal.UserInfo.EmployeeNo, isAdmin);
                string BackUrl = Uri.EscapeDataString(qryStr);

                result.group_code = SearchItem.group_code;
                result.pr_status = SearchItem.pr_status;
                result.ref_no = SearchItem.ref_no;


                if (lstData.Any())
                {

                    string[] aRequestUser = lstData.Select(s => s.request_user).ToArray();
                    string[] aUpdateUser = lstData.Select(s => s.update_user).ToArray();
                    var _getRequestUser = dbHr.AllInfo_WS.Where(w => aRequestUser.Contains(w.EmpNo)).ToList();
                    var _getUpdateUser = HCMFunc.GetUpdateUser(aUpdateUser); ;
                    result.lstData = (from lstAD in lstData
                                      from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
                                      from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())

                                      select new vPersonnelRequest_obj
                                      {
                                          refno = lstAD.RefNo + "",
                                          sgroup = lstAD.TM_Divisions.division_name_en + "",
                                          position = lstAD.TM_Position.position_name_en + "",
                                          request_type = lstAD.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "",
                                          rank = lstAD.TM_Pool_Rank.TM_Rank.rank_name_en + "",
                                          pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                          pr_status_id = lstAD.TM_PR_Status.Id + "",
                                          pr_status_seq = lstAD.TM_PR_Status.seq + "",
                                          request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                          hc = lstAD.no_of_headcount + "",
                                          request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimeWithTimebyCulture() : "",
                                          create_user = lstAD.create_user,
                                          create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                          update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                          update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                          Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                          sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                      }).ToList();
                }
            }

            return View(result);
        }
        public ActionResult PersonnelRequestCreate()
        {
            var sCheck = acCheckLoginAndPermission();
            if (sCheck != null)
            {
                return sCheck;
            }
            vPersonnelRequest_obj_save result = new vPersonnelRequest_obj_save();
            DateTime dNow = DateTime.Now;


            if (dNow.Month >= 10)
            {
                dNow = dNow.AddYears(1);
            }
            result.lstSubGroup = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPosition = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstTypeReq = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstApprove = new List<vPersonnelApprover_obj>();
            result.lstApprove_save = new List<vPersonnelApprover_obj>();
            result.FY_plan_title = "HC Plan FY " + dNow.ToString("yy") + " :";
            result.FY_plan = "0 person(s)";
            result.cur_headcount = "0 person(s)";
            //result.TIF_type = "N";
            var _getApproveStep = _TM_Step_ApproveService.GetDataForSelect();
            if (_getApproveStep != null && _getApproveStep.Any())
            {
                result.lstApprove = _getApproveStep.Select(s => new vPersonnelApprover_obj
                {
                    nStep = s.step,
                    step_name = (s.step_name_en + "").Replace("(", "<br/>("),
                    create_ddl = s.step != 1 ? "Y" : "N",
                    app_name = s.step != 1 ? "" : CGlobal.UserInfo.FullName + "",
                    approve_date = "",
                    description = "",
                    app_code = "",
                }).ToList();
            }

            return View(result);
        }
        public ActionResult PersonnelRequestEdit(string qryStr)
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



                        result.rank_id = _getData.TM_Pool_Rank != null ? _getData.TM_Pool_Rank.Pool_rank_name_en + "" : "-";
                        result.sub_group_id = _getData.TM_SubGroup != null ? _getData.TM_SubGroup.sub_group_name_en + "" : "-";
                        result.position_id = _getData.TM_Position != null ? _getData.TM_Position.position_name_en + "" : "-";
                        result.employment_type_id = _getData.TM_Employment_Request.TM_Employment_Type != null ? _getData.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "" : "-";
                        result.request_type_id = _getData.TM_Employment_Request.TM_Request_Type != null ? _getData.TM_Employment_Request.TM_Request_Type.request_type_name_en + "" : "-";
                        //teve fix for  bug
                        //result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
                        //result.rank_id = _getData.TM_Pool_Rank != null ? _getData.TM_Pool_Rank.Id + "" : "";//_getData.TM_Pool_Rank != null ? _getData.TM_Pool_Rank.Pool_rank_name_en + "" : "-";
                        //if (_getData.TM_Divisions.TM_Pool.TM_Pool_Rank != null && _getData.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").Any())
                        //{
                        //    result.lstrank.AddRange(_getData.TM_Divisions.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Rank.piority).Select(s => new vSelect_PR { id = s.Id + "", name = s.Pool_rank_name_en }).ToList());
                        //}
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
                    }
                }
            }
            else
            {
                return RedirectToAction("PersonnelRequestList", "PersonnelRequest");
            }

            return View(result);
        }
        #region Ajax Function
        [HttpPost]
        public ActionResult LoadPersonnelRequestList(CSearchPersonnelRequest SearchItem)
        {
            bool isAdmin = CGlobal.UserIsAdmin();
            vPersonnelRequest_Return result = new vPersonnelRequest_Return();
            List<vPersonnelRequest_obj> lstData_resutl = new List<vPersonnelRequest_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            int nStatus = SystemFunction.GetIntNullToZero(SearchItem.pr_status + "");
            int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
            int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
            var lstData = _PersonnelRequestService.GetPersonnelPRListData(
            SearchItem.group_code, aDivisionPermission,
            "Y", nStatus, SearchItem.ref_no, nSubGroup, nPosition, CGlobal.UserInfo.EmployeeNo, isAdmin);
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
                lstData_resutl = (from lstAD in lstData
                                  from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())

                                  select new vPersonnelRequest_obj
                                  {
                                      refno = lstAD.RefNo + "",
                                      sgroup = lstAD.TM_Divisions.division_name_en + "",
                                      position = lstAD.TM_Position.position_name_en + "",
                                      request_type = lstAD.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "",
                                      rank = lstAD.TM_Pool_Rank.TM_Rank.rank_name_en + "",
                                      pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                      pr_status_id = lstAD.TM_PR_Status.Id + "",
                                      pr_status_seq = lstAD.TM_PR_Status.seq + "",
                                      request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                      hc = lstAD.no_of_headcount + "",
                                      request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimeWithTimebyCulture() : "",
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult LoadPersonnelRequestListManage(CSearchPersonnelRequest SearchItem)
        {
            bool isAdmin = true;
            vPersonnelRequest_Return result = new vPersonnelRequest_Return();
            List<vPersonnelRequest_obj> lstData_resutl = new List<vPersonnelRequest_obj>();
            string[] aDivisionPermission = CGlobal.GetDivision().Select(s => s.sID + "").ToArray();
            int nStatus = SystemFunction.GetIntNullToZero(SearchItem.pr_status + "");
            int nSubGroup = SystemFunction.GetIntNullToZero(SearchItem.sub_group_id + "");
            int nPosition = SystemFunction.GetIntNullToZero(SearchItem.position_id + "");
            var lstData = _PersonnelRequestService.GetPersonnelPRListData(
            SearchItem.group_code, aDivisionPermission,
            "Y", nStatus, SearchItem.ref_no, nSubGroup, nPosition, CGlobal.UserInfo.EmployeeNo, isAdmin);
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
                lstData_resutl = (from lstAD in lstData
                                  from lstEmpReq in _getRequestUser.Where(w => w.EmpNo == lstAD.request_user).DefaultIfEmpty(new AllInfo_WS())
                                  from lstEmpUp in _getUpdateUser.Where(w => w.UserID == (lstAD.update_user + "").Trim().ToLower()).DefaultIfEmpty(new AllInfo_WS())

                                  select new vPersonnelRequest_obj
                                  {
                                      refno = lstAD.RefNo + "",
                                      sgroup = lstAD.TM_Divisions.division_name_en + "",
                                      position = lstAD.TM_Position.position_name_en + "",
                                      request_type = lstAD.TM_Employment_Request.TM_Employment_Type.employee_type_name_en + "",
                                      rank = lstAD.TM_Pool_Rank.TM_Rank.rank_name_en + "",
                                      pr_status = lstAD.TM_PR_Status.status_name_en + "",
                                      pr_status_id = lstAD.TM_PR_Status.Id + "",
                                      pr_status_seq = lstAD.TM_PR_Status.seq + "",
                                      request_by = lstEmpReq.EmpFullName + "",//lstAD.request_user,
                                      hc = lstAD.no_of_headcount + "",
                                      request_date = lstAD.request_date.HasValue ? lstAD.request_date.Value.DateTimeWithTimebyCulture() : "",
                                      create_user = lstAD.create_user,
                                      create_date = lstAD.create_date.HasValue ? lstAD.create_date.Value.DateTimebyCulture() : "",
                                      update_user = lstEmpUp.EmpFullName + "",//lstAD.update_user,
                                      update_date = lstAD.update_date.HasValue ? lstAD.update_date.Value.DateTimebyCulture() : "",
                                      Edit = @"<button id=""btnEdit""  type=""button"" onclick=""Edit('" + HCMFunc.Encrypt(lstAD.Id + "") + "&bUrl=" + BackUrl + @"')"" class=""btn btn-xs btn-primary""><i class=""glyphicon glyphicon-edit""></i></button>",
                                      sub_group = lstAD.TM_SubGroup != null ? lstAD.TM_SubGroup.sub_group_short_name_en + "" : "-",
                                  }).ToList();
                result.lstData = lstData_resutl.ToList();
            }
            result.Status = SystemFunction.process_Success;

            return Json(new { result });
        }
        [HttpPost]
        public ActionResult CreatePersonnelRequest(vPersonnelRequest_obj_save ItemData)
        {
            objPersonnelRequest_Return result = new objPersonnelRequest_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                if (!string.IsNullOrEmpty(ItemData.group_id))
                {
                    int nID_employment_request = SystemFunction.GetIntNullToZero(ItemData.request_type_id);
                    var _getGrpup = _DivisionService.FindByCode(ItemData.group_id);
                    var _getEmploment_request = _TM_Employment_RequestService.Find(nID_employment_request);
                    if (_getGrpup != null && _getEmploment_request != null)
                    {
                        DateTime? dtarget_period = null, dtarget_period_to = null;
                        int no_of_headcount = SystemFunction.GetIntNullToZero(ItemData.no_of_head);
                        int nPosition = SystemFunction.GetIntNullToZero(ItemData.position_id);
                        int nRank = SystemFunction.GetIntNullToZero(ItemData.rank_id);
                        var _getPosition = _getGrpup.TM_Position.Where(w => w.Id == nPosition && w.active_status == "Y").FirstOrDefault();
                        var _getRank = _getGrpup.TM_Pool.TM_Pool_Rank.Where(w => w.Id == nRank && w.active_status == "Y").FirstOrDefault();
                        var _getStatus = _TM_PR_StatusService.Find(2);
                        string Req_BUApprove_user = "", Req_HeadApprove_user = "", Req_CeoApprove_user = "";
                        string user_replace = "", replace_status = "N";
                        //validate
                        #region validate request column
                        //target_start

                        if (_getEmploment_request.TM_Employment_Type.target_period_validate + "" == "Y" && string.IsNullOrEmpty(ItemData.target_end))
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter end period";
                            return Json(new
                            {
                                result
                            });
                        }

                        if (!string.IsNullOrEmpty(ItemData.target_end))
                        {
                            dtarget_period_to = SystemFunction.ConvertStringToDateTime(ItemData.target_end, "");
                        }
                        if (!string.IsNullOrEmpty(ItemData.target_start))
                        {
                            dtarget_period = SystemFunction.ConvertStringToDateTime(ItemData.target_start, "");
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter target period";
                            return Json(new
                            {
                                result
                            });
                        }
                        //no_of_headcount
                        if (no_of_headcount <= 0)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter No. of headcount or  No. of headcount more than 0";
                            return Json(new
                            {
                                result
                            });
                        }
                        if (_getPosition == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter Position or Position not found please call Admin.";
                            return Json(new
                            {
                                result
                            });
                        }
                        if (_getRank == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please enter Rank or Rank not found please call Admin.";
                            return Json(new
                            {
                                result
                            });
                        }
                        var _getRequest_type = _getEmploment_request.TM_Request_Type;
                        if (_getRequest_type != null)
                        {
                            if (_getRequest_type.replaced_user + "" == "Y")
                            {
                                if (!string.IsNullOrEmpty(ItemData.user_no))
                                {
                                    //user_replace = "", replace_status = "";
                                    string sCheckDup = ItemData.user_no;
                                    if (_PersonnelRequestService.CheckDuplicateReplace(ref sCheckDup))
                                    {
                                        result.Status = SystemFunction.process_Failed;
                                        result.Msg = "Error, This resignee's name has been used for another PR already. The PR number(Ref no.) is " + sCheckDup + ". Please input a different name or contact HCM admin.";
                                        return Json(new
                                        {
                                            result
                                        });
                                    }

                                    user_replace = ItemData.user_no + "";
                                    replace_status = "Y";


                                }
                                else
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, please enter replaced user.";
                                    return Json(new
                                    {
                                        result
                                    });
                                }
                                if (no_of_headcount > 1)
                                {
                                    result.Status = SystemFunction.process_Failed;
                                    result.Msg = "Error, For replacements, you can request only ONE headcount per PR.";
                                    return Json(new
                                    {
                                        result
                                    });
                                }
                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Request Type not found plase call Admin.";
                            return Json(new
                            {
                                result
                            });
                        }
                        //get approver
                        var Bu = ItemData.lstApprove_save.Where(w => w.nStep == 2).Select(s => s.app_code).FirstOrDefault();
                        var Hop = ItemData.lstApprove_save.Where(w => w.nStep == 3).Select(s => s.app_code).FirstOrDefault();
                        var Ceo = ItemData.lstApprove_save.Where(w => w.nStep == 4).Select(s => s.app_code).FirstOrDefault();
                        var _getBu = dbHr.Employee.Where(w => w.Employeeno == Bu && (w.Status == 3 || w.Status == 1)).FirstOrDefault();
                        var _getHop = dbHr.Employee.Where(w => w.Employeeno == Hop && (w.Status == 3 || w.Status == 1)).FirstOrDefault();
                        var _getCeo = dbHr.Employee.Where(w => w.Employeeno == Ceo && (w.Status == 3 || w.Status == 1)).FirstOrDefault();
                        //check bu is active Req_BUApprove_user = "", Req_HeadApprove_user = "", Req_CeoApprove_user = "";
                        if (_getBu != null && _getGrpup.TM_UnitGroup_Approve_Permit != null && _getGrpup.TM_UnitGroup_Approve_Permit.Any(w => w.active_status == "Y" && w.user_no.Contains(Bu)))
                        {
                            Req_BUApprove_user = Bu;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Group Head not found plase call Admin.";
                            return Json(new
                            {
                                result
                            });
                        }

                        //check Hop is active
                        if (_getHop != null && _getGrpup.TM_Pool.TM_Pool_Approve_Permit != null && _getGrpup.TM_Pool.TM_Pool_Approve_Permit.Any(w => w.active_status == "Y" && w.user_no.Contains(Hop)))
                        {
                            Req_HeadApprove_user = Hop;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Functional HOP not found plase call Admin.";
                            return Json(new
                            {
                                result
                            });
                        }
                        //check ceo is active
                        if (_getCeo != null && _getGrpup.TM_Pool.TM_Company.TM_Company_Approve_Permit != null && _getGrpup.TM_Pool.TM_Company.TM_Company_Approve_Permit.Any(w => w.active_status == "Y" && w.user_no.Contains(Ceo)))
                        {
                            Req_CeoApprove_user = Ceo;
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, CEO not found plase call Admin.";
                            return Json(new
                            {
                                result
                            });
                        }

                        #endregion

                        int nSGroup = 0;
                        if (!string.IsNullOrEmpty(ItemData.sub_group_id))
                        {
                            nSGroup = SystemFunction.GetIntNullToZero(ItemData.sub_group_id);
                            //   objSave.TM_SubGroup = _getGrpup.TM_SubGroup.Any(w => w.Id == nSGroup).FirstOrDefault();
                        }
                        PersonnelRequest objSave = new PersonnelRequest()
                        {
                            // Id = 0,
                            RefNo = generateRefNo(_getGrpup.TM_Pool.Pool_code + ""),//ItemData.code,//
                            target_period = dtarget_period,
                            target_period_to = dtarget_period_to,
                            job_descriptions = ItemData.job_descriptions,
                            qualification_experience = ItemData.qualification_experience,
                            remark = ItemData.description,
                            request_date = dNow,
                            request_user = CGlobal.UserInfo.EmployeeNo,
                            update_user = CGlobal.UserInfo.UserId,
                            update_date = dNow,
                            create_user = CGlobal.UserInfo.UserId,
                            create_date = dNow,
                            active_status = "Y",
                            TM_Divisions = _getGrpup,
                            TM_SubGroup_Id = _getGrpup.TM_SubGroup.Any(w => w.Id == nSGroup) ? _getGrpup.TM_SubGroup.FirstOrDefault(w => w.Id == nSGroup).Id : (int?)null,
                            TM_Pool_Rank = _getRank,
                            TM_Position = _getPosition,
                            TM_Employment_Request = _getEmploment_request,
                            user_replaced = user_replace,
                            replaced_status = replace_status,
                            Req_BUApprove_user = Req_BUApprove_user,
                            Req_HeadApprove_user = Req_HeadApprove_user,
                            Req_CeoApprove_user = Req_CeoApprove_user,
                            need_ceo_approve = _getRank.TM_Rank != null ? _getRank.TM_Rank.ceo_approve + "" : "N",
                            no_of_headcount = no_of_headcount,
                            TM_PR_Status = _getStatus,
                            type_of_TIFForm = "N",
                        };



                        var sComplect = _PersonnelRequestService.CreateNew(ref objSave);
                        if (sComplect > 0)
                        {
                            result.Status = SystemFunction.process_Success;
                            string sError = "";
                            string mail_to_log = "";
                            var Mail1 = _MailContentService.Find(1);
                            var bSuss = SendRequestMail(objSave, Mail1, ref sError, ref mail_to_log);
                            if (!bSuss)
                            {

                                result.Msg = "Error, " + sError;
                            }
                            AddMailLog(objSave, Mail1, sError, mail_to_log, bSuss);
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, please try again.";
                        }

                    }
                    else
                    {
                        if (_getGrpup == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, select group";
                        }
                        else if (_getEmploment_request == null)
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Employment Type";
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, System error! plese call Admin";
                        }
                    }
                }
                else
                {
                    result.Status = SystemFunction.process_Failed;
                    result.Msg = "Error, select group";

                }
            }
            return Json(new
            {
                result
            });
        }
        private string generateRefNo(string comp)
        {
            DateTime? dFyYear = null;
            var dNow = DateTime.Now;
            if (dNow.Month >= 10)
            {
                dFyYear = dNow.AddYears(1);
            }
            else
            {
                dFyYear = dNow;
            }

            bool FYisThisYear = false;
            if (DateTime.Now.Month < 10)
            {
                FYisThisYear = true;
            }
            List<PersonnelRequest> lstPR = new List<PersonnelRequest>();
            if (FYisThisYear)
            {
                var _getPr = _PersonnelRequestService.GetAllPR();
                if (_getPr.Any())
                {
                    lstPR = _getPr.Where(x => x.TM_Divisions.TM_Pool.Pool_code == comp && ((x.request_date.Value.Year == DateTime.Now.Year - 1 && x.request_date.Value.Month >= 10) || (x.request_date.Value.Year == DateTime.Now.Year && x.request_date.Value.Month < 10))).ToList();
                }

            }
            else
            {
                var _getPr = _PersonnelRequestService.GetAllPR();
                if (_getPr.Any())
                {
                    lstPR = _getPr.Where(x => x.TM_Divisions.TM_Pool.Pool_code == comp && (x.request_date.Value.Year == DateTime.Now.Year && x.request_date.Value.Month >= 10)).ToList();
                }

            }

            int yrCompNo = lstPR.Count + 1;
            string year = dFyYear.Value.ToString("yy", new CultureInfo("en-US"));
            switch (comp)
            {
                //case "Audit": return year + "A" + yrCompNo.ToString().PadLeft(4, '0');
                //case "Tax": return year + "T" + yrCompNo.ToString().PadLeft(4, '0');
                //case "Advisory": return year + "B" + yrCompNo.ToString().PadLeft(4, '0');
                //case "Shared Services": return year + "S" + yrCompNo.ToString().PadLeft(4, '0');
                case "1": return year + "A" + yrCompNo.ToString().PadLeft(4, '0');
                case "2": return year + "T" + yrCompNo.ToString().PadLeft(4, '0');
                case "3": return year + "B" + yrCompNo.ToString().PadLeft(4, '0');
                case "4": return year + "S" + yrCompNo.ToString().PadLeft(4, '0');
                case "5": return year + "L" + yrCompNo.ToString().PadLeft(4, '0');
                default: return "";
            }
        }
        [HttpPost]
        public ActionResult CancelPersonnelRequest(vPersonnelAP_obj_save ItemData)
        {
            string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
            vSubGroup_Return result = new vSubGroup_Return();
            if (ItemData != null)
            {
                DateTime dNow = DateTime.Now;
                int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
                if (nId != 0)
                {
                    var _getData = _PersonnelRequestService.FindForCancel(nId);
                    if (_getData != null)
                    {
                        var _cancelStatus = _TM_PR_StatusService.Find(6);
                        if (_cancelStatus != null && (_getData.request_user == CGlobal.UserInfo.EmployeeNo || CGlobal.UserIsAdmin()))
                        {
                            _getData.update_user = CGlobal.UserInfo.UserId;
                            _getData.update_date = dNow;
                            _getData.TM_PR_Status = _cancelStatus;
                            _getData.cancel_reason = (ItemData.app_remark + "").Trim();
                            _getData.cancel_user = CGlobal.UserInfo.EmployeeNo;
                            var sComplect = _PersonnelRequestService.UpdateApprove(_getData);
                            if (sComplect > 0)
                            {
                                result.Status = SystemFunction.process_Success;
                                //send mail
                                string sError = "";
                                string mail_to_log = "";
                                if (_getData.BUApprove_status == "Y" || _getData.CeoApprove_status == "Y" || _getData.HeadApprove_status == "Y")
                                {
                                    //var Mail3 = _MailContentService.Find(3);
                                    //var bSuss = SendCancelMail(_getData, Mail3, ref sError, ref mail_to_log);
                                    //if (!bSuss)
                                    //{
                                    //    result.Msg = "Error, " + sError;
                                    //}

                                    //AddMailLog(_getData, Mail3, sError, mail_to_log, bSuss);
                                }

                            }
                        }
                        else
                        {
                            result.Status = SystemFunction.process_Failed;
                            result.Msg = "Error, Status not found.";
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

        //[HttpPost]
        //public ActionResult FixbugPR(vPersonnelAP_obj_save ItemData)
        //{
        //    string actionName = this.ControllerContext.RouteData.Values["action"].ToString();
        //    vSubGroup_Return result = new vSubGroup_Return();
        //    if (ItemData != null)
        //    {
        //        DateTime dNow = DateTime.Now;
        //        int nId = SystemFunction.GetIntNullToZero(HCMFunc.Decrypt(ItemData.IdEncrypt + ""));
        //        if (nId != 0)
        //        {
        //            var _getData = _PersonnelRequestService.FindForCancel(nId);
        //            if (_getData != null)
        //            {
        //                DateTime? dtarget_period = null, dtarget_period_to = null;
        //                int nID_employment_request = SystemFunction.GetIntNullToZero(ItemData.request_type_id);
        //                int nRank = SystemFunction.GetIntNullToZero(ItemData.rank_id);
        //                var _getEmploment_request = _TM_Employment_RequestService.Find(nID_employment_request);
        //                string user_replace = "", replace_status = "N";

        //                if (!string.IsNullOrEmpty(ItemData.target_end))
        //                {
        //                    dtarget_period_to = SystemFunction.ConvertStringToDateTime(ItemData.target_end, "");
        //                }
        //                if (!string.IsNullOrEmpty(ItemData.target_start))
        //                {
        //                    dtarget_period = SystemFunction.ConvertStringToDateTime(ItemData.target_start, "");
        //                }

        //                if (_getEmploment_request.TM_Employment_Type.target_period_validate + "" == "Y" && string.IsNullOrEmpty(ItemData.target_end))
        //                {
        //                    result.Status = SystemFunction.process_Failed;
        //                    result.Msg = "Error, please enter end period";
        //                    return Json(new
        //                    {
        //                        result
        //                    });
        //                }
        //                var _getRequest_type = _getEmploment_request.TM_Request_Type;
        //                if (_getRequest_type != null)
        //                {
        //                    if (_getRequest_type.replaced_user + "" == "Y")
        //                    {
        //                        if (!string.IsNullOrEmpty(ItemData.user_no))
        //                        {
        //                            //user_replace = "", replace_status = "";
        //                            user_replace = ItemData.user_no + "";
        //                            replace_status = "Y";
        //                        }
        //                        else
        //                        {
        //                            result.Status = SystemFunction.process_Failed;
        //                            result.Msg = "Error, please enter replaced user.";
        //                            return Json(new
        //                            {
        //                                result
        //                            });
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    result.Status = SystemFunction.process_Failed;
        //                    result.Msg = "Error, Request Type not found plase call Admin.";
        //                    return Json(new
        //                    {
        //                        result
        //                    });
        //                }

        //                var _getRank = _TM_Pool_RankService.Find(nRank);

        //                if (_getRank != null)
        //                {
        //                    _getData.TM_Pool_Rank = _getRank;
        //                }
        //                _getData.user_replaced = user_replace;
        //                _getData.replaced_status = replace_status;
        //                _getData.TM_Employment_Request = _getEmploment_request;
        //                _getData.target_period = dtarget_period;
        //                _getData.target_period_to = dtarget_period_to;
        //                _getData.RefNo = ItemData.code;
        //                //_getData.update_user = CGlobal.UserInfo.UserId;
        //                //_getData.update_date = dNow;
        //                //_getData.TM_PR_Status = _cancelStatus;
        //                //_getData.cancel_reason = (ItemData.app_remark + "").Trim();
        //                //_getData.cancel_user = CGlobal.UserInfo.EmployeeNo;
        //                var sComplect = _PersonnelRequestService.UpdateApprove(_getData);
        //                if (sComplect > 0)
        //                {
        //                    result.Status = SystemFunction.process_Success;
        //                    //send mail
        //                    string sError = "";
        //                    string mail_to_log = "";
        //                    var Mail3 = _MailContentService.Find(3);
        //                    var bSuss = SendCancelMail(_getData, Mail3, ref sError, ref mail_to_log);
        //                    if (!bSuss)
        //                    {
        //                        result.Msg = "Error, " + sError;
        //                    }
        //                    AddMailLog(_getData, Mail3, sError, mail_to_log, bSuss);
        //                }

        //            }
        //            else
        //            {
        //                result.Status = SystemFunction.process_Failed;
        //                result.Msg = "Error, Request Type Not Found.";
        //            }
        //        }

        //    }

        //    return Json(new
        //    {
        //        result
        //    });
        //}

        [HttpPost]
        public JsonResult GetGroupDetail(string SearchItem)
        {
            vGroup_onchange result = new vGroup_onchange();
            result.lstSubGroup = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPosition = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstApprove = new List<vPersonnelApprover_obj>();
            var _getApproveStep = _TM_Step_ApproveService.GetDataForSelect();
            if (_getApproveStep != null && _getApproveStep.Any())
            {
                result.lstApprove = _getApproveStep.Select(s => new vPersonnelApprover_obj
                {
                    nStep = s.step,
                    step_name = (s.step_name_en + "").Replace("(", "<br/>("),
                    create_ddl = s.step != 1 ? "Y" : "N",
                    app_name = s.step != 1 ? "" : CGlobal.UserInfo.FullName + "",
                    approve_date = "",
                    description = "",
                    app_code = "",
                    lstselect = new List<ViewModel.CommonVM.lstDataSelect>(),
                    //default_value = s.step == 2 ? "" : (s.step == 3 ? "" : (s.step == 4 ? "" : "")),
                }).ToList();
            }

            result.sub_group_id = "";
            result.rank_id = "";
            result.position_id = "";
            result.FY_plan = "0 person(s)";
            result.cur_headcount = "0 person(s)";
            result.remark_group_approve = "";
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            New_HRISEntities dbHr = new New_HRISEntities();
            var lst = _DivisionService.FindByCode(SearchItem);

            if (lst != null)
            {
                if (lst.TM_SubGroup != null && lst.TM_SubGroup.Where(w => w.active_status == "Y").Any())
                {
                    result.lstSubGroup.AddRange(lst.TM_SubGroup.Where(w => w.active_status == "Y").OrderBy(o => o.sub_group_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.sub_group_name_en }).ToList());
                }
                if (lst.TM_Pool.TM_Pool_Rank != null && lst.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").Any())
                {
                    result.lstrank.AddRange(lst.TM_Pool.TM_Pool_Rank.Where(w => w.active_status == "Y").OrderBy(o => o.TM_Rank.piority).Select(s => new vSelect_PR { id = s.Id + "", name = s.Pool_rank_name_en }).ToList());
                }
                if (lst.TM_Position != null && lst.TM_Position.Where(w => w.active_status == "Y").Any())
                {
                    result.lstPosition.AddRange(lst.TM_Position.Where(w => w.active_status == "Y").OrderBy(o => o.position_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.position_name_en }).ToList());
                }
                result.remark_group_approve = lst.approve_description + "";
                //set Approver
                if (lst.TM_UnitGroup_Approve_Permit != null && lst.TM_UnitGroup_Approve_Permit.Where(w => w.active_status == "Y").Any())
                {
                    var _GetApprover = lst.TM_UnitGroup_Approve_Permit.Where(w => w.active_status == "Y").OrderBy(o => o.user_id).ToList();
                    string[] aNo = _GetApprover.Select(s => s.user_no).ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aNo.Contains(w.EmpNo)).ToList();
                    if (_getEmp.Any())
                    {
                        result.lstApprove.Where(w => w.nStep == 2).ToList().ForEach(ed =>
                        {
                            ed.create_ddl = "Y";
                            ed.default_value = lst.default_grouphead + "";
                            ed.lstselect = _GetApprover.Select(s =>
                            new ViewModel.CommonVM.lstDataSelect
                            {
                                text = _getEmp.Where(w => w.EmpNo == s.user_no).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                value = s.user_no,
                            }).ToList();

                        });
                    }

                }
                if (lst.TM_Pool.TM_Pool_Approve_Permit != null && lst.TM_Pool.TM_Pool_Approve_Permit.Where(w => w.active_status == "Y").Any())
                {
                    var _GetApprover = lst.TM_Pool.TM_Pool_Approve_Permit.Where(w => w.active_status == "Y").OrderBy(o => o.user_id).ToList();
                    string[] aNo = _GetApprover.Select(s => s.user_no).ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aNo.Contains(w.EmpNo)).ToList();
                    if (_getEmp.Any())
                    {
                        result.lstApprove.Where(w => w.nStep == 3).ToList().ForEach(ed =>
                          {
                              ed.create_ddl = "Y";
                              ed.default_value = lst.default_practice + "";
                              ed.lstselect = _GetApprover.Select(s =>
                              new ViewModel.CommonVM.lstDataSelect
                              {
                                  text = _getEmp.Where(w => w.EmpNo == s.user_no).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                  value = s.user_no,
                              }).ToList();

                          });

                    }
                }

                if (lst.TM_Pool.TM_Company.TM_Company_Approve_Permit != null && lst.TM_Pool.TM_Company.TM_Company_Approve_Permit.Where(w => w.active_status == "Y").Any())
                {
                    var _GetApprover = lst.TM_Pool.TM_Company.TM_Company_Approve_Permit.Where(w => w.active_status == "Y").OrderBy(o => o.user_id).ToList();
                    string[] aNo = _GetApprover.Select(s => s.user_no).ToArray();
                    var _getEmp = dbHr.AllInfo_WS.Where(w => aNo.Contains(w.EmpNo)).ToList();
                    if (_getEmp.Any())
                    {
                        result.lstApprove.Where(w => w.nStep == 4).ToList().ForEach(ed =>
                          {
                              ed.create_ddl = "Y";
                              ed.default_value = lst.default_ceo + "";
                              ed.lstselect = _GetApprover.Select(s =>
                              new ViewModel.CommonVM.lstDataSelect
                              {
                                  text = _getEmp.Where(w => w.EmpNo == s.user_no).Select(s2 => s2.EmpFullName).FirstOrDefault() + "",
                                  value = s.user_no,
                              }).ToList();

                          });
                    }
                }
                int UnitID = SystemFunction.GetIntNullToZero(SearchItem);
                result.cur_headcount = dbHr.AllInfo_WS.Where(w => w.UnitGroupID == UnitID && (w.Status == 1 || w.Status == 3)).Count() + " person(s)";
                DateTime dNow = DateTime.Now;
                if (dNow.Month >= 10)
                {
                    dNow = dNow.AddYears(1);
                }
                var _getPlan = _TM_FY_PlanService.FindbyYear(dNow.Year);
                if (_getPlan != null && _getPlan.TM_FY_Detail != null)
                {
                    var _getFYPlan = _getPlan.TM_FY_Detail.Where(w => w.TM_Divisions.division_code == SearchItem).FirstOrDefault();
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
            }
            #endregion

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetGroupDetailForSearch(string SearchItem)
        {
            vGroup_onchange result = new vGroup_onchange();
            result.lstSubGroup = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstPosition = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstrank = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.lstApprove = new List<vPersonnelApprover_obj>();
            result.sub_group_id = "";
            result.rank_id = "";
            result.position_id = "";

            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            New_HRISEntities dbHr = new New_HRISEntities();
            var lst = _DivisionService.FindByCode(SearchItem);

            if (lst != null)
            {
                if (lst.TM_SubGroup != null && lst.TM_SubGroup.Any())
                {
                    result.lstSubGroup.AddRange(lst.TM_SubGroup.OrderBy(o => o.sub_group_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.sub_group_name_en }).ToList());
                }
                if (lst.TM_Pool.TM_Pool_Rank != null && lst.TM_Pool.TM_Pool_Rank.Any())
                {
                    result.lstrank.AddRange(lst.TM_Pool.TM_Pool_Rank.OrderBy(o => o.TM_Rank.piority).Select(s => new vSelect_PR { id = s.Id + "", name = s.Pool_rank_name_en }).ToList());
                }
                if (lst.TM_Position != null && lst.TM_Position.Any())
                {
                    result.lstPosition.AddRange(lst.TM_Position.OrderBy(o => o.position_name_en).Select(s => new vSelect_PR { id = s.Id + "", name = s.position_name_en }).ToList());
                }

            }
            #endregion

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetEmploymentType(string SearchItem)
        {
            vEmployment_onchange result = new vEmployment_onchange();
            result.lstTypeReq = new List<vSelect_PR>() { new vSelect_PR { id = "", name = " - Select - ", } };
            result.request_type_id = "";
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            New_HRISEntities dbHr = new New_HRISEntities();
            int nID = SystemFunction.GetIntNullToZero(SearchItem);
            var lst = _EmploymentTypeService.Find(nID);

            if (lst != null)
            {
                if (lst.TM_Employment_Request != null && lst.TM_Employment_Request.Where(w => w.active_status == "Y").Any())
                {
                    result.lstTypeReq.AddRange(lst.TM_Employment_Request.Where(w => w.active_status == "Y").Select(s => new vSelect_PR { id = s.Id + "", name = s.TM_Request_Type.request_type_name_en }).ToList());
                }
            }
            #endregion

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPosition(string SearchItem)
        {
            vPosition_onchange result = new vPosition_onchange();
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            New_HRISEntities dbHr = new New_HRISEntities();
            int nID = SystemFunction.GetIntNullToZero(SearchItem);
            var lst = _TM_PositionService.Find(nID);
            if (lst != null)
            {
                result.job_descriptions = lst.job_descriptions + "";
                result.qualification_experience = lst.qualification_experience + "";
            }
            #endregion

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetRequestType(string SearchItem)
        {
            vRequestType_onchange result = new vRequestType_onchange();
            #region แก้ไขข้อมูลในการค้นหา
            SearchItem = (SearchItem + "").Trim().ToLower();
            New_HRISEntities dbHr = new New_HRISEntities();
            int nID = SystemFunction.GetIntNullToZero(SearchItem);
            var lst = _RequestTypeService.Find(nID);
            if (lst != null)
            {
                result.replaced_user = lst.replaced_user + "";
            }
            #endregion

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private void AddMailLog(Models.MainModels.PersonnelRequest PersonnelRequest, Models.Common.MailContent MailContent, string descriptions, string mail_to_log, bool suss)
        {
            try
            {
                DateTime dNow = DateTime.Now;
                E_Mail_History objSave = new E_Mail_History();
                objSave.PersonnelRequest = PersonnelRequest;
                objSave.MailContent = MailContent;
                objSave.create_user = CGlobal.UserInfo.UserId;
                objSave.update_user = CGlobal.UserInfo.UserId;
                objSave.create_date = dNow;
                objSave.update_date = dNow;
                objSave.sent_status = suss ? "Y" : "N";
                objSave.descriptions = descriptions;
                objSave.mail_to = mail_to_log;
                _E_Mail_HistoryService.CreateNew(objSave);
            }
            catch (Exception e)
            {

            }
        }
        #endregion
    }
}